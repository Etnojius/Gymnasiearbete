using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTest : MonoBehaviour
{
    public Renderer privateRenderer;
    [SerializeField]
    public bool isRightHand;
    [SerializeField]
    private InputManager input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        byte zone;
        if (isRightHand) 
        {
            zone = input.rightHandZone; 
        }
        else
        {
            zone = input.leftHandZone;
        }

        switch (zone)
        {
            case 1:
                privateRenderer.material.color = Color.black;
                break;
            case 2:
                privateRenderer.material.color = Color.blue;
                break;
            case 3:
                privateRenderer.material.color = Color.cyan;
                break;
            case 4:
                privateRenderer.material.color = Color.gray;
                break;
            case 5:
                privateRenderer.material.color = Color.green;
                break;
            case 6:
                privateRenderer.material.color = Color.magenta;
                break;
        }

        if (isRightHand)
        {
            if (input.rightGrip)
            {
                privateRenderer.material.color = Color.red;
            }
            if (input.rightTrigger)
            {
                privateRenderer.material.color = Color.white;
            }
            if (input.aButton)
            {
                privateRenderer.material.color = Color.yellow;
            }
            if (input.bButton)
            {
                privateRenderer.material.color = Color.clear;
            }
        }
        else
        {
            if (input.leftGrip)
            {
                privateRenderer.material.color = Color.red;
            }
            if (input.leftTrigger)
            {
                privateRenderer.material.color = Color.white;
            }
            if (input.xButton)
            {
                privateRenderer.material.color = Color.yellow;
            }
            if (input.yButton)
            {
                privateRenderer.material.color = Color.clear;
            }
        }
    }
}
