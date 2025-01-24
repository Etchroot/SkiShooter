using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// inspector에 redscreen 활성화 버튼 추가. 로직 작동 확인용
[CustomEditor(typeof(RedScreen))]
public class RedScreenEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RedScreen myTarget = (RedScreen)target;

        if (GUILayout.Button("레드스크린 작동"))
        {
            myTarget.TriggerRedScreenEffect();
        }
    }
}
public class RedScreen : MonoBehaviour
{
    public Image redScreenImage;

    void Start()
    {
        //redScreenImage = GetComponent<Image>();
        redScreenImage.enabled = false;

    }

    public void TriggerRedScreenEffect()
    {
        Debug.Log("레드 스크린 띄움");
        redScreenImage.enabled = true;
        Invoke(nameof(HideRedScreen), 0.4f);
    }

    private void HideRedScreen()
    {
        redScreenImage.enabled = false;
    }
}
