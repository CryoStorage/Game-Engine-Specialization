using System;
using UnityEngine;
using CryoStorage;
using UnityEngine.Serialization;


[SelectionBase]
public class LightHouse : Interactable
{
    [Header("LightHouse Settings")]
    [SerializeField] private float angleLerpSpeed = 1f;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent _onEnterLightHouse;
    [SerializeField] private GameEvent _onExitLightHouse;
    
    [FormerlySerializedAs("inputDir")]
    [FormerlySerializedAs("_inputDir")]
    [Header("Serialized Fields")]
    [SerializeField] private Vector2VariableSo inputDirSo;
    [SerializeField] private GameObject lightObject;
    
    private bool _isInLightHouse = true;
    private float _currentLookAngle;
    private float _targetLookAngle;
    private Vector3 _lookPos;

    void Update()
    {
        GetLightDirection();
    }
    
    private void GetLightDirection()
    {
        if (inputDirSo.value == Vector2.zero) return;
        _targetLookAngle = CryoMath.AngleFromOffset(inputDirSo.value);
        var position = lightObject.transform.position;
        _lookPos = CryoMath.PointOnRadius(position, 5 , _currentLookAngle);
        LerpAngle(ref _currentLookAngle, _targetLookAngle, angleLerpSpeed);
        lightObject.transform.rotation = CryoMath.AimAtDirection(position, _lookPos);  
    }

    private void LerpAngle(ref float currentAngle, float targetAngle, float angleChangeSpeed)
    {
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, angleChangeSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(lightObject.transform.position, 5);
        Gizmos.DrawSphere(_lookPos, 0.5f);
    }

    private void EnableLight()
    {
        lightObject.SetActive(true);
    }
    
    private void DisableLight()
    {
        lightObject.SetActive(false);
    }

    public override void Interact()
    {
        if (_isInLightHouse)
        {
            _onExitLightHouse.Raise();
            _isInLightHouse = false;
            DisableLight();
        }
        else
        {
            _onEnterLightHouse.Raise();
            _isInLightHouse = true;
            EnableLight();
        }
    }
}
