using UnityEngine;

public enum VisualCategory
{
    SystemTile,
    PlanetTile,
    Planet,
    PlanetStructure,
    Star,
    BaseBuilding,
    Resource,
    FogOfWar
}
/// <summary>
/// Parse Name -> nom Du sprite doit être écrit avec ID en préfixe séparé de '_'
/// </summary>
[CreateAssetMenu(menuName = "GW/Visual Definition")]
public class VisualDefinition : ScriptableObject
{
    public int image_id;

    public VisualCategory category;

    public Sprite imageSprite;

    public float renderScale = 1f;

    public Vector2 offset;
}

