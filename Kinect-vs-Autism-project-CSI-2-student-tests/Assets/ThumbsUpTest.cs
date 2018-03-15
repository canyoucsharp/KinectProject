using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbsUpTest : MonoBehaviour {

	private KinectManager kinectManager;
	private InteractionManager interactionManager;
	private Text leftPlayerThumbTextPos;


	private long leftId;
	private long rightId;
	int p1Index=0;
	int p2Index=1;
	// Use this for initialization
	void Start () {
		 kinectManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KinectManager>();
		interactionManager=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InteractionManager>();
		leftPlayerThumbTextPos=GameObject.FindGameObjectWithTag("leftthumb").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
		assignIdByPosition();
		detectThumbsUp();


	}


	public void assignIdByPosition()
	{

		long p1Id = KinectManager.Instance.GetUserIdByIndex(p1Index);
		long p2Id = KinectManager.Instance.GetUserIdByIndex(p2Index);
		Vector3 p1pos = KinectManager.Instance.GetUserPosition(p1Id);
		Vector3 p2pos = KinectManager.Instance.GetUserPosition(p2Id);
		int userCount=KinectManager.Instance.GetUsersCount();


		if (userCount == 1)
		{
			leftId = p1Id;
		}
		else if (userCount>1)
		{
			if (p1pos.x <= p2pos.x)
			{
				leftId = p1Id;
				rightId = p2Id;
			}
			else if (p2pos.x <= p1pos.x)
			{
				leftId = p2Id;
				rightId = p1Id;
			}

		}
	}

	public void detectThumbsUp()
	{
		Debug.Log("insided detectthumbsUp");
		Vector3 p1ThumbPos=kinectManager.GetJointPosition(leftId,24);
		Vector3 p1HandPos=kinectManager.GetJointPosition(leftId,11);
		Vector3 spine=kinectManager.GetJointPosition(leftId,1);
		Vector3 rightShoulder=kinectManager.GetJointPosition(leftId,(int)KinectInterop.JointType.ShoulderRight);
		Vector3 fingerTips=kinectManager.GetJointPosition(leftId,(int)KinectInterop.JointType.HandTipRight);
		Vector3 Hip=kinectManager.GetJointPosition(leftId,(int)KinectInterop.JointType.HipRight);
		//p1Handpos.x<=rightShoulder.x&&p1Handpos.z<spine.z//p1ThumbPos.y>=p1Handpos.y

		leftPlayerThumbTextPos.text=p1ThumbPos.x.ToString();
		// interactionManager.PrimaryHandEvent != InteractionManager.HandEventType.Release
		if( (p1ThumbPos.y>=fingerTips.y && p1HandPos.y>Hip.y) ||(p1ThumbPos.y>=p1HandPos.y && p1HandPos.y>Hip.y) )
		{
			leftPlayerThumbTextPos.text="thumbs up";
			Debug.Log("thumbs up");
		}
		//Thumb Y greater than Hand Y.
		//2) Hand Z less than Spine Z.
		//3) HandState not equal to Open.

	}
}
