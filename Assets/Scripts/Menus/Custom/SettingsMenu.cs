using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SettingsMenu : Menu
{
    #region components

    InputAction look;

    #endregion

    #region inspector
    
    [SerializeField] TMP_InputField msSenTxtInX, msSenTxtInY;
    [SerializeField] Slider msSenSldX, msSenSldY;

    [Space]

    #endregion

    #region variables

    [HideInInspector] public float mouseSensitivityX, mouseSensitivityY;
    [HideInInspector] public bool playerActive = true;

    #endregion

    public override void EnableMenu()
    {
        base.EnableMenu();

        #region mouse sensitivity

        if (FindObjectOfType<PlayerInput>()) look = FindObjectOfType<PlayerInput>().actions["Look"];
        else playerActive = false;

        SetVerticalSensitivity(PlayerPrefs.GetFloat("MouseVerticalSensitivity", 1f));
        SetHorizontalSensitivity(PlayerPrefs.GetFloat("MouseHorizontalSensitivity", 1f));

        #endregion
    }

    #region mouse sensitivity

    public void SetVerticalSensitivity(TMP_InputField input)
    {
        SetVerticalSensitivity(float.Parse(input.text));
    }

    public void SetVerticalSensitivity(Slider input)
    {
        SetVerticalSensitivity(input.value);
    }

    private void SetVerticalSensitivity(float input) 
    {
        mouseSensitivityY = Mathf.Clamp(input, 0.1f, 2f);
        msSenTxtInY.text = mouseSensitivityY.ToString("0.0");
        msSenSldY.value = mouseSensitivityY;

        PlayerPrefs.SetFloat("MouseVerticalSensitivity", mouseSensitivityY);

        if (playerActive) look.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scaleVector2(x={mouseSensitivityX}, y={mouseSensitivityY})" });
    }

    public void SetHorizontalSensitivity(TMP_InputField input)
    {
        SetHorizontalSensitivity(float.Parse(input.text));
    }

    public void SetHorizontalSensitivity(Slider input)
    {
        SetHorizontalSensitivity(input.value);
    }
    
    public void SetHorizontalSensitivity(float input)
    {
        mouseSensitivityX = Mathf.Clamp(input, 0.1f, 2f);
        msSenTxtInX.text = mouseSensitivityX.ToString("0.0");
        msSenSldX.value = mouseSensitivityX;

        PlayerPrefs.SetFloat("MouseHorizontalSensitivity", mouseSensitivityX);

        if (playerActive) look.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scaleVector2(x={mouseSensitivityX}, y={mouseSensitivityY})" });
    }

    #endregion
}
