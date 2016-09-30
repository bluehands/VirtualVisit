using UnityEngine;
using UnityEngine.UI;
using System;

public class VisitEdge : MonoBehaviour, ButtonListener {

    public ButtonStep buttonStepPrefab;

    public VisitNode FromNode { get; private set; }

    public VisitNode ToNode { get; private set; }

    private ButtonStep m_ButtonStep;

    private Visit m_Visit;

    public void Initialize(Visit visit, VisitNode fromNode, VisitNode toNode)
    {
        m_Visit = visit;
        var canvasNext = GetComponentInChildren<Canvas>();
        var textNext = canvasNext.GetComponentInChildren<Text>();

        FromNode = fromNode;
        ToNode = toNode;

        name = string.Format("VisitEdge({0},{1})", fromNode.Id, toNode.Id);
        transform.parent = fromNode.transform;

        textNext.text = toNode.Title;

        setRotation(fromNode.Position, toNode.Position);

        fromNode.AddEdge(this);

        m_ButtonStep = Instantiate(buttonStepPrefab) as ButtonStep;
        m_ButtonStep.Initialize(canvasNext.transform, this);
    }

    private void setRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
    {
        var dir = toNodePosition - fromNodePosition;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void DoButtonAction(Type clazz)
    {
        if (clazz.Equals(typeof(ButtonStep)))
        {
            m_Visit.MoveTo(this);
        }
    }
}
