using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnedPrefab;
    private GameObject instance;
    public int cooldown = 10;
    private int currentCooldown = 0;
    private float nextUpdate = 0f;
    private void Start()
    {
        nextUpdate = Time.time + 1f;
    }

    private void OnBecameVisible()
    {
        if (instance == null && currentCooldown <= 0)
        {
            instance = Instantiate(spawnedPrefab, transform.position, transform.rotation);
            currentCooldown = cooldown;
            nextUpdate = Time.time + 1f;
        }
        
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            if (Time.time > nextUpdate)
            {
                currentCooldown -= 1;
                nextUpdate = Time.time + 1f;
            }
        }
    }
}
