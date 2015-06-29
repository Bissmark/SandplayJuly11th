using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Questions : MonoBehaviour 
{
	private bool questioning = false;
	public GameObject questionBackground;
	public GameObject questionButtonText;
	public QuestionLogic questionLogic;

	public void ShowQuestion() 
	{
		if(!questioning)
		{
			questionLogic.UpdateContent();
			questionBackground.SetActive(true);
			questionButtonText.GetComponent<Text>().text = "Welcome to Sandplay - If you are new to Sandplay, Please complete the training before continuing";
			questioning = true;
		}
		else
		{
			questionBackground.SetActive(false);
			questionButtonText.SetActive(false);
			questionButtonText.GetComponent<Text>().text = "Show Question";
			questioning = false;
		}
	}
}