using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{   
    public List<Drop> Drops = new List<Drop>();
    public int MinRerolls = 1;
    public int MaxRerolls = 1;

    public List<Item> GetDropItems() {
        List<Item> generatedDrops = new List<Item>();

        int rerolls = Random.Range(this.MinRerolls, this.MaxRerolls + 1);

        if (rerolls <= 0) {
            throw new System.Exception("Rerolls is set to 0");
        }

        for (int i = rerolls; i > 0; i--) {
            int totalWeight = 0;
            // Calculate total weight
            foreach (Drop drop in Drops) {
                if (drop.Weight <= 0) {
                    throw new System.Exception("Weight is set to 0");
                }
                totalWeight += drop.Weight;
            }

            if (totalWeight <= 0) {
                return null;
            }

            int randomWeight = Random.Range(1, totalWeight + 1);

            foreach (Drop drop in Drops) {
                randomWeight -= drop.Weight;

                if (randomWeight <= 0 && drop.Item != null) {
                    for (int j = drop.RandomAmount(); j > 0; j--) {
                        generatedDrops.Add(drop.Item);
                    }
                    break;
                }
            }
        }

        return generatedDrops;
    }
}
