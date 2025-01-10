using UnityEngine;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour
{
    public Text targetText; // 변경할 텍스트
    private Color hoverColor = new Color(0.513f, 0.549f, 1.0f); // 마우스가 올라갔을 때 색상
    private Color originalColor; // 원래 색상

    void Start()
    {
        if (targetText != null)
        {
            originalColor = targetText.color; // 원래 색상 저장
        }
    }

    public void OnPointerEnter()
    {
        if (targetText != null)
        {
            targetText.color = hoverColor;
        }
    }

    public void OnPointerExit()
    {
        if (targetText != null)
        {
            targetText.color = originalColor;
        }
    }
}
