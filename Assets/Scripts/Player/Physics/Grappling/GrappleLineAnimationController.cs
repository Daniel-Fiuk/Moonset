using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GrappleLineAnimationController
{
    public static void DrawGrappleLine(PlayerController player)
    {
        GrapplingPhysicsObj GPO = player.grapplingPhysicsObj;
        LineRenderer line = player.grappleLineRenderer;

        float drawSpeed = Time.deltaTime * GPO.drawSpeed / Vector3.Distance(player.transform.position, player.grappleLinePos);

        Transform lineOrigin = player.grapplingHandObj;
        Vector3 lineVector = player.grappleLinePos - lineOrigin.position;
        int sampleRate = GPO.sampleRate;

        if (player.grappleJoint)
        {
            if (line.positionCount != GPO.sampleRate) line.positionCount = GPO.sampleRate;
            player.grappleLineLength = Mathf.Clamp(player.grappleLineLength + drawSpeed, 0, 1);

            for (int i = 0; i < GPO.sampleRate; i++)
            {
                Vector3 offset = Vector3.up * GPO.curveShape.Evaluate((float)i / (float)sampleRate) * GPO.curveSize * (1 - player.grappleLineLength) * (player.grappleLineLength) * lineVector.magnitude;

                line.SetPosition(i, Vector3.Lerp(lineOrigin.position, lineOrigin.position + (lineVector * player.grappleLineLength), (float)i / (float)sampleRate) + offset);
                if (i == GPO.sampleRate - 1) player.grappleHookObj.position = line.GetPosition(i);

                if (player.grappleLineLength == 1 && !player.grappleLineOut) 
                {
                    player.grappleLineOut = true;
                    AudioClip clickSound = GPO.hookSounds[Random.Range(0, GPO.hookSounds.Length)];
                    player.audioSource.pitch = Random.Range(0.9f, 1.1f);
                    player.audioSource.PlayOneShot(clickSound, GPO.hookVolume);
                }
            }
        }
        else if (player.grappleLineLength > 0)
        {
            if (line.positionCount != GPO.sampleRate) line.positionCount = GPO.sampleRate;
            player.grappleLineLength = Mathf.Clamp(player.grappleLineLength - drawSpeed, 0, 1);

            for (int i = 0; i < GPO.sampleRate; i++)
            {
                line.SetPosition(i, Vector3.Lerp(lineOrigin.position, lineOrigin.position + (lineVector * player.grappleLineLength), (float)i / (float)sampleRate));
                if (i == GPO.sampleRate - 1) player.grappleHookObj.position = line.GetPosition(i);
            }
        }
        else if (line.positionCount != 0)
        {
            line.positionCount = 0;
            if (player.grappleHookObj) GameObject.Destroy(player.grappleHookObj.gameObject);
        }
    }
}
