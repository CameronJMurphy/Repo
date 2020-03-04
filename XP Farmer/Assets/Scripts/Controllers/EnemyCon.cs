using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
/// <summary>
/// \brief Controlles all the behaviours assiociated with enemy units
/// </summary>
public class EnemyCon : MonoBehaviour
{
	private List<IEnemy> enemies; ///< Create list of enemies
	private PlayerCon player; ///<  Get Reference to player
	public IEnemy rat;///<  Get reference to rat

	public delegate void NextWaveCheck(); ///< links to spawner con
	public static NextWaveCheck nextWaveCheck; ///< links to spawner con


	/// Start is called before the first frame update
	void Start()
    {
		///instanciate enemies and grab references to player
		enemies = new List<IEnemy>(); 
		player = FindObjectOfType<PlayerCon>();
		/// Set up delegate
		SpawnerCon.areWeAllDead += AreWeAllDead;
    }

    // Update is called once per frame
    void Update()
    {
		/// Movement
		Movement();
		/// check to see if this unit needs its image flipped
		CheckForImageFlip();
		/// Check if the unit is still alive
		CheckIfAlive();
	}

	private void CheckIfAlive() 
	{
		foreach (IEnemy enemy in enemies) /// foreach enemy
		{
			enemy.IsAlive(); /// check to see if this unit is alive
			if (enemy.IsDead()) ///if this unit isn't alive
			{
				FindObjectOfType<GameCon>().AddToScore(enemy.scoreValue); ///Add this units score to the players score
				enemies.Remove(enemy); /// Remove this unit from our list of enemies
				enemy.gameObject.SetActive(false); ///Set this game object to not active
				nextWaveCheck(); /// check if that was the last enemy of that wave
				break;
			}
			else ///else
			{
				enemy.DeathCheck(); ///check to see if they need to start there death animations
			}
			
		}
	}
	public bool AreWeAllDead() /// \return bool \param none
	{
		foreach (var enemy in enemies)///foreach enemy
		{
			if(enemy != null) /// if an enemy isnt dead
			{
				return false; ///return false
			}
		}
		return true; ///if all enemies are dead then return true
	}

	private void Movement() 
	{
		foreach (IEnemy enemy in enemies) ///foreach enemy 
		{
			if (!enemy.IsDying()) ///if the monster isnt dying (dying is a state between dead and alive)
			{
				enemy.Move(player.transform.position); ///chase the player
			}
		}
	}
	public void AddToEnemies(IEnemy enemy) /// \return void \param IEnemy
	{
		enemies.Add(enemy); ///add new enemy to enemies list
	}

	private void CheckForImageFlip()
	{
		foreach (var enemy in enemies) ///foreach enemy
		{
			enemy.FlipImage(player.transform.position); ///check to see if I need to flip this units image
		}
	}

	
}
