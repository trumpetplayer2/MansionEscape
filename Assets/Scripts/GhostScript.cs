using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    public float speed = 2;
    private float currentSpeed;

    private void Start()
    {
        currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }

    public void updateCurrentSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }
}
