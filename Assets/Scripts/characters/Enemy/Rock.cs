using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    public Vector3 direction;

    void Start() 
    {
        rb = GetComponent<Rigidbody>();
        FlyToTarget();

        
    }
    public void FlyToTarget()
    {
        if(target == null)
            target = FindAnyObjectByType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force,ForceMode.Impulse);
    }
}
