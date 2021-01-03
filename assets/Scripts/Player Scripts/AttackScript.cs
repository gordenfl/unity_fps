using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage = 5.0f;
    public float radius = 1.0f;
    public LayerMask layerMask;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);
        if (hits.Length > 0)
        {
            print("we hit:" + hits[0].gameObject.tag);
            hits[0].gameObject.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
