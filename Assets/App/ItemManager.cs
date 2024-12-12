using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BombItem))]
public class ItemManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BombItem bomb = (BombItem)target;

        if (GUILayout.Button("폭탄 활성화"))
        {
            bomb.ActivateEffect();
        }
    }
}
