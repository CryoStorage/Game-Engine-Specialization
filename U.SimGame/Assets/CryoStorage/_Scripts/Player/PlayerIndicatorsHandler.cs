using UnityEngine;

public class PlayerIndicatorsHandler : MonoBehaviour
{ 
    [Header("Serialized References")]
    [SerializeField] private SpriteRenderer indicatorRenderer;
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite controllerSprite;
    
    public void SetIndicatorsKeyboard()
    {
        indicatorRenderer.sprite = keyboardSprite;
    }

    public void SetIndicatorsGamepad()
    {
        indicatorRenderer.sprite = controllerSprite;
    }
}
