using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCon : MonoBehaviour
{
	private List<IEnemy> enemies;
	private PlayerCon player;
	public IEnemy rat;

	public delegate void NextWaveCheck(); //links to spawner con
	public static NextWaveCheck nextWaveCheck;


	// Start is called before the first frame update
	void Start()
    {
		enemies = new List<IEnemy>();
		player = FindObjectOfType<PlayerCon>();

		SpawnerCon.areWeAllDead += AreWeAllDead;
    }

    // Update is called once per frame
    void Update()
    {
		Movement();
		CheckForImageFlip();
		CheckIfAlive();
	}

	private void CheckIfAlive()
	{
		foreach (IEnemy enemy in enemies)
		{
			enemy.IsAlive(); // this will set the dying status on your rat if its below 0 hp
			if (enemy.IsDead())
			{
				FindObjectOfType<GameCon>().AddToScore(enemy.scoreValue);
				enemies.Remove(enemy);
				enemy.gameObject.SetActive(false);
				nextWaveCheck();
				break;
			}
			else 
			{
				enemy.DeathCheck(); //check to see if they need to start there death animations
			}
			
		}
	}
	public bool AreWeAllDead()
	{
		foreach (var enemy in enemies)
		{
			if(enemy != null)
			{
				return false;
			}
		}
		return true;
	}

	private void Movement()
	{
		foreach (IEnemy enemy in enemies)
		{
			if (!enemy.IsDying()) //if the monster isnt dying
			{
				enemy.Move(player.transform.position); //chase the player
			}
		}
	}
	public void AddToEnemies(IEnemy enemy)
	{
		enemies.Add(enemy);
	}

	private void CheckForImageFlip()
	{
		foreach (var enemy in enemies)
		{
			enemy.FlipImage(player.transform.position);
		}
	}

	
}
