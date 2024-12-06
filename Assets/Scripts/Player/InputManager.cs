using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    List<InputDevice> leftHandDevices = new List<InputDevice>();
    List<InputDevice> rightHandDevices = new List<InputDevice>();

    public bool leftTrigger;
    public bool leftGrip;
    public bool rightTrigger;
    public bool rightGrip;
    public bool aButton;
    public bool bButton;
    public bool xButton;
    public bool yButton;

    public byte leftHandZone;
    public byte rightHandZone;

    public Vector3 lookDirection;
    public Vector3 rightHandDirection;
    public Vector3 leftHandDirection;


    private Vector3 prevRightHandPos = Vector3.zero;
    private Vector3 prevLeftHandPos = Vector3.zero;
    public Vector3 rightHandMovementDirection;
    public Vector3 leftHandMovementDirection;

    private readonly float zoneWidth = 0.3f;

    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public Transform colliderTransform;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left | InputDeviceCharacteristics.HeldInHand, leftHandDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.HeldInHand, rightHandDevices);

        InputDevices.deviceConnected += OnDeviceConnected;
        InputDevices.deviceDisconnected += OnDeviceDisconnected;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        GetControllerZones();
        FixCollider();
    }

    private void GetInput()
    {
        if (leftHandDevices.Count >= 1)
        {
            foreach (InputDevice leftController in leftHandDevices)
            {
                if (leftController.isValid)
                {
                    bool inputValue;
                    if (leftController.TryGetFeatureValue(CommonUsages.triggerButton, out inputValue))
                    {
                        leftTrigger = inputValue;
                    }
                    if (leftController.TryGetFeatureValue(CommonUsages.gripButton, out inputValue))
                    {
                        leftGrip = inputValue;
                    }
                    if (leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out inputValue))
                    {
                        yButton = inputValue;
                    }
                    if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue))
                    {
                        xButton = inputValue;
                    }
                }
            }
        }

        if (rightHandDevices.Count >= 1)
        {
            foreach (InputDevice rightController in rightHandDevices)
            {
                if (rightController.isValid)
                {
                    bool inputValue;
                    if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out inputValue))
                    {
                        rightTrigger = inputValue;
                    }
                    if (rightController.TryGetFeatureValue(CommonUsages.gripButton, out inputValue))
                    {
                        rightGrip = inputValue;
                    }
                    if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out inputValue))
                    {
                        bButton = inputValue;
                    }
                    if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue))
                    {
                        aButton = inputValue;
                    }
                }
            }
        }

        leftHandMovementDirection = (leftHandTransform.position - prevLeftHandPos).normalized;
        prevLeftHandPos = leftHandTransform.position;

        rightHandMovementDirection = (rightHandTransform.position - prevRightHandPos).normalized;
        prevRightHandPos = rightHandTransform.position;
    }

    private void GetControllerZones()
    {
        Vector3 leftRelativePos = headTransform.InverseTransformPoint(leftHandTransform.position);
        if (leftRelativePos.x < -zoneWidth / 2)
        {
            leftHandZone = 1;
        }
        else if (leftRelativePos.x <= zoneWidth / 2)
        {
            leftHandZone = 2;
        }
        else
        {
            leftHandZone = 3;
        }
        if (leftHandTransform.position.y - headTransform.position.y > 0)
        {
            leftHandZone += 3;
        }


        Vector3 rightRelativePos = headTransform.InverseTransformPoint(rightHandTransform.position);
        if (rightRelativePos.x < -zoneWidth / 2)
        {
            rightHandZone = 1;
        }
        else if (rightRelativePos.x <= zoneWidth / 2)
        {
            rightHandZone = 2;
        }
        else
        {
            rightHandZone = 3;
        }
        if (rightHandTransform.position.y - headTransform.position.y > 0)
        {
            rightHandZone += 3;
        }

        lookDirection = headTransform.forward;
        leftHandDirection = leftHandTransform.forward;
        rightHandDirection = rightHandTransform.forward;
    }

    private void FixCollider()
    {
        colliderTransform.transform.position = new Vector3(headTransform.position.x, transform.position.y, headTransform.position.z);
    }

    private void OnDeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.HeldInHand))
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                leftHandDevices.Add(device);
            }
            else if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                rightHandDevices.Add(device);
            }
        }
    }

    private void OnDeviceDisconnected(InputDevice device)
    {
        if (leftHandDevices.Contains(device))
        {
            leftHandDevices.Remove(device);
        }
        else if (rightHandDevices.Contains(device))
        {
            rightHandDevices.Remove(device);
        }
    }
}

