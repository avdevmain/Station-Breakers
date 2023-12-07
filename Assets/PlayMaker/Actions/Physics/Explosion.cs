// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Applies an explosion Force to all Game Objects with a Rigid Body inside a Radius.")]
	public class Explosion : FsmStateAction
	{
		
		[RequiredField]
		[UIHint(UIHint.Variable)] 
		[Tooltip("The Array to sort.")] 
		public FsmArray array;

		[RequiredField] [Tooltip("If should the object be visible to apply force to it")]
		public FsmBool visibilityRequired;
		
		[RequiredField]
		[Tooltip("The world position of the center of the explosion.")]
        public FsmVector3 center;

		[RequiredField]
        [Tooltip("The strength of the explosion.")]
		public FsmFloat force;

		[RequiredField]
        [Tooltip("The radius of the explosion. Force falls of linearly with distance.")]
		public FsmFloat radius;

        [Tooltip("Applies the force as if it was applied from beneath the object. This is useful because explosions that throw things up instead of pushing things to the side look cooler. A value of 2 will apply a force as if it is applied from 2 meters below while not changing the actual explosion position.")]
		public FsmFloat upwardsModifier;

        [Tooltip("The type of force to apply.")]
		public ForceMode forceMode;

		[UIHint(UIHint.Layer)]
        [Tooltip("Layers to effect.")]
		public FsmInt[] layerMask;
		
		
		private FsmInt[] visibilityLayerMask;

		[Tooltip("Invert the mask, so you effect all layers except those defined above.")]
		public FsmBool invertMask;
		
        [Tooltip("Repeat every frame while the state is active.")]
        public bool everyFrame;

		public override void Reset()
		{
			center = null;
			upwardsModifier = 0f;
			forceMode = ForceMode.Force;
			everyFrame = false;
		}

        public override void OnPreprocess()
        {
            Fsm.HandleFixedUpdate = true;
        }



        private List<GameObject> list;
		public override void OnEnter()
		{
			visibilityLayerMask = new FsmInt[layerMask.Length+1];
			visibilityLayerMask[0] = 0;
			layerMask.CopyTo(visibilityLayerMask,1);
			
			
			list = new List<GameObject>();
			DoExplosion();
			
			if (!everyFrame)
			{
			    Finish();
			}		
		}

		public override void OnFixedUpdate()
		{
			DoExplosion();
		}

		void DoExplosion()
		{
			var colliders = Physics.OverlapSphere(center.Value, radius.Value);
			RaycastHit hitInfo;
			foreach (var hit in colliders)
			{
			    var rigidBody = hit.gameObject.GetComponent<Rigidbody>();
                if (rigidBody != null && ShouldApplyForce(hit.gameObject))
				{
					if (visibilityRequired.Value)
					{
						Physics.Raycast(center.Value, hit.ClosestPointOnBounds(center.Value) - center.Value,
							out hitInfo, radius.Value,
							ActionHelpers.LayerArrayToLayerMask(visibilityLayerMask, invertMask.Value));
						if (hitInfo.collider==null) continue;
						if (hitInfo.collider.gameObject != hit.gameObject) continue;
							
					}
					list.Add(hit.gameObject);
                    rigidBody.AddExplosionForce(force.Value, center.Value, radius.Value, upwardsModifier.Value, forceMode);
				}
			}

			array.Values = list.ToArray();
		}
		
		bool ShouldApplyForce(GameObject go)
		{
			var mask = ActionHelpers.LayerArrayToLayerMask(layerMask, invertMask.Value);
			
			return ((1 << go.layer) & mask) > 0;
		}
	}
}