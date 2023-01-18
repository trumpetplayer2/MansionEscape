using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If object is not a player
        if (!collision.gameObject.tag.Contains("Player")) { return; }
        //Reset
        GameManager.instance.Reset();
    }
}
