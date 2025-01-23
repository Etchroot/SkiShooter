using System.Collections;
using UnityEditor;
using UnityEngine;


public class EmissionController : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private float duration = 5f; // 색 변화 시간

    private float initialEmissionValue = 43f; // 초기 G 값
    private float emissionR;
    private float emissionB;
    private bool isFading = false; // 현재 진행 중 확인 플래그

    private void Start()
    {
        if (targetMaterial != null)
        {
            Color initialColor = targetMaterial.GetColor("_EmissionColor");
            emissionR = initialColor.r;
            emissionB = initialColor.b;
        }

    }
    public void WarnningLight()
    {
        InvokeRepeating(nameof(FadeEmission), 0f, 5f);
    }

    public void FadeEmission()
    {
        if (!isFading && targetMaterial != null)
        {
            isFading = true;
            StartCoroutine(FadeCoroutine());
        }
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newValue = Mathf.Lerp(initialEmissionValue, 0, elapsedTime / duration);
            Color newColor = new Color(emissionR, newValue, emissionB);
            targetMaterial.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 복원 과정
        while (elapsedTime < duration * 2)
        {
            float newValue = Mathf.Lerp(0, initialEmissionValue, (elapsedTime - duration) / duration);
            Color newColor = new Color(emissionR, newValue, emissionB);
            targetMaterial.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isFading = false;

    }

    public void RestoreInitialEmission()
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_EmissionColor", new Color(targetMaterial.GetColor("_EmissionColor").r, initialEmissionValue, targetMaterial.GetColor("_EmissionColor").b));
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(EmissionController))]
    public class EmissionControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EmissionController myScript = (EmissionController)target;

            if (GUILayout.Button("복구"))
            {
                myScript.RestoreInitialEmission();
            }

        }
    }
#endif



}