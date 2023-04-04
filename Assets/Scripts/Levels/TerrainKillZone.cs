using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainKillZone : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player) && Mathf.Abs(collision.contacts[0].normal.y) > 0.5f) player.Die();
    }
}
