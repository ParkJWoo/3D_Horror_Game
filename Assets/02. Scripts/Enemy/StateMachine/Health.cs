using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private int maxHealth = 100;
    public int CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    //  이벤트
    public event Action OnDie;
    public event Action<int> OnDamage;
    public event Action<int> OnHeal;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        IsDead = false;
    }

    public void TakeDamage(int damage)
    {
        if(IsDead)
        {
            return;
        }

        int prevHealth = CurrentHealth;
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        OnDamage?.Invoke(damage);

        if (CurrentHealth == 0 && !IsDead)
        {
            IsDead = true;
            OnDie?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if(IsDead)
        {
            return;
        }

        int prevHealth = CurrentHealth;
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);

        OnHeal?.Invoke(amount);
    }

    public void Revive()
    {
        IsDead = false;
        CurrentHealth = maxHealth;
    }
}
