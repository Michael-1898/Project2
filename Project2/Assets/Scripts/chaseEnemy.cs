using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chaseEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] Transform playerPosition;
    [SerializeField] float kbSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        agent.SetDestination(playerPosition.position);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player")) {
            //col.gameObject.GetComponent<TPM_CharacterController>().TakeKnockback(kbSpeed);
        }
    }
}
