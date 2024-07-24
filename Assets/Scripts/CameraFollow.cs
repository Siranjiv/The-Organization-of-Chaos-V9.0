using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player;
    private Vector3 tempPos;

    // Start is called before the first frame update
    void Start()
    {
        //Finding the Player Object using "Player" tag
        player = GameObject.FindWithTag("Player").transform;
    }

    
    void LateUpdate()
    {

        if(!player)//if player gets destoryed
            return;

        //current position of the camera
        tempPos = transform.position;

        //Changing the Position based on the player's position in x, y axis
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;

        //updating the camera position
        transform.position = tempPos;
    }
}
