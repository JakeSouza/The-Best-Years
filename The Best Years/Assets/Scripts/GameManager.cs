using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private Text mainText;
    private float imageDelay = 2f;
    private GameObject main;
    private GameObject Allison;
    private GameObject Kayla;
    private GameObject Leo;
    private GameObject Leonard;
    private GameObject Logan;
    private GameObject Mykailah;
    private GameObject Paul;
    private GameObject ProfCates;
    private GameObject ProfSanchez;
    private GameObject currentImage;
    private List<string> characters = new List<string>(new string[] {"Allison", "Kayla", "Leo", "Leonard", "Logan", "Mykailah", "Paul", "ProfCates", "ProfSanchez" });
    [HideInInspector] public bool playersTurn = true;
    private string character;
    private float speed = 19900f;
    private int days;

    private static GameObject gradeBar;
    private static GameObject socialBar;
    private static GameObject healthBar;
    private static GameObject moneyBar;

    public static GameObject loseGrades;
    public static GameObject loseSocial;
    public static GameObject loseHealth;
    public static GameObject loseMoney;
    public static GameObject win;

    private Text scenarioText;
    private Text dayText;

    //private playerHealth social;

    // Use this for initialization
    void Awake () {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        Allison = GameObject.Find("Allison");
        Kayla = GameObject.Find("Kayla");
        Leo = GameObject.Find("Leo");
        Leonard = GameObject.Find("Leonard");
        Logan = GameObject.Find("Logan");
        Mykailah = GameObject.Find("Mykailah");
        Paul = GameObject.Find("Paul");
        ProfCates = GameObject.Find("ProfCates");
        ProfSanchez = GameObject.Find("ProfSanchez");

        gradeBar = GameObject.Find("gradeBar");
        socialBar = GameObject.Find("socialBar");
        healthBar = GameObject.Find("healthBar");
        moneyBar = GameObject.Find("moneyBar");

        loseGrades = GameObject.Find("LoseGrades");
        loseHealth = GameObject.Find("LoseHealth");
        loseSocial = GameObject.Find("LoseSocial");
        loseMoney = GameObject.Find("LoseMoney");
        win = GameObject.Find("Win");

        scenarioText = GameObject.Find("Scenarios").GetComponent<Text>();
        dayText = GameObject.Find("Days").GetComponent<Text>();
        days = 0;

        hideAllnpc();
        hideBars();
        hideAllOutcomes();
        //hideText(scenarioText);
    }
	
	// Update is called once per frame
	void Update () {

        InitGame();
        Interaction();
    }

    void hideImage(GameObject image)
    {
        image.SetActive(false);
    }

    void hideText( Text text)
    {
        text.enabled = false;
    }

    void showText(Text text)
    {
        text.enabled = true;
    }

    void showImage (GameObject image)
    {
        image.SetActive(true);
        //print("Image");
    }

    void hideAllnpc()
    {
        Allison.SetActive(false);
        Kayla.SetActive(false);
        Leo.SetActive(false);
        Leonard.SetActive(false);
        Logan.SetActive(false);
        Mykailah.SetActive(false);
        Paul.SetActive(false);
        ProfCates.SetActive(false);
        ProfSanchez.SetActive(false);
    }

    void hideAllOutcomes()
    {
        loseGrades.SetActive(false);
        loseSocial.SetActive(false);
        loseHealth.SetActive(false);
        loseMoney.SetActive(false);
        win.SetActive(false);
    }

    static void hideBars()
    {
        gradeBar.SetActive(false);
        socialBar.SetActive(false);
        healthBar.SetActive(false);
        moneyBar.SetActive(false);
    }

    void showBars()
    {
        gradeBar.SetActive(true);
        socialBar.SetActive(true);
        healthBar.SetActive(true);
        moneyBar.SetActive(true);
    }

    void InitGame()
    {
        mainText = GameObject.Find("Text").GetComponent<Text>();
        main = GameObject.Find("Main");

        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, Mathf.Sin(Time.time * 3));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //hideAllnpc();
            hideImage(main);
            hideText(mainText);
            character = getCharacter();
            loadnpc(character);
            showBars();
        }
    }
    void loadnpc(string character)
    {
        if (character == "Allison")
        {
            print(Allison);
            showImage(Allison);
            currentImage = Allison;
            scenarioText.text = "Scenario";
            setDays();
            youWin();
        }
        else if (character == "Kayla")
        {
            print("Kayla");
            showImage(Kayla);
            currentImage = Kayla;
            setDays();
            youWin();
        }
        else if (character == "Leo")
        {
            print("Leo");
            showImage(Leo);
            currentImage = Leo;
            setDays();
            youWin();
        }
        else if (character == "Leonard")
        {
            print("Leonard");
            showImage(Leonard);
            currentImage = Leonard;
            setDays();
            youWin();
        }
        else if (character == "Logan")
        {
            print("Logan");
            showImage(Logan);
            currentImage = Logan;
            setDays();
            youWin();
        }
        else if (character == "Mykailah")
        {
            print("Mykailah");
            showImage(Mykailah);
            currentImage = Mykailah;
            setDays();
            youWin();
        }
        else if (character == "Paul")
        {
            print("Paul");
            showImage(Paul);
            currentImage = Paul;
            setDays();
            youWin();
        }
        else if (character == "ProfCates")
        {
            print("ProfCates");
            showImage(ProfCates);
            currentImage = ProfCates;
            setDays();
            youWin();
        }
        else
        {
            print("ProfSanchez");
            showImage(ProfSanchez);
            currentImage = ProfSanchez;
            setDays();
            youWin();
        }
    }

    void hidenpc (string character)
    {
        if (character == "Allison")
        {
            hideImage(Allison);
        }
        else if (character == "Kayla")
        {
            hideImage(Kayla);
        }
        else if (character == "Leo")
        {
            hideImage(Leo);
        }
        else if (character == "Leonard")
        {
            hideImage(Leonard);
        }
        else if (character == "Logan")
        {
            hideImage(Logan);
        }
        else if (character == "Mykailah")
        {
            hideImage(Mykailah);
        }
        else if (character == "Paul")
        {
            hideImage(Paul);
        }
        else if (character == "ProfCates")
        {
            hideImage(ProfCates);
        }
        else
        {
            hideImage(ProfSanchez);
        }
    }

    string getCharacter()
    {
        System.Random rnd = new System.Random();
        int number = rnd.Next(0,9);
        return characters[number];
    }

    void Interaction()
    {

        character = getCharacter();
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //currentImage.transform.Translate(-Vector3.right * speed * Time.deltaTime);
            hideImage(currentImage);
            loadnpc(character);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //currentImage.transform.Translate(Vector3.right * speed * Time.deltaTime);
            hideImage(currentImage);
            loadnpc(character);
        }
    }

    public static void youLoseGrades()
    {
        loseGrades.SetActive(true);
        hideBars();
        
    }

    public static void youLoseSocial()
    {
        loseSocial.SetActive(true);
        hideBars();
    }

    public static void youLoseHealth()
    {
        loseHealth.SetActive(true);
        hideBars();
    }

    public static void youLoseMoney()
    {
        loseMoney.SetActive(true);
        hideBars();
    }

    void youWin()
    {
        if(days == 50)
        {
            win.SetActive(true);
            hideBars();
        }
        
    }

    void setDays ()
    {
        days++;
        dayText.text = "Day " + days;
    }
}
