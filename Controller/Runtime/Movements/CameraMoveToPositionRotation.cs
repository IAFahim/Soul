using System.Collections.Generic;
using _Root.Scripts.Model.Runtime;
using _Root.Scripts.Model.Runtime.Containers;
using _Root.Scripts.Model.Runtime.Tweens;
using Alchemy.Inspector;
using LitMotion;
using QuickEye.Utility;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable UnusedMember.Local

namespace _Root.Scripts.Controller.Runtime.Movements
{
    public class CameraMoveToPositionRotation : MonoBehaviour
    {
        public Transform transformToMove;
        public UnityDictionary<PlaceEnum, List<MoveWithRotateVariableTween>> locationDictionary;
        [FormerlySerializedAs("currentLocation")] public PlaceEnum currentPlace;
        public int currentLocationSubIndex;
        

        public MoveWithRotateVariableTween currentMoveWithRotateVariableTween =>
            locationDictionary[currentPlace][currentLocationSubIndex];

        [Button]
        public MotionHandle PlayMove() => currentMoveWithRotateVariableTween.Play(transformToMove);

        [Button]
        private void MoveToStart() => transformToMove.SetPositionAndRotation(
            currentMoveWithRotateVariableTween.start.Value, currentMoveWithRotateVariableTween.start.Value
        );

#if UNITY_EDITOR
        [Button]
        private void CaptureStart() => transformToMove.GetPositionAndRotation(
            out currentMoveWithRotateVariableTween.start.Value.position, out currentMoveWithRotateVariableTween.start.Value.rotation
        );

        private void Capture(PositionRotation positionRotation) =>
            UnityEditor.SceneView.lastActiveSceneView.camera.transform.GetPositionAndRotation(
                out positionRotation.position, out positionRotation.rotation
            );

        [Button]
        private void CaptureEnd() => Capture(currentMoveWithRotateVariableTween.end);
#endif

        private void OnDrawGizmosSelected()
        {
            if (transformToMove == null ||
                locationDictionary == null ||
                locationDictionary.Count == 0 ||
                !locationDictionary.ContainsKey(currentPlace) ||
                locationDictionary[currentPlace] == null ||
                locationDictionary[currentPlace].Count == 0 ||
                currentLocationSubIndex < 0
               ) return;

            Vector3 cubeSize = new Vector3(16, 9, 1);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(currentMoveWithRotateVariableTween.start.Value.position, cubeSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(currentMoveWithRotateVariableTween.end.position, cubeSize);
        }
    }
}