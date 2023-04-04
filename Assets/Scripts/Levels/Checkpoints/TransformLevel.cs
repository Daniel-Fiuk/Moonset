using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLevel : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] TransformMode mode;

    [SerializeField] private Transform level;

    [SerializeField] private Vector3 vector;
    [SerializeField] private float acceleration, maxSpeed;

    [Space][SerializeField] private MeshCollider[] concaveColliders;

    #endregion

    #region variables

    CheckPoint checkPoint;

    bool transformed = false;

    #endregion

    private void Awake()
    {
        checkPoint = GetComponent<CheckPoint>();
    }

    private void Update()
    {
        if (checkPoint.isActivated && !transformed) switch (mode)
            {
                case TransformMode.Translate:
                    FindObjectOfType<LevelProgressManager>().StartCoroutine(TranslateLevel());
                    return;
                case TransformMode.Rotate:
                    FindObjectOfType<LevelProgressManager>().StartCoroutine(RotateLevel());
                    return;
                case TransformMode.Scale:
                    FindObjectOfType<LevelProgressManager>().StartCoroutine(ScaleLevel());
                    return;
                default:
                    Debug.LogError($"{gameObject.name} TransformMode is invalid!");
                    return;
            }
    }

    private IEnumerator TranslateLevel()
    {
        transformed = true;

        Vector3 startPos = level.position, endPos = vector;
        float t = 0, currentSpeed = 0;

        ConvertConcaveColliders(true);

        while (t < 0.99f)
        {
            t = Mathf.SmoothDamp(t, 1f, ref currentSpeed, acceleration, maxSpeed);
            level.position = Vector3.Lerp(startPos, endPos, t);
            yield return new WaitForEndOfFrame();
        }

        level.position = endPos;
        ConvertConcaveColliders(false);
    }

    private IEnumerator RotateLevel()
    {
        transformed = true;
        
        Quaternion startRotation = level.rotation, endRotation = Quaternion.Euler(vector);
        float t = 0, currentSpeed = 0;

        ConvertConcaveColliders(true);

        while (t < 0.99f)
        {
            t = Mathf.SmoothDamp(t, 1f, ref currentSpeed, acceleration, maxSpeed);
            level.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return new WaitForEndOfFrame();
        }

        level.rotation = endRotation;
        ConvertConcaveColliders(false);
    }

    private IEnumerator ScaleLevel()
    {
        transformed = true;

        Vector3 startScale = level.localScale, endScale = vector;
        float t = 0, currentSpeed = 0;

        ConvertConcaveColliders(true);

        while (t < 0.99f)
        {
            t = Mathf.SmoothDamp(t, 1f, ref currentSpeed, acceleration, maxSpeed);
            level.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return new WaitForEndOfFrame();
        }

        level.localScale = endScale;
        ConvertConcaveColliders(false);
    }

    private void ConvertConcaveColliders(bool isConvex)
    {
        foreach (MeshCollider meshCollider in concaveColliders) meshCollider.convex = isConvex;
    }
}

public enum TransformMode
{
    Translate,
    Rotate,
    Scale
}