using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private Item item;
    public Item Item { get => item; set { item = Instantiate(value); SpriteRenderer.sprite = Item.Sprite; }}
    public SpriteRenderer SpriteRenderer;
    public bool Tangible = false;
    private bool pulledByMagnet = false;
    private Vector3 magnetTarget;
    private float magnetStrength;
    public Rigidbody2D rb;

    private void Awake() {
        if (SpriteRenderer.sprite == null && Item != null) {
            SpriteRenderer.sprite = Item.Sprite;
        }
    }

    public void StopMagnetPull() {
        this.pulledByMagnet = false;
    }

    public void MagnetPull(Vector3 magnetTarget, float magnetStrength) {
        if (InventoryManager.CanAddItemAmount(Item, 1)) {
            this.magnetTarget = magnetTarget;
            this.pulledByMagnet = true;
            this.magnetStrength = magnetStrength;
        } else {
            this.pulledByMagnet = false;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (this.Tangible && other.gameObject.CompareTag("Player")) {
            if (InventoryManager.TryAddItem(Item)) {
                Destroy(this.gameObject);
            } 
        }
    }

    private void FixedUpdate() {
        if (pulledByMagnet && Tangible) {
            Vector2 targetDirection = (this.magnetTarget - this.transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * magnetStrength;
        }
    }

    
}
