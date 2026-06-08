using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère l'affichage, la pile et la navigation entre tous les écrans
/// </summary>
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    private Stack<UIScreen> screenStack = new Stack<UIScreen>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Afficher un écran en cherchant dans screenStack le premier dernier élément sans modifier les données.
    /// </summary>
    public void Push(UIScreen screen)
    {
        if (screenStack.Count > 0)
            screenStack.Peek().Hide();

        screenStack.Push(screen);
        screen.Show();
    }
    /// <summary>
    /// Retire l'écran au sommet de la pile ScreenStack pour le remplacer par nouvel écran
    /// </summary>
    /// <param name="screen"></param>
    public void Pop()
    {
        if (screenStack.Count == 0) return;

        var screen = screenStack.Pop();
        screen.Hide();

        if (screenStack.Count > 0)
            screenStack.Peek().Show();
    }
    /// <summary>
    /// Retire le dernier élément de la pile screenStack et le désactive
    /// </summary>
    public void Replace(UIScreen screen)
    {
        if (screenStack.Count > 0)
        {
            var old = screenStack.Pop();
            old.Hide();
        }

        screenStack.Push(screen);
        screen.Show();
    }
    public void Clear()
    {
        while (screenStack.Count > 0)
        {
            screenStack.Pop().Hide();
        }
    }
}
