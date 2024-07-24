using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [HideInInspector]
    public float speed;

    private Rigidbody2D myBody;




    // Start is called before the first frame update
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //The speed is the movement speed through the x axis
        //myBody.velocity.y --> we dont change the y velocity, so this is the current velocity that we have
        myBody.velocity = new Vector2(speed, myBody.velocity.y);

    }

}
