using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRule : MonoBehaviour
{
    public enum StatusGame
	{
        START_GAME, IN_GAME, END_GAME
	};

    public StatusGame statusGame;

    public List<GameObject> chipsInBlueZone = new List<GameObject>();
    public GameObject StartBtn;
    public GameObject preview;
    public int totalScore;

    private List<GameObject> prevStack;
    private List<GameObject> stack = new List<GameObject>();
    private List<GameObject> myStack = new List<GameObject>();
    private List<GameObject> monicaStack = new List<GameObject>();
    private GameObject rotatedCard;
    private Text myScore;
    private Text MonicaScore;
    private int i;
    private int j = -6;
    private int q = -6;
    private Button take; 
    private Button stop;
    private GameObject restart;
    private bool totalWin = false;
    private bool totalLose = false;
    private static GameObject score;

    public AudioClip touchSound;
	private new AudioSource audio;

    public GameObject d2;
    public GameObject d3;
    public GameObject d4;
    public GameObject d5;
    public GameObject d6;
    public GameObject d7;
    public GameObject d8;
    public GameObject d9;
    public GameObject d10;
    public GameObject dJ;
    public GameObject dQ;
    public GameObject dK;
    public GameObject dA;

    public GameObject c2;
    public GameObject c3;
    public GameObject c4;
    public GameObject c5;
    public GameObject c6;
    public GameObject c7;
    public GameObject c8;
    public GameObject c9;
    public GameObject c10;
    public GameObject cJ;
    public GameObject cQ;
    public GameObject cK;
    public GameObject cA;

    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject s5;
    public GameObject s6;
    public GameObject s7;
    public GameObject s8;
    public GameObject s9;
    public GameObject s10;
    public GameObject sJ;
    public GameObject sQ;
    public GameObject sK;
    public GameObject sA;

    public GameObject h2;
    public GameObject h3;
    public GameObject h4;
    public GameObject h5;
    public GameObject h6;
    public GameObject h7;
    public GameObject h8;
    public GameObject h9;
    public GameObject h10;
    public GameObject hJ;
    public GameObject hQ;
    public GameObject hK;
    public GameObject hA;
    // Start is called before the first frame update
    void Start()
    {
        totalScore = 0;
        StartBtn = GameObject.Find("StartButton");
        myScore = GameObject.Find("MyScore").GetComponent<Text>();
        MonicaScore = GameObject.Find("MonicaScore").GetComponent<Text>();
        preview = GameObject.Find("Preview");
        audio = gameObject.AddComponent<AudioSource>();

        score  = GameObject.Find("Score");


        statusGame = StatusGame.START_GAME;

        prevStack = new List<GameObject>() { 
            d2, d3, d4, d5, d6, d7, d8, d9, d10, dJ, dQ, dK, dA, 
            c2, c3, c4, c5, c6, c7, c8, c9, c10, cJ, cQ, cK, cA,
            s2, s3, s4, s5, s6, s7, s8, s9, s10, sJ, sQ, sK, sA,
            h2, h3, h4, h5, h6, h7, h8, h9, h10, hJ, hQ, hK, hA};

        // Количество карт в игре - 7 колод по 52 карты
        for (int i = 0; i < 7; ++i)
            stack.AddRange(prevStack);

        take = GameObject.Find("Take").GetComponent<Button>();
        take.onClick.AddListener(TakeOne);

        stop = GameObject.Find("Stop").GetComponent<Button>();
        stop.onClick.AddListener(StopIt);

        restart = GameObject.Find("Restart");
        restart.GetComponent<Button>().onClick.AddListener(RestartScene);
        restart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
		{
            audio.PlayOneShot(touchSound);
        }

        if (statusGame == StatusGame.IN_GAME)
        {
            var rnd = new System.Random();
            GameObject card;

            // Первая карта Моники не будет перевернута по оси z
            float zz = 0.0f;
            

            // Выдача карт игроку
            for (int k = 0; k < 2; ++k)
			{
                i = rnd.Next(0, stack.Count-1);
                stack[i].transform.localScale = new Vector3(400.0f, 400.0f, 400.0f);
                card = Instantiate(stack[i], new Vector3(7, 36, j), Quaternion.Euler(0.0f, 90.0f, 180.0f));
                j += 6;

                // Карта из общего стека карт переходит в стек игрока
                myStack.Add(card);
                stack.RemoveAt(i);
            }

            // Выдача карт для Моники
            for (int k = 0; k < 2; ++k)
            {
                i = rnd.Next(0, stack.Count-1);
                stack[i].transform.localScale = new Vector3(400.0f, 400.0f, 400.0f);
                card = Instantiate(stack[i], new Vector3(-3, 36, q), Quaternion.Euler(0.0f, 90.0f, zz));

                // Запоминаем вниз лежащую карту Моники
                if (k == 0)
                    rotatedCard = card;

                // Карта из общего стека карт переходит в стек Моники
                monicaStack.Add(card);
                stack.RemoveAt(i);

                q += 6;
                zz = 180.0f;
            }
            // Окончание старта игры
            statusGame = StatusGame.END_GAME;
        }
    }

    // Игрок берет еще одну карту
    void TakeOne()
	{
        GameObject obj;

        var rnd = new System.Random();
        i = rnd.Next(0, stack.Count - 1);
        stack[i].transform.localScale = new Vector3(400.0f, 400.0f, 400.0f);
        obj = Instantiate(stack[i], new Vector3(7, 36, j), Quaternion.Euler(0.0f, 90.0f, 180.0f));
        j += 6;

        // Карта из общего стека карт переходит в стек игрока
        myStack.Add(obj);
        stack.RemoveAt(i);
    }

    // Игрок нажал Хватит
    void StopIt()
    {
        int myResult = Calculate(myStack);
        int monicaResult;

        // Проверка на досрочную победу или поражение
        if (myResult == 21)
		{
            totalWin = true;
            StartCoroutine(CompareResult(0, myResult));
        }
        else if (myResult > 21)
		{
            totalLose = true;
            StartCoroutine(CompareResult(0, myResult));
        }
        else
		{
            monicaResult = Calculate(monicaStack);

            // Переворачиваю первую карту Моники
            StartCoroutine(RotateCard());

            // Проверяю, чтобы Моника набрала хотя бы 17 очков
            if (monicaResult < 17)
		    {
                StartCoroutine(AddCardToMonica());
		    } else
		    {
                StartCoroutine(CompareResult(monicaResult, myResult));
            }
		}

        StartCoroutine(ShowRestart());
    }

    // Функция для подсчета очков
    int Calculate(List<GameObject> items)
	{
        int totalScore = 0;
        foreach (var elem in items)
        {
            if (elem.name.StartsWith("2"))
                totalScore += 2;
            if (elem.name.StartsWith("3"))
                totalScore += 3;
            if (elem.name.StartsWith("4"))
                totalScore += 4;
            if (elem.name.StartsWith("5"))
                totalScore += 5;
            if (elem.name.StartsWith("6"))
                totalScore += 6;
            if (elem.name.StartsWith("7"))
                totalScore += 7;
            if (elem.name.StartsWith("8"))
                totalScore += 8;
            if (elem.name.StartsWith("9"))
                totalScore += 9;
            if (elem.name.StartsWith("10") || elem.name.StartsWith("Jack") || elem.name.StartsWith("Queen") || elem.name.StartsWith("King"))
                totalScore += 10;
            if (elem.name.StartsWith("Ace"))
			{
                totalScore += 11;
                if (totalScore > 21)
				{
                    totalScore -= 10;
                }
            }  
        }
        return totalScore;
    }

    // Перезапуск сцены
    void RestartScene()
	{
        myScore.text = "0";
        MonicaScore.text = "0";
        GameObject.Find("Score").GetComponent<Text>().text = "Ставка: 0";
        restart.SetActive(false);

        foreach (var elem in myStack)
        {
            Destroy(elem);
        }
        myStack = new List<GameObject>();

        foreach (var elem in monicaStack)
        {
            Destroy(elem);
        }
        monicaStack = new List<GameObject>();

        // Если в общей колоде карт становится меньше трети о изначального количества,
        //               то сыгранные карты возвращаются в колоду и тасуются
        if (stack.Count < 120)
		{
            stack = new List<GameObject>();
            for (int i = 0; i < 7; ++i)
                stack.AddRange(prevStack);
        }

        StartBtn.GetComponent<StartToPlay>().StartButton.SetActive(true);
        StartBtn.GetComponent<Button>().onClick.AddListener(StartBtn.GetComponent<StartToPlay>().TaskOnClick);
        preview.SetActive(true);

        j = -6;
        q = -6;
        totalWin = false;
        totalLose = false;
    }


     void MonicaTakes()
    {
        GameObject obj;

        var rnd = new System.Random();
        i = rnd.Next(0, stack.Count - 1);
        stack[i].transform.localScale = new Vector3(400.0f, 400.0f, 400.0f);
        obj = Instantiate(stack[i], new Vector3(-3, 36, q), Quaternion.Euler(0.0f, 90.0f, 180.0f));
        q += 6;

        // Карта из общего стека карт переходит в стек Моники
        monicaStack.Add(obj);
        stack.RemoveAt(i);
    }


    public void ChangeScore(int value)
	{
        totalScore += value;
		score.GetComponent<Text>().text = $"Ставка: {totalScore}";
    }

    // Моника берет еще одну карту
    IEnumerator AddCardToMonica()
	{
        while (true)
		{
            yield return new WaitForSeconds(1);
            MonicaTakes();
            int monicaResult = Calculate(monicaStack);
            int myResult = Calculate(myStack);
            myScore.text = myResult.ToString();
            MonicaScore.text = monicaResult.ToString();
            if (monicaResult >= 17)
			{
                StartCoroutine(CompareResult(monicaResult, myResult));
                yield break;
			}
        }
	}

    // Переворот закрытой карты Моники
    IEnumerator RotateCard()
	{
        rotatedCard.GetComponent<Rigidbody>().AddForce(new Vector3(0, 60.0f, 0));
        rotatedCard.GetComponent<Rigidbody>().angularVelocity += new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;
        yield return new WaitForSeconds(2);
        yield break;
    }

    // Сравнение набранных очков Моники и игрока
    IEnumerator CompareResult(int Monica, int me)
	{
        yield return new WaitForSeconds(1);
        foreach (var elem in chipsInBlueZone)
        {
            Destroy(elem);
        }
        if ((totalWin || Monica <= me && me <= 21 || Monica > 21 && me <= 21) && !totalLose)
        {
            foreach (var elem in chipsInBlueZone)
            {
                int times = Monica != me ? 2 : 1;
                Vector3 pos = new Vector3();
                if (elem.name.StartsWith("GreenChip"))
                {
                    pos = new Vector3(18.48f, 50.0f, 8.64f);
                }
                else if (elem.name.StartsWith("RedChip"))
                {
                    pos = new Vector3(18.31f, 50.0f, 12.84f);
                }
                else if (elem.name.StartsWith("BlackChip"))
                {
                    pos = new Vector3(21.64f, 50.0f, 10.28f);
                }
                int v = new System.Random().Next(-90, 90);
                for (; times > 0; --times)
                {
                    GameObject chip = Instantiate(elem.gameObject, pos, Quaternion.Euler(90.0f, 0.0f, float.Parse(v.ToString())));
                    chip.AddComponent<Chip>();
                }
            }
        }
        chipsInBlueZone = new List<GameObject>();
        myScore.text = me.ToString();
        MonicaScore.text = Monica.ToString();
        yield break;
    }

    // Появление кнопки перезапуска игры
    IEnumerator ShowRestart()
	{
        yield return new WaitForSeconds(1);
        restart.SetActive(true);
        yield break;
    }
}