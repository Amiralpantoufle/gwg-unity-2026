using System.IO;
using UnityEditor;
using UnityEngine;

public class VisualDefinitionGenerator : EditorWindow
{
    private DefaultAsset sourceFolder;

    private string manualAssetName = "NewVisualDefinition";

    private Sprite manualSprite;

    private VisualCategory manualCategory;

    [MenuItem("Tools/Visual Definition Generator")]
    public static void Open()
    {
        GetWindow<VisualDefinitionGenerator>("Visual Definitions");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.LabelField(
            "Batch Generation",
            EditorStyles.boldLabel);

        sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Source Folder",
            sourceFolder,
            typeof(DefaultAsset),
            false);

        if (GUILayout.Button("Generate VisualDefinitions"))
        {
            GenerateFolder();
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField(
            "Manual Creation",
            EditorStyles.boldLabel);

        manualAssetName = EditorGUILayout.TextField(
            "Asset Name",
            manualAssetName);

        manualCategory = (VisualCategory)EditorGUILayout.EnumPopup(
            "Category",
            manualCategory);

        manualSprite = (Sprite)EditorGUILayout.ObjectField(
            "Sprite",
            manualSprite,
            typeof(Sprite),
            false);

        if (GUILayout.Button("Create Single VisualDefinition"))
        {
            CreateManualAsset();
        }
    }

    private void GenerateFolder()
    {
        if (sourceFolder == null)
        {
            Debug.LogError("No folder selected.");
            return;
        }

        string folderPath =
            AssetDatabase.GetAssetPath(sourceFolder);

        string folderName =
            Path.GetFileName(folderPath);

        if (!System.Enum.TryParse(folderName,
            out VisualCategory category))
        {
            Debug.LogError(
                $"Folder name '{folderName}' does not match any VisualCategory.");
            return;
        }

        string outputFolder =
            $"Assets/Data/VisualDefinitions/{folderName}";

        if (!AssetDatabase.IsValidFolder(outputFolder))
        {
            CreateFolderRecursive(outputFolder);
        }

        string[] guids =
            AssetDatabase.FindAssets(
                "t:Sprite",
                new[] { folderPath });

        int createdCount = 0;

        foreach (string guid in guids)
        {
            string spritePath =
                AssetDatabase.GUIDToAssetPath(guid);

            Sprite sprite =
                AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            if (sprite == null)
                continue;

            string assetPath =
                $"{outputFolder}/{sprite.name}.asset";

            if (File.Exists(assetPath))
                continue;

            VisualDefinition vd =
                ScriptableObject.CreateInstance<VisualDefinition>();

            //Parse Name -> ID Du sprite doit être écrit en préfixe séparé de '_'
            string[] parts = sprite.name.Split('_');
            if (!int.TryParse(parts[0], out int imageId))
            {
                Debug.LogWarning(
                    $"Cannot extract ID from sprite '{sprite.name}'");
                continue;
            }
            vd.image_id = imageId;
             
            vd.category = category;

            vd.imageSprite = sprite;

            vd.renderScale = 1f;

            vd.offset = Vector2.zero;

            AssetDatabase.CreateAsset(vd, assetPath);

            createdCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            $"Created {createdCount} VisualDefinitions.");
    }

    private void CreateManualAsset()
    {
        string savePath =
            EditorUtility.SaveFilePanelInProject(
                "Save VisualDefinition",
                manualAssetName,
                "asset",
                "");

        if (string.IsNullOrEmpty(savePath))
            return;

        VisualDefinition vd =
            ScriptableObject.CreateInstance<VisualDefinition>();

        vd.category = manualCategory;

        vd.imageSprite = manualSprite;

        vd.renderScale = 1f;

        vd.offset = Vector2.zero;

        vd.image_id = 0;

        AssetDatabase.CreateAsset(vd, savePath);

        AssetDatabase.SaveAssets();

        Selection.activeObject = vd;
    }

    private static void CreateFolderRecursive(string folderPath)
    {
        string[] parts = folderPath.Split('/');

        string current = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";

            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }

            current = next;
        }
    }
}