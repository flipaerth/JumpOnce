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
 */



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Reference for the RigidBody2D -> Drag the component into the reference to establish a line of communication
    public Rigidbody2D myRigidBody;

    // Variables
    public float jumpStrength = 7.5f;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gets Unity's left and right horizontal input - Axis is a scale of -1 (left) to 1 (right)
        float h = Input.GetAxis("Horizontal");

        Rigidbody2D rb = myRigidBody;
        Vector2 vel = new Vector2();

        vel.x = h * speed;
        vel.y = rb.velocity.y;

        rb.velocity = vel;

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            myRigidBody.velocity = Vector2.up * jumpStrength;
            Debug.Log("jump key pressed");
        }
    }
}
