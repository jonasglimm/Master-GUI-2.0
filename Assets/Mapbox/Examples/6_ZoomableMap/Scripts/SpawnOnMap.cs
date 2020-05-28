namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		public Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		private MapboxTaskControl mapboxTaskControl; // added to spawn each object at a time
		private int previousI;

		void Start()
		{
			mapboxTaskControl = GameObject.Find("MapManager").GetComponent<MapboxTaskControl>(); //added
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();

			
			var i = mapboxTaskControl.targetCount; // all below is added - use green code for original functionality
			var locationString = _locationStrings[i];
			_locations[i] = Conversions.StringToLatLon(locationString);
			var instance = Instantiate(_markerPrefab);
			instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			_spawnedObjects.Add(instance);
			

			//Normal rendering of objects - use this for normal functionality
			/*
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}
			*/
		}

		private void Update()
		{
			//all Code is added - only use green code for original functionality
			var i = mapboxTaskControl.targetCount - 1;
			
			if (mapboxTaskControl.targetCount == 0)
            {
				i = 0;
            }
			
			if (previousI != i)
            {
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
				var previousInstance = Instantiate(_markerPrefab); //delete prevo
				previousInstance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				previousInstance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Remove(previousInstance);
				previousI = i;
			}
			
			
			var spawnedObject = _spawnedObjects[0]; // 0 instead of i because the list always only contains one object
			var location = _locations[i];
			spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
			spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			

			//Normal rendering of objects - use this for normal functionality
			/*
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
			*/
		}
	}
}