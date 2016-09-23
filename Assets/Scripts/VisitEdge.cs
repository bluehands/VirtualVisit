using UnityEngine;
using UnityEngine.UI;

public class VisitEdge : MonoBehaviour {

    public VisitNode FromNode { get; private set; }

    public VisitNode ToNode { get; private set; }

    public void Initialize(VisitNode fromNode, VisitNode toNode, ButtonStep btnStep)
    {
        var canvasNext = GetComponentInChildren<Canvas>();
        var textNext = canvasNext.GetComponentInChildren<Text>();

        FromNode = fromNode;
        ToNode = toNode;

        name = string.Format("VisitEdge({0},{1})", fromNode.Id, toNode.Id);
        transform.parent = fromNode.transform;

        textNext.text = toNode.Title;

        setRotation(fromNode.Position, toNode.Position);

        fromNode.AddEdge(this);

        btnStep.transform.SetParent(canvasNext.transform, false);
    }

    private void setRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
    {
        var dir = toNodePosition - fromNodePosition;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
