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
    public float baseSpeed;
    public float movementSpeed;
    public Rigidbody2D Player;
    public float jumpPower = 8f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public bool isOnGround;
    private float inX = 0f;
    public AudioClip jumpSound;
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip tripSound;
    public AudioSource jumpSource;
    public AudioSource shootSource;
    public AudioSource damageAudioSource;
    public AudioSource tripSource;
    public int range = 10;
    bool reloading = false;
    public SpriteRenderer colorFlash;
    public Color defaultColor;
    public Color flashColor;
    public Color transparentColor;
    private int timeBetweenFlash = 10;
    private int timeSinceLastFlash = 0;
    public float iframes = 3;
    public int health = 3;

    public ParticleSystem muzzleFlash;

    private float nextUpdate = 0f;

    public SpriteRenderer gun;
    public Transform pistolPivot;
    public Transform pistolBarrel;
    public LineRenderer bulletLine;

    public int tripTimer = 3;
    public int tripTime = 0;
    public int tripSpeed = 2;
    public bool isTripped = false;

    private Animator animator;

    private void Start()
    {
        //gameManager = GameManager.instance;
        bulletLine.startColor = transparentColor;
        nextUpdate = Time.time + 1f;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        inX = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(inX));
        if (Input.GetButtonDown("Fire1"))
        {
            if (!reloading)
            {
                shootGun();
            }
        }
        if (isTripped)
        {
            if (Time.time > nextUpdate)
            {
                tripTime -= 1;
                nextUpdate = Time.time + 1f;
            }
            if(tripTime == 0)
            {
                untrip();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale <= 0) { return; }
        if (iframes > 0)
        {
            timeSinceLastFlash -= 1;
            if (colorFlash.color.a > 0.5f && timeSinceLastFlash < 1)
            {
                colorFlash.color = flashColor;
                timeSinceLastFlash = timeBetweenFlash;
            }
            else if (timeSinceLastFlash < 1)
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
            if (!jumpSource.isPlaying)
            {
                jumpSource.PlayOneShot(jumpSound);
            }
        }
        animator.SetBool("OnGround", isOnGround);
        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        pistolPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > 90 || angle <= -90)
        {
            gun.flipY = true;
            colorFlash.flipX = true;
            pistolBarrel.localPosition = new Vector3(0.06f, -0.02f, 0f);
        }
        else
        {
            gun.flipY = false;
            colorFlash.flipX = false;
            pistolBarrel.localPosition = new Vector3(0.06f, 0.02f, 0f);
        }

        Vector2 moveX = new Vector2(inX * movementSpeed, Player.velocity.y);
        Player.velocity = moveX;
        Debug.DrawLine(pistolBarrel.position, mousePos, Color.green);
    }

    public void damagePlayer(int amount, bool ignoreIFrames)
    {
        if (iframes <= 0 || ignoreIFrames)
        {
            if (!damageAudioSource.isPlaying)
            {
                damageAudioSource.PlayOneShot(damageSound);
            }
            health -= amount;
            if (health < 0)
            {
                health = 0;
            }
            iframes = 2;
            if (health > 0)
            {
                GameManager.instance.mainCamera.GetComponent<CameraFollow>().shakeDuration = 0.05f;
            }
        }
        if (health == 0)
        {
            //Player is dead
            Time.timeScale = 0;
            Die();
        }
    }

    private void shootGun()
    {
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - gunPos.x;
        //mousePos.y = mousePos.y - gunPos.y;
        ////Shoot
        //RaycastHit2D hit = Physics2D.Raycast(pistolBarrel.position, mousePos, range);
        effect();
        //if (hit)
        //{
        //    if (hit.collider.gameObject.tag.Equals("Enemy")){
        //        Destroy(hit.collider.gameObject);
        //    }
        //}
    }

    private void effect()
    {
        if(Time.timeScale == 0) { return; }
        if (shootSource.isPlaying) { return; }
        muzzleFlash.time = 0;
        muzzleFlash.Play();
        shootSource.PlayOneShot(shootSound);
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

    public void trip()
    {
        //Play Trip Animation

        //Trip Sound

        if (!tripSource.isPlaying)
        {
            tripSource.PlayOneShot(tripSound);
        }
        //Decrease stats
        nextUpdate = Time.time + 1f;
        movementSpeed = baseSpeed / tripSpeed;
        tripTime = tripTimer;
        isTripped = true;
    }

    public void untrip()
    {
        movementSpeed = baseSpeed;
        isTripped = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Fire1") || Input.GetButton("Vertical"))
        {
            if(collision.gameObject.tag == "Door")
            {
                //Finish Level
                GameManager.instance.finishLevel();
            }
        }
    }
}
