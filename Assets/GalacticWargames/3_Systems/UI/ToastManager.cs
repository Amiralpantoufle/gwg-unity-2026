using System.Collections.Generic;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    public Transform container;

    [Tooltip("récupère le type de message : 0 = Warning | 1 = Error")]
    public GameObject[] toastPrefab;

    private List<GameObject> activeToasts = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<ShowToastEvent>(OnShowToast);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ShowToastEvent>(OnShowToast);
    }
    public void GenerateToast(string response, int type, float time)
    {
        EventBus.Publish(new ShowToastEvent
        {
            toastType = type,
            message = response,
            duration = time
        });

        //Play Audio Feedback
        if(type==0)
            AudioManager.Instance.PlaySound("Toast_Error");
        else
            AudioManager.Instance.PlaySound("Toast_Warning");

    }
    private void OnShowToast(ShowToastEvent e)
    {

        var toastGO = Instantiate(toastPrefab[e.toastType], container);

        var toast = toastGO.GetComponent<ToastUI>();
        toast.Init(e.message, e.duration, this);

        activeToasts.Add(toastGO);
    }

    public void RemoveToast(GameObject toast)
    {
        activeToasts.Remove(toast);
        Destroy(toast);
    }
}