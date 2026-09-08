using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fleet_SpaceshipVignette : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shipName;
    [SerializeField] private TextMeshProUGUI shipCount;
    [SerializeField] private Image shipRender;

    public void Load_ShipVignette(spaceShip_Construct construct)
    {
        shipName.text = construct.nom_vas;
        shipCount.text = construct.nb_cible_vas.ToString();

        Sprite v = GridVisualService.Instance.GetSprite(construct.id_vas);
        if(v!=null)
            shipRender.sprite = v;
    }
}
