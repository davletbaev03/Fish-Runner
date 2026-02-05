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

        if (!Directory.Exists(prefabsPath))
            Directory.CreateDirectory(prefabsPath);

        var guids = AssetDatabase.FindAssets("t:Sprite", new[] { spritesPath });

        foreach (var guid in guids)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            GameObject go = new GameObject(sprite.name);
            go.tag = "Coral";
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size /= 2f;

            go.transform.localScale = new Vector3(0.3f, 0.3f, 0f);

            string prefabPath = Path.Combine(prefabsPath, sprite.name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        AssetDatabase.Refresh();
        Debug.Log("Все спрайты сконвертированы в префабы!");
    }
}