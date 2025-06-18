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
    private FaceDetection faceDetection;

    // Start is called before the first frame update
    private float speed = 5f;

    //The Average coodinates of x and y axis in the frame
    private float lastY = 125f;
    private float lastX = 500f;
    private float smoothedFaceX;


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
        smoothedFaceX = lastX;
    }

    // Update is called once per frame
    void Update()
    {

        SmoothFaceInput();
        AnimatePlayer();
        PlayerJump();
        Shoot();       
    
    }

    void FixedUpdate()
    {
        playerMove(); // Use physics movement here
    }

    void SmoothFaceInput()
    {
        // Smooths the face detection input to reduce jitter
        smoothedFaceX = Mathf.Lerp(smoothedFaceX, faceDetection.faceX, Time.deltaTime * 5f);
    }
    void playerMove()
    {
        // Scale down face movement and clamp
        float normX = Mathf.Clamp((lastX - smoothedFaceX) / 100f, -1f, 1f);

        // Calculate target movement
        Vector2 targetPos = myBody.position + new Vector2(normX * moveForce * Time.fixedDeltaTime, 0f);

        // Clamp movement within screen bounds (adjust -8f and 8f to fit your level)
        float clampedX = Mathf.Clamp(targetPos.x, -8f, 8f);
        targetPos = new Vector2(clampedX, targetPos.y);

        myBody.MovePosition(targetPos);

    }

    void AnimatePlayer()
    {
        float normX = Mathf.Clamp((lastX - smoothedFaceX) / 100f, -1f, 1f);

        if (normX > 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);

            if (!isFacingRight)
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = true;
                isFacingLeft = false;
            }
        }
        else if (normX < 0)
        {
            anim.SetBool(WALK_ANIMATION, true);
            anim.SetBool(JUMP_ANIMATION, false);

            if (!isFacingLeft)
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

    void Shoot()
    {
        float normY = Mathf.Clamp(lastY - faceDetection.faceY, -1, 1);

        if (normY < 0)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

}
