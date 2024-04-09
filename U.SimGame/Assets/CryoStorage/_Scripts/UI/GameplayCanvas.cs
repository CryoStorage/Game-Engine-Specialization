using System;
using TMPro;
using UnityEngine;

public class GameplayCanvas : MonoBehaviour
{
    [Header("Serialized References")]
    [SerializeField]private TextMeshProUGUI _goldText;
    [Header("GameVariables")]
    [SerializeField]private IntVariableSo _intCurrentGoldSo;

    private void Start()
    {
        _intCurrentGoldSo.value = 0;
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        _goldText.text = _intCurrentGoldSo.value.ToString();
    }
}
