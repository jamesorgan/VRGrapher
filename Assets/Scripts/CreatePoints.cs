// ToDo
// Spin graph by holding trigger and moving controller horizontally
// Move graph by holding grip and moving controller
// Scale graph by holding grip button on both controllers and moving them together/ away
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Microsoft.CodeAnalysis.CSharp.Scripting;

public class CreatePoints : MonoBehaviour
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
    List<string> startingList = new List<string>() { "(float)Math.Cos( ", "x ", "* ", "x ", " +", "y ", "* ", "y ", ") ", "/ ", "( ", "x ", "* ", "x ", " +", "y ", "* ", "y ", ") " };
    private MethodInfo methodInfo;
    #endregion


    private void Awake()
    {
        methodInfo = DealWithEquations(startingList);

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
            UpdatePointTransform(listOfPoints, methodInfo, touchPadValue.x, touchPadValue.y);

        }

        if (Changed)
        {
            methodInfo = DealWithEquations(equationInput.rawEquationList);
            UpdatePointTransform(listOfPoints, methodInfo, touchPadValue.x, touchPadValue.y);

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
        for (int i = -count; i <= count; i++)
        {
            xpos.AddRange(Enumerable.Repeat(i, count * 2 + 1).Select(x => x * offset));
        }
        for (int i = -count; i <= count; i++)
        {
            ypos.AddRange(Enumerable.Range(-count, count * 2 + 1).Select(x => x * offset));
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

        UpdatePointTransform(listOfPoints, methodInfo, 0f, 0f);
    }

    public MethodInfo DealWithEquations(List<string> equationList)
    {
        foreach (string eqPart in equationList)
        {
            equationString += eqPart;
        }
        equationString = equationString.Substring(0, equationString.Length - 1);
        print(equationString);

        // Evaluate the expression.
        // Turn the equation into a function.
        string function_text =
            "using System;" +
            "public static class Evaluator" +
            "{" +
            "    public static float Evaluate(float x, float y, float var1, float var2)" +
            "    {" +
            "        return " + equationString + ";" +
            "    }" +
            "}";
        equationString = string.Empty;



        return null;
}

public float GetZValues(MethodInfo methodInfo, float xpos, float ypos, float var1, float var2)
{
    // Make the parameter list.
    object[] methodParams = new object[]
    {
                xpos,
                ypos,
                var1,
                var2
    };

    // Execute the method.
    float expressionResult = (float)methodInfo.Invoke(null, methodParams);

    // Add the returned result.
    return expressionResult;
}

public List<GameObject> UpdatePointTransform(List<GameObject> listOfPoints, MethodInfo methodInfo, float var1, float var2)
{
    foreach (GameObject point in listOfPoints)
    {
        point.transform.localPosition = new Vector3(point.transform.localPosition.x,
                GetZValues(methodInfo, point.transform.localPosition.x, point.transform.localPosition.z, var1, var2),
                point.transform.localPosition.z);
    }
    return listOfPoints;
}
}