// Designed by KINEMATION, 2023

using Kinemation.FPSFramework.Runtime.Core.Types;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using UnityEngine;
using UnityEngine.Events;

namespace Kinemation.Demo.Scripts.Runtime
{
    public enum FPSMovementState
    {
        Idle,
        Walking,
        Sprinting,
        InAir,
        Sliding
    }

    public enum FPSPoseState
    {
        Standing,
        Crouching,
        Prone
    }
    
    public class FPSMovement : MonoBehaviour
    {
        private FPSController main_controller;
        
        public delegate bool ConditionDelegate();
        
        [SerializeField] private FPSMovementSettings movementSettings;
        [SerializeField] public Transform rootBone;
        
        [SerializeField] public UnityEvent onStartMoving;
        [SerializeField] public UnityEvent onStopMoving;
        
        [SerializeField] public UnityEvent onSprintStarted;
        [SerializeField] public UnityEvent onSprintEnded;

        [SerializeField] public UnityEvent onCrouch;
        [SerializeField] public UnityEvent onUncrouch;

        [SerializeField] public UnityEvent onJump;
        [SerializeField] public UnityEvent onLanded;
        
        public ConditionDelegate sprintCondition;
        
        public FPSMovementState MovementState { get; private set; }
        public FPSPoseState PoseState { get; private set; }

        public Vector2 AnimatorVelocity { get; private set; }
        
        private CharacterController _controller;
        private Animator _animator;
        private Vector2 _inputDirection;

        public Vector3 MoveVector { get; private set; }
        
        private Vector3 _velocity;

        private float _originalHeight;
        private Vector3 _originalCenter;
        
        private GaitSettings _desiredGait;

        private Vector3 _prevPosition;
        private Vector3 _velocityVector;

        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int Crouching = Animator.StringToHash("Crouching");
        private static readonly int Sprinting = Animator.StringToHash("Sprinting");
        private static readonly int spaced = Animator.StringToHash("spaced");

        private float _sprintAnimatorInterp = 8f;

        private bool _wasMoving = false;

        public bool IsInAir()
        {
            return !_controller.isGrounded;
        }
        
        private bool IsMoving()
        {
            return !Mathf.Approximately(_inputDirection.normalized.magnitude, 0f);
        }

        private float GetSpeedRatio()
        {
            return _velocity.magnitude / _desiredGait.velocity;
        }

        private bool CanSprint()
        {
            return sprintCondition == null || sprintCondition.Invoke();
        }

        private bool TryJump()
        {
            if (!Input.GetKeyDown(movementSettings.jumpKey) || PoseState == FPSPoseState.Crouching)
            {
                return false;
            }

            MovementState = FPSMovementState.InAir;
            return true;
        }

        private bool TrySprint()
        {
            if (PoseState is FPSPoseState.Crouching or FPSPoseState.Prone)
            {
                return false;
            }

            if (_inputDirection.y <= 0f || _inputDirection.x != 0f || !Input.GetKey(movementSettings.sprintKey))
            {
                return false;
            }

            if (!CanSprint()) return false;
            
            MovementState = FPSMovementState.Sprinting;
            return true;
        }

        private bool CanUnCrouch()
        {
            float height = _originalHeight - _controller.radius * 2f;
            Vector3 position = rootBone.TransformPoint(_originalCenter + Vector3.up * height / 2f);
            return !Physics.CheckSphere(position, _controller.radius);
        }
        

        private void Crouch()
        {
            float crouchedHeight = _originalHeight * movementSettings.crouchRatio;
            float heightDifference = _originalHeight - crouchedHeight;

            _controller.height = crouchedHeight;

            // Adjust the center position so the bottom of the capsule remains at the same position
            Vector3 crouchedCenter = _originalCenter;
            crouchedCenter.y -= heightDifference / 2;
            _controller.center = crouchedCenter;

            PoseState = FPSPoseState.Crouching;
            
            _animator.SetBool(Crouching, true);
            onCrouch.Invoke();
        }

        private void UnCrouch()
        {
            _controller.height = _originalHeight;
            _controller.center = _originalCenter;
            
            PoseState = FPSPoseState.Standing;
            
            _animator.SetBool(Crouching, false);
            onUncrouch.Invoke();
        }

        private void UpdatePoseState()
        {
            if (MovementState is FPSMovementState.Sprinting or FPSMovementState.InAir)
            {
                return;
            }

            if (!Input.GetKeyDown(movementSettings.crouchKey))
            {
                return;
            }

            if (PoseState == FPSPoseState.Standing)
            {
                Crouch();

                _desiredGait = movementSettings.crouching;
                return;
            }

            if (!CanUnCrouch()) return;

            UnCrouch();
            _desiredGait = movementSettings.walking;
        } //2

        private void UpdateMovementState()
        {
            if (MovementState == FPSMovementState.InAir && IsInAir())
            {
                // Do not update player movement while jumping or falling
                return;
            }
            
            // Get the current player input
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            _inputDirection.x = moveX;
            _inputDirection.y = moveY;

            // Jump action overrides any other input
            if (TryJump())
            {
                return;
            }
            
            if (TrySprint())
            {
                return;
            }

            if (!IsMoving())
            {
                MovementState = FPSMovementState.Idle;
                return;
            }
            
            MovementState = FPSMovementState.Walking;
        } //1

        private void OnMovementStateChanged(FPSMovementState prevState)
        {
            if (prevState == FPSMovementState.InAir)
            {
                onLanded.Invoke();
            }

            if (prevState == FPSMovementState.Sprinting)
            {
                _sprintAnimatorInterp = 7f;
                onSprintEnded.Invoke();
            }

            if (MovementState == FPSMovementState.Idle)
            {
                float prevVelocity = _desiredGait.velocity;
                _desiredGait = movementSettings.idle;
                _desiredGait.velocity = prevVelocity;
                return;
            }

            if (MovementState == FPSMovementState.InAir)
            {
                _velocity.y = movementSettings.jumpHeight;
                onJump.Invoke();
                return;
            }

            if (MovementState == FPSMovementState.Sprinting)
            {
                _desiredGait = movementSettings.sprinting;
                onSprintStarted.Invoke();
                return;
            }
            

            if (PoseState == FPSPoseState.Crouching)
            {
                _desiredGait = movementSettings.crouching;
                return;
            }

            // Walking state
            _desiredGait = movementSettings.walking;
        }

        private void UpdateGrounded() //3
        {
            var normInput = _inputDirection.normalized;
            var desiredVelocity = rootBone.right * normInput.x + rootBone.forward * normInput.y;

            desiredVelocity *= _desiredGait.velocity;

            desiredVelocity = Vector3.Lerp(_velocity, desiredVelocity, 
                FPSAnimLib.ExpDecayAlpha(_desiredGait.velocitySmoothing, Time.deltaTime));
            
            _velocity = desiredVelocity;

            desiredVelocity.y = -movementSettings.gravity;
            MoveVector = desiredVelocity;
            
        }
        
        private void UpdateInAir()
        {
            var normInput = _inputDirection.normalized;
            _velocity.y -= movementSettings.gravity * Time.deltaTime;
            _velocity.y = Mathf.Max(-movementSettings.maxFallVelocity, _velocity.y);
            
            var desiredVelocity = rootBone.right * normInput.x + rootBone.forward * normInput.y;
            desiredVelocity *= _desiredGait.velocity;

            desiredVelocity = Vector3.Lerp(_velocity, desiredVelocity * movementSettings.airFriction, 
                FPSAnimLib.ExpDecayAlpha(movementSettings.airVelocity, Time.deltaTime));

            desiredVelocity.y = _velocity.y;
            _velocity = desiredVelocity;
            
            MoveVector = desiredVelocity;
        }
        
        
        private float verticalVelocity = 0f;

        [SerializeField] private PlayMakerFSM spacedMovement;
        [SerializeField] private CapsuleCollider spacedCollider;

        private bool _jetpackIsOn;
        private void UpdateInSpace()
        {
            float moveX = spacedMovement.Fsm.GetFsmVector3("strafeVector").Value.normalized.x;
            float moveY = spacedMovement.Fsm.GetFsmVector3("strafeVector").Value.normalized.z;

            verticalVelocity = spacedMovement.Fsm.GetFsmFloat("moveUP").Value +
                               spacedMovement.Fsm.GetFsmFloat("moveDOWN").Value;
            verticalVelocity = Mathf.Clamp(verticalVelocity, -1f, 1f);

            moveX *= 1f - verticalVelocity;
            moveY *= 1f - verticalVelocity;

            _inputDirection.x = moveX;
            _inputDirection.y = moveY;
            
            if (!IsMoving())
            {
                MovementState = FPSMovementState.Idle;
            }
            else
            {
                MovementState = FPSMovementState.Walking;
            }
            
            _desiredGait = movementSettings.walking;
            
            var normInput = _inputDirection.normalized;
            
            var desiredVelocity = rootBone.right * normInput.x + rootBone.forward * normInput.y + rootBone.up * verticalVelocity;

            desiredVelocity *= _desiredGait.velocity;

            desiredVelocity = Vector3.Lerp(_velocity, desiredVelocity, 
                FPSAnimLib.ExpDecayAlpha(_desiredGait.velocitySmoothing, Time.deltaTime));
            
            _velocity = desiredVelocity;
            
            MoveVector = desiredVelocity;
            
            var animatorVelocity = _inputDirection;
            //animatorVelocity *= MovementState == FPSMovementState.InAir ? 0f : 1f;

            AnimatorVelocity = Vector2.Lerp(AnimatorVelocity, animatorVelocity, 
                FPSAnimLib.ExpDecayAlpha(_desiredGait.velocitySmoothing, Time.deltaTime));

            _animator.SetFloat(MoveX, AnimatorVelocity.x);
            _animator.SetFloat(MoveY, AnimatorVelocity.y);

            main_controller.charAnimData.upDownInput = verticalVelocity;

            _jetpackIsOn = spacedMovement.Fsm.GetFsmFloat("jetpackOn").Value > 0f;
            main_controller.charAnimData.jetpackIsOn = _jetpackIsOn;
        }
        
        
        private void UpdateMovement() //4
        {
            _controller.Move(MoveVector * Time.deltaTime);
        }

        private void UpdateAnimatorParams() //5
        {
            var animatorVelocity = _inputDirection;
            animatorVelocity *= MovementState == FPSMovementState.InAir ? 0f : 1f;

            AnimatorVelocity = Vector2.Lerp(AnimatorVelocity, animatorVelocity, 
                FPSAnimLib.ExpDecayAlpha(_desiredGait.velocitySmoothing, Time.deltaTime));

            _animator.SetFloat(MoveX, AnimatorVelocity.x);
            _animator.SetFloat(MoveY, AnimatorVelocity.y);
            _animator.SetFloat(Velocity, AnimatorVelocity.magnitude);
            _animator.SetBool(InAir, IsInAir());
            _animator.SetBool(Moving, IsMoving());

            // Sprinting needs to be blended manually
            float a = _animator.GetFloat(Sprinting);
            float b = MovementState == FPSMovementState.Sprinting ? 1f : 0f;

            a = Mathf.Lerp(a, b, FPSAnimLib.ExpDecayAlpha(_sprintAnimatorInterp, Time.deltaTime));
            _animator.SetFloat(Sprinting, a);
        }

        private void Start()
        {
            main_controller = GetComponent<FPSController>();
            
            _controller = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            
            _originalHeight = _controller.height;
            _originalCenter = _controller.center;
            
            MovementState = FPSMovementState.Idle;
            PoseState = FPSPoseState.Standing;

            _desiredGait = movementSettings.walking;
        }

        private void Update()
        {
            if (!main_controller.spaced)
            {
                var prevState = MovementState;
                UpdateMovementState();
                UpdatePoseState();

                if (prevState != MovementState)
                {
                    OnMovementStateChanged(prevState);
                }

                bool isMoving = IsMoving();

                if (_wasMoving != isMoving)
                {
                    if (isMoving)
                    {
                        onStartMoving?.Invoke();
                    }
                    else
                    {
                        onStopMoving?.Invoke();
                    }
                }

                _wasMoving = isMoving;

                if (MovementState == FPSMovementState.InAir)
                {
                    UpdateInAir();
                }
                else
                {
                    UpdateGrounded();
                }

                UpdateMovement();
                UpdateAnimatorParams();
            }
            else
            {
                UpdateInSpace();
            }
        }
        
        public void SetupGravityState(bool isThereAnyGravity)
        {
            _animator.SetBool(spaced, !isThereAnyGravity);
            
            _controller.enabled = isThereAnyGravity;
            spacedCollider.enabled = !isThereAnyGravity;
            spacedMovement.enabled = !isThereAnyGravity;
            main_controller.charAnimData._spaced = !isThereAnyGravity;
            if (PoseState == FPSPoseState.Crouching)
                UnCrouch();

            
        }


    }

}