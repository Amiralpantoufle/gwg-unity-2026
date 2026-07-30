using UnityEngine;

public class GameDataStorage : MonoBehaviour
{
    public static GameDataStorage Instance;
    public GlobalDataOutput _GlobalData { get; private set; }
    public UserDataOutput _UserData { get; private set; }

    //Base Data
    private const string LAST_BASE_KEY = "LAST_ACTIVE_BASE";
    private BaseOutput currentBase;
    public BaseOutput _CurrentBase { get { return currentBase; }}

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
    public void LoadCurrentBaseData(BaseOutput data)
    {
        if(data == null)
        {
            Debug.LogWarning("Empty Data received from BaseIndex");
            return;
        }

        currentBase = data;

        //si base chargée
        if (currentBase.base_id != 0)
        {
           SaveLastBaseId(currentBase.base_id);
           Debug.Log($"Base chargée :" + currentBase.base_id);
        }
        else Debug.LogWarning("no base loaded");
    }
    
    //Data
    public void SetUserStartData(UserDataOutput user)
    {
        _UserData = user;
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
        Debug.Log(PlayerPrefs.GetInt(LAST_BASE_KEY, 0));
        return PlayerPrefs.GetInt(LAST_BASE_KEY, 0);
    }
}
