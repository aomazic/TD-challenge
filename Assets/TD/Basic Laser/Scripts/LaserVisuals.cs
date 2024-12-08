using UnityEngine;

public static class LaserVisuals
{
    private const float FirePointYOffset = 1f;
    private const float FirePointXOffset = 0.4f;
    
    /**
     * The offset to make the fire point appear above the tower
     */
    private const float AboveYOffset = 0.6f;

    public static void SetLaserPositions(LineRenderer lineRenderer, Vector3 tower, Collider2D enemyCollider)
    {
        var firePoint = tower;
        var closestPoint = enemyCollider.ClosestPoint(tower);
        var direction = GetDirection(tower, closestPoint);
        
        AdjustTowerPosition(ref firePoint, direction);
        lineRenderer.SetPosition(0, firePoint);
        lineRenderer.SetPosition(1, closestPoint);
    }

    private static Vector2 GetDirection(Vector3 tower, Vector3 enemy)
    {
        return new Vector2(enemy.x - tower.x, enemy.y - tower.y).normalized;
    }

    /// <summary>
    ///  Adjusts the fire point position based on the direction of the enemy
    /// </summary>
    /// <param name="firePoint">Reference to the fire point position</param>
    /// <param name="direction">Direction to the enemy from tower</param>
    private static void AdjustTowerPosition(ref Vector3 firePoint, Vector2 direction)
    {
        firePoint.y += FirePointYOffset; 
        
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            AdjustHorizontalPosition(ref firePoint, direction.x);
        }
        else if (direction.y > 0)
        {
            firePoint.y += AboveYOffset;
        }
    }

    /// <summary>
    ///  Adjusts the fire point x position based on the direction of the enemy
    /// </summary>
    /// <param name="firePoint">Reference to the fire point position</param>
    /// <param name="directionX">X Direction to the enemy from tower</param>
    private static void AdjustHorizontalPosition(ref Vector3 firePoint, float directionX)
    {
        if (directionX > 0)
        {
            firePoint.x += FirePointXOffset;
        }
        else
        {
            firePoint.x -= FirePointXOffset;
        }

    }
}