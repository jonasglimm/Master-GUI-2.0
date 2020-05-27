using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapControlAlt : MonoBehaviour
{
    public Camera mapCamera;
    //public GameObject cameraTrigger;
    public GameObject pyramidTrigger;

    public float speed = 5f;

    public Vector3 lastPosition;
   

    void Update()
    {
        // restrict camera movement
        if (pyramidTrigger.GetComponent<Collider>().bounds.Contains(mapCamera.GetComponent<Transform>().position))
        {
            lastPosition = mapCamera.GetComponent<Transform>().position;

            Debug.Log("Es ist drin");
        // using Unity axis
        if (Input.GetAxis("Horizontal") != 0)
            {
                mapCamera.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed *Time.deltaTime, 0, 0));
            }

            if (Input.GetAxis("Vertical") != 0)
            {
                mapCamera.transform.Translate(new Vector3(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0));
            }

            if (Input.GetAxis("Lateral") != 0) //new axis for Zoom generated in "Edit" -> "Project Settings" -> "Input"
            {
                mapCamera.transform.Translate(new Vector3(0, 0, Input.GetAxis("Lateral") * speed / 2 * Time.deltaTime));
            }

            //lastPosition = mapCamera.GetComponent<Transform>().position;

        }
        else
        {
            Debug.Log("Es ist draußen");
            mapCamera.GetComponent<Transform>().position = lastPosition;
        }

        ///
        /// using separate keys
        ///
      /*
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mapCamera.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mapCamera.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mapCamera.transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mapCamera.transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.Y))
        {
            mapCamera.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.X))
        {
            mapCamera.transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        */
    }


}
