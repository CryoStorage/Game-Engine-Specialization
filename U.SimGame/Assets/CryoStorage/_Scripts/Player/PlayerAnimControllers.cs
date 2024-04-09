using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimControllers", menuName = "PlayerAnimControllers")]
public class PlayerAnimControllers : ScriptableObject
{
    public AnimatorController[] animatorControllers;
}
