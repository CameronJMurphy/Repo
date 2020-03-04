using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
public class Shield : MonoBehaviour
{
    private PlayerCon PC;
    private float timer;
    [SerializeField] private float shieldTime;
    [SerializeField] private float shieldCooldown;
    private bool shieldReady;
    [SerializeField] private Joybutton shieldButton;

    // Start is called before the first frame update
    void Start()
    {
        PC = FindObjectOfType<PlayerCon>();
        shieldReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        ///set the player to not defending
        PC.SetDefending(false);
        GetComponent<SpriteRenderer>().color = Color.white; ///set the player shade to white

        if (shieldReady) ///if the shield is off cooldown
        {
#if UNITY_STANDALONE_WIN
            ///on computers only
            if (Input.GetMouseButton(1))//right mouse button, then shield /// if the right mouse button is pressed 
            {
                ///increase the timer the shows how long you have used the shield
                timer += Time.deltaTime;
                PC.SetDefending(true); ///set the player defending status to true
                PC.weapon.gameObject.SetActive(false); ///set the players weapon to not active so they cant kill and shield at the same time
                GetComponent<SpriteRenderer>().color = new Vector4(timer / shieldTime, 1, 1, 1); ///make the shield have a decaying colour effect from cyan to white
                

            }
#endif
#if UNITY_ANDROID
            ///for android only
            if (shieldButton.Pressed)///shield button is pressed 
            {
                ///increase the timer the shows how long you have used the shield
                timer += Time.deltaTime;
                PC.SetDefending(true);///set the player defending status to true
                PC.weapon.gameObject.SetActive(false);///set the players weapon to not active so they cant kill and shield at the same time
                GetComponent<SpriteRenderer>().color = new Vector4(timer / shieldTime, 1, 1, 1); ///make the shield have a decaying colour effect from cyan to white
            }
#endif
            ///for all platforms
            if (timer > shieldTime) ///if the timer is geater than our shield time variable
            {
                shieldReady = false; ///sheildReady is set to false
                timer = 0; /// reset timer
            }
        }
        else ///else if the players shield goes on cooldown
        {
            ///timer increases
            timer += Time.deltaTime;
            if(timer >shieldCooldown ) ///if the timer is greater than the shield cooldown
            {
                ///shieldReady is set to true and timer is reset
                shieldReady = true;
                timer = 0; //reset timer
            }
        }

    }
}
