using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, IHitByProjectile
{
    public LevelObject info;

    public virtual void Hit(int damage = 0)
    {
        GetComponent<BeSplashed>().Rid();
    }
}
