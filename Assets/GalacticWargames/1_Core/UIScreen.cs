using UnityEngine;

public class UIScreen : MonoBehaviour
{
    /// <summary>
    /// Fonction d'initialisation du Gameobject UI
    /// </summary>
    /// <param name="data"></param>
    public virtual void Init(object data = null) { }
    /// <summary>
    /// Active le GameObject UI lié
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Desactive le GameObject UI lié
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void PostToken(string response, int type, float time)
    {
        EventBus.Publish(new ShowToastEvent
        {
            toastType = type,
            message = response,
            duration = time
        });
    }
}
