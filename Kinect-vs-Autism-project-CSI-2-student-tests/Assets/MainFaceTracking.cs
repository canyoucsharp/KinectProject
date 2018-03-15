using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System;

public class MainFaceTracking : MonoBehaviour {
   //Objects to retreive
   private static  PecCard              pecCard;
   private static FacetrackingManager  faceTrackingManagerP1;
   private static FacetrackingManager  faceTrackingManagerP2;
   
    public PecCard getPecCard()
    {
        return pecCard;
    }
  
    public static FacetrackingManager getFaceTrackingManager(int playerNum)
    {
       
        return playerNum == 0 ? faceTrackingManagerP1 : faceTrackingManagerP2;
    }
    void Awake()
    {   faceTrackingManagerP1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FacetrackingManager>();
        //faceTrackingManagerP2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<FacetrackingManager>()[1];
    }
   
    // Use this for initialization
	void Start () {
    


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
