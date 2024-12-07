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
    public bool prevXButton = false;

    public bool waitUntilRelease = false;

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
        if (input.aButton != prevAButton)
        {
            prevAButton = input.aButton;
            AButtonChange(prevAButton);
        }

        if (input.xButton != prevXButton)
        {
            prevXButton = input.xButton;
            XButtonChange(prevXButton);
        }

        if (spellCaster != null)
        {
            //if (inputDelayTimer <= 0)
            //{
            //    if (prevState.leftTrigger == input.leftTrigger && prevState.leftGrip == input.leftGrip && prevState.rightTrigger == input.rightTrigger && prevState.rightGrip == input.rightGrip && prevState.aButton == input.aButton && prevState.bButton == input.bButton && prevState.xButton == input.xButton && prevState.yButton == input.yButton && prevState.rightZone == input.rightHandZone && prevState.leftZone == input.leftHandZone)
            //    {
            //        prevState = CreateInputState(prevState, Time.deltaTime);
            //        spellCaster.CastSpell(prevState);
            //    }
            //    else
            //    {
            //        if (inputDelayActive)
            //        {
            //            prevState = CreateInputState(prevState, newInputDelay);
            //            spellCaster.CastSpell(prevState);
            //            inputDelayActive = false;
            //        }
            //        else
            //        {
            //            inputDelayTimer = newInputDelay;
            //            inputDelayActive = true;
            //        }
            //    }
            //}
            //else
            //{
            //    inputDelayTimer -= Time.deltaTime;
            //}

            if (waitUntilRelease)
            {
                if (!(input.leftGrip || input.leftTrigger || input.rightGrip || input.rightTrigger))
                {
                    waitUntilRelease = false;
                }
            }
            else
            {
                prevState = CreateInputState(prevState, Time.deltaTime);
                spellCaster.CastSpell(prevState);
            }
        }

        if (transform.position.y < -20)
        {
            transform.position = new Vector3(0, 40, -45);
        }
    }

    public void ResetInputState()
    {
        prevState = CreateInputState(new InputState(), 0, true);
        waitUntilRelease = true;
    }

    private void AButtonChange(bool down)
    {
        if (down && canJump)
        {
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(input.rightHandDirection * movementForce, ForceMode.Impulse);
        }
    }

    private void XButtonChange(bool down)
    {
        if (down && canJump)
        {
            playerRB.velocity = Vector3.zero;
            playerRB.AddForce(input.leftHandDirection * movementForce, ForceMode.Impulse);
        }
    }

    public InputState CreateInputState(InputState prev, float timeSincePrev, bool reset = false)
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

        if (reset)
        {
            state.prevLeftTrigger = input.leftTrigger;
            state.prevLeftGrip = input.leftGrip;
            state.prevRightTrigger = input.rightTrigger;
            state.prevRightGrip = input.rightGrip;
            state.prevAButton = input.aButton;
            state.prevBButton = input.bButton;
            state.prevXButton = input.xButton;
            state.prevYButton = input.yButton;
            state.prevLeftZone = input.leftHandZone;
            state.prevRightZone = input.rightHandZone;
        }
        else
        {
            if (state.leftTrigger != prev.leftTrigger)
            {
                state.prevLeftTrigger = prev.leftTrigger;
            }
            else
            {
                state.prevLeftTrigger = prev.prevLeftTrigger;
            }
            if (state.leftGrip != prev.leftGrip)
            {
                state.prevLeftGrip = prev.leftGrip;
            }
            else
            {
                state.prevLeftGrip = prev.prevLeftGrip;
            }
            if (state.leftZone != prev.leftZone)
            {
                state.prevLeftZone = prev.leftZone;
            }
            else
            {
                state.prevLeftZone = prev.prevLeftZone;
            }
            if (state.rightTrigger != prev.rightTrigger)
            {
                state.prevRightTrigger = prev.rightTrigger;
            }
            else
            {
                state.prevRightTrigger = prev.prevRightTrigger;
            }
            if (state.rightGrip != prev.rightGrip)
            {
                state.prevRightGrip = prev.rightGrip;
            }
            else
            {
                state.prevRightGrip = prev.prevRightGrip;
            }
            if (state.rightZone != prev.rightZone)
            {
                state.prevRightZone = prev.rightZone;
            }
            else
            {
                state.prevRightZone = prev.prevRightZone;
            }
            if (state.aButton != prev.aButton)
            {
                state.prevAButton = prev.aButton;
            }
            else
            {
                state.prevAButton = prev.prevAButton;
            }
            if (state.bButton != prev.bButton)
            {
                state.prevBButton = prev.bButton;
            }
            else
            {
                state.prevBButton = prev.prevBButton;
            }
            if (state.xButton != prev.xButton)
            {
                state.prevXButton = prev.xButton;
            }
            else
            {
                state.prevXButton = prev.prevXButton;
            }
            if (state.yButton != prev.yButton)
            {
                state.prevYButton = prev.yButton;
            }
            else
            {
                state.prevYButton = prev.prevYButton;
            }
        }

        if (state.leftTrigger == prev.leftTrigger)
        {
            state.leftTriggerDuration = timeSincePrev + prev.leftTriggerDuration;
        }
        else
        {
            state.leftTriggerDuration = 0;
        }

        if (state.rightTrigger == prev.rightTrigger)
        {
            state.rightTriggerDuration = timeSincePrev + prev.rightTriggerDuration;
        }
        else
        {
            state.rightTriggerDuration = 0;
        }

        if (state.leftGrip == prev.leftGrip)
        {
            state.leftGripDuration = timeSincePrev + prev.leftGripDuration;
        }
        else
        {
            state.leftGripDuration = 0;
        }

        if (state.rightGrip == prev.rightGrip)
        {
            state.rightGripDuration = timeSincePrev + prev.rightGripDuration;
        }
        else
        {
            state.rightGripDuration = 0;
        }

        if (state.aButton == prev.aButton)
        {
            state.aButtonDuration = timeSincePrev + prev.aButtonDuration;
        }
        else
        {
            state.aButtonDuration = 0;
        }

        if (state.bButton == prev.bButton)
        {
            state.bButtonDuration = timeSincePrev + prev.bButtonDuration;
        }
        else
        {
            state.bButtonDuration = 0;
        }

        if (state.xButton == prev.xButton)
        {
            state.xButtonDuration = timeSincePrev + prev.xButtonDuration;
        }
        else
        {
            state.xButtonDuration = 0;
        }

        if (state.yButton == prev.yButton)
        {
            state.yButtonDuration = timeSincePrev + prev.yButtonDuration;
        }
        else
        {
            state.yButtonDuration = 0;
        }

        if (state.leftZone == prev.leftZone)
        {
            state.leftZoneDuration = timeSincePrev + prev.leftZoneDuration;
        }
        else
        {
            state.leftZoneDuration = 0;
        }

        if (state.rightZone == prev.rightZone)
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
