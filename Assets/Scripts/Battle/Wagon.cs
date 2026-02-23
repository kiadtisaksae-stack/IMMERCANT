using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Wagon : Identity
{

    protected override void Awake()
    {
        base.Awake(); // เรียกใช้การตั้งค่า RB ของแม่
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }

    public override void Die()
    {
        BattleManager battleManager = FindAnyObjectByType<BattleManager>();
        if (battleManager != null)
        {
            battleManager.LostBattle();
        }
        else
        {
            Debug.LogWarning("BattleManager not found in the scene.");
        }
    }
}