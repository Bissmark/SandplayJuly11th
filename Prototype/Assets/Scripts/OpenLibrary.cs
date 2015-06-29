using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenLibrary : MonoBehaviour 
{
	public bool isLibraryOpen = false;
	public Animator libraryAnimation;
	
	// Update is called once per frame
	public void LetsWork () 
	{
		if (isLibraryOpen == false) 
		{
			libraryAnimation.Play ("Pressed");
			isLibraryOpen = true;
		} 
		else 
		{
			libraryAnimation.Play ("Disabled");
			isLibraryOpen = false;
		}
	}
}