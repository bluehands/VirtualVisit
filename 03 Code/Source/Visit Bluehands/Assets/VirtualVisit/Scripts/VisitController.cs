using UnityEngine;
using System.Collections.Generic;
using System;

public class VisitController : MonoBehaviour, VisitPathListener, FollowingDisplayListener
{
    public VisitPoint pointPrefab;

    private VisitSetting m_VisitSettings;

    private List<VisitPoint> m_VisitPoints;

    private VisitPoint m_CurrerntVisitPoint;

    public void Initialize(VisitSetting visitSettings)
    {
        m_VisitSettings = visitSettings;

        m_VisitPoints = new List<VisitPoint>();
        foreach (var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            var node = createNode(m_VisitSettings, visitNodeSetting);
            m_VisitPoints.Add(node);
        }

        foreach (var visitNodeSetting in m_VisitSettings.nodeSettings)
        {
            var fromPoint = getPoint(visitNodeSetting.id);

            foreach (var edgeId in visitNodeSetting.getEdgeIds())
            {
                var toPoint = getPoint(edgeId);
                fromPoint.createEdge(toPoint, this);
            }
            if (visitNodeSetting.edgeSettings != null)
            {
                foreach (var edgeSetting in visitNodeSetting.edgeSettings)
                {
                    var toPoint = getPoint(edgeSetting.toId);
                    fromPoint.createEdge(toPoint, this, edgeSetting.u, edgeSetting.v);
                }
            }
            if (visitNodeSetting.markSettings != null)
            {
                foreach (var visitMarkSetting in visitNodeSetting.markSettings)
                {
                    fromPoint.createMark(visitMarkSetting.title, visitMarkSetting.description, visitMarkSetting.u, visitMarkSetting.v);
                }
            }
        }

        foreach (var visitPoint in m_VisitPoints)
        {
            visitPoint.Leave();
        }
        m_CurrerntVisitPoint = m_VisitPoints[0];
        m_VisitPoints[0].GoThere();
    }

    private VisitPoint createNode(VisitSetting visitSettings, VisitNodeSetting visitNodeSetting)
    {
        var node = Instantiate(pointPrefab) as VisitPoint;

        Texture textureLeft = null;
        Texture textureRight = null;

        bool isStereo = TexturesFactory.TryToLoadPointTextures(visitSettings.id, visitNodeSetting.id, out textureLeft, out textureRight);

        if (isStereo)
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.position, transform, textureLeft, textureRight);
        }
        else
        {
            node.Initialize(visitNodeSetting.id, visitNodeSetting.title, visitNodeSetting.position, transform, textureLeft);
        }
        return node;
    }

    private VisitPoint getPoint(string id)
    {
        for (int i = 0; i < m_VisitPoints.Count; i++)
        {
            if (m_VisitPoints[i].Id.Equals(id))
            {
                return m_VisitPoints[i];
            }
        }
        throw new InvalidOperationException(String.Format("Tour couldn't find node for {0} in visit {1}.", id, m_VisitSettings.id));
    }

    public void Go(VisitPath path)
    {
        var fromPoint = path.FromPoint;
        var toPoint = path.ToPoint;

        Debug.Log(String.Format("MoveTo({0},{1})", fromPoint.Id, toPoint.Id));

        fromPoint.Leave();
        toPoint.GoThereFrom(fromPoint);
        m_CurrerntVisitPoint = toPoint;
    }

    public void openDisplay()
    {
        m_CurrerntVisitPoint.FadeOut();
        foreach (var visitPoint in m_VisitPoints)
        {
            visitPoint.SetEdgesActive(false);
            visitPoint.SetMarksActive(false);
        }
    }

    public void closeDisplay()
    {
        m_CurrerntVisitPoint.FadeIn();
        foreach (var visitPoint in m_VisitPoints)
        {
            visitPoint.SetEdgesActive(true);
            visitPoint.SetMarksActive(true);
        }
    }
}
