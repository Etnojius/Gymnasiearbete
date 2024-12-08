using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellRequirements
{
    public static readonly Requirements redCircle = new Requirements(true, true, true, true, 4, 3, 1, 6);
    public static readonly Requirements yellowCircle = new Requirements(true, true, true, true, 4, 6, 5, 5);
    public static readonly Requirements purpleCircle = new Requirements(true, true, true, true, 2, 2, 3, 1);
    public static readonly Requirements greenCircle = new Requirements(true, true, true, true, 4, 6, 1, 3);
    public static readonly Requirements blueCircle = new Requirements(true, true, true, true, 5, 5, 4, 2);

    public static readonly Requirements shield = new Requirements(false, true, false, true, 2, 2, 1, 3);
    public static readonly Requirements flamethrower = new Requirements(false, false, true, false, 0, 6, 0, 3, SpellCaster.yellowCircle, SpellCaster.redCircle);
    public static readonly Requirements battlefield = new Requirements(false, true, false, true, 1, 3, 4, 6, SpellCaster.purpleCircle, SpellCaster.purpleCircle, SpellCaster.purpleCircle);
}

public struct Requirements
{
    public bool leftTrigger;
    public bool rightTrigger;
    public bool leftGrip;
    public bool rightGrip;
    public byte rightZone;
    public byte leftZone;
    public byte prevRightZone;
    public byte prevLeftZone;
    public byte innerCircle;
    public byte middleCircle;
    public byte outerCircle;

    public Requirements(bool leftTrigger, bool leftGrip, bool rightTrigger, bool rightGrip, byte prevLeftZone, byte prevRightZone, byte leftZone, byte rightZone, byte innerCircle = 0, byte middleCircle = 0, byte outerCircle = 0)
    {
        this.leftTrigger = leftTrigger;
        this.leftGrip = leftGrip;
        this.rightTrigger = rightTrigger;
        this.rightGrip = rightGrip;
        this.prevLeftZone = prevLeftZone;
        this.prevRightZone = prevRightZone;
        this.leftZone = leftZone;
        this.rightZone = rightZone;
        this.innerCircle = innerCircle;
        this.middleCircle = middleCircle;
        this.outerCircle = outerCircle;
    }
}
