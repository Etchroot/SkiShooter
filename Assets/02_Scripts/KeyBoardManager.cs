using UnityEngine;

public class KeyBoardManager : MonoBehaviour
{
    [SerializeField] private GameObject keyboardCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowKeyboard()
    {
        if (!keyboardCanvas.activeSelf)
        {
            keyboardCanvas.SetActive(true);
        }
    }
}
