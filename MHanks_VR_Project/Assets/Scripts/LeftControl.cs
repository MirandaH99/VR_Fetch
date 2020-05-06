using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;



public class LeftControl : MonoBehaviour
{
    InputDevice Device;

    // Start is called before the first frame update
    public List<InputDevice> controllerDevices = new List<InputDevice>();
    public bool isLeft = true;
    public XRNode NodeHand;

    public GameObject StraitRenderObject;
    public GameObject ArcRenderObject;
    public ArcRender arcRender;
    public StraitLineRender straitRender;

    public TargetableObject validTarget;
    RaycastHit hit;

    public Camera cam;

    public GrabbableObject heldObj;
    public GameObject GrabPoint;
    public bool InTeleportMode = false;

    public const float LoThresh = 0.1f;
    public bool triggerButton;
    public float triggerValue;
    public Vector2 joyStickRaw;
    public bool joyStickClick;
    public bool gripButton;
    public float gripValue;
    public bool touchButton;
    public bool touched;
    public Vector2 touchRaw;

    public bool grabbing = false;
    public float grabRange = 3.5f;

    public float movementSpeed = 2f;
    public float movementDeadzone = 0.15f;

    public float turningAngle = 60f;
    public float turnActzone = .75f;
    public bool turnIsAvailable = true;

    Rigidbody rb; 
    public bool canRaycast = true;
    public bool canArc = false;
    public float rayMaxDistance = 3.5f;
    public bool canInteract = true;

    public bool uiIsShowing;

    public float throwVariable = 5f;

    BallScript bs;

    public bool throwing;
    Rigidbody otherRB;
    public Vector3 HandVelocity;
    public Vector3 HandAngularVelocity;

    public GameObject playerPrefab;

    DaBox box;
    RespawnButton boxResp;
    TargetableObject targetable;
    LineRenderer lr;

    void Start()
    {
        if (isLeft) { NodeHand = XRNode.LeftHand; }
        else { NodeHand = XRNode.RightHand; }

        rb = gameObject.GetComponentInParent<Rigidbody>();

        playerPrefab = rb.gameObject;
        straitRender = gameObject.GetComponentInChildren<StraitLineRender>();
        StraitRenderObject = straitRender.gameObject;
        arcRender.StartObject = gameObject;

        throwing = false;
        otherRB = null;
        
        NoInput();
    }


    // Update is called once per frame
    void Update()
    {
        straitRender.lr.startColor = Color.red;
        straitRender.lr.endColor = Color.red;

        GetInput();
        PerformMove(joyStickRaw.y);
        PerformTurn(joyStickRaw.x);

        if (heldObj)
        {
            GrabMode();
            return;
        }

        if (triggerButton)
        {
            InTeleportMode = true;
        }
     
        Debug.DrawRay(transform.position, transform.forward * rayMaxDistance, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, rayMaxDistance))
        {
            validTarget = hit.collider.GetComponentInParent<TargetableObject>();

            if (validTarget is TeleportableLocation && InTeleportMode)
            {
                straitRender.lr.startColor = Color.blue;
                straitRender.lr.endColor = Color.blue;
                TeleportMode();
                return;
            }

            if (hit.distance > grabRange)
            {
                canInteract = false;
                straitRender.lr.startColor = Color.blue;
                straitRender.lr.endColor = Color.blue;
                return;
            }

            if (validTarget)
            {
                HasValidTarget();
                return;
            }
            NoValidTarget();
            return;
        }
        NoRayCasthit();
    }

    private void FixedUpdate()
    {
        List<XRNodeState> nodes = new List<XRNodeState>();
        InputTracking.GetNodeStates(nodes);
        if (throwing)
        {
            if(gameObject.transform != null)
            {
                foreach (XRNodeState ns in nodes)
                {
                            Device.TryGetFeatureValue(CommonUsages.deviceVelocity, out HandVelocity);
                            Device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out HandAngularVelocity);
                            otherRB.velocity = HandVelocity * throwVariable;
                            otherRB.angularVelocity = HandAngularVelocity;
                }
            }
            else
            {
                otherRB.velocity = HandVelocity;
                otherRB.angularVelocity = HandAngularVelocity;
                return;
            }
        }
        throwing = false;
    }
    void HasValidTarget()
    {
        StraitRenderObject.SetActive(true);
        ArcRenderObject.SetActive(false);
        straitRender.StartObject = gameObject;
        straitRender.EndObject = hit.point;

        straitRender.lr.startColor = Color.green;
        straitRender.lr.endColor = Color.green;

        if ((validTarget is GrabbableObject) && gripButton)
        {
            grab(validTarget as GrabbableObject);
            return;
        }

        if ((validTarget is InteractableObject) && triggerButton)      
        {
            (validTarget as InteractableObject).OnInteract();
            return; 
        } 
    }

    void TeleportMode()
    {
        if (uiIsShowing)
        {
            if (isLeft)
            {
                Player.Instance.LTeleportImage.enabled = true;
            }
            else
            {
                Player.Instance.RTeleportImage.enabled = true;
            }
        }

        StraitRenderObject.SetActive(false);
        ArcRenderObject.SetActive(true);
        arcRender.StartObject = gameObject;
        arcRender.EndObject = hit.point;

        if (!triggerButton)
        {
            PerformTeleport();
            InTeleportMode = false;
        }
    }

    void PerformTeleport()
    {
        if (uiIsShowing)
        {
            if (isLeft)
            {
                Player.Instance.setImageToFalse(Player.Instance.LTeleportImage);
            }
            else
            {
                Player.Instance.setImageToFalse(Player.Instance.RTeleportImage);
            }
        }

        //when trigger is released:
        if (validTarget)
        {
            playerPrefab.transform.position = hit.point;

            StraitRenderObject.SetActive(true);
            ArcRenderObject.SetActive(false);
        }
    }

    void NoRayCasthit()
    {
        StraitRenderObject.SetActive(true);
        ArcRenderObject.SetActive(false);
        arcRender.StartObject = gameObject;
        arcRender.EndObject = gameObject.transform.position + (gameObject.transform.forward * rayMaxDistance);

        if (!triggerButton)
        {
            InTeleportMode = false;
        }
    }

    void NoValidTarget()
    {
        StraitRenderObject.SetActive(true);
        ArcRenderObject.SetActive(false);
        arcRender.StartObject = gameObject;
        arcRender.EndObject = hit.point;
    }

    void GrabMode()
    {
        if (uiIsShowing)
        {
            if (isLeft)
            {
                Player.Instance.LGrabImage.enabled = true;
            }
            else
            {
                Player.Instance.RGrabImage.enabled = true;
            }
        }

        StraitRenderObject.SetActive(false);
        ArcRenderObject.SetActive(false);

        if (!gripButton)
        {
            drop();
        }
    }

    void grab(GrabbableObject grabbedObject)
    {
        heldObj = grabbedObject;
        heldObj.isGrabbed = true;
        heldObj.holdingController = gameObject;

        bs = heldObj.GetComponent<BallScript>();
        if(bs)
        {
            bs.currentState = BallScript.ToyStates.Held;
        }

        heldObj.GetComponent<Rigidbody>().useGravity = false;
        heldObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        heldObj.GetComponent<Rigidbody>().isKinematic = true;

        heldObj.GetComponent<Collider>().isTrigger = true;

        heldObj.transform.position = GrabPoint.transform.position;
        heldObj.transform.rotation = GrabPoint.transform.rotation;
        heldObj.transform.parent = GrabPoint.transform;

        //otherRB = heldObj.GetComponent<Rigidbody>();
        throwing = false;
    }

    void drop()
    {
        if (uiIsShowing)
        {
            if (isLeft)
            {
                Player.Instance.setImageToFalse(Player.Instance.LGrabImage);
            }
            else
            {
                Player.Instance.setImageToFalse(Player.Instance.RGrabImage);
            }
        }

        bs = heldObj.GetComponent<BallScript>();
        if (bs)
        {
            bs.currentState = BallScript.ToyStates.Thrown;
        }

        heldObj.isGrabbed = false;
        heldObj.holdingController = null;

        heldObj.GetComponent<Rigidbody>().useGravity = true;
        heldObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        heldObj.GetComponent<Rigidbody>().isKinematic = false;

        heldObj.GetComponent<Collider>().isTrigger = false; 

        otherRB = heldObj.GetComponent<Rigidbody>();

        heldObj.transform.parent = null;

        heldObj = null;
        throwing = true;
    }

    void NoInput()
    {
        triggerButton = false;
        triggerValue = 0;
        joyStickRaw = Vector2.zero;
        joyStickClick = false;
        gripValue = 0;
        gripButton = false;
        touchButton = false;
        touched = false;
        touchRaw = Vector2.zero;
    }
    void GetInput()
    {
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(NodeHand, controllerDevices);
        if (controllerDevices.Count == 1)
        {
            Device = controllerDevices[0];
            transform.localPosition =  InputTracking.GetLocalPosition(NodeHand);
            
            transform.rotation = playerPrefab.transform.rotation * InputTracking.GetLocalRotation(NodeHand);

            Device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButton);
            Device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
            Device.TryGetFeatureValue(CommonUsages.primary2DAxis, out joyStickRaw);
            Device.TryGetFeatureValue(CommonUsages.thumbrest, out joyStickClick);
            Device.TryGetFeatureValue(CommonUsages.grip, out gripValue);
            Device.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);
            Device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out touchRaw);
            Device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out touchButton);
            Device.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out touched);

            if (Mathf.Abs(joyStickRaw.x) <= LoThresh)
                joyStickRaw.x = 0;
            if (Mathf.Abs(joyStickRaw.y) <= LoThresh)
                joyStickRaw.y = 0;
        }
        else
        {
            NoInput();
            // asign default values
        }
    }

    public void PerformMove(float value)
    {
         Vector3 newVelocity = Vector3.zero;
        if ((Mathf.Abs(value) >= movementDeadzone))
        {
            newVelocity = (value * movementSpeed * cam.transform.forward);
            if (newVelocity.y > 0)
            {
                newVelocity.y = 0;
            }
        }
        else if(Mathf.Abs(value) < movementDeadzone)
        {
            return;
        }
       
        rb.velocity = newVelocity;
    }

    public void PerformTurn(float value)
    {
        // Above .75
        if (Mathf.Abs(value) >= turnActzone)
        {            
            if (turnIsAvailable)
            {
                float direction = 1; 
                if (value < 0)
                {
                    direction = -1;
                }                               
                // Perform Turn; 
                Vector3 PlayerRotation = playerPrefab.transform.eulerAngles;
                float preturnY = PlayerRotation.y; 
                PlayerRotation.y += turningAngle * direction;
                
                playerPrefab.transform.eulerAngles = PlayerRotation;
                gameObject.transform.eulerAngles = PlayerRotation;
                //Debug.Log("======== Turn");
                //Debug.Log("Value    : " + value);
                //Debug.Log("direction: " + direction);
                //Debug.Log("Pre Y    : " + preturnY);
                //Debug.Log("Post Y   : " + PlayerRotation.y);

                // set the controling bool to false so we don't keep turning
                turnIsAvailable = false;
            }
        }
        // Below .75 
        else
        {
            turnIsAvailable = true;
        }
    }
 }
