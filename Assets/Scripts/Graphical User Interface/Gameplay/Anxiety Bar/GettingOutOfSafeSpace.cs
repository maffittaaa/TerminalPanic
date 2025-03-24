using System;
using UnityEngine;

public class GettingOutOfSafeSpace : MonoBehaviour
{
    [SerializeField] private AnxietyBar anxietyBar;
    private bool collided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anxietyBar.interacted = false;
            collided = true;
        }
        else
            collided = false;
    }

    public bool Collided(bool colliding)
    {
        return collided;
    }
}
