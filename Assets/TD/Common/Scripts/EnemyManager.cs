using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    
    private readonly List<IEnemy> _enemies = new();
    
    private void Awake() {
        Instance = this;
    }
    public void RegisterEnemy(IEnemy enemy) {
        _enemies.Add(enemy);
    }

    public void UnregisterEnemy(IEnemy enemy) {
        _enemies.Remove(enemy);
    }
    
    public IEnemy GetClosestEnemyInRange(Vector3 position, float range) {
        IEnemy closestEnemy = null;
        var closestDistance = range;
        
        foreach (var enemy in _enemies) {
            var distance = Vector3.Distance(position, enemy.Transform.position);
            
            if (distance >= closestDistance) {
                continue;
            }

            closestDistance = distance;
            closestEnemy = enemy;
        }
        
        return closestEnemy;
    }
    
    public List<IEnemy> GetEnemiesInRange(Vector3 position, float range) {
        
        return _enemies
            .Where(enemy => Vector3.Distance(position, enemy.Transform.position) <= range)
            .ToList();
    }
}
