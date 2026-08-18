using System.Collections.Generic;
using UnityEngine;
public static class FleetUtility
{
    public static string ShipsToApiString(List<FleetShip> ships)
    {
        List<string> entries = new List<string>();

        foreach (FleetShip ship in ships)
        {
            entries.Add(
                $"{ship.ship_id}:{ship.count}:{ship.x}:{ship.y}"
            );
        }

        return string.Join(";", entries);
    }
}