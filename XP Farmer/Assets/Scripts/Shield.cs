using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private PlayerCon PC;
    private float timer;
    [SerializeField] private float shieldTime;
    [SerializeField] private float shieldCooldown;
    private bool shieldReady;

    // Start is called before the first frame update
    void Start()
    {
        PC = FindObjectOfType<PlayerCon>();
        shieldReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        //set the player to normal with there weapon
        PC.weapon.gameObject.SetActive(true);
        PC.SetDefending(false);
        GetComponent<SpriteRenderer>().color = Color.white;

        if (shieldReady)
        {   
            if (Input.GetMouseButton(1))//right mouse button, then shield 
            {
                timer += Time.deltaTime;
                PC.SetDefending(true);
                PC.weapon.gameObject.SetActive(false);
                GetComponent<SpriteRenderer>().color = new Vector4(timer / shieldTime, 1, 1, 1); //cyan with a fade to white
                

            }
            if (timer > shieldTime) // let the player have the shield for a duration
            {
                shieldReady = false;
                timer = 0; // reset timer
            }
        }
        else //if the players shield goes on cooldown
        {
            timer += Time.deltaTime;
            if(timer >shieldCooldown )
            {
                shieldReady = true;
                timer = 0; //reset timer
            }
        }

    }
}
