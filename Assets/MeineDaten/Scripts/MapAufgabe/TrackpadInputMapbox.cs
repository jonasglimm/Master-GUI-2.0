using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace TrackpadTouch
{

	public class TrackpadInputMapbox : MonoBehaviour
	{
		public GameObject prefab;

		private Vector3 touchZeroPrevPos = new Vector3(0,0,0); //store the position on each touch from the last frame
		private Vector3 touchOnePrevPos = new Vector3(0,0,0);
		private Vector2 panTouchPrevPos = new Vector2(0, 0);
		private Vector2 touchStart = new Vector2();
		private int counter = 0;

		private MapboxTaskControl mapboxTaskControl;

		Dictionary<int, GameObject> touchObjects = new Dictionary<int, GameObject>(); //dictionary based on the imported TrackpadTouch - asset

        private void Awake()
        {
			mapboxTaskControl = GameObject.Find("MapManager").GetComponent<MapboxTaskControl>(); //reference to map control script
        }

        void Update()
		{
			for (var i = 0; i < TrackpadInput.touches.Count; ++i) 
			{
				var touch = TrackpadInput.touches[i]; //creates an array of each touch
				var deviceSize = TrackpadInput.deviceSizes[i]; //stores the size of the touchdevice in pixels

				var screenPoint = new Vector3(touch.position.x, touch.position.y, 0);
				//var worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
				//worldPos.z = 0;
				//Debug.Log(worldPos);


				GameObject debugSphere;

				switch (touch.phase)
				{
					//this is acutally the code which is used in the example scripts of the Trackpad Touch asset
					//This is used to create gameobject for the first two touches so that the position of these gameobjects can be analysed for possible pinch gestures
					//This is kind of a detour but the measurement of the touch input was too inconsistent 
					case TouchPhase.Began:
						if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
							Object.Destroy(debugSphere);
						//debugSphere = touchObjects[touch.fingerId] = (GameObject)Object.Instantiate(prefab, worldPos, Quaternion.identity);
						debugSphere = touchObjects[touch.fingerId] = (GameObject)Object.Instantiate(prefab, screenPoint, Quaternion.identity);
						var texts = debugSphere.GetComponentsInChildren<Text>();
						texts[0].text = touch.fingerId.ToString();
						texts[1].text = deviceSize.ToString();
						debugSphere.name = "Touch " + touch.fingerId;
						break;

					case TouchPhase.Moved:
						if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
							//debugSphere.transform.position = worldPos;
							debugSphere.transform.position = screenPoint;
						break;

					case TouchPhase.Canceled:
					case TouchPhase.Ended:
						if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
							Object.Destroy(debugSphere);
						touchObjects.Remove(touch.fingerId);
						break;

					// case TouchPhase.Stationary:
					// break;

					default:
						break;
				}
			}
			//Debug.Log(TrackpadInput.touchCount);
			PinchToZoom();
		}

		void OnDisable()
		{
			foreach (var gameObject in touchObjects.Values)
				Object.Destroy(gameObject);
			touchObjects.Clear();
		}

		public void PinchToZoom()
		{
			if (TrackpadInput.touchCount != 0)
			{
				// Two gameobjects are being created to analyse the movement of these
				GameObject touch0 = GameObject.Find("Touch 0");
				GameObject touch1 = GameObject.Find("Touch 1");

				if (GameObject.Find("Touch 0") && GameObject.Find("Touch 1"))
				{
					if (counter == 0) //first frame to set the PrevPos
					{
						counter++;
					}
					else
					{
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //calculate the distance between both touch points of the last frame

						float touchDeltaMag = (touch0.transform.position - touch1.transform.position).magnitude; //calculate the distance between both touch points of this frame
						var zoomDelta = 0.01f * (touchDeltaMag - prevTouchDeltaMag); //factor to decrease the magnitude
						//Debug.Log(zoomDelta);
						if (TrackpadInput.touches[0].phase != TouchPhase.Ended || TrackpadInput.touches[0].phase != TouchPhase.Canceled) //fire the zoom value if the touchphase has ended
						{
							if (Mathf.Abs(zoomDelta) < 0.5)
							{
								mapboxTaskControl.ZoomMapUsingTouchOrMouse(zoomDelta);
							}
						}
						//Debug.Log("ZoomDelta = " + zoomDelta);
					}
					touchZeroPrevPos = touch0.transform.position; //reset last frame position
					touchOnePrevPos = touch1.transform.position;
				}
			}
		}

		

		public Vector2 TrackpadPan()
        {
			if (TrackpadInput.touchCount != 0)
			{
				if (GameObject.Find("Touch 0") && !GameObject.Find("Touch 1"))
				{
					if(TrackpadInput.touches[0].phase == TouchPhase.Began)
                    {
						touchStart = TrackpadInput.touches[0].position; //store the initial position
						return Vector2.zero;
					}

					else if(TrackpadInput.touches[0].phase == TouchPhase.Moved)
					{
						Vector2 panTouchDelta = touchStart - TrackpadInput.touches[0].position ; // measure the position difference between last frame and current frame
						touchStart = TrackpadInput.touches[0].position; //reset the touchStart value
						if (panTouchDelta.x > 80 || panTouchDelta.y > 80) //avoid pan jumps caused by incorrectly detected touchInput
						{
							return Vector2.zero;
						}
						else
						{
							return panTouchDelta; //only in this case a pan value is sent 
						}
					}
					else if (TrackpadInput.touches[0].phase == TouchPhase.Ended || TrackpadInput.touches[0].phase == TouchPhase.Canceled)
					{
						Cursor.lockState = CursorLockMode.Locked;
					}
					return Vector2.zero;
				}
				return Vector2.zero;
			}
			return Vector2.zero;
		}
	}
}