using UnityEngine;

public interface IEnemy
{
    Transform Transform { get; }
    Collider2D Collider { get; }
    float Health { get; set; }
    void TakeDamage(float amount);
    void Die();
    event System.Action<IEnemy> OnDeath;
}
