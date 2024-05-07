using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class CameraShakeWrapper : MonoBehaviour
{
    
    [SerializeField] private CameraShaker cameraShaker;
    
    [Header("ShakeData")]
    [SerializeField] private ShakeData shakeDataTiny;
    [SerializeField] private ShakeData shakeDataSmall;
    [SerializeField] private ShakeData shakeDataMedium;
    [SerializeField] private ShakeData shakeDataLarge;
    
    
    [Header("GameVariables")]
    [SerializeField] private BoolVariableSO disableScreenShakeSo;
    
    public void ShakeTiny()
    {
        if (disableScreenShakeSo.value) return;
        cameraShaker.Shake(shakeDataTiny);
    }
    public void ShakeSmall()
    {
        if (disableScreenShakeSo.value) return;
        cameraShaker.Shake(shakeDataSmall);
    }
    
    public void ShakeMedium()
    {
        if (disableScreenShakeSo.value) return;
        cameraShaker.Shake(shakeDataMedium);
    }
    
    public void ShakeLarge()
    {
        if (disableScreenShakeSo.value) return;
        cameraShaker.Shake(shakeDataLarge);
    }
}
