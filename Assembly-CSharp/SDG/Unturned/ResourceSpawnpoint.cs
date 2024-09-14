using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000504 RID: 1284
	public class ResourceSpawnpoint
	{
		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06002837 RID: 10295 RVA: 0x000A9C60 File Offset: 0x000A7E60
		// (set) Token: 0x06002838 RID: 10296 RVA: 0x000A9C68 File Offset: 0x000A7E68
		public Guid guid { get; protected set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002839 RID: 10297 RVA: 0x000A9C71 File Offset: 0x000A7E71
		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000A9C7C File Offset: 0x000A7E7C
		public bool checkCanReset(float multiplier)
		{
			return this.isDead && this.asset != null && this.asset.reset > 1f && Time.realtimeSinceStartup - this.lastDead > this.asset.reset * multiplier;
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600283B RID: 10299 RVA: 0x000A9CC8 File Offset: 0x000A7EC8
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x000A9CD3 File Offset: 0x000A7ED3
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600283D RID: 10301 RVA: 0x000A9CDB File Offset: 0x000A7EDB
		public bool isGenerated
		{
			get
			{
				return this._isGenerated;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600283E RID: 10302 RVA: 0x000A9CE3 File Offset: 0x000A7EE3
		public Quaternion angle
		{
			get
			{
				return this._angle;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600283F RID: 10303 RVA: 0x000A9CEB File Offset: 0x000A7EEB
		public Vector3 scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000A9CF3 File Offset: 0x000A7EF3
		public ResourceAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002841 RID: 10305 RVA: 0x000A9CFB File Offset: 0x000A7EFB
		// (set) Token: 0x06002842 RID: 10306 RVA: 0x000A9D03 File Offset: 0x000A7F03
		public bool isEnabled { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002843 RID: 10307 RVA: 0x000A9D0C File Offset: 0x000A7F0C
		// (set) Token: 0x06002844 RID: 10308 RVA: 0x000A9D14 File Offset: 0x000A7F14
		public bool isSkyboxEnabled { get; private set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002845 RID: 10309 RVA: 0x000A9D1D File Offset: 0x000A7F1D
		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x000A9D25 File Offset: 0x000A7F25
		public Transform stump
		{
			get
			{
				return this._stump;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x000A9D2D File Offset: 0x000A7F2D
		public Transform skybox
		{
			get
			{
				return this._skybox;
			}
		}

		/// <summary>
		/// Can this tree be damaged?
		/// Allows holiday restrictions to be taken into account. (Otherwise holiday trees could be destroyed out of season.)
		/// </summary>
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002848 RID: 10312 RVA: 0x000A9D35 File Offset: 0x000A7F35
		public bool canBeDamaged
		{
			get
			{
				return this.areConditionsMet;
			}
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000A9D3D File Offset: 0x000A7F3D
		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
				return;
			}
			this.health -= amount;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000A9D6C File Offset: 0x000A7F6C
		public void wipe()
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			if (this.asset != null)
			{
				this.health = 0;
				if (this.asset.isForage)
				{
					Transform transform = this.model.Find("Forage");
					if (transform != null)
					{
						transform.gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.model != null)
					{
						this.model.gameObject.SetActive(false);
					}
					if (this.stump != null)
					{
						this.stump.gameObject.SetActive(this.isEnabled);
					}
					if (this.stumpCollider != null)
					{
						this.stumpCollider.enabled = true;
					}
				}
			}
			if (this.skybox)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000A9E48 File Offset: 0x000A8048
		public void revive()
		{
			if (this.isAlive)
			{
				return;
			}
			this.isAlive = true;
			if (this.asset != null)
			{
				if (this.asset.isForage)
				{
					Transform transform = this.model.Find("Forage");
					if (transform != null)
					{
						transform.gameObject.SetActive(true);
					}
					this.health = this.asset.health;
				}
				else
				{
					if (this.model != null && this.areConditionsMet)
					{
						this.model.gameObject.SetActive(this.isEnabled);
					}
					this.health = this.asset.health;
					if (this.stump != null && this.areConditionsMet)
					{
						this.stump.gameObject.SetActive(this.isEnabled && this.asset.isSpeedTree);
					}
					if (this.stumpCollider != null && this.areConditionsMet)
					{
						this.stumpCollider.enabled = false;
					}
				}
			}
			if (this.skybox && this.areConditionsMet)
			{
				this.skybox.gameObject.SetActive(this.isSkyboxEnabled);
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000A9F7C File Offset: 0x000A817C
		public void kill(Vector3 ragdoll)
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			this._lastDead = Time.realtimeSinceStartup;
			if (this.asset != null)
			{
				this.health = 0;
				if (this.asset.isForage)
				{
					Transform transform = this.model.Find("Forage");
					if (transform != null)
					{
						transform.gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.model != null)
					{
						this.model.gameObject.SetActive(false);
					}
					if (this.stump != null)
					{
						this.stump.gameObject.SetActive(this.isEnabled);
					}
					if (this.stumpCollider != null)
					{
						this.stumpCollider.enabled = true;
					}
				}
			}
			if (this.skybox)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000AA064 File Offset: 0x000A8264
		public void forceFullEnable()
		{
			this.isEnabled = true;
			if (this.model != null)
			{
				this.model.gameObject.SetActive(true);
			}
			if (this.stump != null)
			{
				this.stump.gameObject.SetActive(true);
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000AA0B8 File Offset: 0x000A82B8
		public void enable()
		{
			this.isEnabled = true;
			if (this.asset != null && this.asset.isForage)
			{
				if (this.model != null && this.areConditionsMet)
				{
					this.model.gameObject.SetActive(true);
					return;
				}
			}
			else
			{
				if (this.model != null && this.areConditionsMet)
				{
					this.model.gameObject.SetActive(this.isAlive);
				}
				if (this.stump != null && this.areConditionsMet)
				{
					this.stump.gameObject.SetActive(!this.isAlive || (this.asset != null && this.asset.isSpeedTree));
				}
				if (this.stumpCollider != null && this.areConditionsMet)
				{
					this.stumpCollider.enabled = !this.isAlive;
				}
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000AA1AC File Offset: 0x000A83AC
		public void enableSkybox()
		{
			this.isSkyboxEnabled = true;
			if (this.skybox != null && this.areConditionsMet)
			{
				this.skybox.gameObject.SetActive(this.isAlive);
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000AA1E4 File Offset: 0x000A83E4
		public void disable()
		{
			this.isEnabled = false;
			if (this.model != null)
			{
				this.model.gameObject.SetActive(false);
			}
			if (this.stump != null)
			{
				this.stump.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000AA236 File Offset: 0x000A8436
		public void disableSkybox()
		{
			this.isSkyboxEnabled = false;
			if (this.skybox != null)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000AA260 File Offset: 0x000A8460
		public void destroy()
		{
			if (this.model != null)
			{
				Object.Destroy(this.model.gameObject);
			}
			if (this.stump != null)
			{
				Object.Destroy(this.stump.gameObject);
			}
			if (this.skybox != null)
			{
				Object.Destroy(this.skybox.gameObject);
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000AA2C8 File Offset: 0x000A84C8
		internal Vector3 GetEffectSpawnPosition()
		{
			if (this.model == null)
			{
				return this.point;
			}
			Transform transform = this.model.Find("Effect");
			if (transform != null)
			{
				return transform.position;
			}
			if (this.asset.hasDebris)
			{
				return this.model.position + Vector3.up * 8f;
			}
			return this.model.position;
		}

		/// <summary>
		/// Used if the asset has holiday restrictions.
		/// </summary>
		// Token: 0x06002854 RID: 10324 RVA: 0x000AA344 File Offset: 0x000A8544
		private void updateConditions()
		{
			if (this.asset == null)
			{
				return;
			}
			bool flag = HolidayUtil.isHolidayActive(this.asset.holidayRestriction);
			if (this.areConditionsMet != flag)
			{
				this.areConditionsMet = flag;
				if (this.areConditionsMet)
				{
					if (this.isEnabled)
					{
						this.enable();
					}
					if (this.isSkyboxEnabled)
					{
						this.enableSkybox();
						return;
					}
				}
				else
				{
					if (this.isEnabled)
					{
						this.disable();
					}
					if (this.isSkyboxEnabled)
					{
						this.disableSkybox();
					}
				}
			}
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x000AA3BC File Offset: 0x000A85BC
		public ResourceSpawnpoint(byte newType, ushort newID, Guid newGuid, Vector3 newPoint, bool newGenerated, NetId netId)
		{
			this.type = newType;
			this.id = newID;
			this.guid = newGuid;
			this._point = newPoint;
			this._isGenerated = newGenerated;
			if (this.guid == Guid.Empty)
			{
				this._asset = (Assets.find(EAssetType.RESOURCE, this.id) as ResourceAsset);
				if (this.asset != null)
				{
					UnturnedLog.info(string.Format("Tree without GUID loaded by legacy ID {0}, updating to {1:N} \"{2}\"", this.id, this.asset.GUID, this.asset.FriendlyName));
					this.guid = this.asset.GUID;
				}
			}
			else
			{
				this._asset = (Assets.find(this.guid) as ResourceAsset);
				if (this.asset == null)
				{
					ClientAssetIntegrity.ServerAddKnownMissingAsset(this.guid, "Tree");
				}
			}
			if (this.asset != null)
			{
				this.health = this.asset.health;
				this.isAlive = true;
				this.areConditionsMet = true;
				float num = Mathf.Sin((this.point.x + 4096f) * 32f + (this.point.z + 4096f) * 32f);
				this._angle = Quaternion.Euler(num * 5f, num * 360f, 0f);
				this._scale = new Vector3(1.1f + this.asset.scale + num * this.asset.scale, 1.1f + this.asset.scale + num * this.asset.scale, 1.1f + this.asset.scale + num * this.asset.scale);
				GameObject gameObject = null;
				if (this.asset.modelGameObject != null)
				{
					gameObject = this.asset.modelGameObject;
				}
				Vector3 position = this.point + Vector3.up * this.scale.y * this.asset.verticalOffset;
				if (gameObject != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, position, this.angle);
					this._model = gameObject2.transform;
					this.model.name = this.asset.name;
					this.model.localScale = this.scale;
					this.isEnabled = true;
					if (!netId.IsNull())
					{
						NetIdRegistry.AssignTransform(netId, this.model.transform);
					}
				}
				GameObject gameObject3 = null;
				if (this.asset.stumpGameObject != null)
				{
					gameObject3 = this.asset.stumpGameObject;
				}
				if (gameObject3 != null)
				{
					this._stump = Object.Instantiate<GameObject>(gameObject3, position, this.angle).transform;
					this.stump.name = this.asset.name + "_Stump";
					this.stump.localScale = this.scale;
					this.stump.gameObject.SetActive(false);
					if (this.asset.isSpeedTree)
					{
						this.stumpCollider = this.stump.GetComponent<Collider>();
						this.stumpCollider.enabled = false;
					}
				}
				if (this.asset.holidayRestriction != ENPCHoliday.NONE && !Level.isEditor)
				{
					this.updateConditions();
				}
			}
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x000AA70B File Offset: 0x000A890B
		public ResourceSpawnpoint(byte newType, ushort newID, Vector3 newPoint, bool newGenerated, NetId netId) : this(newType, newID, Guid.Empty, newPoint, newGenerated, netId)
		{
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x000AA71F File Offset: 0x000A891F
		public ResourceSpawnpoint(ushort newID, Vector3 newPoint, bool newGenerated, NetId netId) : this(0, newID, Guid.Empty, newPoint, newGenerated, netId)
		{
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000AA732 File Offset: 0x000A8932
		public ResourceSpawnpoint(ushort newID, Guid guid, Vector3 newPoint, bool newGenerated, NetId netId) : this(0, newID, guid, newPoint, newGenerated, netId)
		{
		}

		// Token: 0x04001559 RID: 5465
		private static List<Collider> colliders = new List<Collider>();

		// Token: 0x0400155A RID: 5466
		[Obsolete("Unused index into LevelGround.resources for early versions of the level editor.")]
		public byte type;

		// Token: 0x0400155B RID: 5467
		[Obsolete("Trees are now saved by asset GUID. Please use the asset property rather than finding asset by legacy ID.")]
		public ushort id;

		// Token: 0x0400155D RID: 5469
		private float _lastDead;

		// Token: 0x0400155E RID: 5470
		private bool areConditionsMet;

		// Token: 0x0400155F RID: 5471
		private bool isAlive;

		// Token: 0x04001560 RID: 5472
		private Vector3 _point;

		// Token: 0x04001561 RID: 5473
		private bool _isGenerated;

		// Token: 0x04001562 RID: 5474
		private Quaternion _angle;

		// Token: 0x04001563 RID: 5475
		private Vector3 _scale;

		// Token: 0x04001564 RID: 5476
		private ResourceAsset _asset;

		// Token: 0x04001567 RID: 5479
		private Transform _model;

		// Token: 0x04001568 RID: 5480
		private Transform _stump;

		// Token: 0x04001569 RID: 5481
		private Collider stumpCollider;

		// Token: 0x0400156A RID: 5482
		private Transform _skybox;

		// Token: 0x0400156B RID: 5483
		public ushort health;
	}
}
