using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

public class SaveUI : MonoBehaviour 
{
	public Transform _screenshot_parent;
	public GameObject _preview_prefab;

	void Start()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(SaveEntry));

		string[] paths = GetSaveGameNames();
		int i = 0;
		foreach (string path in paths)
		{
			StreamReader reader = new StreamReader(path);
			var save = (SaveEntry)serializer.Deserialize(reader);


			FileStream fs = new FileStream(save._screenshot_filename, FileMode.Open, FileAccess.Read);
			byte[] imageData = new byte[fs.Length];
			fs.Read(imageData, 0, (int)fs.Length);

			Texture2D texture = new Texture2D(4, 4);
			texture.LoadImage(imageData);

			GameObject go = (GameObject)GameObject.Instantiate(_preview_prefab);
			go.transform.parent = _screenshot_parent;
			go.transform.localScale = Vector3.one;
			//go.transform.localPosition = new Vector3(_screenshot_parent.GetComponent<UIWrapContent>().itemSize * i, 0, 0);
			//go.transform.GetChild(0).GetComponent<UITexture>().mainTexture = texture;
			go.GetComponent<SaveSlot>().SaveEntry = save;
			fs.Close();
			++i;

			reader.Close();
		}
	}

	string[] GetSaveGameNames()
	{
		return Directory.GetFiles(Application.persistentDataPath, "Sandplay_Save*");
	}

	public void StartLoad(SaveEntry a_Save)
	{
		SaveScene._save_game = a_Save;
		Application.LoadLevel( 1 );
	}

}
