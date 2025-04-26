using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            WorldInteractions player = FindFirstObjectByType<WorldInteractions>();
            Debug.Log(player);
            KillPlayer(player);
        }
    }

    void KillPlayer(WorldInteractions player)
    {
        Debug.Log("Killing");
        player.KillPlayer();
    }
}
