using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public int facing = 1;
    public float speed = 8;
    public SpriteRenderer sprite;
    // Update is called once per frame
    void Update()
    {
        sprite.flipX = !(facing > 0);
        transform.Translate(Vector3.left * speed * Time.deltaTime * facing);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().trip();
            Destroy(this.gameObject);
        }
        else if(collision.tag == "Ghost" || collision.tag == "Bullet")
        {
            Destroy(this.gameObject);
        }
        else
        {
            facing = -facing;
            sprite.flipX = !sprite.flipX;
        }
    }
}
