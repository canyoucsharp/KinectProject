using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positionmanager : MonoBehaviour {




	InteractionManager player1;
	InteractionManager player2;
	KinectManager _KinectManager;
	public Texture [] leftMostHand;//assigned in editor
	public Texture [] RightMostHand;//assigned in editor



	// Use this for initialization
	void Awake()
	{
		player1 = (GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0]);//.playerIndex == 0) ? 
			//GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[0] : 
			//GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		player2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>()[1];
		_KinectManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KinectManager>();



	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		


		playerSideAdjuster();

		
	}






public void playerSideAdjuster()
{




	var p1id=_KinectManager.GetUserIdByIndex(0);
	var p2id=_KinectManager.GetUserIdByIndex(1);


		//player1.isRight = false;//temporary for testing

		if(	p1id !=0 && p2id!=0)
	{
		var p1pos= _KinectManager.GetUserPosition(p1id);
		var p2pos= _KinectManager.GetUserPosition(p2id);
		//Debug.Log("p1pos="+p1pos);
		//Debug.Log("p2pos="+p2pos);

		if(p1pos.x <= p2pos.x)
		{
			player1.isRight = false;
			player2.isRight = true;
			player1.gripHandTexture=leftMostHand[0];
			player1.releaseHandTexture=leftMostHand[1];
			player1.normalHandTexture=leftMostHand[2];
			
			player2.gripHandTexture=RightMostHand[0];
			player2.releaseHandTexture=RightMostHand[1];
			player2.normalHandTexture=RightMostHand[2];
			
			//player1.playerIndex=0;
			//player2.playerIndex=1;

		}
		else 
		{
				player1.isRight = true;
				player2.isRight = false;
				player2.gripHandTexture=leftMostHand[0];
				player2.releaseHandTexture=leftMostHand[1];
				player2.normalHandTexture=leftMostHand[2];

				player1.gripHandTexture=RightMostHand[0];
				player1.releaseHandTexture=RightMostHand[1];
				player1.normalHandTexture=RightMostHand[2];


			//player1.playerIndex=1;
			//player2.playerIndex=0;



		}
	}


}




}