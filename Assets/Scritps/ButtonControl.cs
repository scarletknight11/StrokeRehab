using System;
using System.Collections;
using System.Collections.Generic;
using Assets.OVR.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonControl : MonoBehaviour
{
    public GameManager gm;
    public UnityEvent OnButtonDown;

    private bool _pressedInProgress = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Button" && !_pressedInProgress)
        {
            OnButtonDown.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Button")
        {
            _pressedInProgress = false;
        }
    }
}
