using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartToPlay : MonoBehaviour
{
    public GameObject StartButton;
    private GameObject Take;
    private GameObject Stop;
    public GameObject prev;
    public GameRule GameRule;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
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

            GameRule.statusGame += 1;
        }
    }
}
