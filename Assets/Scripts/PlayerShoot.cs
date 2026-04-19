using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootCooldown = 0.3f;

    private PlayerMovement playerMovement;
    private float nextShootTime;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        Debug.Log("PlayerShoot Awake called on: " + gameObject.name);

        if (playerMovement == null)
        {
            Debug.LogError("PlayerShoot could not find PlayerMovement on the same GameObject.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F was pressed.");

            if (playerMovement != null && playerMovement.IsCrouching)
            {
                Debug.Log("Cannot shoot while crouching.");
                return;
            }

            if (Time.time >= nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + shootCooldown;
            }
            else
            {
                Debug.Log("Shoot blocked by cooldown.");
            }
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot() called.");

        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is NOT assigned.");
            return;
        }

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement reference is null.");
            return;
        }

        if (playerMovement.FirePoint == null)
        {
            Debug.LogError("FirePoint is NOT assigned in PlayerMovement.");
            return;
        }

        Debug.Log("Spawning projectile at: " + playerMovement.FirePoint.position);

        GameObject projectileObj = Instantiate(
            projectilePrefab,
            playerMovement.FirePoint.position,
            Quaternion.identity
        );

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            float direction = playerMovement.FacingRight ? 1f : -1f;
            Debug.Log("Projectile direction set to: " + direction);
            projectile.SetDirection(direction);
        }
        else
        {
            Debug.LogError("Spawned projectile does not have a Projectile script.");
        }
    }
}