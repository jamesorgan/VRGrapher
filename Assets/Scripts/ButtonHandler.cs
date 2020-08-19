using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public InputField outputText;
    public Text debugText;
    public EquationManager equationManager;

    public void addText()
    {
        if (this.GetComponentInChildren<Text>().text == "DEL")
        {
            //print("character deleted");
            equationManager.equationList.RemoveAt(equationManager.equationList.Count - 1);
            equationManager.rawEquationList.RemoveAt(equationManager.rawEquationList.Count - 1);
            outputText.text = string.Empty;
            debugText.text = string.Empty;
            for (int i = 0; i < equationManager.equationList.Count; i++)
            {
                outputText.text += equationManager.equationList[i];
                debugText.text += equationManager.rawEquationList[i];
            }
        }
        else if (this.GetComponentInChildren<Text>().text == "AC")
        {
            //print("deleted all characters");
            equationManager.equationList.Clear();
            equationManager.rawEquationList.Clear();
            outputText.text = string.Empty;
            debugText.text = string.Empty;
        }
        else
        {
            //print("character added");
            // this = the button that this script is added as a component to
            // Getcomponentinchildren<Text> = the child of the button that is type text
            equationManager.equationList.Add(this.GetComponentInChildren<Text>().text);
            equationManager.rawEquationList.Add(equationManager.equationDict[this.GetComponentInChildren<Text>().text]);
            outputText.text = string.Empty;
            debugText.text = string.Empty;
            for (int i = 0; i < equationManager.equationList.Count; i++)
            {
                outputText.text += equationManager.equationList[i];
                debugText.text += equationManager.rawEquationList[i];
            }
        }
    }
}
// THIS CANNOT BE ON EACH BUTTON OR EACH BUTTON HAS ITS OWN RAWEQUATIONLIST!!!!!!!