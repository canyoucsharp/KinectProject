using Assets.KinectScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
//this class contains a collection of functions to determine the left and right players information
public class UserPositions : MonoBehaviour, UserObserver {

   
    private const int p1Index = 0;
    private const int p2Index = 1;

    private int userCount;

    private static Vector3 leftMeshPos;
    private static Vector3 rightMeshPos;
    private GameObject leftFace;
    private GameObject rightFace;
    private static long leftId;
    private static long rightId;

    public static long LeftId
    {
        get
        {
            return leftId;
        }

    }
    public static long RightId
    {
        get
        {
            return rightId;
        }
    }
    

    //this function assigns the meshInstances to the left and right players accordingly
    public void assignMeshesByPositions()
    {

        Vector3 p1MeshPos = MainFaceTracking.getFaceTrackingManager(p1Index).getMeshTransform();
        Vector3 p2MeshPos = MainFaceTracking.getFaceTrackingManager(p2Index).getMeshTransform();

        if (userCount==1)
            leftMeshPos = p1MeshPos;

        else if (userCount>1){
            if (p1MeshPos.x < p2MeshPos.x){
                leftMeshPos = p1MeshPos;
                leftFace = MainFaceTracking.getFaceTrackingManager(p1Index).faceModelMesh;
                rightMeshPos = p2MeshPos;
               
            }
            else {
                leftMeshPos = p2MeshPos;
                rightMeshPos = p1MeshPos;
            }
        }
    }

    public void assignFaceByPosition()
    {

    }
    //for arrow
    public void assignIdByPosition()
    {
        long p1Id = KinectManager.Instance.GetUserIdByIndex(p1Index);
        long p2Id = KinectManager.Instance.GetUserIdByIndex(p2Index);
        Vector3 p1pos = KinectManager.Instance.GetUserPosition(p1Id);
        Vector3 p2pos = KinectManager.Instance.GetUserPosition(p2Id);


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
            else if (p2pos.x >= p1pos.x)
            {
                leftId = p2Id;
                rightId = p1Id;
            }
      
        }
    }

    
    void UserObserver.updateUserPositions(int newUserCount)
    {
        Debug.Log("user count is" + newUserCount);
        userCount = newUserCount;
        assignMeshesByPositions();
        assignIdByPosition();

    }



}
*/