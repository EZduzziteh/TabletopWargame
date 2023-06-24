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
    public NavMeshAgent agent;
    public float stoppingDistance=0.25f;
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<NavMeshAgent>())
        {
            gameObject.AddComponent<NavMeshAgent>();
        }
        agent = GetComponent <NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.speed = GetComponentInParent<Unit>().stats.move;
        agent.radius = GetComponentInParent<Unit>().basesize;
        startpos = transform.position;
        endpos = new Vector3(0, 0, 0);
        currenttime = 0.0f;
        //ismoving = true;
    }


    private void Update()
    {
      
     
       
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
        //#TODO, maybe have to add some sort of acceptance range
        agent.SetDestination(endpoint);
    }

    public void Startsink()
    {
        Debug.Log(name + "started sinking");
        Destroy(agent);
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().mass = 1;
        GetComponent<Rigidbody>().drag = 30;
        GetComponent<Rigidbody>().useGravity = true;
        
        
    }

  
}
