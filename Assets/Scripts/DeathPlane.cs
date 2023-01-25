using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    public int damage = 1;
    public bool ignoreIFrames = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        //If object is not a player
        if (!collision.gameObject.tag.Contains("Player")) { return; }
        //Damage the player
        GameManager.instance.player.damagePlayer(damage, ignoreIFrames);
    }
}
