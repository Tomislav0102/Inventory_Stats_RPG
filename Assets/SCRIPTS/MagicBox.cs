using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;

namespace TK
{
    public static class InventoryUtilities
    {
       // public static System.Action FinishInvExchanges;
        public static System.Action<bool> MainMenuOpen;
        public static T[] GetChildByType<T>(Transform parentTransform)
        {
            T[] tempType = new T[parentTransform.childCount];
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                tempType[i] = parentTransform.GetChild(i).GetComponent<T>();
            }
            return tempType;
        }

        public static List<int> Stoh_IntSimple(int sizeOfList)
        {
            List<int> first = new List<int>();
            List<int> second = new List<int>();
            for (int i = 0; i < sizeOfList; i++)
            {
                first.Add(i);
            }
            int broj = first.Count;
            for (int i = 0; i < broj; i++)
            {
                int rdn = Random.Range(0, first.Count);
                second.Add(first[rdn]);
                first.RemoveAt(rdn);
            }
            return second;
        }

    }
    public class Effect
    {
        public string nameOfEffect;
        public Stats affectedStat;
        public int value;
    }


    namespace Enums
    {
        public enum PickUpType
        {
            MouseClick,
            WalkOver,
            WalkNear,
            WalkNearRange
        }
        public enum UseType
        {
            Heal,
            Mana,
            Gold,
            StatChange
        }
        public enum SpendableType
        {
            HitPoints,
            ManaPoints
        }
        public enum ItemType
        {
            Wearable,
            Consumable,
            PermanentUsage,
            UselessQuest
        }
        public enum EquipmentType
        {
            Weapon,
            Shield,
            Helmet,
            Armor,
            Boots,
            Rings
        }
        public enum StatType
        {
            Ability,
            Derivative
        }
        public enum Stats
        {
            Strength,
            Dextrerity,
            Constitution,
            Intelligence,
            Luck,
            Damage,
            ArmorClass,
            HitPoints,
            ManaPoints,
            NA //if bow is equiped, dexterity affects damage and strength shouldn't affect anything so its NA
        }
        public enum ValueResetType
        {
            MagicalItems,
            Buffs,
            PrecentageModifiers
        }
        public enum InvSlotType
        {
            Inventory,
            Equipment
        }
    }
}