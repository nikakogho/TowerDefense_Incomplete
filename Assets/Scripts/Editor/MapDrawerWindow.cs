using UnityEngine;
using UnityEditor;

public class MapDrawerWindow : EditorWindow {
    GameObject nodePrefab, pathPrefab;
    int width = 50;
    int height = 50;
    GameObject result = null;
    Transform nodes, path;

    Color nodeColor = Color.white, pathColor = Color.grey;
    Texture2D map;

    [MenuItem("Window/Map Drawer")]
    public static void ShowWindow()
    {
        GetWindow<MapDrawerWindow>("Map Drawer");
    }

    void OnGUI()
    {
        nodePrefab = (GameObject)EditorGUILayout.ObjectField("Node Prefab", nodePrefab, typeof(GameObject), false);
        pathPrefab = (GameObject)EditorGUILayout.ObjectField("Path Prefab", pathPrefab, typeof(GameObject), false);
        width = EditorGUILayout.IntField("Width", width);
        height = EditorGUILayout.IntField("Height", height);
        map = (Texture2D)EditorGUILayout.ObjectField("Map Sprite", map, typeof(Texture2D), false);
        nodeColor = EditorGUILayout.ColorField("Node Color", nodeColor);
        pathColor = EditorGUILayout.ColorField("Path Color", pathColor);

        if(nodePrefab != null && width > 0 && height > 0 && map != null && pathPrefab != null)
        {
            if (GUILayout.Button("Generate"))
            {
                if (result != null) DestroyOld();

                GenerateNew();

                //Camera.main.transform.position = new Vector3(width / 2, 100, height / 2) * 5;
                //Camera.main.transform.rotation = Quaternion.Euler(60, 0, 0);
            }
        }

        if(result != null)
        {
            if (GUILayout.Button("Destroy"))
            {
                DestroyOld();
            }
        }
    }

    void DestroyOld()
    {
         DestroyImmediate(result);
    }

    void GenerateNew()
    {
        result = new GameObject("Level");

        nodes = new GameObject("Nodes").transform;
        path = new GameObject("Path").transform;

        nodes.parent = path.parent = result.transform;

        nodePrefab.GetComponent<Node>().SetDefaultColor(nodeColor);
        pathPrefab.GetComponent<Renderer>().sharedMaterial.color = pathColor;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = map.GetPixel(x, y);

                if (ColorsAlmostMatch(color, nodeColor))
                    Instantiate(nodePrefab, new Vector3(x * 5, 0, y * 5), Quaternion.identity, nodes).name = "Node";
                else if (ColorsAlmostMatch(color, pathColor))
                    Instantiate(pathPrefab, new Vector3(x * 5, 0, y * 5), Quaternion.identity, path).name = "Path Node";
            }
        }
    }

    bool ColorsAlmostMatch(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.1f && Mathf.Abs(a.g - b.g) < 0.1f && Mathf.Abs(a.b - b.b) < 0.1f && Mathf.Abs(a.a - b.a) < 0.1f;
    }
}
