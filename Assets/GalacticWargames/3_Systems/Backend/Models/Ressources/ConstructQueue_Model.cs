using NUnit.Framework;
using System;
using UnityEngine;

public class ConstructQueue_Model
{
    public ConstructQueue_Building[] buildings;
    public ConstructQueue_SpaceShip[] ships;
}
[Serializable]
public class ConstructQueue_Building
{
    public int base_id;
    public int id;

    public int building_id;
    public int x;
    public int y;

    public int started_at;
    public int end_at;
    /*       
        "same_slot_active_count": 1,
        "progress_ratio": 1,
        "is_overdue": true,
        "eta_delay_seconds": 300,
        "queue_state_label": "overdue",
        "base_lost": false,
        "diagnostics": [
          {
            "code": "overdue_active",
            "severity": "critical",
            "label": "ETA depassee"
          }
        ],
        "diagnostic_status": "critical",
        "has_attention": true*/
}

[Serializable]
public class ConstructQueue_SpaceShip
{
    public int id;
    public int ship_id;
    public int quantity;
    public int remaining;
    public int remaining_batch_seconds;
}