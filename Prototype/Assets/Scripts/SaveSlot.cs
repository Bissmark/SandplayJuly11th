using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SaveSlot : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private RawImage background = null;
	public RawImage Background { get { return background; } }
	
	[SerializeField]
	private Text saveName = null;
	public Text SaveName { get { return saveName; } }
	
	[SerializeField]
	private Text saveDate = null;
	public Text SaveDate { get { return saveDate; } }
	
	private SaveEntry saveEntry = null;
	public SaveEntry SaveEntry { get { return saveEntry; } set { saveEntry = value; } }
	
	/// <summary>
	/// Detects double click and loads the scene
	/// </summary>
	public void OnPointerClick( PointerEventData eventData )
	{
		if ( eventData.clickCount == 2)
		{
			// check for errors
			DebugUtils.Assert( saveEntry != null, "Save entry is not initialized" );
			DebugUtils.Assert( saveName != null, "Save name is not initialized" );
			DebugUtils.Assert( saveDate != null, "Save date is not initialized" );
			
			if ( eventData.clickCount == 2 )
			{
				// Start loading the scene

				MenuLogic.Instance.GetComponent<LoadScenes>().StartLoad( saveEntry );
			}
		}
		
	}
}