using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayableCharacterSo", menuName = "PlayableCharacters/PlayableCharacterSo")]
public class PlayableCharacterSo : ScriptableObject
{
    public AnimatorController baseController;
    public CharacterSkin[] skins;
}

[Serializable]
public struct CharacterSkin
{
    public string name;
    public AnimatorController controller;
    public bool owned;

}
