using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRControlPosition : MonoBehaviour
{
    public GameObject rightHand;
    public SteamVR_Action_Boolean moveAction;

    private Coroutine routine;
    private Vector3 startPosition;
    private Vector3 nextPosition;


    private void Update()
    {
        if (moveAction.GetStateDown(SteamVR_Input_Sources.RightHand) == true && moveAction.GetStateDown(SteamVR_Input_Sources.LeftHand) != true)
        {
            BeginMovingObject();
        }

        if (moveAction.GetState(SteamVR_Input_Sources.RightHand) == true && moveAction.GetState(SteamVR_Input_Sources.LeftHand) != true)
        {
            transform.position += GetMovementDelta() * 2;
        }

        if (moveAction.GetStateUp(SteamVR_Input_Sources.RightHand) == true && moveAction.GetStateUp(SteamVR_Input_Sources.LeftHand) != true)
        {
            FinishMovingObject();
        }
    }

    public void BeginMovingObject()
    {
        FinishMovingObject();

        routine = StartCoroutine(ControlMovement());
    }

    public void FinishMovingObject()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    public Vector3 GetMovementDelta()
    {
        if (startPosition != Vector3.zero && routine != null)
        {
            return nextPosition - startPosition;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public IEnumerator ControlMovement()
    {
        startPosition = Vector3.zero;
        nextPosition = rightHand.transform.position;

        while (true)
        {
            yield return new WaitForEndOfFrame();
            startPosition = nextPosition;
            nextPosition = rightHand.transform.position;
        }
    }
}
