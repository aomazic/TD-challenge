using UnityEngine;

public class LaserTower : MonoBehaviour, ITower
{
    [Header("Laser Tower Properties")]
    [SerializeField]
    private int damage;
    [SerializeField]
    private int maximumRange;
    [SerializeField]
    private int minimumRange;
    /**
     * Fire rate is the time between each shot in seconds
     */
    [SerializeField]
    private int fireRate;
    [SerializeField]
    private int accuracy;
    
    public IEnemy Target { get; set; }
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    public int MaximumRange
    {
        get => maximumRange;
        set => maximumRange = value;
    }
    public int MinimumRange
    {
        get => minimumRange;
        set => minimumRange = value;
    }
    
    public int FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }
    public int Accuracy
    {
        get => accuracy;
        set => accuracy = value;
    }
    

    private float _nextFireTime;

    private void Start()
    {
        _nextFireTime = Time.time;
    }

    private void Update()
    {
        FindTarget();
        
        if (Time.time < _nextFireTime || Target == null)
        {
            return;
        }

        Fire();
        _nextFireTime = Time.time + FireRate;
    }
    
    public void FindTarget()
    {
        var enemy = EnemyManager.Instance.GetClosestEnemyInRange(transform.position, maximumRange);
        
        SetTarget(enemy);
    }
    
    public void SetTarget(IEnemy target)
    {
        Target = target;
    }

    public void Fire()
    {
        Debug.Log($"Firing at {Target.Transform.name}");
        Target?.TakeDamage(Damage);
    }
    
    /**
     * Draw the range of the tower in the editor
     */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maximumRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumRange);
    }
}