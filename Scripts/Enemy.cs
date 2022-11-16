using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy: MonoBehaviour
{
    public PlayerController targetPlayer;
    public Rigidbody rig;
    public float speed;
    public int enemyLogicNumber;
    public Transform[] patrolPoints;
    int patrolCounter;
    public int chaseRange;
    public Transform startPosition;
    public float delay;
    public int startSpeed;
    Vector3 targetDirection = new Vector3(0, 0, 0);
    public void OnLoggedIn()
    {
        speed = startSpeed;
        Invoke("DelaySpawn", delay);
        transform.position = startPosition.position;
    }
    void DelaySpawn()
    {
        gameObject.SetActive(true);
        InvokeRepeating("Move", .1f, .5f);
    }
    private void Move()
    {
        if (enemyLogicNumber == 0)
        {
            targetDirection = targetPlayer.transform.position - transform.position;
        }
        else if (enemyLogicNumber == 1)
        {
            if ((patrolPoints[patrolCounter].position - transform.position).magnitude <= speed/2)
            {
                if (patrolCounter == 3)
                {
                    patrolCounter = -1;
                }
                patrolCounter++;
                speed = startSpeed *2;
            }
            targetDirection = patrolPoints[patrolCounter].position - transform.position;
            Vector3 chaseCheck = targetPlayer.transform.position - patrolPoints[patrolCounter].position;
            if (chaseCheck.magnitude <= chaseRange)
            {
                targetDirection = targetPlayer.transform.position - transform.position;
                speed = startSpeed;
            }
        }
        else if (enemyLogicNumber == 2)
        {
            int randomChance = Random.Range(1, 10);
            if (randomChance == 1)
            {
                targetDirection = new Vector3(Random.Range(1, 20), 1.9f, Random.Range(1, 20));
            }
            else {
                targetDirection = targetPlayer.transform.position + targetPlayer.rig.velocity - transform.position;
            }
        }
        else
        {
            targetDirection = targetPlayer.transform.position + targetPlayer.rig.velocity - transform.position;
        }
        rig.velocity = targetDirection.normalized * speed;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == ("Wall"))
        {
             
        }
        if (other.gameObject.tag == ("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {

        }
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.TakeDamage();
        }
    }
}
