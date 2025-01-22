using System.Collections;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private float duration = 5f; // 색 변화 시간

    private float initialEmissionValue; // 초기 G 값
    private bool isFading = false; // 현재 진행 중 확인 플래그

    private void Start()
    {
        if (targetMaterial != null)
        {
            initialEmissionValue = targetMaterial.GetColor("_EmissionColor").g;
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
            Color newColor = new Color(targetMaterial.GetColor("_EmissionColor").r, newValue, targetMaterial.GetColor("_EmissionColor").b);
            targetMaterial.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 복원 과정
        while (elapsedTime < duration * 2)
        {
            float newValue = Mathf.Lerp(0, initialEmissionValue, (elapsedTime - duration) / duration);
            Color newColor = new Color(targetMaterial.GetColor("_EmissionColor").r, newValue, targetMaterial.GetColor("_EmissionColor").b);
            targetMaterial.SetColor("_EmissionColor", newColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isFading = false;

    }


}
