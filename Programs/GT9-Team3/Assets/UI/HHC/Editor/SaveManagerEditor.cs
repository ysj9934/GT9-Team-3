using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveManager sm = (SaveManager)target;

        EditorGUILayout.LabelField("Stage Clear Stars", EditorStyles.boldLabel);
        if (sm.data.stageClearStars != null)
        {
            foreach (var kvp in sm.data.stageClearStars)
            {
                EditorGUILayout.LabelField(
                    $"StageID: {kvp.Key}  Star: {kvp.Value}");
            }
        }
        else
        {
            EditorGUILayout.LabelField("No stage data");
        }
    }
}