using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables used for the Player Object
    [SerializeField]
    private float moveForce = 8f;
    
    [SerializeField]
    private float jumpForce = 5f;

    private float movementX;

    //To access compornents
    [SerializeField]
    private Rigidbody2D myBody;

    private Transform Tr;

    private SpriteRenderer sr;

    private Animator anim;

    private bool isGrounded = true;

    private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";


    private string WALK_ANIMATION = "walk";
    private string JUMP_ANIMATION = "jump";


    
    private bool isFacingRight = true;
    private bool isFacingLeft = false;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    void Start()
    {
        myBody.AddForce(new Vector2(2, 2));
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveKeyboard();
        AnimatePlayer();
        PlayerJump();
    }


    //To control the Character through keyboard
    void playerMoveKeyboard() {

        //input from A,D key(or left right keys) where value can be either negative or positive and zero if didn't press any key
        movementX = Input.GetAxisRaw("Horizontal");

        //Time.deltaTime->The completion time in seconds since the last frame(basically Time.deltaTime is the time between each frame)
        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime*moveForce;
        

    }

    //New Method to flip the character
    void Flip()
    {
        isFacingRight = !isFacingRight;
        isFacingLeft = !isFacingLeft;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }



    //The Character Animation when moving
    void AnimatePlayer() {

        //if the chatacter going to the right
        if (movementX > 0)
        {
            //in oreder to animeate the player we call anim to reference the compornent
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);

            if (!isFacingRight)
            {
                Flip(); // Flip to the right
            }

            #region Old Code to rotate the character instead of flipping
            ////if the chatacter is facing right there won't be any rotation
            //if (isFacingRight == true)
            //{

            //    transform.Rotate(0f, 0f, 0f);
            //    isFacingRight = true;
            //    isFacingLeft = false;
            //}
            //else   //if the chatacter isn't facing right there will be any rotation
            //{
            //    transform.Rotate(0f, 180f, 0f);
            //    isFacingRight = true;
            //    isFacingLeft = false;

            //}
            #endregion
        }
        else if (movementX < 0)//if the chatacter going to the left
        {
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);

            if (!isFacingLeft)
            {
                Flip(); // Flip to the left
            }

            #region Old Code to rotate the character instead of flipping
            //if the chatacter is facing left there won't be any rotation
            if (isFacingLeft == true)
            {

                transform.Rotate(0f, 0f, 0f);
                isFacingLeft = true;
                isFacingRight = false;
            }
            else  //if the chatacter isn't facing right there will be any rotation
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingLeft = true;
                isFacingRight = false;

            }

            #endregion
        }
        else 
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
        


    }

    //Control and Character Animation for jumping
    void PlayerJump()
    {
        // this is for spacebar so GetButtonDown("Jump") is for space bar being pressed"
        if (Input.GetButtonDown("Jump") && isGrounded) //isGrounded is to check if player is on the ground
        {


            isGrounded = false;
            //ForceMode2D.Impulse-> Add an instant force impluse to the rigid body using its mass(basically just gonna push the player right up)
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);


        }
        

    }


    
    //build in fuction which allow to detect collision between 2 game objects
    //The collision object is the second parameter we are colliding with
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = true;

        // if the player comes in contact with the monsters
        if (collision.gameObject.CompareTag(ENEMY_TAG))
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the player comes in contact with the ghosts
        if (collision.CompareTag(ENEMY_TAG))
            Destroy(gameObject);
    }

   
}//Class
