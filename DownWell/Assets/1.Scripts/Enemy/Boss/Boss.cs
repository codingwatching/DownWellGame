using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    GameObject bossObject;

    public enum BossState { normal, rage }
    [SerializeField] BossState currentState = BossState.normal;

    public int maxHealth = 100;
    public Health health;

    private bool died = false;

    private void Start()
    {
        health = new Health(maxHealth);
    }

    #region Object Handle
    public void GetBoss()
    {
        bossObject = GameObject.FindGameObjectWithTag("Boss");
        SetBossActive(false);
    }

    public void SetBossActive(bool value)
    {
        bossObject.SetActive(value);
    }
    #endregion

    public void SetState(BossState state)
    {
        currentState = state;
    }

    public bool UnderHealthRatio(int healthRatio)
    {
        if (health.CurrentRatio() < healthRatio)
        {
            //SetState(BossState.rage);
            return true;
        }

        return false;
    }

    public void Damaged(int amount)
    {
        if (died) return;

        health.Lose(amount);

        if (health.Current <= 0)
            Die();

        StartCoroutine(DamagedFX());
    }

    private void Die()
    {
        Debug.Log("Boss Die");
        died = true;
        Destroy(this.gameObject);
        BossStageManager.instance.EndBossStage();
    }

    private IEnumerator DamagedFX()
    {
        GetComponent<SpriteRenderer>().color = Color.black;

        yield return null;

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
