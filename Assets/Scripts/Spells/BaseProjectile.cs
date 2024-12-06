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
    public Collider privateCollider;
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            privateCollider = GetComponent<Collider>();
            privateCollider.enabled = false;
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

            StartCoroutine(StartCollider());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (target != null)
            {
                transform.LookAt(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, anglesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 10));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<NetworkPlayer>().hp -= damage;
            NetworkObject.Despawn(true);
        }
    }

    IEnumerator StartCollider()
    {
        yield return new WaitForSeconds(0.2f);
        privateCollider.enabled = true;
    }
}
