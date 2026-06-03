using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class GridVisualService : MonoBehaviour
{
    public static GridVisualService Instance;

    [SerializeField] private VisualDefinition[] visuals;

    [SerializeField] private VisualDefinition[] vd_SystemTile;
    [SerializeField] private VisualDefinition[] vd_Stars;

    [SerializeField] private VisualDefinition[] vd_PlanetTile;
    [SerializeField] private VisualDefinition[] vd_PlanetStruct;
    [SerializeField] private VisualDefinition[] vd_Planets;

    [SerializeField] private VisualDefinition[] vd_BaseBuildings;

    [SerializeField] private VisualDefinition[] vd_Ressources;
    [SerializeField] private VisualDefinition[] vd_FogOfWar;

    private VisualDefinition fallbackVisual;

    private void Awake()
    {
        Instance = this;
        CreateFallback();
    }

    public VisualDefinition Get(int imageId, int tag)
    {
        VisualDefinition[] selected_VD = TaggedDic((int)tag);

        foreach (VisualDefinition vd in selected_VD)
        {
            if (vd.image_id == imageId)
                return vd;
        }

        //Aucun id trouvé
        Debug.LogWarning($"Missing visual for imageId {imageId}");
        return fallbackVisual;
    }

    /// <summary>
    /// Categories : 0=SystemTiles | 1=PlanetTiles
    /// </summary>
    /// <param name="category"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public VisualDefinition GetVisual(int id)
    {
        var visual = visuals.FirstOrDefault(v =>

        v.image_id == id);

        if (visual == null)
        {
            Debug.LogError(
                $"No VisualDefinition found for id={id}");
        }

        return visual;
    }

    //Utility
    private void CreateFallback()
    {
        fallbackVisual = new VisualDefinition();

        fallbackVisual.image_id = -1;
        fallbackVisual.renderScale = 1f;
    }

    /// <summary>
    /// 0 & 1 = SystemTile | 2 = Planet Tiles | 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private VisualDefinition[] TaggedDic(int index)
    {

        switch(index)
        {
            case 0:
                return vd_SystemTile;
            case 1:
                return vd_SystemTile;
            case 2:
                return vd_PlanetTile;

        }

        //pas de tableau graphique trouvé
        return vd_SystemTile;
    }
}


