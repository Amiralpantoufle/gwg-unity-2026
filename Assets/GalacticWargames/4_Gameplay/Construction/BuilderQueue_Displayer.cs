using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuilderQueue_Displayer : MonoBehaviour
{
    public static BuilderQueue_Displayer Instance;

    [SerializeField] private VignetteView entityPrefab;
    [SerializeField] private int initialSize = 5;

    private readonly Queue<VignetteView> available = new();


    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < initialSize; i++)
        {
            CreateNewEntity();
        }
    }


    //Pooling
    private void CreateNewEntity()
    {
        VignetteView v = Instantiate(entityPrefab, transform);
        available.Enqueue(v);
        v.gameObject.SetActive(false);
    }
    public VignetteView Get()
    {
        if (available.Count == 0)
        {
            CreateNewEntity();
        }

        VignetteView v = available.Dequeue();
        v.gameObject.SetActive(true);

        return v;
    }
    public void Release(VignetteView vignette)
    {
        vignette.gameObject.SetActive(false);
        available.Enqueue(vignette);
    }

    public VignetteView SpawnVignette(int id, Vector2 tilePosition)
    {
        VignetteView view = Get();

        view.transform.position = tilePosition;
        view.Init(id);

        return view;
    }
}
