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
    private async Task Display_AvailableFleets()
    {
        FleetListResponse listOutput = await Load_PlayerFleets();
        if (listOutput == null)
            return;

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
            Create_FleetInstance(fleet);
            Debug.Log("Create new fleet");
        }
    }

    private async Task<FleetListResponse> Load_PlayerFleets()
    {

        string endpoint = "/fleet/list";
        var response = await API_Client.Instance.LoadApiResponse<FleetListResponse>(endpoint);

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
        GameObject obj = Instantiate(fleetPrefab, fleetsRoot);
        Fleet_Instance inst = obj.GetComponent<Fleet_Instance>();

        if(inst!=null)
        {
            displayedFleets.Add(inst);
            inst.Load_FleetInstance(fleet);

            Debug.Log("Adding fleet instance");
        }
    }
}
