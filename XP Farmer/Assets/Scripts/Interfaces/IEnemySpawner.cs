using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
public class IEnemySpawner : MonoBehaviour
{
	EnemyCon enemyCon;
	public IEnemy enemy;
	[SerializeField]private int amount; ///< amount of enemies to be spawned from this spawner
	private void Start()
	{
		enemyCon = FindObjectOfType<EnemyCon>();
	}
	public void Spawn() 
	{
		IEnemy creature = Instantiate(enemy, transform.position + new Vector3(Random.Range(-4, 4) , 0, Random.Range(-4, 4)), Quaternion.identity); ///instantiate enemy
		enemyCon.AddToEnemies(creature); ///add this enemy to the enemyCon enemies list
	}

	public void AddAmount(int num) /// \param int num
	{
		amount += num; /// add num to amount var
	}

	public int SpawnAmount() /// \return int return amount
	{
		return amount;
	}
}
