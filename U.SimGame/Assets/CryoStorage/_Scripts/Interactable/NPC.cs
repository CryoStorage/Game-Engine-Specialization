using UnityEngine;

public class NPC : Interactable
{
    [Header("NPC Settings")]
    [SerializeField] private string npcName;
    [TextArea][SerializeField] private string dialogue;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onBeginDialogue;
    [SerializeField] private GameEvent onEndDialogue;
    
    [Header("GameVariables")]
    [SerializeField] private StringVariableSo currentNpcName;
    [SerializeField] private StringVariableSo currentDialogue;


    public override void Interact()
    {
        currentNpcName.value = npcName;
        currentDialogue.value = dialogue;
        onBeginDialogue.Raise();
    }
}
