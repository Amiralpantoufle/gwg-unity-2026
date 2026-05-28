using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class GridVisualService : MonoBehaviour
{
    public static GridVisualService Instance;
    private bool loaded = false;

    private Dictionary<int, VisualDefinition> visualData;
    private VisualDefinition fallbackVisual;

    private void Awake()
    {
        Instance = this;
        CreateFallback();
    }

    public async Task LoadVisuals()
    {
        if (loaded) return;

        loaded = true;
    }

    public VisualDefinition Get(int imageId)
    {
        if (visualData.ContainsKey(imageId))
        {
            return visualData[imageId];
        }

        Debug.LogWarning($"Missing visual for imageId {imageId}");

        return fallbackVisual;
    }

    //Utility
    private void CreateFallback()
    {
        fallbackVisual = new VisualDefinition();

        fallbackVisual.image_id = -1;
        fallbackVisual.assetPath = "Design/Gameplay/missing";
        fallbackVisual.renderScale = 1f;
    }
}


