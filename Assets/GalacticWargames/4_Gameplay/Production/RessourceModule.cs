using UnityEngine;

public class RessourceModule : MonoBehaviour
{
    [SerializeField] private int availableCarbon;
    public int _AvailableCarbon { get { return availableCarbon; } }

    [SerializeField] private int availableHydrogen;
    public int _AvailableHydrogen { get { return availableHydrogen; } }

    [SerializeField] private int availableStone;
    public int _AvailableStone { get { return availableStone; } }

    public void RefreshRessources(int[] r)
    {
        if (r.Length > 3 || r.Length < 3)
        {
            Debug.LogError("Uncomplete ressource package");
            return;
        }

        availableCarbon = r[0];
        availableHydrogen = r[1];
        availableStone = r[2];
    }

    public bool CanBuild(int[] costs)
    {
        if (costs[0] > availableCarbon)
            return false;
        else if (costs[1] > availableHydrogen)
            return false;
        else if (costs[2] > availableStone)
            return false;
        else
            return true;
    }
}
