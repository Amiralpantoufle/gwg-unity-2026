using System.Collections.Generic;
using UnityEngine;

public enum ScreenID
{
    Auth,
    Home
}
/// <summary>
/// Centralise la navigation avec des IDs au lieu de références directes.
/// </summary>
public class NavigationService : MonoBehaviour
{
    public static NavigationService Instance;

    [System.Serializable]
    public class ScreenEntry
    {
        public ScreenID id;
        public UIScreen screen;
    }

    public List<ScreenEntry> screens;

    private Dictionary<ScreenID, UIScreen> screenDict;

    private void Awake()
    {
        Instance = this;

        screenDict = new Dictionary<ScreenID, UIScreen>();
        foreach (var entry in screens)
        {
            screenDict[entry.id] = entry.screen;
        }
    }

    public void Open(ScreenID id, object data = null)
    {
        if (!screenDict.ContainsKey(id))
        {
            Debug.LogError($"Screen not found: {id}");
            return;
        }

        var screen = screenDict[id];
        screen.Init(data);

        ScreenManager.Instance.Push(screen);
    }

}
