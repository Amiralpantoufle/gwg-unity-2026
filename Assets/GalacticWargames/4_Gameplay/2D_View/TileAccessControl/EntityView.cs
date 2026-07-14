using UnityEngine;

public class EntityView : MonoBehaviour
{
    private int entitiesLayer = 10001;

    [SerializeField] private EntityDto entity;
    private SpriteRenderer spriteR;

    public void Init(EntityDto data)
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
