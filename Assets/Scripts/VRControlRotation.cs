using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRControlRotation : MonoBehaviour
{
    public GameObject rightHand;
    public SteamVR_Action_Boolean rotateAction;
    private Vector3 velocityEstimate;

    private void Update()
    {
        if (rotateAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            rightHand.GetComponent<VelocityEstimator>().BeginEstimatingVelocity();
            velocityEstimate = rightHand.GetComponent<VelocityEstimator>().GetVelocityEstimate();
        }

        if (rotateAction.GetState(SteamVR_Input_Sources.Any))
        {
            // Make the graph rotate to face the controller
            // triangles ;)
        }

        if (rotateAction.GetStateUp(SteamVR_Input_Sources.Any))
        {
            velocityEstimate = rightHand.GetComponent<VelocityEstimator>().GetVelocityEstimate();

            //print(velocityEstimate.magnitude);

            if (velocityEstimate.magnitude <= 0.05)
            {
                velocityEstimate = Vector3.zero;
            }
            else if(velocityEstimate.magnitude >= 1)
            {
                velocityEstimate = Vector3.one;
            }

            rightHand.GetComponent<VelocityEstimator>().FinishEstimatingVelocity();
        }
        transform.Rotate(new Vector3(0, velocityEstimate.magnitude, 0));
    }
}
