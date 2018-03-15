using UnityEngine;
using System.Collections;

public class log : MonoBehaviour {

	// Use this for initialization

	public System.IO.StreamWriter file ;
	void Start () {

        file = new System.IO.StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "\\GameLog.txt", true);

        file.WriteLine(System.DateTime.Now.ToString("hh:mm:ss")+"  level Loaded: "+ Application.loadedLevelName); 


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
