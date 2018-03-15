using UnityEngine;
using System.Collections;

public class ColorPicker : MonoBehaviour {

	// Use this for initialization

    public static Color selectedColor = Color.red;
    public Color myColor = Color.red;
    Vector3 defaultSize = new Vector3();
    public Material currentMat = null;
    void Start()
    {
        
        defaultSize = transform.localScale;
        myColor = currentMat.color;// gameObject.GetComponent<Renderer>().GetComponent<Material>().color;
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnMouseDown()
    {
        selectedColor = myColor;
        transform.localScale = defaultSize;
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
    public void OnMouseEnter()
    {
        transform.localScale = defaultSize + 0.2f * defaultSize;
        CursorSettings.WaitClick = true;
    }
}
