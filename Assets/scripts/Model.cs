using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Model : MonoBehaviour
{
 
    public Vector3 startpos;
    public float currenttime=0f;
    public Vector3 endpos;
    public float totaltime;
    public bool ismoving = false;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<NavMeshAgent>())
        {
            gameObject.AddComponent<NavMeshAgent>();
        }
        agent = GetComponent <NavMeshAgent>();
        agent.speed = GetComponentInParent<Unit>().stats.move;
        agent.radius = GetComponentInParent<Unit>().basesize;
        startpos = transform.position;
        endpos = new Vector3(0, 0, 0);
        currenttime = 0.0f;
        //ismoving = true;
    }


    private void Update()
    {
      
       /* if (Input.GetKeyDown(KeyCode.H))
        {
            LerpToPos(transform.position*10f, 4);
        }*/
       
        if (ismoving)
        {
            transform.position = Vector3.Lerp(startpos, endpos, currenttime);
            currenttime += Time.deltaTime/totaltime;

            if(Vector3.Distance(transform.position, endpos)<=0.1f){
                ismoving = false;
               
            }
        }
    }

    public void LerpToPos(Vector3 endpoint, float lerptime) 
    {
        startpos = transform.position;
        endpos = endpoint;
        totaltime = lerptime;
        currenttime = 0;
        ismoving = true;
    }

    public void MoveToPos(Vector3 endpoint)
    {
        agent.SetDestination(endpoint);
    }

    public void Startsink()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().mass = 1;
        GetComponent<Rigidbody>().drag = 30;
        GetComponent<Rigidbody>().useGravity = true;
        
    }

    // Update is called once per frame
    /* void fixedUpdate()
     {
         currenttime += 1;
        
     }*/
}
