using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public const float FoodDecay = 0.1f;
    public const int FoodDecayDelay = 50;
    private int FoodDecayDelayCounter = 0;
    public UIFoodBar UIBar;
    protected FoodListener listener;
    public float Food { get; set; } = 75;
    public float MaxFood { get; set; } = 100;

    void Awake()
    {
        listener = UIBar;
    }

    void Start()
    {
        InformListeners();
    }

    public void RemoveFood(int food)
    {
        this.Food -= food;

        if (this.Food < 0) {
            this.Food = 0;
        }

        InformListeners();
    }

    public void AddFood(int food)
    {
        this.Food += food;

        if (this.Food > this.MaxFood) {
            this.Food = this.MaxFood;
        }

        InformListeners();
    }

    private void InformListeners()
    {
        // 0.2, 0.6 etc should be displayed as 1, since there is still food left
        int formattedFood = (int) Math.Ceiling((double)this.Food);
        int formattedMaxFood = (int) Math.Ceiling((double)this.MaxFood);
        listener.FoodChanged(formattedFood, formattedMaxFood);
    }

    void FixedUpdate()
    {
        FoodDecayDelayCounter++;

        if (FoodDecayDelayCounter >= FoodDecayDelay) {
            FoodDecayDelayCounter = 0;

            float prevFoodAmount = this.Food;
            this.Food -= FoodDecay;

            if (this.Food < 0) {
                this.Food = 0;
                InformListeners();
            }

            // Only update if full integer changed, for performance reasons
            if (Math.Ceiling((double)prevFoodAmount) != Math.Ceiling((double)this.Food)) {
                Debug.Log(prevFoodAmount + "   " + this.Food);
                InformListeners();
            }
        }
    }
}
