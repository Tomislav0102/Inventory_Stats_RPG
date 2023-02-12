using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalItem : MonoBehaviour
{
    public virtual SO_items Item { get; set; }
    public virtual int StackNum { get; set; }
    public virtual int CurrentDurability { get; set; }
    public virtual void TakeItem(TotalItem sourceInv)
    {
        Item = sourceInv.Item;
        StackNum = sourceInv.StackNum;
        CurrentDurability = sourceInv.CurrentDurability;
    }
}
