using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMissiles : PoolManager<Projectile>
{
    [Header("Missile Shooting")] 
    public int missileSlots = 2;
    
    [SerializeField] private Transform missileSpawnPoint;
    [SerializeField] private float missileCooldown = 0.5f;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onMissileFired;
    [SerializeField] private GameEvent onNoMissileAvailable;
    [SerializeField] private GameEvent onMissilesReplenished;
    
    [Header("GameVariables")] 
    [SerializeField] private IntVariableSo missileCountSo;
    [SerializeField] private IntVariableSo missileSlotsSo;
    
    private PlayerInputHandler _playerInputHandler;
    private float _shootTimer;
    private int _firedMissiles;

    protected override void Start()
    {
        base.Start();
        
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _playerInputHandler.InputActions.Player.Fire2.performed += FireMissile;

    }

    private void OnDisable()
    {
        _playerInputHandler.InputActions.Player.Fire2.performed -= FireMissile;
    }

    private void Update()
    {
        _shootTimer += Time.deltaTime;
    }
    private void FireMissile(InputAction.CallbackContext context)
    {
        if (_firedMissiles >= missileSlots) return;
        if (_shootTimer < missileCooldown) return;

        var missile = Pool.Get();
        missile.Activate(missileSpawnPoint.position, missileSpawnPoint.rotation);
        missile.gameObject.SetActive(true);
        _firedMissiles++;
        missileCountSo.value = missileSlots - _firedMissiles;
        _shootTimer = 0;
    }

    protected override void OnTakeFromPool(Projectile missile)
    {
        missile.gameObject.SetActive(true);
    }

    protected override void OnReturnedToPool(Projectile missile)
    { 
        missile.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(Projectile missile)
    {
        Destroy(missile);
    }

    public void Replenish()
    {
        _firedMissiles = 0;
        onMissilesReplenished.Raise();
    }


    public void IncreaseCapacity()
    {
        missileSlots ++;
        missileSlotsSo.value = missileSlots;
    }
}
