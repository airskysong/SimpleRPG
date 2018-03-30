using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace MyRPG.Characters
{
[RequireComponent(typeof(RawImage))]
public class HealthBar : MonoBehaviour
{

    RawImage healthBar;
    // Use this for initialization
    GameObject player;
    Player playerState;

    void Start()
    {
        healthBar = GetComponent<RawImage>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("can't find Player");
        }
        else
        {
            playerState = player.GetComponent<Player>();
            playerState.OnHealthChange += OnHealthChange;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnHealthChange(int Health)
    {
        float original = healthBar.uvRect.x;
        float ShowHealth = (50 - Health) * 0.01f;
        healthBar.uvRect = new Rect(ShowHealth, 0f, 1, 1);
    }

}
}
