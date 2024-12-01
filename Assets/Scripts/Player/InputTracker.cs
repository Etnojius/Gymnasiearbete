using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTracker : MonoBehaviour
{
    [SerializeField]
    private float movementForce = 100f;
    [SerializeField]
    private InputManager input;
    [SerializeField]
    private Rigidbody playerRB;

    public bool prevAButton = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (input.aButton != prevAButton)
        {
            prevAButton = input.aButton;
            AButtonChange(prevAButton);
        }
    }

    private void AButtonChange(bool down)
    {
        if (down)
        {
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(input.rightHandDirection * movementForce, ForceMode.Impulse);
        }
    }
}
