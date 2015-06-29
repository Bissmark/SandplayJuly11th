using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenshotBar : MonoBehaviour 
{
	[SerializeField]
	private GameObject screenshotPanel = null;
	public GameObject ScreenshotPanel { get { return screenshotPanel; } }
	
	[SerializeField]
	private InputField screenshotInput = null;
	public InputField ScreenshotInput { get { return screenshotInput; } }
	
	private void Awake()
	{
		// checking for errors
		DebugUtils.Assert( screenshotPanel != null, "Check if save panel is hooked up" );
		DebugUtils.Assert( screenshotInput != null, "Check if save input is hooked up" );
	}

	public GameObject buttonScreenshot;
	public GameObject buttonInput;

	// Use this for initialization
	public void OnClick()
	{
		if (buttonScreenshot.activeSelf == false) 
		{
			buttonInput.SetActive(true);
			buttonScreenshot.SetActive (true);
		} 
		else 
		{
			buttonScreenshot.SetActive(false);
			buttonInput.SetActive(false);
		}
	}
}