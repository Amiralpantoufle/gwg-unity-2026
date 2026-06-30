using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileActions_Model
{
    public List<TileAction_Model> actions;
}

[System.Serializable]
public class TileAction_Model
{
    public string label;
    public int priority;
}
