using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    [SerializeField] private float moveForce = 8f;
    [SerializeField] private Rigidbody2D myBody;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private SpriteRenderer sr;
    private Animator anim;

    private bool isFacingRight = true;
    private bool isFacingLeft = false;
    private bool isGrounded = true;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private const string GROUND_TAG = "Ground";
    private const string ENEMY_TAG = "Enemy";
    private const string WALK_ANIMATION = "walk";
    private const string JUMP_ANIMATION = "jump";

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        myBody.AddForce(new Vector2(2, 2));

        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }

        actions.Add("left", () => StartMoveLeft());
        actions.Add("right", () => StartMoveRight());
        actions.Add("stop", StopMovement);
        actions.Add("jump", Jump);
        actions.Add("shoot", Shoot);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
        Debug.Log("Voice recognition started.");
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log($"Heard: {speech.text}");
        if (actions.ContainsKey(speech.text))
            actions[speech.text].Invoke();
    }

    private void Update()
    {
        HandleContinuousMovement();

        if (isGrounded)
        {
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }

    private void HandleContinuousMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (isMovingLeft)
        {
            moveDirection = Vector3.left;

            if (!isFacingLeft)
            {
                transform.localRotation = Quaternion.Euler(0, 180f, 0);
                isFacingLeft = true;
                isFacingRight = false;
            }

            anim.SetBool(WALK_ANIMATION, true);
        }
        else if (isMovingRight)
        {
            moveDirection = Vector3.right;

            if (!isFacingRight)
            {
                transform.localRotation = Quaternion.Euler(0, 0f, 0);
                isFacingRight = true;
                isFacingLeft = false;
            }

            anim.SetBool(WALK_ANIMATION, true);
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, false);
        }

        // Apply movement
        transform.position += moveDirection * moveForce * Time.deltaTime;
    }

    private void StartMoveLeft()
    {
        isMovingLeft = true;
        isMovingRight = false;
    }

    private void StartMoveRight()
    {
        isMovingRight = true;
        isMovingLeft = false;
    }

    private void StopMovement()
    {
        isMovingLeft = false;
        isMovingRight = false;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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

    private void OnDisable()
    {
        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
            Debug.Log("Voice recognition stopped.");
        }
    }
}

