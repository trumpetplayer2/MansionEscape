using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public AudioClip killEnemy;
    public int moveSpeed = 170;
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, 1f);
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Ignore the door
        if (collision.tag.Equals("Door") || collision.tag.Equals("Shootable") || collision.tag.Equals("Ghost") || collision.tag.Equals("Light")) { return; }
        //Spawn hit particle

        //Kill Enemy
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        //Despawn
        Destroy(this.gameObject);
    }
}
