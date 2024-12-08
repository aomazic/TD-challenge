using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    int Damage { get; set; }
    int MaximumRange { get; set; }
    int MinimumRange { get; set; }
    int Accuracy { get; set; }
    IEnemy Target { get; set; }
    HashSet<IEnemy> enemiesInMaxRange { get; set; }
    void NewEnemyDetected(IEnemy enemy);
    void EnemyOutOfRange(IEnemy enemy);
    void FindNewTarget();
    void SetTarget(IEnemy target);
    void Fire();
}
