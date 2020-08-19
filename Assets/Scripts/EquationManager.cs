using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationManager : MonoBehaviour
{
    public Dictionary<string, string> equationDict = new Dictionary<string, string>();
    public List<string> equationList = new List<string>();
    public List<string> rawEquationList = new List<string>();
    
    private void Awake()
    {
        equationDict.Add("X", "x ");
        equationDict.Add("Y", "y ");
        equationDict.Add("+", "+ ");
        equationDict.Add("-", "- ");
        equationDict.Add("*", "* ");
        equationDict.Add("/", "/ ");
        equationDict.Add("(", "( ");
        equationDict.Add(")", ") ");
        equationDict.Add("1", "1 ");
        equationDict.Add("Var1", "var1 ");
        equationDict.Add("Var2", "var2 ");
        equationDict.Add("sin(", "(float)Math.Sin( ");
        equationDict.Add("cos(", "(float)Math.Cos( ");
        equationDict.Add("tan(", "(float)Math.Tan( ");
        equationDict.Add("ln(", "(float)Math.Log( ");
        equationDict.Add("e^(", "(float)Math.Pow(Math.E, ");
    }
}
