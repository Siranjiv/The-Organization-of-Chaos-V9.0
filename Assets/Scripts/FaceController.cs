using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    // Movement force values
    [SerializeField]
    private float moveForce = 8f;
    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    private bool isGrounded = true;

    private string GROUND_TAG = "Ground";
    private string ENEMY_TAG = "Enemy";

    private string WALK_ANIMATION = "walk";
    private string JUMP_ANIMATION = "jump";

    private bool isFacingRight = true;

    // Shooting variables
    public Transform firePoint;
    public GameObject bulletPrefab;

    // OpenCV variables
    [SerializeField]
    private FaceDetection faceDetection;

    // Thresholds to avoid jittery movement
    private float moveThreshold = 15f;
    private float jumpThreshold = 20f;

    // Smoothing for face input
    private float smoothedFaceX;
    private float smoothedFaceY;

    // Reference values (initial average face position)
    private float neutralX = 500f;
    private float neutralY = 125f;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        smoothedFaceX = neutralX;
        smoothedFaceY = neutralY;

        faceDetection = FindObjectOfType<FaceDetection>();
    }

    void Update()
    {
        SmoothFaceInput(); // Smooth face data to reduce jitter
        HandleJump();      // Handle upward head movement as jump
        HandleShoot();     // Handle downward head movement as shoot
    }

    void FixedUpdate()
    {
        HandleMovement();  // Physics-based movement should go in FixedUpdate
    }

    void SmoothFaceInput()
    {
        // Smooths the face detection input to reduce jitter
        smoothedFaceX = Mathf.Lerp(smoothedFaceX, faceDetection.faceX, Time.deltaTime * 5f);
        smoothedFaceY = Mathf.Lerp(smoothedFaceY, faceDetection.faceY, Time.deltaTime * 5f);
    }

    void HandleMovement()
    {
        float deltaX = smoothedFaceX - neutralX;

        if (Mathf.Abs(deltaX) > moveThreshold)
        {
            float direction = Mathf.Sign(deltaX);
            Vector2 newPosition = myBody.position + new Vector2(direction * moveForce * Time.fixedDeltaTime, 0f);
            myBody.MovePosition(newPosition); // Let physics handle collisions

            // Animate walking
            anim.SetBool(WALK_ANIMATION, true);

            // Flip sprite based on direction
            if ((direction > 0 && !isFacingRight) || (direction < 0 && isFacingRight))
            {
                Flip();
            }
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
    }

    void HandleJump()
    {
        float deltaY = neutralY - smoothedFaceY; // Head going up means jump

        if (deltaY > jumpThreshold && isGrounded)
        {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
        }
        else if (isGrounded)
        {
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }

    void HandleShoot()
    {
        float deltaY = smoothedFaceY - neutralY; // Head going down means shoot

        if (deltaY > jumpThreshold)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
