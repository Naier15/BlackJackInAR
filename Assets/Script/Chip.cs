using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public enum ChipType
	{
        RED, GREEN, BLACK
	};

    private float tempPosComp;
    private float tempPosAndr;
    private GameRule GameRule;
    private int cost;

    public float distance = 15.0f;
    public ChipType type;

    void Start()
    {
        GameRule = GameObject.Find("BlackJackScene").GetComponent<GameRule>();
        if (type == ChipType.RED)
            cost = 5;
        else if (type == ChipType.GREEN)
            cost = 10;
        else
            cost = 20;
    }

    void Update()
    {
        // ¬вод с android
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
		{
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.name.Contains("Chip"))
                {
                    tempPosAndr = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance)).y;
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;

                        Vector3 objPositionAndr = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance));

                        if (objPositionAndr.y > tempPosAndr)
                            objPositionAndr.x -= (objPositionAndr.y - tempPosAndr) * 3.0f;

                        hit.collider.transform.position = objPositionAndr;
                    }
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    }
                }
            }
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("BetPlatform"))
		{
            if (GameRule.statusGame == GameRule.StatusGame.START_GAME || GameRule.statusGame == GameRule.StatusGame.IN_GAME)
			{
                GameRule.chipsInBlueZone.Add(gameObject);
                GameRule.ChangeScore(cost);
			}
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("BetPlatform"))
        {
            if (GameRule.statusGame == GameRule.StatusGame.START_GAME || GameRule.statusGame == GameRule.StatusGame.IN_GAME)
            {
                GameRule.chipsInBlueZone.Remove(gameObject);
                GameRule.ChangeScore(-cost);
            }
        }
    }

    private void OnMouseDown()
    {
        tempPosComp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)).y;
    }

    private void OnMouseDrag()
    {
        GetComponent<Rigidbody>().useGravity = false;

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        
        if (objPosition.y > tempPosComp)
        {
            objPosition.x -= (objPosition.y - tempPosComp) * 3.0f;
        }
        transform.position = objPosition;
    }

    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }
}
