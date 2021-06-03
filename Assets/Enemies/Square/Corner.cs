using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner : MonoBehaviour
{
    public float launchPower = 1000f;
    public Transform parent;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        rb = parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Bad hit");

        Rigidbody2D playerRb = col.gameObject.GetComponent<Rigidbody2D>();
        Vector3 force = Vector3.Normalize(transform.position - col.gameObject.transform.position);
        playerRb.AddForce(-force * launchPower);

    }
}
