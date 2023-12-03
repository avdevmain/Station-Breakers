using Kinemation.FPSFramework.Runtime.Core.Components;
using UnityEngine;


namespace Kinemation.FPSFramework.Runtime.Layers
{
    public class ShootLayer : AnimLayer
    {
        //public FPSAnimator activeWpn;
        public bool shooting;
        
        public Transform projectile_spawn;
        public Transform muzzle_spawn;
        
        public float projectile_speed;
        public GameObject projectile_prefab;
        public GameObject muzzle_prefab;

        public float projectile_damage;

        public float projectile_hor_disp;
        public float projectile_ver_disp;
        

        public override void OnPostAnimUpdate()
        {
            if (!shooting) return;
            
            SpawnMuzzle();
            SpawnProjectile();
            
            shooting = false;
        }

        private void SpawnMuzzle()
        {
            if (!muzzle_prefab) return;
            var go = Instantiate(muzzle_prefab, muzzle_spawn);
            Destroy(go, 2f);
        }
        private void SpawnProjectile()
        {
            if (!projectile_prefab) return;
            var go = Instantiate(projectile_prefab, projectile_spawn.position, projectile_spawn.rotation);

            float d_x = Random.Range(-1f * projectile_hor_disp, projectile_hor_disp);
            float d_y = Random.Range(-1f * projectile_ver_disp, projectile_ver_disp);
            
            go.GetComponent<Rigidbody>().AddRelativeForce(d_x,d_y,projectile_speed, ForceMode.Impulse);
            
            //Set damage value to bullet
        }
    }
}
