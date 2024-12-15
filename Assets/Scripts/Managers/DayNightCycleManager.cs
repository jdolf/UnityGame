using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycleManager : MonoBehaviour
{
    public ShopManager ShopManager;
    public Gradient lightColor;
    public GameObject worldLight;
    // default test 50
    private const float timePerDay = 20;
    private float timeOfTheDay = 10;

    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeOfTheDay > timePerDay)
        {
            timeOfTheDay = 0;
            ShopManager.GenerateNewItems();
            Debug.Log("Day completed");
        }

        timeOfTheDay += Time.deltaTime;
        worldLight.GetComponent<Light2D>().color = lightColor.Evaluate(timeOfTheDay / timePerDay);
    }

    private string ConvertToTime(float timeOfTheDay)
    {
        return string.Format("{0}h", Mathf.Floor(timeOfTheDay / timePerDay * 24));
    }
}
