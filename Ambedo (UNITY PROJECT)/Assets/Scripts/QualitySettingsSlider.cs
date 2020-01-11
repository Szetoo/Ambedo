using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// Callback executed when the value of the slider is changed.
    /// </summary>

    public class QualitySettingsSlider : MonoBehaviour
    {
        public Slider mainSlider;
        public void Start()
        {
            //Adds a listener to the main slider and invokes a method when the value changes.
            mainSlider.onValueChanged.AddListener(delegate { Debug.Log(mainSlider.value); });

            QualitySettings.SetQualityLevel((int)mainSlider.value);

            Debug.Log(QualitySettings.GetQualityLevel());
        }
    }
}

