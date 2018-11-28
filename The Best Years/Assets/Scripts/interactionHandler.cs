using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionHandler: MonoBehaviour {
    // initialize lists to hold interactions
    //non-used
    private List<int> first = new List<int>();
    //used
    private List<int> second = new List<int>();

    //***FOR TESTING***
    // fills list with ints from 0-9 indicating cell in list
    public void quickFill(){
        //fill in first with dummy data
        for (int i = 0; i < 10; i++)
        {
            first.Add(i);
        }
        //print contents in the arrays
        foreach (int x in first)
        {
            Debug.Log("First:" + x);
        }
        foreach (int x in second)
        {
            Debug.Log("Second" + x);
        }
    }
   
    //
    public stackedLists pickRandomly(List<int> one, List<int> two){
        // initialize variables to be used in method
        int selector = 0;
        int temp = 0;
        stackedLists returnHolder = new stackedLists();


        //if more than one interaction left
        if (one.Count > 1)
        {
            //pick random cell in list
            selector = Random.Range(0, (one.Count - 1));
            temp = one[selector];
            //add it to the used list and remove from active list
            two.Add(temp);
            one.RemoveAt(selector);
            foreach (int x in first)
            {
                Debug.Log("First: " + x);
            }
            foreach (int y in second)
            {
                Debug.Log("Second: " + y);
            }
            return new stackedLists { Prop1 = one, Prop2 = two };
        }
        //if only one interaction left
        else
        {
            temp = one[0];
            two.Add(temp);
            one.RemoveAt(selector);
            //run isEmpty method, which refills the active list and empties used list
            returnHolder = isEmpty(one, two);
            one = returnHolder.Prop1;
            two = returnHolder.Prop2;
            //return edited lists
            foreach (int x in first)
            {
                Debug.Log("First: " + x);
            }
            foreach (int x in second)
            {
                Debug.Log("Second: " + x);
            }
            return new stackedLists { Prop1 = one, Prop2 = two };
        }
    }

    //checks if the active array is empty, if yes refill it and empty used array
    public stackedLists isEmpty(List<int> one, List<int> two){
        if (one.Count == 0)
        {
            foreach(int n in two){
                one.Add(n);
            }
            for(int i = 0; i < two.Count - 1; i++)
            {
                two.RemoveAt(0);
            }
        }
        foreach (int x in first)
        {
            Debug.Log("First: " + x);
        }
        foreach (int x in second)
        {
            Debug.Log("Second: " + x);
        }
        return new stackedLists { Prop1 = one, Prop2 = two };
    }
    // Use this for initialization
    void Start () {
        quickFill();
    }

    // Update is called once per frame
    void Update()
    {
        stackedLists hold = new stackedLists();
        if (Input.GetKeyDown(KeyCode.A))
        {
            hold = pickRandomly(first, second);
            first = hold.Prop1;
            second = hold.Prop2;
        }
    }
}

//Object to allow returning of more than one list at a time
public class stackedLists {
    public List<int> Prop1 { get; set; }
    public List<int> Prop2 { get; set; }
}