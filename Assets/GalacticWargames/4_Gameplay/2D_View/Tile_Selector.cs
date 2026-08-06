using UnityEngine;

public class Tile_Selector : MonoBehaviour
{
    [SerializeField] private Vector2 offset;

    //Selection
    [SerializeField] private SpriteRenderer selector;
    [SerializeField] private Sprite[] selectorVariants;

    public void HighlightTile(int typeOf, Vector2 pos)
    {
        selector.sprite = selectorVariants[typeOf];
        selector.transform.position = pos+offset;

        selector.enabled = true;
    }

    public void Disable_Selector()
    {
        selector.enabled = false;
    }
}
