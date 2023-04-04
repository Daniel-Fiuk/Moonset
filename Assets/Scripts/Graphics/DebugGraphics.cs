using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugGraphics : MonoBehaviour
{
    #region components

    PlayerInput playerInput;
    #region input actions

    InputAction debugStart, textures;

    #endregion

    #endregion

    #region inspector



    #endregion

    #region variables

    bool debug = false;

    #endregion

    private void OnEnable()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        
        debugStart = playerInput.actions["Debug Start"];
        debugStart.started += ctx => debug = true;
        debugStart.canceled += ctx => debug = false;

        textures = playerInput.actions["Textures"];
        textures.started += ctx => ToggleTextures();
    }

    private void ToggleTextures()
    {
        if (!debug || !this) return;
        if (!TryGetComponent<GUITextures>(out GUITextures textures)) { Debug.LogError("GRAPHICS GUITexture Component Not Attached To Camera!"); return; }
        textures.enabled = !textures.enabled; 
        Debug.Log("GRAPHICS: Textures " + (textures.enabled ? "enabled!" : "disabled!"));
    }
}
