using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveRotation : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] Axis rotationAxis = Axis.X;
    [SerializeField] float rotationSpeed = 1f;

    #endregion

    #region variables



    #endregion]

    private void Update()
    {
        switch (rotationAxis)
        {
            case Axis.X:
                transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
                break;
            case Axis.Y:
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                break;
            case Axis.Z:
                transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
                break;
        }
    }
}

public enum Axis
{
    X, 
    Y,
    Z
}
