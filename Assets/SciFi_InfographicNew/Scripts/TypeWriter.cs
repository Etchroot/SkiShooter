using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
	Text txt;
	string story;
	public float delay = 0.125f;
	public bool _continue = false;
	void Awake()
	{
		txt = GetComponent<Text>();
		story = txt.text;
		txt.text = "";

		Invoke("StartAnimation", 2f);
	}
   
    void StartAnimation()
    {
		
		StartCoroutine("PlayText");
	}
	IEnumerator PlayText()
	{
		foreach (char c in story)
		{
			txt.text += c;
			yield return new WaitForSeconds(delay);
		}
		if(_continue == true)
        {
			// Pause for a moment when the story is complete
			yield return new WaitForSeconds(1.0f);

			// Remove the story
			txt.text = "";

			// Pause for a moment before typing it again
			yield return new WaitForSeconds(1.0f);

			// Start typing the story again
			StartCoroutine("PlayText");
		}
	
	}
}
