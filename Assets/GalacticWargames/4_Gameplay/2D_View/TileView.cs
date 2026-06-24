using UnityEngine;

public class TileView : MonoBehaviour
{
    [SerializeField] private GridTile tile;

    public void Init(GridTile data)
    {
        tile = data;
    }
}