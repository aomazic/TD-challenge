using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    [SerializeField]
    private CircleCollider2D maxRangeCollider;
    [SerializeField]
    private CircleCollider2D minRangeCollider;
    
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
    public HashSet<IEnemy> enemiesInMaxRange { get; set; } = new HashSet<IEnemy>();

    public float currentEnergy;
    
    private bool isInCooldown = false; 
    private LineRenderer laser;
    private Coroutine firingCoroutine;

    private void Start()
    {
        maxRangeCollider.radius = maximumRange;
        minRangeCollider.radius = minimumRange;
        laser = laserPrefab.GetComponent<LineRenderer>();
        currentEnergy = energyCapacity;
    }

    private void Update()
    {
        if (Target == null)
        {
            FindNewTarget();
        }
        HandleFiring();
        RegenerateEnergy();
    }

    private void HandleFiring()
    {
        if (isInCooldown)
        {
            return;
        }
        
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
            firingCoroutine = null;
            
            laser.enabled = false;
            
            if (currentEnergy <= 0)
            {
                isInCooldown = true;
            }
        }
    }

    private IEnumerator FireContinuously()
    {
        laser.enabled = true;
        while (currentEnergy > 0)
        {
            Fire();
            yield return null;
        }
        laser.enabled = false;
    }

    private void RegenerateEnergy()
    {
        if (currentEnergy < energyCapacity)
        {
            currentEnergy += energyRegenRate * Time.deltaTime;
        }
        else if (isInCooldown)
        {
            isInCooldown = false;
        }
    }

    public void NewEnemyDetected(IEnemy enemy)
    {
        enemiesInMaxRange.Add(enemy);
        if (Target == null)
        {
            SetTarget(enemy);
        }
    }
    
    public void EnemyOutOfRange(IEnemy enemy)
    {
        enemiesInMaxRange.Remove(enemy);
        if (Target == enemy)
        {
            Target = null;
        }
    }
    
    public void FindNewTarget()
    {
        if (enemiesInMaxRange.Count <= 0)
        {
            return;
        }

        SetTarget(enemiesInMaxRange.FirstOrDefault());
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
        if (Target != enemy)
        {
            return;
        }
        // Unsubscribe from the dead target's OnDeath event
        Target.OnDeath -= HandleTargetDeath;
        Target = null;
    }
    

    public void Fire()
    {
        LaserVisuals.SetLaserPositions(laser, transform.position, Target.Collider);
        Target?.TakeDamage(Damage * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maximumRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumRange);
    }
}