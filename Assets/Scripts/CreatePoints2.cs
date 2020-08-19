// ToDo
// Get the new equation handling to work
// Compile once
// Make a mesh instead of many cubes/planes
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

public class CreatePoints2 : MonoBehaviour
{
    public SteamVR_Action_Vector2 touchPadAction;

    // Constructor
    #region Point Settings
    /// <summary>
    /// Number of cubes on one axis
    /// Total number of cubes will be (count*2+1)^2
    /// </summary>
    public int count = 50;
    public int total;
    /// <summary>
    /// Distance between cubes
    /// </summary>
    public float offset = 0.05f;

    /// <summary>
    /// The appearance of the point
    /// </summary>
    public GameObject point;

    /// <summary>
    /// Where the points will be created
    /// </summary>
    public GameObject origin;
    #endregion

    /// <summary>
    /// List of x coordinates of points
    /// </summary>
    private List<float> xpos = new List<float>();
    /// <summary>
    /// List of y coordinates of points
    /// </summary>
    private List<float> ypos = new List<float>();
    /// <summary>
    /// List of z positions
    /// </summary>
    private List<float> zpos = new List<float>();
    /// <summary>
    /// List of all the points in the scene
    /// </summary>
    private List<GameObject> listOfPoints = new List<GameObject>();

    #region Equation Settings
    public EquationManager equationInput;
    public Button equalsButton;
    public bool Changed { get; set; }
    private string equationString = string.Empty;

    private ScriptRunner<float> eqScript;
    
    List<string> startingList = new List<string>() { "(float)Math.Cos( ", "x ", "* ", "x ", " +", "y ", "* ", "y ", ") ", "/ ", "( ", "x ", "* ", "x ", " +", "y ", "* ", "y ", ") " };
    #endregion

    private void Awake()
    {
        DealWithEquations(startingList);

        InitialisePoints();
    }

    private void Update()
    {
        //Check for inputs
        //Update z coordinates of each point accordingly
        //Check for more inputs

        Vector2 touchPadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);
        if (touchPadValue != Vector2.zero)
        {
            // There needs to be some function here that makes the equation live
            // (foreach z in Listz) {Listz = fun(touchpadvalue1, touchpadvalue2, Listx, Listy)}
            GetZValues(listOfPoints, touchPadValue.x, touchPadValue.y);

        }

        if (Changed)
        {
            DealWithEquations(equationInput.rawEquationList);
            GetZValues(listOfPoints, touchPadValue.x, touchPadValue.y);
            
            Changed = false;
        }

    }

    public void InitialisePoints()
    {
        // Total number of points generated
        total = (int)Math.Pow(count * 2 + 1, 2);

        // This can now be run after start up so all the lists are cleared
        xpos.Clear();
        ypos.Clear();
        zpos.Clear();
        // and all points removed from the scene
        foreach (GameObject point in listOfPoints)
        {
            Destroy(point);
        }
        listOfPoints.Clear();

        // Generate a List of integer positions using Enumerable.Range(start, number of numbers) and Repeat
        // also multiply by the scale
        for (int i = -count; i <= count; i++)
        {
            xpos.AddRange(Enumerable.Repeat(i, count * 2 + 1).Select(x => x * offset * GetComponentInParent<VRControlScale>().transform.localScale.x));
        }
        for (int i = -count; i <= count; i++)
        {
            ypos.AddRange(Enumerable.Range(-count, count * 2 + 1).Select(x => x * offset * GetComponentInParent<VRControlScale>().transform.localScale.y));
        }

        // Create the initial list of z positions
        zpos.AddRange(Enumerable.Repeat(0.0f, total));

        //Debug.Log(total);

        // Instantiate all the points
        // They are parented to the graph :)
        for (int i = 0; i < total; i++)
        {
            listOfPoints.Add(GameObject.Instantiate(point, 
                (new Vector3(xpos[i], zpos[i], ypos[i]) + origin.transform.position), 
                Quaternion.identity, 
                origin.transform));
        }

        GetZValues(listOfPoints, 0f, 0f);
    }

    public void DealWithEquations(List<string> equationList)
    {
        foreach (string eqPart in equationList)
        {
            equationString += eqPart;
        }
        equationString = equationString.Substring(0, equationString.Length - 1);
        print(equationString);

        // Code should only be compiled once
        eqScript = CSharpScript.Create<float>(
            code: equationString,
            options: ScriptOptions.Default.WithImports("System"),
            globalsType: typeof(Globals)).CreateDelegate();
        // Could use System.Math instead

    }

    public void GetZValues(List<GameObject> listOfPoints, float var1, float var2)
    {
        print("attempting to calculate z values");
        
            foreach (GameObject point in listOfPoints)
            {
                float eqResult = eqScript.Invoke(globals: new Globals
                {
                    x = point.transform.localPosition.x,
                    y = point.transform.localPosition.z,
                    var1 = var1,
                    var2 = var2
                }).Result;

                // Add the returned result.
                point.transform.localPosition = new Vector3(point.transform.localPosition.x,
                        eqResult,
                        point.transform.localPosition.z);
            }

            equationString = string.Empty;
    }
}
public class Globals
{
    public float x;
    public float y;
    public float var1;
    public float var2;
}