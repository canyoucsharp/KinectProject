using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GrabDropScript : MonoBehaviour 
{
	// dragable objects list
	public GameObject[] draggableObjects;


	public float dragSpeed = 3.0f;
	public Material selectedObjectMaterial;

	// public options (used by the Options GUI)
	public bool useGravity = true;
	public bool resetObjects = false;

	// info GUI text
	public GUIText infoGuiText;

	// interaction manager reference
	//private InteractionManager manager;

	// currently dragged object and its parameters
	public GameObject pause; 
	public GameObject draggedObject1 = null;
    public GameObject draggedObject2 = null;
	public GameObject pecObj = null;
	public GameObject pecObj1 = null;
	public GameObject pecObj2 = null;
	private float draggedObjectDepth;
	private Vector3 draggedObjectOffset;
	private Material draggedObjectMaterial;
	private bool player1PushingButton;
	private bool player2PushingButton;

	// initial objects' positions and rotations (used for resetting objects)
	private Vector3[] initialObjPos;
	private Quaternion[] initialObjRot;

	private float draggedX, draggedY;

	public GameObject emptyObject;
	private GameObject tempDraggedObject1;
	private GameObject tempDraggedObject2;
    List<InteractionManager> Managers = new List<InteractionManager>();
    public bool isGrabbed1
    {
		get {

            var manager = (from m in Managers
                           where
                              m.PrimaryHandEvent == InteractionManager.HandEventType.Grip
                               && m.playerIndex == 0
                           select m).FirstOrDefault();
            if (manager != null && draggedObject1!=null){
				//look through array for draggedObject1
				/*for(int i = 0; i < draggableObjects.Length; i++){
					if(draggedObject1.tag == draggableObjects[i].tag && draggedObject1.name == draggableObjects[i].name)
					{
						//Replace with Junk GameObject
						draggableObjects[i] = emptyObject;
						tempDraggedObject1 = draggableObjects[i];
					}
				}*/
                return true;
			}
            else {
				//Look through array for junk
				/*for(int i = 0; i < draggableObjects.Length; i++) {
				//replace junk with original draggedObject1
					if(draggableObjects[i].tag == "Junk")
					{
						draggableObjects[i] = tempDraggedObject1;
					}
				}
				//Send to origin*/
                return false;
			}
            //return manager.GetLastLeftHandEvent() == InteractionManager.HandEventType.Grip || 
            //    manager.GetLastRightHandEvent() == InteractionManager.HandEventType.Grip;
		}
	}

    public bool isGrabbed2
    {

        get
        {
            var manager = (from m in Managers
                           where
                             m.PrimaryHandEvent == InteractionManager.HandEventType.Grip                           
                               
                           && m.playerIndex == 1

                           select m).FirstOrDefault();
            if (manager != null && draggedObject2!=null){
				//look through array for draggedObject1
				/*for(int i = 0; i < draggableObjects.Length; i++){
					if(draggedObject2.tag == draggableObjects[i].tag && draggedObject2.name == draggableObjects[i].name)
					{
						//Replace with Junk GameObject
						draggableObjects[i] = emptyObject;
						tempDraggedObject2 = draggableObjects[i];
					}
				}*/
                return true;
			}

            else{
				//Look through array for junk
				/*for(int i = 0; i < draggableObjects.Length; i++) {
					//replace junk with original draggedObject1
					if(draggableObjects[i].tag == "Junk")
					{
						draggableObjects[i] = tempDraggedObject2;
					}
				}*/
                return false;
			}
            
        }
    }

    void Start()
    {
		
        foreach (var m in GameObject.FindGameObjectWithTag("MainCamera").GetComponents<InteractionManager>())
        {
            Managers.Add(m);
            
        }
        //Debug.Log("" + Managers.Count);
        // save the initial positions and rotations of the objects
        initialObjPos = new Vector3[draggableObjects.Length];
        initialObjRot = new Quaternion[draggableObjects.Length];

        for (int i = 0; i < draggableObjects.Length; i++)
        {
            initialObjPos[i] = draggableObjects[i].transform.position;
            initialObjRot[i] = draggableObjects[i].transform.rotation;
        }
    }

	void Update() 
	{

		transitionOnRayCast();
        if (resetObjects && draggedObject1 == null && draggedObject2 == null) 
		{
			// reset the objects as needed
			resetObjects = false;
			ResetObjects ();
		}

		// get the interaction manager instance
        //if(manager == null)
        //{
        //    manager = InteractionManager.Instance;
        //}


        foreach (var manager in Managers)            
		{
			
			if (manager != null && manager.IsInteractionInited())
            {
                Vector3 screenNormalPos = Vector3.zero;
                Vector3 screenPixelPos = Vector3.zero;
                var draggedObject = manager.playerIndex == 0 ? draggedObject1 : draggedObject2;
				pecObj = manager.playerIndex == 0 ? pecObj1 : pecObj2;
                if (draggedObject == null)
                {
                    // if there is a hand grip, select the underlying object and start dragging it.

                    screenNormalPos = manager.PrimaryHandScreenPos;
                    
					// check if there is an underlying object to be selected
                    if (screenNormalPos != Vector3.zero)
                    {
                        // convert the normalized screen pos to pixel pos
                        screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
                        screenPixelPos.y = (int)(screenNormalPos.y * Camera.main.pixelHeight);
                        Ray ray = Camera.main.ScreenPointToRay(screenPixelPos);

                        // check if there is an underlying objects
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            foreach (GameObject obj in draggableObjects)
                            {
                                if (hit.collider.gameObject == obj)
                                {
                                    // an object was hit by the ray. select it and start dragging

                                    var objState = obj.GetComponent<Zzero>();
                                    if (objState.PlayerIndex == -1)
                                    {
                                        objState.PlayerIndex = manager.playerIndex;
                                        draggedObject = obj;

                                        if (manager.playerIndex == 0 && draggedObject2 != obj)
                                        {
                                            draggedObject1 = obj;
											pecObj1=obj;
                                        }
                                        else if (manager.playerIndex == 1 && draggedObject1 != obj)
                                            {
                                                draggedObject2 = obj;
											pecObj2=obj;
                                            }
                                        else continue;
                                        //draggedObjectDepth = draggedObject.transform.position.z - Camera.main.transform.position.z;
                                        //---------------------------------------------------------------------------------
                                        //---------------------------------------------------------------------------------
                                        //Original LINE of code above, the below code is trying to restrict the dragged object's Z-axis



										draggedX = draggedObject.transform.position.x;
                                        draggedY = draggedObject.transform.position.y;

										draggedObject.transform.position.Set(draggedX, draggedY, 0);
                                        draggedObjectDepth = 0 - Camera.main.transform.position.z;
                                        draggedObjectOffset = hit.point - draggedObject.transform.position;
                                      //  Debug.Log("Dragged Object Depth: " + draggedObjectDepth);
                                      //  Debug.Log("Dragged Object Z: " + draggedObject.transform.position.z);
                                        //----------------------------------------------------------------------------------
                                        //---------------------------------------------------------------------------------
                                        // set selection material
                                        draggedObjectMaterial = draggedObject.GetComponent<Renderer>().material;
                                        draggedObject.GetComponent<Renderer>().material = selectedObjectMaterial;

                                        // stop using gravity while dragging object
                                        draggedObject.GetComponent<Rigidbody>().useGravity = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // continue dragging the object

                    // check if the object (hand grip) was released
                    bool isReleased = (manager.PrimaryHandEvent == InteractionManager.HandEventType.Release);

                    if (isReleased)
                    {
                        // restore the object's material and stop dragging the object
                        draggedObject.GetComponent<Renderer>().material = draggedObjectMaterial;
                        var objState = draggedObject.GetComponent<Zzero>();
                        if (objState.triggeredObjects.Count == 0)
                            objState.PlayerIndex = -1;
                        
                        if (useGravity)
                        {
                            // add gravity to the object
                            draggedObject.GetComponent<Rigidbody>().useGravity = true;
                        }

                        draggedObject = null;
                        if (manager.playerIndex == 0)
                        {
                            draggedObject1 = null;
                        }
                        else
                            draggedObject2 = null;

                    }

                    else
                    {
                        

					
                        screenNormalPos = manager.PrimaryHandScreenPos;
                        screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
						screenPixelPos.y =(int)(screenNormalPos.y * Camera.main.pixelHeight);
                        screenPixelPos.z = screenNormalPos.z + draggedObjectDepth;

                        Vector3 newObjectPos = Camera.main.ScreenToWorldPoint(screenPixelPos) - draggedObjectOffset;
                        

						if(draggedObject.tag=="leftdoor")
						{
							draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, 
								new Vector3(Mathf.Clamp( newObjectPos.x,-15,-6),0,draggedObject.transform.position.z), dragSpeed * Time.deltaTime);
						
							if(draggedObject.transform.position.x<=-12)
							{
								draggedObject.SetActive(false);
							}
						
						}
						if(draggedObject.tag=="rightdoor")
						{
							draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, 
								new Vector3(Mathf.Clamp( newObjectPos.x,6,15),0,draggedObject.transform.position.z), dragSpeed * Time.deltaTime);
							if(draggedObject.transform.position.x>=12)
							{
								draggedObject.SetActive(false);
							}

						
						}


						if(!(draggedObject.tag=="rightdoor"||draggedObject.tag=="leftdoor"))
						draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, 
							new Vector3(newObjectPos.x,newObjectPos.y,draggedObject.transform.position.z), dragSpeed * Time.deltaTime);

                    
					
					
					}
                }
            }
		}
	}


	public void transitionOnRayCast()
	{
		
		foreach (var manager in Managers)            
		{
			if (manager != null && manager.IsInteractionInited())
			{
				Vector3 screenNormalPos = Vector3.zero;
				Vector3 screenPixelPos = Vector3.zero;

					screenNormalPos = manager.PrimaryHandScreenPos;

					// check if there is an underlying object to be selected
					if (screenNormalPos != Vector3.zero)
					{
						// convert the normalized screen pos to pixel pos
						screenPixelPos.x = (int)(screenNormalPos.x * Camera.main.pixelWidth);
						screenPixelPos.y = (int)(screenNormalPos.y * Camera.main.pixelHeight);
						Ray ray = Camera.main.ScreenPointToRay(screenPixelPos);

						// check if there is an underlying objects
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit))
						{
						//Debug.Log("hit.collider.tag "+hit.collider.tag);
							
						if (hit.collider.tag=="pause"&&manager.playerIndex==0){
							player1PushingButton=true;
							Debug.Log("collider is hit by"+ manager.playerIndex);
							}
						else if(hit.collider.tag!="pause"&& manager.playerIndex==0)
						{
							player1PushingButton=false;
						}
						if (hit.collider.tag=="pause"&&manager.playerIndex==1)
						{	player2PushingButton=true;
							Debug.Log("collider is hit by"+ manager.playerIndex);
						}
						else if(hit.collider.tag!="pause"&& manager.playerIndex==1)
						{
							player2PushingButton=false;
						}
						}
					if(player1PushingButton&&player2PushingButton)
					{
						Debug.Log("both players pushing button");
						pause.SetActive(false);

					}
				}
			}
		}


	}

	// reset positions and rotations of the objects
	private void ResetObjects()
	{
		for(int i = 0; i < draggableObjects.Length; i++)
		{
			draggableObjects[i].GetComponent<Rigidbody>().useGravity = false;
			draggableObjects[i].GetComponent<Rigidbody>().velocity = Vector3.zero;

			draggableObjects[i].transform.position = initialObjPos[i];
			draggableObjects[i].transform.rotation = initialObjRot[i];
		}
	}




	
	void OnGUI()
	{
        var gui = infoGuiText.GetComponent<GUIText>();
        gui.text = "";
        foreach (var manager in Managers)
        {
            if (infoGuiText != null && manager != null && manager.IsInteractionInited())
            {
                string sInfo = string.Empty;

                long userID = manager.GetUserID();
                if (userID != 0)
                {
                    if (draggedObject1 != null)
                        sInfo = "Dragging the " + draggedObject1.name + " around.";
                    else
                        sInfo = "Please grab and drag an object around.";
                    if (draggedObject2 != null)
                        sInfo += "Dragging the " + draggedObject2.name + " around.";
                    else
                        sInfo += "Please grab and drag an object around.";
                }
                else
                {
                    KinectManager kinectManager = KinectManager.Instance;

                    if (kinectManager && kinectManager.IsInitialized())
                    {
                        sInfo = "Waiting for Users...";
                    }
                    else
                    {
                        sInfo = "Kinect is not initialized. Check the log for details.";
                    }
                }
                gui.text += string.Format("Player {0} ", manager.playerIndex) + (manager.GetLastLeftHandEvent() == InteractionManager.HandEventType.Grip ||
                                                            manager.GetLastRightHandEvent() == InteractionManager.HandEventType.Grip) + " isGrabbed\r\n";

                //infoGuiText.GetComponent<GUIText>().text = draggedObject.transform.position.z.ToString();
            }
        }
	}
}
