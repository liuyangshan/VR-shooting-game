using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public GameObject end, start; // The gun start and end point
    public GameObject[] target;
    public GameObject[] cover;
    public GameObject player;
    public GameObject gun;

    public GameObject bulletHole;


    public float health = 100;
    //public Double spreadFactor = 0.02;

    private float fireTimer;
    private float hideTimer;
    private bool enemySawPlayer;
    private Vector3 targetPos;
    private Vector3 newTargetPos;
    private Vector3 closesCover;

    private float enemyCoverDis;

    int index;
    // Start is called before the first frame update
    void Start()
    {

        enemySawPlayer = false;
        targetPos = cover[0].transform.position;

    }

    // Update is called once per frame
    void Update()
    {


        // detect player
        Vector3 enemyForward = transform.forward;
        Vector3 playerPos = (player.transform.position - transform.position).normalized;
        //float playerEnemyDis = Vector3.Distance(playerPos, transform.position);
        //float playerEnemyDis = Vector3.Distance(player.transform.position, transform.position);
        //print(health);

        if (Vector3.Dot(enemyForward, playerPos) > 0.5)
        {
            //targetPos = player.transform.position;
            enemySawPlayer = true;

        }
        else
        {
            targetPos = new Vector3(target[index].transform.position.x, transform.position.y, target[index].transform.position.z);

        }
        
        


       
        // if enemy get hit by player, the enemy will discover player
        if (health < 100)
        {
            enemySawPlayer = true;
        }



        if (enemySawPlayer == true)
        {
            float mindis = 1000;
            for (int i = 0; i < cover.Length; i++)
            {
                float playerCoverDis = Vector3.Distance(player.transform.position, cover[i].transform.position);
                if (playerCoverDis < mindis)
                {
                    mindis = playerCoverDis;
                    closesCover = new Vector3(cover[i].transform.position.x, transform.position.y, cover[i].transform.position.z);
                }
            }



        }

        enemyCoverDis = Vector3.Distance(transform.position, closesCover);


        if (enemyCoverDis >= 3.0f && enemySawPlayer == true && health > 0)
        {
            targetPos = closesCover;
            GetComponent<Animator>().SetBool("run", true);
            //targetPos = newTargetPos;
        }

        print(enemyCoverDis);
        print(enemySawPlayer);
        print(health);
        

        if (enemyCoverDis < 3.0f && enemySawPlayer == true && health > 0)
        {
            

            hideTimer += Time.deltaTime;
            if (hideTimer < 5.0f)
            {
                targetPos = closesCover;
                float xdis = 0;
                float zdis = 0;
                float coversize = 0.8f;
                if (targetPos.x - player.transform.position.x > 0)
                {
                    xdis = coversize;
                }
                else
                {
                    xdis = -coversize;
                }

                if (targetPos.z - player.transform.position.z > 0)
                {
                    zdis = coversize;
                }
                else
                {
                    zdis = -coversize;
                }

                transform.position = new Vector3(targetPos.x + xdis, transform.position.y, targetPos.z + zdis);
                GetComponent<Animator>().SetBool("run", false);
                GetComponent<Animator>().SetBool("hide", true);
                GetComponent<CharacterController>().enabled = false;

            }

            else if (hideTimer < 10.0f)
            {
                targetPos = player.transform.position;

                fireTimer += Time.deltaTime;
                if (fireTimer > 0.2f)
                {
                    GetComponent<CharacterController>().enabled = true;
                    GetComponent<Animator>().SetBool("hide", false);
                    GetComponent<Animator>().SetTrigger("fire");
                    shotDetection();
                    fireTimer = 0;
                }
            }
            else
            {
                hideTimer = 0;
            }

        }



        if (health <= 0)
        {
            
            GetComponent<Animator>().SetBool("dead", true);
            GetComponent<Animator>().SetBool("run", false);
            GetComponent<CharacterController>().enabled = false;

            gun.GetComponent<Rigidbody>().isKinematic = false;
            gun.GetComponent<BoxCollider>().enabled = true;
            gun.transform.parent = null;
            

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


        float shootSpreadY = UnityEngine.Random.Range(0.8f, 1.0f);
        float shootSpreadZ = UnityEngine.Random.Range(0.8f, 1.0f);

        Vector3 tempEnd = new Vector3(end.transform.position.x, end.transform.position.y * shootSpreadY, end.transform.position.z * shootSpreadZ);
        //Vector3 tempEnd = end.transform.position;



        if (Physics.Raycast(end.transform.position, (tempEnd - start.transform.position), out rayHit, 100.0f))
        {
            //print(rayHit.transform.tag);
            if (rayHit.transform.tag == "Player")
            {
                //Being_shot(20);
            }
            else
            {
                //Instantiate(bulletHole, rayHit.point + rayHit.transform.up * 0.01f, rayHit.transform.rotation);
            }
        }
    }
}
