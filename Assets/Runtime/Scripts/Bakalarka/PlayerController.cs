using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Woska.Core;

namespace Woska.Bakalarka
{
    public enum PlayerState
    {
        Movement,
        Stunded
    }
    public class PlayerController : MonoBehaviour
    {
        [Header("Walking")] 
        [SerializeField, Range(0f, 100f)] private float moveSpeed = 4f;
        [SerializeField, Range(0f, 1f)] private float groundDotProduct = 0.5f;
        
        [Header("Jumping")] 
        [SerializeField, Range(0.1f, 10f)] private float groundJumpHeight = 2f;
        [SerializeField, Range(0.1f, 10f)] private float airJumpHeight = 1f;
        
        [SerializeField, Range(1f, 10f)] private float jumpGravityScale = 1;
        [SerializeField, Range(1f, 10f)] private float fallGravityScale = 2;
        [SerializeField, Range(0f, 5)] private int numberOfJumps = 2;
        
        
        [Header("Stun")] 
        [SerializeField, Range(0.1f, 10f)] private float stunTime = 2f;
        
        // Public
        
        public PlayerState CurrentState { get; private set; } = PlayerState.Movement;
        
        
        public Vector2 MoveDirection { get; private set; }
        public int FacingDirection { get; private set; } = 1;
        public Vector2 Velocity => _rigidbody2D.velocity;

        public bool CanJump => _jumpsLeft > 0;

        public bool OnGround => _groundContact != null;
        public bool OnWall => _wallContact != null;
        public bool OnCeiling => _ceilingContact != null;
        
        
        
        // Private animator bools

        private bool _running;
        private bool _jumping;
        private bool _falling;



        // Private
        
        private PhotonView _photonView;
        
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        
        private SpriteRenderer _spriteRenderer;
        private PlayerInputActions _playerInputActions;
        

        private ContactFilter2D _contactFilter2D;
        private readonly ContactPoint2D[] _contactPoints2D = new ContactPoint2D[32];

        private ContactPoint2D? _groundContact;
        private ContactPoint2D? _wallContact;
        private ContactPoint2D? _ceilingContact;

        private bool _jumpRequest;

        private int _jumpsLeft;

        private bool _stunded;

        

        private void OnEnable()
        {
            if(!_photonView.IsMine) return;
            _playerInputActions.Gameplay.Enable();
            SubscribeToActions();
        }

        private void OnDisable()
        {
            if(!_photonView.IsMine) return;
            _playerInputActions.Gameplay.Disable();
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();


            _contactFilter2D = new ContactFilter2D();
            _contactFilter2D.SetLayerMask(LayerMask.GetMask("Ground"));
            
            if(!_photonView.IsMine) return;
            _playerInputActions = new PlayerInputActions();
        }

        private void Start()
        {
            _spriteRenderer.color = App.Instance.GameManager.GetColorOfPlayer(_photonView.Owner);
        }

        #region Events
        private void StunTimerEnd()
        {
            CurrentState = PlayerState.Movement;
            _spriteRenderer.flipY = false;
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            MoveDirection = obj.ReadValue<float>() * Vector2.right;
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            JumpPressed();
        }
        private void OnAttack(InputAction.CallbackContext obj)
        {
            
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            Debug.Log(_photonView.Owner + " is master " + PhotonNetwork.IsMasterClient);
            if (other.CompareTag("Wheel"))
            {
                FindObjectOfType<GameController>().MiniGameEnd.Invoke();
                _photonView.RPC("RPC_PlayerDead", RpcTarget.All);
            }
            else if (other.CompareTag("Player"))
            {
                /*
                var otherPoint = other.transform.position.normalized;
                var myPoint = transform.position.normalized;
                var dotProduct = Vector2.Dot(myPoint, otherPoint);

                if (dotProduct > groundDotProduct)
                {
                    Debug.Log("Below");
                    if(CurrentState == PlayerState.Stunded) return;
                    CurrentState = PlayerState.Stunded;
                }
                else if (dotProduct < -groundDotProduct)
                {
                    Debug.Log("Up");
                }
                else
                {
                    Debug.Log("ID");
                }
                */
            }
        }

        #endregion
        
        private void FixedUpdate()
        {
            if(!_photonView.IsMine) return;
            
            CheckCollisions();

            switch (CurrentState)
            {
                case PlayerState.Movement:
                    UpdateMovement();
                    break;
                case PlayerState.Stunded:
                    //UpdateStunded();
                    break;
            }

            SetAnimatorValues();
        }

        private void SetAnimatorValues()
        {
            _animator.SetBool("Jumping", _jumping);
            _animator.SetBool("Falling", _falling);
            _animator.SetBool("Running", _running);
        }

        #region States

        private void UpdateMovement()
        {
            var previousVelocity = Velocity;
            var velocityChange = Vector2.zero;

            FlipDirection();

            if (_jumpRequest)
            {
                _jumpRequest = false;
                var currentJumpHeight = OnGround ? groundJumpHeight : airJumpHeight;
                velocityChange.y = Mathf.Sqrt(-2f * Physics2D.gravity.y * currentJumpHeight) - previousVelocity.y;
                _rigidbody2D.gravityScale = jumpGravityScale;
            }
            if (previousVelocity.y < 0)
            {
                _rigidbody2D.gravityScale = fallGravityScale;
                _jumping = false;
                _falling = true;
            }
            else if (previousVelocity.y >= 0)
            {
                _rigidbody2D.gravityScale = jumpGravityScale;
                _jumping = true;
            }
            if (OnGround)
            {
                _jumpsLeft = numberOfJumps;
                _rigidbody2D.gravityScale = 0;
                _jumping = false;
                _falling = false;
            }

            velocityChange.x = (MoveDirection.x * moveSpeed - previousVelocity.x);
            //Stop movement if we hit wall
            Debug.Log(OnWall);
            if (OnWall)
            {
                var wallDirection = (int) Mathf.Sign(_wallContact.Value.point.x - transform.position.x);
                var moveDirection = (int) MoveDirection.x;
                if (moveDirection == wallDirection) velocityChange.x = 0;
            }
            
            _rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
            if(OnGround)
                _running = Velocity.x != 0;
        }
        private void JumpPressed()
        {
            if(CurrentState != PlayerState.Movement || !CanJump) return;
            _jumpRequest = true;
            _jumpsLeft--;
        }
        private void FlipDirection()
        {
            if (MoveDirection.x > 0)
                FacingDirection = 1;
            else if (MoveDirection.x < 0)
                FacingDirection = -1;
            _spriteRenderer.transform.localScale = new Vector2( FacingDirection, 1);
        }

        #endregion
        private void SubscribeToActions()
        {
            var gamePlayMap = _playerInputActions.Gameplay;
            
            gamePlayMap.Movement.performed += OnMove;
            gamePlayMap.Movement.canceled += OnMove;
            
            gamePlayMap.Jump.performed += OnJump;

            gamePlayMap.Attack.performed += OnAttack;
            
        }
        private void CheckCollisions()
        {
            _groundContact = null;
            _wallContact = null;
            _ceilingContact = null;

            var numberOfContacts = _rigidbody2D.GetContacts(_contactFilter2D, _contactPoints2D);

            for (var i = 0; i < numberOfContacts; i++)
            {
                var contactPoint = _contactPoints2D[i];
                var dotProduct = Vector2.Dot(Vector2.up, contactPoint.normal);

                /*
                 We are checking how much do two vectors point in the same direction
                 if they are point in same we get 1
                 if in opposite direction we get -1
                 if angle is 90 then we get 0                
                 */
                
                if (dotProduct > groundDotProduct)
                {
                    _groundContact = contactPoint;
                }
                else if (dotProduct < -groundDotProduct)
                {
                    _ceilingContact = contactPoint;
                }
                else
                {
                    _wallContact = contactPoint;
                }
            }
        }

        #region RPCs

        [PunRPC]
        private void RPC_PlayerDead()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}