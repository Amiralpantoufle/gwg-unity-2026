using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TickService
{
    private readonly API_Client _api;
    public TickService(API_Client api)
    {
        _api = api;
    }

    [SerializeField] private DateTimeOffset next_Cursor;
    [SerializeField] private DateTimeOffset server_Cursor;


    public async Task<TickResponse> GetTick(string cursor)
    {
        return await API_Client.Instance.GetTickAsync(cursor,null,null);
    }
}
