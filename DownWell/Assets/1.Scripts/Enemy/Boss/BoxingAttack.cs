using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingAttack : MonoBehaviour
{
    Collider2D collider;
    ContactFilter2D pFilter;

    public float time;

    // Start is called before the first frame update
    void OnEnable()
    {
        collider = GetComponentInParent<Collider2D>();
        pFilter = new ContactFilter2D();
        pFilter.layerMask = 1 << 3;

        Invoke("Destory", time);
    }

    private void Update()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        collider.OverlapCollider(pFilter, colliders);

        if(colliders.Count > 0)
        {
            foreach(var col in colliders)
            {
                if(col.gameObject.tag == "Player")
                {
                    col.GetComponent<PlayerCombat>().Damaged(transform);
                }
            }
        }
    }

    void Destory()
    {
        Destroy(this.gameObject);
    }
}
