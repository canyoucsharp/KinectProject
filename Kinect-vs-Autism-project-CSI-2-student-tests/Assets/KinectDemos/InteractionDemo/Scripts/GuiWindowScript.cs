using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GuiWindowScript : MonoBehaviour 
{
	// GUI rectangle and skin
	public Rect guiWindowRect = new Rect(-140, 40, 300, 700);
	public GUISkin guiSkin;

	// public parameters
	public GameObject planeObj = null;
	public bool hiddenWindow = false;
	
	private bool resetObjectsClicked = false;
	private bool hideWindowClicked = false;
	private bool isGravityOn = false;
	private bool isPlaneOn = true;
	private bool isControlMouseOn = false;
    private bool UseLeftHandPlayer0 = false;
    private bool UseLeftHandPlayer1 = false;

	public GameObject optionsButton;

	private string label1Text = string.Empty;
	private string label2Text = string.Empty;


	void Start()    
	{
		planeObj = GameObject.Find("Plane");
	}
    public List<InteractionManager> Managers = new List<InteractionManager>();
	public void openOptions()
	{
		hiddenWindow = false;
		optionsButton.SetActive (false);
        foreach (var m in GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>())
        {
            Managers.Add(m);

        }
    }


	private void ShowGuiWindow(int windowID) 
	{
		GUILayout.BeginVertical();

		GUILayout.Space(30);
		isPlaneOn = GUILayout.Toggle(isPlaneOn, "Plane On");
		SetPlaneVisible(isPlaneOn);

		GUILayout.Space(30);
		isGravityOn = GUILayout.Toggle(isGravityOn, "Gravity On");
		SetGravity(isGravityOn);
		
		GUILayout.Space(30);
		isControlMouseOn = GUILayout.Toggle(isControlMouseOn, "Control Mouse");
		SetMouseControl(isControlMouseOn);

        GUILayout.Space(30);
        InteractionManager.UseFreeHand = GUILayout.Toggle(InteractionManager.UseFreeHand, "Use Free Hand");
        if (!InteractionManager.UseFreeHand)
        {
            GUILayout.Space(30);
            UseLeftHandPlayer0 = GUILayout.Toggle(UseLeftHandPlayer0, "Use left hand for player 0");
            SetPlayerPreferHand(0, UseLeftHandPlayer0);
            GUILayout.Space(30);
            UseLeftHandPlayer1 = GUILayout.Toggle(UseLeftHandPlayer1, "Use left hand for player 1");
            SetPlayerPreferHand(1, UseLeftHandPlayer1);
        }

        //  GUILayout.FlexibleSpace();
        GUILayout.Space(30);
        resetObjectsClicked = GUILayout.Button("Reset Objects");
		if(resetObjectsClicked)
		{
			//label1Text = "Resetting objects...";
			ResetObjects(resetObjectsClicked);
		}

		GUILayout.Label(label1Text);

		hideWindowClicked = GUILayout.Button("Hide Options");
		if(hideWindowClicked)
		{
			//label2Text = "Hiding options window...";
			HideWindow(hideWindowClicked);
		}
		
		GUILayout.Label(label2Text);
		GUILayout.EndVertical();
		// Make the window draggable.
		GUI.DragWindow();
	}
	
	
	void OnGUI()
	{
		if(!hiddenWindow)
		{
			Rect windowRect = guiWindowRect;
			if(windowRect.x < 0)
				windowRect.x += Screen.width;
			if(windowRect.y < 0)
				windowRect.y += Screen.height;
			
			GUI.skin = guiSkin;
			guiWindowRect = GUI.Window(1, windowRect, ShowGuiWindow, "Options");
		}
	}


	// set gravity on or off
	private void SetGravity(bool gravityOn)
	{
		GrabDropScript compGrabDrop = GetComponent<GrabDropScript>();

		if(compGrabDrop != null && compGrabDrop.useGravity != gravityOn)
		{
			compGrabDrop.useGravity = gravityOn;
		}
	}

	// make plane visible or not
	private void SetPlaneVisible(bool planeOn)
	{
		if(planeObj && planeObj.activeInHierarchy != planeOn)
		{
			planeObj.SetActive(planeOn);
		}
	}    private void SetPlayerPreferHand(int playerIndex, bool uselefthand)
    {
        var manager = (from m in Managers where m.playerIndex == playerIndex select m).FirstOrDefault();
        if (manager != null)    
            manager.UseLeftHand = uselefthand;
    }


    // turn off or on mouse-cursor control
    private void SetMouseControl(bool controlMouseOn)
	{
        if (Managers!=null && Managers.Count > 0)
        {
            foreach (var m in Managers)
            {
               // InteractionManager manager = InteractionManager.Managers[0];

                if (m.IsInteractionInited())
                {
                    if (m.controlMouseCursor != controlMouseOn)
                    {
                        m.controlMouseCursor = controlMouseOn;
                    }
                }
            }
        }
	}

	// reset objects if needed
	private void ResetObjects(bool resetObjs)
	{
		if(resetObjs)
		{
			GrabDropScript compGrabDrop = GetComponent<GrabDropScript>();
			
			if(compGrabDrop != null)
			{
				compGrabDrop.resetObjects = true;
			}
		}
	}

	// hide options window
	private void HideWindow(bool hideWin)
	{
		if(hideWin)
		{
			hiddenWindow = true;
			optionsButton.SetActive (true);
		}
	}
	

}
