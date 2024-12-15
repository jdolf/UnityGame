using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using static GameData;

public class GameDataManager : MonoBehaviour
{
    public TaskManager TaskManager;
    public CrewManager CrewManager;
    public ShopManager ShopManager;
    public SkillManager SkillManager;
    public InventoryManager InventoryManager;
    public EquipmentManager EquipmentManager;
    public CraftingScreenManager CraftingScreenManager;
    public static GameData GameData { get; private set; }
    private static Dictionary<string, List<AttributeListener>> AttributeListenerPairs = new Dictionary<string, List<AttributeListener>>();

    public static void RegisterAttributeListener(string attributeName, AttributeListener listener)
    {
        // Check if registration under a specific attribute key already exists, if yes, then add extra listeners to it
        if (AttributeListenerPairs.ContainsKey(attributeName)) {
            AttributeListenerPairs[attributeName].Add(listener);
        } else {
            // Else create a new list first
            List<AttributeListener> list = new List<AttributeListener>();
            list.Add(listener);
            AttributeListenerPairs.Add(attributeName, list);
        }
    }

    public static void SaveJsonData(SavableGameData savable)
    {
        savable.PopulateGameData(GameData);
    }
    
    public static void LoadJsonData(SavableGameData savable)
    {
        if (FileManager.LoadFromFile("GameData.dat", out var json))
        {
            GameData gd = new GameData();
            gd.LoadFromJson(json);

            savable.LoadFromGameData(gd);
            
            Debug.Log("Load complete");
        }
    }

    public static void NotifyAttributeListeners(string attributeName, int value)
    {
        foreach (var item in AttributeListenerPairs)
        {
            Debug.Log(item.Key);
        }
        Debug.Log(attributeName);
        if (AttributeListenerPairs.ContainsKey(attributeName)) {
            foreach (AttributeListener listener in AttributeListenerPairs[attributeName]) {
                listener.AttributeChanged(value);
            }
        }
    }

    // Mainly for tasks
    public static void NotifyAllAttributeListeners()
    {
        foreach (KeyValuePair<string, List<AttributeListener>> listenerDictionary in AttributeListenerPairs)
        {
            foreach (AttributeListener listener in listenerDictionary.Value) {
                listener.AttributeChanged(GameData.GetIntValue(listenerDictionary.Key, AccessMode.PascalCase));
            }
        }
    }

    private static void LoadJsonDataFull()
    {
        if (GameData == null) {
            GameData = new GameData();
        }

        if (FileManager.LoadFromFile("GameData.dat", out var json))
        {
            GameData gd = new GameData();
            gd.LoadFromJson(json);
            GameData = gd;
        }
    }

    public void SaveAllGameData()
    {
        TaskManager.Save(GameData);
        CrewManager.Save(GameData);
        ShopManager.Save(GameData);
        SkillManager.Save(GameData);
        InventoryManager.Save(GameData);
        EquipmentManager.Save(GameData);
        CraftingScreenManager.Save(GameData);

        if (FileManager.WriteToFile("GameData.dat", GameData.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }

    public void LoadAllGameData()
    {
        TaskManager.Load(GameData);
        CrewManager.Load(GameData);
        ShopManager.Load(GameData);
        SkillManager.Load(GameData);
        InventoryManager.Load(GameData);
        EquipmentManager.Load(GameData);
        CraftingScreenManager.Load(GameData);
        // If some tasks aren't saved or are new, update data from attribute game data
        NotifyAllAttributeListeners();
    }

    // First thing is to load the json into memory (GameData)
    public void Awake()
    {
        LoadJsonDataFull();
    }

    // Then insert these values into objects that try to load it
    void Start()
    {
        LoadAllGameData();
    }
}