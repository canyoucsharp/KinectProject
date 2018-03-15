using UnityEngine;
using System.Collections;

public enum CursorType
{
	Hand,
	Hand1,
	Hand2,
	Hand3,
	Hand4,
    Grip
}

public class CursorSettings : MonoBehaviour {

	public static CursorType currentCursor = CursorType.Hand;
	public  Texture2D handCusor;
	public  Texture2D handCusor1;
	public  Texture2D handCusor2;
	public  Texture2D handCusor3;
	public  Texture2D handCusor4;
    public Texture2D gripCursor;
	public  static int Progress = System.Environment.TickCount;
	public static bool _WaitClick = false;
    private CursorMode curMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    InteractionManager Manager;
	
	public static bool WaitClick
	{
		get{return _WaitClick;}
		set
		{
		//	Debug.Log("Wait state "+value.ToString());
			Progress = 0;
			_WaitClick = value;
		}
	}

	// Use this for initialization
	void Start ()
    {
        Manager = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0];
		ResetCursor();
	}


	// Update is called once per frame
	void Update () 
	{


        if (WaitClick)
        {
            if (Progress == 0)
                Progress = System.Environment.TickCount;

            if (System.Environment.TickCount - Progress < 700 && currentCursor != CursorType.Hand1)
            {

                SetCusorHand1();
            }
            else
                if (System.Environment.TickCount - Progress > 700 && System.Environment.TickCount - Progress < 1500 && currentCursor != CursorType.Hand2)
                {

                    SetCusorHand2();
                }
                else
                    if (System.Environment.TickCount - Progress > 1500 &&
                       System.Environment.TickCount - Progress < 2000 && currentCursor != CursorType.Hand3)
                    {

                        SetCusorHand3();
                    }
                    else
                        if (System.Environment.TickCount - Progress >= 2000 && System.Environment.TickCount - Progress <= 2500 && currentCursor != CursorType.Hand4)
                        {

                            SetCusorHand4();
                        }
                        else
                            if (System.Environment.TickCount - Progress >= 2500)
                            {


                                MouseControl.MouseClick();
                                Progress = 0;
                                WaitClick = false;
                                ResetCursor();
                            }
            if (System.Environment.TickCount - Progress >= 2500)
            {
                Progress = 0;
                WaitClick = false;
                ResetCursor();

            }

        }
        else
        {
            ResetCursor();
            Progress = 0;

        }
	}


    public void ResetCursor()
    {
        if (

            currentCursor != CursorType.Grip
            && Manager != null
            && Manager.PrimaryHandEvent == InteractionManager.HandEventType.Grip
            )
        {
            Cursor.SetCursor(gripCursor, hotSpot,
            curMode);
            currentCursor = CursorType.Grip;
        }
        else
            if (Manager != null && Manager.PrimaryHandEvent != InteractionManager.HandEventType.Grip)
        {

            Cursor.SetCursor(handCusor,
                              hotSpot,
                              curMode);
            currentCursor = CursorType.Hand;
        }
    }
	public   void SetCusorHand1()
	{
		currentCursor = CursorType.Hand1;
		Cursor.SetCursor (
			handCusor1,
            hotSpot,
            curMode);
		
	}

	public  void SetCusorHand2()
	{
		currentCursor = CursorType.Hand2;
		Cursor.SetCursor (
			handCusor2,
            hotSpot,
            curMode);
	}

	public  void SetCusorHand3()
	{
		currentCursor = CursorType.Hand3;
		Cursor.SetCursor (
			handCusor3,
            hotSpot,
            curMode);		
	}

	public  void SetCusorHand4()
	{
		currentCursor = CursorType.Hand4;
		Cursor.SetCursor (
			handCusor4,
            hotSpot,
            curMode);		
	}
}
