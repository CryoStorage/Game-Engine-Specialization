using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [Header("Serialized References")]
    [SerializeField] private GameObject nextIndicator;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField] private TMPro.TextMeshProUGUI npcNameText;
    
    [Header("GameVariables")]
    [SerializeField] private StringVariableSo currentNpcName;
    [SerializeField] private StringVariableSo currentDialogue;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onEndDialogue;
    
    private int _currentStringId;
    private string[] _subStrings;
    private bool _lastSubstring;
    private bool _isDialogueActive;

    public void BeginDialogue()
    {
        if(_isDialogueActive) return;
        _isDialogueActive = true;
        _subStrings = BuildSubstrings(currentDialogue.value);
        npcNameText.text = currentNpcName.value;
        _currentStringId = 0;
        DisplayDialogueSequence(_subStrings);
    }

    private void DisplayDialogueSequence(string[] text)
    {
        nextIndicator.SetActive(true);
        dialogueText.text = "";

        if (_currentStringId >= text.Length-1)
        {
            dialogueText.text = text[_currentStringId];
        }
        else
        {
            dialogueText.text = text[_currentStringId];
            nextIndicator.SetActive(true);
        }
    }
        
    public void StepSequence()
    {
        if (!_isDialogueActive) return;

        if (_currentStringId >= _subStrings.Length - 1) // If at or beyond the last substring
        {
            if (_lastSubstring) // Already at the last substring
            {
                onEndDialogue.Raise();
                Debug.Log("end dialogue");
                _isDialogueActive = false; // Dialogue is no longer active
                return;
            }
            else // First time at the last substring
            {
                _lastSubstring = true;
            }
        }
        else
        {
            _currentStringId++;
        }

        DisplayDialogueSequence(_subStrings);
    }
    
    // Recursive function splits the dialogue into substrings
    private string[] BuildSubstrings(string text, int startId = 0)
    {
        List<string> substrings = new List<string>();
        if (string.IsNullOrEmpty(text))
        {
            return substrings.ToArray();
        }

        int commaIndex = text.IndexOf(",", startId, StringComparison.Ordinal);
        int periodIndex = text.IndexOf(".", startId, StringComparison.Ordinal);

        int delimiterIndex = -1;
        if (commaIndex != -1 && periodIndex != -1)
            delimiterIndex = Math.Min(commaIndex, periodIndex);
        else if (commaIndex != -1)
            delimiterIndex = commaIndex;
        else if (periodIndex != -1)
            delimiterIndex = periodIndex;

        if (delimiterIndex == -1) // Delimiter not found
        {
            substrings.Add(text);
            return substrings.ToArray();
        }

        string sub = text.Substring(0, delimiterIndex);
        substrings.Add(sub);
            
        return substrings.Concat(BuildSubstrings(text.Substring(delimiterIndex + 1).Trim())).ToArray();
    }
}
