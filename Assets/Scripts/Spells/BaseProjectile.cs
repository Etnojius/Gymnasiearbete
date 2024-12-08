using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BaseProjectile : BaseSpell
{
    public GameObject target;
    public Vector3 direction;
    public float speed = 5;
    public float anglesPerSecond = 15;
    public float damage = 5;
    public Collider privateCollider;
    public bool onlyTargetAir = false;
    public float homingDelay = 0;
    public float homingPreDelay = 0.2f;
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
                if (potentialTarget.GetComponent<NetworkObject>().NetworkObjectId != ownerId.Value)
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
            if (target != null && !(onlyTargetAir && target.transform.position.y < 2.5f))
            {
                if (homingDelay <= 0)
                {
                    transform.LookAt(transform.position + Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, anglesPerSecond * Mathf.Deg2Rad * Time.deltaTime, 0));
                }
                else
                {
                    transform.LookAt(transform.position + Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, anglesPerSecond * Mathf.Deg2Rad * Time.deltaTime * homingPreDelay, 0));
                }
            }
            homingDelay -= Time.deltaTime;
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
