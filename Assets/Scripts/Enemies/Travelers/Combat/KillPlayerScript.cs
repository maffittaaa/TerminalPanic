using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerScript : MonoBehaviour
{
    public bool canDie { get; set; }

    private void Start()
    {
        canDie = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canDie)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                WorldInteractions player = FindFirstObjectByType<WorldInteractions>();
                KillPlayer(player);
            }
        }
    }

    void KillPlayer(WorldInteractions player)
    {
        player.KillPlayer();
    }
}
