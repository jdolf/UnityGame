using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    protected static List<Trait> allTraits;
    public GameObject UITrait;
    public Transform TraitsParent;
    [HideInInspector]
    public List<Trait> Traits = new List<Trait>();
    private StatsChangedListener statsListener;

    protected static void LoadAllTraits() {
        if (allTraits == null) {
            allTraits = new List<Trait>(Resources.LoadAll<Trait>("ScriptableObjects/Traits"));
        }
    }

    protected static Trait FetchRandomTraitByRarity(Rarity.Classification rarity, List<Trait> existingTraits) {
        // List might contain duplicates
        List<Trait.FamilyClassification> disallowedFamilies = new List<Trait.FamilyClassification>();
        existingTraits.FindAll(element => element.Family != Trait.FamilyClassification.None).ForEach(element => disallowedFamilies.Add(element.Family));

        List<Trait> fetchedTraits = allTraits.FindAll(element => element.Rarity == rarity && !disallowedFamilies.Contains(element.Family));

        if (fetchedTraits.Count == 0) {
            throw new System.Exception("No trait of rarity " + rarity.ToString() + " found");
        }

        return fetchedTraits[Random.Range(0, fetchedTraits.Count)];
    }

    void Awake()
    {
        statsListener = transform.GetComponent<PlayerStatsManager>();
        LoadAllTraits();
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize() {
        TryClearTraits();
        GenerateTraits();
        DisplayTraits();
    }

    protected void GenerateTraits() {
        Traits.Clear();
        int totalWeight = 0;
        List<Rarity> allRarities = Rarity.GetAllRarities();
        allRarities.ForEach(e => totalWeight += e.TraitWeight);

        for (int i = 0; i < 3; i++) {
            int usedWeight = 0;
            int randomNumber = Random.Range(1, totalWeight);
            Debug.Log(randomNumber);

            foreach (Rarity rarity in allRarities) {
                if (randomNumber <= rarity.TraitWeight + usedWeight) {
                    Traits.Add(FetchRandomTraitByRarity(rarity.Identifier, Traits));
                    break;
                }
                usedWeight += rarity.TraitWeight;
            }
        }
        statsListener.StatsChanged();
    }

    protected void DisplayTraits() {
        foreach (Trait trait in Traits) {
            Rarity rarity = Rarity.GetRarityByClassification(trait.Rarity);
            UITrait uiTrait = Instantiate(this.UITrait, Vector3.zero, Quaternion.identity).GetComponent<UITrait>();
            // HiddenName and Name need to be the same text, as hiddenname stretches out the panel to the correct width
            uiTrait.Trait = trait;
            uiTrait.HiddenName.SetText(trait.Name);
            uiTrait.Name.SetText(trait.Name);

            //uiTrait.Name.color = rarity.SecondaryColor;
            uiTrait.Background.color = rarity.PrimaryColor;
            uiTrait.Name.color = rarity.SecondaryColor;

            uiTrait.transform.SetParent(TraitsParent);
            uiTrait.transform.localScale = Vector3.one;
        }
    }

    public void TryClearTraits() {
        foreach (Transform child in TraitsParent) {
            Destroy(child.gameObject);
        }

        Traits.Clear();
    }

    public PlayerStats CalculateStats() {
        PlayerStats newStats = new PlayerStats();

        foreach (Trait trait in Traits) {
            newStats = PlayerStats.Add(trait.AffectedStats, newStats);
        }

        return newStats;
    }

}
