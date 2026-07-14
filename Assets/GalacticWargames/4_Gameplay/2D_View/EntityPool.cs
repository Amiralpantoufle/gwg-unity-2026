using System.Collections.Generic;
using UnityEngine;

public class EntityPool : MonoBehaviour
{
    [SerializeField] private EntityView entityPrefab;
    [SerializeField] private int initialSize = 100;

    private readonly Queue<EntityView> available = new();


    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewEntity();
        }
    }

    private void CreateNewEntity()
    {
        EntityView entity = Instantiate(entityPrefab, transform);
        entity.gameObject.SetActive(false);

        available.Enqueue(entity);
    }
    public EntityView Get()
    {
        if (available.Count == 0)
        {
            CreateNewEntity();
        }

        EntityView entity = available.Dequeue();
        entity.gameObject.SetActive(true);

        return entity;
    }
    public void Release(EntityView entity)
    {
        entity.gameObject.SetActive(false);
        available.Enqueue(entity);
    } 

    public EntityView Spawn(EntityDto data, Vector2 tilePosition)
    {
        EntityView view = Get();

        view.transform.localPosition = tilePosition;
        view.Init(data);

        return view;
    }
}
