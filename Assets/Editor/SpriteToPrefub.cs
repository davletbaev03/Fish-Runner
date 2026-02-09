using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteToPrefab : MonoBehaviour
{
    [MenuItem("Tools/Convert Sprites To Prefabs")]
    static void Convert()
    {

        string spritesPath = "Assets/Assets/Art/Препятствия"; // путь к папке со спрайтами
        string prefabsPath = "Assets/Prefubs/Obstacles"; // куда сохранять префабы
        string configPathTrash = "Assets/Configs/TrashConfig.asset";
        string configPathNet = "Assets/Configs/NetConfig.asset";

        if (!Directory.Exists(prefabsPath))
            Directory.CreateDirectory(prefabsPath);

 
        PlayerInteractionConfig configTrash = AssetDatabase.LoadAssetAtPath<PlayerInteractionConfig>(configPathTrash);
        PlayerInteractionConfig configNet = AssetDatabase.LoadAssetAtPath<PlayerInteractionConfig>(configPathNet);
        if (configTrash == null)
        {
            Debug.LogError("Не найден конфиг по пути: " + configPathTrash);
            return;
        }
        if (configNet == null)
        {
            Debug.LogError("Не найден конфиг по пути: " + configPathNet);
            return;
        }

        var guids = AssetDatabase.FindAssets("t:Sprite", new[] { spritesPath });
        
        foreach (var guid in guids)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            GameObject go = new GameObject(sprite.name);
            var inter = go.AddComponent<PlayerInteractionHandler>();

            if (sprite.name.StartsWith("net"))
                inter.config = configNet;
            else
                inter.config = configTrash;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size /= 2f;

            go.transform.localScale = new Vector3(0.2f, 0.2f, 0f);

            string prefabPath = Path.Combine(prefabsPath, sprite.name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        AssetDatabase.Refresh();
        Debug.Log("Все спрайты сконвертированы в префабы!");
    }
}