using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class QuestionLogic : MonoBehaviour
{
	public GameObject questionText;
	public GameObject neutralButton;
	public GameObject negativeButton;
	public GameObject positiveButton;
	public GameObject inputBox;
	public Questions questionEnable;

	public Animator libraryOpen;
	public string libaryPressed;
	public string libaryDisabled;

	//public Button questionButtonlibrary;
	
	public ObjectSpawner mainScript;
	
	string[] contentText = 
	{
		"Think of something that is happening in your life that is bothering you, this can be a feeling, a situation or an individual.",
		"Once you have the issue in mind, choose a piece that you are most drawn to and that can represent you and your feelings in this situation.",
		"Think about it and describe what attracted you to the piece and how this piece represents you.",
		"",
		"Now choose a second piece that you are drawn to that represents the issue.",
		"Tune into and connect with the feeling of the first piece.",
		"As that first piece, what do you want to say to that second piece about the issue?",
		"",
		"From your feelings, what or who does this remind you of when you were young?",
		"",
		"What do you want to say to that person or situation?",
		"",
		"What decision did you make at this time regarding the situation?",
		"",
		"Have a think about how this relates to the current issue.",
		"",
		"Choose a piece that can handle the situation.",
		"Share your sandbox.",
		"Summary of session",
		"*** Links to social media, counselling services and websites***"
	};
	
	string[] negativeText = 
	{
		"Share your sandbox for interpretation or advice.",
		"Does this help clarify?",
		"What would happen if you said Yes?",
		"What is the benefit in you saying I don't know/No?",
		"Now that you know there is a benefit to you in answering yes would you like to go back to the beginning of the previous question and try again?",
		"*** Links to social media, counselling services and websites***"
	};
	
	string log;
	
	int contentIndex;
	int negativeIndex;
	
	bool negativeQuestions;
	
	public void UpdateContent () 
	{
		if(contentIndex == 1 || contentIndex == 4 ||  contentIndex == 16)
		{
			if(mainScript.CurrentState != SPAWN_STATE.NOT_SPAWNING)
			{
				contentIndex++;
			}
		}
		
		if(contentIndex == 3 || contentIndex == 7 ||  contentIndex == 9 ||  contentIndex == 11 ||  contentIndex == 13 ||  contentIndex == 15)
		{
			inputBox.SetActive(true);
			inputBox.GetComponentInChildren<Text>().text = "You can type here.";
		}
		else
		{
			inputBox.SetActive(false);
		}
	
		UpdateText();
		UpdateButtons();
	}
	
	void UpdateText () 
	{
		questionText.SetActive(true);
		
		if(negativeQuestions)
		{
			if(negativeIndex < negativeText.Length)
			{
				questionText.GetComponent<Text>().text = negativeText[negativeIndex];
			}
			
		}
		else
		{
			if(contentIndex < contentText.Length)
			{
				questionText.GetComponent<Text>().text = contentText[contentIndex];
			}
		}	
	}
	
	void UpdateButtons () 
	{
		neutralButton.SetActive(false);
		positiveButton.SetActive(false);
		negativeButton.SetActive(false);
		
		if(negativeQuestions)
		{
			switch(negativeIndex)
			{
			case 0:
				neutralButton.SetActive(true);
				neutralButton.GetComponentInChildren<Text>().text = "Continue";
				break;	
			case 1:
				positiveButton.SetActive(true);
				negativeButton.SetActive(true);
				positiveButton.GetComponentInChildren<Text>().text = "Yes";
				negativeButton.GetComponentInChildren<Text>().text = "No";
				break;
			case 4:
				goto case 1;
			default:
				goto case 0;
			}
			
		}
		else
		{
			switch(contentIndex)
			{
				case 0:
					neutralButton.SetActive(true);
					neutralButton.GetComponentInChildren<Text>().text = "Continue";
					break;
				case 1:
					neutralButton.SetActive(true);
					neutralButton.GetComponentInChildren<Text>().text = "Open Library";
					//if(questionButtonlibrary.interactable == true)
					//{
						libraryOpen.Play(libaryPressed);
					//}
					break;
				case 2:
					neutralButton.SetActive(true);
					neutralButton.GetComponentInChildren<Text>().text = "Play Video";
					libraryOpen.Play(libaryDisabled);
					break;
				case 4:
					goto case 1;
				case 6:
					positiveButton.SetActive(true);
					negativeButton.SetActive(true);
					positiveButton.GetComponentInChildren<Text>().text = "Continue";
					negativeButton.GetComponentInChildren<Text>().text = "I don't know";
					break;
				case 8:
					goto case 6;
				case 10:
					goto case 6;
				case 12:
					goto case 6;
				case 14:
					goto case 6;
				case 16:
					goto case 6;
				case 20:
					WriteLog();
					Application.LoadLevel(0);
					break;
				default:
					goto case 0;
			}
		}
	}
	
	public void NeutralAction () 
	{
		if(negativeQuestions)
		{
			switch(negativeIndex)
			{
				case 0:
					questionEnable.ShowQuestion();
					negativeIndex++;
					break;
				case 2:
					negativeIndex++;
					UpdateContent();
					break;
				case 5:
					WriteLog();
					Application.LoadLevel(0);
					break;
				default:
					goto case 2;				
			}
			
		}
		else
		{
			switch(contentIndex)
			{
				case 0:
					NextContent();
					break;
				case 1:
					questionEnable.ShowQuestion();
					//questionEnable.Tween();
					break;
				case 4:
					goto case 1;
				default:
					goto case 0;
			}
		}
	}
	
	public void PositiveAction () 
	{
		if(negativeQuestions)
		{
			negativeQuestions = false;
			UpdateContent();
		}
		else
		{
			switch(contentIndex)
			{
				case 0:
					NextContent();
					break;
				case 16:
					questionEnable.ShowQuestion();
					//questionEnable.Tween();
					break;
			
				default:
					goto case 0;
			}
		}
	}
	
	public void NegativeAction () 
	{
		if(negativeQuestions)
		{
			negativeIndex++;
			UpdateContent();
		}
		else
		{
			negativeQuestions = true;
			negativeIndex = 0;
			UpdateContent();
		}
	}
	
	public void NextContent () 
	{
		if(contentIndex == 3 || contentIndex == 7 ||  contentIndex == 9 ||  contentIndex == 11 ||  contentIndex == 13 ||  contentIndex == 15)
		{
			log += " " + contentIndex + ". " + inputBox.GetComponentInChildren<Text>().text;
		}
		contentIndex++;
		UpdateContent();
	}
	
	void OnSubmit () 
	{
		NextContent ();
	}
	
	public void WriteLog () 
	{
		//string path = Application.dataPath + "/SandplayLog.txt";
		//File.WriteAllText(path, log);
	}
}