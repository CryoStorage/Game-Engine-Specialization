using UnityEngine;

public class CharacterSwapper : MonoBehaviour
{
    [Header("GameVariables")]
    [SerializeField] private IntVariableSo intSelectedCharacterIndexSo;
    [SerializeField] private IntVariableSo intSelectedSkinIndexSo;
    [SerializeField] private PlayerAnimControllersSo playerAnimControllersSo;
    public void NextCharacter()
    {
        if(intSelectedSkinIndexSo.value > playerAnimControllersSo.characters[intSelectedCharacterIndexSo.value].skins.Length - 1)
        {
            intSelectedSkinIndexSo.value = 0;
        }
        else
        {
            intSelectedSkinIndexSo.value++;
        }
    }
}
