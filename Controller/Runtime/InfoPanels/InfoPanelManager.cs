using System.Collections;
using System.Collections.Generic;
using Pancake.Pools;
using UnityEngine;

namespace Soul.Controller.Runtime.InfoPanels
{
    public class InfoPanelManager : MonoBehaviour
    {
        public LayerMask targetLayer;
        public Vector2 checkAreaSize;
        public Color gizmoColor = Color.green;
        private readonly RaycastHit[] _rayCastHits = new RaycastHit[20];

        private Transform _camTransform;

        public Vector3 lastPosition;
        public float movementThreshold = 0.1f;

        [Range(0, 2)] public float removeDelay = .3f;
        bool _workedStartedOnRemovingInfoPanels;
        private readonly Dictionary<Transform, IInfoPanel> _activeInfoPanels = new();

        private Coroutine _removeInfoPanelsCoroutine;

        void Start()
        {
            _camTransform = Camera.main.transform;
            lastPosition = _camTransform.position;
        }

        void Update()
        {
            if (Vector3.Distance(lastPosition, _camTransform.position) > movementThreshold)
            {
                var hitCount = BoxCast();
                SpanInfoPanels(hitCount);
                if (_removeInfoPanelsCoroutine != null)
                {
                    
                    if (_workedStartedOnRemovingInfoPanels)
                    {
                        StopCoroutine(_removeInfoPanelsCoroutine);
                        _workedStartedOnRemovingInfoPanels = false;
                    }
                }
            }
            else
            {
                if (!_workedStartedOnRemovingInfoPanels)
                {
                    _removeInfoPanelsCoroutine = StartCoroutine(RemoveInfoPanelsThatAreNotInList());
                }
            }

            lastPosition = _camTransform.position;
        }
        
        
        
        private IEnumerator RemoveInfoPanelsThatAreNotInList()
        {
            _workedStartedOnRemovingInfoPanels = true;
            yield return new WaitForSeconds(removeDelay);
            List<Transform> toRemove = new();
            foreach (var activeInfoPanel in _activeInfoPanels)
            {
                toRemove.Add(activeInfoPanel.Key);
            }

            foreach (var t in toRemove)
            {
                _activeInfoPanels[t].GameObject.Return();
                _activeInfoPanels.Remove(t);
            }

            _workedStartedOnRemovingInfoPanels = false;
        }

        private int BoxCast()
        {
            Vector3 cameraForward = _camTransform.forward;
            Vector3 halfSize = new Vector3(checkAreaSize.x / 2f, checkAreaSize.y / 2f, 0f);
            Vector3 checkAreaCenter = _camTransform.position + cameraForward * 2f;

            int hitCount = Physics.BoxCastNonAlloc(checkAreaCenter, halfSize, cameraForward, _rayCastHits,
                _camTransform.rotation, 100f, targetLayer);
            return hitCount;
        }

        private void SpanInfoPanels(int hitCount)
        {
            HashSet<Transform> newlyAddedTransforms = new();

            for (int i = 0; i < hitCount; i++)
            {
                var hitTransform = _rayCastHits[i].transform;

                if (_activeInfoPanels.ContainsKey(hitTransform))
                {
                    continue;
                }

                if (hitTransform.TryGetComponent<IInfoPanelReference>(out var infoPanel))
                {
                    var instantiated = infoPanel.InfoPanelPrefab.GameObject.Request<InfoPanel>(
                        infoPanel.InfoPanelWorldPosition, Quaternion.identity, hitTransform
                    );
                    bool hasInfo = instantiated.Setup(_camTransform, hitTransform);
                    if (hasInfo)
                    {
                        _activeInfoPanels.Add(hitTransform, instantiated);
                        newlyAddedTransforms.Add(hitTransform);
                    }
                }
            }
        }


        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireCube(Vector3.zero, checkAreaSize);
            Gizmos.DrawLine(Vector3.zero, Vector3.right * checkAreaSize.x / 2f);
            Gizmos.DrawLine(Vector3.zero, Vector3.left * checkAreaSize.x / 2f);
            Gizmos.DrawLine(Vector3.zero, Vector3.up * checkAreaSize.y / 2f);
            Gizmos.DrawLine(Vector3.zero, Vector3.down * checkAreaSize.y / 2f);
        }
    }
}