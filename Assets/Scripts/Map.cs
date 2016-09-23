using UnityEngine;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public MapNode nodePrefab;

    private List<MapNode> m_Nodes;

    private bool m_IsVisible;

    void Update()
    {
        transform.LookAt(new Vector3(0, 0, 10000));
    }

    public void Initialize()
    {
        SetVisibility();

        m_Nodes[0].Select();
    }

    public void Generate(List<VisitNode> visitNodes)
    {
        m_Nodes = new List<MapNode>();

        foreach (VisitNode visitNode in visitNodes)
        {
            MapNode node = Instantiate(nodePrefab) as MapNode;
            node.Initialize(visitNode.Position, this.transform);
            m_Nodes.Add(node);
            visitNode.MapNode = node;

            foreach (VisitEdge visitEdge in visitNode.GetEdges())
            {
                createMapEdge(0.2f, 0, visitEdge, node);
                createMapEdge(0, 0.2f, visitEdge, node);
            }
        }

        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        transform.Translate(new Vector3(0, -1, 3));
    }

    private void createMapEdge(float width, float height, VisitEdge visitEdge, MapNode mapNode)
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
        var fromPos = visitEdge.FromNode.Position;
        var toPos = visitEdge.ToNode.Position;
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

    internal bool GetVisibility()
    {
        return m_IsVisible;
    }

    public bool ToggleVisibility()
    {
        m_IsVisible = !m_IsVisible;
        SetVisibility();
        return m_IsVisible;
    }

    public void SetVisibility()
    {
        gameObject.SetActive(m_IsVisible);
        foreach(MapNode mapNode in m_Nodes)
        {
            if(m_IsVisible)
            {
                mapNode.Show();
            } else
            {
                mapNode.Hide();
            }
        }
    }
}
