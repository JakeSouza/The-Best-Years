using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private Text mainText;
    private Image main;
    public List<Image> npc;
    private List<string> characters = new List<string>(new string[] {"Allison", "Kayla", "Leo", "Leonard", "Logan", "Mykailah", "Paul", "ProfCates", "ProfSanchez" });
    [HideInInspector] public bool playersTurn = true;
    private string character;

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
    }
	
	// Update is called once per frame
	void Update () {

        InitGame();

    }

    void hideImage(Image image)
    {
        image.enabled = false;
    }

    void hideText( Text text)
    {
        text.enabled = false;
    }

    void InitGame()
    {
        mainText = GameObject.Find("Text").GetComponent<Text>();
        main = GameObject.Find("Main").GetComponent<Image>();
        mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, Mathf.Sin(Time.time * 3));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            hideImage(main);
            hideText(mainText);
        }
        character = getCharacter();
        loadnpc(character);
    }
    void loadnpc(string character)
    {

        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].tag == character)
            {
                npc[i].enabled = true;
            }
        }
    }

    string getCharacter()
    {
        System.Random rnd = new System.Random();
        int number = rnd.Next();
        return characters[number];
    }
}
