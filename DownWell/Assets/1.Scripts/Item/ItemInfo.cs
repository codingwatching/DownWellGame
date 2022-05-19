using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item/Item")]
public class ItemInfo : LevelObject
{
    [Header("Drop")]
    public ItemDroppingSetting dropSetting;

    [Header("Item")]
    public float chacePercent;

    public virtual void PickUp()
    {

    }
}
