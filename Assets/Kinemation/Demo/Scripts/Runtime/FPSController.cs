// Designed by KINEMATION, 2023

using System;
using Kinemation.FPSFramework.Runtime.Camera;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using Kinemation.FPSFramework.Runtime.Layers;
using Kinemation.FPSFramework.Runtime.Recoil;

using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using Obscure.SDC;


namespace Kinemation.Demo.Scripts.Runtime
{

    public enum FPSAimState
    {
        None,
        Ready,
        Aiming,
        PointAiming
    }

    public enum FPSActionState
    {
        None,
        Reloading,
        WeaponChange
    }

    // An example-controller class
    public class FPSController : FPSAnimController
    {

        [TabGroup("Tab Group 1", "Animation")]
        [BoxGroup("Tab Group 1/Animation/General")]
        [SerializeField] private Animator animator;
        
        [BoxGroup("Tab Group 1/Animation/Turn In Place")]
        [SerializeField] private float turnInPlaceAngle;
        [BoxGroup("Tab Group 1/Animation/Turn In Place")]
        [SerializeField] private AnimationCurve turnCurve = new AnimationCurve(new Keyframe(0f, 0f));
        [BoxGroup("Tab Group 1/Animation/Turn In Place")]
        [SerializeField] private float turnSpeed = 1f;
        
        [BoxGroup("Tab Group 1/Animation/Leaning")]
        [SerializeField] private float smoothLeanStep = 1f;
        [BoxGroup("Tab Group 1/Animation/Leaning")]
        [SerializeField, Range(0f, 1f)] private float startLean = 1f;
        
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation aimMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation leanMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation crouchMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation unCrouchMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation onJumpMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation onLandedMotionAsset;
        [BoxGroup("Tab Group 1/Animation/Dynamic Motions")]
        [SerializeField] private IKAnimation onStartStopMoving;

        // Animation Layers
        [SerializeField] [HideInInspector] private LookLayer lookLayer;
        [SerializeField] [HideInInspector] private AdsLayer adsLayer;
        [SerializeField] [HideInInspector] private SwayLayer swayLayer;
        [SerializeField] [HideInInspector] private LocomotionLayer locoLayer;
        [SerializeField] [HideInInspector] private SlotLayer slotLayer;
        [SerializeField] [HideInInspector] private WeaponCollision collisionLayer;
        [SerializeField] [HideInInspector] private ShootLayer shootLayer;
        // Animation Layers
        

        
        [TabGroup("Tab Group 1", "Controller")]
        [BoxGroup("Tab Group 1/Controller/General")]
        [SerializeField] private float timeScale = 1f;
        [BoxGroup("Tab Group 1/Controller/General")]
        [SerializeField, Min(0f)] private float equipDelay = 0f;
        
        [BoxGroup("Tab Group 1/Controller/Camera")]
        [SerializeField] private Transform mainCamera;

        [BoxGroup("Tab Group 1/Controller/Camera")]
        [SerializeField] private Transform cameraHolder;
        [BoxGroup("Tab Group 1/Controller/Camera")]
        [SerializeField] private Transform firstPersonCamera;
        [BoxGroup("Tab Group 1/Controller/Camera")]
        [SerializeField] private float sensitivity;
        [BoxGroup("Tab Group 1/Controller/Camera")]
        [SerializeField] private Vector2 freeLookAngle;
        
        [BoxGroup("Tab Group 1/Controller/Movement")]
        [SerializeField] private FPSMovement movementComponent;
        
        [TabGroup("Tab Group 1", "Weapon")]
        [BoxGroup("Tab Group 1/Weapon/General")]
        [SerializeField] private List<Weapon> weapons;
        [BoxGroup("Tab Group 1/Weapon/General")]
        public Transform weaponBone;
        
        
        [TabGroup("Tab Group 1", "Dubspac3d")]
        [BoxGroup("Tab Group 1/Dubspac3d/General")]
        public bool spaced;
        [BoxGroup("Tab Group 1/Dubspac3d/General")]
        [SerializeField] private PlayMakerFSM fsm_interactions;

        [BoxGroup("Tab Group 1/Dubspac3d/General")] 
        [SerializeField] private float rollSpeed = 1f;


        [BoxGroup("Tab Group 1/Animation/Turn In Place")]
        [SerializeField] private float turnInPlaceAngleY;
        [BoxGroup("Tab Group 1/Animation/Turn In Place")]
        [SerializeField] private Transform turnYConsiderRotation, pivotPoint;
        
        [TabGroup("Tab Group HUD", "General")]
        [SerializeField] private GameObject ch_parent;
                /*
        [Tab("HUD")] 
        [Header("Task")]
        [SerializeField] private GameObject obj_task;
        [SerializeField] private Image sprite_task;
        [SerializeField] private TMP_Text text_task_name, text_task_details;
        
        [Header("Hint_1")]
        [SerializeField] private GameObject obj_hint1;
        [SerializeField] private Image sprite_hint1;
        [SerializeField] private TMP_Text text_hint1_button, text_hint1_details;
        [SerializeField] private RectTransform rt_hint1_cooldown;
        
        [Header("Hint_2")]
        [SerializeField] private GameObject obj_hint2;
        [SerializeField] private Image sprite_hint2;
        [SerializeField] private TMP_Text text_hint2_button, text_hint2_details;
        [SerializeField] private RectTransform rt_hint2_cooldown;

        [Header("Health")] 
        [SerializeField] private GameObject obj_health;
        [SerializeField] private Slider slider_health;
        [SerializeField] private TMP_Text text_health;

        [Header("Grenade")] 
        [SerializeField] private GameObject obj_grenade;
        [SerializeField] private Image sprite_grenade;
        [SerializeField] private TMP_Text text_grenade_amount, text_grenade_button;
        [SerializeField] private RectTransform rt_grenade_cooldown;

        
        [Header("Medkit")]
        [SerializeField] private GameObject obj_medkit;
        [SerializeField] private Image sprite_medkit;
        [SerializeField] private TMP_Text text_medkit_amount, text_medkit_button;
        [SerializeField] private RectTransform rt_medkit_cooldown;


        [Header("Jetpack")]
        [SerializeField] private GameObject obj_jetpack;
        [SerializeField] private Image sprite_jetpack;
        [SerializeField] private Slider slider_jetpack;
        
        [Header("Equipment")]
        [SerializeField] private GameObject obj_equipment;
        
        [Header("Weapon current")] 
        [SerializeField] private Image sprite_wpn_curr;
        [SerializeField] private TMP_Text text_wpn_curr_ammo, text_wpn_curr_ammo_max;
        [SerializeField] private RectTransform rt_wpn_curr_cooldown;
        
        [Header("Weapon other")] 
        [SerializeField] private Image sprite_wpn_oth;
        [SerializeField] private TMP_Text text_wpn_oth_button;
        
        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot1, obj_empty_slot1, obj_full_slot1;
        [SerializeField] private Image sprite_slot1;
        [SerializeField] private TMP_Text text_slot1_amount, text_slot1_button;

        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot2, obj_empty_slot2, obj_full_slot2;
        [SerializeField] private Image sprite_slot2;
        [SerializeField] private TMP_Text text_slot2_amount, text_slot2_button;

        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot3, obj_empty_slot3, obj_full_slot3;
        [SerializeField] private Image sprite_slot3;
        [SerializeField] private TMP_Text text_slot3_amount, TMP_Text text_slot3_button;
        
        [Header("Inventory Slot 1")]
        [SerializeField] private GameObject obj_slot4, obj_empty_slot4, obj_full_slot4;
        [SerializeField] private Image sprite_slot4;
        [SerializeField] private TMP_Text text_slot4_amount, text_slot4_button;
        */

        private Rigidbody rb;
        

        
        
        private Vector2 _playerInput;

        // Used for free-look
        private Vector2 _freeLookInput;

        private int _index;
        private int _lastIndex;
        
        private int _bursts;
        private bool _freeLook;
        
        private FPSAimState aimState;
        private FPSActionState actionState;

        private float smoothCurveAlpha = 0f;
        
        private static readonly int Crouching = Animator.StringToHash("Crouching");
        private static readonly int OverlayType = Animator.StringToHash("OverlayType");
        private static readonly int TurnRight = Animator.StringToHash("TurnRight");
        private static readonly int TurnLeft = Animator.StringToHash("TurnLeft");
        private static readonly int UnEquip = Animator.StringToHash("Unequip");

        private Vector2 _controllerRecoil;
        private float _recoilStep;
        private bool _isFiring;

        private bool _isUnarmed;

        private void InitLayers()
        {
            InitAnimController();
            
            animator = GetComponentInChildren<Animator>();
            lookLayer = GetComponentInChildren<LookLayer>();
            adsLayer = GetComponentInChildren<AdsLayer>();
            locoLayer = GetComponentInChildren<LocomotionLayer>();
            swayLayer = GetComponentInChildren<SwayLayer>();
            slotLayer = GetComponentInChildren<SlotLayer>();
            collisionLayer = GetComponentInChildren<WeaponCollision>();
            shootLayer = GetComponentInChildren<ShootLayer>();
        }

        private bool HasActiveAction()
        {
            return actionState != FPSActionState.None;
        }

        private bool IsAiming()
        {
            return aimState is FPSAimState.Aiming or FPSAimState.PointAiming;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            moveRotation = transform.rotation;

            movementComponent = GetComponent<FPSMovement>();
            
            movementComponent.onStartMoving.AddListener(() => slotLayer.PlayMotion(onStartStopMoving));
            movementComponent.onStopMoving.AddListener(() => slotLayer.PlayMotion(onStartStopMoving));
            
            movementComponent.onCrouch.AddListener(OnCrouch);
            movementComponent.onUncrouch.AddListener(OnUncrouch);
            
            movementComponent.onJump.AddListener(OnJump);
            movementComponent.onLanded.AddListener(OnLand);
            
            movementComponent.onSprintStarted.AddListener(OnSprintStarted);
            movementComponent.onSprintEnded.AddListener(OnSprintEnded);
            
            movementComponent.sprintCondition += () => !HasActiveAction();

            actionState = FPSActionState.None;

            rb = GetComponent<Rigidbody>();
            fsm_interactions.Fsm.GetFsmGameObject("camera").Value = mainCamera.gameObject;
                
            InitLayers();
            EquipWeapon();
            
            SetupGravityState(!spaced); // TEMP, TBD IN SCENES (maps)
        }
        
        private void UnequipWeapon()
        {
            Destroy(ch_parent.transform.GetChild(0).gameObject);
            
            DisableAim();

            actionState = FPSActionState.WeaponChange;
            animator.CrossFade(UnEquip, 0.1f);
        }

        public void ResetActionState()
        {
            actionState = FPSActionState.None;
        }

        public void RefreshStagedState()
        {
        }
        
        public void ResetStagedState()
        {
        }

        private void EquipWeapon()
        {
            if (weapons.Count == 0) return;

            weapons[_lastIndex].gameObject.SetActive(false);
            var gun = weapons[_index];

            _bursts = gun.burstAmount;
            
            StopAnimation(0.1f);
            InitWeapon(gun);
            gun.gameObject.SetActive(true);
            gun.ch = Instantiate(gun.ch_prefab, ch_parent.transform).GetComponent<Crosshair>();

            animator.SetFloat(OverlayType, (float) gun.overlayType);
            actionState = FPSActionState.None;

            //weaponData.muzzle_spawn = gun.muzzle_spawn;
            //weaponData.projectile_spawn = gun.projectile_spawn;
            //weaponData.projectile_prefab = gun.projectile_prefab;
            //weaponData.muzzle_prefab = gun.muzzle_prefab;
            //weaponData.projectile_speed = gun.projectile_speed;

            SetupShootLayer(gun);
        }

        private void SetupShootLayer(Weapon gun)
        {
            shootLayer.muzzle_spawn = gun.muzzle_spawn;
            shootLayer.projectile_spawn = gun.projectile_spawn;
            shootLayer.projectile_prefab = gun.projectile_prefab;
            shootLayer.muzzle_prefab = gun.muzzle_prefab;
            shootLayer.projectile_speed = gun.projectile_speed;
            shootLayer.projectile_damage = gun.damage;
            shootLayer.projectile_hor_disp = gun.projectile_hor_disp;
            shootLayer.projectile_ver_disp = gun.projectile_ver_disp;
            shootLayer.projectile_damage = gun.damage;
        }
        private void EnableUnarmedState()
        {
            if (weapons.Count == 0) return;
            
            weapons[_index].gameObject.SetActive(false);
            animator.SetFloat(OverlayType, 0);
        }
        
        private void ChangeWeapon_Internal()
        {
            if (movementComponent.PoseState == FPSPoseState.Prone) return;
            
            if (HasActiveAction()) return;
            
            OnFireReleased();
            
            int newIndex = _index;
            newIndex++;
            if (newIndex > weapons.Count - 1)
            {
                newIndex = 0;
            }

            _lastIndex = _index;
            _index = newIndex;

            UnequipWeapon();
            Invoke(nameof(EquipWeapon), equipDelay);
        }

        private void DisableAim()
        {
            if (!GetGun().canAim) return;
            
            aimState = FPSAimState.None;
            OnInputAim(false);
            
            adsLayer.SetAds(false);
            adsLayer.SetPointAim(false);
            //swayLayer.SetFreeAimEnable(true);
            swayLayer.SetLayerAlpha(1f);
            slotLayer.PlayMotion(aimMotionAsset);
        }

        public void ToggleAim()
        {
            if (!GetGun().canAim) return;
            
            if (!IsAiming())
            {
                aimState = FPSAimState.Aiming;
                OnInputAim(true);
                
                adsLayer.SetAds(true);
                //swayLayer.SetFreeAimEnable(false);
                swayLayer.SetLayerAlpha(0.5f);
                slotLayer.PlayMotion(aimMotionAsset);
            }
            else
            {
                DisableAim();
            }

            recoilComponent.isAiming = IsAiming();
        }

        public void ChangeScope()
        {
            InitAimPoint(GetGun());
        }

        private void Fire()
        {
            if (HasActiveAction()) return;

            Weapon wpn = GetGun();

            if (!wpn.HasEnoughAmmo()) 
            {
                Debug.Log("no ammo");
                
                OnFireReleased();
                return;
                //NO AMMO ADDITIONAL LOGIC
            }

            shootLayer.shooting = true;
           // shootLayer.Invoke("SpawnProjectile", 0f);
            
            wpn.OnFire();
            
            wpn.ReduceAmmo();

            PlayAnimation(wpn.fireClip);
            
            PlayCameraShake(wpn.cameraShake);

            if (wpn.recoilPattern != null)
            {
                float aimRatio = IsAiming() ? wpn.recoilPattern.aimRatio : 1f;
                float hRecoil = Random.Range(wpn.recoilPattern.horizontalVariation.x,
                    wpn.recoilPattern.horizontalVariation.y);
                _controllerRecoil += new Vector2(hRecoil, _recoilStep) * aimRatio;
            }
            
            if (recoilComponent == null || wpn.weaponAsset.recoilData == null)
            {
                return;
            }
            
            recoilComponent.Play();
            
            if (recoilComponent.fireMode == FireMode.Burst)
            {
                if (_bursts == 0)
                {
                    OnFireReleased();
                    return;
                }
                
                _bursts--;
            }

            if (recoilComponent.fireMode == FireMode.Semi)
            {
                _isFiring = false;
                return;
            }
            
            Invoke(nameof(Fire), 60f / wpn.fireRate);
            _recoilStep += wpn.recoilPattern.acceleration;
            
            
            if (spaced)
            {
                //var worldRecoilVector =  wpn.physRecoilPoint.transform.TransformDirection(0, 0, -1f * wpn.physRecoilPower);
                //rb.AddForce(worldRecoilVector, ForceMode.Impulse);
                
                rb.AddRelativeForce(Vector3.back*wpn.physRecoilPower, ForceMode.Impulse);
            }
        }

        private void OnFirePressed()
        {
            if (weapons.Count == 0 || HasActiveAction()) return;

            _bursts = GetGun().burstAmount - 1;

            if (GetGun().recoilPattern != null)
            {
                _recoilStep = GetGun().recoilPattern.step;
            }
            
            _isFiring = true;
            Fire();
        }

        private Weapon GetGun()
        {
            if (weapons.Count == 0) return null;
            
            return weapons[_index];
        }

        private void OnFireReleased()
        {
            if (weapons.Count == 0) return;

            if (recoilComponent != null)
            {
                recoilComponent.Stop();
            }

            _recoilStep = 0f;
            _isFiring = false;
            CancelInvoke(nameof(Fire));
        }

        private void OnSlideStarted()
        {
            lookLayer.SetLayerAlpha(0.3f);
        }

        private void OnSlideEnded()
        {
            lookLayer.SetLayerAlpha(1f);
        }

        private void OnSprintStarted()
        {
            OnFireReleased();
            lookLayer.SetLayerAlpha(0.5f);
            adsLayer.SetLayerAlpha(0f);
            locoLayer.SetReadyWeight(0f);
            
            aimState = FPSAimState.None;

            if (recoilComponent != null)
            {
                recoilComponent.Stop();
            }
        }

        private void OnSprintEnded()
        {
            lookLayer.SetLayerAlpha(1f);
            adsLayer.SetLayerAlpha(1f);
        }

        private void OnCrouch()
        {
            lookLayer.SetPelvisWeight(0f);
            animator.SetBool(Crouching, true);
            slotLayer.PlayMotion(crouchMotionAsset);
        }

        private void OnUncrouch()
        {
            lookLayer.SetPelvisWeight(1f);
            animator.SetBool(Crouching, false);
            slotLayer.PlayMotion(unCrouchMotionAsset);
        }

        private void OnJump()
        {
            slotLayer.PlayMotion(onJumpMotionAsset);
        }

        private void OnLand()
        {
            slotLayer.PlayMotion(onLandedMotionAsset);
        }

        private void TryReload()
        {
            if (HasActiveAction()) return;

            var reloadClip = GetGun().reloadClip;

            if (reloadClip == null) return;
            
            OnFireReleased();
            
            PlayAnimation(reloadClip);
            GetGun().Reload();
            actionState = FPSActionState.Reloading;
        }

        private void TryGrenadeThrow()
        {
            if (HasActiveAction()) return;

            if (GetGun().grenadeClip == null) return;
            
            OnFireReleased();
            DisableAim();
            PlayAnimation(GetGun().grenadeClip);
            actionState = FPSActionState.Reloading;
        }

        private bool _isLeaning;

        private void UpdateActionInput()
        {
            smoothCurveAlpha = Mathf.Lerp(smoothCurveAlpha, IsAiming() ? 0.5f : 1f, 
                FPSAnimLib.ExpDecayAlpha(10f, Time.deltaTime));
            
            animator.SetLayerWeight(3, smoothCurveAlpha);

            if (movementComponent.MovementState == FPSMovementState.Sprinting)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                SetupGravityState(spaced);
            }

            
            if (Input.GetKeyDown(KeyCode.Alpha0) && !HasActiveAction())
            {
                _isUnarmed = !_isUnarmed;

                if (_isUnarmed)
                {
                    UnequipWeapon();
                    Invoke(nameof(EnableUnarmedState), equipDelay);
                }
                else
                {
                    //weapons[_index].gameObject.SetActive(true);
                    EquipWeapon();
                    //animator.SetFloat(OverlayType, (float) GetGun().overlayType);
                }
                
                lookLayer.SetLayerAlpha(_isUnarmed ? 0.3f : 1f);
            }

            
            if (_isUnarmed) return;
            
            if (aimState != FPSAimState.Ready)
            {
                bool wasLeaning = _isLeaning;
                
                bool rightLean = Input.GetKey(KeyCode.E);
                bool leftLean = Input.GetKey(KeyCode.Q);

                _isLeaning = rightLean || leftLean;
                
                if (_isLeaning != wasLeaning)
                {
                    slotLayer.PlayMotion(leanMotionAsset);
                    charAnimData.SetLeanInput(wasLeaning ? 0f : rightLean ? -startLean : startLean);
                }

                if (_isLeaning)
                {
                    float leanValue = Input.GetAxis("Mouse ScrollWheel") * smoothLeanStep;
                    charAnimData.AddLeanInput(leanValue);
                }
                
                if (Input.GetKey(KeyCode.Q))
                {
                    charAnimData.leanDirection = 1;
                    rb.AddRelativeTorque(Vector3.forward* rb.mass/500 * rollSpeed,ForceMode.Impulse);
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    charAnimData.leanDirection = -1;
                    rb.AddRelativeTorque(Vector3.back * rb.mass/500 * rollSpeed, ForceMode.Impulse);
                }

               // if (Input.GetKeyDown(KeyCode.Mouse1))
               // {
                //    ToggleAim();
                //}
            }
        }

        private Quaternion desiredRotation, desiredRotationY;
        private Quaternion moveRotation, moveRotationY;
        private float turnProgress = 1f, turnProgressY = 1f;
        private bool isTurning = false, isTurningY = false;

        private void TurnInPlace()
        {
            float turnInput = _playerInput.x;
            _playerInput.x = Mathf.Clamp(_playerInput.x, -90f, 90f);
            turnInput -= _playerInput.x;

            float sign = Mathf.Sign(_playerInput.x);
            if (Mathf.Abs(_playerInput.x) > turnInPlaceAngle)
            {
                if (!isTurning)
                {
                    turnProgress = 0f;
                    
                    animator.ResetTrigger(TurnRight);
                    animator.ResetTrigger(TurnLeft);
                    
                    animator.SetTrigger(sign > 0f ? TurnRight : TurnLeft);
                }
                
                isTurning = true;
            }

            transform.rotation *= Quaternion.Euler(0f, turnInput, 0f);
            
            float lastProgress = turnCurve.Evaluate(turnProgress);
            turnProgress += Time.deltaTime * turnSpeed;
            turnProgress = Mathf.Min(turnProgress, 1f);
            
            float deltaProgress = turnCurve.Evaluate(turnProgress) - lastProgress;

            _playerInput.x -= sign * turnInPlaceAngle * deltaProgress;
            
            transform.rotation *= Quaternion.Slerp(Quaternion.identity,
                Quaternion.Euler(0f, sign * turnInPlaceAngle, 0f), deltaProgress);
            
            if (Mathf.Approximately(turnProgress, 1f) && isTurning)
            {
                isTurning = false;
            }
        }
        
        private void TurnInPlaceSpacedX()
        {
            float turnInput = _playerInput.x;
            
            _playerInput.x = Mathf.Clamp(_playerInput.x, -1*turnInPlaceAngle, turnInPlaceAngle);

            turnInput -= _playerInput.x; ///////

            float sign = Mathf.Sign(_playerInput.x);
            
            if (_playerInput.x>turnInPlaceAngle)
            {
                if (!isTurning)
                {
                    turnProgress = 0f;
                    
                    animator.ResetTrigger(TurnRight);
                    animator.ResetTrigger(TurnLeft);
                    
                    animator.SetTrigger(sign > 0f ? TurnRight : TurnLeft);
                }
                isTurning = true; 
            }
            
            transform.rotation *= Quaternion.Euler(0f, turnInput, 0f); /////////
            
            rb.AddRelativeTorque(Vector3.up * turnInput * rb.mass/750,ForceMode.Impulse);
            
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
            
        }
        
        
        private void TurnInPlaceSpacedY()
        {

            float turnInput = _playerInput.y;
            _playerInput.y = Mathf.Clamp(_playerInput.y, -1*turnInPlaceAngleY, turnInPlaceAngle);
            turnInput -= _playerInput.y;

            float sign = Mathf.Sign(_playerInput.y);
            if (_playerInput.y>turnInPlaceAngleY)
            {
                if (!isTurningY)
                {
                    turnProgressY = 0f;
                }
                isTurningY = true;
            }

            transform.rotation *= Quaternion.Euler(turnInput, 0f, 0f);
            rb.AddRelativeTorque(Vector3.right * turnInput * rb.mass/750,ForceMode.Impulse);
            
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
        }



        private float _jumpState = 0f;

        private void UpdateLookInput()
        {
            float deltaMouseX = Input.GetAxis("Mouse X") * sensitivity;
            float deltaMouseY = -Input.GetAxis("Mouse Y") * sensitivity;
            
            _playerInput.x += deltaMouseX;
            _playerInput.y += deltaMouseY;
            
            float proneWeight = animator.GetFloat("ProneWeight");
            Vector2 pitchClamp = Vector2.Lerp(new Vector2(-90f, 90f), new Vector2(-30, 0f), proneWeight);
            
            _playerInput.y = Mathf.Clamp(_playerInput.y, pitchClamp.x, pitchClamp.y);
            

            if (!spaced)
            {
                moveRotation *= Quaternion.Euler(0f, deltaMouseX, 0f);
                TurnInPlace();
                
                float moveWeight = Mathf.Clamp01(movementComponent.AnimatorVelocity.magnitude);
                transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, moveWeight);
                transform.rotation = Quaternion.Slerp(transform.rotation, moveRotation, _jumpState);
                _playerInput.x *= 1f - moveWeight;
                _playerInput.x *= 1f - _jumpState;
                
                _jumpState = Mathf.Lerp(_jumpState, movementComponent.IsInAir() ? 1f : 0f,
                    FPSAnimLib.ExpDecayAlpha(10f, Time.deltaTime));
            }
            else
            {
                moveRotation *= Quaternion.Euler(deltaMouseY, deltaMouseX, 0f);
                TurnInPlaceSpacedX();
                TurnInPlaceSpacedY();
            }
            
            charAnimData.SetAimInput(_playerInput);
            charAnimData.AddDeltaInput(new Vector2(deltaMouseX, charAnimData.deltaAimInput.y));
        }

        private Quaternion lastRotation;

        private void OnDrawGizmos()
        {
            if (weaponBone != null)
            {
                Gizmos.DrawWireSphere(weaponBone.position, 0.03f);
            }
        }

        private Vector2 _cameraRecoilOffset;

        private void UpdateRecoil()
        {
            if (Mathf.Approximately(_controllerRecoil.magnitude, 0f)
                && Mathf.Approximately(_cameraRecoilOffset.magnitude, 0f))
            {
                return;
            }
            
            float smoothing = 8f;
            float restoreSpeed = 8f;
            float cameraWeight = 0f;

            if (GetGun().recoilPattern != null)
            {
                smoothing = GetGun().recoilPattern.smoothing;
                restoreSpeed = GetGun().recoilPattern.cameraRestoreSpeed;
                cameraWeight = GetGun().recoilPattern.cameraWeight;
            }
            
            _controllerRecoil = Vector2.Lerp(_controllerRecoil, Vector2.zero,
                FPSAnimLib.ExpDecayAlpha(smoothing, Time.deltaTime));

            _playerInput += _controllerRecoil * Time.deltaTime;
            
            Vector2 clamp = Vector2.Lerp(Vector2.zero, new Vector2(90f, 90f), cameraWeight);
            _cameraRecoilOffset -= _controllerRecoil * Time.deltaTime;
            _cameraRecoilOffset = Vector2.ClampMagnitude(_cameraRecoilOffset, clamp.magnitude);

            if (_isFiring) return;

            _cameraRecoilOffset = Vector2.Lerp(_cameraRecoilOffset, Vector2.zero,
                FPSAnimLib.ExpDecayAlpha(restoreSpeed, Time.deltaTime));
        }

        private void Update()
        {
            Time.timeScale = timeScale;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit(0);
            }
            
            UpdateActionInput();
            UpdateLookInput();
            UpdateRecoil();

            charAnimData.moveInput = movementComponent.AnimatorVelocity;
            //todo: add recoil here to the input
            UpdateAnimController();
        }
        
        public void UpdateCameraRotation()
        {
            Vector2 input = _playerInput;
            input += _cameraRecoilOffset;
            
            (Quaternion, Vector3) cameraTransform =
                (transform.rotation * Quaternion.Euler(input.y, input.x, 0f),
                    firstPersonCamera.position);

            cameraHolder.rotation = cameraTransform.Item1;
            cameraHolder.position = cameraTransform.Item2;

            mainCamera.rotation = cameraHolder.rotation * Quaternion.Euler(_freeLookInput.y, _freeLookInput.x, 0f);
        }
        


        /// ////////////////////////////////////////////////////////

        private void SetupGravityState(bool isThereAnyGravity)
        {
            spaced = !isThereAnyGravity;
            rb.isKinematic = isThereAnyGravity;
            movementComponent.SetupGravityState(isThereAnyGravity);

            lookLayer.leanAmount = spaced ? 0 : 40;
            turnInPlaceAngle = spaced ? 30 : 85;

        }

        public void SwapGravityState()
        {
            Debug.Log("Aboba");
            SetupGravityState(spaced);
        }
        
        ///////////////////////////////////////////////////////////
        public void ShootPressed(InputAction.CallbackContext context)
        {
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
            if (spaced) return;
            if (context.performed)
            { 
                OnSprintStarted();
            }
            else
            {
                OnSprintEnded();
            }
            
        }

        public void ReloadPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryReload();
            }
            
        }
        
        public void GrenadePressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryGrenadeThrow();
            }
            
        }
        
        public void HealthpackPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //TO DO
            }
            
        }

        public void SwapWeaponPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ChangeWeapon_Internal();
            }
        }

        /// <summary>
        /// Used in FSM
        /// </summary>
        /// <returns></returns>

        public GameObject GetCamera()
        {
            return mainCamera.gameObject;
        }
        
        
        
    }
}