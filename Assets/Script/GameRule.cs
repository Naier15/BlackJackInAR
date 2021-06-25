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

    public StatusGame statusGame = StatusGame.START_GAME;
    public List<GameObject> chipsInBlueZone = new List<GameObject>();
    public GameObject StartBtn;
    public GameObject preview;
    public static int totalScore;
    public List<GameObject> prevStack;

    private List<GameObject> stack = new List<GameObject>();
    private List<GameObject> myStack = new List<GameObject>();
    private List<GameObject> monicaStack = new List<GameObject>();
    private GameObject rotatedCard;
    private Text myScore;
    private Text MonicaScore;
    private int i;
    private float j = 0.4f;
    private float q = 0.4f;
    private Button take; 
    private Button stop;
    private GameObject restart;
    private bool totalLose = false;
    private static GameObject score;
    private GameObject card;
    private GameObject scene;
    private GameObject chips;
    private Vector3 positionOfGreenChips = new Vector3(-0.05f, 0.7f, 1.8f);
    private Vector3 positionOfRedChips = new Vector3(-0.05f, 0.7f, 2.2f);
    private Vector3 positionOfBlackChips = new Vector3(0.29f, 0.7f, 1.95f);
    private Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, 180.0f);

    public AudioClip touchSound;
	private new AudioSource audio;

	private void Start()
	{
        totalScore = 0;
        StartBtn = GameObject.Find("StartButton");
        myScore = GameObject.Find("MyScore").GetComponent<Text>();
        MonicaScore = GameObject.Find("MonicaScore").GetComponent<Text>();
        preview = StartBtn.GetComponent<StartToPlay>().prev;
        audio = gameObject.AddComponent<AudioSource>();
        scene = GameObject.Find("ScenePrefab");
        chips = GameObject.Find("Chips");
        score = GameObject.Find("Score");

        // Количество карт в игре - 7 колод по 52 карты
        for (int i = 0; i < 7; ++i)
            stack.AddRange(prevStack);


        take = StartBtn.GetComponent<StartToPlay>().Take.GetComponent<Button>();
        take.onClick.AddListener(TakeOne);

        stop = StartBtn.GetComponent<StartToPlay>().Stop.GetComponent<Button>();
        stop.onClick.AddListener(StopIt);

        restart = GameObject.Find("Restart");
        restart.GetComponent<Button>().onClick.AddListener(RestartScene);
        restart.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
            audio.PlayOneShot(touchSound);

        if (statusGame == StatusGame.IN_GAME)
        {
            var rnd = new System.Random();
            // Первая карта Моники не будет перевернута по оси z
            float rotationOfFirstCard = 0.0f;
            

            // Выдача карт игроку
            for (int k = 0; k < 2; ++k)
			{
                i = rnd.Next(0, stack.Count-1);
                card = Instantiate(stack[i], new Vector3(-1.0f, 0.2f, j), rotation, scene.transform);
                
                j += 0.6f;

                // Карта из общего стека карт переходит в стек игрока
                myStack.Add(card);
                stack.RemoveAt(i);
            }

            // Выдача карт для Моники
            for (int k = 0; k < 2; ++k)
            {
                i = rnd.Next(0, stack.Count-1);
                card = Instantiate(stack[i], new Vector3(-1.8f, 0.2f, q), Quaternion.Euler(0.0f, 90.0f, rotationOfFirstCard), scene.transform);

                // Запоминаем вниз лежащую карту Моники
                if (k == 0)
                    rotatedCard = card;

                // Карта из общего стека карт переходит в стек Моники
                monicaStack.Add(card);
                stack.RemoveAt(i);

                q += 0.6f;
                rotationOfFirstCard = 180.0f;
            }
            // Окончание старта игры
            statusGame = StatusGame.END_GAME;
        }
    }

    // Игрок берет еще одну карту
    void TakeOne()
	{
        var rnd = new System.Random();
        i = rnd.Next(0, stack.Count - 1);
        card = Instantiate(stack[i], new Vector3(-1.0f, 0.2f, j), rotation, scene.transform);
        j += 0.6f;

        // Карта из общего стека карт переходит в стек игрока
        myStack.Add(card);
        stack.RemoveAt(i);
    }

    // Игрок нажал Хватит
    void StopIt()
    {
        int myResult = Calculate(myStack);

        // Проверка на досрочное поражение
        if (myResult > 21)
		{
            totalLose = true;
            StartCoroutine(CompareResult(0, myResult));
        }
        else
		{
            // Переворачиваю первую карту Моники
            StartCoroutine(RotateCard());
            StartCoroutine(AddCardToMonica());
		}
        StartCoroutine(ShowRestart());
    }

    // Функция для подсчета очков
    int Calculate(List<GameObject> items)
	{
        int result = 0;
        foreach (var elem in items)
        {
            if (elem.name.StartsWith("2"))
                result += 2;
            else if (elem.name.StartsWith("3"))
                result += 3;
            else if (elem.name.StartsWith("4"))
                result += 4;
            else if (elem.name.StartsWith("5"))
                result += 5;
            else if (elem.name.StartsWith("6"))
                result += 6;
            else if (elem.name.StartsWith("7"))
                result += 7;
            else if (elem.name.StartsWith("8"))
                result += 8;
            else if (elem.name.StartsWith("9"))
                result += 9;
            else if (elem.name.StartsWith("10") || elem.name.StartsWith("Jack") || elem.name.StartsWith("Queen") || elem.name.StartsWith("King"))
                result += 10;
            else if (elem.name.StartsWith("Ace"))
			{
                result += 11;
                if (result > 21)
                    result -= 10;
            }  
        }
        return result;
    }

    // Перезапуск сцены
    public void RestartScene()
	{
        myScore.text = "0";
        MonicaScore.text = "0";
        score.GetComponent<Text>().text = "Ставка: 0";
        restart.SetActive(false);
        totalScore = 0;
        statusGame = StatusGame.START_GAME;
        j = 0.4f;
        q = 0.4f;
        totalLose = false;

        foreach (var elem in myStack)
            Destroy(elem);
        myStack = new List<GameObject>();

        foreach (var elem in monicaStack)
            Destroy(elem);
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
        preview.SetActive(true);
    }


     void MonicaTakes()
    {
        var rnd = new System.Random();
        i = rnd.Next(0, stack.Count - 1);
        card = Instantiate(stack[i], new Vector3(-1.8f, 0.3f, q), rotation, scene.transform);
        q += 0.6f;

        // Карта из общего стека карт переходит в стек Моники
        monicaStack.Add(card);
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
            int monicaResult = Calculate(monicaStack);
            if (monicaResult >= 17)
            {
                int myResult = Calculate(myStack);
                myScore.text = myResult.ToString();
                StartCoroutine(CompareResult(monicaResult, myResult));
                yield break;
            }
            yield return new WaitForSeconds(1);
            MonicaTakes();
            MonicaScore.text = monicaResult.ToString();

        }
	}

    // Переворот закрытой карты Моники
    IEnumerator RotateCard()
	{
        rotatedCard.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3000.0f, 0));
        rotatedCard.GetComponent<Rigidbody>().angularVelocity += new Vector3(900.0f, 0.0f, 0.0f) * Time.deltaTime;
        yield return new WaitForSeconds(2);
        yield break;
    }

    // Сравнение набранных очков Моники и игрока
    IEnumerator CompareResult(int Monica, int me)
	{
        yield return new WaitForSeconds(1);
        foreach (var elem in chipsInBlueZone)
            Destroy(elem);

        if ((Monica <= me || Monica > 21) && me <= 21 && !totalLose)
        {
            foreach (var elem in chipsInBlueZone)
            {
                int times = Monica != me ? 2 : 1;
                Vector3 pos;

                if (elem.name.StartsWith("Green"))
                    pos = positionOfGreenChips;
                else if (elem.name.StartsWith("Red"))
                    pos = positionOfRedChips;
                else
                    pos = positionOfBlackChips;

                for (; times > 0; --times)
                {
                    GameObject chip = Instantiate(elem.gameObject, pos, Quaternion.Euler(90.0f, 0.0f, Random.Range(-90, 90)), chips.transform);
                    chip.GetComponent<Chip>().enabled = true;
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