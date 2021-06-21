using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArRaycast : MonoBehaviour
{
    public GameObject planeMarker;
    public GameObject objectToSpawn;

    private ARRaycastManager ARRaycastManagerScript;
    private Vector2 touchPosition;
    private int countOfTouches = 0;


    // Start is called before the first frame update
    void Start()
    {
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        planeMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (countOfTouches == 0)
        {
            ShowMarker();
        }
                
    }

    void ShowMarker()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        ARRaycastManagerScript.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            planeMarker.transform.position = hits[0].pose.position;
            planeMarker.SetActive(true);

            
        }
        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            List<ARRaycastHit> hits2 = new List<ARRaycastHit>();

            ARRaycastManagerScript.Raycast(touchPosition, hits2, TrackableType.Planes);
            Instantiate(objectToSpawn, hits2[0].pose.position, objectToSpawn.transform.rotation);
            countOfTouches++;
            planeMarker.SetActive(false);
        }
    }
}
