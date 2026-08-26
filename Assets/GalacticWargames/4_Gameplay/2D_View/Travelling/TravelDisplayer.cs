using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TravelDisplayer : MonoBehaviour
{
    /// <summary>
    /// 0=TileDepartire(xx,yy) 1=TileDepartureLarge 2=TileArrival(xx,yy) 3=TimeLeft 4= targetName
    /// </summary>
    [SerializeField] TextMeshProUGUI[] info_txts;
    [SerializeField] Image[] travelTiles;
    [SerializeField] RectTransform shipTravel_Icon;
    [SerializeField] Image gaugeFiller;

    private void Start()
    { 
        Vector2[] list = new Vector2[travelTiles.Length];
        list[0] = new Vector2(24, 50);
        list[1] = new Vector2(50, 20);

        TravelInfoPatch travel = new TravelInfoPatch()
        {
            tiles_v = new Vector2Int(10, 20),
            tilesPos = list,
            timeLeft = 1900,
            timeTravelled = 100,
            targetName = "entity"
        };

        PatchTravel(travel);
    }
    private void PatchTravel(TravelInfoPatch patch)
    {
        string depString = patch.tilesPos[0].x.ToString() + "x." + patch.tilesPos[0].y.ToString() +"y";
        string arivString = patch.tilesPos[1].x.ToString() + "x." + patch.tilesPos[1].y.ToString() + "y";
        info_txts[0].text = depString;
        info_txts[1].text = patch.targetName;
        info_txts[2].text = arivString;

        /*VisualDefinition depVisual = GridVisualService.Instance.GetVisual(patch.tiles_v.x);
        VisualDefinition arivVisual = GridVisualService.Instance.GetVisual(patch.tiles_v.y);
        travelTiles[0].sprite = depVisual.imageSprite;
        travelTiles[1].sprite = arivVisual.imageSprite;*/

        //Display Time Left
        info_txts[3].text = GetTimeLeft(patch.timeLeft);
        float total = patch.timeLeft + patch.timeTravelled;
        float travelPercentage = (patch.timeTravelled * 100) / total;
        gaugeFiller.fillAmount = travelPercentage/100;
    }

    private string GetTimeLeft(float time)
    {
        TimeSpan t = TimeSpan.FromSeconds(Mathf.Max(0f, time));
        return $"{(int)t.TotalHours}h:{t.Minutes:00}m'{t.Seconds:00}''";
    }
}

public class TravelInfoPatch
{
    public Vector2Int tiles_v;
    public Vector2[] tilesPos;
    public float timeLeft;
    public float timeTravelled;
    public string targetName;
}