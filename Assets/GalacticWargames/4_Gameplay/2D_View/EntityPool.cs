using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityPool : MonoBehaviour
{
    [SerializeField] private EntityView entityPrefab;
    [SerializeField] private int initialSize = 100;

    private readonly Queue<EntityView> available = new();
    private readonly Dictionary<int, EntityView> activeEntities = new();


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

    public EntityView SpawnMapEntity(MapEntity data, Vector2 tilePosition)
    {
        EntityView view = Get(); 

        view.transform.position = tilePosition;
        view.Init(data);
        
        if(data.id == 0)
        {
            Debug.Log("No Id On entity found");
            return view;
        }
        activeEntities.Add(data.id, view);

        return view;
    }
    public EntityView SpawnBaseEntity(BaseEntity data, Vector2 tilePosition)
    {
        EntityView view = Get();

        view.transform.position = tilePosition;
        view.Init(data);

        Debug.Log("new Entity with ID : " + data.building_id);
        if(data.id != 0)
            activeEntities.Add(data.id, view);

        return view;
    }

    public void Delete_EntityFromID(int id)
    {
        if (activeEntities.TryGetValue(id, out EntityView entity))
        {
            activeEntities.Remove(id);
            Release(entity);
        }
        else
        {
            Debug.LogWarning($"Aucune Entity trouvée avec l'ID : {id}");
        }
    }
    public void ResetAllEntities()
    {
        foreach (EntityView entity in activeEntities.Values)
        {
            Release(entity);
        }

        activeEntities.Clear();
    }
}
