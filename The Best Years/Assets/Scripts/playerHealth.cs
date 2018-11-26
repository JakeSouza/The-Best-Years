using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class playerHealth : MonoBehaviour {

    public float gradeHealth { get; set; }
    public float socialHealth { get; set; }
    public float healthHealth { get; set; }
    public float moneyHealth { get; set; }
    public float maxHealth { get; set; }
    public float startHealth { get; set; }
    public Slider gradeBar;
    public Slider socialBar;
    public Slider healthBar;
    public Slider moneyBar;

    // Use this for initialization
    void Start () {
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
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Z)){
            gradeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            socialDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            healthDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            moneyDamage(10);
        }
    }

    void gradeDamage(float damageValue){
        // deduct damage
        gradeHealth -= damageValue;
        gradeBar.value = calculateGradeHealth();

        // if no health, you lose
        if (gradeHealth <= 0)
        {
            youLose();
        }
    }

    void socialDamage(float damageValue)
    {
        // deduct damage
        socialHealth -= damageValue;
        socialBar.value = calculateSocialHealth();

        // if no health, you lose
        if (socialHealth <= 0)
        {
            youLose();
        }
    }

    void healthDamage(float damageValue)
    {
        // deduct damage
        healthHealth -= damageValue;
        healthBar.value = calculateHealthHealth();

        // if no health, you lose
        if (healthHealth <= 0)
        {
            youLose();
        }
    }

    void moneyDamage(float damageValue)
    {
        // deduct damage
        moneyHealth -= damageValue;
        moneyBar.value = calculateMoneyHealth();

        // if no health, you lose
        if (moneyHealth <= 0)
        {
            youLose();
        }
    }

    float calculateGradeHealth(){
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

    void youLose(){

        Debug.Log("You Lose");
    }
}
