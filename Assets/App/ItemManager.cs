using UnityEngine;
using UnityEditor;

namespace ItemManager
{
    [CustomEditor(typeof(BombItem))]
    public class BombItemEditor : Editor
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

    [CustomEditor(typeof(SpeedUpItem))]
    public class SpeedUpItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SpeedUpItem speedUpItem = (SpeedUpItem)target;
            if (GUILayout.Button("가속 활성화"))
            {
                speedUpItem.ActivateEffect();
            }
        }
    }
}

