using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableShop : MonoBehaviour
{
    public Shop shop;
    public ShopManager ShopManager;
    private bool PlayerInRange;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerInRange) {
            ShopManager.DisplayShop(this.shop);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            InteractionIndicator.Show(other.transform.localPosition);
            PlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            InteractionIndicator.Hide();
            PlayerInRange = false;
        }
    }
}
