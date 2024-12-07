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
    public float lifeTime = 15f;
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
                    if ((transform.position - potentialTarget.transform.position).sqrMagnitude < closest)
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
                transform.LookAt(transform.position + Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, anglesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 0));
            }
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                NetworkObject.Despawn(true);
            }
        }
    }

    public void ReFire(ulong ownerId)
    {
        var targetList = GameObject.FindGameObjectsWithTag("Player");
        float closest = Mathf.Infinity;
        foreach (var potentialTarget in targetList)
        {
            if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId != ownerId)
            {
                if ((transform.position - potentialTarget.transform.position).sqrMagnitude < closest)
                {
                    closest = (transform.position - potentialTarget.transform.position).sqrMagnitude;
                    target = potentialTarget;
                }
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (IsServer)
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<NetworkPlayer>().TakeDamage(damage);
                NetworkObject.Despawn(true);
            }
            else if (other.gameObject.tag == "Shield")
            {
                var shieldScript = other.GetComponent<Shield>();
                if (shieldScript.isParry)
                {
                    transform.Rotate(180, 0, 0);
                    ReFire(shieldScript.ownerId.Value);
                    privateCollider.enabled = false;
                    StartCoroutine(StartCollider());
                }
            }
            else
            {
                NetworkObject.Despawn(true);
            }
        }
    }

    IEnumerator StartCollider()
    {
        yield return new WaitForSeconds(0.2f);
        privateCollider.enabled = true;
    }
}
