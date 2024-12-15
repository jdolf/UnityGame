using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesMagnet : MonoBehaviour
{
    public float MagnetStrength = 5f;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent<Collectible>(out Collectible collectible)) {
            collectible.MagnetPull(this.transform.parent.position, this.MagnetStrength);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent<Collectible>(out Collectible collectible)) {
            collectible.StopMagnetPull();
        }
    }
}
