using UnityEngine;

public class CloseScreenButton : MonoBehaviour
{
    public ScreenID targetScreen;

    /// <summary>
    /// Close Screen to go backward
    /// </summary>
    public void OnClick()
    {
        ScreenManager.Instance.Pop();
    }
}
