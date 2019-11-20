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
        //float playerEnemyDis = Vector3.Distance(playerPos, transform.position);
        float playerEnemyDis = Vector3.Distance(player.transform.position, transform.position);
        //print(health);

        if (Vector3.Dot(enemyForward, playerPos) > 0.5)
        {
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
        }

        if (playerEnemyDis < 10.0f && enemySawPlayer == true && health > 0)
        {
            GetComponent<Animator>().SetBool("run", false);
            fireTimer += Time.deltaTime;

            if (fireTimer > 0.2f)
            {
                print("set fire trigger to True");
                GetComponent<Animator>().SetTrigger("fire");
                shotDetection();
                fireTimer = 0;
            }
        }

        if (health <= 0)
        {
            GetComponent<Animator>().SetBool("dead", true);
            GetComponent<Animator>().SetBool("run", false);

        }

        Quaternion desiredRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);

        if (enemySawPlayer != true)
        {
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
        

    }

    public void Being_shot(float damage) // getting hit from enemy
    {
        player.GetComponent<GunVR>().health -= damage;
        print(player.GetComponent<GunVR>().health);
    }

    void shotDetection() // Detecting the object which player shot 
    {
        RaycastHit rayHit;
        if (Physics.Raycast(end.transform.position, (end.transform.position - start.transform.position), out rayHit, 100.0f))
        {
            print(rayHit.transform.tag);
            if (rayHit.transform.tag == "Player")
            {
                Being_shot(10);
            }
            else
            {
                //Instantiate(bulletHole, rayHit.point + rayHit.transform.up * 0.01f, rayHit.transform.rotation);
            }
        }
    }
}
