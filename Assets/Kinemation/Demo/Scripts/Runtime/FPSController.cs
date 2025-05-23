// Designed by KINEMATION, 2023

using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using Kinemation.FPSFramework.Runtime.Layers;
using Kinemation.FPSFramework.Runtime.Recoil;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

using UnityEngine.InputSystem;
using Obscure.SDC;

namespace Demo.Scripts.Runtime
{
    public enum FPSMovementState
    {
        Idle,
        Walking,
        Running,
        Sprinting
    }

    public enum FPSPoseState
    {
        Standing,
        Crouching
    }

    public enum FPSActionState
    {
        None,
        Ready,
        Aiming,
        PointAiming,
    }

    public enum FPSCameraState
    {
        Default,
        Barrel,
        InFront
    }

    // An example-controller class
    public class FPSController : FPSAnimController
    {
        [Tab("Dubspac3d")] [Header("Parameters")] 
        [SerializeField] private bool spaced;

        [SerializeField] private bool physicsMovement;

        [SerializeField] private TMP_Text debugText;
        [SerializeField] private PlayMakerFSM spacedMovement;
        [SerializeField] private PlayMakerFSM fsm_interactions;
        
        [Tab("Animation")] [Header("General")] [SerializeField]
        private Animator animator;

        [SerializeField] private float turnInPlaceAngle;
        [SerializeField] private float turnInPlaceAngleY;
        [SerializeField] private Transform turnYConsiderRotation;
        [SerializeField] private Transform pivotPoint;
        [SerializeField] private AnimationCurve turnCurve = new AnimationCurve(new Keyframe(0f, 0f));
        [SerializeField] private float turnSpeed = 1f;
        
        [Header("Dynamic Motions")]
        [SerializeField] private IKAnimation aimMotionAsset;
        [SerializeField] private IKAnimation leanMotionAsset;
        [SerializeField] private IKAnimation crouchMotionAsset;
        [SerializeField] private IKAnimation unCrouchMotionAsset;
        [SerializeField] private IKAnimation onJumpMotionAsset;
        [SerializeField] private IKAnimation onLandedMotionAsset;

        // Animation Layers
        [SerializeField] [HideInInspector] private LookLayer lookLayer;
        [SerializeField] [HideInInspector] private AdsLayer adsLayer;
        [SerializeField] [HideInInspector] private SwayLayer swayLayer;
        [SerializeField] [HideInInspector] private LocomotionLayer locoLayer;
        [SerializeField] [HideInInspector] private SlotLayer slotLayer;
        // Animation Layers

        [Tab("HUD")] 
        [Header("Task")]
        [SerializeField] private GameObject obj_task;
        [SerializeField] private Image sprite_task;
        [SerializeField] private TMP_Text text_task_name;
        [SerializeField] private TMP_Text text_task_details;
        
        [Header("Hint_1")]
        [SerializeField] private GameObject obj_hint1;
        [SerializeField] private Image sprite_hint1;
        [SerializeField] private TMP_Text text_hint1_button;
        [SerializeField] private TMP_Text text_hint1_details;
        [SerializeField] private RectTransform rt_hint1_cooldown;
        
        [Header("Hint_2")]
        [SerializeField] private GameObject obj_hint2;
        [SerializeField] private Image sprite_hint2;
        [SerializeField] private TMP_Text text_hint2_button;
        [SerializeField] private TMP_Text text_hint2_details;
        [SerializeField] private RectTransform rt_hint2_cooldown;

        [Header("Health")] 
        [SerializeField] private GameObject obj_health;
        [SerializeField] private Slider slider_health;
        [SerializeField] private TMP_Text text_health;

        [Header("Grenade")] 
        [SerializeField] private GameObject obj_grenade;
        [SerializeField] private Image sprite_grenade;
        [SerializeField] private TMP_Text text_grenade_amount;
        [SerializeField] private TMP_Text text_grenade_button;
        [SerializeField] private RectTransform rt_grenade_cooldown;

        
        [Header("Medkit")]
        [SerializeField] private GameObject obj_medkit;
        [SerializeField] private Image sprite_medkit;
        [SerializeField] private TMP_Text text_medkit_amount;
        [SerializeField] private TMP_Text text_medkit_button;
        [SerializeField] private RectTransform rt_medkit_cooldown;


        [Header("Jetpack")]
        [SerializeField] private GameObject obj_jetpack;
        [SerializeField] private Image sprite_jetpack;
        [SerializeField] private Slider slider_jetpack;
        
        [Header("Equipment")]
        [SerializeField] private GameObject obj_equipment;
        
        [Header("Weapon current")] 
        [SerializeField] private Image sprite_wpn_curr;
        [SerializeField] private TMP_Text text_wpn_curr_ammo;
        [SerializeField] private TMP_Text text_wpn_curr_ammo_max;
        [SerializeField] private RectTransform rt_wpn_curr_cooldown;
        
        [Header("Weapon other")] 
        [SerializeField] private Image sprite_wpn_oth;
        [SerializeField] private TMP_Text text_wpn_oth_button;
        
        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot1;
        [SerializeField] private GameObject obj_empty_slot1;
        [SerializeField] private GameObject obj_full_slot1;
        [SerializeField] private Image sprite_slot1;
        [SerializeField] private TMP_Text text_slot1_amount;
        [SerializeField] private TMP_Text text_slot1_button;

        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot2;
        [SerializeField] private GameObject obj_empty_slot2;
        [SerializeField] private GameObject obj_full_slot2;
        [SerializeField] private Image sprite_slot2;
        [SerializeField] private TMP_Text text_slot2_amount;
        [SerializeField] private TMP_Text text_slot2_button;

        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot3;
        [SerializeField] private GameObject obj_empty_slot3;
        [SerializeField] private GameObject obj_full_slot3;
        [SerializeField] private Image sprite_slot3;
        [SerializeField] private TMP_Text text_slot3_amount;
        [SerializeField] private TMP_Text text_slot3_button;
        
        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot4;
        [SerializeField] private GameObject obj_empty_slot4;
        [SerializeField] private GameObject obj_full_slot4;
        [SerializeField] private Image sprite_slot4;
        [SerializeField] private TMP_Text text_slot4_amount;
        [SerializeField] private TMP_Text text_slot4_button;


        [Tab("Controller")] 
        [Header("General")] 
        [SerializeField] private CharacterController controller;
        [SerializeField] private float gravity = 8f;
        [SerializeField] private float jumpHeight = 9f;
        private bool _isInAir = false;

        [Header("Camera")]
        [SerializeField] public Transform mainCamera;
        [SerializeField] private Transform cameraHolder;
        [SerializeField] private Transform firstPersonCamera;
        [SerializeField] private Transform freeCamera;
        [SerializeField] private float sensitivity;
        [SerializeField] private Vector2 freeLookAngle;
        
        [Header("Movement")] 
        [SerializeField] private float curveLocomotionSmoothing = 2f;
        [SerializeField] private float moveSmoothing = 2f;
        [SerializeField] private float sprintSpeed = 3f;
        [SerializeField] private float walkingSpeed = 2f;
        [SerializeField] private float crouchSpeed = 1f;
        [SerializeField] private float crouchRatio = 0.5f;
        private float speed;

        [Tab("Weapon")] 
        [SerializeField] private List<Weapon> weapons;
        [SerializeField] private FPSCameraShake shake;

        //[Tab("Inventory")]
        //[SerializeField] private List<Item> items;

        private bool disableInput = false;
        private Vector2 _playerInput;

        // Used for free-look
        private Vector2 _freeLookInput;
        private Vector2 _smoothAnimatorMove;
        private Vector2 _smoothMove;

        private int _index;
        private int _lastIndex;

        private float _fireTimer = -1f;
        private int _bursts;
        private bool _aiming;
        private bool _freeLook;
        private bool _hasActiveAction;
        
        private FPSActionState actionState;
        private FPSMovementState movementState;
        private FPSPoseState poseState;
        private FPSCameraState cameraState = FPSCameraState.Default;
        
        private float originalHeight;
        private Vector3 originalCenter;

        private float smoothCurveAlpha = 0f;

        private static readonly int Crouching = Animator.StringToHash("Crouching");
        private static readonly int Moving = Animator.StringToHash("Moving");
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int OverlayType = Animator.StringToHash("OverlayType");
        private static readonly int TurnRight = Animator.StringToHash("TurnRight");
        private static readonly int TurnLeft = Animator.StringToHash("TurnLeft");
        private static readonly int InAir = Animator.StringToHash("InAir");
        private static readonly int Equip = Animator.StringToHash("Equip");
        private static readonly int UnEquip = Animator.StringToHash("Unequip");

        private Rigidbody rb;
        
        private void InitLayers()
        {
            InitAnimController();

            controller = GetComponentInChildren<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            lookLayer = GetComponentInChildren<LookLayer>();
            adsLayer = GetComponentInChildren<AdsLayer>();
            locoLayer = GetComponentInChildren<LocomotionLayer>();
            swayLayer = GetComponentInChildren<SwayLayer>();
            slotLayer = GetComponentInChildren<SlotLayer>();
        }

        private void Start()
        {
            Time.timeScale = 1f;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            speed = walkingSpeed;
            
            originalHeight = controller.height;
            originalCenter = controller.center;

            moveRotation = transform.rotation;

            InitLayers();
            EquipWeapon();
            rb = GetComponent<Rigidbody>();
            
            fsm_interactions.Fsm.GetFsmGameObject("camera").Value = mainCamera.gameObject;
        }
        
        private void StartWeaponChange()
        {
            DisableAim();
            
            _hasActiveAction = true;
            animator.CrossFade(UnEquip, 0.1f);
        }

        public void SetActionActive(int isActive)
        {
            _hasActiveAction = isActive != 0;
        }

        public void RefreshStagedState()
        {
            GetGun().stagedReloadSegment++;
        }
        
        public void ResetStagedState()
        {
            GetGun().stagedReloadSegment = 0;
        }

        public void EquipWeapon()
        {
            if (weapons.Count == 0) return;

            weapons[_lastIndex].gameObject.SetActive(false);
            var gun = weapons[_index];

            _bursts = gun.burstAmount;
            
            StopAnimation(0.1f);
            InitWeapon(gun);
            gun.gameObject.SetActive(true);
            
            if (ch) ch.SetSizeNoSmooth(minSize);
            ch = gun.ch;
            increment = new Vector2(gun.chIncrement,gun.chIncrement);
            minSize = new Vector2(gun.chMinSize, gun.chMinSize);
            maxSize = new Vector2(gun.chMaxSize, gun.chMaxSize);
            reduceTime = gun.chReduceSpeed;

            sprite_wpn_curr.sprite = gun.weaponSprite;
            text_wpn_curr_ammo.text = gun._currentAmmo.ToString();
            text_wpn_curr_ammo_max.text = gun.ammoInMag.ToString();

            animator.SetFloat(OverlayType, (float) gun.overlayType);
            animator.Play(Equip);
        }
        
        
        private void ChangeWeapon_Internal()
        {
            if (movementState == FPSMovementState.Sprinting) return;
            if (_hasActiveAction) return;
            
            OnFireReleased();
            
            int newIndex = _index;
            newIndex++;
            if (newIndex > weapons.Count - 1)
            {
                newIndex = 0;
            }

            _lastIndex = _index;
            _index = newIndex;

            StartWeaponChange();
        }

        private void DisableAim()
        {
            _aiming = false;
            OnInputAim(_aiming);
            
            actionState = FPSActionState.None;
            adsLayer.SetAds(false);
            adsLayer.SetPointAim(false);
            swayLayer.SetFreeAimEnable(true);
            swayLayer.SetLayerAlpha(1f);
            slotLayer.PlayMotion(aimMotionAsset);
        }

        public void ToggleAim()
        {
            if (_hasActiveAction)
            {
                //return;
            }
            
            _aiming = !_aiming;

            if (_aiming)
            {
                actionState = FPSActionState.Aiming;
                adsLayer.SetAds(true);
                swayLayer.SetFreeAimEnable(false);
                swayLayer.SetLayerAlpha(0.3f);
                slotLayer.PlayMotion(aimMotionAsset);
                OnInputAim(_aiming);
            }
            else
            {
                DisableAim();
            }

            recoilComponent.isAiming = _aiming;
        }

        public void ChangeScope()
        {
            InitAimPoint(GetGun());
        }


        private Crosshair ch;
        private Vector2 increment = new Vector2(10, 10);
        private Vector2 minSize = new Vector2(20, 20);
        private Vector2 maxSize = new Vector2(100, 100);
        private float reduceTime = 0.01f;
        
        private void Fire() 
        {
            if (_hasActiveAction) return;
            

            Weapon wpn = GetGun();


            if (!wpn.HasEnoughAmmo()) 
            {Debug.Log("no ammo");
                return;
            }
            
            wpn.OnFire();
            
            recoilComponent.Play();
            PlayCameraShake(shake);
            
            wpn.ReduceAmmo();
            text_wpn_curr_ammo.text = wpn._currentAmmo.ToString();
            _bursts = wpn.burstAmount - 1;
            _fireTimer = 0f;

            var worldRecoilVector = wpn.physRecoilPoint.transform.TransformDirection(0,0,-1f*wpn.physRecoilPower);
            rb.AddForce(worldRecoilVector, ForceMode.Impulse);

            Vector2 newSize = ch.GetSize() + increment;
            
            //if (newSize.x > maxSize.x) newSize = maxSize;
            ch.SetSizeNoSmooth(newSize);
        }

   
        
        public void OnFirePressed()
        {      
            if (weapons.Count == 0) return;
            if (_hasActiveAction) return;
    
            Fire();
            
        }

        private Weapon GetGun()
        {
            return weapons[_index];
        }

        public void OnFireReleased()
        {
            recoilComponent.Stop();
            _fireTimer = -1f;
        }

        private void SprintPressed()
        {
            if (poseState == FPSPoseState.Crouching || _hasActiveAction || _isInAir)
            {
                return;
            }

            if (spaced) return;

            OnFireReleased();
            lookLayer.SetLayerAlpha(0.5f);
            adsLayer.SetLayerAlpha(0f);
            locoLayer.SetReadyWeight(0f);

            movementState = FPSMovementState.Sprinting;
            actionState = FPSActionState.None;

            recoilComponent.Stop();

            speed = sprintSpeed;
        }

        private void SprintReleased()
        {
            if (poseState == FPSPoseState.Crouching)
            {
                return;
            }

            lookLayer.SetLayerAlpha(1f);
            adsLayer.SetLayerAlpha(1f);
            movementState = FPSMovementState.Walking;

            speed = walkingSpeed;
        }

        private void Crouch()
        {
            //todo: crouching implementation
            
            float crouchedHeight = originalHeight * crouchRatio;
            float heightDifference = originalHeight - crouchedHeight;

            controller.height = crouchedHeight;

            // Adjust the center position so the bottom of the capsule remains at the same position
            Vector3 crouchedCenter = originalCenter;
            crouchedCenter.y -= heightDifference / 2;
            controller.center = crouchedCenter;

            speed = crouchSpeed;

            lookLayer.SetPelvisWeight(0f);

            poseState = FPSPoseState.Crouching;
            animator.SetBool(Crouching, true);
            slotLayer.PlayMotion(crouchMotionAsset);
        }

        private void Uncrouch()
        {
            //todo: crouching implementation
            controller.height = originalHeight;
            controller.center = originalCenter;

            speed = walkingSpeed;

            lookLayer.SetPelvisWeight(1f);

            poseState = FPSPoseState.Standing;
            animator.SetBool(Crouching, false);
            slotLayer.PlayMotion(unCrouchMotionAsset);
        }

        public void TryReload()
        {
            if (movementState == FPSMovementState.Sprinting || _hasActiveAction) return;

            var wpn = GetGun();
            var reloadClip = wpn.reloadClip;
            
            if (reloadClip == null) return;
            
            OnFireReleased();
            //DisableAim();
            
            PlayAnimation(reloadClip);
            
            wpn.Reload();
            text_wpn_curr_ammo.text = wpn._currentAmmo.ToString();
        }

        public void TryGrenadeThrow()
        {
            if (movementState == FPSMovementState.Sprinting || _hasActiveAction) return;

            if (GetGun().grenadeClip == null) return;

            
            OnFireReleased();
            DisableAim();
            PlayAnimation(GetGun().grenadeClip);
        }


        public void ShootPressed(InputAction.CallbackContext context)
        {   
            if (disableInput) return;
            if (context.performed)
            { 
                OnFirePressed();
            }
            else
            {
                OnFireReleased();
            }
            
        }
        
        public void JetpackPressed(InputAction.CallbackContext context)
        {
            if (disableInput) return;
            if (context.performed)
            { 
                SprintPressed();
            }
            else
            {
                SprintReleased();
            }
            
        }

        public void ReloadPressed(InputAction.CallbackContext context)
        {
            if (disableInput) return;
            if (context.performed)
            {
                TryReload();
            }
            
        }
        
        public void GrenadePressed(InputAction.CallbackContext context)
        {
            if (disableInput) return;
            if (context.performed)
            {
                TryGrenadeThrow();
            }
            
        }
        
        public void HealthpackPressed(InputAction.CallbackContext context)
        {
            if (disableInput) return;
            if (context.performed)
            {
                //TO DO
            }
            
        }

        public void SwapWeaponPressed(InputAction.CallbackContext context)
        {
            if (disableInput) return;
            if (context.performed)
            {
                ChangeWeapon_Internal();
            }
        }
        

        private void UpdateActionInput()
        {
            smoothCurveAlpha = FPSAnimLib.ExpDecay(smoothCurveAlpha, _aiming ? 0.4f : 1f, 10,
                Time.deltaTime);
            
            animator.SetLayerWeight(3, smoothCurveAlpha);
            

          /*  if (Input.GetKeyDown(KeyCode.Y))
            {
                StopAnimation(0.2f);
            }*/

            charAnimData.leanDirection = 0;
            

            if (movementState == FPSMovementState.Sprinting)
            {
                return;
            }
            
            if (actionState != FPSActionState.Ready)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyUp(KeyCode.Q)
                                                || Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.E))
                {
                    slotLayer.PlayMotion(leanMotionAsset);
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    charAnimData.leanDirection = 1;
                    if (physicsMovement)
                    {
                        rb.AddRelativeTorque(Vector3.forward,ForceMode.Impulse);
                    }
                    else
                    {
                        transform.rotation *= Quaternion.Euler(0f, 0f, 1f); /////////
                        transform.rotation *= Quaternion.Slerp(Quaternion.identity,
                            Quaternion.Euler(0f, 0f, turnInPlaceAngle / 100), Time.deltaTime);
                    }

                }
                else if (Input.GetKey(KeyCode.E))
                {
                    charAnimData.leanDirection = -1;
                    if (physicsMovement)
                    {
                        rb.AddRelativeTorque(Vector3.back,ForceMode.Impulse);
                    }
                    else
                    {
                        transform.rotation *= Quaternion.Euler(0f,0f, -1f); /////////
                        transform.rotation *= Quaternion.Slerp(Quaternion.identity,
                            Quaternion.Euler(0f, 0f, turnInPlaceAngle/100), Time.deltaTime);
                    }
                    
                }
    
                /*
                if (Input.GetKeyDown(KeyCode.V))
                {
                    ChangeScope();
                }

                if (Input.GetKeyDown(KeyCode.B) && _aiming)
                {
                    if (actionState == FPSActionState.PointAiming)
                    {
                        adsLayer.SetPointAim(false);
                        actionState = FPSActionState.Aiming;
                    }
                    else
                    {
                        adsLayer.SetPointAim(true);
                        actionState = FPSActionState.PointAiming;
                    }
                } */
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (spaced) return;
                if (poseState == FPSPoseState.Standing)
                {
                    Crouch();
                }
                else
                {
                    Uncrouch();
                }
            } 

            /*if (Input.GetKeyDown(KeyCode.H))
            {
                if (actionState == FPSActionState.Ready)
                {
                    actionState = FPSActionState.None;
                    locoLayer.SetReadyWeight(0f);
                    lookLayer.SetLayerAlpha(1f);
                }
                else
                {
                    actionState = FPSActionState.Ready;
                    locoLayer.SetReadyWeight(1f);
                    lookLayer.SetLayerAlpha(.5f);
                    OnFireReleased();
                }
            } */
        }

        private Quaternion desiredRotation;
        private Quaternion desiredRotationY;
        private Quaternion moveRotation;
        private Quaternion moveRotationY;
        private float turnProgress = 1f;
        private float turnProgressY;
        private bool isTurning = false;
        private bool isTurningY = false;
        

        private void TurnInPlace()
        {
            float turnInput = _playerInput.x;
            
            if (spaced)
            {
                _playerInput.x = Mathf.Clamp(_playerInput.x, -1*turnInPlaceAngle, turnInPlaceAngle);
            }
            else
            {
                _playerInput.x = Mathf.Clamp(_playerInput.x, -90f, 90f);
            }
            
            turnInput -= _playerInput.x; ///////
            
            
            
            
            float sign = Mathf.Sign(_playerInput.x);
            /////////if (Mathf.Abs(turnInput) > 60f) 
            if (_playerInput.x>turnInPlaceAngle)
            {

               ///////// rb.AddRelativeTorque(0f,sign * Mathf.Abs(_playerInput.x-turnInput)/2 ,0f,ForceMode.Impulse);
                
                
                ///////////
                if (!isTurning)
                {
                    turnProgress = 0f;
                    
                    animator.ResetTrigger(TurnRight);
                    animator.ResetTrigger(TurnLeft);
                    
                    animator.SetTrigger(sign > 0f ? TurnRight : TurnLeft);
                }
                
                isTurning = true; 
                //////////////
            }
            
            transform.rotation *= Quaternion.Euler(0f, turnInput, 0f); /////////
            if (physicsMovement)
                rb.AddRelativeTorque(Vector3.up * turnInput * rb.mass/250,ForceMode.Impulse);
            
            /////////////
            float lastProgress = turnCurve.Evaluate(turnProgress);
            turnProgress += Time.deltaTime * turnSpeed;
            turnProgress = Mathf.Min(turnProgress, 1f);
            
            float deltaProgress = turnCurve.Evaluate(turnProgress) - lastProgress;

            _playerInput.x -= sign * turnInPlaceAngle * deltaProgress;
            
            transform.rotation *= Quaternion.Slerp(Quaternion.identity,Quaternion.Euler(0f, sign * turnInPlaceAngle, 0f), deltaProgress);
            
            if (Mathf.Approximately(turnProgress, 1f) && isTurning)
            {
                isTurning = false;
            }
            ////////////////
        }

        private void TurnY()
        {
            if (!spaced)
            {
                _playerInput.y = Mathf.Clamp(_playerInput.y, -80f, 80f);
                return;
            }
            float turnInput = _playerInput.y;
            _playerInput.y = Mathf.Clamp(_playerInput.y, -1*turnInPlaceAngleY, turnInPlaceAngle);
            turnInput -= _playerInput.y;///////////

            float sign = Mathf.Sign(_playerInput.y);
           // if (Mathf.Abs(turnInput) > 45f) //_playerInput.y>turnInPlaceAngle
            if (_playerInput.y>turnInPlaceAngleY)
            {
               /////////////////// rb.AddRelativeTorque(sign * Mathf.Abs(_playerInput.y*turnInput)/1200,0f ,0f,ForceMode.Impulse);
                //////////
                if (!isTurningY)
                {
                    turnProgressY = 0f;
                }
                
                isTurningY = true;
                ////////////////
            }

           /////////////////
            //transform.rotation *= turnYConsiderRotation.localRotation;
            //transform.rotation *= Quaternion.Euler(0f, turnInput, 0f); /////////

            transform.rotation *= Quaternion.Euler(turnInput, 0f, 0f);
            if (physicsMovement)
                rb.AddRelativeTorque(Vector3.right * turnInput * rb.mass/250,ForceMode.Impulse);
            
            float lastProgress = turnCurve.Evaluate(turnProgressY);
            turnProgressY += Time.deltaTime * turnSpeed;
            turnProgressY = Mathf.Min(turnProgressY, 1f);
            
            float deltaProgress = turnCurve.Evaluate(turnProgressY) - lastProgress;

            _playerInput.y -= sign * turnInPlaceAngleY * deltaProgress;
            
            transform.rotation *= Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(sign * turnInPlaceAngleY, 0f, 0f), deltaProgress);
            
            
            if (Mathf.Approximately(turnProgressY, 1f) && isTurningY)
            {
                isTurningY = false;
            }
            
            /////////////////////
            
            
        }
        

        private float _jumpState = 0f;
        
        private void UpdateLookInput()
        {
            
    
          //  _freeLook = Input.GetKey(KeyCode.X);

            float deltaMouseX = Input.GetAxis("Mouse X") * sensitivity;
            float deltaMouseY = -Input.GetAxis("Mouse Y") * sensitivity;

            /*
            if (_freeLook)
            {
                // No input for both controller and animation component. We only want to rotate the camera

                _freeLookInput.x += deltaMouseX;
                _freeLookInput.y += deltaMouseY;

                _freeLookInput.x = Mathf.Clamp(_freeLookInput.x, -freeLookAngle.x, freeLookAngle.x);
                _freeLookInput.y = Mathf.Clamp(_freeLookInput.y, -freeLookAngle.y, freeLookAngle.y);

                return;
            }

            _freeLookInput = FPSAnimLib.ExpDecay(_freeLookInput, Vector2.zero, 15f, Time.deltaTime); */
            
            _playerInput.x += deltaMouseX;
            _playerInput.y += deltaMouseY;

            /*
            if (spaced)
            {
                _playerInput.y = Mathf.Clamp(_playerInput.y, -30f, 30f);
                
            }
            else
            {
                _playerInput.y = Mathf.Clamp(_playerInput.y, -90f, 90f);
            } */

            moveRotation *= Quaternion.Euler(deltaMouseY, deltaMouseX, 0f);
            TurnInPlace();
            TurnY();

           // _jumpState = FPSAnimLib.ExpDecay(_jumpState, _isInAir ? 1f : 0f, 10f, Time.deltaTime);
           if (!spaced)
           {
            float moveWeight = Mathf.Clamp01(Mathf.Abs(_smoothMove.magnitude));
            
                transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, moveWeight);
                transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, _jumpState);
            
            

            _playerInput.x *= 1f - moveWeight;
            _playerInput.x *= 1f - _jumpState;
           }
            
            charAnimData.SetAimInput(_playerInput);
            charAnimData.AddDeltaInput(new Vector2(deltaMouseX, charAnimData.deltaAimInput.y));
        }
        
        private void UpdateFiring()
        {
            ch.SetSize(minSize,reduceTime);
            if (recoilComponent == null) return;
            
            if (recoilComponent.fireMode != FireMode.Semi && _fireTimer >= 60f / GetGun().fireRate)
            {
                Fire();

                if (recoilComponent.fireMode == FireMode.Burst)
                {
                    _bursts--;

                    if (_bursts == 0)
                    {
                        _fireTimer = -1f;
                        OnFireReleased();
                    }
                    else
                    {
                        _fireTimer = 0f;
                    }
                }
                else
                {
                    _fireTimer = 0f;
                }
            }

            if (_fireTimer >= 0f)
            {
                _fireTimer += Time.deltaTime;
            }
        }

        private bool IsZero(float value)
        {
            return Mathf.Approximately(0f, value);
        }

        private float verticalVelocity = 0f;
        private float smoothIsMoving = 0f;

        private void UpdateMovementSpaced()
        {
            float moveX = spacedMovement.Fsm.GetFsmVector3("strafeVector").Value.normalized.x;
            float moveY = spacedMovement.Fsm.GetFsmVector3("strafeVector").Value.normalized.z;

            verticalVelocity = spacedMovement.Fsm.GetFsmFloat("moveUP").Value +
                               spacedMovement.Fsm.GetFsmFloat("moveDOWN").Value;
            verticalVelocity = Mathf.Clamp(verticalVelocity, -1f, 1f);
            
            moveX *= 1f - verticalVelocity;
            moveY *= 1f - verticalVelocity;
            
            Vector2 rawInput = new Vector2(moveX, moveY);
            Vector2 normInput = new Vector2(moveX, moveY);
            normInput.Normalize();

            _smoothMove = FPSAnimLib.ExpDecay(_smoothMove, normInput, moveSmoothing, Time.deltaTime);

            moveX = _smoothMove.x;
            moveY = _smoothMove.y;
            
            charAnimData.moveInput = normInput;
            charAnimData.upDownInput = verticalVelocity;

            _smoothAnimatorMove.x = FPSAnimLib.ExpDecay(_smoothAnimatorMove.x, rawInput.x, 5f, Time.deltaTime);
            _smoothAnimatorMove.y = FPSAnimLib.ExpDecay(_smoothAnimatorMove.y, rawInput.y, 4f, Time.deltaTime);
            
            bool idle = Mathf.Approximately(0f, normInput.magnitude);
            animator.SetBool(Moving, !idle);
            
            smoothIsMoving = FPSAnimLib.ExpDecay(smoothIsMoving, idle ? 0f : 1f, curveLocomotionSmoothing, 
                Time.deltaTime);
            
            animator.SetFloat(Velocity, Mathf.Clamp01(smoothIsMoving));
            animator.SetFloat(MoveX, _smoothAnimatorMove.x);
            animator.SetFloat(MoveY, _smoothAnimatorMove.y);


            
        }

        private void UpdateMovement()
        {
            if (spaced)
            {
                UpdateMovementSpaced();
                return;
            }

            
            float moveX = Input.GetAxisRaw("Horizontal");
            
            float moveY = Input.GetAxisRaw("Vertical");
            

            charAnimData.moveInput = new Vector2(moveX, moveY);

            moveX *= 1f - _jumpState;
            moveY *= 1f - _jumpState;
            

            Vector2 rawInput = new Vector2(moveX, moveY);
            Vector2 normInput = new Vector2(moveX, moveY);
            normInput.Normalize();
            
            if ((IsZero(normInput.y) || !IsZero(normInput.x)) 
                && movementState == FPSMovementState.Sprinting)
            {
                SprintReleased();
            }
            
            if (movementState == FPSMovementState.Sprinting)
            {
                normInput.x = rawInput.x = 0f;
                normInput.y = rawInput.y = 2f;
            }

            _smoothMove = FPSAnimLib.ExpDecay(_smoothMove, normInput, moveSmoothing, Time.deltaTime);

            moveX = _smoothMove.x;
            moveY = _smoothMove.y;
            
            charAnimData.moveInput = normInput;

            _smoothAnimatorMove.x = FPSAnimLib.ExpDecay(_smoothAnimatorMove.x, rawInput.x, 5f, Time.deltaTime);
            _smoothAnimatorMove.y = FPSAnimLib.ExpDecay(_smoothAnimatorMove.y, rawInput.y, 4f, Time.deltaTime);
            
            bool idle = Mathf.Approximately(0f, normInput.magnitude);
            animator.SetBool(Moving, !idle);
            
            smoothIsMoving = FPSAnimLib.ExpDecay(smoothIsMoving, idle ? 0f : 1f, curveLocomotionSmoothing, 
                Time.deltaTime);
            
            animator.SetFloat(Velocity, Mathf.Clamp01(smoothIsMoving));
            animator.SetFloat(MoveX, _smoothAnimatorMove.x);
            animator.SetFloat(MoveY, _smoothAnimatorMove.y);
            
            Vector3 move = transform.right * moveX + transform.forward * moveY;

            if (_isInAir)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                verticalVelocity = Mathf.Max(-30f, verticalVelocity);
            }
            
            move.y = verticalVelocity;
            controller.Move(move * speed * Time.deltaTime);

            bool bWasInAir = _isInAir;
            _isInAir = !controller.isGrounded;
            animator.SetBool(InAir, _isInAir);

            if (!_isInAir)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalVelocity = jumpHeight;
                }
            }

            if (bWasInAir != _isInAir)
            {
                if (_isInAir)
                {
                    SprintReleased();
                }
                else
                {
                    verticalVelocity = -0.5f;
                }

                slotLayer.PlayMotion(_isInAir ? onJumpMotionAsset : onLandedMotionAsset);
            }
            
        }

        System.Collections.IEnumerator Cooldown(RectTransform rt, float time)
        {
            Vector2 initSize = new Vector2(rt.sizeDelta.x, 0f);
            Vector2 targetSize = new Vector2(rt.sizeDelta.x, 100f);

            while (rt.sizeDelta.y < 100f)
            {
                Vector2 smoothedSize = Vector2.Lerp(initSize, targetSize, time);
                GetComponent<RectTransform>().sizeDelta = smoothedSize;
            }

            if (rt.sizeDelta.y >= 100f) yield return null;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(0);
            }

            if (!disableInput)
            {
                UpdateActionInput();
                UpdateLookInput();
                UpdateFiring();
                UpdateMovement();
            }
            
            UpdateAnimController();
        }

        private Quaternion _smoothBodyCam;

        public void UpdateCameraRotation()
        {
            Vector2 finalInput = new Vector2(_playerInput.x, _playerInput.y);
            (Quaternion, Vector3) cameraTransform =
                (transform.rotation * Quaternion.Euler(finalInput.y, finalInput.x, 0f),
                    firstPersonCamera.position);
            

            cameraHolder.rotation = cameraTransform.Item1;
            cameraHolder.position = cameraTransform.Item2;

            mainCamera.rotation = cameraHolder.rotation * Quaternion.Euler(_freeLookInput.y, _freeLookInput.x, 0f);
        }
    }
}