using System;
using UnityEngine;
public class ScreenClicker : MonoBehaviour
{
    private Vector3 _screenCoords; 
    private void Update()
    {
        _screenCoords = Input.mousePosition;
    }
}
