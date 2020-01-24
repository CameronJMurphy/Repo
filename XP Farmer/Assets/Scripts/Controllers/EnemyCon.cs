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
		CheckIfAlive();
	}

	private void CheckIfAlive()
	{
		foreach (IEnemy enemy in enemies)
		{
			if (!enemy.IsAlive())
			{
				enemy.gameObject.SetActive(false);
				enemies.Remove(enemy);
				FindObjectOfType<GameCon>().AddToScore(enemy.scoreValue);
				nextWaveCheck();
				break;
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
			enemy.Move(player.transform.position);
		}
	}
	public void AddToEnemies(IEnemy enemy)
	{
		enemies.Add(enemy);
	}

}
