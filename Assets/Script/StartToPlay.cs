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
    public GameObject gr;

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
        gr = GameObject.Find("BlackJackScene");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TaskOnClick()
    {
        string score = GameObject.Find("Score").GetComponent<Text>().text;
        if (int.Parse(score.Split(new char[] { ' ' })[1]) > 0)
        {
            StartButton.SetActive(false);
            
            if (prev.activeInHierarchy)
			{
                prev.SetActive(false);
            }
            

            Take.SetActive(true);
            Stop.SetActive(true);

            gr.GetComponent<GameRule>().started = true;
        }
    }


    /*if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began) {
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);

    if (Physics.Raycast(ray, out hit, float.MaxValue) && dist <= 58.5f) {
        hit.SendMessage("OnPress" , SendMessageOptions.DontRequireReceiver);
    }
    }*/
}
