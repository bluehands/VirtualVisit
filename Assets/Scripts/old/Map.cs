using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public MapNode nodePrefab;

    private List<MapNode> nodes = new List<MapNode>();

    private bool mapVisibility;

    void Update()
    {
        transform.LookAt(new Vector3(0, 0, 10000));
    }

    public void Initialize()
    {
        SetVisibility();

        nodes[0].select();
    }

    public void Generate(Visit visit)
    {
        foreach (VisitNode visitNode in visit.getNodes())
        {
            MapNode node = Instantiate(nodePrefab) as MapNode;
            nodes.Add(node);
            node.Initialize(visitNode, this);

            foreach (VisitEdge visitEdge in visitNode.getEdges())
            {
                createGameObject(0.2f, 0, visitEdge, node);
                createGameObject(0, 0.2f, visitEdge, node);
            }
        }

        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.Translate(new Vector3(0, -1, 3));
    }

    private void createGameObject(float width, float height, VisitEdge visitEdge, MapNode mapNode)
    {
        GameObject plane = new GameObject("MapEdge");
        plane.transform.parent = mapNode.transform;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(width, height, visitEdge);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material.shader = Shader.Find("Particles/Additive");
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.green);
        tex.Apply();
        renderer.material.mainTexture = tex;
        renderer.material.color = Color.green;
    }

    private Mesh CreateMesh(float width, float height, VisitEdge visitEdge)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        var fromPos = visitEdge.fromNode.transform.position;
        var toPos = visitEdge.toNode.transform.position;
        m.vertices = new Vector3[] {
             new Vector3(fromPos.x + width, fromPos.y + height, fromPos.z + width),
             new Vector3(fromPos.x - width, fromPos.y - height, fromPos.z - width),
             new Vector3(toPos.x + width, toPos.y - height, toPos.z + width),
             new Vector3(toPos.x - width, toPos.y + height, toPos.z - width)
        };
        m.uv = new Vector2[] {
             new Vector2 (0, 0),
             new Vector2 (0, 1),
             new Vector2(1, 1),
             new Vector2 (1, 0)
        };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }

    public void ToggleVisibility()
    {
        mapVisibility = !mapVisibility;
        SetVisibility();
    }

    public void SetVisibility()
    {
        gameObject.SetActive(mapVisibility);
        foreach(MapNode mapNode in nodes)
        {
            if(mapVisibility)
            {
                mapNode.Show();
            } else
            {
                mapNode.Hide();
            }
        }
    }
}
