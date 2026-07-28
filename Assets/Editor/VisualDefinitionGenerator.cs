using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class VisualDefinitionGenerator : EditorWindow
{
    private DefaultAsset sourceFolder;
    private Texture2D atlasTexture;
    private int startImageId = 0;

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

        EditorGUILayout.LabelField("Batch Generation",EditorStyles.boldLabel);

        sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField("Source Folder",sourceFolder,typeof(DefaultAsset),false);

        atlasTexture = (Texture2D)EditorGUILayout.ObjectField("Atlas Texture",atlasTexture,typeof(Texture2D),false);

        startImageId = EditorGUILayout.IntField("Start Image ID",startImageId);

        if (GUILayout.Button("Generate VisualDefinitions"))
        {
            if (sourceFolder != null)
            {
                GenerateFolder();
            }
            else if (atlasTexture != null)
            {
                GenerateAtlas();
            }
            else
            {
                Debug.LogError("Select either a Source Folder or an Atlas Texture.");
            }
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

        if (!System.Enum.TryParse(folderName, out VisualCategory category))
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

        string[] guids = AssetDatabase.FindAssets(
            "t:Sprite",
            new[] { folderPath });


        int currentId = startImageId;
        int createdCount = 0;


        foreach (string guid in guids)
        {
            string spritePath =
                AssetDatabase.GUIDToAssetPath(guid);

            Sprite sprite =
                AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);


            if (CreateVisualDefinition(
                sprite,
                category,
                outputFolder,
                currentId))
            {
                createdCount++;
                currentId++;
            }
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created {createdCount} VisualDefinitions.");
    }
    private void GenerateAtlas()
    {
        if (atlasTexture == null)
        {
            Debug.LogError("No atlas selected.");
            return;
        }

        string atlasPath =
            AssetDatabase.GetAssetPath(atlasTexture);


        string folderPath =
            Path.GetDirectoryName(atlasPath)
            .Replace("\\", "/");


        string folderName =
            Path.GetFileName(folderPath);


        if (!System.Enum.TryParse(folderName, out VisualCategory category))
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

        Object[] assets =
            AssetDatabase.LoadAllAssetsAtPath(atlasPath);


        List<Sprite> sprites =
            assets
            .OfType<Sprite>()
            .ToList();

        int currentId = startImageId;
        int createdCount = 0;


        foreach (Sprite sprite in sprites)
        {
            if (CreateVisualDefinition(
                sprite,
                category,
                outputFolder,
                currentId))
            {
                createdCount++;
                currentId++;
            }
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        Debug.Log(
            $"Created {createdCount} VisualDefinitions from atlas.");
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

    private bool CreateVisualDefinition(
      Sprite sprite,
      VisualCategory category,
      string outputFolder,
      int imageId)
    {
        if (sprite == null)
            return false;


        string assetPath =
            $"{outputFolder}/{sprite.name}.asset";


        if (File.Exists(assetPath))
            return false;


        VisualDefinition vd =
            ScriptableObject.CreateInstance<VisualDefinition>();


        vd.image_id = imageId;
        vd.category = category;
        vd.imageSprite = sprite;
        vd.renderScale = 1f;
        vd.offset = Vector2.zero;


        AssetDatabase.CreateAsset(
            vd,
            assetPath);


        return true;
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