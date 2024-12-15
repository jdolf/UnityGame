using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackground : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!UICursorItem.Empty) {
            ItemStack itemStackToDrop = UICursorItem.Instance.ItemStackToDisplay;
            DropManager.CreateDrops(itemStackToDrop, null, DropManager.DropDirection.PlayerFacing, 10f);
            UICursorItem.TryRemoveItemAmount(itemStackToDrop.Item, itemStackToDrop.Amount);
        }
        
        Debug.Log("background clickedd");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
