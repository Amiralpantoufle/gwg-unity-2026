using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class TickResponse
{
    public bool error;
    public string error_code;
    public string error_msg;

    public TickOutput output;
}

public class TickOutput
{
    public string server_time;

    public string next_cursor;

    public int recommended_poll_ms;

    public string cursor_received;

    public List<string> scopes;

    public TickChanges changes;

    public TickDeleted deleted;
}

public class TickChanges
{
    public List<FleetData> fleets;

    public List<MovementData> movements;

    public List<BaseData> bases;
    
    public List<ResourceData> resources;

    //public List<MovementData> allied_movements;

    //public List<FleetContactData> visible_fleet_contacts;

    //public ConstructionChanges constructions;

    //public List<CombatData> combats;

    //public List<NotificationData> notifications;

    //public MapDeltaData map;
}

public class TickDeleted
{
    public List<int> fleets;

    public List<int> movements;

    public List<int> notifications;
}

/*
[System.Serializable]
public class ConstructionChanges
{
    //public List<BuildingConstructionData> buildings;

    //public List<ShipConstructionData> ships;
}*/