using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BaseProjectile : NetworkBehaviour
{
    public Vector3 direction;
    public float speed;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
