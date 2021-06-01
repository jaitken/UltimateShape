using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 screenBounds;

    public float defMass = .1f;
    public float launchMass = .05f;

    public float defSpeed = .04f;
    public float launchSpeed = .03f;
    float playerSpeed;

    public float defVelocityCap = 15f;
    public float launchVelocityCap = 15f;
    float velocityCap;

    public float defAngularDrag = 10;
    public float launchAngularDrag = 10;

    public float defDrag = 5;
    public float launchDrag = 0;

    Rigidbody rb;
    public bool bDrawing;
    public float playerOffset = .6f;

    public bool bWallBounced;
    public float maxBounceTime = 10f;
    public float currBounceTimer = 0f;

    public float sampleTime = .1f;
    public float currSampleTimer = 0f;
    public ArrayList motionInput;
    public float launchTimer = 0f;
    public float launchTime = 2f;
    public bool bLaunched = false;

    public Vector3 mouseDownWorldPosition;
    public Vector3 mouseUpWorldPostion;
 

    public Vector3 prevMouseWorldPostion;
    public Vector3 mouseWorldPostion;

    public float drawTimer;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        rb = GetComponent<Rigidbody>();
        velocityCap = defVelocityCap;
        rb.mass = defMass;
        rb.drag = defDrag;
        rb.angularDrag = defAngularDrag;
        playerSpeed = defSpeed;
        motionInput = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left * playerSpeed);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right * playerSpeed);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up * playerSpeed);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down * playerSpeed);


        

        if (Input.GetMouseButtonDown(0) && !bDrawing)
        {
         
            motionInput.Clear();
            bDrawing = true;

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            prevMouseWorldPostion = Camera.main.ScreenToWorldPoint(mousePos);
            currSampleTimer = sampleTime;

        }
        if (Input.GetMouseButtonUp(0))
        {
            
            bDrawing = false;
            bLaunched = true;
            launchTimer = launchTime;

            prevMouseWorldPostion = new Vector3(0, 0, 0);
            mouseWorldPostion = new Vector3(0, 0, 0);

        }


        if (bDrawing)
        {
            drawTimer += Time.deltaTime;

            if (currSampleTimer > 0)
            {
                currSampleTimer -= Time.deltaTime;
            }
            else
            {
                prevMouseWorldPostion = mouseWorldPostion;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 0;
                mouseWorldPostion = Camera.main.ScreenToWorldPoint(mousePos);
                Vector3 input = (mouseWorldPostion - prevMouseWorldPostion);
                rb.AddForce((input)*playerSpeed);

                currSampleTimer = sampleTime;
                Debug.Log(this.GetComponent<Renderer>().material.GetFloat("_Speed"));
            }
        }
        else
        {
            drawTimer = 0;
        }
        
        

        if(bLaunched && launchTimer > 0 && !bDrawing)
        {
            
            launchTimer -= Time.deltaTime;
            rb.drag = launchDrag;
           
        }
        else
        {
            bLaunched = false;
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


    //cap velocity and player within camera bounds
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
                    rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, 0);
                    bounced = true;
                }
                transform.position = new Vector3(screenBounds.x - playerOffset, transform.position.y, 0);
            }
            if (transform.position.x - playerOffset < -screenBounds.x)
            {
                if (rb.velocity.x < 0)
                {
                    rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y, 0);
                    bounced = true;
                }
                transform.position = new Vector3(-screenBounds.x + playerOffset, transform.position.y, 0);
            }

            //cap player in y direction
            if (transform.position.y + playerOffset > screenBounds.y)
            {
                if (rb.velocity.y > 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, 0);
                    bounced = true;
                }
                transform.position = new Vector3(transform.position.x, screenBounds.y - playerOffset, 0);
            }
            if (transform.position.y - playerOffset < -screenBounds.y)
            {
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y, 0);
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

        if (GetComponent<Rigidbody>().velocity.magnitude > velocityCap)
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * velocityCap;
        }


    }
}
