using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class DebugGraphics : MonoBehaviour
{
    #region components

    PlayerInput playerInput;
    #region input actions

    InputAction debugStart, textures, filterMode;

    #endregion

    [SerializeField] UniversalRendererData data;

    #endregion

    #region inspector



    #endregion

    #region variables

    bool debug = false, colourBlind = false;

    #endregion

    private void OnEnable()
    {
        playerInput = FindObjectOfType<PlayerInput>();

        debugStart = playerInput.actions["Debug Start"];
        debugStart.started += ctx => debug = true;
        debugStart.canceled += ctx => debug = false;

        textures = playerInput.actions["Textures"];
        textures.started += ctx => ToggleTextures();

        filterMode = playerInput.actions["Filter Mode"];
        filterMode.started += ctx => ToggleFilterMode();
    }

    private void ToggleTextures()
    {
        if (!debug || !this) return;
        if (!TryGetComponent<GUITextures>(out GUITextures textures)) { Debug.LogError("GRAPHICS GUITexture Component Not Attached To Camera!"); return; }
        textures.enabled = !textures.enabled;
        Debug.Log("GRAPHICS: Textures " + (textures.enabled ? "enabled!" : "disabled!"));
    }

    private void ToggleFilterMode()
    {
        if (!debug || !this) return;

        colourBlind = !colourBlind;

        data.rendererFeatures[3].SetActive(!colourBlind);
        data.rendererFeatures[4].SetActive(colourBlind);

        Debug.Log("GRAPHICS: Filter " + (colourBlind ? "normal!" : "colour blind!"));
    }
}
