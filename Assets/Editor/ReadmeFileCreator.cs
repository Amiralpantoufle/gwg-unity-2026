using UnityEditor;
using UnityEngine;
using System.IO;

public static class ReadmeGenerator
{
    [MenuItem("Assets/Create/ReadMe File", false, 81)]
    private static void CreateReadMeFile()
    {
        var selectedObjects = Selection.objects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.LogWarning("No folder selected.");
            return;
        }

        foreach (var obj in selectedObjects)
        {
            string path = AssetDatabase.GetAssetPath(obj);

            if (!AssetDatabase.IsValidFolder(path))
            {
                Debug.LogWarning($"Skipped (not a folder): {path}");
                continue;
            }

            CreateReadmeInFolder(path);
        }

        AssetDatabase.Refresh();
    }

    private static void CreateReadmeInFolder(string folderPath)
    {
        string folderName = Path.GetFileName(folderPath);
        string fileName = $"{folderName}.md";
        string filePath = Path.Combine(folderPath, fileName);

        if (File.Exists(filePath))
        {
            Debug.LogWarning($"ReadMe already exists in: {folderPath}");
            return;
        }

        string content = GenerateTemplate(folderName);

        File.WriteAllText(filePath, content);
        Debug.Log($"ReadMe created: {filePath}");
    }

    private static string GenerateTemplate(string folderName)
    {
        return
$@"# {folderName}

TODO: Describe the purpose of this folder.

## Contains
- 

## Notes
- Generated on {System.DateTime.Now}
";
    }

    // 👇 Optionnel mais PROPRE : grise le menu si mauvaise sélection
    [MenuItem("Assets/Create/ReadMe File", true)]
    private static bool ValidateCreateReadMeFile()
    {
        foreach (var obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (AssetDatabase.IsValidFolder(path))
                return true;
        }
        return false;
    }
}