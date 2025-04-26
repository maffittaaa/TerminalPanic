using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WorldInteractions player = FindFirstObjectByType<WorldInteractions>();
            KillPlayer(player);
        }
    }

    void KillPlayer(WorldInteractions player)
    {
        player.KillPlayer();
    }
}
