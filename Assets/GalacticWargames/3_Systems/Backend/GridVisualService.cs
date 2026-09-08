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
        var visual = visuals.FirstOrDefault(v => v.image_id == id);

        if (visual == null)
        {
            Debug.Log($"No VisualDefinition found for id={id}");
        }

        return visual;
    }
    public Sprite GetSprite(int id)
    {
        var visual = visuals.FirstOrDefault(v => v.image_id == id);

        if (visual == null)
        {
            Debug.Log($"No VisualDefinition found for id={id}");
        }

        Sprite s = visual.imageSprite;
        return s;
    }
}


