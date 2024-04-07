using UnityEngine;
using TMPro;

[SelectionBase]
public class UIManager : MonoBehaviour
{
    [Header("Serialized References")]
    [SerializeField] private TMP_Text timerValue;
    [SerializeField] private TMP_Text waveValue;
    [SerializeField] private TMP_Text finalScoreValue;
    
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onGameStart;
    [SerializeField] private GameEvent onGameEnd;
    
    [Header("GameVariables")]
    [SerializeField] private FloatVariableSo timeUntilNextWaveSo;
    [SerializeField] private IntVariableSo waveNumberSo;
    
    private bool _gameStarted;

    private void Start()
    {
        waveNumberSo.value = 0;
        UpdateWaveNumber();
    }

    public void StartGame()
    {
        if (_gameStarted)return;
        onGameStart.Raise();
        _gameStarted = true;
    }
    
    public void EndGame()
    {
        onGameEnd.Raise();
    }
    
    public void UpdateWaveNumber()
    {
        waveValue.text = waveNumberSo.value.ToString();
        finalScoreValue.text = waveNumberSo.value.ToString();
    }

    private void Update()
    {
        switch (timeUntilNextWaveSo.value)
        {
            case float f when f < 9:
            timerValue.text = "0:0" + timeUntilNextWaveSo.value.ToString("F0");
                break;
            case float f when f > 10:
            timerValue.text = "0:" + timeUntilNextWaveSo.value.ToString("F0");
                break;
        }
    }
}
