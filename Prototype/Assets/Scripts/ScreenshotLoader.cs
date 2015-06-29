using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class ScreenshotLoader : MonoBehaviour 
{

	public Transform _screenshot_parent;
	public GameObject _preview_prefab;
	public GameObject screenshotHelp;

	public Screenshot test;

	private List<string> _imageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

	// Use this for initialization
	void Start () 
	{
		LoadScreens();
	}

	string[] GetScreenShotNames()
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.OSXWebPlayer) 
		{
			return Directory.GetFiles (Application.dataPath + "/StreamingAssets/");
		} 
		else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
		{
			return Directory.GetFiles (Application.persistentDataPath + "/Screenshots");
		}
		else
		{
			return Directory.GetFiles (Application.dataPath + "/Screenshots");
		}
	}

	public void LoadScreens()
	{
		string[] filenames = GetScreenShotNames();

		if (filenames.Length == 0)
			return;

		int i = 0;
		foreach (string s in filenames)
		{
			if (!_imageExtensions.Contains( Path.GetExtension( s ).ToUpperInvariant() ) )
			{
				continue;
			}
			FileStream fs = new FileStream(s, FileMode.Open, FileAccess.Read);
			byte[] imageData = new byte[fs.Length];
			fs.Read(imageData, 0, (int)fs.Length);

			Texture2D texture = new Texture2D(4, 4);
			texture.LoadImage(imageData);

			GameObject go = Instantiate (_preview_prefab) as GameObject;
			go.transform.SetParent(_screenshot_parent);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3(_screenshot_parent.GetComponent<HorizontalLayoutGroup>().spacing * i, 0, 0);

			//RawImage ri = new GameObject("Image").AddComponent<RawImage>();
			//ri.texture = texture;

			go.transform.GetChild(0).GetComponent<RawImage>().texture = texture;
			fs.Close();
			++i;
		}
	}
}
