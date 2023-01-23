using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public bool isSwitched = false;
    public bool flipY = false;
    public SpriteRenderer sprite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Player")) { return; }
        if (!isSwitched)
        {
            isSwitched = !isSwitched;
            if (flipY)
            {
                sprite.flipY = !sprite.flipY;
            }
            else
            {
                sprite.flipX = !sprite.flipX;
            }
        }
    }
}
