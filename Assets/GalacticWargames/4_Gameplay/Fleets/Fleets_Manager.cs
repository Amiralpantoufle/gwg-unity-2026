using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class Fleets_Manager : MonoBehaviour
{
    [SerializeField] private GameObject fleetPrefab;
    [SerializeField] private Transform fleetsRoot;

    private List<Fleet_Instance> displayedFleets;


    private async void Start()
    {
        displayedFleets = new List<Fleet_Instance>();
        await Display_AvailableFleets();
    }

    public void Open_FleetScreen()
    {
        EventBus.Publish(new ReplaceScreenEvent
        {
            screenID = ScreenID.fleetScreen,
        });
    }

    private async Task Display_AvailableFleets()
    {
        FleetListResponse listOutput = await Load_PlayerFleets();

        if (listOutput == null)
        {
            Debug.LogError("No fleet found");
            return;
        }

        Fleet[] fleetList = listOutput.fleets;

        if (fleetList == null || fleetList.Length <= 0)
        {
            Debug.Log("No Fleet Loaded");
            return;
        }

        //Clear And Init
        Clear_Fleets();
        foreach (Fleet fleet in fleetList)
        {
            //Create_FleetInstance(fleet);
            Debug.Log("Create new fleet");
        }
    }

    private async Task<FleetListResponse> Load_PlayerFleets()
    {
        string endpoint = "/fleet/list";

        var response = await API_Client.Instance.LoadApiResponse<FleetListResponse>(endpoint);

        Debug.Log("Displaying fleets prcess");

        if (response == null)
        {
            Debug.LogError("Réponse fleet nulle.");
            return null;
        }

        return response.output;
    }
    private void Clear_Fleets()
    {
        displayedFleets.Clear();

        foreach (Transform child in fleetsRoot)
            Destroy(child);
    }

    private void Create_FleetInstance(Fleet fleet)
    {
        Fleet_Instance inst = Instantiate(fleetPrefab, fleetsRoot).GetComponent<Fleet_Instance>();

        if(inst!=null)
        {
            displayedFleets.Add(inst);

            inst._FleetName = fleet.name;
            inst._Fleet_AttackMode = fleet.active;

            Debug.Log("Adding fleet instance");
        }
    }
}
