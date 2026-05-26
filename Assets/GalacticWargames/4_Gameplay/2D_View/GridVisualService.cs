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

        string json = await API_Client.Instance.GetAsync("/base/isometric/all");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("Isometric visual json is null");
            return;
        }

        var data = JsonConvert.DeserializeObject<List<VisualDefinition>>(json);

        visualData = new Dictionary<int, VisualDefinition>();
        foreach (VisualDefinition visual in data)
        {
            visualData.Add(visual.image_id, visual);
        }

        Debug.Log("Finished Loading API Visuals");
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


