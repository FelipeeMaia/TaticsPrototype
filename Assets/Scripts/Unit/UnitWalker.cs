using Brisanti.Tactics.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brisanti.Tactics.Units
{
    public class UnitWalker : MonoBehaviour
    {
        [SerializeField] Transform _graffics;

        public Action OnStartMoving; 
        public Action OnStopMoving;

        //Recieve a new path
        public void SetNewPath(List<Tile> path)
        {
            OnStartMoving?.Invoke();
            StartCoroutine(MoveThroughPath(path));
        }

        //Move unit through each tile of the path
        private IEnumerator MoveThroughPath(List<Tile> path)
        {
            while (path.Count > 1)
            {
                Vector3 startPos = path[0].transform.position;
                Vector3 endPos = path[1].transform.position;
                float lerpTime = 0;

                StartCoroutine(_RotateTowards(endPos));

                while (lerpTime < 1)
                {
                    lerpTime += 0.05f;
                    transform.position = Vector3.Lerp(startPos, endPos, lerpTime);
                    yield return new WaitForFixedUpdate();
                }

                path.RemoveAt(0);
            }

            transform.position = path[0].transform.position;
            OnStopMoving?.Invoke();
        }

        //Recieve target rotation
        public void RotateTowards(Vector3 target)
        {
            StartCoroutine(_RotateTowards(target));
        }

        //Rotate unit towards direction it's moving to
        private IEnumerator _RotateTowards(Vector3 target)
        {
            Vector3 dist = target - transform.position;
            Quaternion startRotation = _graffics.rotation;
            Quaternion desiredRotation = Quaternion.LookRotation(dist);

            if (startRotation == desiredRotation) yield break;

            float lerpTime = 0;

            while (lerpTime < 1)
            {
                lerpTime += 0.1f;
                _graffics.rotation = Quaternion.Lerp(startRotation, desiredRotation, lerpTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}