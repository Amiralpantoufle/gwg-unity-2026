using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class GridVisualService : MonoBehaviour
{
    public static GridVisualService Instance;

    [SerializeField] private VisualDefinition[] visuals;

    private void Awake()
    {
        Instance = this;
    }

    public VisualDefinition GetVisual(int id)
    {
        var visual = visuals.FirstOrDefault(v =>

        v.image_id == id);

        if (visual == null)
        {
            Debug.Log($"No VisualDefinition found for id={id}");
            visual = EmptyModelCallback();
        }

        return visual;
    }

    private VisualDefinition EmptyModelCallback()
    {
        VisualDefinition empty = new VisualDefinition
        {
            category = VisualCategory.SystemTile,
            imageSprite = null,
            renderScale = 1
        };

        return empty;
    }
}


