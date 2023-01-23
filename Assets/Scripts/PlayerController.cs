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
    public Rigidbody2D Player;
    public float jumpPower = 8f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isOnGround;
    private float inX = 0f;
    GameManager gameManager;
    public AudioClip jumpSound;
    public int range = 1000;
    bool reloading = false;
    public SpriteRenderer colorFlash;
    public Color defaultColor;
    public Color flashColor;
    public Color transparentColor;
    private int timeBetweenFlash = 10;
    private int timeSinceLastFlash = 0;
    public float iframes = 3;
    public int health = 3;

    public SpriteRenderer gun;
    public Transform pistolPivot;
    public Transform pistolBarrel;
    public LineRenderer bulletLine;

    private void Start()
    {
        //gameManager = GameManager.instance;
        bulletLine.startColor = transparentColor;
    }

    private void Update()
    {
        inX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Fire1"))
        {
            if (!reloading)
            {
                shootGun();
            }
        }
    }

    private void FixedUpdate()
    {
        if(Time.timeScale <= 0) { return; }
        if(iframes > 0)
        {
            timeSinceLastFlash -= 1;
            if (colorFlash.color.a > 0.5f && timeSinceLastFlash < 1)
            {
                colorFlash.color = flashColor;
                timeSinceLastFlash = timeBetweenFlash;
            }
            else if(timeSinceLastFlash < 1)
            {
                colorFlash.color = defaultColor;
                timeSinceLastFlash = timeBetweenFlash;
            }
        }
        else
        {
            colorFlash.color = defaultColor;
        }
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

        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        pistolPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > 90 || angle <= -90)
        {
            gun.flipY = true;
            pistolBarrel.localPosition = new Vector3(0.06f, -0.02f, 0f);
        }
        else
        {
            gun.flipY = false;
            pistolBarrel.localPosition = new Vector3(0.06f, 0.02f, 0f);
        }

        Vector2 moveX = new Vector2(inX * movementSpeed, Player.velocity.y);
        Player.velocity = moveX;
        Debug.DrawLine(pistolBarrel.position, mousePos, Color.green);
    }

    public void damagePlayer()
    {
        if (iframes <= 0)
        {
            health -= 1;
            iframes = 2;
            if (health > 0)
            {
                GameManager.instance.mainCamera.GetComponent<CameraFollow>().shakeDuration = 0.05f;
            }
        }
        if(health == 0)
        {
            //Player is dead
            Time.timeScale = 0;
            Die();
        }
    }

    private void shootGun()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        //Shoot
        RaycastHit2D hit = Physics2D.Raycast(pistolBarrel.position, mousePos, range);
        effect();
        if (hit)
        {
            if (hit.collider.gameObject.tag.Equals("Enemy")){
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private void effect()
    {
        Instantiate(bulletLine, pistolBarrel.position, pistolPivot.rotation);
    }

    public void updateIFrames()
    {
        if(iframes > 0)
        {
            iframes -= 1;
        }
    }

    public void Die()
    {
        //Play death animation if time

        GameManager.instance.Death();
    }
}
