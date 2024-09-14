using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Allows Unity events to spawn effects.
	/// </summary>
	// Token: 0x020005C8 RID: 1480
	[AddComponentMenu("Unturned/Effect Spawner")]
	public class EffectSpawner : MonoBehaviour
	{
		// Token: 0x06002FE5 RID: 12261 RVA: 0x000D3BA7 File Offset: 0x000D1DA7
		public void SpawnDefaultEffect()
		{
			this.SpawnEffect(this.DefaultEffectAssetGuid);
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x000D3BB8 File Offset: 0x000D1DB8
		public void SpawnEffect(string guid)
		{
			if (this.AuthorityOnly && !Provider.isServer)
			{
				return;
			}
			Guid guid2;
			if (!Guid.TryParse(guid, ref guid2))
			{
				UnturnedLog.warn("{0} unable to parse effect asset guid \"{1}\"", new object[]
				{
					base.transform.GetSceneHierarchyPath(),
					guid
				});
				return;
			}
			EffectAsset effectAsset = Assets.find(guid2) as EffectAsset;
			if (effectAsset == null)
			{
				UnturnedLog.warn("{0} unable to find effect asset with guid \"{1}\"", new object[]
				{
					base.transform.GetSceneHierarchyPath(),
					guid
				});
				return;
			}
			TriggerEffectParameters parameters = new TriggerEffectParameters(effectAsset);
			parameters.shouldReplicate = this.AuthorityOnly;
			parameters.reliable = this.Reliable;
			if (this.OverrideRelevantDistance > 0.01f)
			{
				parameters.relevantDistance = this.OverrideRelevantDistance;
			}
			Transform transform = (this.OverrideTransform == null) ? base.transform : this.OverrideTransform;
			parameters.position = transform.position;
			parameters.SetRotation(transform.rotation);
			EffectManager.triggerEffect(parameters);
		}

		/// <summary>
		/// GUID of effect asset to spawn when SpawnDefaultEffect is invoked.
		/// </summary>
		// Token: 0x040019E2 RID: 6626
		public string DefaultEffectAssetGuid;

		/// <summary>
		/// If true the server will spawn the effect and replicate it to clients,
		/// otherwise clients will predict their own local copy.
		/// </summary>
		// Token: 0x040019E3 RID: 6627
		public bool AuthorityOnly;

		/// <summary>
		/// Should the RPC be called in reliable mode? Unreliable effects might not be received.
		/// </summary>
		// Token: 0x040019E4 RID: 6628
		public bool Reliable;

		/// <summary>
		/// Transform to spawn the effect at.
		/// If unset this game object's transform will be used instead.
		/// </summary>
		// Token: 0x040019E5 RID: 6629
		public Transform OverrideTransform;

		/// <summary>
		/// Applied if greater than zero. Defaults to 128.
		/// </summary>
		// Token: 0x040019E6 RID: 6630
		public float OverrideRelevantDistance;
	}
}
