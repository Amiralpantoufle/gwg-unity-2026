using System.Collections.Generic;
using UnityEngine;
using static BaseIndexOutput;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.Audio.ProcessorInstance;

public class GameDataStorage : MonoBehaviour
{
    public static GameDataStorage Instance;
    public GlobalDataOutput GlobalData { get; private set; }
    public UserDataOutput UserData { get; private set; }

    //Base Data
    private const string LAST_BASE_KEY = "LAST_ACTIVE_BASE";
    /// <summary>
    /// Infos metier de BaseIndexOutput Response
    /// </summary>
    public PlayerBaseData CurrentBase { get; private set; }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGlobalData(GlobalDataOutput data)
    {
        GlobalData = data;
    }
    public void SetUserData(UserDataOutput data)
    {
        UserData = data;
    }
    public void SetBaseIndexData(List<BaseIndexOutput> data)
    {
            BaseIndexOutput firstData = data[0];
            if (GetLastBaseId() < 0 && data[0] != null && data.Count > 0)
            {
                // chargement premiere base
                CurrentBase = new PlayerBaseData
                {
                    BaseId = firstData.id_oes,
                    TileId = firstData.idesp_oes,
                    LocationEntityId = firstData.idesp_oes,
                    PlanetId = firstData.planet_id,
                    SystemId = firstData.id_parent_esp,
                    PlanetX = firstData.x_p_esp,
                    PlanetY = firstData.y_p_esp
                };

                SaveLastBaseId(CurrentBase.BaseId);
                Debug.Log($"Base chargée :" + CurrentBase.BaseId);
        }
        else
            {
                // TODO
            }
        
    }

    /// <summary>
    /// Sauvegarde localement la dernière base utilisée.
    /// </summary>
    public void SaveLastBaseId(int baseId)
    {
        PlayerPrefs.SetInt(LAST_BASE_KEY, baseId);
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Retourne l'identifiant de la dernière base active.
    /// </summary>
    public int GetLastBaseId()
    {
        return PlayerPrefs.GetInt(LAST_BASE_KEY, -1);
    }
}
