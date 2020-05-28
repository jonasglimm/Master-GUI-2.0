using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

using Mapbox.Utils;
using Mapbox.Unity.Map;
using System;
using Mapbox.Unity.Utilities;

public class MapboxTaskControl : MonoBehaviour
{
    private Mapbox.Unity.Map.AbstractMap abstractMap;
    private Mapbox.Examples.SpawnOnMap spawnOnMap;
    private Mapbox.Examples.QuadTreeCameraMovement quadTreeCameraMovement;
    private ValueControlCenter valueControlCenter;
    private AudioSource clickSound;

    private AbstractMap _mapManager;

    private Camera _referenceCamera;
    private bool _shouldDrag;

    public GameObject nameAufgabe;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;
    public GameObject panelCorrect;
    public GameObject endPanel;
    public TextMeshProUGUI timeTextField;
    public GameObject pointer;

    private GameObject valueAdjustmentPanel;
    private GameObject panSpeedValue;
    private GameObject zoomSpeedValue;

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

    public float _panSpeed = 2f;
    public float _zoomSpeed = 0.25f;
    public float zoomBarrier = 16f; //zoomvalue which has to be reached - start value = 16
    public double targetOffset = 0.005f; //offset for each target - start value = 0.005
    private DateTime startTime;

    private void Awake()
    {
        abstractMap = GameObject.Find("Map").GetComponent<Mapbox.Unity.Map.AbstractMap>();
        spawnOnMap = GameObject.Find("Map").GetComponent<Mapbox.Examples.SpawnOnMap>();
        quadTreeCameraMovement = GameObject.Find("Map").GetComponent<Mapbox.Examples.QuadTreeCameraMovement>();
        valueControlCenter = GameObject.Find("MapManager").GetComponent<ValueControlCenter>();
        clickSound = GameObject.Find("MapManager").GetComponent<AudioSource>();

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
        
        if (valueControlCenter.touchpadInput == true) 
        {
            //_zoomSpeed = 0.5f;
            //_panSpeed = 2f;
            quadTreeCameraMovement.enabled = false; //preventing conflict with quadTreeCameraMovement script
            InvokeRepeating("CursorLock", valueControlCenter.cursorResetTime, valueControlCenter.cursorResetTime);
            HideCursor();
        }

        if(valueControlCenter.touchscreenInput == true)
        {
            //_panSpeed = 2f;
            //_zoomSpeed = 0.25f;
        }
        startTime = System.DateTime.Now;
    }

    void Update()
    {
        SetGUI();
        ValueAdjustment();
        GetTargetLocations();
        GetCurrentLocation();
        GetCurrentZoom();
        CheckZoomAndOffset();

        if(valueControlCenter.touchpadInput == true)
        {
            handleTrackpadInput();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndScreen();
        }
    }

    #region ValueAdjustments

    private void ValueAdjustment()
    {
        valueAdjustmentPanel = GameObject.Find("ValueAdjustment");
        valueAdjustmentPanel.SetActive(true);

        panSpeedValue = GameObject.Find("PanSpeedValue");
        zoomSpeedValue = GameObject.Find("ZoomSpeedValue");
        panSpeedValue.GetComponent<Text>().text = _panSpeed.ToString();
        zoomSpeedValue.GetComponent<Text>().text = _zoomSpeed.ToString();
    }

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

    private void SetGUI()
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteMarkierung;
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = targetCount.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = valueControlCenter.numberOfTasks.ToString();
        gesuchteMarkierung = "Markierung im " + targetNames[targetCount - 1] + "!";

    }

    private void GetTargetLocations()
    {
        targetLocations = spawnOnMap._locations;
    }

    private void GetCurrentLocation()
    {
        currentLocationString = abstractMap.Options.locationOptions.latitudeLongitude;
        currentLocation = Conversions.StringToLatLon(currentLocationString);
    }

    private void GetCurrentZoom()
    {
        zoom = abstractMap.Options.locationOptions.zoom;
    }

    private void CheckZoomAndOffset()
    {
        if (zoom > zoomBarrier)
        {
            CheckOffset(targetCount - 1);
        }
    }

    private void CheckOffset(int target)
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
                    targetCount = 0; 
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

    
    private void handleTrackpadInput()
    {
        CursorUnlock();

        UseMeterConversion();

        float scrollDelta = 0.0f;
        scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        ZoomMapUsingTouchOrMouse(scrollDelta);

    }

    public void UseMeterConversion()
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
    }

    void ZoomMapUsingTouchOrMouse(float zoomFactor)
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
            CancelInvoke();
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




