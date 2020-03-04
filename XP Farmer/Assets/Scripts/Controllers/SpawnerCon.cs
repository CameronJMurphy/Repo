 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
/// <summary>
/// This controllers all the spawner in the scene
/// </summary>
public class SpawnerCon : MonoBehaviour
{
    /// <summary>
    /// Setup variables
    /// </summary>
    private List<IEnemySpawner> spawners;
    private bool spawning;

    public delegate bool AreWeAllDead(); /// links to Enemy Con
    public static AreWeAllDead areWeAllDead;

    public delegate void WaveCompleted(); /// links to game con
    public static WaveCompleted waveCompleted;

    [SerializeField]private float timer;
    private float timerMax;
    private bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        ///fill variables
        EnemyCon.nextWaveCheck += NextWaveCheck;
        spawners = new List<IEnemySpawner>(FindObjectsOfType<IEnemySpawner>()); ///< find all the spawners
        spawning = true;
        timerMax = timer;
        startTimer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer == true) ///if the startTimer is true
        {
            timer -= Time.deltaTime; ///start countdown to start spawning
        }
        if (spawning && timer < 0) /// if spawning and 
        {
            startTimer = false; ///reset timers
            timer = timerMax;
            foreach (var spawner in spawners) ///spawn
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
        if (areWeAllDead() && startTimer == false) /// if all enemies are dead, start the next wave
        {
            startTimer = true;
            spawning = true;
            waveCompleted(); /// increment the wave number
            foreach (var spawner in spawners)
            {
                spawner.AddAmount(1); /// increment all the spawners to spawn one more monster than before
            }
        }
    }
   

}
