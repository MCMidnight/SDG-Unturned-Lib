using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// When moving between physics materials we need to continue any previous tire kickup particles until they expire.
	/// This class manages the individual effect per-physics-material. Each wheel can have multiple at once. When the
	/// particles have despawned and the effect is no longer needed, the effect game object is returned to the effect
	/// pool and this class is returned to <see cref="F:SDG.Unturned.Wheel.motionEffectInstancesPool" />.
	/// </summary>
	// Token: 0x02000487 RID: 1159
	internal class TireMotionEffectInstance
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x0009009D File Offset: 0x0008E29D
		public void StopParticleSystem()
		{
			if (this.particleSystem != null)
			{
				this.particleSystem.Stop();
			}
			this.isReadyToPlay = false;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000900BF File Offset: 0x0008E2BF
		public void DestroyEffect()
		{
			if (this.gameObject != null)
			{
				EffectManager.DestroyIntoPool(this.gameObject);
				this.gameObject = null;
				this.transform = null;
				this.particleSystem = null;
				this.isReadyToPlay = false;
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000900F8 File Offset: 0x0008E2F8
		public void InstantiateEffect()
		{
			this.hasTriedToInstantiateEffect = true;
			EffectAsset effectAsset = PhysicMaterialCustomData.GetTireMotionEffect(this.materialName).Find();
			if (effectAsset != null && effectAsset.effect != null)
			{
				this.gameObject = EffectManager.InstantiateFromPool(effectAsset);
				this.transform = this.gameObject.transform;
				this.particleSystem = this.gameObject.GetComponent<ParticleSystem>();
				this.isReadyToPlay = false;
			}
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00090166 File Offset: 0x0008E366
		public void Reset()
		{
			this.gameObject = null;
			this.transform = null;
			this.particleSystem = null;
			this.isReadyToPlay = false;
			this.hasTriedToInstantiateEffect = false;
		}

		/// <summary>
		/// Name from <see cref="M:SDG.Unturned.PhysicsTool.GetMaterialName(UnityEngine.Vector3,UnityEngine.Transform,UnityEngine.Collider)" />.
		/// </summary>
		// Token: 0x04001227 RID: 4647
		public string materialName;

		/// <summary>
		/// Instantiated effect. Null after returning to pool.
		/// </summary>
		// Token: 0x04001228 RID: 4648
		private GameObject gameObject;

		/// <summary>
		/// Effect's transform. Null after returning to pool.
		/// </summary>
		// Token: 0x04001229 RID: 4649
		public Transform transform;

		/// <summary>
		/// Component on gameObject. Null after returning to pool.
		/// </summary>
		// Token: 0x0400122A RID: 4650
		public ParticleSystem particleSystem;

		/// <summary>
		/// Whether this effect should be emitting particles. False stops the particle system immediately, whereas true
		/// only starts playing on the next frame to avoid filling a gap between positions, e.g., after a jump.
		/// </summary>
		// Token: 0x0400122B RID: 4651
		public bool isReadyToPlay;

		/// <summary>
		/// Prevents repeated lookups if asset is null, while allowing asset to be looked up each time this effect
		/// becomes active so that it can be iterated on without restarting the game.
		/// </summary>
		// Token: 0x0400122C RID: 4652
		public bool hasTriedToInstantiateEffect;
	}
}
