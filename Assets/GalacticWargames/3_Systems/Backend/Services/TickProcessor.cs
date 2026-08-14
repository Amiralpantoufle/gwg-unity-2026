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
        {
            int count = tick.changes.resources.Count;

            int[] r = new int[count];

            for (int i=0; i < count; i++)
                r[i] = tick.changes.resources[i].nombre_oer;

            GameDataStorage.Instance._Current_RessourceModule.RefreshRessources(r);
            Debug.Log($"Tick : {tick.changes.resources.Count} resources");
        }

        if (tick.changes.bases.Count > 0)
            Debug.Log($"Tick : {tick.changes.bases.Count} bases");
    }
}
