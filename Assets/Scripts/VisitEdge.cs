using UnityEngine;
using UnityEngine.UI;
using System;

public class VisitEdge : MonoBehaviour, ButtonListener {

    public ButtonStep buttonStepPrefab;

    public VisitNode FromNode { get; private set; }

    public VisitNode ToNode { get; private set; }

    private ButtonStep m_ButtonStep;

    private VisitEdgeListener m_VisitEdgeListener;

    public void Initialize(VisitNode fromNode, VisitNode toNode, VisitEdgeListener visitEdgeListener)
    {
        init(fromNode, toNode, visitEdgeListener);

        setRotation(fromNode.Position, toNode.Position);
    }

    public void Initialize(VisitNode fromNode, VisitNode toNode, VisitEdgeListener visitEdgeListener, float u, float v)
    {
        init(fromNode, toNode, visitEdgeListener);

        if((0 <= u && u <= 1) && (0 <= v && v <= 1))
        {
            rotateToMarke(u, v);
        } else
        {
            setRotation(fromNode.Position, toNode.Position);
        }
    }

    private void init(VisitNode fromNode, VisitNode toNode, VisitEdgeListener visitEdgeListener)
    {
        m_VisitEdgeListener = visitEdgeListener;
        var canvasNext = GetComponentInChildren<Canvas>();
        var textNext = canvasNext.GetComponentInChildren<Text>();

        FromNode = fromNode;
        ToNode = toNode;

        name = string.Format("VisitEdge({0},{1})", fromNode.Id, toNode.Id);
        transform.parent = fromNode.transform;

        textNext.text = toNode.Title;

        m_ButtonStep = Instantiate(buttonStepPrefab) as ButtonStep;
        m_ButtonStep.Initialize(canvasNext.transform, this);
    }

    private void setRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
    {
        var dir = toNodePosition - fromNodePosition;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void rotateToMarke(float u, float v)
    {
        float xRot = 180 * v - 90;
        float yRot = 360 * u + 184;
        Quaternion rotation = Quaternion.Euler(new Vector3(xRot, yRot, 0));
        transform.rotation = rotation;
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonStep)))
        {
            m_VisitEdgeListener.MoveTo(this);
        }
    }
}
