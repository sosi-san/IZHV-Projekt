using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Woska.Bakalarka
{
    enum WheelType
    {
        Full,
        Part
    }
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private WheelType _wheelType;
        private float wheelSpeed;
        private Rigidbody2D _rigidbody2D;
    
        private float _wheelRadius;

        private Transform _wheel;
        private Transform _spikes;
    
        public Vector2 Velocity => _rigidbody2D.velocity;
        
        public int Direction { get; set; }


        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _wheel = transform.GetChild(0);
            _spikes = transform.GetChild(1);
        }

        private void Start()
        {
            if (_wheelType == WheelType.Full)
            {
                transform.localScale = 2 * Vector3.one;
                wheelSpeed = 30f;
            }
            else if (_wheelType == WheelType.Part)
            {
                transform.localScale = 5 * Vector3.one;
                wheelSpeed = 20f;
            }
            _wheelRadius = transform.localScale.x*0.5f;

            transform.position = Vector3.up * -4 + Vector3.up * transform.localScale.y*0.5f + transform.position.x*Vector3.right;
            Destroy(gameObject, 15f);
        }

        private void FixedUpdate()
        {
            var previousVelocity = Velocity;
            var velocityChange = Vector2.zero;
        
            velocityChange.x = (Direction * wheelSpeed - previousVelocity.x);

            //_rigidbody2D.velocity = _rigidbody2D.velocity + velocityChange;
            _rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);

            RotateWheel();
        }

        private void RotateWheel()
        {
            Vector3 movement = Velocity * Time.deltaTime;

            float distance = movement.magnitude;
            float angle = distance * (180f / Mathf.PI) / wheelSpeed;
            _wheel.localRotation = Quaternion.Euler(Vector3.forward * angle) * _wheel.localRotation;
            _spikes.localRotation = Quaternion.Euler(Vector3.forward * angle) * _spikes.localRotation;
        }
    }
}