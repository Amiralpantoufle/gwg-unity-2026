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

    //User Data
    private string username;
    private int level;
    private float experience;
    public string _Username { get { return username; } set { username = value; } }
    public int _Level { get { return level; } set { level = value; } }
    public float _Experience { get { return experience; } set { experience = value; } }

    //Base Data
    private const string LAST_BASE_KEY = "LAST_ACTIVE_BASE";
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

    /// <summary>
    /// Charge les données bases en local
    /// </summary>
    /// <param name="data"></param>
    public void LoadCurrentBaseData(BaseIndexOutput data)
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
        if (CurrentBase.BaseId != 0 && CurrentBase != null)
        {
           SaveLastBaseId(CurrentBase.BaseId);
           Debug.Log($"Base chargée :" + CurrentBase.BaseId);
        }
        else Debug.LogWarning("no base loaded");
    }
    
    //Data
    public void SetUserStartData(UserInfos user)
    {
        level = user.level;
        username = user.name;
        experience = user.experience;
    }
    /*public void SetGlobalData(GlobalDataOutput data)
    {
        GlobalData = data;
    }*/

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
