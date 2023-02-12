using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(SO_items))]
public class CustomEditor_Item : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SO_items items = (SO_items)target;
        EditorUtility.SetDirty(target);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("---CUSTOM EDITOR---");
        EditorGUILayout.Space();

        if (items.itemType == ItemType.Wearable)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Equipment type", GUILayout.MaxWidth(130));
            items.equipType = (EquipmentType)EditorGUILayout.EnumPopup(items.equipType);
            EditorGUILayout.EndHorizontal();


            if (items.equipType == EquipmentType.Weapon)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Damage", GUILayout.MaxWidth(80));
                items.damage = EditorGUILayout.IntField(items.damage);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }
            else if(items.equipType != EquipmentType.Rings)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Armor Class Bonus", GUILayout.MaxWidth(160));
                items.ACbonus = EditorGUILayout.IntField(items.ACbonus);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

            }
            EditorGUILayout.Space();

        }

        if (items.itemType == ItemType.Consumable || items.itemType == ItemType.PermanentUsage)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Use Type", GUILayout.MaxWidth(80));
            items.useType = (UseType)EditorGUILayout.EnumPopup(items.useType);
            EditorGUILayout.EndHorizontal();

            if(items.useType != UseType.StatChange)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Use Ammout", GUILayout.MaxWidth(100));
                items.useAmmout = EditorGUILayout.IntField(items.useAmmout);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }


        if (items.itemType == ItemType.Consumable)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stackable", GUILayout.MaxWidth(80));
            items.stackable = EditorGUILayout.Toggle(items.stackable);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stack capacity (999 is infinte)", GUILayout.MaxWidth(200));
            items.stackMax = EditorGUILayout.IntField(items.stackMax);
            EditorGUILayout.EndHorizontal();

            if(items.useType == UseType.StatChange)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Affected stat", GUILayout.MaxWidth(100));
                items.statToChange = (Stats)EditorGUILayout.EnumPopup(items.statToChange);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Incresed value", GUILayout.MaxWidth(100));
                items.increaseValue = EditorGUILayout.IntField(items.increaseValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Incresed percentage", GUILayout.MaxWidth(140));
                items.increasePercentage = EditorGUILayout.FloatField(items.increasePercentage);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ramp up time", GUILayout.MaxWidth(100));
                items.rampUpTime = EditorGUILayout.FloatField(items.rampUpTime);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Duration of buff", GUILayout.MaxWidth(100));
                items.buffTime = EditorGUILayout.FloatField(items.buffTime);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Has tick behaviour (only HP or MP)", GUILayout.MaxWidth(215));
                items.hasTickBehaviour = EditorGUILayout.Toggle(items.hasTickBehaviour);
                EditorGUILayout.EndHorizontal();
                if (items.hasTickBehaviour)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Tick rate of fire", GUILayout.MaxWidth(150));
                    items.tickRateOfFire = EditorGUILayout.IntField(items.tickRateOfFire);
                    EditorGUILayout.EndHorizontal();
                }

            }

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
        }


        
    }
}
#endif 
