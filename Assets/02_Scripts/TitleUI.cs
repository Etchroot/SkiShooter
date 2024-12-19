using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] Button loginButton;
    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject[] uiToAcivate;
    [SerializeField] TMP_InputField inputCodeName;
    [SerializeField] TMP_Text codenameText;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        string codename = inputCodeName.text;

        codenameText.text = codename;

        loginPanel.SetActive(false);

        foreach (var uiElement in uiToAcivate)
        {
            uiElement.SetActive(true);
        }
    }
}
