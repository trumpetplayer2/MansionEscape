using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float speed = 5;
    // Update is called once per frame
    void Update()
    {
        //Move left
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
