using Pancake;
using Soul.Model.Runtime.Interfaces;
using Soul.Model.Runtime.Movements;
using UnityEngine;

namespace Soul.Controller.Runtime.Movements
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class MovementModel : GameComponent, ISpeedReference, IDebugAble
    {
        public Rigidbody rb;
        public float force = 2;
        public Vector3 direction = Vector3.left;
        public Vector3 addedForce;
        
        [SerializeField] private bool debugEnabled;
        public bool DebugEnabled
        {
            get => debugEnabled;
            set => debugEnabled = value;
        }

        protected void Move()
        {
            Vector3 newMovement = rb.position + (direction * force + addedForce) * Time.fixedDeltaTime;
            rb.MovePosition(newMovement);
        }


        private void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        public float Speed
        {
            get => force;
            set => force = value;
        }
    }
}