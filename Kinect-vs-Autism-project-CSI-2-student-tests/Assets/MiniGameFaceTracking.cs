using System.Collections;
using System.Collections.Generic;
using UnityEngine;



	using UnityEngine;
	using UnityEngine.UI;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Assets.KinectScripts;
	using System.IO;
	using System.Text;
	using System.Xml.Linq;
	using System; 

	//using System.Runtime.InteropServices;
	#region outerclass
public class MiniGameFaceTracking : MonoBehaviour {
	

		private Vector3 leftMeshPos;
		private Vector3 rightMeshPos;
		//public GameObject engagement;
		private int p1Index=0;
		private int p2Index=1;
		private Vector3 p1shoulderpos;//extrenous remove later
		private Vector3 p1rightShoulderPos;
		private Vector3 p1leftShoulderPos;
		private Vector3 p2rightShoulderPos;
		private Vector3 p2leftShoulderPos;
		private Vector3 p2shoulderpos;
		private Quaternion p1rot;
		private Quaternion p1shoulderrot;
		private Quaternion p2shoulderrot;
		private Quaternion p2rot;
		private float p1shoulderToChin;
		private float p1shoulderToShoulder;
		private float p1TotalEngagement;
		private float p2shoulderToChin;
		private float p2shoulderToShoulder;
		private float p2TotalEngagement;
		private float leftPlayerEngagement;
		private float rightPlayerEngagement;
		private float combinedPlayerEngagement;
		private float leftPlayerEngagementsecondHalf;
		private float rightPlayerEngagementsecondHalf;




		private float combinedPlayerEngagementsecondHalf;
		private Text leftFaceEngagementText;
		private Text rightFaceEngagementText;
		private Text combinedFaceEngagementText;


		private int updateCounter;
		private InteractionManager player1;
		private InteractionManager player2;


		private long leftId;
		private long rightId;

		public int  fixedUpdateCounter=0;
		private PecCard _PecCard;
		//public GameObject face1;
		//public GameObject face2;
		private Renderer faceMat1;
		private Renderer faceMat2;
		private Vector3[] avModelVertices1 = null;
		private Vector3[] avModelVertices2 = null;


		private bool once = true;
		private float timeLeft;
		private Animator[] stars;

		public enum PositionState
		{
			Begin,
			FacingForward,
			FacingTowardsPartner
		};

		private PositionState leftPlayerPosState=PositionState.Begin;
		private PositionState rightPlayerPosState=PositionState.Begin;


		private class PlayerFacetrackingData
		{


			// The index of the player, whose face this manager tracks. Default is 0 (first player).
			public int playerIndex = 0;

			public Mesh mesh = null;

			public bool getFaceModelData = false;

			// whether the face model mesh was initialized
			public bool bFaceModelMeshInited = false;

			// Is currently tracking user's face
			public bool isTrackingFace = false;

			// Tolerance (in seconds) allowed to miss the tracked face before losing it
			public float faceTrackingTolerance = 0.25f;

			// The game object that will be used to display the face model mesh
			public GameObject faceModelMesh = new GameObject();

			// Public Bool to determine whether the model mesh should be mirrored or not
			public bool mirroredModelMesh = true;

			// returns true if the facetracking system is tracking a face
			public bool IsTrackingFace()
			{
				return isTrackingFace;
			}

			// primary user ID, as reported by KinectManager
			public long primaryUserID = 0;

			private float lastFaceTrackedTime = 0f;

			// Animation units
			public Dictionary<KinectInterop.FaceShapeAnimations, float> dictAU = new Dictionary<KinectInterop.FaceShapeAnimations, float>();
			public bool bGotAU = false;

			// Shape units
			public Dictionary<KinectInterop.FaceShapeDeformations, float> dictSU = new Dictionary<KinectInterop.FaceShapeDeformations, float>();
			public bool bGotSU = false;

			// Vertices of the face model
			public Vector3[] avModelVertices = null;
			public bool bGotModelVertices = false;

			// Head position and rotation
			public Vector3 headPos = Vector3.zero;
			public bool bGotHeadPos = false;

			public Quaternion headRot = Quaternion.identity;
			public bool bGotHeadRot = false;

			public void updatePlayer(bool facesUpdatedSuccessfully, KinectInterop.SensorData sensorData)
			{
				//replace primaryUserId with user1 and user2




				// update the face tracker
				if (facesUpdatedSuccessfully)
				{
					// estimate the tracking state
					isTrackingFace = sensorData.sensorInterface.IsFaceTracked(primaryUserID);

					// get the facetracking parameters
					if (isTrackingFace)
					{
						lastFaceTrackedTime = Time.realtimeSinceStartup;

						// get face rectangle
						//bGotFaceRect = sensorData.sensorInterface.GetFaceRect(primaryUserID, ref faceRect);

						// get head position
						bGotHeadPos = sensorData.sensorInterface.GetHeadPosition(primaryUserID, ref headPos);

						// get head rotation
						bGotHeadRot = sensorData.sensorInterface.GetHeadRotation(primaryUserID, ref headRot);

						// get the animation units
						bGotAU = sensorData.sensorInterface.GetAnimUnits(primaryUserID, ref dictAU);

						// get the shape units
						bGotSU = sensorData.sensorInterface.GetShapeUnits(primaryUserID, ref dictSU);

						//Debug.Log("updateUser: " + playerIndex);

						if (faceModelMesh != null && faceModelMesh.activeInHierarchy)
						{
							// apply model vertices to the mesh
							if (!bFaceModelMeshInited)
							{
								//commenting out creating the mesh for now 9/3/2017
								//CreateFaceModelMesh(sensorData);
							}
						}

						if (getFaceModelData)
						{
							UpdateFaceModelMesh(sensorData);
						}

					}
					else if ((Time.realtimeSinceStartup - lastFaceTrackedTime) <= faceTrackingTolerance)
					{
						// allow tolerance in tracking
						isTrackingFace = true;
					}

					if (faceModelMesh != null && bFaceModelMeshInited)
					{
						faceModelMesh.SetActive(isTrackingFace);
					}
				}
			}

			private void CreateFaceModelMesh(KinectInterop.SensorData sensorData)
			{
				if (faceModelMesh == null)
					return;

				int iNumTriangles = sensorData.sensorInterface.GetFaceModelTrianglesCount();
				if (iNumTriangles <= 0)
					return;

				int[] avModelTriangles = new int[iNumTriangles];
				bool bGotModelTriangles = sensorData.sensorInterface.GetFaceModelTriangles(mirroredModelMesh, ref avModelTriangles);

				if (!bGotModelTriangles)
					return;

				int iNumVertices = sensorData.sensorInterface.GetFaceModelVerticesCount(0);
				if (iNumVertices < 0)
					return;

				avModelVertices = new Vector3[iNumVertices];
				bGotModelVertices = sensorData.sensorInterface.GetFaceModelVertices(0, ref avModelVertices);


				if (!bGotModelVertices)
					return;

				Vector2[] avModelUV = new Vector2[iNumVertices];

				//Quaternion faceModelRot = faceModelMesh.transform.rotation;
				//faceModelMesh.transform.rotation = Quaternion.identity;

				mesh = new Mesh();
				mesh.name = "FaceMesh";
				faceModelMesh.GetComponent<MeshFilter>().mesh = mesh;

				mesh.vertices = avModelVertices;

				mesh.uv = avModelUV;
				mesh.triangles = avModelTriangles;
				mesh.RecalculateNormals();



				//faceModelMesh.transform.rotation = faceModelRot;

				bFaceModelMeshInited = true;
			}

			private void UpdateFaceModelMesh(KinectInterop.SensorData sensorData)
			{
				// init the vertices array if needed
				if (avModelVertices == null)
				{
					int iNumVertices = sensorData.sensorInterface.GetFaceModelVerticesCount(primaryUserID);
					avModelVertices = new Vector3[iNumVertices];
				}

				// get face model vertices
				bGotModelVertices = sensorData.sensorInterface.GetFaceModelVertices(primaryUserID, ref avModelVertices);

				if (bGotModelVertices && faceModelMesh != null && bFaceModelMeshInited)
				{
					Quaternion faceModelRot = faceModelMesh.transform.rotation;
					//faceModelMesh.transform.rotation = Quaternion.identity;

					mesh = faceModelMesh.GetComponent<MeshFilter>().mesh;
					mesh.vertices = avModelVertices;
					mesh.RecalculateNormals();
					mesh.RecalculateBounds();

					Debug.Log("Index " + playerIndex + "'s Mesh Vertices: " + mesh.vertices[10]);
				}
			}

			public Vector3 getMeshTransform()
			{
				Vector3 meshVector;
				if (isTrackingFace)
				{
					Debug.Log("getMeshTransform: " + playerIndex);
					meshVector = new Vector3(mesh.vertices[KinectVertices.getLefteyeInnercorner()].x,
						mesh.vertices[KinectVertices.getLefteyeInnercorner()].y,
						mesh.vertices[KinectVertices.getLefteyeInnercorner()].z);
				}
				else
					meshVector = new Vector3(0, 0, 0);

				return meshVector;
			}
		};
		#endregion 

		private int leftPlayerTurnedRight=0;
		private int leftPLayerTurnedBack=0;
		const int numRows=1000;
		const int numCols=2;
		string [,] turnTable=new string[numRows,numCols];
		string [,] turnTableTwo=new string[numRows,numCols];



		int topRow=0;
		int topRowTwo=0;
		const int col1=0;
		const int col2=1;

		[Range(1, 6)]
		public int players = 2;

		// The game object that will be used to display the face model mesh
		public GameObject[] faceModelMesh = null;

		PlayerFacetrackingData[] playerData = null;

		private KinectManager kinectManager = null;

		// Public bool to determine whether to track face model data or not
		public bool getFaceModelData = false;

		// Public Bool to determine whether to display face rectangle on the GUI
		public bool displayFaceRect = false;

		// Tolerance (in seconds) allowed to miss the tracked face before losing it
		public float faceTrackingTolerance = 0.25f;

		// Public Bool to determine whether the model mesh should be mirrored or not
		public bool mirroredModelMesh = true;

		// GUI Text to show messages.
		public GUIText debugText;

		// Tracked face rectangle
		//	private Rect faceRect;
		//	private bool bGotFaceRect;

		// primary sensor data structure
		private KinectInterop.SensorData sensorData = null;

		// Bool to keep track of whether face-tracking system has been initialized
		private bool isFacetrackingInitialized = false;

		// Keeps track of whether or not faces were updated to prevent reduntant updating each frame
		private bool facesUpdatedSuccessfully = false;

		// The single instance of FacetrackingManager
	private static MiniGameFaceTracking instance;

		// returns the single FacetrackingManager instance
	public static MiniGameFaceTracking Instance
		{
			get
			{
				return instance;
			}
		}

		// returns true if facetracking system was successfully initialized, false otherwise
		public bool IsFaceTrackingInitialized()
		{
			return isFacetrackingInitialized;
		}

		// returns the skeleton ID of the tracked user, or 0 if no user was associated with the face
		public long GetFaceTrackingID(int playerIndex)
		{
			return playerData[playerIndex].IsTrackingFace() ? playerData[playerIndex].primaryUserID : 0;
		}

		// returns true if the the face of the specified user is being tracked, false otherwise
		public bool IsTrackingFace(long userId)
		{
			if (sensorData != null && sensorData.sensorInterface != null)
			{
				return sensorData.sensorInterface.IsFaceTracked(userId);
			}


			return false;
		}

		public bool IsTrackingAnyFace()
		{
			foreach (PlayerFacetrackingData singlePlayerData in playerData)
				if (singlePlayerData.IsTrackingFace())
					return true;
			return false;
		}

		// returns the tracked head position
		public Vector3 GetHeadPosition(bool bMirroredMovement, int index)
		{
			Vector3 vHeadPos = playerData[index].bGotHeadPos ? 
				playerData[index].headPos : Vector3.zero;

			if (!bMirroredMovement)
			{
				vHeadPos.z = -vHeadPos.z;
			}

			return vHeadPos;
		}

		// returns the tracked head position for the specified user
		public Vector3 GetHeadPosition(long userId, bool bMirroredMovement)
		{
			Vector3 vHeadPos = Vector3.zero;
			bool bGotPosition = sensorData.sensorInterface.GetHeadPosition(userId, ref vHeadPos);

			if (bGotPosition)
			{
				if (!bMirroredMovement)
				{
					vHeadPos.z = -vHeadPos.z;
				}

				return vHeadPos;
			}

			return Vector3.zero;
		}
		public void takeSnapShot()
		{

			turnTable[topRow,col2]+="In SnapShot"+ leftEyeMidTop.ToString()+lowerJawLeft.ToString()+leftCheekBone.ToString();
			turnTableTwo[topRowTwo,col2]+="In SnapShot"+rightEyeMidTop.ToString()+ lowerJawRight.ToString()+rightCheekBone.ToString();
			Debug.Log("Snaps val"+turnTable[topRow,col2]);

		}
		// returns the tracked head rotation
		public Quaternion GetHeadRotation(bool bMirroredMovement, int index)
		{
			Vector3 rotAngles = playerData[index].bGotHeadRot ? 
				playerData[index].headRot.eulerAngles : Vector3.zero;

			if (bMirroredMovement)
			{
				rotAngles.x = -rotAngles.x;
				rotAngles.z = -rotAngles.z;
			}
			else
			{
				rotAngles.x = -rotAngles.x;
				rotAngles.y = -rotAngles.y;
			}

			return Quaternion.Euler(rotAngles);
		}

		// returns the tracked head rotation for the specified user
		public Quaternion GetHeadRotation(long userId, bool bMirroredMovement)
		{
			Quaternion vHeadRot = Quaternion.identity;
			bool bGotRotation = sensorData.sensorInterface.GetHeadRotation(userId, ref vHeadRot);

			if (bGotRotation)
			{
				Vector3 rotAngles = vHeadRot.eulerAngles;

				if (bMirroredMovement)
				{
					rotAngles.x = -rotAngles.x;
					rotAngles.z = -rotAngles.z;
				}
				else
				{
					rotAngles.x = rotAngles.x;
					rotAngles.y = rotAngles.y;
				}

				return Quaternion.Euler(rotAngles);
			}

			return Quaternion.identity;
		}

		// returns the tracked face rectangle for the specified user in color coordinates, or zero-rect if the user's face is not tracked
		public Rect GetFaceColorRect(long userId)
		{
			Rect faceRect = new Rect();
			sensorData.sensorInterface.GetFaceRect(userId, ref faceRect);

			return faceRect;
		}

		// returns true if there are valid anim units
		public bool IsGotAU(int index)
		{
			return playerData[index].bGotAU;
		}

		// returns the animation unit at given index, or 0 if the index is invalid
		public float GetAnimUnit(KinectInterop.FaceShapeAnimations faceAnimKey, int index)
		{
			if (playerData[index].dictAU.ContainsKey(faceAnimKey))
			{
				return playerData[index].dictAU[faceAnimKey];
			}

			return 0.0f;
		}

		// gets all animation units for the specified user. returns true if the user's face is tracked, false otherwise
		public bool GetUserAnimUnits(long userId, ref Dictionary<KinectInterop.FaceShapeAnimations, float> dictAnimUnits)
		{
			if (sensorData != null && sensorData.sensorInterface != null)
			{
				bool bGotIt = sensorData.sensorInterface.GetAnimUnits(userId, ref dictAnimUnits);
				return bGotIt;
			}

			return false;
		}

		// returns true if there are valid shape units
		public bool IsGotSU(int index)
		{
			return playerData[index].bGotSU;
		}

		// returns the shape unit at given index, or 0 if the index is invalid
		public float GetShapeUnit(KinectInterop.FaceShapeDeformations faceShapeKey, int index)
		{
			if (playerData[index].dictSU.ContainsKey(faceShapeKey))
			{
				return playerData[index].dictSU[faceShapeKey];
			}

			return 0.0f;
		}

		// gets all animation units for the specified user. returns true if the user's face is tracked, false otherwise
		public bool GetUserShapeUnits(long userId, ref Dictionary<KinectInterop.FaceShapeDeformations, float> dictShapeUnits)
		{
			if (sensorData != null && sensorData.sensorInterface != null)
			{
				bool bGotIt = sensorData.sensorInterface.GetShapeUnits(userId, ref dictShapeUnits);
				return bGotIt;
			}

			return false;
		}

		// returns the count of face model vertices
		public int GetFaceModelVertexCount(int index)
		{
			if (playerData[index].bGotModelVertices)
			{
				return playerData[index].avModelVertices.Length;
			}

			return 0;
		}

		// returns the face model vertices, if face model is available and index is in range; Vector3.zero otherwise
		public Vector3 GetFaceModelVertex(int index)
		{
			if (playerData[index].bGotModelVertices)
			{
				if (index >= 0 && index < playerData[index].avModelVertices.Length)
				{
					return playerData[index].avModelVertices[index];
				}
			}

			return Vector3.zero;
		}

		// returns the face model vertices, if face model is available; null otherwise
		public Vector3[] GetFaceModelVertices(int index)
		{
			if (playerData[index].bGotModelVertices)
			{
				return playerData[index].avModelVertices;
			}

			return null;
		}

		// gets all face model vertices for the specified user. returns true if the user's face is tracked, false otherwise
		public bool GetUserFaceVertices(long userId, ref Vector3[] avVertices)
		{
			if (sensorData != null && sensorData.sensorInterface != null)
			{
				bool bGotIt = sensorData.sensorInterface.GetFaceModelVertices(userId, ref avVertices);
				return bGotIt;
			}

			return false;
		}

		// returns the face model triangle indices, if face model is available; null otherwise
		public int[] GetFaceModelTriangleIndices(bool bMirroredModel)
		{
			if (sensorData != null && sensorData.sensorInterface != null)
			{
				int iNumTriangles = sensorData.sensorInterface.GetFaceModelTrianglesCount();

				if (iNumTriangles > 0)
				{
					int[] avModelTriangles = new int[iNumTriangles];
					bool bGotModelTriangles = sensorData.sensorInterface.GetFaceModelTriangles(bMirroredModel, ref avModelTriangles);

					if (bGotModelTriangles)
					{
						return avModelTriangles;
					}
				}
			}

			return null;
		}

		public GameObject getFaceModelMesh(int index)
		{
			return playerData[index].faceModelMesh;
		}

		//----------------------------------- end of public functions --------------------------------------//
		//checks if faces were updated yet this frame. if not, updates facetracker for all faces
		bool updateAllFaces()
		{
			if (!facesUpdatedSuccessfully)
			{
				sensorData.sensorInterface.UpdateFaceTracking();
				return true;
			}
			return false;
		}
		private Text p1State;
		private Text p2State;
		private Text rightVector;
		private Text p1Engagement;
		private Text p2Engagement;

		private Text a,b,c;

		void Start()
		{
		faceMat1=this.faceModelMesh[0].GetComponent<Renderer>();
		faceMat2=this.faceModelMesh[1].GetComponent<Renderer>();
			if(GameObject.FindGameObjectWithTag("p1state")!=null)
				p1State=GameObject.FindGameObjectWithTag("p1state").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("p2state")!=null)
				p2State=GameObject.FindGameObjectWithTag("p2state").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("vectorright")!=null)
				rightVector= GameObject.FindGameObjectWithTag("vectorright").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("leftEngagement")!=null)
				p1Engagement=GameObject.FindGameObjectWithTag("leftEngagement").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("rightEngagement")!=null)
				p2Engagement=GameObject.FindGameObjectWithTag("rightEngagement").GetComponent<Text>();

			_PecCard =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PecCard>();

			if(GameObject.FindGameObjectWithTag("a")!=null)
				a= GameObject.FindGameObjectWithTag("a").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("b")!=null)
				b= GameObject.FindGameObjectWithTag("b").GetComponent<Text>();

			if(GameObject.FindGameObjectWithTag("b")!=null)
				c= GameObject.FindGameObjectWithTag("c").GetComponent<Text>();
			//leftFaceEngagementText=GameObject.FindGameObjectWithTag("leftface").GetComponent<Text>();
			//rightFaceEngagementText=GameObject.FindGameObjectWithTag("rightface").GetComponent<Text>();
			//combinedFaceEngagementText=GameObject.FindGameObjectWithTag("bothfaces").GetComponent<Text>();




			Debug.Log(" before face tracking manager instance" + instance);
			if (instance == null)
				instance = this;
			//else
			//Destroy(this);

			//Debug.Log(" after face tracking manager instance" + instance);
			//DontDestroyOnLoad(gameObject);

			playerData = new PlayerFacetrackingData[players];

			for (int i = 0; i < players; i++)
			{
				playerData[i] = new PlayerFacetrackingData();
				//playerData[i].faceModelMesh = faceModelMesh[i];
				// playerData's faceModelMesh was assigned, so delete FacetrackingManager's to reduce memory usage
				//faceModelMesh[i] = null; 
			}
			//faceModelMesh = null;

			for (int i = 0; i < players; i++)
				playerData[i].getFaceModelData = getFaceModelData;

			try
			{
				// get sensor data
				kinectManager = KinectManager.Instance;
				if (kinectManager && kinectManager.IsInitialized())
				{
					sensorData = kinectManager.GetSensorData();
				}
				if (sensorData == null || sensorData.sensorInterface == null)
				{
					throw new Exception("Face tracking cannot be started, because KinectManager is missing or not initialized.");
				}

				if (debugText != null)
				{
					debugText.GetComponent<GUIText>().text = "Please, wait...";
				}

				// ensure the needed dlls are in place and face tracking is available for this interface
				bool bNeedRestart = false;
				if (sensorData.sensorInterface.IsFaceTrackingAvailable(ref bNeedRestart))
				{
					if (bNeedRestart)
					{
						KinectInterop.RestartLevel(gameObject, "FM");
						return;
					}
				}
				else
				{
					string sInterfaceName = sensorData.sensorInterface.GetType().Name;
					throw new Exception(sInterfaceName + ": Face tracking is not supported!");
				}

				// Initialize the face tracker
				if (!sensorData.sensorInterface.InitFaceTracking(getFaceModelData, displayFaceRect))
				{
					throw new Exception("Face tracking could not be initialized.");
				}

				isFacetrackingInitialized = true;

				if (debugText != null)
				{
					debugText.GetComponent<GUIText>().text = "Ready.";
				}
			}
			catch (DllNotFoundException ex)
			{
				Debug.LogError(ex.ToString());
				if (debugText != null)
					debugText.GetComponent<GUIText>().text = "Please check the Kinect and FT-Library installations.";
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
				if (debugText != null)
					debugText.GetComponent<GUIText>().text = ex.Message;
			}
		}

		void OnDestroy()
		{
			if (isFacetrackingInitialized && sensorData != null && sensorData.sensorInterface != null)
			{
				// finish face tracking
				sensorData.sensorInterface.FinishFaceTracking();
			}
			//		// clean up
			//		Resources.UnloadUnusedAssets();
			//		GC.Collect();

			isFacetrackingInitialized = false;
			instance = null;
		}

		void Update()
		{
			//Debug.Log("inside update of Face Tracking Manager");

			if (isFacetrackingInitialized)
			{
				if (kinectManager == null)
					kinectManager = KinectManager.Instance;

				//update faces
				facesUpdatedSuccessfully = updateAllFaces();
				for (int i = 0; i < players; i++)
				{
					if (kinectManager && kinectManager.IsInitialized())
						playerData[i].primaryUserID = kinectManager.GetUserIdByIndex(i);

					playerData[i].updatePlayer(facesUpdatedSuccessfully, sensorData);
				}
				facesUpdatedSuccessfully = false;
			}


			fixedUpdateCounter++;

			//if(fixedUpdateCounter==24)
			//	{
			measureEngagement();
			fixedUpdateCounter=0;
			//}
			//leftFaceEngagementText.text=leftPlayerEngagement.ToString();
			//rightFaceEngagementText.text=rightPlayerEngagement.ToString();
			//combinedFaceEngagementText.text=combinedPlayerEngagement.ToString();

			if(Input.GetKeyDown(KeyCode.R))
			{

				Debug.Log("Calling SnapShot");
				takeSnapShot();
			}

		}

		void OnGUI()
		{
			if (isFacetrackingInitialized)
			{
				if (debugText != null)
				{
					if (playerData[0].isTrackingFace)
					{
						debugText.GetComponent<GUIText>().text = 
							"Tracking - skeletonID: " + playerData[0].primaryUserID;
					}
					else
					{
						debugText.GetComponent<GUIText>().text = "Not tracking...";
					}
				}
			}
		}

		public Vector3 getMeshTransform(int index)
		{
			return playerData[index].getMeshTransform();
		}







		public void sendToCsv()
		{
			// divide by 50 because its in update itll happen 50 times
			combinedPlayerEngagement/=2f;
			leftPlayerEngagement/=2f;
			rightPlayerEngagement/=2f;
			combinedPlayerEngagement/=2f;
			combinedPlayerEngagementsecondHalf/=25f;
			leftPlayerEngagementsecondHalf/=2f;
			rightPlayerEngagementsecondHalf/=2f;
			string currTime = System.DateTime.Now.ToString("h:mm:ss tt");
			currTime=System.DateTime.Now.ToString("yyyy-MM-dd h:mm tt");
			var p1id=kinectManager.GetUserIdByIndex(0);
			bool ran=false;
			StringBuilder csvcontent=new StringBuilder();
			StringBuilder csvContentTwo=new StringBuilder();
			StringBuilder csvContentThree=new StringBuilder();
			string csvfile="C:\\Users\\user\\Desktop\\cloud.csv";
			string csvFileTwo="C:\\Users\\user\\Desktop\\turns.csv";
			string csvFileThree="C:\\Users\\user\\Desktop\\turnsp2.csv";
			if(!ran)
			{
				Debug.Log("in sendtocsv writing to file cloud.csv");
				Debug.Log(leftPlayerEngagement.ToString()+",   "+ rightPlayerEngagement.ToString()+ ","+ combinedPlayerEngagement.ToString()+","+currTime);
				Debug.Log(leftPlayerEngagementsecondHalf.ToString()+","+ rightPlayerEngagementsecondHalf.ToString()+ ","+ 
					combinedPlayerEngagementsecondHalf.ToString()+","+currTime);

				csvContentTwo.AppendLine(starttime.ToString());
				csvContentThree.AppendLine(starttime.ToString());
				for(int r=0;r<topRow;r++)
				{
					for(int c=0;c<numCols;c++)
					{
						if(c==0)
						{
							csvContentTwo.AppendLine(turnTable[r,c]+",");
						}
						else
							csvContentTwo.AppendLine(turnTable[r,c]);
					}
					csvContentTwo.AppendLine();
				}
				for(int r=0;r<topRowTwo;r++)
				{
					for(int c=0;c<numCols;c++)
					{
						if(c==0)
						{
							csvContentThree.AppendLine(turnTableTwo[r,c]+",");
						}
						else
							csvContentThree.AppendLine(turnTableTwo[r,c]);
					}
					csvContentThree.AppendLine();


				}
				//csvcontent.AppendLine("LeftPlayerEngagement,RightPlayerEngagement,CombinedPlayerEngagement");
				csvcontent.AppendLine(leftPlayerEngagement.ToString()+",   "+ rightPlayerEngagement.ToString()+ ",   "+
					combinedPlayerEngagement.ToString()+",   "+currTime+"diving by 2 now instead of 25");//System.DateTime.UtcNow.ToString("HH:mm dd MMMM, yyyy")
				csvcontent.AppendLine(leftPlayerEngagementsecondHalf.ToString()+",   "+ rightPlayerEngagementsecondHalf.ToString()+ ",   "+ combinedPlayerEngagementsecondHalf.ToString()+","+currTime);
				ran=true;

				File.AppendAllText(csvfile,csvcontent.ToString() );
				File.AppendAllText(csvFileTwo,csvContentTwo.ToString());
				File.AppendAllText(csvFileThree,csvContentThree.ToString());
			}
			/*
		avModelVertices1=new Vector3[thisP1.GetFaceModelVertexCount()];
		thisP1.GetUserFaceVertices(p1id, ref avModelVertices1);


			avModelVertices1=new Vector3[thisP1.GetFaceModelVertexCount()];
			thisP1.GetUserFaceVertices(p1id, ref avModelVertices1);
		if(avModelVertices1.Length > 0){
			Debug.Log(avModelVertices1[18].x *100f + "  "+ avModelVertices1[18].z *100f);
		}		for(int i=0;i<thisP1.GetFaceModelVertexCount();i++)
			{

			csvcontent.AppendLine(avModelVertices1[i].x.ToString()+","+ avModelVertices1[i].y.ToString()+"," + avModelVertices1[i].z.ToString());

				}
				*/

		}



		DateTime starttime;
		float angleBtwn2=0;
		Vector3 rightEyeMidTop;//a
		//Vector3	rightEyeMidBot;//b
		Vector3 rightCheekBone;//c
		Vector3 lowerJawRight;

		Vector3 leftEyeMidTop;//a
		//Vector3	leftEyeMidBot;//b
		Vector3 lowerJawLeft;
		Vector3 leftCheekBone;//c
		Vector3 unitVector=new Vector3(1,0,0);

		public void measureEngagement()
		{
			//		Debug.Log("in measure engagement");
			//		Debug.Log("(this.GetFaceTrackingID(p1Index)) = " + (this.GetFaceTrackingID(p1Index)));
			//		if(this.IsTrackingFace(this.GetFaceTrackingID(p1Index)))
			//			{
			//				Debug.Log("is tracking");
			//			}
			//			else
			//			{
			//			Debug.Log("isnt tracking");
			//			}

			//Debug.Log("this.IsTrackingFace(this.GetFaceTrackingID(p1Index) "+ this.IsTrackingFace(this.GetFaceTrackingID(p1Index))+"this.IsTrackingFace(this.GetFaceTrackingID(p2Index)) "+this.IsTrackingFace(this.GetFaceTrackingID(p2Index))+"kinectManager.IsUserDetected() "+kinectManager.IsUserDetected());

			if(starttime==DateTime.MinValue)
			{
				starttime=System.DateTime.Now;
			}
			string currTime;


			assignIdByPosition();
			if (IsTrackingFace(GetFaceTrackingID(p1Index)) && IsTrackingFace(GetFaceTrackingID(p2Index)) && kinectManager.IsUserDetected())
			{

				//HighDetailFacePoints_RighteyeMidtop	731
				//HighDetailFacePoints_RighteyeMidbottom	1090
				//HighDetailFacePoints_Rightcheekbone	674








				//Debug.Log("in this.isTrackingFace");
				//process left player
				// allocate storage for the point cloud points in a vector
				//get the face point cloud from the FaceTrackingManager and fill up the vector
				//get the joint position for the left shoulder 
				avModelVertices1=new Vector3[this.GetFaceModelVertexCount(p1Index)];
				this.GetUserFaceVertices(leftId, ref avModelVertices1);


				leftEyeMidTop	=	avModelVertices1[241] 	*  100f;
				lowerJawLeft	=	avModelVertices1[1307]	*  100f;
				leftCheekBone	=	avModelVertices1[674] 	*  100f;

				p1shoulderpos=kinectManager.GetJointPosition(leftId,4);
				//get the distance between the chin and the left shoulder
				p1shoulderToChin=Mathf.Abs((((avModelVertices1[4].x)-(p1shoulderpos.x))*(100f)));
				//get the joint position for the right shoulder
				p1rightShoulderPos=kinectManager.GetJointPosition(leftId,8);
				//same as above (left)
				p1leftShoulderPos=kinectManager.GetJointPosition(leftId,4);
				//difference of the .z positions for the shoulders (if body turns)
				p1shoulderToShoulder=(p1rightShoulderPos.z - p1leftShoulderPos.z)*20f;
				//head distance plus shoulder distance
				p1TotalEngagement=p1shoulderToChin;// + p1shoulderToShoulder;
				Debug.Log("p1TotalEngagement "+p1TotalEngagement);

				// process right player
				p2rightShoulderPos=kinectManager.GetJointPosition(rightId,8);
				p2leftShoulderPos=kinectManager.GetJointPosition(rightId,4);
				p2shoulderToShoulder=((p2leftShoulderPos.z)-(p2rightShoulderPos.z))*20f;
				//get the face point cloud from the FaceTrackingManager and fill up the vector
				avModelVertices2=new Vector3[this.GetFaceModelVertexCount(p2Index)];
				this.GetUserFaceVertices(rightId, ref avModelVertices2);
				p2shoulderpos=kinectManager.GetJointPosition(rightId,4);
				p2shoulderToChin=Mathf.Abs((((avModelVertices2[4].x)-(p2shoulderpos.x))*(100f)));
				p2TotalEngagement =(p2shoulderToChin);//-(p2shoulderToShoulder);

				rightEyeMidTop=avModelVertices2[731]*100f;
				//rightEyeMidBot=avModelVertices2[1090];
				lowerJawRight=avModelVertices2[1327]*100f;
				rightCheekBone=avModelVertices2[674]*100f;

				var dir2 = Vector3.Cross(lowerJawRight-rightEyeMidTop,rightCheekBone-rightEyeMidTop);
				var norm2=Vector3.Normalize(dir2);
				angleBtwn2=Vector3.Angle(norm2,unitVector);
				if(a!=null)
					//a.text=rightEyeMidTop.ToString("F4");
					a.text=angleBtwn2.ToString();
				if(b!=null)
					b.text=lowerJawRight.ToString("F4");
				if(c!=null)
					c.text=rightCheekBone.ToString("F4");




				var dir1 = Vector3.Cross(lowerJawLeft-leftEyeMidTop,leftCheekBone-leftEyeMidTop);
				var norm1=Vector3.Normalize(dir1);



				//var angleBtwn=Vector3.Angle(norm1,norm2);
				var angleBtwn=Vector3.Angle(norm1,unitVector);


				if(rightVector!=null)
				{
					rightVector.text=angleBtwn.ToString();
					Debug.Log("p2TotalEngagement "+p2TotalEngagement);
				}




				/*
			p2rightShoulderPos=kinectManager.GetJointPosition(rightid,8);
			p2leftShoulderPos=kinectManager.GetJointPosition(rightid,4);
			p2shoulderToShoulder=((p2rightShoulderPos.z)-(p2leftShoulderPos.z))*20f;
			avModelVertices2=new Vector3[thisP2.GetFaceModelVertexCount()];
			thisP2.GetUserFaceVertices(rightid, ref avModelVertices2);
			p2shoulderpos=kinectManager.GetJointPosition(rightid,4);
			p2shoulderToChin=(((avModelVertices2[4].x)-(p2shoulderpos.x))*(100f));
			p2TotalEngagement =(p2shoulderToChin)-(p2shoulderToShoulder);
*/
				Debug.Log("p1TotalEngagement"+p1TotalEngagement+"p2TotalEngagement"+p2TotalEngagement);


				//Debug.Log("leftshoulder.z=" +p1leftShoulderPos.z+"rightshoulder.z"+p1rightShoulderPos.z);
				//Debug.Log("leftshoulder-rightshoulder");
				//Debug.Log("p1leftshoulder.z=" +p1leftShoulderPos.z+"p1rightshoulder.z"+p1rightShoulderPos.z);
				//Debug.Log("p2leftshoulder.z=" +p2leftShoulderPos.z+"p1rightshoulder.z"+p2rightShoulderPos.z);
				//Debug.Log("p1engagement= "+ ((p1shoulderToChin)+(p1shoulderToShoulder)));
				//Debug.Log("p2engagment="+((p2shoulderToChin)-(p2shoulderToShoulder)));
				//Debug.Log("leftshoulder-rightshoulder"+(rightShoulderPos.z-leftShoulderPos.z ));



				//commented out 09/12/2017 to test engagement

				if(p1Engagement!=null)
					p1Engagement.text=p1TotalEngagement.ToString();

				if(p2Engagement!=null)
					p2Engagement.text=p2TotalEngagement.ToString();
				/*
			if(p1TotalEngagement>=20f&&p2TotalEngagement<=10f)
			{
				//faceMat1.material.SetColor("_Color",Color.green);
				//faceMat2.material.SetColor("_Color",Color.green);

				if(_PecCard.pecStarted)
				{
					combinedPlayerEngagementsecondHalf++;
				}
				else
				{
					combinedPlayerEngagement++;
					currTime=System.DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
					Debug.Log("both players turning: "+currTime);


				}
				//engagement.SetActive(true);
				//StartCoroutine(wait());
				return;
			}
			*/

				//new state logic section
				/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////




				if(angleBtwn>=115f/*p1TotalEngagement>=20f*/&&leftPlayerPosState==PositionState.Begin)/*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{

					//currTime=(System.DateTime.Now-starttime).ToString("yyyy-MM-dd h:mm:ss tt");	
					faceMat1.material.SetColor("_Color",Color.blue);
					currTime=(System.DateTime.Now-starttime).ToString();
					if(_PecCard.pecStarted)
					{
						leftPlayerEngagementsecondHalf++;;
						leftPlayerTurnedRight++;
						leftPlayerPosState=PositionState.FacingTowardsPartner;



					}
					else
					{
						leftPlayerEngagement++;
						leftPlayerTurnedRight++;
						leftPlayerPosState=PositionState.FacingTowardsPartner;


						Debug.Log("left players turning: "+currTime);

					}
					if(p1State!=null)
						p1State.text=leftPlayerPosState.ToString();
					turnTable[topRow,col1]=leftPlayerPosState.ToString()+" " +angleBtwn.ToString();//p1TotalEngagement.ToString();
					turnTable[topRow,col2]=currTime;
					topRow++;


				}

				else if(!(/*p1TotalEngagement>=20f*/angleBtwn>=115f) && leftPlayerPosState==PositionState.Begin)
				{
					currTime=(System.DateTime.Now-starttime).ToString();

					leftPlayerPosState=PositionState.FacingForward;

				}


				else  if(/*p1TotalEngagement>=20f*/angleBtwn>=115f&&leftPlayerPosState==PositionState.FacingForward)/*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{

					currTime=(System.DateTime.Now-starttime).ToString();
					faceMat1.material.SetColor("_Color",Color.blue);

					


				}

				else if(/*p1TotalEngagement>=20f*/angleBtwn>=115f&&leftPlayerPosState==PositionState.FacingTowardsPartner)/*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{
					faceMat1.material.SetColor("_Color",Color.blue);
					


				}

				else if(!(/*p1TotalEngagement>=20f*/angleBtwn>=115f )&&leftPlayerPosState==PositionState.FacingTowardsPartner)
				{

					currTime=(System.DateTime.Now-starttime).ToString();
					leftPlayerPosState=PositionState.FacingForward;
					


				}




				//new state logic section end
				/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////
				/// 			/////////////////////////////////////////////////////////////////////////////////////////////////////////


				//*********************** COmmented out this block 09/26/2017 to test new state logic//
				//			if(p1TotalEngagement>=20f /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/)
				//			{
				//
				//				//faceMat1.material.SetColor("_Color",Color.blue);
				//				if(_PecCard.pecStarted)
				//				{
				//					leftPlayerEngagementsecondHalf++;;
				//				}
				//				else
				//				{
				//					leftPlayerEngagement++;
				//					currTime=System.DateTime.Now.ToString("yyyy-MM-dd h:mm tt");
				//					Debug.Log("left players turning: "+currTime);
				//				}
				//
				//			}
				/*//*********************** COmment end forthis block 09/26/2017 to test new state logic//



if(!(p1TotalEngagement>=20f)/&&!(p1TotalEngagement>=20f&&p2TotalEngagement<=15f))
			{
				//faceMat1.material.SetColor("_Color",Color.gray);

			}
			*/
				//if the state is in begin and hes facing forward
				if(/*p2TotalEngagement<=10f*/angleBtwn2<=40f &&rightPlayerPosState==PositionState.Begin) /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{
					//currTime=(System.DateTime.Now-starttime).ToString("yyyy-MM-dd h:mm:ss tt");
					faceMat2.material.SetColor("_Color",Color.blue);
					rightPlayerPosState=PositionState.FacingTowardsPartner;



				}
				else if(!(/*p2TotalEngagement<=10f*/angleBtwn2<=40f)&&rightPlayerPosState==PositionState.Begin) /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{
					//currTime=System.DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");
					currTime=(System.DateTime.Now-starttime).ToString();
					faceMat2.material.SetColor("_Color",Color.blue);
					rightPlayerPosState=PositionState.FacingForward;
					


				}

				else if(/*p2TotalEngagement<=10f*/angleBtwn2<=40f&&rightPlayerPosState==PositionState.FacingForward) /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
				{
					//currTime=System.DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt")-starttime;
					currTime=(System.DateTime.Now-starttime).ToString();
					faceMat2.material.SetColor("_Color",Color.blue);
					
					rightPlayerPosState=PositionState.FacingTowardsPartner;



				}
			}

			else if((/*p2TotalEngagement<=10f*/angleBtwn2<=40f)&&rightPlayerPosState==PositionState.FacingTowardsPartner) /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
			{
				//currTime=(System.DateTime.Now-starttime).ToString("yyyy-MM-dd h:mm:ss tt");	
				faceMat2.material.SetColor("_Color",Color.blue);
				currTime=(System.DateTime.Now-starttime).ToString();	



			}
			else if(!(/*p2TotalEngagement<=10f*/angleBtwn2<=40f)&&rightPlayerPosState==PositionState.FacingTowardsPartner) /*&& !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f)*/
			{

				//currTime=(System.DateTime.Now-starttime).ToString("yyyy-MM-dd h:mm:ss tt");
				currTime=(System.DateTime.Now-starttime).ToString();	
				rightPlayerPosState=PositionState.FacingForward;



			}










			/*commented out 09/12/17 to test combined engagement
			if(!(p2TotalEngagement<=15f) && !(p1TotalEngagement>=20f&&p2TotalEngagement<=15f))
			{
				//faceMat2.material.SetColor("_Color",Color.gray);
			}
*/
			//return;

			/*
		if(thisP1.IsTrackingFace()&&kinectManager.IsUserDetected())
			{
			//p1rot=thisP1.GetHeadRotation(leftid,false);//shoulderight=8//shoulderleft=4

			avModelVertices1=new Vector3[thisP1.GetFaceModelVertexCount()];
			 thisP1.GetUserFaceVertices(leftid, ref avModelVertices1);
			 p1shoulderpos=kinectManager.GetJointPosition(leftid,4);
			 p1shoulderToChin=(((avModelVertices1[4].x)-(p1shoulderpos.x))*(100f));
			 p1rightShoulderPos=kinectManager.GetJointPosition(leftid,8);
			 p1leftShoulderPos=kinectManager.GetJointPosition(leftid,4);
			 p1TotalEngagement=p1shoulderToChin+p1shoulderToShoulder;
			 p1shoulderToShoulder=(p1rightShoulderPos.z-p1leftShoulderPos.z)*20f;
			if((p1TotalEngagement>=25f))
				{
				faceMat1.material.SetColor("_Color",Color.blue);
				leftPlayerEngagement++;
				}
			else
				{
				faceMat1.material.SetColor("_Color",Color.gray);
				}
			}




		if(thisP2.IsTrackingFace())
		{
			avModelVertices2=new Vector3[thisP2.GetFaceModelVertexCount()];
			thisP2.GetUserFaceVertices(leftid, ref avModelVertices2);
			p2shoulderpos=kinectManager.GetJointPosition(rightid,4);
			p2shoulderToChin=(((avModelVertices1[4].x)-(p2shoulderpos.x))*(100f));
			p2rightShoulderPos=kinectManager.GetJointPosition(rightid,8);
			p2leftShoulderPos=kinectManager.GetJointPosition(rightid,4);
			//Debug.Log("leftshoulder.z=" +leftShoulderPos.z+"rightshoulder.z"+rightShoulderPos.z);
			//Debug.Log("leftshoulder-rightshoulder"+(rightShoulderPos.z-leftShoulderPos.z ));
		    p2shoulderToShoulder=(p2leftShoulderPos.z-p2rightShoulderPos.z)*20f;
			if(((p2shoulderToChin)-(p2shoulderToShoulder))<=12f)
			{
				faceMat2.material.SetColor("_Color",Color.blue);
				rightPlayerEngagement++;
			}
			else
			{
				faceMat2.material.SetColor("_Color",Color.gray);
			}
		
		}

		}

	IEnumerator wait() {
		
		//yield return new WaitForSeconds(3);

		engagement.SetActive(true);
		for(int i=0;i<5;i++)
		{
			stars[i].SetBool("isSpinning",true);
		}
		yield return new WaitForSeconds(3);	//Wait 3 Secs
		engagement.SetActive(false);
		//engagement.SetActive(false);
		
*/

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



	
}
