using System;
using UnityEngine;
using UnityEngine.UI;

public static class UserInterfaceController
{
    public static void UpdateReticle(PlayerController player)
    {
        if (player.lockGrappleLineLength && player.grappleJoint)
        {
            SetReticleColor(player.reticle, Color.red, player.userInterfaceObj);
            SetReticleSize(player.reticle, 65f, player.userInterfaceObj);
            return;
        }

        if (!player.ReticleTarget().collider)
        {
            DefaultReticleState(player.reticle, player.userInterfaceObj);
            return;
        }
        
        switch (player.ReticleTarget().collider.tag)
        {
            case "GrappleTarget":
                if (Vector3.Distance(player.transform.position, player.ReticleTarget().point) <= player.grapplingPhysicsObj.maxLength)
                {
                    SetReticleColor(player.reticle, player.userInterfaceObj.grappleTargetReadyColor, player.userInterfaceObj);
                    SetReticleSize(player.reticle, 50f, player.userInterfaceObj);
                }
                else
                {
                    SetReticleColor(player.reticle, Color.yellow, player.userInterfaceObj);
                    SetReticleSize(player.reticle, Vector3.Distance(player.transform.position, player.ReticleTarget().point), player.userInterfaceObj);
                }
                return;
            /*case "MovableObject":
                if (player.ReticleTarget().collider && Vector3.Distance(player.transform.position, player.ReticleTarget().point) <= player.grapplingPhysicsObj.maxLength) SetReticle(player.reticle, 1f, Color.magenta, player.userInterfaceObj);
                else DefaultReticleState(player.reticle, player.userInterfaceObj);
                return;*/
            default:
                DefaultReticleState(player.reticle, player.userInterfaceObj);
                return;
        }
    }

    private static void DefaultReticleState(Image reticle, UserInterfaceObj userInterfaceObj)
    {
        SetReticleSize(reticle, 100f, userInterfaceObj);
        SetReticleColor(reticle, userInterfaceObj.defaultColor, userInterfaceObj);
    }

    private static void SetReticleSize(Image reticle, float size, UserInterfaceObj userInterfaceObj)
    {
        size = Mathf.Clamp(size, 0f, 150f);
        reticle.rectTransform.sizeDelta = Vector2.Lerp(reticle.rectTransform.sizeDelta, new Vector2(size, size), userInterfaceObj.sizeLerpSpeed * Time.unscaledDeltaTime);
    }

    private static void SetReticleColor(Image reticle, Color color, UserInterfaceObj userInterfaceObj)
    {
        reticle.color = Color.Lerp(reticle.color, color, userInterfaceObj.colorLerpSpeed * Time.unscaledDeltaTime);
    }
}
