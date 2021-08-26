using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    private TMP_Text text;

    [SerializeField]
    private Slider slider;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        slider.onValueChanged.AddListener(SetValue);

        SetValue(slider.value);
    }

    private void SetValue(float value)
    {
        text.SetText(slider.value.ToString());
    }
}
