using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FleetListResponse
{
    public string contract_version;
    public Fleet[] fleets;
    //public FleetQuota quota;
    public int active_count;
}

[Serializable]
public class Fleet
{
    public int fleet_id;
    public int id_flt;

    public string name;

    public bool is_garrison;

    public int status;
    public bool active;
    public bool is_actionable;
    public bool is_destroyed;

    public bool can_move;
    public bool can_edit;
    public bool can_delete;
    public bool can_rebuild;

    public int posture;

    public int esp_id;
    public int? garrison_base_id;

    public string formation_code;
    /*
    public FleetShip[] composition;

    public FleetStatistics statistics;

    public FleetCargo cargo;

    public FleetLoadout loadout;

    public FleetState state;*/
}

[Serializable]
public class FleetShip
{
    public int x;
    public int y;

    public string name;
    public int count;
    public int ship_id;

    public int? skin_id;
}

[Serializable]
public class FleetStatistics
{
    public int ship_count;
    public int points;
    public int points_maximum;
    public int speed;
    public int power;
}

[Serializable]
public class FleetCargo
{
    public Dictionary<int, int> materials;

    public int used;
    public int capacity;
    public int capacity_base;
    public int over_capacity;
}

[Serializable]
public class FleetLoadout
{
    public int commander_count;
    public int equipment_count;
    public int tactic_count;
    public int tech_option_count;
    public int active_consumable_effect_count;

    public int loadout_revision;
}

[Serializable]
public class FleetState
{
    public bool moving;
    public bool in_combat;
    public bool actionable;

    public string[] blocking_reasons;
}

[Serializable]
public class FleetQuota
{
    public int level;
    public int used;
    public int maximum;
    public int available;

    public int next_level;
    public int next_maximum;

    public bool garrisons_counted;

    public FleetQuotaCurve[] contract_curve;
}
[Serializable]
public class FleetQuotaCurve
{
    public string levels;
    public int maximum;
}