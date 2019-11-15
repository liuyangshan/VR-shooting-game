using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public GameObject[] target;
    int index;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 targetPos = new Vector3(target[index].transform.position.x, transform.position.y, target[index].transform.position.z);
       
        Quaternion desiredRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime);

        Vector3 enemyPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        float dis = Vector3.Distance(targetPos, enemyPos);

        if (dis < 0.5)
        {
            index += 1;
         
            if (index == target.Length)
            {
                index = 0;
            }
        }

    }
}
