using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] Collider myCollider;

    List<Collider> alreadyCollidedWith = new List<Collider>();
    int damage;
    float knockback;

    void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
        }

        if (other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
