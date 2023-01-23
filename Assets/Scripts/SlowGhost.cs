using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGhost : MonoBehaviour
{
    public float slowBy = 2f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Ghost") { return; }
        GhostScript ghostScript = collision.gameObject.GetComponent<GhostScript>();
        ghostScript.updateCurrentSpeed(ghostScript.speed/slowBy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Ghost") { return; }
        GhostScript ghostScript = collision.gameObject.GetComponent<GhostScript>();
        ghostScript.updateCurrentSpeed(ghostScript.speed);
    }
}
