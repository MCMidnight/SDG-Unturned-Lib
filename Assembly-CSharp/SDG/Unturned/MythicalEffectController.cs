using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Manages lifetime and attachment of a mythical effect. Added by <see cref="M:SDG.Unturned.ItemTool.ApplyMythicalEffect(UnityEngine.Transform,System.UInt16,SDG.Unturned.EEffectType)" />.
	/// Was called `MythicLocker` with a paired `MythicLockee` prior to 2024-06-11.
	/// </summary>
	// Token: 0x020005FC RID: 1532
	public class MythicalEffectController : MonoBehaviour
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x000D4E45 File Offset: 0x000D3045
		// (set) Token: 0x06003055 RID: 12373 RVA: 0x000D4E4D File Offset: 0x000D304D
		public bool IsMythicalEffectEnabled
		{
			get
			{
				return this._isMythicalEffectEnabled;
			}
			set
			{
				this._isMythicalEffectEnabled = value;
				this.MaybeInstantiateOrDestroySystem();
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000D4E5C File Offset: 0x000D305C
		private void Update()
		{
			if (this.systemTransform != null)
			{
				Vector3 position;
				Quaternion rotation;
				base.transform.GetPositionAndRotation(out position, out rotation);
				this.systemTransform.SetPositionAndRotation(position, rotation);
			}
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000D4E94 File Offset: 0x000D3094
		private void LateUpdate()
		{
			if (this.systemTransform != null)
			{
				Vector3 position;
				Quaternion rotation;
				base.transform.GetPositionAndRotation(out position, out rotation);
				this.systemTransform.SetPositionAndRotation(position, rotation);
			}
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000D4ECB File Offset: 0x000D30CB
		private void OnEnable()
		{
			this.MaybeInstantiateOrDestroySystem();
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000D4ED3 File Offset: 0x000D30D3
		private void OnDisable()
		{
			if (this.systemTransform != null)
			{
				Object.Destroy(this.systemTransform.gameObject);
				this.systemTransform = null;
			}
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x000D4EFA File Offset: 0x000D30FA
		private void OnDestroy()
		{
			if (this.systemTransform != null)
			{
				Object.Destroy(this.systemTransform.gameObject);
				this.systemTransform = null;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000D4F21 File Offset: 0x000D3121
		private void Start()
		{
			this.MaybeInstantiateOrDestroySystem();
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000D4F2C File Offset: 0x000D312C
		private void MaybeInstantiateOrDestroySystem()
		{
			if (this._isMythicalEffectEnabled && base.gameObject.activeInHierarchy)
			{
				if (this.systemTransform == null && this.systemPrefab != null)
				{
					Vector3 position;
					Quaternion rotation;
					base.transform.GetPositionAndRotation(out position, out rotation);
					this.systemTransform = Object.Instantiate<GameObject>(this.systemPrefab, position, rotation).transform;
					this.systemTransform.name = "System";
					return;
				}
			}
			else if (this.systemTransform != null)
			{
				Object.Destroy(this.systemTransform.gameObject);
				this.systemTransform = null;
			}
		}

		// Token: 0x04001B69 RID: 7017
		public GameObject systemPrefab;

		// Token: 0x04001B6A RID: 7018
		public Transform systemTransform;

		// Token: 0x04001B6B RID: 7019
		private bool _isMythicalEffectEnabled = true;
	}
}
