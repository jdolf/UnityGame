using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    private const float DefaultDropPushMultiplier = 3f;
    public static DropManager Instance { get; private set; }
    public PlayerMovementManager Player;
    public GameObject CollectiblePrefab;

    private void Awake() {
        Instance = this;
    }
    
    public static void CreateDrop(Item item, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        Instance.createDrop(item, position, dropDirection, dropPushMultiplier);
    }

    public static void CreateDrops(List<Item> items, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        Instance.createDrops(items, position, dropDirection, dropPushMultiplier);
    }

    public static void CreateDrops(ItemStack itemStack, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        Instance.createDrops(itemStack, position, dropDirection, dropPushMultiplier);
    }

    private void createDrop(Item item, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        if (position == null) {
            position = Player.transform.position;
        }

        Collectible collectible = Instantiate(CollectiblePrefab, (Vector3) position, Quaternion.identity).GetComponent<Collectible>();
        collectible.Item = Instantiate(item);

        if (dropDirection == DropDirection.Random) {
            collectible.rb.AddForce(
                new Vector2(Mathf.Cos(Random.Range(0f, 360f)) * dropPushMultiplier, Mathf.Sin(Random.Range(0f, 360f)) * dropPushMultiplier),
                ForceMode2D.Impulse
            );
        } else if (dropDirection == DropDirection.PlayerFacing) {
            Vector2 direction = PlayerMovementManager.DirectionDictionary[Player.facing];
            Debug.Log(direction.x + " " + direction.y);
            float usedX = 0f;
            float usedY = 0f;

            // x axis takes precedence on diagonals
            if (direction.x != 0f && direction.y != 0f) {
                usedY = 0f;
            } else {
                usedY = direction.y;
            }
            usedX = direction.x;

            collectible.rb.AddForce(
                new Vector2(usedX * dropPushMultiplier, usedY * dropPushMultiplier),
                ForceMode2D.Impulse
            );
        }
    }

    private void createDrops(List<Item> items, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        foreach (Item item in items) {
            createDrop(item, position, dropDirection, dropPushMultiplier);
        }
    }

    private void createDrops(ItemStack itemStack, Vector3? position = null, DropDirection dropDirection = DropDirection.Random, float dropPushMultiplier = DefaultDropPushMultiplier) {
        for (int i = 0; i < itemStack.Amount; i++) {
            createDrop(itemStack.Item, position, dropDirection, dropPushMultiplier);
        }
    }

    public enum DropDirection {
        Random,
        PlayerFacing
    }
}
