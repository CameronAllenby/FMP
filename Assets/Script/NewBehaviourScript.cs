using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 100f;
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerExit(Collider collision)
    {
        Destroy(gameObject);
    }
}
