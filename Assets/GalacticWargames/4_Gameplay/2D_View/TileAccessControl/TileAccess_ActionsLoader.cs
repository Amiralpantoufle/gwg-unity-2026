using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileAccess_ActionsLoader : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject actionButtonPrefab;
    public void PopulateActions(TileActions_Model actionsList)
    {
        // Nettoyage
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (TileAction_Model action in actionsList.actions)
        {
            GameObject buttonGO = Instantiate(actionButtonPrefab, content);

            // Exemple texte
            TMP_Text label = buttonGO.GetComponentInChildren<TMP_Text>();
            label.text = action.label;

            // Ajout du composant placeholder

        }
    }
}
