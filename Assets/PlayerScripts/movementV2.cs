using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementV2 : MonoBehaviour
{
    private Vector2 screenBounds;
    Rigidbody2D rb;

    public float defSpeed = .04f;
    public float launchSpeed = .03f;
    float playerSpeed;
    public float defVelocityCap = 15f;
    public float launchVelocityCap = 15f;
    float velocityCap;
    public float defMass = .1f;
    public float launchMass = .05f;
    public float defDrag = .25f;
    public float launchDrag = 0;
    public float defAngularDrag = 0;
    public float launchAngularDrag = 0;

    public bool bWallBounced;
    public float maxBounceTime = 10f;
    public float currBounceTimer = 0f;
    public float wallSpeedPenalty;

    public bool bDrawing;
    public float playerOffset = .6f;

    public Vector3 mouseDownWorldPosition;
    public Vector3 mouseUpWorldPostion;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        rb = GetComponent<Rigidbody2D>();
        rb.mass = defMass;
        rb.drag = defDrag;
        rb.angularDrag = defAngularDrag;
        
        playerSpeed = defSpeed;
        velocityCap = defVelocityCap;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !bDrawing)
        {

            bDrawing = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            mouseDownWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            rb.drag = launchDrag;

        }

        if (Input.GetMouseButtonUp(0))
        {
            bDrawing = false;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            mouseUpWorldPostion = Camera.main.ScreenToWorldPoint(mousePos);
            rb.AddForce((mouseUpWorldPostion - mouseDownWorldPosition)*playerSpeed);

            rb.drag = defDrag;

        }

        //countdown bounce timer if the player wallbounced
        if (currBounceTimer > 0)
        {
            currBounceTimer -= Time.deltaTime;
        }
        else
        {
            bWallBounced = false;
        }
    }

    void LateUpdate()
    {
        if (!bWallBounced)
        {
            bool bounced = false;
            //cap player in x direction
            if (transform.position.x + playerOffset > screenBounds.x)
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, 0) * wallSpeedPenalty;
                    bounced = true;
                }
                transform.position = new Vector3(screenBounds.x - playerOffset, transform.position.y, 0);
            }
            if (transform.position.x - playerOffset < -screenBounds.x)
            {
                if (rb.velocity.x < 0)
                {
                    rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, 0) * wallSpeedPenalty; 
                    bounced = true;
                }
                transform.position = new Vector3(-screenBounds.x + playerOffset, transform.position.y, 0);
            }

            //cap player in y direction
            if (transform.position.y + playerOffset > screenBounds.y)
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, 0) * wallSpeedPenalty;
                    bounced = true;
                }
                transform.position = new Vector3(transform.position.x, screenBounds.y - playerOffset, 0);
            }
            if (transform.position.y - playerOffset < -screenBounds.y)
            {
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, 0) * wallSpeedPenalty;
                    bounced = true;
                }
                transform.position = new Vector3(transform.position.x, -screenBounds.y + playerOffset, 0);

            }

            if (bounced)
            {
                currBounceTimer = maxBounceTime;
                bWallBounced = true;
            }
        }

        if (rb.velocity.magnitude > velocityCap)
        {
            Debug.Log("capped");
            rb.velocity = rb.velocity.normalized * velocityCap;
        }


    }
}
