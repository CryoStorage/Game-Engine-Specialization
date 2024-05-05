using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    public void OnEventRaised()
    {
        response.Invoke();
    }
    
    private void OnEnable()
    {
        if (gameEvent == null) return;
        gameEvent.RegisterListener(this);
    }
    
    private void OnDisable()
    {
        if (gameEvent == null) return;
        gameEvent.UnregisterListener(this);
    }

}
