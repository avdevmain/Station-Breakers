// Designed by Kinemation, 2023

using System;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using Kinemation.FPSFramework.Runtime.FPSAnimator;
using UnityEngine;
using PlayMaker;
using TMPro;
using Obscure.SDC;

namespace Demo.Scripts.Runtime
{
    public enum OverlayType
    {
        Default,
        Pistol,
        Rifle
    }
    
    public class Weapon : FPSAnimWeapon
    {
        public Sprite weaponSprite;
        public float physRecoilPower;
        public Transform physRecoilPoint;
        public TMP_Text ammoDisplay;
        public Crosshair ch;
        public float chMinSize;
        public float chMaxSize;
        public float chIncrement;
        public float chReduceSpeed;
        
        public AnimSequence reloadClip;
        public AnimSequence grenadeClip;
        public OverlayType overlayType;

        [HideInInspector] public int stagedReloadSegment = 0;

        [SerializeField] private List<Transform> scopes;
        //[SerializeField] private GameObject magBone;
        
        private PlayMakerFSM weaponFSM;
        
        private Animator _animator;
        private int _scopeIndex;

        private int _stagedSegments;
        public int _currentAmmo;

        public void ReduceAmmo()
        {
            _currentAmmo -= ammoPerShot;
            
            if (ammoDisplay!=null) ammoDisplay.text = _currentAmmo.ToString();
        }

        public bool HasEnoughAmmo()
        {
            if (_currentAmmo > ammoPerShot-1) return true;
            
            if (ammoDisplay!=null) ammoDisplay.color = Color.red;
            GetComponent<AudioSource>().Play();
            
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
        
            
        protected void Start()
        {
            weaponFSM = PlayMakerFSM.FindFsmOnGameObject(this.gameObject, "Weapon");
            _animator = GetComponentInChildren<Animator>();
            RestoreAmmo();
            /*
            var animEvents = reloadClip.clip.events;

            foreach (var animEvent in animEvents)
            {
                if (animEvent.functionName.Equals("RefreshStagedState"))
                {
                    _stagedSegments++;
                }
            }
            
            _animator.Play("Empty");
            */
        }
        

        // Returns a normalized reload time ratio
        public float GetReloadTime()
        {
            if (_stagedSegments == 0) return 0f;

            return (float) stagedReloadSegment / _stagedSegments;
        }

        public override Transform GetAimPoint()
        {
            _scopeIndex++;
            _scopeIndex = _scopeIndex > scopes.Count - 1 ? 0 : _scopeIndex;
            return scopes[_scopeIndex];
        }

 
        
        
        
        public void OnFire()
        {
            weaponFSM.SendEvent("OnFire");
            if (_animator == null)
            {
                return;
            }
            
            _animator.Play("Fire", 0, 0f);
            
        }

        public void Reload()
        {
            if (_currentAmmo == ammoInMag) return;
            
            Debug.LogWarning("There has to be reload animation (TO-DO)");
            RestoreAmmo();
            
            if (_animator == null)
            {
                return;
            }
            
            _animator.Rebind();
            _animator.Play("Reload", 0);
            
        }

        public void UpdateMagVisibility(bool bVisible)
        {
            //if (magBone == null) return;

            //magBone.transform.localScale = bVisible ? Vector3.one : Vector3.zero;
        }
    }
}