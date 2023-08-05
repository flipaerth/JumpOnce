/*
 * * * Movement and Jump Feel & Guidelines * * *
 * 
 * 1. Standard movement -> left and right by A & D keys
 *  - Movement needs to be "tight" and "snappy"
 *  - Movement stops when key is released with a slight 'last step' or 'slide' in either last pressed direction
 * 
 * 2. Jump -> space key
 *  - Jump moves the character up (duh)
 *  
 *  - Simulate gravity but with 'tricks'
 *      - Speed decreases at the apex of the jump
 *      - Speed increases back towards the ground after the apex of the jump
 *          - Fall Speed
 *          
 *  - Jump variation
 *      - Length of key press determines height of jump
 *          - The longer the jump key is held, the bigger the jump
 *          - The shorter the jump key is held, the smaller the jump
 *              - Variable jump height
 *              
 *  - Jump buffering
 *      - Not necessary for the theme of only jumping once, but could be cool to include as a mechanic later on
 *      - Queue another jump while jumping for a 'snappier' feel of jumping
 *      
 *  - Coyote Jump
 *      - Being able to jump after leaving a platform for a few miliseconds
 *      - Need a 'grounded' threshold to track when the player can jump
 *      
 *  - Clamped Fall Speed
 *      - 
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
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference for the RigidBody2D -> Drag the component into the reference to establish a line of communication
    public Rigidbody2D myRigidBody;

    // Player Variables
    private float moveHorizontal;
    private float moveSpeed = 10;

    public float jumpStrength = 7.5f;

    // Action Variables
    public bool canJump; // Start with one jump
    public bool canWallJump; // True when on the wall

    public bool canDash; // Start with one dash
    public bool canShoot; // Start with one bullet
    public bool canDie; // Start with one life
    public bool canGetHit; // Start with one hit

    public bool isGrounded; // True when on the ground


    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
        canWallJump = false;

        canDash = false;
        canShoot = false;
        canDie = false;
        canGetHit = false;

        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Gets Unity's left and right horizontal input - Axis is a scale of -1 (left) to 1 (right)
        moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Creates The Vector Variable vel - Vector2 is an X and Y axis
        Vector2 vel = new Vector2();

        // Horizontal Movement Speed - moving left or right
        vel.x = moveHorizontal * moveSpeed;
        // Vertical Movement Speed - like falling
        vel.y = myRigidBody.velocity.y;
        // Sets the velocities for the rigidbody
        myRigidBody.velocity = vel;

        // Checks if the jump key is pressed, while also checking if the player can jump
        if (Input.GetKeyDown(KeyCode.Space) == true && canJump == true && isGrounded == true)
        {
            myRigidBody.velocity = Vector2.up * jumpStrength;
            Debug.Log("jump key pressed.");
            canJump = false;
            Debug.Log("Player has jumnped once.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
