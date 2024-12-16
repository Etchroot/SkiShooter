using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Counter : MonoBehaviour
{
    Text txt;
    string numbers;
    int num = 0;
    public
        string[] count;
    // Start is called before the first frame update
    void Awake()
    {
        txt = GetComponent<Text>();
        numbers = txt.text;
        txt.text = "";
        num = Random.Range(0, count.Length);
        // TODO: add optional delay when to start
        Invoke("StartAnim", 2f);
    }
    void StartAnim()
    {
        StartCoroutine("PlayNumber");
    }
    IEnumerator PlayNumber()
    {
        txt.text = count[num].ToString();
        if (num < 4)
        {
            num++;
        }
        else
        {
            num = 0;
        }
     
        yield return new WaitForSeconds(0.125f);
        StartCoroutine("PlayNumber");
    }
}
