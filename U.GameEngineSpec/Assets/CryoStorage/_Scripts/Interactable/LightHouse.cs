using UnityEngine;
using CryoStorage;


[SelectionBase]
public class LightHouse : Interactable
{
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent _onEnterLightHouse;
    [SerializeField] private GameEvent _onExitLightHouse;
    
    [Header("Serialized Fields")]
    [SerializeField] private Vector3VariableSo _inputDir;
    [SerializeField] private GameObject lightObject;
    
    private bool _isInLightHouse = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Visualize();
        GetLightDirection();
    }
    
    private void GetLightDirection()
    {
        float targetAngle = CryoMath.AngleFromOffset(_inputDir.Value);
        var position = transform.position;
        Vector3 lightDirection = CryoMath.PointOnRadius(position, 1, targetAngle);
        lightObject.transform.rotation = CryoMath.AimAtDirection(position, lightDirection.normalized);  
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

    private void Visualize()
    {
        Debug.DrawRay(lightObject.transform.position, lightObject.transform.forward * 5, Color.green);
    }
}
