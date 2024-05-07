using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameplayUi : MonoBehaviour
{

    [FormerlySerializedAs("playerScore")]
    [Header("GameVariables")]
    [SerializeField]private IntVariableSo playerScoreSo  ;
    [SerializeField]private IntVariableSo playerHealthSo ;
    [SerializeField]private IntVariableSo playerShieldSo ;
    [SerializeField]private IntVariableSo missileCountSo ;
    [SerializeField]private IntVariableSo missileSlotsSo ;
    [SerializeField]private StringVariableSo timePlayedSo;
    
    
    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI missileCounter;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider shieldBar;
    
    [Header("Serialized References")]
    [SerializeField] private Image missileSlotPrefab;
    
    private List<Image> _missileSlots;
    private GameObject missilesLayoutGroup;
    private bool _gameRunning;
    
    void Start()
    {
        timePlayedSo.value = "00:00";
        timeText.text = timePlayedSo.value;
        playerScoreSo.value = 0;
        UpdateHealth();
        // for (int i = 0; i < missileSlotsSo.value; i++)
        // { 
        //     Instantiate(missileSlotPrefab, missilesLayoutGroup.transform);
        // }
    }

    void Update()
    {
        if (!_gameRunning) return;
        UpdateTime();
        UpdateHealth();
    }

    private void UpdateTime()
    {
        var minutes = TimeSpan.FromSeconds(Time.time).Minutes;
        var secs = TimeSpan.FromSeconds(Time.time).Seconds;
        var t = minutes.ToString("00") + ":" + secs.ToString("00");
        timeText.text = t;
        timePlayedSo.value = t;
    }
    
    public void UpdateScore()
    {
        playerScoreSo.value ++;
        scoreText.text = playerScoreSo.value.ToString();
    }
    
    public void UpdateHealth()
    {
        healthBar.value = playerHealthSo.value;
        shieldBar.value = playerShieldSo.value;
    }
    
    public void UpdateMissiles()
    {
        missileCounter.text = missileCountSo.value.ToString();
    }
    
    public void UpdateMissileSlots()
    {
        
    }
    
    public void ToggleGameRunning()
    {
        _gameRunning = !_gameRunning;
    }

}
