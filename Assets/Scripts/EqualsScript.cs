using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualsScript : MonoBehaviour
{
    public CreatePoints2 destination;

    public void UpdateEquation()
    {
        destination.Changed = true;
    }
    // If the equation is changed then change the current equation
    // text.changed = true
    //    
}
