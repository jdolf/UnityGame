using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Drop
{
    public Item Item;
    public int MinAmount;
    public int MaxAmount;
    public int Weight;

    public int RandomAmount() {
        return Random.Range(this.MinAmount, this.MaxAmount + 1);
    }

}
