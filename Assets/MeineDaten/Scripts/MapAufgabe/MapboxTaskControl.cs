﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

//additional mapbox functions
using Mapbox.Utils;
using Mapbox.Unity.Map;
using System;
using Mapbox.Unity.Utilities;

using TrackpadTouch; //using an add on from the unity asset store 

public class MapboxTaskControl : MonoBehaviour
{
    private Mapbox.Unity.Map.AbstractMap abstractMap; // script which is linked to the Map gameobject and which was created by mapbox
    private Mapbox.Examples.SpawnOnMap spawnOnMap; // script which is linked to the Map gameobject and which was created by mapbox (controls the different targets which sparn on definded locations)
    private Mapbox.Examples.QuadTreeCameraMovement quadTreeCameraMovement; // script which is linked to the Map gameobject and which was created by mapbox (controls the map movement)
    //other scripts to interact with
    private ValueControlCenter valueControlCenter;
    private AudioSource clickSound;
    private TrackpadInputMapbox trackpadInputMapbox;
    private IDriveController iDriveController;

    private AbstractMap _mapManager; // a mapbox specific object category 

    private Camera _referenceCamera;
    private bool _shouldDrag;

    //GUI elements
    public GameObject nameAufgabe;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;
    public GameObject panelCorrect;
    public GameObject endPanel;
    public TextMeshProUGUI timeTextField;
    public GameObject pointer;
    private Slider zoomSlider;

    //different canvases and GUI elements
    private GameObject valueCanvas;
    private GameObject valueAdjustmentPanel;
    private GameObject panSpeedValue;
    private GameObject zoomSpeedValue;

    // vectors to control the map movement
    private Vector3 _origin;
    private Vector3 _mousePosition;
    private Vector3 _mousePositionPrevious;

    private Vector2d currentLocation;
    private Vector3 lastMouseCoordinate = Vector3.zero;

    [HideInInspector]
    public int targetCount;
    private string[] targetNames = { "Norden", "Osten", "Süden", "Westen" };
    private string gesuchteMarkierung;

    private String currentLocationString;
    private float zoom;

    private Vector2d[] targetLocations;

    //variables to adjust 
    public bool valueCanvasIsVisible;
    public float _panSpeed = 2f;
    public float _zoomSpeed = 0.25f;
    public float zoomBarrier = 16f; //zoomvalue which has to be reached - start value = 16
    public double targetOffset = 0.005f; //offset for each target - start value = 0.005
    private DateTime startTime;
    public GameObject startPanel;
    public GameObject startPanelTouchscreen;

    private void Awake()
    {
        abstractMap = GameObject.Find("Map").GetComponent<Mapbox.Unity.Map.AbstractMap>();
        spawnOnMap = GameObject.Find("Map").GetComponent<Mapbox.Examples.SpawnOnMap>();
        quadTreeCameraMovement = GameObject.Find("Map").GetComponent<Mapbox.Examples.QuadTreeCameraMovement>();
        valueControlCenter = GameObject.Find("MapManager").GetComponent<ValueControlCenter>();
        trackpadInputMapbox = GameObject.Find("MapManager").GetComponent<TrackpadInputMapbox>();
        clickSound = GameObject.Find("MapManager").GetComponent<AudioSource>();
        zoomSlider = GameObject.Find("ZoomIndicator").GetComponentInChildren<Slider>();
        valueCanvas = GameObject.Find("ReloadMapCanvas");
        iDriveController = GameObject.Find("MapManager").GetComponent<IDriveController>();

        //Exchanging values with quadTreeCameraMovement
        _mapManager = abstractMap;
        _referenceCamera = quadTreeCameraMovement._referenceCamera;
        quadTreeCameraMovement._panSpeed = _panSpeed;
        quadTreeCameraMovement._zoomSpeed = _zoomSpeed;
        _shouldDrag = quadTreeCameraMovement._shouldDrag;
    }

    void Start()
    {
        targetCount = 1;
        SetStartPanel();

        if (valueControlCenter.touchpadInput == true) // in order to use the Apple Trackpad a an input device, the standard movement scripts needs to be change/deactivated
        {
            // specific for the touchpad
            _zoomSpeed = 0.75f;
            _panSpeed = 40.0f;

            quadTreeCameraMovement.enabled = false; //preventing conflict with quadTreeCameraMovement script
            //InvokeRepeating("CursorLock", valueControlCenter.cursorResetTime, valueControlCenter.cursorResetTime);
            HideCursor();
        }
        else
        {
            trackpadInputMapbox.enabled = false;
        }

        if (valueControlCenter.touchscreenInput == true)
        {
            // specific values for touchscreen
            _panSpeed = 1.75f;
            _zoomSpeed = 0.25f;
        }

        if (valueCanvasIsVisible == false) // a GUI to adjust pan and zoom values while testing (during game mode)
        {
            valueCanvas.GetComponent<Canvas>().enabled = false;
        }

        if (valueControlCenter.iDriveInput)
        {
            // specific values for the iDrive-Controller
            _panSpeed = 30f;
        }
        else
        {
            iDriveController.enabled = false;
        }
    }

    void Update()
    {
        SetGUI();
        if (valueCanvasIsVisible == true)
        {
            ValueAdjustment();
        }
        GetTargetLocations();
        GetCurrentLocation();
        GetCurrentZoom();
        CheckZoomAndOffset();
        zoomSlider.value = zoom; // zoom indicator is adjusted 

        if(valueControlCenter.touchpadInput == true)
        {
            handleTrackpadInput();
        }
        else if (valueControlCenter.iDriveInput)
        {
            handleIDriveController();
        }

        // if the supervisor manually starts or ends the test
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartTime();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndScreen();
        }
    }

    #region ValueAdjustments (a GUI to adjust pan and zoom values while testing (during game mode))

    private void ValueAdjustment() // a GUI to adjust pan and zoom values while testing (during game mode)
    {
        valueAdjustmentPanel = GameObject.Find("ValueAdjustment");
        valueAdjustmentPanel.SetActive(true);

        panSpeedValue = GameObject.Find("PanSpeedValue");
        zoomSpeedValue = GameObject.Find("ZoomSpeedValue");
        panSpeedValue.GetComponent<Text>().text = _panSpeed.ToString();
        zoomSpeedValue.GetComponent<Text>().text = _zoomSpeed.ToString();
    }

    // connect each of these functions to the correspondig buttons
    public void PanSpeedUp()
    {
        _panSpeed = _panSpeed + 0.25f;
        quadTreeCameraMovement._panSpeed = _panSpeed;
    }

    public void PanSpeedDown()
    {
        _panSpeed = _panSpeed - 0.25f;
        quadTreeCameraMovement._panSpeed = _panSpeed;
    }

    public void ZoomSpeedUp()
    {
        _zoomSpeed = _zoomSpeed + 0.125f;
        quadTreeCameraMovement._zoomSpeed = _zoomSpeed;
    }

    public void ZoomSpeedDown()
    {
        _zoomSpeed = _zoomSpeed - 0.125f;
        quadTreeCameraMovement._zoomSpeed = _zoomSpeed;
    }

    #endregion

    public void SetStartPanel() //start panel, same as in every other task
    {
        if (valueControlCenter.touchscreenInput == true)
        {
            startPanel.SetActive(false);
            startPanelTouchscreen.SetActive(true);
        }
        else
        {
            startPanelTouchscreen.SetActive(false);
            startPanel.SetActive(true);
        }
    }

    public void StartTime() //start timer for ToT and deactivate the start panels
    {
        startTime = System.DateTime.Now;
        clickSound.Play();
        startPanel.SetActive(false);
        startPanelTouchscreen.SetActive(false);
    }

    private void SetGUI() //adjust the GUI each frame
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteMarkierung;
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = targetCount.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = valueControlCenter.numberOfTasks.ToString();
        //gesuchteMarkierung = "Markierung im " + targetNames[targetCount - 1] + "!";
        gesuchteMarkierung = "Bewegen Sie sich zum markierten Ziel!";

    }

    private void GetTargetLocations() // get the locations of each spawn point in an array
    {
        targetLocations = spawnOnMap._locations;
    }

    private void GetCurrentLocation() // get the current map location each frame
    {
        currentLocationString = abstractMap.Options.locationOptions.latitudeLongitude;
        currentLocation = Conversions.StringToLatLon(currentLocationString); // this conversion and type of vector is unique for mapbox
    }

    private void GetCurrentZoom() // get current zoom value
    {
        zoom = abstractMap.Options.locationOptions.zoom;
        //Debug.Log(zoom);
    }

    private void CheckZoomAndOffset() // check if the zoom barrier is broken (user has zoomed in and the target zoom value has been 
    {
        if (zoom > zoomBarrier)
        {
            CheckOffset(targetCount - 1);
        }
    }

    private void CheckOffset(int target) // compare the current location and the target locations in x- and y-coordinates to see, if these are within the barriers
    {
        if ((targetLocations[target].x + targetOffset >= currentLocation.x) && (targetLocations[target].x - targetOffset <= currentLocation.x))
        {
            if ((targetLocations[target].y + targetOffset >= currentLocation.y) && (targetLocations[target].y - targetOffset <= currentLocation.y))
            {
                StartCoroutine(FeedbackCorrect());
                clickSound.Play();
                targetCount++;

                if (target == 3)
                {
                    EndScreen();
                    targetCount = 1; 
                }
            }
        }
    }

    IEnumerator FeedbackCorrect()
    {
        panelCorrect.SetActive(true);
        yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
        panelCorrect.SetActive(false);
    }

    private void handleIDriveController() 
    {
        // using the predefined panning mechanism to pan with an iDrive-Controller
        if (iDriveController.RotaryLeft)
        {
            quadTreeCameraMovement.PanMapUsingKeyBoard(-1f, 0f);
        }
        else if (iDriveController.RotaryRight)
        {
            quadTreeCameraMovement.PanMapUsingKeyBoard(1f, 0f);
        }
        else if (iDriveController.RotaryUp)
        {
            quadTreeCameraMovement.PanMapUsingKeyBoard(0f, 1f);
        }
        else if (iDriveController.RotaryDown)
        {
            quadTreeCameraMovement.PanMapUsingKeyBoard(0f, -1f);
        }

        // zoom delta is directly correlated to the rotation
        if (iDriveController.turnedClockwise)
        {
            float zoomDelta = iDriveController.rotationClockwiseSteps;
            ZoomMapUsingTouchOrMouse(zoomDelta);
        }
        else if (iDriveController.turnedCounterclockwise)
        {
            float zoomDelta = -1 * iDriveController.rotationCounterclockwiseSteps;
            ZoomMapUsingTouchOrMouse(zoomDelta);
        }

    }

    private void handleTrackpadInput()
    {
        CursorUnlock();

        PanWithTrackpad();

        #region Alternatives and debugging
        //PinchTrackpadZoom(); //Problem with touchdetection on trackpad - detects multiple fingers instate of just one

        //UseMeterConversion(); //Exchanged with PanWithTrackpad

        //float scrollDelta = 0.0f;
        //scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        //ZoomMapUsingTouchOrMouse(scrollDelta);

        //zoom is done using the TrackpadInputMapbox script
        #endregion
    }

    public void UseMeterConversion() // this functions is copied and slighly change out of the quadTreeCameraMovement script from mapbox
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            var mousePosScreen = Input.mousePosition;
            //assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
            //http://answers.unity3d.com/answers/599100/view.html
            mousePosScreen.z = _referenceCamera.transform.localPosition.y;
            _mousePosition = _referenceCamera.ScreenToWorldPoint(mousePosScreen);

            if (_shouldDrag == false)
            {
                _shouldDrag = true;
                _origin = _referenceCamera.ScreenToWorldPoint(mousePosScreen);
            }
        }
        else
        {
            _shouldDrag = false;
        }

        if (_shouldDrag == true)
        {
            var changeFromPreviousPosition = _mousePositionPrevious - _mousePosition;
            if (Mathf.Abs(changeFromPreviousPosition.x) > 0.0f || Mathf.Abs(changeFromPreviousPosition.y) > 0.0f)
            {
                _mousePositionPrevious = _mousePosition;
                var offset = _origin - _mousePosition;

                if (Mathf.Abs(offset.x) > 0.0f || Mathf.Abs(offset.z) > 0.0f)
                {
                    if (null != _mapManager)
                    {
                        float factor = _panSpeed * Conversions.GetTileScaleInMeters((float)0, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;
                        var latlongDelta = Conversions.MetersToLatLon(new Vector2d(offset.x * factor, offset.z * factor));
                        var newLatLong = _mapManager.CenterLatitudeLongitude + latlongDelta;

                        _mapManager.UpdateMap(newLatLong, _mapManager.Zoom);
                    }
                }
                _origin = _mousePosition;
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                _mousePositionPrevious = _mousePosition;
                _origin = _mousePosition;
            }
        }
    } //exchanged with PanWithTrackpad

    public void PanWithTrackpad() //function to collect values from TrackpadPan and convert into values to be used by UpdateMap
    {
        Vector2 offset = trackpadInputMapbox.TrackpadPan() * 0.01f;

        float factor = _panSpeed * Conversions.GetTileScaleInMeters((float)0, _mapManager.AbsoluteZoom) / _mapManager.UnityTileSize;
        var latlongDelta = Conversions.MetersToLatLon(new Vector2d(offset.x * factor, offset.y * factor));
        var newLatLong = _mapManager.CenterLatitudeLongitude + latlongDelta;

        _mapManager.UpdateMap(newLatLong, _mapManager.Zoom);
    }

    public void PinchTrackpadZoom() //Problem with multiple touches detected -> done in TrackpadInputMapbox script
    {
        if(TrackpadInput.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = TrackpadInput.GetTouch(0);
            Touch touchOne = TrackpadInput.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            var zoomDelta = 0.01f * (touchDeltaMag - prevTouchDeltaMag);

            if (Math.Abs(zoomDelta) < 0.5) //sometimes the touchdelta is detected incorrectly, then the zoomDelta is very high -> therefore this 
            {
                ZoomMapUsingTouchOrMouse(zoomDelta);
            }

            Debug.Log(zoomDelta);
        }
    } //Problem with multiple touches detected -> done in TrackpadInputMapbox script

    public void ZoomMapUsingTouchOrMouse(float zoomFactor) // this functions is copied and slighly change out of the quadTreeCameraMovement script from mapbox
    {
        var zoom = Mathf.Max(0.0f, Mathf.Min(_mapManager.Zoom + zoomFactor * _zoomSpeed, 21.0f));
        if (Math.Abs(zoom - _mapManager.Zoom) > 0.0f)
        {
            _mapManager.UpdateMap(_mapManager.CenterLatitudeLongitude, zoom);
        }
    }

    public void EndScreen()
    {
        var totalTime = System.DateTime.Now - startTime;
        timeTextField.text = totalTime.Minutes.ToString()+" min : "+totalTime.Seconds.ToString() + " sec";
        pointer.SetActive(false);
        endPanel.SetActive(true);

        if (valueControlCenter.touchpadInput == true)
        {
            //CancelInvoke();
            ShowCursor();
        } 
    }

    
    private void CursorLock() //reset the Cursor by first locking it with this function and unlock it with the next on
    {

            Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void CursorUnlock()
    {
        if (Cursor.lockState == CursorLockMode.Locked) // If Cursor is Locked, unlock it to reset in the middle of the screen
        {
            Cursor.lockState = CursorLockMode.None;
            var mousePosScreen = Input.mousePosition;
            mousePosScreen.z = _referenceCamera.transform.localPosition.y;
            _mousePosition = _referenceCamera.ScreenToWorldPoint(mousePosScreen);
            _mousePositionPrevious = _mousePosition;
            _origin = _mousePosition; //prevent the false recognition of cursor reset as a swipe
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }

    
    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}




