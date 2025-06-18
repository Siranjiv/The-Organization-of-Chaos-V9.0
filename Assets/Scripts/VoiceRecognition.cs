using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{

    //Player Movement Variables
    [SerializeField]
    private float moveForce = 8f;

    private bool isFacingRight = true;
    private bool isFacingLeft = false;

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

    //Shooting Variables
    public Transform firePoint;
    public GameObject bulletPrefab;

    //Voice_Recognition_Variables
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();



    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        myBody.AddForce(new Vector2(2, 2));

        // Only create the recognizer if it's not already running
        if (keywordRecognizer != null)
        {
            Debug.LogWarning("KeywordRecognizer already exists! Disposing old one.");
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
            keywordRecognizer = null;
        }

        actions.Add("left", Left);
        actions.Add("jump", Jump);
        actions.Add("right", Right);
        actions.Add("shoot", Shoot);

        //Pass all the actions as a array of strings
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        //Recognize the Speech phrase
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        Debug.Log("Voice recognition started.");
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {

        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    //Voice Recognition Movements
    private void Left() {

       
        transform.Translate(2, 0, 0);

        anim.SetBool(JUMP_ANIMATION, false);
        anim.SetBool(WALK_ANIMATION, true);


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

    private void Jump()
    {

        if (isGrounded) 
        {


            isGrounded = false;
            myBody.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);


        }


    }

    private void Right()
    {


         transform.Translate(2, 0, 0);


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

        //Animation
        anim.SetBool(WALK_ANIMATION, true);


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
            Debug.Log("Voice recognition stopped and disposed.");
        }
    }

}
