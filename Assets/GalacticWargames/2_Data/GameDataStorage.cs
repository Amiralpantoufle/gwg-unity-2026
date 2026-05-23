using System.Collections.Generic;
using System.Linq;
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

    //Launch Data Process
    public void SetBaseIndexData(List<BaseIndexOutput> dataList)
    {
        //Protection
        if (dataList[0] == null || dataList.Count <= 0)
        {
            Debug.Log("No data found in list");
            return;
        }

        BaseIndexOutput firstData;

        //Si le joueur à plusieur bases
        if (dataList.Count > 1)
        {
            firstData = dataList.LastOrDefault();
        }
        else
        {
            firstData = dataList[0];
        }
        BaseDataLoader(firstData);
    }
    private void BaseDataLoader(BaseIndexOutput data)
    {
        if(data== null)
        {
            Debug.LogError("Empty Data received from BaseIndex");
            return;
        }

        CurrentBase = new PlayerBaseData
        {
            BaseId = data.id_oes,
            TileId = data.idesp_oes,
            LocationEntityId = data.idesp_oes,
            PlanetId = data.planet_id,
            SystemId = data.id_parent_esp,
            PlanetX = data.x_p_esp,
            PlanetY = data.y_p_esp
        };

        //si base chargée
        if (CurrentBase != null)
        {
           /* SaveLastBaseId(CurrentBase.BaseId);
            Debug.Log($"Base chargée :" + CurrentBase.BaseId);

            BootStrapLoader.Instance.TryLoadingPlanet();*/
        }
        else Debug.LogWarning("no base loaded");
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
