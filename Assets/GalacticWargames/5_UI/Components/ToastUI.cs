using UnityEngine;
using TMPro;
using System.Collections;

public class ToastUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    private ToastManager manager;
    private float duration;

    public void Init(string message, float duration, ToastManager manager)
    {
        messageText.text = message;
        this.duration = duration;
        this.manager = manager;

        StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(duration);
        Close();
    }

    public void OnClick()
    {
        Close();
    }

    private void Close()
    {
        manager.RemoveToast(gameObject);
    }
}