/*
 * * * Movement and Jump Feel & Guidelines * * *
 * 
 * 1. Standard movement -> left and right by A & D keys
 *  - Movement needs to be "tight" and "snappy"
 *  - Movement stops when key is released with a slight 'last step' or 'slide' in either last pressed direction
 * 
 * 2. Jump -> space key {DONE}
 *  - Jump moves the character up (duh)
 *  
 *  - Simulate gravity but with 'tricks' {DONE}
 *      - Speed decreases at the apex of the jump
 *      - Speed increases back towards the ground after the apex of the jump
 *          - Fall Speed
 *          
 *  - Jump variation {DONE}
 *      - Length of key press determines height of jump
 *          - The longer the jump key is held, the bigger the jump
 *          - The shorter the jump key is held, the smaller the jump
 *              - Variable jump height
 *              
 *  - Jump buffering 
 *      - Not necessary for the theme of only jumping once, but could be cool to include as a mechanic later on
 *      - Queue another jump while jumping for a 'snappier' feel of jumping
 *      
 *  - Coyote Jump {DONE}
 *      - Being able to jump after leaving a platform for a few miliseconds
 *      - Need a 'grounded' threshold to track when the player can jump
 *  
 *  - Edge Detection
 *      - If jumping underneath an edge, nudges the player slightly into the direction they intended to go
 *      - Works for ledge catching to provide some leeway for getting 'up'
 *      
 * 
 * * * Other Parameters * * *
 * 
 * 1. You Can Only Jump Once
 *  - Jumping a second time kills player
 *  
 * 2. You Can Only Dash Once
 *  - Dashing a second time kills player
 *  - Dashes in the direction of the cursor on screen
 *  
 * 3. You Can Only Shoot Once
 *  - Shooting a second time kills player
 *  - Sends a projectile in the direction of the cursor on screen
 *  - Can be used to tigger events
 *  
 * 4. You Can Only Die Once
 *  - When unlocked, allows the player to die an additional time
 *  - Upon death, other mechanics (jump, dash, shoot) are reset and allow the player to do them a 'second' time
 *  
 * 5. You Can Only Wall Jump Once
 * 6. You Can Only Get Hit Once
 * 7. You Can Only Stop Time Once
 * 8. You Can Only Slide Once
 *  
 */



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference for the RigidBody2D -> Drag the component into the reference to establish a line of communication
    public Rigidbody2D myRigidBody;

    // Player Variables
    private float moveHorizontal;
    private float moveSpeed = 10;

    // Sprite Bool
    private bool facingRight = true;

    // Jump Variables
    [Header("Jump Variables")]
    public float jumpStrength;
    public float wallJumpStrength;

    // Original Variable Jump
    /*
    private float jumpTimeCounter;
    public float jumpTime;
    */

    // Variable Jump
    private float fallSpeed = 5f;
    private float lowJumpSpeed = 5f;

    // Apex of Jump - Better Control
    private float jumpApexThreshold = 10f;
    private float apexPoint;
    private float minFallSpeed = 5f;
    private float maxFallSpeed = 15f;

    // Coyote Time Variables
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    // Position Variables
    [Header("Position Variables")]
    public bool isGrounded; // True when on the ground
    public bool wallTouch; // True when touching the wall
    //public bool isJumping; // True while in the air

    // Action Variables
    [Header("Action Variables")]
    public bool canJump; // Start with one jump
    public bool canWallJump; // True when on the wall
    public bool wallJumped; // True when jumping from the wall

    public bool canDash; // Start with one dash
    public bool canShoot; // Start with one bullet
    public bool canDie; // Start with one life
    public bool canGetHit; // Start with one hit


    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        canWallJump = false;
        wallJumped = false;

        canDash = false;
        canShoot = false;
        canDie = false;
        canGetHit = false;

        isGrounded = false;
        wallTouch = false;
        //isJumping = false;
    }

    // Fixed Update runs at the same framerate as the physics engine
    void FixedUpdate()
    {
        // Gets Unity's left and right horizontal input - Axis is a scale of -1 (left) to 1 (right)
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Flipping the sprite left or right based on movement
        if (moveHorizontal > 0 && !facingRight) // Checks if going right and facing left to flip the character
        {
            flip();
        } else if (moveHorizontal < 0 && facingRight) // Checks if going left and facing right to flip the character
        {
            flip();
        }

        // Creates The Vector Variable vel - Vector2 is an X and Y axis
        Vector2 vel = new Vector2();

        // Horizontal Movement Speed - moving left or right
        vel.x = moveHorizontal * moveSpeed;
        // Vertical Movement Speed - like falling
        vel.y = myRigidBody.velocity.y;
        // Sets the velocities for the rigidbody
        myRigidBody.velocity = vel;

        CalculateJumpApex();
    }

    // Update is called once per frame
    void Update()
    {
        // Variable Jump Height + adjustment to jump height (low jump speed) and fall speed
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeed - 1) * Time.deltaTime;
        } else if (myRigidBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            myRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpSpeed - 1) * Time.deltaTime;
        }

        if (isGrounded == true || coyoteTimeCounter > 0)
        {
            // Checks if the jump key is pressed, while also checking if the player can jump
            if (Input.GetKeyDown(KeyCode.Space) == true && canJump == true && wallTouch == false)
            {
                //isJumping = true;
                //jumpTimeCounter = jumpTime;

                myRigidBody.velocity = Vector2.up * jumpStrength;
                Debug.Log("jump key pressed.");
                //canJump = false;
                Debug.Log("Player has jumnped once.");
            }
        }

        // Checks for wall jumping
        if (Input.GetKeyDown(KeyCode.Space) == true && canWallJump == true)
        {
            myRigidBody.velocity = Vector2.up * wallJumpStrength;
            Debug.Log("jump key pressed.");
            canWallJump = false;
            //wallJumped = true;
            Debug.Log("Player has wall jumnped once.");
        }

        // Coyote Time
        if (isGrounded == true)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        /*
        // Checks the jumping and applies variable jump based on key press duration
        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                myRigidBody.velocity = Vector2.up * jumpStrength;
                jumpTimeCounter -= Time.deltaTime;
            } else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
        */
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            wallTouch = true;
            if (wallTouch == true && wallJumped == false)
            {
                canWallJump = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            wallTouch = false;
            if (wallTouch == false || wallJumped == true)
            {
                canWallJump = false;
            }
        }
    }

    private void CalculateJumpApex()
    {
        if (!isGrounded)
        {
            // Gets stronger the closer to the top of the jump
            apexPoint = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(myRigidBody.velocity.y));
            fallSpeed = Mathf.Lerp(minFallSpeed, maxFallSpeed, apexPoint);
        }
        else
        {
            apexPoint = 0;
        }
    }

    void flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        // Positive 1 faces right, Negative 1 faces left
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
}
