using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TK.Enums;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(InvIndividual))]
public class CustomEditor_InvIndividual : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InvIndividual invI = (InvIndividual)target;
        EditorUtility.SetDirty(target);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("---CUSTOM EDITOR---");
        EditorGUILayout.Space();

        if (invI.slotType == InvSlotType.Equipment)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Equipment type", GUILayout.MaxWidth(130));
            invI.equipment = (EquipmentType)EditorGUILayout.EnumPopup(invI.equipment);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

        }

    }
}

[CustomEditor(typeof(EffectsTesting))]
public class CustomEditor_EffectsTesting: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EffectsTesting eff = (EffectsTesting)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("---CUSTOM EDITOR---");
        EditorGUILayout.Space();

        if(GUILayout.Button("Generate buff"))
        {
            eff.GenerateBuff();
        }

       

    }
}
