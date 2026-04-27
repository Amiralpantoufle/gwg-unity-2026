using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    public string key;
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        Refresh();
    }

    /// <summary>
    /// Met à jour le texte vers nouvelle traduction indexée
    /// </summary>
    public void Refresh()
    {
        if (LocalizationManager.Instance != null)
        {
            textComponent.text = LocalizationManager.Instance.Get(key);
        }
    }
}
