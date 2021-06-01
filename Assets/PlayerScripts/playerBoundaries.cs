using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBoundaries : MonoBehaviour
{
    private Vector2 screenBounds;
    Rigidbody rb;
    public bool wallBounced;


    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        //cap player in x direction
        if(transform.position.x + .5 > screenBounds.x)
        {
            Debug.Log("RIGHT");
            if(rb.velocity.x > 0)
            {
                rb.AddForce(Vector3.left);
            }
        }
        if (transform.position.x -.5 < screenBounds.x * -1)
        {
            Debug.Log("LEFT");
            if (rb.velocity.x < 0)
            {
                rb.AddForce(Vector3.right);
            }
        }

        //cap player in y direction
        if (transform.position.y + .5 > screenBounds.y)
        {
            Debug.Log("UP");
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down);
            }
        }
        if (transform.position.y - .5 < screenBounds.y * -1)
        {
            Debug.Log("DOWN");
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.up);
            }
        }
    }
}
