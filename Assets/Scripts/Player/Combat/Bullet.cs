using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [field:SerializeField] public Vector3 shootingPoint;
    [SerializeField] private Rigidbody rb;

    void Start()
    {
        CalculateDirection();
    }

    private void CalculateDirection()
    {
        Vector3 newDirection = shootingPoint - transform.position;
        newDirection = newDirection.normalized;

        rb.velocity = newDirection * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
