using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        //If object is not a player
        if (!collision.gameObject.tag.Contains("Player")) { return; }
        //Damage the player
        GameManager.instance.player.damagePlayer();
    }
}
