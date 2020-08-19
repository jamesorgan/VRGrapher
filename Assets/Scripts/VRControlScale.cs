using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRControlScale : MonoBehaviour
{
    public SteamVR_Action_Boolean scaleAction;

    public GameObject rightHand;
    public GameObject leftHand;

    private Vector3 leftStartPos;
    private Vector3 leftNextPos;
    private Vector3 rightStartPos;
    private Vector3 rightNextPos;

    private float initialScale;

    private float initialDistance;
    private float newDistance;

    private float ratio;

    private void Update()
    {
        if (scaleAction.GetStateDown(SteamVR_Input_Sources.LeftHand) || scaleAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            leftStartPos = leftHand.transform.position;
            rightStartPos = rightHand.transform.position;

            initialDistance = (leftStartPos - rightStartPos).magnitude;

            initialScale = transform.localScale.x;
        }

        if (scaleAction.GetState(SteamVR_Input_Sources.LeftHand) && scaleAction.GetState(SteamVR_Input_Sources.RightHand))
        {
            //print("leftstart: " + leftStartPos + "\n" + "rightstart: " + rightStartPos + "\n");
            //print("leftnxt: " + leftNextPos + "\n" + "rightnxt: " + rightNextPos);
            //print("initialDis: " + initialDistance + "\n" + "newDis: " + newDistance + "\n");

            leftNextPos = leftHand.transform.position;
            rightNextPos = rightHand.transform.position;

            newDistance = (leftNextPos - rightNextPos).magnitude;

            ratio = initialScale * newDistance / initialDistance;

            //print("Ratio: " + ratio);
            transform.localScale = new Vector3(ratio, ratio, ratio);
        }
    }
}
