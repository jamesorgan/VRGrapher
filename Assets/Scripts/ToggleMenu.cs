using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

public class ToggleMenu : MonoBehaviour
{
    public SteamVR_ActionSet menuActions;
    public SteamVR_ActionSet graphActions;
    public SteamVR_Action_Boolean openMenuAction;
    public SteamVR_Action_Boolean closeMenuAction;
    public SteamVR_Input_Sources m_TargetSource;
    public Canvas menu;

    // change the active action set when the menu is active/inactive
    // SteamVR_ActionSet.Activate

    private void Awake()
    {
        //graphActions.Activate();
        menuActions.Activate();
    }

    void Update()
    {
        if (openMenuAction.GetStateUp(m_TargetSource) || closeMenuAction.GetStateUp(m_TargetSource))
        {
            if (menu.enabled == false)
            {
                menu.enabled = true;
                
                menuActions.Activate();
                graphActions.Deactivate();
                
            }
            else
            {
                menu.enabled = false;
                
                graphActions.Activate();
                menuActions.Deactivate();
            }
        }
    }

}
