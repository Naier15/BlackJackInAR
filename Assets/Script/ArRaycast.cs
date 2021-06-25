using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArRaycast : MonoBehaviour
{
    public GameObject planeMarker;
    public GameObject objectToSpawn;
    public Camera ARCamera;

    private ARRaycastManager ARRaycastManagerScript;
    private Vector2 touchPosition;
    private int countOfTouches;
    private float distance = 1.0f;
    //private float tempPosAndr;
    private GameObject selectedObject;
    private Vector3 objPositionAndr;


    // Start is called before the first frame update
    void Start()
    {
        countOfTouches = 0;
        ARRaycastManagerScript = FindObjectOfType<ARRaycastManager>();
        planeMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (countOfTouches == 0)
            ShowMarker();

        Touch touch = Input.GetTouch(0);

        SelectObject(touch);

        if (selectedObject != null)
		{
            MoveObject(touch, selectedObject);
            UnTouchObject(touch);
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
            Instantiate(objectToSpawn, hits2[0].pose.position, Quaternion.identity);
            countOfTouches++;
            planeMarker.SetActive(false);
        }
    }

    void SelectObject(Touch touch)
	{
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
		{
            Ray ray = ARCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name.Contains("Chip"))
				{
                    selectedObject = hit.collider.gameObject;
                    //tempPosAndr = ARCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance)).y;
                }
            }
        }
    }


    void MoveObject(Touch touch, GameObject selectedObject)
	{
        
        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        {
            selectedObject.GetComponent<Rigidbody>().useGravity = false;

            objPositionAndr = ARCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance));

            //if (objPositionAndr.y > tempPosAndr)
            //    objPositionAndr.x -= (objPositionAndr.y - tempPosAndr) * 3.0f;
            selectedObject.transform.position = objPositionAndr;
        }
        else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            selectedObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void UnTouchObject(Touch touch)
	{
        if (touch.phase == TouchPhase.Ended)
		{
            selectedObject = null;
        }
    }
}
