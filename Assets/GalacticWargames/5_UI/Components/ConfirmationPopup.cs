using TMPro;
using UnityEngine;
using System;

public class ConfirmationPopup : MonoBehaviour
{
    public static ConfirmationPopup Instance { get; private set;  }
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI messageText;

    private Action onConfirm;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public static void Show(string title,string message,Action onConfirm)
    {
        Instance.ShowInternal(title, message, onConfirm);
    }

    private void ShowInternal(
        string title,
        string message,
        Action onConfirm)
    {
        titleText.text = title;
        messageText.text = message;

        this.onConfirm = onConfirm;

        panel.SetActive(true);
    }
    //Actions
    public void Confirm()
    {
        onConfirm?.Invoke();
        Close();
    }

    public void Cancel()
    {
        Close();
    }

    private void Close()
    {
        onConfirm = null;
        panel.SetActive(false);
    }
}
