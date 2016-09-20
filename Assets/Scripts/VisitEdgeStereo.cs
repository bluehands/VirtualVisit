using UnityEngine;
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

        name = string.Format("VisitEdge({0},{1})", fromNode.id, toNode.id);
        transform.parent = fromNode.transform;

        textNext.text = toNode.title;

        setRotation(fromNode.position, toNode.position);

        fromNode.addEdge(this);

        nextButton.transform.SetParent(canvasNext.transform, false);
    }

    private void setRotation(Vector3 fromNodePosition, Vector3 toNodePosition)
    {
        var dir = toNodePosition - fromNodePosition;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
