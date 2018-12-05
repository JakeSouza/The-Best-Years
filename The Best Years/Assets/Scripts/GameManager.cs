using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    private Text mainText;
    private Text instructions;
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
    [HideInInspector] public bool playersTurn = true;
    private string character;
    private float speed = 19900f;
    private int days;

    public static Slider gradeBar;
    public static Slider socialBar;
    public static Slider healthBar;
    public static Slider moneyBar;

    public static float gradeHealth { get; set; }
    public static float socialHealth { get; set; }
    public static float healthHealth { get; set; }
    public static float moneyHealth { get; set; }
    public static float maxHealth { get; set; }
    public static float startHealth { get; set; }

    public static GameObject loseGrades;
    public static GameObject loseSocial;
    public static GameObject loseHealth;
    public static GameObject loseMoney;
    public static GameObject win;
    stackedLists returnHolder = new stackedLists();

    private Text scenarioText;
    private Text daysSurvived;
    private Text dayText;
    private Text characterName;
    System.Random rnd = new System.Random();
    private int remaining = 25;

    private List<string> used = new List<string>();
    private List<string> active = new List<string>(); //stores questions
    private string line;    //used for reading in questions
    //private playerHealth social;

    // Use this for initialization
    void Awake()
    {
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

        gradeBar = GameObject.Find("gradeBar").GetComponent<Slider>();
        socialBar = GameObject.Find("socialBar").GetComponent<Slider>();
        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();
        moneyBar = GameObject.Find("moneyBar").GetComponent<Slider>();

        loseGrades = GameObject.Find("LoseGrades");
        loseHealth = GameObject.Find("LoseHealth");
        loseSocial = GameObject.Find("LoseSocial");
        loseMoney = GameObject.Find("LoseMoney");
        win = GameObject.Find("Win");

        scenarioText = GameObject.Find("Scenarios").GetComponent<Text>();
        daysSurvived = GameObject.Find("Days Survived").GetComponent<Text>();
        characterName = GameObject.Find("Character").GetComponent<Text>();
        dayText = GameObject.Find("Days").GetComponent<Text>();
        days = 0;

        maxHealth = 100f;
        startHealth = 50f;
        //resest health at start of game
        gradeHealth = startHealth;
        socialHealth = startHealth;
        healthHealth = startHealth;
        moneyHealth = startHealth;
        gradeBar.value = calculateGradeHealth();
        socialBar.value = calculateSocialHealth();
        healthBar.value = calculateHealthHealth();
        moneyBar.value = calculateMoneyHealth();

        hideAllnpc();
        hideBars();
        hideAllOutcomes();
        hideText(daysSurvived);
        //hideText(scenarioText);

        var fileStream = new FileStream(@"C:\Users\laure\OneDrive\Documents\GitHub\The-Best-Years\The Best Years\questions.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream))
        {
            while ((line = streamReader.ReadLine()) != null)
            {
                active.Add(line);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        InitGame();
        Interaction();
    }

    void hideImage(GameObject image)
    {
        image.SetActive(false);
    }

    void hideText(Text text)
    {
        text.enabled = false;
    }

    void showText(Text text)
    {
        text.enabled = true;
    }

    void showImage(GameObject image)
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
        gradeBar.enabled = false;
        socialBar.enabled = false;
        healthBar.enabled = false;
        moneyBar.enabled = false;
    }

    void showBars()
    {
        gradeBar.enabled = true;
        socialBar.enabled = true;
        healthBar.enabled = true;
        moneyBar.enabled = true;
    }

    void InitGame()
    {
        mainText = GameObject.Find("Text").GetComponent<Text>();
        instructions = GameObject.Find("Instructions").GetComponent<Text>();
        main = GameObject.Find("Main");

        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, Mathf.Sin(Time.time * 3));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //hideAllnpc();
            hideImage(main);
            hideText(mainText);
            hideText(instructions);
            stackedLists temp = new stackedLists();
            temp = getCharacter();
            character = temp.name;
            scenarioText.text = temp.interaction;
            active = temp.Prop1;
            used = temp.Prop2;
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
            characterName.text = "Allison";
            setDays();
            youWin();
        }
        else if (character == "Kayla")
        {
            print("Kayla");
            showImage(Kayla);
            currentImage = Kayla;
            characterName.text = "Kayla";
            setDays();
            youWin();
        }
        else if (character == "Leo")
        {
            print("Leo");
            showImage(Leo);
            currentImage = Leo;
            characterName.text = "Leo";
            setDays();
            youWin();
        }
        else if (character == "Leonard")
        {
            print("Leonard");
            showImage(Leonard);
            currentImage = Leonard;
            characterName.text = "Leonard";
            setDays();
            youWin();
        }
        else if (character == "Logan")
        {
            print("Logan");
            showImage(Logan);
            currentImage = Logan;
            characterName.text = "Logan";
            setDays();
            youWin();
        }
        else if (character == "Mykailah")
        {
            print("Mykailah");
            showImage(Mykailah);
            currentImage = Mykailah;
            characterName.text = "Mykailah";
            setDays();
            youWin();
        }
        else if (character == "Paul")
        {
            print("Paul");
            showImage(Paul);
            currentImage = Paul;
            characterName.text = "Paul";
            setDays();
            youWin();
        }
        else if (character == "ProfCates")
        {
            print("ProfCates");
            showImage(ProfCates);
            currentImage = ProfCates;
            characterName.text = "Professor Cates";
            setDays();
            youWin();
        }
        else
        {
            print("ProfSanchez");
            showImage(ProfSanchez);
            currentImage = ProfSanchez;
            characterName.text = "Professor Sanchez";
            setDays();
            youWin();
        }
    }

    void hidenpc(string character)
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
        else if (character == "ProfScanchez")
        {
            hideImage(ProfSanchez);
        }
    }

    stackedLists getCharacter()
    {

        returnHolder = pickRandomly(active, used);
        return returnHolder;
    }

    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //currentImage.transform.Translate(-Vector3.right * speed * Time.deltaTime);
            gradeDamage(returnHolder.no[0]);
            socialDamage(returnHolder.no[1]);
            healthDamage(returnHolder.no[2]);
            moneyDamage(returnHolder.no[3]);
            stackedLists temp = new stackedLists();
            temp = getCharacter();
            scenarioText.text = temp.interaction;
            character = temp.name;
            active = temp.Prop1;
            used = temp.Prop2;
            hideImage(currentImage);
            loadnpc(character);
        }

        else if (Input.GetKeyDown(KeyCode.Y))
        {
            //currentImage.transform.Translate(Vector3.right * speed * Time.deltaTime); 
            gradeDamage(returnHolder.yes[0]);
            socialDamage(returnHolder.yes[1]);
            healthDamage(returnHolder.yes[2]);
            moneyDamage(returnHolder.yes[3]);
            stackedLists temp = new stackedLists();
            temp = getCharacter();
            scenarioText.text = temp.interaction;
            character = temp.name;
            active = temp.Prop1;
            used = temp.Prop2;
            hideImage(currentImage);
            loadnpc(character);
        }
    }

    public void youLoseGrades()
    {
        loseGrades.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);

    }

    public void youLoseSocial()
    {
        loseSocial.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
    }

    public void youLoseHealth()
    {
        loseHealth.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
    }

    public void youLoseMoney()
    {
        loseMoney.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
    }

    void youWin()
    {
        if (days == 50)
        {
            win.SetActive(true);
            hideBars();
            daysSurvived.text = "Congratulations you graduated!";
            showText(daysSurvived);
        }

    }

    void setDays()
    {
        days++;
        dayText.text = "Day " + days;
    }

    public stackedLists pickRandomly(List<string> one, List<string> two)
    {
        // initialize variables to be used in method
        int selector = 0;
        string name = "";
        string interaction = "";
        List<string> yes = new List<string>();
        List<string> no = new List<string>();
        stackedLists returnHolder = new stackedLists();


        //if more than one interaction left
        if (one.Count > 10)
        {
            //pick random cell in list
            selector = rnd.Next(0, remaining - 1) * 10;
            remaining--;
            name = one[selector];
            two.Add(name);
            one.RemoveAt(selector);
            interaction = one[selector];
            two.Add(interaction);
            one.RemoveAt(selector);

            for (int i = 0; i < 4; i++)
            {
                yes.Add(active[selector]);
                two.Add(active[selector]);
                one.RemoveAt(selector);
            }

            for (int i = 0; i < 4; i++)
            {
                no.Add(active[selector]);
                two.Add(active[selector]);
                one.RemoveAt(selector);
            }

            return new stackedLists { Prop1 = one, Prop2 = two, name = name, interaction = interaction, yes = yes, no = no };
        }
        //if only one interaction left
        else
        {
            //pick random cell in list
            selector = 0;
            remaining--;
            print(one.Count);
            name = one[selector];
            two.Add(name);
            print(one.Count);
            one.RemoveAt(selector);
            interaction = one[selector];
            two.Add(interaction);
            print(one.Count);
            one.RemoveAt(selector);

            for (int i = 0; i < 4; i++)
            {
                yes.Add(active[selector]);
                two.Add(active[selector]);
                print(one.Count);
                one.RemoveAt(selector);
            }

            for (int i = 0; i < 4; i++)
            {
                no.Add(active[selector]);
                two.Add(active[selector]);
                print(one.Count);
                one.RemoveAt(selector);
            }

            //run isEmpty method, which refills the active list and empties used list
            returnHolder = isEmpty(one, two);
            one = returnHolder.Prop1;
            two = returnHolder.Prop2;
            //return edited lists

            return new stackedLists { Prop1 = one, Prop2 = two, name = name, interaction = interaction, yes = yes, no = no };
        }
    }

    //checks if the active array is empty, if yes refill it and empty used array
    public stackedLists isEmpty(List<string> one, List<string> two)
    {

        foreach (string n in two)
        {
            one.Add(n);
        }
        for (int i = 0; i < two.Count - 1; i++)
        {
            two.RemoveAt(0);
        }
        remaining = 33;
        return new stackedLists { Prop1 = one, Prop2 = two };
    }

    public void gradeDamage(string damageValue)
    {
        // deduct damage
        gradeHealth += int.Parse(damageValue);
        gradeBar.value = calculateGradeHealth();

        // if no health, you lose
        if (gradeHealth <= 0)
        {
            youLoseGrades();
        }
    }

    public void socialDamage(string damageValue)
    {
        // deduct damage
        socialHealth += int.Parse(damageValue);
        socialBar.value = calculateSocialHealth();
        // if no health, you lose
        if (socialHealth <= 0)
        {
           youLoseSocial();
        }
    }

    public void healthDamage(string damageValue)
    {
        // deduct damage
        healthHealth += int.Parse(damageValue);
        healthBar.value = calculateHealthHealth();

        // if no health, you lose
        if (healthHealth <= 0)
        {
            youLoseHealth();
        }
    }

    public void moneyDamage(string damageValue)
    {
        // deduct damage
        moneyHealth += int.Parse(damageValue);
        moneyBar.value = calculateMoneyHealth();

        // if no health, you lose
        if (moneyHealth <= 0)
        {
            youLoseMoney();
        }
    }

    float calculateGradeHealth()
    {
        return gradeHealth / maxHealth;
    }

    float calculateSocialHealth()
    {
        return socialHealth / maxHealth;
    }

    float calculateHealthHealth()
    {
        return healthHealth / maxHealth;
    }

    float calculateMoneyHealth()
    {
        return moneyHealth / maxHealth;
    }
}

public class stackedLists
{
    public List<string> Prop1 { get; set; }
    public List<string> Prop2 { get; set; }

    public string name;
    public string interaction;
    public List<string> yes { get; set; }
    public List<string> no { get; set; }
}


