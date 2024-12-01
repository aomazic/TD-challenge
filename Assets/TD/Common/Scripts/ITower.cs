using UnityEngine;

public interface ITower
{
    int Damage { get; set; }
    int MaximumRange { get; set; }
    int MinimumRange { get; set; }
    int Accuracy { get; set; }
    IEnemy Target { get; set; }
    
    void FindTarget();
    void SetTarget(IEnemy target);
    void Fire();
}
