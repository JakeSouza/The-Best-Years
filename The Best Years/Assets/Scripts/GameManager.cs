using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;

/*
 * Created by: Lauren Ramirez, Isaac Martin, Jake Souza, Victoria Cigarroa
 * COMSC 450 Game Design
 * Final Project: The Best Years
 */

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    //Store Text
    private Text mainText;
    private Text instructions;

    //Store Images
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
    private int days;
    private int remaining = 54;

    public static Slider gradeBar;
    public static Slider socialBar;
    public static Slider healthBar;
    public static Slider moneyBar;

    public AudioClip interactionSound;
    public AudioClip loseSound;
    public AudioClip winSound;

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

    //File Path
    private string path;

    System.Random rnd = new System.Random();

    private List<string> used = new List<string>();
    //Stores Questions
    private List<string> active = new List<string>();
    //Used for reading in questions
    private string line; 
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

        //Find NPC Player Images
        Allison = GameObject.Find("Allison");
        Kayla = GameObject.Find("Kayla");
        Leo = GameObject.Find("Leo");
        Leonard = GameObject.Find("Leonard");
        Logan = GameObject.Find("Logan");
        Mykailah = GameObject.Find("Mykailah");
        Paul = GameObject.Find("Paul");
        ProfCates = GameObject.Find("ProfCates");
        ProfSanchez = GameObject.Find("ProfSanchez");

        //Find Slider Bars
        gradeBar = GameObject.Find("gradeBar").GetComponent<Slider>();
        socialBar = GameObject.Find("socialBar").GetComponent<Slider>();
        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();
        moneyBar = GameObject.Find("moneyBar").GetComponent<Slider>();

        //Find Losing Scenario Images
        loseGrades = GameObject.Find("LoseGrades");
        loseHealth = GameObject.Find("LoseHealth");
        loseSocial = GameObject.Find("LoseSocial");
        loseMoney = GameObject.Find("LoseMoney");
        win = GameObject.Find("Win");

        //Find Text for gameplay
        scenarioText = GameObject.Find("Scenarios").GetComponent<Text>();
        daysSurvived = GameObject.Find("Days Survived").GetComponent<Text>();
        characterName = GameObject.Find("Character").GetComponent<Text>();
        dayText = GameObject.Find("Days").GetComponent<Text>();

        //Set Number of days, maximum health and start health
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

        //Hide Images
        hideAllnpc();
        hideBars();
        hideAllOutcomes();
        hideText(daysSurvived);

        //Read Questions from text file
        path = Application.dataPath + "/StreamingAssets"; //Streaming Assets is used so that the file could be used in built version of game
        var fileStream = new FileStream(path+"/questions.txt", FileMode.Open, FileAccess.Read);
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
        //Call Functions
        InitGame();
        Interaction();
    }

    /*
     * Hides images from main screen 
     */
    void hideImage(GameObject image)
    {
        image.SetActive(false);
    }

    /*
     * Hides Text from main screen 
     */
    void hideText(Text text)
    {
        text.enabled = false;
    }

    /*
     * Shows text from main screen 
     */
    void showText(Text text)
    {
        text.enabled = true;
    }

    /*
     * Shows images from main screen 
     */
    void showImage(GameObject image)
    {
        image.SetActive(true);
        //print("Image");
    }

    /*
     * Hides all the NPC images from main screen 
     */
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

    /*
     * Hides all the losing/winning images from main screen 
     */
    void hideAllOutcomes()
    {
        loseGrades.SetActive(false);
        loseSocial.SetActive(false);
        loseHealth.SetActive(false);
        loseMoney.SetActive(false);
        win.SetActive(false);
    }

    /*
     * Hides all health bars from main screen 
     */
    static void hideBars()
    {
        gradeBar.enabled = false;
        socialBar.enabled = false;
        healthBar.enabled = false;
        moneyBar.enabled = false;
    }

    /*
     * Shows all health bars from main screen 
     */
    void showBars()
    {
        gradeBar.enabled = true;
        socialBar.enabled = true;
        healthBar.enabled = true;
        moneyBar.enabled = true;
    }

    /*
     * Method Initializes the Game. Waits for user to press space bar
     * to initialize the game.
     */
    void InitGame()
    {
        //Find main text, instructions and main image in Unity
        mainText = GameObject.Find("Text").GetComponent<Text>();
        instructions = GameObject.Find("Instructions").GetComponent<Text>();
        main = GameObject.Find("Main");

        //Make Instruction text fade in and out
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, Mathf.Sin(Time.time * 3));

        //Compute when user presses space bar. Initializes game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Hide Main image, and displayed texts
            hideImage(main);
            hideText(mainText);
            hideText(instructions);
            //Create a temporary stacked list to store questions from text file
            stackedLists temp = new stackedLists();
            //Add required features to the list
            temp = getCharacter();
            character = temp.name;
            scenarioText.text = temp.interaction;
            active = temp.Prop1;
            used = temp.Prop2;
            //Load NPC images and show bars
            loadnpc(character);
            showBars();
        }
    }

    /*
     * Method takes in a character name and shows the image for that character.
     * Checks if the player won every time a new image is loaded
     * Keeps teck of the current image on screen
     */
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

    /*
     * Picks a random character
     */
    stackedLists getCharacter()
    {

        returnHolder = pickRandomly(active, used);
        return returnHolder;
    }

    /*
     * Method deals with user input. Whenever a player accepts or declines a scenario
     */
    void Interaction()
    {
        //Decline Scenario
        if (Input.GetKeyDown(KeyCode.N))
        {
            //Play Sound 
            SoundManager.instance.PlaySingle(interactionSound);
            //Damage for health bars
            gradeDamage(returnHolder.no[0]);
            socialDamage(returnHolder.no[1]);
            healthDamage(returnHolder.no[2]);
            moneyDamage(returnHolder.no[3]);
            //Update the stacked List
            stackedLists temp = new stackedLists();
            temp = getCharacter();
            scenarioText.text = temp.interaction;
            character = temp.name;
            active = temp.Prop1;
            used = temp.Prop2;
            //Hide Current Character Image
            hideImage(currentImage);
            //Show New Character Image
            loadnpc(character);
        }

        //Accept Scenario
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            //Play Sound
            SoundManager.instance.PlaySingle(interactionSound);
            //Dame for health bars
            gradeDamage(returnHolder.yes[0]);
            socialDamage(returnHolder.yes[1]);
            healthDamage(returnHolder.yes[2]);
            moneyDamage(returnHolder.yes[3]);
            //Update the stacked list
            stackedLists temp = new stackedLists();
            temp = getCharacter();
            scenarioText.text = temp.interaction;
            character = temp.name;
            active = temp.Prop1;
            used = temp.Prop2;
            //Hide current character image
            hideImage(currentImage);
            //Load new character image
            loadnpc(character);
        }
    }

    /*
     * Method Displays you lose screen when grades reach 0
     * Displays the number of days survived.
     * Stop main sound and play losing sound
     */
    public void youLoseGrades()
    {
        loseGrades.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySingle(loseSound);

    }

    /*
     * Method Displays you lose screen when social reach 0
     * Displays the number of days survived.
     * Stop main sound and play losing sound
     */
    public void youLoseSocial()
    {
        loseSocial.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySingle(loseSound);
    }

    /*
     * Method Displays you lose screen when health reach 0
     * Displays the number of days survived.
     * Stop main sound and play losing sound
     */
    public void youLoseHealth()
    {
        loseHealth.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySingle(loseSound);
    }

    /*
     * Method Displays you lose screen when money reach 0
     * Displays the number of days survived.
     * Stop main sound and play losing sound
     */
    public void youLoseMoney()
    {
        loseMoney.SetActive(true);
        hideBars();
        daysSurvived.text = "You made it to " + days + " days.";
        showText(daysSurvived);
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.PlaySingle(loseSound);
    }

    /*
     * Method Displays you win screen when player reaches 50 days
     * Stop main sound and play winning sound
     */
    void youWin()
    {
        if (days == 50)
        {
            win.SetActive(true);
            hideBars();
            daysSurvived.text = "Congratulations you graduated!";
            showText(daysSurvived);
            SoundManager.instance.musicSource.Stop();
            SoundManager.instance.PlaySingle(winSound);
        }

    }

    /*
     * Update the Number of Days
     */
    void setDays()
    {
        days++;
        dayText.text = "Day " + days;
    }

    /*
     * Picks a Random Character
     * Method deals with the repetition of questions. It stores used questions in one array
     * and the rest in another array. All these questions are read from a text file.
     */
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

    /*
     * Method Damages grade health bar when user accepts or declines a scenario
     */
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

    /*
     * Method Damages social health bar when user accepts or declines a scenario
     */
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

    /*
     * Method Damages health health bar when user accepts or declines a scenario
     */
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

    /*
     * Method Damages money health bar when user accepts or declines a scenario
     */
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

    /*
     * Returns the current health bar value
     */
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

/*
 * StackedList class
 */
public class stackedLists
{
    public List<string> Prop1 { get; set; }
    public List<string> Prop2 { get; set; }

    //StackedList features
    public string name; //Name of the character
    public string interaction; //Scenario
    public List<string> yes { get; set; } // HealthBar decrease/increase if sceanrio is accepted
    public List<string> no { get; set; } // HealthBar decrease/increase if scenario is declined
}


