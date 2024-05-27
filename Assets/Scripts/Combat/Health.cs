using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    public event Action OnTakeDamage;
    public event Action OnDie;
    public bool IsDead => currentHealth == 0f;

    int currentHealth;
    bool isInvulnerable;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        if (currentHealth <= 0 || isInvulnerable) { return; }

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        OnTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
        }

        Debug.Log(currentHealth);
    }
}
