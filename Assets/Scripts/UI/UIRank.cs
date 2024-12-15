using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRank : MonoBehaviour
{
    public List<Sprite> LevelSprites = new List<Sprite>();
    public Image Rank;

    public void ChangeRank(int level) {
        int levelIndex = level - 1;

        if (levelIndex < LevelSprites.Count) {
            Rank.sprite = LevelSprites[levelIndex];
        }
    }
}
