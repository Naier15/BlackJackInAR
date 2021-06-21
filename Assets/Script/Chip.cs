using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chip : MonoBehaviour
{
    private Touch touch;
    private float tempPos;
    private bool bet = false;
    private Text score;
    private int totalScore;
    private string objName;
    private GameObject BlackJackScene;

    public float distance = 15.0f;

    void Start()
    {
        objName = name;
        score = GameObject.Find("Score").GetComponent<Text>();
        BlackJackScene = GameObject.Find("BlackJackScene");
    }


    void Update()
    {
        bool inGame = BlackJackScene.GetComponent<GameRule>().inGame;

        // Вносим в счет фишки, лежащие в синей зоне
        if (inGame && !bet && 14 < transform.position.x && transform.position.x < 24 && -13 < transform.position.z && transform.position.z < -4)
        {
            List<GameObject> chips = GameObject.Find("BlackJackScene").GetComponent<GameRule>().chips;
            totalScore = int.Parse(score.text.Split(new char[] {' '})[1]);
            bet = true;
            if (objName.StartsWith("RedChip"))
            {
                totalScore += 5;
            } else if (objName.StartsWith("GreenChip"))
            {
                totalScore += 10;
            } else if (objName.StartsWith("BlackChip"))
            {
                totalScore += 20;
            }
            chips.Add(gameObject);
            score.text = $"Ставка: {totalScore}";
        } 

        // Выносим из счета забранные из синей зоны фишки
        if (inGame && bet && !(14 < transform.position.x && transform.position.x < 24 && -13 < transform.position.z && transform.position.z < -4))
		{
            List<GameObject> chips = GameObject.Find("BlackJackScene").GetComponent<GameRule>().chips;
            totalScore = int.Parse(score.text.Split(new char[] { ' ' })[1]);
            bet = false;
            if (objName.StartsWith("RedChip"))
            {
                totalScore -= 5;
            }
            else if (objName.StartsWith("GreenChip"))
            {
                totalScore -= 10;
            }
            else if (objName.StartsWith("BlackChip"))
            {
                totalScore -= 20;
            }
            chips.Remove(gameObject);
            score.text = $"Ставка: {totalScore}";
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
