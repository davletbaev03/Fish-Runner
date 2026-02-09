using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Obstacle") && !other.CompareTag("Food"))
            return;

        Destroy(other.gameObject);
    }
}
