using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    List<InputDevice> leftHandDevices = new List<InputDevice>();
    List<InputDevice> rightHandDevices = new List<InputDevice>();

    public static bool leftTrigger;
    public static bool leftGrip;
    public static bool rightTrigger;
    public static bool rightGrip;
    public static bool aButton;
    public static bool bButton;
    public static bool xButton;
    public static bool yButton;

    public static byte leftHandZone;
    public static byte rightHandZone;

    public Vector3 lookDirection;
    public Vector3 rightHandDirection;
    public Vector3 leftHandDirection;

    private readonly float zoneWidth = 0.3f;

    public Transform headTransform;
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    // Start is called before the first frame update
    void Start()
    {
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
                        xButton = inputValue;
                    }
                    if (leftController.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue))
                    {
                        yButton = inputValue;
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
                        aButton = inputValue;
                    }
                    if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out inputValue))
                    {
                        bButton = inputValue;
                    }
                }
            }
        }
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
