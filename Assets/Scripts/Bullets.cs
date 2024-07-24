using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	public float speed = 20f;
	public Rigidbody2D rb;

	void Start()
	{
		rb.velocity = transform.right * speed;
	}

	//when triggered on any onTrigger game object the bullet will destroy
    void OnTriggerEnter2D(Collider2D collision)
    {

		Destroy(gameObject);

		if (collision.CompareTag("Enemy")) 
		{
			// Destory the gameObject when collided with enemy 
			Destroy(collision.gameObject);
			//Add 10 scores if enemy is killed
			ScoreScript.scoreValue += 10;
		}
	}

}
