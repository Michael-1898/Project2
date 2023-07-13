using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAxe : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") && !playerAlreadyHit) {
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
