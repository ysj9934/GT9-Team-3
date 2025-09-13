using UnityEngine;
using UnityEditor; // 에디터 전용 네임스페이스

[CustomEditor(typeof(ResourceManager))] // ResourceManager의 inspcetor를 커스터마이징 가능
public class ResourceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 기본 Inspector 먼저 표시
        base.OnInspectorGUI();

        // 대상 스크립트 가져오기
        ResourceManager rm = (ResourceManager)target;

        EditorGUILayout.Space();    //세로 여백
        EditorGUILayout.LabelField("=== 디버그용 자원 표시 ===", EditorStyles.boldLabel);

        if (rm.resources != null)
        {
            foreach (var kvp in rm.resources)
            {
                EditorGUILayout.LabelField($"{kvp.Key}: {kvp.Value}");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Resource Dictionary 가 초기화되지 않았습니다.", MessageType.Warning);
        }
    }
}