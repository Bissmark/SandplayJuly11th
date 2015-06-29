using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PortImage : MonoBehaviour 
{
	public GameObject zoomedInImage;
	public Button sandBox;

	public Image changingImage;
	public Image originalImage;


	// Update is called once per frame
	public void OnClick () 
	{
		if (sandBox.interactable == true) 
		{
			originalImage.overrideSprite = changingImage.overrideSprite;
			zoomedInImage.SetActive (true);
		}
	}
}
