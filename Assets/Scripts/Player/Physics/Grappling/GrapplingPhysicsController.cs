using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GrapplingPhysicsController
{
    public static void StartGrapple(PlayerController player)
    {
        if (!player) return;

        GrapplingPhysicsObj GPO = player.grapplingPhysicsObj;

        if (Vector3.Distance(player.GetComponent<Rigidbody>().position, player.ReticleTarget().point) >= GPO.maxLength || !player.ReticleTarget().collider) return;

        switch (player.ReticleTarget().collider.tag)
        {
            case "GrappleTarget":
                StartSwing(player);
                return;
        }

    }

    private static void StartSwing(PlayerController player)
    {
        GrapplingPhysicsObj GPO = player.grapplingPhysicsObj;
        AudioClip whoosh = GPO.whooshSounds[Random.Range(0, GPO.whooshSounds.Length)];
        player.audioSource.pitch = Random.Range(GPO.whooshPitchRange.x, GPO.whooshPitchRange.y);
        player.audioSource.PlayOneShot(whoosh, GPO.whooshVolume);

        player.transform.parent = GameObject.FindObjectOfType<GameManager>().transform;

        player.grappleJoint = player.AddComponent<SpringJoint>();

        player.grappleJoint.autoConfigureConnectedAnchor = false;
        player.StartCoroutine(UpdateAnchor());
        player.StartCoroutine(ScaleGrapple(player));

        player.grappleScale = Vector3.Distance(player.transform.position, player.ReticleTarget().point);

        player.grappleJoint.maxDistance = player.grappleScale * GPO.maxDistanceMultiple;
        player.grappleJoint.minDistance = player.grappleScale * GPO.minDistanceMultiple;
        
        player.grappleJoint.spring = GPO.springForce;
        player.grappleJoint.damper = GPO.damperForce;
        player.grappleJoint.massScale = GPO.massScale;
        
        player.GetComponent<LineRenderer>().positionCount = 2;

        player.doubleJumpReady = true;

        player.grappleLinePos = player.ReticleTarget().point;

        PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Grapple, player.animator);

        IEnumerator UpdateAnchor()
        {
            Transform connectedAnchor = new GameObject().transform;
            connectedAnchor.SetPositionAndRotation(player.ReticleTarget().point, Quaternion.LookRotation(player.ReticleTarget().normal));
            connectedAnchor.SetParent(player.ReticleTarget().collider.transform);

            if (!player.grappleHookObj) player.grappleHookObj = GameObject.Instantiate(GPO.hookPrefab, connectedAnchor.position, Quaternion.identity, connectedAnchor).transform;

            while (player.grappleJoint)
            {
                player.grappleJoint.connectedAnchor = connectedAnchor.position;

                player.grappleHookObj.LookAt(player.transform);

                if (player.grappleLineRenderer.positionCount == 2)
                {
                    player.grappleLineRenderer.SetPosition(0, connectedAnchor.position);
                    player.grappleLineRenderer.SetPosition(1, player.grapplingHandObj.position);
                }

                player.grappleLinePos = connectedAnchor.position;

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator ScaleGrapple(PlayerController player)
        {
            while (player.grappleScale >= GPO.minLength)
            {
                if (!player.grappleJoint) yield break;

                if (!player.lockGrappleLineLength)
                {
                    player.grappleScale = Mathf.SmoothDamp(player.grappleScale, GPO.minLength, ref player.grappleScaleSpeed, GPO.scaleSpeed);
                    player.grappleJoint.maxDistance = player.grappleScale * GPO.maxDistanceMultiple;
                    player.grappleJoint.minDistance = player.grappleScale * GPO.minDistanceMultiple;
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public static void GrappleLunge(PlayerController player)
    {
        if (!player.grappleJoint) return;

        GrapplingPhysicsObj GPO = player.grapplingPhysicsObj;

        Vector3 vectorToAnchor = player.grappleJoint.connectedAnchor - player.transform.position;
        Vector3 lungeForce = Vector3.Lerp(vectorToAnchor * GPO.lungeForce, -Physics.gravity * vectorToAnchor.magnitude * GPO.lungeGravityComp, 0.5f);
        player.body.velocity = Vector3.Lerp(player.body.velocity, lungeForce, 0.25f);

        CancelGrapple(player);
    }

    public static void CancelGrapple(PlayerController player)
    {
        if (player.grappleJoint)
        {
            //Audio
            GrapplingPhysicsObj GPO = player.grapplingPhysicsObj;
            AudioClip zipSound = GPO.zipSounds[Random.Range(0, GPO.zipSounds.Length)];
            player.audioSource.pitch = Random.Range(GPO.zipPitchRange.x, GPO.zipPitchRange.y);
            player.audioSource.PlayOneShot(zipSound, GPO.zipVolume);
            
            //Destroy Joint Component
            GameObject.Destroy(player.grappleJoint);
            player.grappleLineOut = false;
        }
    }
}
