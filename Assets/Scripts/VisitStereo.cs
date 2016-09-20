using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class VisitStereo : MonoBehaviour {

    public VisitNodeStereo nodePrefab;

    public VisitEdgeStereo edgePrefab;

    public MapStereo mapPrefab;

    public ButtonNextStereo nextButtonPrefab;

    private VisitSettingsStereo visitSettings;

    private List<VisitNodeStereo> nodes = new List<VisitNodeStereo>();

    public MapStereo map;

    public List<VisitNodeStereo> getNodes()
    {
        return nodes;
    }

    public void Generate(string visitId)
    {
        this.visitSettings = getVisitSettings(visitId); ;

        VisitNodeSettingsStereo[] nodeSettings = visitSettings.nodeSettings;
        for (int i = 0; i < nodeSettings.Length; i++)
        {
            VisitNodeSettingsStereo settings = nodeSettings[i];
            Texture textureLeft = null;
            Texture textureRight = null;

            bool isStereo = tryToLoadTextures(visitSettings.id, settings.id, out textureLeft, out textureRight);

            createNode(settings.id, settings.title, settings.postion, textureLeft, textureRight, isStereo);
        }
        for (int i = 0; i < nodeSettings.Length; i++)
        {
            VisitNodeSettingsStereo settings = nodeSettings[i];
            string[] edgeIds = getEdgeIds(settings.edgeIds);
            for (int k = 0; k < edgeIds.Length; k++)
            {
                createEdge(settings.id, edgeIds[k]);
            }
        }

        map = Instantiate(mapPrefab) as MapStereo;

        map.Generate(this);
        map.Initialize();
    }

    private VisitSettingsStereo getVisitSettings(string visitId)
    {
        if (visitId == null || visitId.Equals(""))
        {
            visitId = "Epple";
        }
        VisitSettingsStereo visitSettings = null;

        var jsonText = Resources.Load(String.Format("Visits\\{0}", visitId)) as TextAsset;

        visitSettings = JsonUtility.FromJson<VisitSettingsStereo>(jsonText.text);

        return visitSettings;
    }

    private bool tryToLoadTextures(string visitId, string nodeId, out Texture textureLeft, out Texture textureRight)
    {
        bool isStereo;
        textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, nodeId, "l")) as Texture;
        textureRight = Resources.Load(String.Format("Panoramas\\{0}_{1}_{2}", visitId, nodeId, "r")) as Texture;
        if (textureLeft != null && textureRight != null)
        {
            isStereo = true;
        } else
        {
            textureLeft = Resources.Load(String.Format("Panoramas\\{0}_{1}", visitId, nodeId)) as Texture;
            isStereo = false;
        }
        return isStereo;
    }

    private string[] getEdgeIds(string edgeIdsStr)
    {
        return edgeIdsStr.Split(',');
    }

    public void createNode(string id, string title, Vector3 position, Texture sphereLeft, Texture sphereRight, bool isStereo)
    {
        VisitNodeStereo node = Instantiate(nodePrefab) as VisitNodeStereo;
        nodes.Add(node);
        if(isStereo)
        {
            node.Initialize(id, title, position, sphereLeft, sphereRight);
        } else
        {
            node.Initialize(id, title, position, sphereLeft);
        }
        node.transform.parent = this.transform;
    }

    public void createEdge(string fromId, string toId)
    {
        VisitEdgeStereo edge = Instantiate(edgePrefab) as VisitEdgeStereo;
        VisitNodeStereo fromNode = getNode(fromId);
        VisitNodeStereo toNode = getNode(toId);

        ButtonNextStereo nextButton = Instantiate(nextButtonPrefab) as ButtonNextStereo;

        edge.Initialize(fromNode, toNode, nextButton);
    }

    private VisitNodeStereo getNode(string id)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if(nodes[i].id.Equals(id))
            {
                return nodes[i];
            }
        }
        throw new InvalidOperationException(String.Format("Tour couldn't find node for {0} in visit {1}." , id, visitSettings.id));
    }

    public void moveTo(VisitEdgeStereo edge)
    {
        VisitNodeStereo fromNode = edge.fromNode;
        VisitNodeStereo toNode = edge.toNode;
        var position = fromNode.transform.position - toNode.transform.position;

        Debug.Log("move to " + position);

        transform.Translate(position);
        fromNode.unselect();
        toNode.select();
    }

}
