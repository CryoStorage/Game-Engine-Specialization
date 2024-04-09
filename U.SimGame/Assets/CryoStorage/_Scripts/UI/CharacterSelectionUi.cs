using UnityEngine;

public class CharacterSelectionUi : MonoBehaviour
{
    [SerializeField] private IntVariableSo currentCharacterIndex;
    public void SetCharacterId(int id)
    {
        currentCharacterIndex.value = id;
    }
}
