using UnityEngine;

public class Slug : MonoBehaviour, IEnemy
{
    
    [Header("Slug Properties")]
    [SerializeField]
    private int health;

    public int Health
    {
        get => health;
        set => health = value;
    }
    public Transform Transform => transform;
    
    private void OnEnable() {
        EnemyManager.Instance.RegisterEnemy(this);
    }

    private void OnDisable() {
        EnemyManager.Instance.UnregisterEnemy(this);
    }
    
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }

}
