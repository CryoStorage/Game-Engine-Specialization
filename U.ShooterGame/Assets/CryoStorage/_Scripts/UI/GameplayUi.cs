using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUi : MonoBehaviour
{

    [Header("GameVariables")]
    [SerializeField]private IntVariableSo playerScore;
    [SerializeField]private IntVariableSo playerHealth;
    [SerializeField]private IntVariableSo playerShield;
    [SerializeField]private IntVariableSo missileCount;
    [SerializeField]private IntVariableSo missileSlots;
    [SerializeField]private StringVariableSo timePlayed;
    
    
    [Header("UI Elements")]
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider shieldBar;
    
    [Header("Serialized References")]
    [SerializeField] private Image missileSlotPrefab;
    
    private List<Image> _missileSlots;
    private GameObject missilesLayoutGroup;
    private bool _gameRunning;
    
    void Start()
    {
        timePlayed.value = "00:00";
        timeText.text = timePlayed.value;
        playerScore.value = 0;
        for (int i = 0; i < missileSlots.value; i++)
        { 
            Instantiate(missileSlotPrefab, missilesLayoutGroup.transform);
        }
    }

    void Update()
    {
        if (!_gameRunning) return;
        UpdateTime();
    }

    private void UpdateTime()
    {
        var minutes = TimeSpan.FromSeconds(Time.time).Minutes;
        var secs = TimeSpan.FromSeconds(Time.time).Seconds;
        var t = minutes.ToString("00") + ":" + secs.ToString("00");
        timeText.text = t;
        timePlayed.value = t;
    }
    
    public void UpdateScore()
    {
        playerScore.value ++;
        scoreText.text = playerScore.value.ToString();
    }
    
    public void UpdateHealth()
    {
        healthBar.value = playerHealth.value;
        shieldBar.value = playerShield.value;
    }
    
    public void UpdateMissiles()
    {
    }
    
    public void UpdateMissileSlots()
    {
        
    }
    
    public void ToggleGameRunning()
    {
        _gameRunning = !_gameRunning;
    }

}
