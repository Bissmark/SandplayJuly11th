using UnityEngine;
using System.Collections;

public class MenuLogic : Manager<MenuLogic>
{
	public GameObject mainMenu;
	public GameObject loadMenu;
	public GameObject mediaMenu;
	public GameObject screenshotMenu;
	public GameObject videoMenu;
	public GameObject loginMenu;
	public GameObject trainingMenu;
	public GameObject sandplayMenu;
	public GameObject trainingMenu1;
	public GameObject trainingMenu2;
	
	private GameObject activeMenu;
	private GameObject prevMenu;
	
	protected override void Awake()
	{
		base.Awake();
		
		activeMenu = mainMenu;	
	}
	
	void ChangeMenu (GameObject newMenu) 
	{
		prevMenu = activeMenu;
		activeMenu.SetActive(false);
		activeMenu = newMenu;
		activeMenu.SetActive(true);
	}
	
	public void MainMenu () 
	{
		ChangeMenu(mainMenu);	
	}
	
	public void LoadMenu () 
	{	
		ChangeMenu (loadMenu);
	}
	
	public void MediaMenu () 
	{	
		ChangeMenu(mediaMenu);
	}
	
	public void ScreenshotMenu () 
	{	
		ChangeMenu(screenshotMenu);
	}

	public void TrainingMenu () 
	{	
		ChangeMenu(trainingMenu);
	}

	public void TrainingMenu1 () 
	{	
		ChangeMenu(trainingMenu1);
	}

	public void TrainingMenu2 () 
	{	
		ChangeMenu(trainingMenu2);
	}

	public void SandplayMenu () 
	{	
		ChangeMenu(sandplayMenu);
	}
	
	public void VideosMenu () 
	{	
		ChangeMenu(videoMenu);
	}
	
	public void LoginMenu()
	{
		ChangeMenu(loginMenu);
	}
	
	public void Back () 
	{	
		ChangeMenu(prevMenu);
	}
	
	public void Play () 
	{	
		Application.LoadLevel(1);
	}
	
	public void Exit () 
	{
		Application.Quit();
	}
}