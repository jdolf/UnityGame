using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitSplat : MonoBehaviour
{
    public static void Create(int damage, Vector3 position) {
        GameObject go = Instantiate(Resources.Load("Prefabs/HitSplat")) as GameObject;
        go.transform.position = position + new Vector3(0, 1.5f);
        HitSplat hitsplat = go.GetComponent<HitSplat>();
        hitsplat.Instantiate(damage);
    }
    public float TimeToLive = 40f;

    // Update is called once per frame
    void Update()
    {
        this.TimeToLive -= 60 * Time.deltaTime;

        if (this.TimeToLive <= 0) {
            this.Die();
        }

        transform.position += new Vector3(0, 3f) * Time.deltaTime;
    }

    public void Instantiate(int damage) {
        TextMeshPro textMesh = this.transform.GetComponent<TextMeshPro>();
        textMesh.SetText(damage.ToString());
    }

    public void Die() {
        Destroy(this.gameObject);
    }
}
