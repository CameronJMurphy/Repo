using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemySpawner : MonoBehaviour
{
	EnemyCon enemyCon;
	public IEnemy enemy;
	[SerializeField]private int amount;

	private void Start()
	{
		enemyCon = FindObjectOfType<EnemyCon>();
	}
	public void Spawn()
	{
		IEnemy creature = Instantiate(enemy, transform.position + new Vector3(Random.Range(-4, 4) , 0, Random.Range(-4, 4)), Quaternion.identity);
		enemyCon.AddToEnemies(creature);
	}

	public void AddAmount(int num)
	{
		amount += num;
	}

	public int SpawnAmount()
	{
		return amount;
	}
}
