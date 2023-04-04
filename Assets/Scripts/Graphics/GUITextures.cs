using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITextures : MonoBehaviour
{
    Renderer[] renderers;
    [SerializeField] Material defaultMat;

    Dictionary<Renderer, Material> renderMats = new Dictionary<Renderer, Material>();

    private void Awake()
    {
        renderers = (Renderer[])FindObjectsOfType(typeof(Renderer));

        foreach (Renderer renderer in renderers)
        {
            renderMats.Add(renderer, renderer.sharedMaterial);
        }
    }

    private void OnEnable()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = defaultMat;
        }
    }

    private void OnDisable()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.sharedMaterial = renderMats[renderer];
        }
    }
}
