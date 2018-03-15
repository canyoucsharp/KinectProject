using UnityEngine;
using System.Collections;

public class ClearButton : MonoBehaviour {

	// Use this for initialization

    public Painter painter;
    public Vector3 defaultSize = new Vector3();
	void Start () {

        defaultSize = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnMouseDown()
    {
        painter.Clear();
        transform.localScale = defaultSize;
    }
    public void OnMouseEnter()
    {
        transform.localScale = defaultSize + 0.2f * defaultSize;
        CursorSettings.WaitClick = true;
    }

    void OnMouseOver()
    {

        //Debug.Log(string.Format("Mouse Over {0}",Name));
    }

    public void OnMouseExit()
    {
        CursorSettings.WaitClick = false;
        transform.localScale = defaultSize;

    }
}
