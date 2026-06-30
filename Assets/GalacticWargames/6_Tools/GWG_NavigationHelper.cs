using UnityEngine;

public class GWG_NavigationHelper : MonoBehaviour
{
    [SerializeField] private GameObject[] ScreenUI;
    [SerializeField] private GameObject[] PopupUI;
    [SerializeField] private GameObject LoadingScreen;

    public void ResetToPlayState()
    {
        foreach (GameObject screen in ScreenUI)
        {
            screen.SetActive(false);

        }
        foreach (GameObject popup in PopupUI)
        {
            popup.SetActive(false);

        }

        LoadingScreen.SetActive(true);
    }
}
