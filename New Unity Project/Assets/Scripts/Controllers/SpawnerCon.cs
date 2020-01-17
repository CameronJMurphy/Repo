using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCon : MonoBehaviour
{
    private List<IEnemySpawner> spawners;
    private bool spawning;
    //public EnemyCon enemyCon;

    public delegate bool AreWeAllDead(); // links to Enemy Con
    public static AreWeAllDead areWeAllDead;

    public delegate void WaveCompleted(); //links to game con
    public static WaveCompleted waveCompleted;

    [SerializeField]private float timer;
    private float timerMax;
    private bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        EnemyCon.nextWaveCheck += NextWaveCheck;
        spawners = new List<IEnemySpawner>(FindObjectsOfType<IEnemySpawner>());
        spawning = true;
        timerMax = timer;
        startTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer == true)
        {
            timer -= Time.deltaTime; //start countdown to start spawning
        }
        if (spawning && timer < 0)
        {
            startTimer = false; //reset timers
            timer = timerMax;
            foreach (var spawner in spawners) //spawn
            {
                for(int i = 0; i < spawner.SpawnAmount(); ++i)
                {
                    spawner.Spawn();
                } 
            }
            spawning = false;
        }


    }

    public void NextWaveCheck()
    {
        if (areWeAllDead() && startTimer == false) // restarts the next wave
        {
            startTimer = true;
            spawning = true;
            waveCompleted(); //find this in game con. It increments the wave number
            foreach (var spawner in spawners)
            {
                spawner.AddAmount(1); //increment all the spawners to spawn one more monster than before
            }
        }
    }
   

}
