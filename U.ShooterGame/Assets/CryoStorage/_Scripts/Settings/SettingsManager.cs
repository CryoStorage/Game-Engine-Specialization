using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private BoolVariableSO disableScreenShake;
    
    public void SetScreenShake(bool b)
    {
        disableScreenShake.value = b;
    }
}
