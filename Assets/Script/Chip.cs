using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour
{
    public enum ChipType
	{
        RED, GREEN, BLACK
	};

    private Touch touch;
    private float tempPos;
    private float tempPosAndr;
    private bool bet = false;
    private Text score;
    private string objName;
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
        bool inGame = GameRule.inGame;

        // Ввод с android
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
		{
            touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name.Contains("Chip"))
                {
                    tempPosAndr = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance)).y;
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;

                        Vector3 objPositionAndr = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distance));

                        if (objPositionAndr.y > tempPosAndr)
                        {
                            objPositionAndr.x -= (objPositionAndr.y - tempPos) * 3.0f;
                        }

                        Touch touch2 = Input.GetTouch(1);
                        
                        Vector2 tappedPos;
                        if ((Input.touchCount > 1) && (Input.touches[1].phase == TouchPhase.Moved))
                        {
                            tappedPos = touch2.position;
                            float dist = Vector2.Distance(touch2.position, tappedPos);
                            hit.collider.GetComponent<Rigidbody>().angularVelocity += new Vector3(0.0f, 0.0f, dist) * Time.deltaTime;
                        }
                        hit.collider.transform.position = objPositionAndr;
                    }
                    else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    }
                }
            }
        }

        



        // Вносим в счет фишки, лежащие в синей зоне
        if (inGame && !bet && 14 < transform.position.x && transform.position.x < 24 && -13 < transform.position.z && transform.position.z < -4)
        {
            List<GameObject> chips = GameObject.Find("BlackJackScene").GetComponent<GameRule>().chips;
            
            bet = true;

            chips.Add(gameObject);
            GameRule.ChangeScore(cost);
        } 

        // Выносим из счета забранные из синей зоны фишки
        if (inGame && bet && !(14 < transform.position.x && transform.position.x < 24 && -13 < transform.position.z && transform.position.z < -4))
		{
            List<GameObject> chips = GameObject.Find("BlackJackScene").GetComponent<GameRule>().chips;
            
            bet = false;
            chips.Remove(gameObject);

            //Смена очков в общей ставке
			GameRule.ChangeScore(-cost);
        }


        /*if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out var hit, 5.0f) && !bet)
        {
            if (hit.collider.name == "BetPlatform")
            {
                Debug.Log($"----- {transform.position} {transform.position - transform.up} {hit.collider.transform.position}");
                bet = true;
            }
        }*/

        //touch = Input.GetTouch(0);
        //transform.position = new Vector3(touch.position.x, touch.position.y, transform.position.z) * Time.deltaTime;

        /*if (Input.touchCount > 0)
        {
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 1.0f, go.transform.position.z);
        }*/
    }

    private void OnMouseDown()
    {
        tempPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)).y;
    }

    private void OnMouseDrag()
    {
        GetComponent<Rigidbody>().useGravity = false;

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        if (objPosition.y > tempPos)
        {
            objPosition.x -= (objPosition.y - tempPos) * 3.0f;
        }
        transform.position = objPosition;
    }
    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().useGravity = true;
    }
}
