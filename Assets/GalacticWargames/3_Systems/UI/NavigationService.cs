using System.Collections.Generic;
using UnityEngine;

public enum ScreenID
{
    Auth,
    Main,
    Base
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


    //Listeners
    private void OnEnable()
    {
        EventBus.Subscribe<OpenScreenByIDEvent>(OnOpenScreen);
        EventBus.Subscribe<ReplaceScreenEvent>(OnReplaceScreen);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OpenScreenByIDEvent>(OnOpenScreen);
        EventBus.Unsubscribe<ReplaceScreenEvent>(OnReplaceScreen);
    }
    /// <summary>
    /// Event => Ouverture d'un écran
    /// </summary>
    /// <param name="e"></param>
    private void OnOpenScreen(OpenScreenByIDEvent e)
    {
        //Open(e.screenID, e.data);

        if (!screenDict.ContainsKey(e.screenID))
        {
            Debug.LogError($"Screen not found: {e.screenID}");
            return;
        }

        var screen = screenDict[e.screenID];
        screen.Init(e.data);

        ScreenManager.Instance.Push(screen);
    }
    private void OnReplaceScreen(ReplaceScreenEvent e)
    {
        if (!screenDict.ContainsKey(e.screenID))
        {
            Debug.LogError($"Screen not found: {e.screenID}");
            return;
        }

        var screen = screenDict[e.screenID];

        ScreenManager.Instance.Replace(screen);
    }
}
