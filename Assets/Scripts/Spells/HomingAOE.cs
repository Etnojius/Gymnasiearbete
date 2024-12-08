using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HomingAOE : AOEDamage
{
    public GameObject target;
    public Vector3 direction;
    public float speed = 5f;
    public float anglesPerSecond = 360f;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(transform.position + direction);
        var targetList = GameObject.FindGameObjectsWithTag("Player");
        float closest = Mathf.Infinity;
        foreach (var potentialTarget in targetList)
        {
            if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId != ownerId.Value)
            {
                if ((transform.position - potentialTarget.transform.position).sqrMagnitude < closest)
                {
                    closest = (transform.position - potentialTarget.transform.position).sqrMagnitude;
                    target = potentialTarget;
                }
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        DealDamage();
        if (IsServer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (target != null)
            {
                transform.LookAt(transform.position + Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, anglesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 0));
            }
        }
    }
}
