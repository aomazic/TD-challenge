using UnityEngine;

public interface IEnemy
{
    Transform Transform { get; }
    int Health { get; set; }
    void TakeDamage(int amount);
    void Die();
}
