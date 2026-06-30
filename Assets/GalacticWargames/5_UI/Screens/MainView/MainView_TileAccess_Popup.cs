using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MainView_TileAccess_Popup : UIScreen
{
    [SerializeField] private TextMeshProUGUI tileName;
    private TileView selectedTileView;
    public TileView _SelectedTileview { get { return selectedTileView; } }

    //Components
    private TileAccess_ActionsLoader tileAccessor;

    private void Awake()
    {
        tileAccessor = GetComponent<TileAccess_ActionsLoader>();
        if (tileAccessor == null) Debug.LogError("No TileAccessor found on " + gameObject.name);
    }
    public async void LoadTileViewData(TileView tile)
    {
        selectedTileView = tile;
        tileName.text = selectedTileView.id_esp.ToString();
        selectedTileView.HighlightTile();

        TileActions_Model actionsList = await GetTileActions(selectedTileView.id_esp);
        if (actionsList.actions.Count != 0) tileAccessor.PopulateActions(actionsList);
    }
    public void HideCurrentTile()
    {
        if (selectedTileView != null)
            selectedTileView.HideTile();
    }
    public void OnClose()
    {
        EventBus.Publish(new HidePopupEvent
        {
            hidePopup = this
        });
    }

    //Utility
    private async Task<TileActions_Model> GetTileActions(int id_esp)
    {
        string json = await API_Client.Instance.GetAsync($"/map/tile-actions?id_esp={id_esp}");

        if (string.IsNullOrEmpty(json))
            return null;

        ApiResponse<TileActions_Model> response = JsonConvert.DeserializeObject<ApiResponse<TileActions_Model>>(json);

        if (response == null)
        {
            Debug.LogError("Impossible de parser EntityModelOuput");
            return null;
        }

        if (response.error)
        {
            Debug.LogError($"EntityModelOuput API ERROR : {response.error}");
            return null;
        }

        if (response.output == null)
        {
            Debug.LogError("EntityModelOuput output null");
            return null;
        }

        return response.output;

    }

}
