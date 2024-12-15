using UnityEngine;

[CreateAssetMenu(fileName = "New Identifiable Scriptable Object", menuName = "Identifiable Scriptable Object")]
public class IdentifiableScriptableObject : ScriptableObject
{
    // How this object should be referenced as (camelCase; i.e. willowTree)
    public string ID;
    // A broad classification of this object
    public IdentificationType Type = IdentificationType.None;
}

public enum IdentificationType {
        None = 0,
        Tree = 1,
        Rock = 2,
        Item = 3,
        Trait = 4,
        Shop = 5,
        Skill = 6,
        Plant = 7
}
