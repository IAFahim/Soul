using System.Collections.Generic;
using Alchemy.Inspector;
using LitMotion;
using QuickEye.Utility;
using Soul.Model.Runtime;
using Soul.Model.Runtime.Containers;
using Soul.Model.Runtime.Tweens;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable UnusedMember.Local

namespace Soul.Controller.Runtime.Movements
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