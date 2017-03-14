using System;
using UnityEngine;

namespace Assets.VirtualVisit.Scripts
{
    public class VisitController : MonoBehaviour, VisitPathListener, FollowingDisplayListener
    {
        public VisitPoint PointPrefab;

        private VisitSetting _visitSettings;

        private VisitPointDictionary _visitPointDictionary;

        private VisitPoint _currerntVisitPoint;

        public void Initialize(VisitSetting visitSettings)
        {
            _visitSettings = visitSettings;

            _visitPointDictionary = new VisitPointDictionary(visitSettings.id);

            foreach (var visitNodeSetting in _visitSettings.nodeSettings)
            {
                var point = CreatePoint(_visitSettings, visitNodeSetting);
                _visitPointDictionary.Insert(point);
            }

            foreach (var visitNodeSetting in _visitSettings.nodeSettings)
            {
                var point = _visitPointDictionary.LockUp(visitNodeSetting.id);
                point.Generate(visitNodeSetting, _visitPointDictionary, this);
            }

            _currerntVisitPoint = _visitPointDictionary.GetFirst();
            if (_currerntVisitPoint != null)
            {
                _currerntVisitPoint.GoThere();
            }
        }

        private VisitPoint CreatePoint(VisitSetting visitSettings, VisitNodeSetting visitNodeSetting)
        {
            var node = Instantiate(PointPrefab);
            node.Initialize(visitSettings, visitNodeSetting, transform);
            return node;
        }

        public void Go(VisitPath path)
        {
            var fromPoint = path.FromPoint;
            var toPoint = path.ToPoint;

            Debug.Log(String.Format("MoveTo({0},{1})", fromPoint.Id, toPoint.Id));

            fromPoint.Leave();
            toPoint.GoThereFrom(fromPoint);
            _currerntVisitPoint = toPoint;
        }

        public void openDisplay()
        {
            _currerntVisitPoint.FadeOut();
            foreach (var visitPoint in _visitPointDictionary.GetAll())
            {
                visitPoint.SetEdgesActive(false);
                visitPoint.SetMarksActive(false);
            }
        }

        public void closeDisplay()
        {
            _currerntVisitPoint.FadeIn();
            foreach (var visitPoint in _visitPointDictionary.GetAll())
            {
                visitPoint.SetEdgesActive(true);
                visitPoint.SetMarksActive(true);
            }
        }
    }
}
