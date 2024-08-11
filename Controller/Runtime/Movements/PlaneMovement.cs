using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using Pancake;
using Soul.Model.Runtime.Tweens;
using UnityEngine;

namespace Soul.Controller.Runtime.Movements
{
    public class PlaneMovement : GameComponent
    {
        public MoveVariableTween liftMoveVariableTween;
        public RotateVariableTween rotateVariableTween;
        public MoveVariableTween travelMoveVariableTween;

        [Button]
        public async UniTaskVoid Play()
        {
            rotateVariableTween.Play(Transform);
            await liftMoveVariableTween.Play(Transform).GetAwaiter();
            travelMoveVariableTween.start = liftMoveVariableTween.end;
            await travelMoveVariableTween.Play(Transform).GetAwaiter();
            Debug.Log("Done");
        }
    }
}