using System;
using UnityEngine;

public class FleetCreateRequest
{
    public int id_oes;
    public string name_flotte;
    public string list_idvas_nb_coords;
    public int mode_flt;

    // Optionnel selon la documentation
    public bool is_garrison;
}
/*FleetCreateRequest request = new FleetCreateRequest
{
    id_oes = GameDataStorage.Instance.GetLastBaseId(),
    name_flotte = "Flotte Alpha",
    list_idvas_nb_coords = "8:2:0:0;4:3:1:0",
    mode_flt = 1,
    is_garrison = false
};

string json = JsonUtility.ToJson(request);*/


[Serializable]
public class FleetEditRequest
{
    public int id_flt;
    public string name_flotte;
    public int mode_flt;
}
/*FleetEditRequest request = new FleetEditRequest
{
    id_flt = 7,
    name_flotte = "Flotte Alpha",
    mode_flt = 1
};

string json = JsonUtility.ToJson(request);*/