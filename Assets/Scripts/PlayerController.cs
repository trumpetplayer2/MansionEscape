using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public int playerSide;
    public Rigidbody2D Player;
    public float jumpPower = 8f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isOnGround;
    private float inX = 0f;
    GameManager gameManager;
    public AudioClip jumpSound;
    public float reloadSpeed = 1.0f;
    public int roundsLeft = 8;
    public int roundsPerClip = 8;
    bool reloading = false;


    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        inX = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if(Time.timeScale <= 0) { return; }
        if (gameManager.getSideID() != playerSide) { return; }
        //Jump
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetButton("Jump") && isOnGround)
        {
            Player.velocity = new Vector2(Player.velocity.x, jumpPower);
            //Get Player audio source and play sound
            if(this.gameObject.GetComponent<AudioSource>() != null)
            {
                if (!this.gameObject.GetComponent<AudioSource>().isPlaying) { 
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(jumpSound);
                }
            }
        }

        if (Input.GetButtonDown("Shoot"))
        {
            if (!reloading)
            {
                shootGun();
            }
        }

        Vector2 moveX = new Vector2(inX * movementSpeed, Player.velocity.y);
        Player.velocity = moveX;
    }

    private void shootGun()
    {
        if(roundsLeft > 0)
        {
            //Shoot

        }
        else
        {
            //Reload

        }
    }
}
