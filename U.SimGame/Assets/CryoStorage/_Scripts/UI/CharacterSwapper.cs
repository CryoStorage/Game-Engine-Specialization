
using UnityEngine;

public class CharacterSwapper : MonoBehaviour
{
    [SerializeField] private IntVariableSo currentCharacterIndex;
    [SerializeField] private PlayerAnimControllers playerAnimControllers;
    public void NextCharacter()
    {
        if (currentCharacterIndex.value >= playerAnimControllers.animatorControllers.Length - 1)
        {
            currentCharacterIndex.value = 0;
            return;
        }
        currentCharacterIndex.value += 1;
    }
    
    public void PreviousCharacter()
    {
        if (currentCharacterIndex.value <= 0)
        {
            currentCharacterIndex.value = playerAnimControllers.animatorControllers.Length - 1;
            return;
        }
        currentCharacterIndex.value -= 1;
    }
}
