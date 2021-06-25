using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartToPlay : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject Take;
    public GameObject Stop;
    public GameObject prev;
    private GameRule GameRule;


    void Start()
    {
        StartButton = GameObject.Find("StartButton");
        StartButton.GetComponent<Button>().onClick.AddListener(TaskOnClick);

        Take = GameObject.Find("Take");
        Take.SetActive(false);

        Stop = GameObject.Find("Stop");
        Stop.SetActive(false);

        prev = GameObject.Find("Preview");
        GameRule = GameObject.Find("BlackJackScene").GetComponent<GameRule>();
    }

    public void TaskOnClick()
    {
        if (GameRule.totalScore > 0)
        {
            StartButton.SetActive(false);
            
            if (prev.activeInHierarchy)
			{
                prev.SetActive(false);
            }
            
            Take.SetActive(true);
            Stop.SetActive(true);
            GameRule.statusGame = GameRule.StatusGame.IN_GAME;
        }
    }
}
