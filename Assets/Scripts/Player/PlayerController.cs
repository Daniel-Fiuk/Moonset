using Cinemachine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    #region components

    [HideInInspector] public GameManager gameManager;

    [HideInInspector] public PlayerInput playerInput;
    #region input actions

    [HideInInspector] public InputAction 
        movement, 
        look,
        jump, 
        sprint, 
        grapple, 
        pause, 
        slowMotionAim,
        stomp,
        respawn;

    [HideInInspector] public AudioSource audioSource;

    #endregion

    [HideInInspector] public Rigidbody body;
    [HideInInspector] public CapsuleCollider capsule;

    #endregion

    #region inspector
    
    public MovementPhysicsObj movementPhyObj;
    public GrapplingPhysicsObj grapplingPhysicsObj;

    public Transform grapplingHandObj;
    
    public PlayerPreferencesObj playerPreferencesObj;
    public UserInterfaceObj userInterfaceObj;

    public Image reticle;

    public TrailRenderer[] speedTrails;

    #endregion

    #region variables

    //states
    [HideInInspector]
    public bool
        actionStarted = false,
        isGrounded,
        doubleJumpReady,
        isSprinting,
        jumpCooldown,
        isSlowed;
    
    [HideInInspector] public float slowT;

    //grappling
    [HideInInspector] public Animator animator;
    [HideInInspector] public SpringJoint grappleJoint;
    [HideInInspector] public Transform grappleHookObj;
    [HideInInspector] public LineRenderer grappleLineRenderer;

    [HideInInspector] public float grappleScale, grappleScaleSpeed, grappleLineLength;
    [HideInInspector] public Vector3 grappleLinePos, grappleLineLengthSpeed;
    [HideInInspector] public bool grappleLineOut, lockGrappleLineLength;

    //air movement
    [HideInInspector] public Vector3 airStrafeSpeed;

    //ground
    [HideInInspector] public Vector3 groundNormal;
    [HideInInspector] public Collider groundCollider;
    [HideInInspector] public string groundTypeTag;

    //camera
    [HideInInspector] public float camFollowVelocity;
    [HideInInspector] public Transform camFollowTargetTransform;
    [HideInInspector] public CinemachineVirtualCamera vCam;

    [HideInInspector] public Vector3 respawnPoint;

    [HideInInspector] public AstraTrack astraTrack;
    [HideInInspector] public AstraTimeManipulation astraTimeManipulation;

    float trailT = 0;
    float defaultFOV, newFOV;

    #endregion


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        #region input actions

        movement = playerInput.actions["Movement"];

        look = playerInput.actions["Look"];
        look.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scaleVector2(x={PlayerPrefs.GetFloat("MouseHorizontalSensitivity")}, y={PlayerPrefs.GetFloat("MouseVerticalSensitivity")})" });

        pause = playerInput.actions["Pause"];
        pause.started += obj => gameManager.TogglePause();

        jump = playerInput.actions["Jump"];
        jump.started += obj => MovementPhysicsController.Jump(this);

        sprint = playerInput.actions["Sprint"];
        sprint.started += obj => MovementPhysicsController.SprintPerformed(this);
        sprint.started += obj => lockGrappleLineLength = true;
        sprint.canceled += obj => lockGrappleLineLength = false;

        grapple = playerInput.actions["Grapple"];
        grapple.started += obj => GrapplingPhysicsController.StartGrapple(this);
        grapple.canceled += obj => GrapplingPhysicsController.CancelGrapple(this);

        slowMotionAim = playerInput.actions["SlowMotionAim"]; 
        slowMotionAim.started += obj => SlowMotionController.StartSlowMotionCoroutine(this);
        slowMotionAim.canceled += obj => SlowMotionController.StopSlowMotionCoroutine(this);

        stomp = playerInput.actions["Stomp"];
        stomp.started += obj => MovementPhysicsController.Stomp(this);

        respawn = playerInput.actions["Respawn"];
        respawn.started += obj => Die();

        #endregion

        gameManager = FindObjectOfType<GameManager>();

        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        grappleLineRenderer = GetComponent<LineRenderer>();

        camFollowTargetTransform = transform.Find("Follow Target").transform;
        vCam = FindObjectOfType<CinemachineVirtualCamera>();

        respawnPoint = transform.position;

        SlowMotionController.SetUpVolumeVignette(this);

        audioSource = GetComponent<AudioSource>();

        astraTrack = FindObjectOfType<AstraTrack>();
        astraTimeManipulation = FindObjectOfType<AstraTimeManipulation>();
        astraTrack.layeringVal = 0.1f;

        defaultFOV = vCam.m_Lens.FieldOfView;
    }

    private void Update()
    {
        float camFollowTargetY = Mathf.SmoothDamp(camFollowTargetTransform.localPosition.y, body.velocity.y / 50, ref camFollowVelocity, 0.02f);
        camFollowTargetY = Mathf.SmoothDamp(camFollowTargetTransform.localPosition.y, body.velocity.y / 50, ref camFollowVelocity, 0.02f);
        camFollowTargetTransform.localPosition = new Vector3(0, camFollowTargetY, 0);

        MovementPhysicsController.Movement(this);
        UserInterfaceController.UpdateReticle(this);

        if (transform.position.y <= gameManager.minLevelYKillLevel || transform.position.y >= gameManager.maxLevelYKillLevel) Die();

        if (!isGrounded)
        {
            astraTrack.layeringVal = Mathf.Lerp(astraTrack.layeringVal, 1.0f, Time.deltaTime);
        }
        else
        {
            Vector2 movementInput = movement.ReadValue<Vector2>();

            if (movementInput.magnitude > 0.1f)
            {
                astraTrack.layeringVal = Mathf.Lerp(astraTrack.layeringVal, 0.5f, Time.deltaTime);
            }
            else
            {
                astraTrack.layeringVal = Mathf.Lerp(astraTrack.layeringVal, 0.1f, Time.deltaTime);
            }
        }

        bool movingFast = body.velocity.magnitude > movementPhyObj.speedEffectThreshold;
        trailT += Time.deltaTime * 0.5f * (movingFast ? 1 : -1);
        trailT = Mathf.Clamp01(trailT);
        
        foreach (TrailRenderer renderer in speedTrails)
        {
            renderer.time = Mathf.Lerp(0.0f, 0.5f, trailT);
            renderer.enabled = trailT >= 0.02f;
            //Debug.Log(renderer.enabled);
        }

        //Debug.Log(movingFast + "  " + (trailT >= 0.02f) + " " + trailT);

        newFOV = Mathf.Lerp(newFOV, defaultFOV + (body.velocity.magnitude - movementPhyObj.speedEffectThreshold) * 2, Time.deltaTime);
        vCam.m_Lens.FieldOfView = Mathf.Lerp(defaultFOV, Mathf.Clamp(newFOV, defaultFOV, defaultFOV + 10), trailT);
    }

    private void LateUpdate()
    {
        camFollowTargetTransform.rotation = Camera.main.transform.rotation;
        GrappleLineAnimationController.DrawGrappleLine(this);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && !jumpCooldown)
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y >= 0.5f)
                {
                    isGrounded = true;
                    doubleJumpReady = true;
                    PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Landed, animator);
                }
            }
        }

        if (collision.contacts[0].normal.y >= 0.5f)
        {
            groundNormal = collision.contacts[0].normal;
            groundTypeTag = collision.contacts[0].otherCollider.tag;
            groundCollider = collision.contacts[0].otherCollider;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isGrounded && !jumpCooldown)
        {
            StartCoroutine(CoyoteTime());
            IEnumerator CoyoteTime()
            {
                yield return new WaitForSeconds(movementPhyObj.coyoteTime);

                isGrounded = false;
                groundCollider = null;
            }
        }

        if (transform.parent != gameManager.transform)
        {
            transform.parent = gameManager.transform;
        }
    }

    public RaycastHit ReticleTarget()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit cameraHit);
        if (!cameraHit.collider) return new RaycastHit();
        
        Physics.Raycast(transform.position, cameraHit.point - body.position, out RaycastHit grappleTarget);

        Debug.DrawLine(Camera.main.transform.position, cameraHit.point, Color.cyan);
        Debug.DrawLine(transform.position, grappleTarget.point, Color.yellow);

        return grappleTarget;
    }

    public void Die()
    {
        if (!gameManager || respawnPoint == null) return;
        gameManager.deathCount++;
        transform.position = respawnPoint;
        body.velocity = Vector3.zero;
        
        GrapplingPhysicsController.CancelGrapple(this);
        grappleLineLength = 0;
    }
}