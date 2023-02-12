using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;

[CreateAssetMenu]
public class SO_items : ScriptableObject
{
    public string Name;
    [TextArea(1, 10)]
    public string Description;
    public Sprite Sprite;
    public ItemType itemType;
    public int durablity;

    //wearable
    [HideInInspector] public EquipmentType equipType;

    //weapon
    [HideInInspector] public int damage;
    //other wearable
    [HideInInspector] public int ACbonus;

    //consumable
    [HideInInspector] public bool stackable;
    [HideInInspector] public int stackMax; //999 infinite capacity
    //if UseType == StatChange
    [HideInInspector] public Stats statToChange;
    [HideInInspector] public int increaseValue;
    [HideInInspector] public float increasePercentage = 1f;
    [HideInInspector] public float rampUpTime;
    [HideInInspector] public float buffTime;
    [HideInInspector] public bool hasTickBehaviour;
    [HideInInspector] public int tickRateOfFire;

    //permanentUse, consumable
    [HideInInspector] public UseType useType;
    [HideInInspector] public int useAmmout;

    public ExtraBonus[] extraBonuses;
    [System.Serializable]
    public struct ExtraBonus
    {
        public Stats statsBonus;
        public int amount;
    }

}
