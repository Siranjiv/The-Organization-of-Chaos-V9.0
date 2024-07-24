using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    //Player moving script variable reference

    [SerializeField]
    private float moveForce = 8f;

    [SerializeField]
    private float jumpForce = 5f;

    private float movementX;
    private float normX;
    private float normY;
    //private float movementY;

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

    //Shooting variables
    public Transform firePoint;
    public GameObject bulletPrefab;


    //OpenCV variables
    [SerializeField]
    FaceDetection faceDetection;

    // Start is called before the first frame update
    float speed = 5f;

    //The Average coodinates of x and y axis in the frame
    float lastY = 125f;
    float lastX = 500f;


    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        myBody.AddForce(new Vector2(2, 2));
        faceDetection = (FaceDetection)FindObjectOfType(typeof(FaceDetection));
    }

    // Update is called once per frame
    void Update()
    {


        playerMove();
        AnimatePlayer();
        PlayerJump();
        Shoot();
        
    
    }

    void playerMove()
    {

        //To get the needed record of the frame
        float normX = Mathf.Clamp(lastX-faceDetection.faceX, -1, 1);

        transform.position += new Vector3(normX, 0f, 0f) * Time.deltaTime * moveForce;
        
    }

    void AnimatePlayer()
    {
        float normX = Mathf.Clamp(lastX - faceDetection.faceX, -1, 1);

        //we are going to the right side
        if (normX > 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);



            if (isFacingRight == true)
            {

                transform.Rotate(0f, 0f, 0f);
                isFacingRight = true;
                isFacingLeft = false;
            }
            else
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = true;
                isFacingLeft = false;

            }
        }
        else if (normX < 0)//we are going to the left
        {
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);


            if (isFacingLeft == true)
            {

                transform.Rotate(0f, 0f, 0f);
                isFacingLeft = true;
                isFacingRight = false;
            }
            else
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingLeft = true;
                isFacingRight = false;

            }
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }



    }

    void PlayerJump()
    {
        float normY = Mathf.Clamp(lastY - faceDetection.faceY, -1, 1);
        Debug.Log(normY);


        if (normY > 0 && isGrounded)
        {


            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);


        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
            isGrounded = true;


        if (collision.gameObject.CompareTag(ENEMY_TAG))
            Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ENEMY_TAG))
            Destroy(gameObject);
    }


    void Shoot()
    {
        float normY = Mathf.Clamp(lastY - faceDetection.faceY, -1, 1);

        if (normY < 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }


}
