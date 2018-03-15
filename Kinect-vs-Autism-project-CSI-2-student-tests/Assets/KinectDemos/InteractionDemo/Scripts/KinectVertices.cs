using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class KinectVertices : MonoBehaviour {
    //list of face vertices you can retrieve from the kinect using these indeces
        private  const int LefteyeInnercorner 	     =		    210;
		private  const int LefteyeOutercorner		 =			469;
		private  const int LefteyeMidtop 			 =			241;
		private  const int LefteyeMidbottom		 	 = 			1104;
		private  const int RighteyeInnercorner		 =			843;
		private  const int RighteyeOutercorner	 	 =			117;
		private  const int RighteyeMidtop		 	 =			731;
		private  const int RighteyeMidbottom		 =			1090;
		private  const int LefteyebrowInner			 =			346;
		private  const int LefteyebrowOuter		 	 =			140;
		private  const int LefteyebrowCenter		 =			222;
		private  const int RighteyebrowInner		 =			803;
		private  const int RighteyebrowOuter		 =			758;
		private  const int RighteyebrowCenter	 	 =			849;
		private  const int MouthLeftcorner  	     =			91;
		private  const int MouthRightcorner			 =			687;
		private  const int MouthUpperlipMidtop	 	 =			19;
		private  const int MouthUpperlipMidbottom  	 =			1072;
		private  const int MouthLowerlipMidtop		 =			10;
		private  const int MouthLowerlipMidbottom 	 =			8;
		private  const int NoseTip				 	 =			18;
		private  const int NoseBottom			     =			14;
		private  const int NoseBottomleft			 =			156;
		private  const int NoseBottomright			 =			783;
		private  const int NoseTop					 =			24;
		private  const int NoseTopleft				 =			151;
		private  const int NoseTopright				 =			772;
		private  const int ForeheadCenter		 	 =			28;
		private  const int LeftcheekCenter			 =			412;
		private  const int RightcheekCenter		 	 =			933;
		private  const int Leftcheekbone			 =			458;
		private  const int Rightcheekbone		 	 =			674;
		private  const int ChinCenter				 =			4;
		private  const int LowerjawLeftend		 	 =			1307;
		private  const int LowerjawRightend			 =			1327;


	public static int getLefteyeInnercorner()
	{
		return LefteyeInnercorner;
	}

}
