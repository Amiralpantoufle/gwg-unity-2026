using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;

public class SyncManager : MonoBehaviour
{
    [SerializeField] private float refreshEvery= 1f;

    private TickService tickService;
    private TickProcessor tickProcessor;

    private string currentCursor;

    private bool running;

    private async void Start()
    {
        tickService = new TickService(API_Client.Instance);
        tickProcessor = new TickProcessor();

        running = true;

        await TickLoop();
        Debug.Log("Start  Live Refreshing");
    }

    private async Task TickLoop()
    {
        while (running)
        {
            await RefreshTick();

            await Task.Delay(TimeSpan.FromSeconds(refreshEvery));
        }
    }
    private async Task RefreshTick()
    {
        Debug.Log("Refresh Tick");

        TickResponse response = await tickService.GetTick(currentCursor);

        if (response == null)
            return;

        currentCursor = response.output.next_cursor;

        tickProcessor.Apply(response.output);
    }

    //Actions
    public void StopRefresh()
    {
        running = false;
        Debug.Log("Stop Live Refreshing");
    }
}
