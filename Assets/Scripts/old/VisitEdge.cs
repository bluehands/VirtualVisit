using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VisitEdge : MonoBehaviour {

    public VisitNode fromNode;

    public VisitNode toNode;

    public void Initialize(VisitNode fromNode, VisitNode toNode, ButtonNext nextButton)
    {
        var canvasNext = GetComponentInChildren<Canvas>();
        var textNext = canvasNext.GetComponentInChildren<Text>();

        this.fromNode = fromNode;
        this.toNode = toNode;

        name = "Tour Edge from " + fromNode.id + " to " + toNode.id;
        transform.parent = fromNode.transform;
        transform.position = fromNode.transform.position;

        textNext.text = toNode.title;

        setRotation(fromNode, toNode);

        fromNode.addEdge(this);

        nextButton.transform.SetParent(canvasNext.transform, false);
    }

    private void setRotation(VisitNode fromNode, VisitNode toNode)
    {
        var dir = toNode.transform.position - fromNode.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
