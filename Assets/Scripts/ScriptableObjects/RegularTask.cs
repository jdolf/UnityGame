using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Regular Task", menuName = "Task/Regular Task")]
public class RegularTask : Task
{
    // The Attribute from GameData that should be tracked
    public string RelevantGameDataAttribute;

    public void OnEnable()
    {
        GameDataManager.RegisterAttributeListener(RelevantGameDataAttribute, this);
    }
    public override void AttributeChanged(int value)
    {
        Debug.Log("--- attribute change check logic ---");
        // Only change if value different
        if (value != Progress) {
            Debug.Log(value + "   " + Progress);
            this.SetProgress(value);
        }
    }
}
