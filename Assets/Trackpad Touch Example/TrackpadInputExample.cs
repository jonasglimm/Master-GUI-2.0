using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace TrackpadTouch
{

	public class TrackpadInputExample : MonoBehaviour
	{
		public GameObject prefab;

		private Vector3 touchZeroPrevPos = new Vector3(0,0,0);
		private Vector3 touchOnePrevPos = new Vector3(0,0,0);
		private int counter = 0;


		Dictionary<int, GameObject> touchObjects = new Dictionary<int, GameObject>();

		void Update()
		{
			for (var i = 0; i < TrackpadInput.touches.Count; ++i)
			{
				var touch = TrackpadInput.touches[i];
				var deviceSize = TrackpadInput.deviceSizes[i];

				var screenPoint = new Vector3(touch.position.x, touch.position.y, 0);
				var worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
				worldPos.z = 0;
				//Debug.Log(worldPos);


				GameObject debugSphere;

				switch (touch.phase)
				{

					case TouchPhase.Began:
						if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
							Object.Destroy(debugSphere);
						debugSphere = touchObjects[touch.fingerId] = (GameObject)Object.Instantiate(prefab, worldPos, Quaternion.identity);
						//debugSphere = touchObjects[touch.fingerId] = (GameObject)Object.Instantiate(prefab, screenPoint, Quaternion.identity);
						var texts = debugSphere.GetComponentsInChildren<Text>();
						texts[0].text = touch.fingerId.ToString();
						texts[1].text = deviceSize.ToString();
						debugSphere.name = "Touch " + touch.fingerId;
						break;

					case TouchPhase.Moved:
						if (touchObjects.TryGetValue(touch.fingerId, out debugSphere))
							debugSphere.transform.position = worldPos;
							//debugSphere.transform.position = screenPoint;
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
			//OwnTesting();
		}

		void OnDisable()
		{
			foreach (var gameObject in touchObjects.Values)
				Object.Destroy(gameObject);
			touchObjects.Clear();
		}

		public void OwnTesting() //added for own testing
		{
			if (TrackpadInput.touchCount != 0)
			{
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
						float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;

						float touchDeltaMag = (touch0.transform.position - touch1.transform.position).magnitude;
						var zoomDelta = 1f * (touchDeltaMag - prevTouchDeltaMag);

						Debug.Log("ZoomDelta = " + zoomDelta);

					}
					touchZeroPrevPos = touch0.transform.position;
					touchOnePrevPos = touch1.transform.position;
				}


			}


		}

	}
}