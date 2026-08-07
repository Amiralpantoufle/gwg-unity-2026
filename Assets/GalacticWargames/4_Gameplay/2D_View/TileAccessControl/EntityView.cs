using UnityEngine;

public class EntityView : MonoBehaviour 
{
    private int entitiesLayer = 10001;

    [SerializeField] private GridEntityModel entity;
    private SpriteRenderer spriteR;

    public void Init(GridEntityModel data)
    {
        if(spriteR==null)
            spriteR = GetComponent<SpriteRenderer>();

        entity = data;
        Refresh();
    }
    private void Refresh()
    {
        VisualDefinition visual = GridVisualService.Instance.GetVisual(entity.v);
        spriteR.sprite = visual.imageSprite;

        //offset & Scale
        transform.localScale = Vector3.one * visual.renderScale;
        Vector2 tilePosition = new Vector2(transform.localPosition.x + visual.offset.x, transform.localPosition.y + visual.offset.y);
        transform.localPosition = tilePosition;

        //SetSpriteOrder
        spriteR.sortingOrder = entitiesLayer;
    }

    public void ResetView()
    {
        spriteR.sprite = null;

        transform.localScale = Vector3.one;

        transform.rotation = Quaternion.identity;

        entity = null;
    }
}
