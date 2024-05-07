using UnityEngine;
using UnityEngine.Serialization;

public class PlayerGun : PoolManager<Projectile>
{
    [Header("Bullet Shooting")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletCooldown = 0.5f;
    [SerializeField] private int poolSize = 20;
    
    [Header("Laser Settings")]
    [SerializeField] private float laserLength = 10f;
    [SerializeField] private LayerMask laserLayerMask;

    private PlayerInputHandler _playerInputHandler;
    private float _shootTimer;
    private LineRenderer _lineRenderer;

    protected override void Start()
    {
        base.Start();
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        PoolSize = poolSize;
    }

    private void Update()
    {
        _shootTimer += Time.deltaTime;
        var pressed = _playerInputHandler.InputActions.Player.Fire1.ReadValue<float>() > .5f;
        Shoot(pressed);
        LaserCast();
    }
    protected override void OnTakeFromPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    protected override void OnReturnedToPool(Projectile bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(Projectile bullet)
    {
        Destroy(bullet);
    }

    private void LaserCast()
    {
        if (!Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out var hit, laserLength, laserLayerMask))
        {
            var positions = new[] {bulletSpawnPoint.position, bulletSpawnPoint.position + bulletSpawnPoint.forward * 10f};
            _lineRenderer.SetPositions(positions);
        }
        else
        {
            var positions = new[] {bulletSpawnPoint.position, hit.point};
            _lineRenderer.SetPositions(positions);
        }
    }
    private void Shoot(bool pressed)
    {
        if (!pressed) return;
        if (_shootTimer < bulletCooldown) return;
    
        var bullet = Pool.Get();
        bullet.Activate(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    
        _shootTimer = 0;
    }
}