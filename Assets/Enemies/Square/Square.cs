using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
S        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -screenBounds.y*2)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        Debug.Log("Good hit");
        Destroy(this.gameObject);

    }
}
