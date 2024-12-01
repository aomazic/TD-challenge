using System.Collections;
using Unity.VisualScripting;
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
    [SerializeField]
    private float energyDepletionRate;
    [SerializeField]
    private float energyRegenRate;
    [SerializeField]
    private float energyCapacity;
    [SerializeField]
    private int accuracy;

    [Header("Laser Tower References")]
    [SerializeField]
    private GameObject laserPrefab;
    
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
    public int Accuracy
    {
        get => accuracy;
        set => accuracy = value;
    }

    public float currentEnergy;
    private LineRenderer laser;
    private Coroutine firingCoroutine;

    private void Start()
    {
        laser = laserPrefab.GetComponent<LineRenderer>();
        currentEnergy = energyCapacity;
    }

    private void Update()
    {
        if (Target == null)
        {
            FindTarget();
        }

        HandleFiring();
        RegenerateEnergy();
    }

    private void HandleFiring()
    {
        if (Target != null && currentEnergy > 0)
        {
            currentEnergy -= Time.deltaTime * energyDepletionRate;
            firingCoroutine ??= StartCoroutine(FireContinuously());
        }
        else
        {
            if (firingCoroutine == null)
            {
                return;
            }
            StopCoroutine(firingCoroutine);
            laser.enabled = false;
        }
    }

    private IEnumerator FireContinuously()
    {
        laser.enabled = true;
        while (currentEnergy > 0)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
        laser.enabled = false;
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < energyCapacity)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
        }
    }

    public void FindTarget()
    {
        var enemy = EnemyManager.Instance.GetClosestEnemyInRange(transform.position, maximumRange);
        SetTarget(enemy);
    }

    public void SetTarget(IEnemy target)
    {
        Target = target;

        // Subscribe to the new target's OnDeath event
        if (Target != null)
        {
            Target.OnDeath += HandleTargetDeath;
        }
    }

    private void HandleTargetDeath(IEnemy enemy)
    {
        if (Target == enemy)
        {
            // Unsubscribe from the dead target's OnDeath event
            Target.OnDeath -= HandleTargetDeath;
            Target = null;
        }
    }

    public void Fire()
    {
        LaserVisuals.SetLaserPositions(laser, transform.position, Target.Transform.position);
        Target?.TakeDamage(Damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maximumRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumRange);
    }
}