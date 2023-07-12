using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAxe : MonoBehaviour
{
    private bool playerHit;
    [SerializeField] float kbSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("Player") && !playerHit) {
            Vector3 kbDir = (col.gameObject.transform.position - transform.position).normalized;
            col.gameObject.GetComponent<TPM_CharacterController>().TakeKnockback(kbSpeed, kbDir);
            playerHit = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        playerHit = false;
    }
}
