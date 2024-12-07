using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRRigReferences : MonoBehaviour
{
    public static VRRigReferences Instance;

    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Image hpMeter;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
}
