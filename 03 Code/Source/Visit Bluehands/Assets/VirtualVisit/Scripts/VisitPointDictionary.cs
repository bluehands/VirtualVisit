using System;
using System.Collections.Generic;

namespace Assets.VirtualVisit.Scripts
{
    public class VisitPointDictionary
    {
        private readonly List<VisitPoint> _visitPoints;

        private readonly string _visitId;

        public VisitPointDictionary(string visitId)
        {
            _visitPoints = new List<VisitPoint>();
            _visitId = visitId;
        }

        public void Insert(VisitPoint visitPoint)
        {
            _visitPoints.Add(visitPoint);
        }

        public VisitPoint LockUp(string id)
        {
            foreach (var point in _visitPoints)
            {
                if (point.Id.Equals(id))
                {
                    return point;
                }
            }
            throw new InvalidOperationException(String.Format("Couldn't find point for {0} in visit {1}.", id, _visitId));
        }

        public List<VisitPoint> GetAll()
        {
            return _visitPoints;
        }

        public VisitPoint GetFirst()
        {
            return _visitPoints.Count != 0 ? _visitPoints[0] : null;
        }

    }
}
