using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public int dimensionID;
    bool onGoal;
    public Color defaultColor;
    public Color finishColor;
    public AudioClip activate;
    public ParticleSystem particles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If its not a player, idc what touched it
        if(collision.gameObject.tag != "Player") { return;}
        //If its not on the same layer, its the wrong player
        if(collision.gameObject.layer != this.gameObject.layer) { return; }
        //Change the parents color to indicate its been touched
        if(this.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>() == null) { Debug.Log("Sprite renderer not found for " + this.gameObject.transform.parent.gameObject.name); return; }
        this.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = finishColor;
        onGoal = true;
        if (this.gameObject.GetComponent<AudioSource>() != null)
        {
            if (!this.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                this.gameObject.GetComponent<AudioSource>().pitch = 3;
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(activate);
            }
        }
        particles.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If its not a player, idc what touched it
        if (collision.gameObject.tag != "Player") { return; }
        //If its not on the same layer, its the wrong player
        if (collision.gameObject.layer != this.gameObject.layer) { return; }
        if (this.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>() == null) { Debug.Log("Sprite renderer not found for " + this.gameObject.gameObject.name); return; }
        this.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
        onGoal = false;
        if (this.gameObject.GetComponent<AudioSource>() != null)
        {
            if (!this.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                this.gameObject.GetComponent<AudioSource>().pitch = 1;
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(activate);
            }
        }
        particles.Stop();
    }

    public bool getOnGoal()
    {
        return onGoal;
    }
}
