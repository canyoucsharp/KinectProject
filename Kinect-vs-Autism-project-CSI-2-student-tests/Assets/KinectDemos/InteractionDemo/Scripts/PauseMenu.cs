using UnityEngine;
using System.Linq;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public static bool paused = false;
	public GUIStyle pauseMenuStyle;
	public GUIStyle bgStyle;
	private int littleButtonWidth;
	private int littleButtonHeight;
	private int buttonWidth;
	private int buttonHeight;
    
	
	private int fontSize;

	public GUISkin guiSkin;
	
	// public parameters
	public GameObject planeObj = null;
	public bool hiddenWindow = false;
	private bool quitDialog = false;
	

	

	// Use this for initialization
	void Start ()
	{
		//set button dimensions
		SetButtonDimensions();
		fontSize = (int)(Screen.width / 24.4f);
		pauseMenuStyle.fontSize = fontSize;

	}
		
	// Update is called once per frame
	void Update ()
	{
		if( Input.GetKeyDown(KeyCode.Escape) )
		{
           
			PauseGame();

		}
	}
	
	void OnGUI()
	{
		if( paused )
		{
			
			if( !quitDialog)
			{
				DrawPauseMenuBG();
				
				// Resume game button
				if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, buttonHeight, buttonWidth, buttonHeight), "Resume", pauseMenuStyle))
				{
					PauseGame();
				}
				
				// Reset level button
				if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, buttonHeight*2, buttonWidth, buttonHeight), "Reset Level", pauseMenuStyle))
				{
					print("Reset Level");
					PauseGame();
					Application.LoadLevel(Application.loadedLevel);
				}
				//Main Menu button
				if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, buttonHeight*3, buttonWidth, buttonHeight), "Main Menu", pauseMenuStyle))
				{
					print("Main Menu");
					PauseGame();
					Application.LoadLevel(0);
				}

				// Quit button
				if(GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, buttonHeight*4, buttonWidth, buttonHeight), "Quit Game", pauseMenuStyle))
				{
					quitDialog = true;
				}
			}
		
			else if( quitDialog )
			{
				DrawPauseMenuBG();
				// Are you sure label
				GUI.Label(new Rect( Screen.width/2 - buttonWidth/2, Screen.height/2 - buttonHeight, buttonWidth, buttonHeight), "Are you sure?", pauseMenuStyle);
			
				// yes
				if( GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2, buttonWidth, buttonHeight), "Yes", pauseMenuStyle) )
				{
					Application.Quit();
				}
				// no
				if( GUI.Button(new Rect(Screen.width/2 - buttonWidth/2, Screen.height/2 + buttonHeight, buttonWidth, buttonHeight), "No", pauseMenuStyle) )
				{
					quitDialog = false;
				}
			}
		}
	}
	
	

	

	

	// toggle pause state
	public void PauseGame()
	{
        
		paused = !paused;
		Time.timeScale = 1.0f - Time.timeScale;
		
		// pause or unpause the music
		if( paused )
		{
			print("Game Paused");
		}
		else
		{
			print("Game Resumed");
		}
	}
	
	void DrawPauseMenuBG()
	{
		// Make a background box
		GUI.Box(new Rect(-10, -10, Screen.width + 20, Screen.height + 20), "", bgStyle);
	}

	

	
	void SetButtonDimensions()
	{
		littleButtonWidth = Screen.width / 12;
		littleButtonHeight = Screen.height / 8;
		buttonWidth = Screen.width / 2;
		buttonHeight = Screen.height / 6;

	}





}
