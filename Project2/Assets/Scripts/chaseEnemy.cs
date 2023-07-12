using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chaseEnemy : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshAgent agent;
    [SerializeField] Transform playerPosition;

    [Header("Hitting Player")]
    [SerializeField] float kbSpeed;
    private bool playerHit;

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

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player") && !playerHit) {
            Vector3 kbDir = (playerPosition.position - transform.position).normalized;
            col.gameObject.GetComponent<TPM_CharacterController>().TakeKnockback(kbSpeed, kbDir);
            playerHit = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        playerHit = false;
    }
}
