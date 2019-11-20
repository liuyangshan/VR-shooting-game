using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject end, start; // The gun start and end point
    public GameObject[] target;
    public GameObject player;

    public GameObject bulletHole;

    public float health = 100;


    private float fireTimer;
    private bool enemySawPlayer;
    private Vector3 targetPos;

    int index;
    // Start is called before the first frame update
    void Start()
    {

        enemySawPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {


        // detect player
        Vector3 enemyForward = transform.forward;
        Vector3 playerPos = (player.transform.position - transform.position).normalized;
        float playerEnemyDis = Vector3.Distance(playerPos, transform.position);
        //print(playerEnemyDis);


        
        if (Vector3.Dot(enemyForward, playerPos) > 0.5)
        {
            //print("In print A");
            targetPos = player.transform.position;
            enemySawPlayer = true;

        }
        else
        {
            targetPos = new Vector3(target[index].transform.position.x, transform.position.y, target[index].transform.position.z);

        }
        
        

        if (playerEnemyDis >= 10.0f && enemySawPlayer == true)
        {
            GetComponent<Animator>().SetBool("run", true);
            fireTimer += Time.deltaTime;

            //print(fireTimer);
            //print(playerEnemyDis);

            if (fireTimer > 0.2f)
            {
                //print("set fire trigger to True");
                GetComponent<Animator>().SetTrigger("fire");
                fireTimer = 0;
            }

        }

        
        if (playerEnemyDis < 10.0f)
        {
            GetComponent<Animator>().SetBool("run", false);

        }


        if (health == 0)
        {
            GetComponent<Animator>().SetBool("dead", true);

        }

        Quaternion desiredRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);

        Vector3 enemyPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float dis = Vector3.Distance(targetPos, enemyPos);

        if (dis < 1)
        {
            index += 1;
         
            if (index == target.Length)
            {
                index = 0;
            }
        }

    }


    void shotDetection() // Detecting the object which player shot 
    {
        RaycastHit rayHit;
        if (Physics.Raycast(end.transform.position, (end.transform.position - start.transform.position), out rayHit, 100.0f))
        {
            if (rayHit.transform.tag == "player")
            {
                player.GetComponent<GunVR>().health -= 20;
                print(player.GetComponent<GunVR>().health);
            }
            else
            {
                Instantiate(bulletHole, rayHit.point + rayHit.transform.up * 0.01f, rayHit.transform.rotation);
            }
        }
    }
}
