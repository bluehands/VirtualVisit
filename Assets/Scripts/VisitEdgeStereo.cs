using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VisitEdgeStereo : MonoBehaviour {

    public VisitNodeStereo fromNode;

    public VisitNodeStereo toNode;

    public void Initialize(VisitNodeStereo fromNode, VisitNodeStereo toNode, ButtonNextStereo nextButton)
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

    private void setRotation(VisitNodeStereo fromNode, VisitNodeStereo toNode)
    {
        var dir = toNode.transform.position - fromNode.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
