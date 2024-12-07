using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTracker : MonoBehaviour
{
    public static InputTracker Instance;

    private bool canJump = true;
    [SerializeField]
    private float movementForce = 100f;
    [SerializeField]
    private InputManager input;
    [SerializeField]
    private Rigidbody playerRB;

    public bool prevAButton = false;

    [SerializeField]
    private float newInputDelay = 0.2f;
    private float inputDelayTimer = 0f;
    private bool inputDelayActive = false;

    public SpellCaster spellCaster;

    private InputState prevState;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (spellCaster != null)
        {
            if (input.aButton != prevAButton)
            {
                prevAButton = input.aButton;
                AButtonChange(prevAButton);
            }

            if (inputDelayTimer <= 0)
            {
                if (prevState.leftTrigger == input.leftTrigger && prevState.leftGrip == input.leftGrip && prevState.rightTrigger == input.rightTrigger && prevState.rightGrip == input.rightGrip && prevState.aButton == input.aButton && prevState.bButton == input.bButton && prevState.xButton == input.xButton && prevState.yButton == input.yButton && prevState.rightZone == input.rightHandZone && prevState.leftZone == input.leftHandZone)
                {
                    prevState = CreateInputState(prevState, Time.deltaTime);
                    spellCaster.CastSpell(prevState);
                }
                else
                {
                    if (inputDelayActive)
                    {
                        prevState = CreateInputState(prevState, newInputDelay);
                        spellCaster.CastSpell(prevState);
                        inputDelayActive = false;
                    }
                    else
                    {
                        inputDelayTimer = newInputDelay;
                        inputDelayActive = true;
                    }
                }
            }
            else
            {
                inputDelayTimer -= Time.deltaTime;
            }
            //prevState = CreateInputState(prevState, Time.deltaTime);
            //spellCaster.CastSpell(prevState);
        }

        if (transform.position.y < -20)
        {
            transform.position = new Vector3(0, 40, -45);
        }
    }

    private void AButtonChange(bool down)
    {
        if (down && canJump)
        {
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(input.rightHandDirection * movementForce, ForceMode.Impulse);
        }
    }

    public InputState CreateInputState(InputState prev, float timeSincePrev)
    {
        InputState state = new InputState();
        state.leftTrigger = input.leftTrigger;
        state.leftGrip = input.leftGrip;
        state.rightTrigger = input.rightTrigger;
        state.rightGrip = input.rightGrip;
        state.aButton = input.aButton;
        state.bButton = input.bButton;
        state.xButton = input.xButton;
        state.yButton = input.yButton;
        state.leftZone = input.leftHandZone;
        state.rightZone = input.rightHandZone;

        state.prevLeftTrigger = prev.leftTrigger;
        state.prevLeftGrip = prev.leftGrip;
        state.prevRightTrigger = prev.rightTrigger;
        state.prevRightGrip = prev.rightGrip;
        state.prevAButton = prev.aButton;
        state.prevBButton = prev.bButton;
        state.prevXButton = prev.xButton;
        state.prevYButton = prev.yButton;
        state.prevLeftZone = prev.leftZone;
        state.prevRightZone = prev.rightZone;

        if (state.leftTrigger == state.prevLeftTrigger)
        {
            state.leftTriggerDuration = timeSincePrev + prev.leftTriggerDuration;
        }
        else
        {
            state.leftTriggerDuration = 0;
        }

        if (state.rightTrigger == state.prevRightTrigger)
        {
            state.rightTriggerDuration = timeSincePrev + prev.rightTriggerDuration;
        }
        else
        {
            state.rightTriggerDuration = 0;
        }

        if (state.leftGrip == state.prevLeftGrip)
        {
            state.leftGripDuration = timeSincePrev + prev.leftGripDuration;
        }
        else
        {
            state.leftGripDuration = 0;
        }

        if (state.rightGrip == state.prevRightGrip)
        {
            state.rightGripDuration = timeSincePrev + prev.rightGripDuration;
        }
        else
        {
            state.rightGripDuration = 0;
        }

        if (state.aButton == state.prevAButton)
        {
            state.aButtonDuration = timeSincePrev + prev.aButtonDuration;
        }
        else
        {
            state.aButtonDuration = 0;
        }

        if (state.bButton == state.prevBButton)
        {
            state.bButtonDuration = timeSincePrev + prev.bButtonDuration;
        }
        else
        {
            state.bButtonDuration = 0;
        }

        if (state.xButton == state.prevXButton)
        {
            state.xButtonDuration = timeSincePrev + prev.xButtonDuration;
        }
        else
        {
            state.xButtonDuration = 0;
        }

        if (state.yButton == state.prevYButton)
        {
            state.yButtonDuration = timeSincePrev + prev.yButtonDuration;
        }
        else
        {
            state.yButtonDuration = 0;
        }

        if (state.leftZone == state.prevLeftZone)
        {
            state.leftZoneDuration = timeSincePrev + prev.leftZoneDuration;
        }
        else
        {
            state.leftZoneDuration = 0;
        }

        if (state.rightZone == state.prevRightZone)
        {
            state.rightZoneDuration = timeSincePrev + prev.rightZoneDuration;
        }
        else
        {
            state.rightZoneDuration = 0;
        }

        return state;
    }
}

public struct InputState
{
    public bool leftTrigger;
    public bool prevLeftTrigger;
    public float leftTriggerDuration;

    public bool leftGrip;
    public bool prevLeftGrip;
    public float leftGripDuration;

    public bool rightTrigger;
    public bool prevRightTrigger;
    public float rightTriggerDuration;

    public bool rightGrip;
    public bool prevRightGrip;
    public float rightGripDuration;

    public bool aButton;
    public bool prevAButton;
    public float aButtonDuration;

    public bool bButton;
    public bool prevBButton;
    public float bButtonDuration;

    public bool xButton;
    public bool prevXButton;
    public float xButtonDuration;

    public bool yButton;
    public bool prevYButton;
    public float yButtonDuration;

    public byte leftZone;
    public byte prevLeftZone;
    public float leftZoneDuration;

    public byte rightZone;
    public byte prevRightZone;
    public float rightZoneDuration;
}
