using UnityEngine;

public interface ITower
{
    IEnemy Target { get; set; }
    int Damage { get; set; }
    int MaximumRange { get; set; }
    int MinimumRange { get; set; }
    int FireRate { get; set; }
    int Accuracy { get; set; }
    
    
    void FindTarget();
    void SetTarget(IEnemy target);
    void Fire();
}
