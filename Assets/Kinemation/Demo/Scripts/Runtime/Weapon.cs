// Designed by Kinemation, 2023

using Kinemation.FPSFramework.Runtime.Camera;
using Kinemation.FPSFramework.Runtime.Core.Types;
using Kinemation.FPSFramework.Runtime.FPSAnimator;

using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Obscure.SDC;
using Unity.Mathematics;
using UnityEditor.Search;

namespace Kinemation.Demo.Scripts.Runtime
{
    public enum OverlayType
    {
        Default,
        Pistol,
        Rifle
    }
    
    public class Weapon : FPSAnimWeapon
    {
        [Header("Animations")]
        public AnimSequence reloadClip;
        public AnimSequence grenadeClip;
        public AnimSequence fireClip;
        public OverlayType overlayType;
        
        [Header("Aiming")]
        public bool canAim = true;
        [SerializeField] private List<Transform> scopes;
        
        [Header("Recoil")]
        public RecoilPattern recoilPattern;
        public FPSCameraShake cameraShake;
        
        private Animator _animator;
        private int _scopeIndex;

        private PlayMakerFSM weaponFSM;
        private int _currentAmmo;
        public float physRecoilPower;
        public TMP_Text ammoDisplay;

        private AudioSource shootSource;

        [Header("Crosshair")] 
        [SerializeField] public GameObject ch_prefab;
        [HideInInspector] public Crosshair ch;
        [SerializeField] public Vector2 ch_increment = new Vector2(10, 10);
        [SerializeField] public Vector2 ch_minSize = new Vector2(20, 20);
        [SerializeField] public Vector2 ch_maxSize = new Vector2(100, 100);
        [SerializeField] public float ch_reduceTime = 0.01f;


        [Header("Shooting")] 
        [SerializeField] public float projectile_speed;
        [SerializeField] public GameObject projectile_prefab;
        [SerializeField] public Transform projectile_spawn;

        [SerializeField] public float projectile_hor_disp = 0f;
        [SerializeField] public float projectile_ver_disp = 0f;
        
        [SerializeField] public GameObject muzzle_prefab;
        [SerializeField] public Transform muzzle_spawn;
        
        [SerializeField] public float damage;
        
        protected void Start()
        {
            weaponFSM = PlayMakerFSM.FindFsmOnGameObject(gameObject, "Weapon");
            _animator = GetComponentInChildren<Animator>();
            RestoreAmmo();

            shootSource = GetComponent<AudioSource>();
        }

        public override Transform GetAimPoint()
        {
            _scopeIndex++;
            _scopeIndex = _scopeIndex > scopes.Count - 1 ? 0 : _scopeIndex;
            return scopes[_scopeIndex];
        }

        public void CreateProjectile()
        {
            var proj = Instantiate(projectile_prefab, projectile_spawn.position, projectile_spawn.rotation);
            proj.transform.localScale *= 0.5f;
            
            proj.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * projectile_speed, ForceMode.Impulse);
            
        }
        
        public void OnFire()
        {
            //weaponFSM.SendEvent("OnFire");
            //CreateProjectile();
            if (ch)
            {
                Vector2 ch_newSize = ch.GetSize() + ch_increment;
                if (ch_newSize.x > ch_maxSize.x) ch_newSize = ch_maxSize;
                ch.SetSizeNoSmooth(ch_newSize);
            }

            if (_animator == null)
            {
                return;
            }
            
            _animator.Play("Fire", 0, 0f);
        }
        
        
        public void Reload()
        {
            Debug.LogWarning("There has to be reload animation (TO-DO)");
            RestoreAmmo();
            if (_animator == null)
            {
                return;
            }
            
            _animator.Rebind();
            _animator.Play("Reload", 0);
        }
        
        public void ReduceAmmo()
        {
            _currentAmmo -= ammoPerShot;
            
            if (ammoDisplay!=null) ammoDisplay.text = _currentAmmo.ToString();
        }

        public bool HasEnoughAmmo()
        {
            if (_currentAmmo > ammoPerShot-1) return true;
            
            if (ammoDisplay!=null) ammoDisplay.color = Color.red;
            
            if (shootSource) shootSource.Play();

            return false;
        }

        public void RestoreAmmo(int value)
        {
            _currentAmmo += value;

            if (_currentAmmo > ammoInMag)
                _currentAmmo = ammoInMag;


            if (ammoDisplay != null)
            {
                ammoDisplay.text = _currentAmmo.ToString();
                ammoDisplay.color = Color.white;
            }
        }

        public void RestoreAmmo()
        {
            RestoreAmmo(ammoInMag);
        }

        private void UpdateCrosshair()
        {
            if (!ch) return;
            ch.SetSize(ch_minSize, ch_reduceTime);
        }
        

        private void Update()
        {
            UpdateCrosshair();
        }

    }
}