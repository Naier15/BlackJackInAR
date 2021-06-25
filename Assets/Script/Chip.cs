using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public enum ChipType
	{
        RED, GREEN, BLACK
	};

    private float tempPosComp;
    private GameRule GameRule;
    private int cost;

    public float distance = 1.0f;
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

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("BetPlatform"))
		{
            if (GameRule.statusGame == GameRule.StatusGame.START_GAME)
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
            if (GameRule.statusGame == GameRule.StatusGame.START_GAME)
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
