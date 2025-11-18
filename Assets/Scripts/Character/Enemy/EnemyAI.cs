using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Chase,
    Shoot,
    Dead
}

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public EnemyDisplay enemyDisplay;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public PlayerDisplay playerDisplay;
    public Transform player;
    public NavMeshAgent agent;
    public Rigidbody rb;

    [Header("Settings")]
    public float shootCooldown = 5f;
    private float shootTimer = 0f;

    private Vector3 lastKnownPlayerPos;
    public EnemyState currentState = EnemyState.Chase;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        playerDisplay = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDisplay>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb.isKinematic = true;
        rb.useGravity = false;

        agent.updateRotation = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Chase when GameManager.Instance.currentState != GameManager.GameState.InGame:
                break;
            case EnemyState.Chase:
                StateChase();
                break;

            case EnemyState.Shoot:
                StateShoot();
                break;

            case EnemyState.Dead:
                break;
        }
    }
    void StateChase()
    {
        if (player == null) return;
        agent.SetDestination(player.position);
        lastKnownPlayerPos = player.position;
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            shootTimer = 0f;
            currentState = EnemyState.Shoot;
        }
    }
    private void StateShoot()
    {
        if (GameManager.Instance.currentState != GameManager.GameState.InGame) return;
        agent.SetDestination(transform.position);
        Vector3 dir = (lastKnownPlayerPos - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
        Shoot();
        // ShootDebug(lastKnownPlayerPos);
        currentState = EnemyState.Chase;
    }

    private void ShootDebug(Vector3 targetPos)
    {
        Debug.Log("Enemy SHOOTS at last position: " + targetPos);
    }

    private void Shoot()
    {
        enemyDisplay.ShootAnimation();
        enemyDisplay.enemyAudioSource.PlayOneShot(AudioManager.AudioInstance.GunshotSFX);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Initialize(enemyDisplay.damage, false);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletScript.velocity, ForceMode.Impulse);
        Destroy(bullet, bulletScript.lifeTime);
    }
    public void Die()
    {
        currentState = EnemyState.Dead;
        agent.enabled = false;
    }
}
