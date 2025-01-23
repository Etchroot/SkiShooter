using System.Collections;
using UnityEditor;
using UnityEngine;


public class EmissionController : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private float duration = 5f; // 색 변화 시간

    private float initialEmissionValue = 255f / 255f; // 초기 G 값
    private float emissionR = 255f / 255f;
    private float emissionB = 0f / 255f;
    private bool isFading = false; // 현재 진행 중 확인 플래그

    private void Start()
    {
        if (targetMaterial != null)
        {
            Color initialColor = new Color(emissionR, initialEmissionValue, emissionB);
            targetMaterial.SetColor("_EmissionColor", initialColor);
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

        targetMaterial.SetColor("_EmissionColor", new Color(emissionR, 0, emissionB));

        // 복원 과정
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float newValue = Mathf.Lerp(0, initialEmissionValue, elapsedTime / duration);
            Color newColor = new Color(emissionR, newValue, emissionB);
            targetMaterial.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetMaterial.SetColor("_EmissionColor", new Color(emissionR, initialEmissionValue, emissionB));

        isFading = false;

    }

    public void RestoreInitialEmission()
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_EmissionColor", new Color(emissionR, initialEmissionValue, emissionB));
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