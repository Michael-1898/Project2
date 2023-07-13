using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemy : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private NavMeshAgent agent;

    [Header("Damage")]
    [SerializeField] private float knockbackSpeed;
    [SerializeField] private float knockbackDuration;
    private bool playerAlreadyHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        agent.SetDestination(playerPosition.position);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player") && !playerAlreadyHit) {
            Vector3 knockbackDirection = (col.gameObject.transform.position - transform.position).normalized;
            col.GetComponent<TPM_CharacterController>().TakeKnockback(knockbackDirection, knockbackSpeed, knockbackDuration);
            playerAlreadyHit = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player")) {
            playerAlreadyHit = false;
        }
    }
}
