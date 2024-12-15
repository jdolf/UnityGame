using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prowess
{
    protected static List<Prowess> Prowesses = new List<Prowess>{
        new Prowess("CLUELESS", new Color32(233,233,233,255)),
        new Prowess("ROOKIE", new Color32(255,255,255,255)),
        new Prowess("AMATEUR", new Color32(91,91,91,255)),
        new Prowess("APPRENTICE", new Color32(0,0,0,255)),
        new Prowess("SKILLED", new Color32(0,152,9,255)),
        new Prowess("ADEPT", new Color32(7,255,0,255)),
        new Prowess("PROFESSIONAL", new Color32(0,18,132,255)),
        new Prowess("EXPERT", new Color32(11,0,255,255)),
        new Prowess("MASTER", new Color32(255,122,0,255)),
        new Prowess("RENOWNED MASTER", new Color32(255,35,0,255)),
        new Prowess("LEADING MASTER", new Color32(255,0,227,255)),
        new Prowess("GRAND MASTER", new Color32(0,255,251,255)),
        new Prowess("CHAMPION", new Color32(241,255,0,255)),
        new Prowess("GRAND CHAMPION", new Color32(255,255,255,255)),
        new Prowess("WORLD-RENOWNED CHAMP.", new Color32(255,255,255,255)),
        new Prowess("LEGEND", new Color32(255,255,255,255)),
        new Prowess("MYTH", new Color32(255,255,255,255)),
        new Prowess("COMPLETIONIST", new Color32(255,255,255,255))
    };

    public static Prowess GetProwessByLevel(int level) {
        if (level == 1) {
            return Prowesses[0];
        }

        if (level >= 50) {
            return Prowesses[Prowesses.Count - 1];
        }

        int index = (int) Mathf.Floor(((float) level - 2) / 3) + 1;
        return Prowesses[index];
    }

    public string Name;
    public Color Color;

    protected Prowess(string name, Color color) {
        this.Name = name;
        this.Color = color;
    }
}
