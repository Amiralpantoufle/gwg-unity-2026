using UnityEngine;

/// <summary>
/// Gère l'état global actuel du jeu 
/// </summary>
public class UIStateManager : MonoBehaviour
{
    public static UIStateManager Instance;

    public UIState CurrentState { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Défini un état d'UI
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(UIState newState)
    {
        CurrentState = newState;

        EventBus.Publish(new UIStateChangedEvent
        {
            newState = newState
        });
    }
}
