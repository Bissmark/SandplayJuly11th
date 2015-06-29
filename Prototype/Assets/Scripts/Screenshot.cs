using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Screenshot : MonoBehaviour
{
	[SerializeField]
    private string folderOrigin;

	public GameObject buttonScreenshot;
	public GameObject buttonInput;

    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
        {
			folderOrigin = Application.dataPath + "/StreamingAssets/";
        }
		else if ( Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
		{
			folderOrigin = Application.persistentDataPath + "/Screenshots/";
		}
		else
		{
			folderOrigin = Application.dataPath + "/Screenshots/";
		}
		System.IO.Directory.CreateDirectory(folderOrigin);
    }

	public void OnClickScreenshotButton()
	{
		SaveScreenshot( this.folderOrigin, GetComponent<ScreenshotBar>().ScreenshotInput.text );
		GetComponent<ScreenshotBar>().ScreenshotPanel.SetActive( false );
	}
	
	public string TakeScreenShot( string folder, string fileName )
	{
		return SaveScreenshot( folder, fileName );
	}

	private string SaveScreenshot(string folder, string fileName)
    {
		string currentDT = DateTime.Now.ToString( "yyyy-MM-dd HH.mm.ss" );
		string filePath = folder + fileName + " " + currentDT + ".png";

		Application.CaptureScreenshot( filePath );
		// TODO: this method is not going to work for the web.
		// Task 1: Capture screenshot using RenderTexture(web only)
		// Task 2: Upload it to the server(web only)

		return filePath;
    }
}