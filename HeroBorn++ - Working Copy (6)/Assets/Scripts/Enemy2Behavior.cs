using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Behavior : MonoBehaviour
{
    public Transform player;
    public Transform PatrolRoute;
    public List<Transform> locations;

    private int locationIndex = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private int _lives = 3;
    public int EnemyLives
    {
        get { return _lives; }

        private set
        {
            _lives = value;
            if (_lives <= 0)
            {
                Destroy(this.gameObject);
                Debug.Log("Enemy Down!");
            }

        }
    }

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
        {
            locations.Add(child);
        }
    }

    void Update()
    {
        if (agent.remainingDistance < 0.2f && !agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }
    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;
        agent.destination = locations[locationIndex].position;
        locationIndex = (locationIndex + 1) % locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        //2 
        if (other.name == "Player")
        {
            agent.destination = player.position;
            Debug.Log("Player detected - attack!");
        }
    }

    // 3
    void OnTriggerExit(Collider other)
    {
        // 4
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Critical hit!");
        }
    }
}
