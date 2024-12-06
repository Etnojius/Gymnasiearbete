using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BaseProjectile : NetworkBehaviour
{
    public GameObject target;
    public Vector3 direction;
    public float speed = 5;
    public float anglesPerSecond = 15;
    public float damage = 5;
    public ulong ownerId;
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            transform.LookAt(transform.position + direction);
            var targetList = GameObject.FindGameObjectsWithTag("Player");
            float closest = Mathf.Infinity;
            foreach (var potentialTarget in targetList)
            {
                if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId != ownerId)
                {
                    if ((transform.position - potentialTarget.transform.position).sqrMagnitude > closest)
                    {
                        closest = (transform.position - potentialTarget.transform.position).sqrMagnitude;
                        target = potentialTarget;
                    }
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            transform.LookAt(Vector3.RotateTowards(transform.forward, target.transform.position, anglesPerSecond * Time.deltaTime, 0));
        }
    }
}
