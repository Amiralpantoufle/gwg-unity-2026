using UnityEngine;

public class TickProcessor
{
    public void Apply(TickOutput tick)
    {
        if (tick == null)
            return;

        if (tick.changes.fleets.Count > 0)
            Debug.Log($"Tick : {tick.changes.fleets.Count} fleets");

        if (tick.changes.movements.Count > 0)
            Debug.Log($"Tick : {tick.changes.movements.Count} movements");

        if (tick.changes.resources.Count > 0)
            Debug.Log($"Tick : {tick.changes.resources.Count} resources");

        if (tick.changes.bases.Count > 0)
            Debug.Log($"Tick : {tick.changes.bases.Count} bases");
    }
}
