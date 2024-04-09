using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimControllersSo", menuName = "PlayableCharacters/PlayerAnimControllersSo")]
public class PlayerAnimControllersSo : ScriptableObject
{
    public PlayableCharacterSo[] characters;
}
