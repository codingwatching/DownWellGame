using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public bool active = false;

    public GameObject projectile;
    public GameObject mucousMembrane;
    public float shotTime = 3f;
    float curTime = 0f;

    Transform target;

    System.Action ShootPattern;

    [SerializeField] string[] normalPatterns = { "ShootNormal" };
    [SerializeField] string[] ragePatterns = { "ShootNormalRage", "ShootMucousMembrane" };
    string[] currentPatterns;

    [Range(0,100)] public int healthRatioRageMode = 60;
    [Range(0, 30)] public float sideProjectileAngle = 15f;

    void Start()
    {
        ChangeShootPatterns(Boss.BossState.rage);
        SetPattern(ragePatterns[0]);
    }

    private void Update()
    {
        if(active)
            Shoot();

        if(GetComponent<Boss>().BecomeRageMode(healthRatioRageMode))
        {
            ChangeShootPatterns(Boss.BossState.rage);
        }
    }

    void Shoot()
    {
        curTime += Time.deltaTime;

        if (curTime > shotTime)
        {
            SetRandomShootPattern();
            ShootPattern.Invoke();

            curTime = 0;
        }
    }

    public void ChangeShootPatterns(Boss.BossState state)
    {
        switch(state)
        {
            case Boss.BossState.normal:
                currentPatterns = normalPatterns;
                break;
            case Boss.BossState.rage:
                currentPatterns = ragePatterns;
                break;
        }
    }

    public void SetPattern(string methodName)
    {
        ShootPattern = () => { Invoke(methodName, 0); };
    }

    void SetRandomShootPattern()
    {
        string seed = (Time.time + Random.value).ToString();
        System.Random rand = new System.Random(seed.GetHashCode());

        SetPattern(currentPatterns[rand.Next(0, currentPatterns.Length)]);
    }

    void ShootNormal()
    {
        var shotProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        shotProjectile.GetComponent<BossProjectile>().SetTarget(target);
    }

    void ShootNormalRage()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        var dir = (target.position - transform.position).normalized;

        var shotProjectile = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile.GetComponent<BossProjectile>().MoveToTarget();

        var shotProjectile1 = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile1.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile1.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile1.GetComponent<BossProjectile>().RotateDirection(sideProjectileAngle);
        shotProjectile1.GetComponent<BossProjectile>().MoveToTarget();

        var shotProjectile2 = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile2.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile2.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile2.GetComponent<BossProjectile>().RotateDirection(-sideProjectileAngle);
        shotProjectile2.GetComponent<BossProjectile>().MoveToTarget();
    }

    void ShootNormalRageByTransform()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        var dir = (target.position - transform.position).normalized;

        var shotProjectile = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile.GetComponent<BossProjectile>().MoveToTargetByTransform();

        var shotProjectile1 = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile1.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile1.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile1.GetComponent<BossProjectile>().RotateDirection(sideProjectileAngle);
        shotProjectile1.GetComponent<BossProjectile>().MoveToTargetByTransform();

        var shotProjectile2 = Instantiate(projectile, transform.position, Quaternion.identity, transform);
        //shotProjectile2.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile2.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile2.GetComponent<BossProjectile>().RotateDirection(-sideProjectileAngle);
        shotProjectile2.GetComponent<BossProjectile>().MoveToTargetByTransform();
    }

    void ShootMucousMembrane()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        var dir = (target.position - transform.position).normalized;

        var shotProjectile = Instantiate(mucousMembrane, transform.position, Quaternion.identity);
        //shotProjectile.GetComponent<BossProjectile>().SetTarget(target);
        shotProjectile.GetComponent<BossProjectile>().SetDirection(dir);
        shotProjectile.GetComponent<BossProjectile>().MoveToTarget();
    }

    void BoxingAttack()
    {
        var hitBox = transform.GetChild(0);

        hitBox.gameObject.SetActive(true);

        StartCoroutine(EndBoxingAttack(hitBox));
    }

    IEnumerator EndBoxingAttack(Transform hitbox)
    {
        var pFilter = new ContactFilter2D();
        pFilter.layerMask = LayerMask.NameToLayer("Player");
        pFilter.useLayerMask = false;
        Debug.Log(LayerMask.LayerToName(pFilter.layerMask));
        var colliders = new List<Collider2D>();

        hitbox.GetComponent<BoxCollider2D>().OverlapCollider(pFilter, colliders);

        if(colliders.Count > 0)
        {
            foreach(var collider in colliders)
            {
                if(collider.tag == "Player")
                    collider.GetComponent<PlayerCombat>().Damaged(hitbox);
            }
        }

        yield return new WaitForSeconds(3f);

        hitbox.gameObject.SetActive(false);
    }
}
