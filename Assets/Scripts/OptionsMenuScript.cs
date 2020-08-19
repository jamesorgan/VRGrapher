using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    public Slider countSlider;
    public Text countText;
    public Text countTextMin;
    public Text countTextMax;
    public Slider offsetSlider;
    public Text offsetText;
    public Text offsetTextMin;
    public Text offsetTextMax;
    public Text totalText;
    public CreatePoints2 pointsManager;

    private void Awake()
    {
        countSlider.value = pointsManager.count;
        countText.text = countSlider.value.ToString();
        countTextMin.text = countSlider.minValue.ToString();
        countTextMax.text = countSlider.maxValue.ToString();

        countSlider.value = pointsManager.offset;
        offsetText.text = Math.Round(offsetSlider.value, 2).ToString();
        offsetTextMin.text = offsetSlider.minValue.ToString();
        offsetTextMax.text = offsetSlider.maxValue.ToString();

        totalText.text = pointsManager.total.ToString() + " points will be drawn";
    }

    public void UpdatePointsSettings()
    {
        pointsManager.count = (int)countSlider.value;
        countText.text = countSlider.value.ToString();
        pointsManager.offset = (float)Math.Round(offsetSlider.value, 2);
        offsetText.text = Math.Round(offsetSlider.value, 2).ToString();

        pointsManager.total = (int)Math.Pow(pointsManager.count * 2 + 1, 2);
        totalText.text = pointsManager.total.ToString() + " points will be drawn";
    }
}
