using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] monsterReferencfe;

    [SerializeField]
    private Transform leftPos, rightPos;

    private GameObject spawnedMonster;



    private int randomIndex;//determine the side of the monster spawning
    private int randomSide;//determine the index of the spawn monster


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    //the spanwning is happening in a co-routine
    // calling over a interval of time -->call it over and over again every 5 to 10 seconds

    IEnumerator SpawnMonsters()
    {

        //This is a co routine
        while (true)
        {//since yield return is there it will wait

            yield return new WaitForSeconds(Random.Range(1, 5));//it is waiting from this specific seconds

            //in order to spawn a monster
            randomIndex = Random.Range(0, monsterReferencfe.Length);//from the array we randaomize the enemy we want
            randomSide = Random.Range(0, 2);

            spawnedMonster = Instantiate(monsterReferencfe[randomIndex]);// function for the copy of the monster(gameobject) that we pass as reference

            //left side
            if (randomSide == 0)
            {
                spawnedMonster.transform.position = leftPos.position;

                spawnedMonster.GetComponent<Monster>().speed = Random.Range(4, 10);
            }
            else//right side
            {
                spawnedMonster.transform.position = rightPos.position;

                spawnedMonster.GetComponent<Monster>().speed = -Random.Range(4, 10);  //value is negative due to the directon

                //based on image we are flipping the monster using scale
                spawnedMonster.transform.localScale = new Vector3(-1f, 1f, 1f);

            }
        }//while
    }


}
