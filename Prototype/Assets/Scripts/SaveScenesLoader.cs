using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class SaveScenesLoader : MonoBehaviour
{
	[SerializeField]
	private GameObject saveSlotPrefab = null;
	
	[SerializeField]
	private Transform parent = null;
	
	private List<SaveEntry> saveEntries = null;

	private void Awake()
	{
		DebugUtils.Assert( saveSlotPrefab != null, "Check if save slot prefab is linked " );
		DebugUtils.Assert( parent != null, "Check if save parent is linked " );
	}

	private void Start()
	{
		saveEntries = MenuLogic.Instance.GetComponent<LoadScenes>().SaveEntries;
		
		if ( saveEntries != null && saveEntries.Count > 0 )
		{
			LoadSaveSlotImages();
		}
	}
	
	public void LoadSaveSlotImages()
	{
		for ( int i = 0; i < saveEntries.Count; i++ )
		{
			// instantiate prefab
			GameObject go = Instantiate( saveSlotPrefab ) as GameObject;
			
			// pass on the information
			go.GetComponent<SaveSlot>().SaveEntry = saveEntries[ i ];
			
			// set positioning and scale
			go.transform.SetParent( parent );
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3( parent.GetComponent<HorizontalLayoutGroup>().spacing * i, 0, 0 );
			
			
			// cashing for ease of use
			SaveSlot sv = go.GetComponent<SaveSlot>();
			
			// populating with data
			//Debug.Log (sv.Background.texture);
			sv.Background.texture = MenuLogic.Instance.GetComponent<LoadScenes>().GetTexture( saveEntries[ i ] );
			sv.SaveName.text = saveEntries[ i ]._saveName;
			sv.SaveDate.text = saveEntries[ i ]._saveTime.ToShortDateString();
		}
	}
}