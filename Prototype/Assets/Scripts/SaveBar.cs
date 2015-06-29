using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaveBar : MonoBehaviour 
{
	[SerializeField]
	private GameObject savePanel = null;
	public GameObject SavePanel { get { return savePanel; } }
	
	[SerializeField]
	private InputField saveInput = null;
	public InputField SaveInput { get { return saveInput; } }
	
	private void Awake()
	{
		// checking for errors
		DebugUtils.Assert( savePanel != null, "Check if save panel is hooked up" );
		DebugUtils.Assert( saveInput != null, "Check if save input is hooked up" );
	}

	public GameObject buttonSave;
	public GameObject buttonInput;

	// Use this for initialization
	public void OnClick()
	{
		if (buttonSave.activeSelf == false) 
		{
			buttonInput.SetActive(true);
			buttonSave.SetActive (true);
		} 
		else 
		{
			buttonSave.SetActive(false);
			buttonInput.SetActive(false);
		}
	}
}