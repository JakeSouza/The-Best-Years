using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LoadQuestions : MonoBehaviour
{
    string line;    //used for reading in questions
    int i = 0;      //used for counter in loop
    string[,] questions = new string[30,4];     //stores questions; column 0 is character name; 1 is questions; 2 is if yes; 3 is if no

    // Use this for initialization
    void Start ()
    {
        //read in file
        var fileStream = new FileStream(@"C:\Users\Isaac\Documents\Unity Projects\The Best Years\Assets\questions.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream))
        {
            while((line = streamReader.ReadLine()) != null && i < 30)
            {
                for (int j = 0; j <= 3; j++)
                {
                    questions[i, j] = line;
                    line = streamReader.ReadLine();
                }
                i++;
            }
        }
        Console.WriteLine(questions[0,1]);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
