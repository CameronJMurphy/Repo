using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCon : MonoBehaviour
{
	private int score;
	public Text scoreText;
	private int wave;
	private PlayerCon PC;

	public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
		PC = FindObjectOfType<PlayerCon>();
		SpawnerCon.waveCompleted += WaveCompleted;
		wave = 1;
    }

    // Update is called once per frame
    void Update()
    {
		UpdateScoreText();
		displayHealthBar();
	}

	private void UpdateScoreText()
	{
		scoreText.text = "Rats Killed: " + score.ToString() + "		Wave: " + wave.ToString();
	}

	public void AddToScore(int amount)
	{
		score += amount;
	}

	//public void NewWave()//start just before the next wave
	//{
	//	float timer = 10000;
	//	while(timer > 0) //creates a pause between waves
	//	{
	//		timer -= Time.deltaTime;
	//	}
	//	Time.timeScale = 1f; // reset time speed to normal
	//}

	public void WaveCompleted() // called as soon as all monsters are killed
	{
		//Time.timeScale = 0.25f; // slow down time
		//NewWave();
		++wave;
		PC.healthIncrease(1);
	}

	public void displayHealthBar()
	{
		if (PC.PlayerHealth() > 0) //stop stop a division by 0
		{
			healthBar.value = PC.PlayerHealth() / PC.MaxPlayerHealth();
		}
		else
		{
			healthBar.value = 0;
		}
		
	}
}
