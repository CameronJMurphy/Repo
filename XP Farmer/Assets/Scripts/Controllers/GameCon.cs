using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// \file
/// <summary>
/// \brief Controller of Score and health
/// </summary>
public class GameCon : MonoBehaviour
{
	/// <summary>
	/// Set up variables
	/// </summary>
	private int score; 
	public Text scoreText;
	private int wave;
	private PlayerCon PC; ///<Grab reference to Player Controller

	public Slider healthBar; ///< Grab reference to healthbar slider
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
		///Update UI
		UpdateScoreText();
		displayHealthBar();
	}

	private void UpdateScoreText() ///Updates The score text displayed ingame
	{
		scoreText.text = "Rats Killed: " + score.ToString() + "		Wave: " + wave.ToString();
	}
	
	/// \Param int 
	public void AddToScore(int amount) ///Add amount to current score
	{
		score += amount;
	}

	public void WaveCompleted() /// called as soon as all monsters are killed
	{
		++wave; ///< increase wave int by 1
		PC.healthIncrease(1); ///< Increase players health by 1 this is a reward for finishing the level
	}

	public void displayHealthBar() ///display health on the UI healthbar shown ingame
	{
		if (PC.PlayerHealth() > 0) /// If the players health is greater than 0 (to stop a division by 0)
		{
			healthBar.value = PC.PlayerHealth() / PC.MaxPlayerHealth(); ///health bar's value will equal player current hp divided by there max hp
		}
		else ///else
		{
			healthBar.value = 0; ///health bar will equal zero
		}
		
	}
}
