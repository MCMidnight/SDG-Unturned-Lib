using System;
using System.Collections.Generic;
using System.Text;
using SDG.Framework.Foliage;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x02000351 RID: 849
	public class ObjectAsset : Asset
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x0005B290 File Offset: 0x00059490
		public string objectName
		{
			get
			{
				switch (this.holidayRestriction)
				{
				case ENPCHoliday.HALLOWEEN:
					return this._objectName + " [HW]";
				case ENPCHoliday.CHRISTMAS:
					return this._objectName + " [XMAS]";
				case ENPCHoliday.APRIL_FOOLS:
					return this._objectName + " [AF]";
				case ENPCHoliday.VALENTINES:
					return this._objectName + " [V]";
				case ENPCHoliday.PRIDE_MONTH:
					return this._objectName + " [PM]";
				default:
					return this._objectName;
				}
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0005B31D File Offset: 0x0005951D
		public override string FriendlyName
		{
			get
			{
				return this.objectName;
			}
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0005B328 File Offset: 0x00059528
		public GameObject GetOrLoadModel()
		{
			if (!this.hasLoadedModel)
			{
				this.hasLoadedModel = true;
				if (this.legacyServerModel != null)
				{
					this.loadedModel = this.legacyServerModel.getOrLoad();
					if (this.loadedModel == null)
					{
						this.loadedModel = this.clientModel.getOrLoad();
					}
				}
				else
				{
					this.loadedModel = this.clientModel.getOrLoad();
				}
			}
			return this.loadedModel;
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0005B398 File Offset: 0x00059598
		protected void validateModel(GameObject asset)
		{
			if (Mathf.Abs(asset.transform.localScale.x - 1f) > 0.01f || Mathf.Abs(asset.transform.localScale.y - 1f) > 0.01f || Mathf.Abs(asset.transform.localScale.z - 1f) > 0.01f)
			{
				this.useScale = false;
				Assets.reportError(this, "should have a scale of one");
			}
			else
			{
				this.useScale = true;
			}
			Transform transform = asset.transform.Find("Block");
			if (transform != null && transform.GetComponent<Collider>() != null && transform.GetComponent<Collider>().sharedMaterial == null)
			{
				Assets.reportError(this, "has a 'Block' collider but no physics material");
			}
			Transform transform2 = asset.transform.Find("Model_0");
			string expectedTag = string.Empty;
			int num = -1;
			if (this.type == EObjectType.SMALL)
			{
				expectedTag = "Small";
				num = 17;
			}
			else if (this.type == EObjectType.MEDIUM)
			{
				expectedTag = "Medium";
				num = 16;
			}
			else if (this.type == EObjectType.LARGE)
			{
				expectedTag = "Large";
				num = 15;
			}
			if (num == -1)
			{
				Assets.reportError(this, "has an unknown tag/layer because it has an unhandled EObjectType");
			}
			else
			{
				this.fixTagAndLayer(asset, expectedTag, num);
				if (transform2 != null)
				{
					this.fixTagAndLayer(transform2.gameObject, expectedTag, num);
				}
				AssetValidation.searchGameObjectForErrors(this, asset);
			}
			if (Assets.shouldValidateAssets)
			{
				if (this.interactability == EObjectInteractability.BINARY_STATE)
				{
					Transform transform3 = asset.transform.Find("Root");
					Animation animation = (transform3 != null) ? transform3.GetComponent<Animation>() : null;
					if (animation != null)
					{
						base.validateAnimation(animation, "Open");
						base.validateAnimation(animation, "Close");
					}
				}
				if (this.interactability == EObjectInteractability.RUBBLE || this.rubble != EObjectRubble.NONE)
				{
					Transform transform4 = asset.transform.Find("Sections");
					if (transform4 != null && transform4.childCount > 8)
					{
						Assets.reportError(this, string.Format("destructible has {0} sections, but the maximum supported is 8", transform4.childCount));
					}
				}
			}
		}

		/// <summary>
		/// Clip.prefab
		/// </summary>
		// Token: 0x06001981 RID: 6529 RVA: 0x0005B5A3 File Offset: 0x000597A3
		protected void OnServerModelLoaded(GameObject asset)
		{
			if (asset == null && this.type != EObjectType.SMALL)
			{
				Assets.reportError(this, "missing \"Clip\" GameObject, loading \"Object\" GameObject instead");
			}
			if (asset != null)
			{
				this.validateModel(asset);
			}
		}

		/// <summary>
		/// Object.prefab
		/// </summary>
		// Token: 0x06001982 RID: 6530 RVA: 0x0005B5D2 File Offset: 0x000597D2
		protected void OnClientModelLoaded(GameObject asset)
		{
			if (asset == null)
			{
				Assets.reportError(this, "missing \"Object\" GameObject");
				return;
			}
			this.validateModel(asset);
			ServerPrefabUtil.RemoveClientComponents(asset);
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0005B5F8 File Offset: 0x000597F8
		protected void onNavGameObjectLoaded(GameObject asset)
		{
			if (asset == null && this.type == EObjectType.LARGE)
			{
				Assets.reportError(this, "missing Nav GameObject. Highly recommended to fix.");
			}
			if (asset != null)
			{
				this.fixTagAndLayer(asset, "Navmesh", 22);
				if (Assets.shouldValidateAssets)
				{
					this.ensureNavMeshReadable();
				}
			}
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0005B64A File Offset: 0x0005984A
		protected void onSlotsGameObjectLoaded(GameObject asset)
		{
			if (asset != null)
			{
				asset.SetTagIfUntaggedRecursively("Logic");
			}
		}

		/// <summary>
		/// If true, object will be hidden when rendering GPS/satellite view.
		/// Defaults to true if <see cref="P:SDG.Unturned.ObjectAsset.holidayRestriction" /> is set.
		/// </summary>
		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x0005B660 File Offset: 0x00059860
		// (set) Token: 0x06001986 RID: 6534 RVA: 0x0005B668 File Offset: 0x00059868
		public bool ShouldExcludeFromSatelliteCapture { get; private set; }

		// Token: 0x06001987 RID: 6535 RVA: 0x0005B671 File Offset: 0x00059871
		public EffectAsset FindInteractabilityEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.interactabilityEffectGuid, this.interactabilityEffect);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0005B684 File Offset: 0x00059884
		public EffectAsset FindRubbleEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.rubbleEffectGuid, this.rubbleEffect);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0005B697 File Offset: 0x00059897
		public bool IsRubbleFinaleEffectRefNull()
		{
			return this.rubbleFinale == 0 && GuidExtension.IsEmpty(this.rubbleFinaleGuid);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0005B6AE File Offset: 0x000598AE
		public EffectAsset FindRubbleFinaleEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this.rubbleFinaleGuid, this.rubbleFinale);
		}

		/// <summary>
		/// Only activated during this holiday.
		/// </summary>
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x0005B6C1 File Offset: 0x000598C1
		// (set) Token: 0x0600198C RID: 6540 RVA: 0x0005B6C9 File Offset: 0x000598C9
		public ENPCHoliday holidayRestriction { get; protected set; }

		/// <summary>
		/// Get asset ref to replace this one for holiday, or null if it should not be redirected.
		/// </summary>
		// Token: 0x0600198D RID: 6541 RVA: 0x0005B6D4 File Offset: 0x000598D4
		public AssetReference<ObjectAsset> getHolidayRedirect()
		{
			ENPCHoliday activeHoliday = HolidayUtil.getActiveHoliday();
			if (activeHoliday == ENPCHoliday.HALLOWEEN)
			{
				return this.halloweenRedirect;
			}
			if (activeHoliday == ENPCHoliday.CHRISTMAS)
			{
				return this.christmasRedirect;
			}
			return AssetReference<ObjectAsset>.invalid;
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0005B704 File Offset: 0x00059904
		public virtual byte[] getState()
		{
			byte[] array;
			if (this.interactability == EObjectInteractability.BINARY_STATE)
			{
				array = new byte[]
				{
					(Level.isEditor && this.interactabilityEditor != EObjectInteractabilityEditor.NONE) ? 1 : 0
				};
			}
			else if (this.interactability == EObjectInteractability.WATER || this.interactability == EObjectInteractability.FUEL)
			{
				array = new byte[]
				{
					this.interactabilityResourceState[0],
					this.interactabilityResourceState[1]
				};
			}
			else
			{
				array = null;
			}
			if (this.rubble == EObjectRubble.DESTROY)
			{
				if (array != null)
				{
					byte[] array2 = new byte[array.Length + 1];
					Array.Copy(array, array2, array.Length);
					array = array2;
				}
				else
				{
					array = new byte[1];
				}
				array[array.Length - 1] = ((Level.isEditor && this.rubbleEditor == EObjectRubbleEditor.DEAD) ? 0 : byte.MaxValue);
			}
			return array;
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x0005B7B9 File Offset: 0x000599B9
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.OBJECT;
			}
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0005B7BC File Offset: 0x000599BC
		public bool areConditionsMet(Player player)
		{
			if (this.conditions != null)
			{
				for (int i = 0; i < this.conditions.Length; i++)
				{
					if (!this.conditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// If any conditions use flags they will be added to a set,
		/// otherwise null is returned.
		/// </summary>
		// Token: 0x06001991 RID: 6545 RVA: 0x0005B7F8 File Offset: 0x000599F8
		internal HashSet<ushort> GetConditionAssociatedFlags()
		{
			if (this.conditions == null)
			{
				return null;
			}
			INPCCondition[] array = this.conditions;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GatherAssociatedFlags(ObjectAsset.tempAssociatedFlags);
			}
			if (ObjectAsset.tempAssociatedFlags.Count > 0)
			{
				HashSet<ushort> result = ObjectAsset.tempAssociatedFlags;
				ObjectAsset.tempAssociatedFlags = new HashSet<ushort>();
				return result;
			}
			return null;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0005B850 File Offset: 0x00059A50
		public bool areInteractabilityConditionsMet(Player player)
		{
			if (this.interactabilityConditions != null)
			{
				for (int i = 0; i < this.interactabilityConditions.Length; i++)
				{
					if (!this.interactabilityConditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0005B88C File Offset: 0x00059A8C
		public void ApplyInteractabilityConditions(Player player)
		{
			if (this.interactabilityConditions != null)
			{
				for (int i = 0; i < this.interactabilityConditions.Length; i++)
				{
					this.interactabilityConditions[i].ApplyCondition(player);
				}
			}
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0005B8C2 File Offset: 0x00059AC2
		public void GrantInteractabilityRewards(Player player)
		{
			this.interactabilityRewards.Grant(player);
		}

		/// <summary>
		/// Recursively change all children including root from oldTag to newTag.
		/// Aborts if a child doesn't match the old tag because it might be something we shouldn't change the tag of.
		/// <return>True if tags were all successfully changed.</return>
		/// </summary>
		// Token: 0x06001995 RID: 6549 RVA: 0x0005B8D0 File Offset: 0x00059AD0
		protected bool recursivelyFixTag(GameObject parentGameObject, string oldTag, string newTag)
		{
			if (parentGameObject.CompareTag(oldTag))
			{
				parentGameObject.tag = newTag;
				int childCount = parentGameObject.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					GameObject gameObject = parentGameObject.transform.GetChild(i).gameObject;
					if (!this.recursivelyFixTag(gameObject, oldTag, newTag))
					{
						return false;
					}
				}
				return true;
			}
			Assets.reportError(this, string.Concat(new string[]
			{
				"unable to automatically fix tag for ",
				this.objectName,
				"'s ",
				parentGameObject.name,
				"! Trying to convert tag ",
				oldTag,
				" to ",
				newTag
			}));
			return false;
		}

		/// <summary>
		/// Recursively change all children including root from oldLayer to newLayer.
		/// Aborts if a child doesn't match the old layer because it might be something we shouldn't change the layer of.
		/// <return>True if layers were all successfully changed.</return>
		/// </summary>
		// Token: 0x06001996 RID: 6550 RVA: 0x0005B974 File Offset: 0x00059B74
		protected bool recursivelyFixLayer(GameObject parentGameObject, int oldLayer, int newLayer)
		{
			if (parentGameObject.layer == oldLayer)
			{
				parentGameObject.layer = newLayer;
				int childCount = parentGameObject.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					GameObject gameObject = parentGameObject.transform.GetChild(i).gameObject;
					if (!this.recursivelyFixLayer(gameObject, oldLayer, newLayer))
					{
						return false;
					}
				}
				return true;
			}
			Assets.reportError(this, string.Concat(new string[]
			{
				"Unable to automatically fix layer for ",
				this.objectName,
				"'s ",
				parentGameObject.name,
				"! Trying to convert layer ",
				oldLayer.ToString(),
				" to ",
				newLayer.ToString()
			}));
			return false;
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0005BA24 File Offset: 0x00059C24
		protected void fixTagAndLayer(GameObject rootGameObject, string expectedTag, int expectedLayer)
		{
			if (!rootGameObject.CompareTag(expectedTag))
			{
				string tag = rootGameObject.tag;
				this.recursivelyFixTag(rootGameObject, tag, expectedTag);
			}
			if (rootGameObject.layer != expectedLayer)
			{
				int layer = rootGameObject.layer;
				this.recursivelyFixLayer(rootGameObject, layer, expectedLayer);
			}
		}

		/// <summary>
		/// Called if we have a valid Nav GameObject.
		/// Recast requires any meshes used on the Nav objects to be CPU readable, so we log errors here if they're not marked as such.
		/// </summary>
		// Token: 0x06001998 RID: 6552 RVA: 0x0005BA68 File Offset: 0x00059C68
		private void ensureNavMeshReadable()
		{
			ObjectAsset.navMCs.Clear();
			IDeferredAsset<GameObject> deferredAsset = this.navGameObject;
			if (deferredAsset != null)
			{
				deferredAsset.getOrLoad().GetComponentsInChildren<MeshCollider>(true, ObjectAsset.navMCs);
			}
			foreach (MeshCollider meshCollider in ObjectAsset.navMCs)
			{
				if (meshCollider.sharedMesh == null)
				{
					Assets.reportError(this, "missing mesh for MeshCollider '" + meshCollider.name + "'");
				}
				else if (!meshCollider.sharedMesh.isReadable)
				{
					Assets.reportError(this, "mesh must have read/write enabled for MeshCollider '" + meshCollider.name + "'");
				}
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0005BB2C File Offset: 0x00059D2C
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._objectName = localization.format("Name");
			this.type = (EObjectType)Enum.Parse(typeof(EObjectType), data.GetString("Type", null), true);
			if (this.type == EObjectType.NPC)
			{
				this.loadedModel = Resources.Load<GameObject>("Characters/NPC_Server");
				this.hasLoadedModel = true;
				this.useScale = true;
				this.interactability = EObjectInteractability.NPC;
				this.chart = EObjectChart.IGNORE;
			}
			else if (this.type == EObjectType.DECAL)
			{
				float num = data.ParseFloat("Decal_X", 1f);
				float num2 = data.ParseFloat("Decal_Y", 1f);
				float lodBias = 1f;
				if (data.ContainsKey("Decal_LOD_Bias"))
				{
					lodBias = data.ParseFloat("Decal_LOD_Bias", 0f);
				}
				Texture2D texture2D = bundle.load<Texture2D>("Decal");
				if (texture2D == null)
				{
					Assets.reportError(this, "missing 'Decal' Texture2D. It will show as pure white without one.");
				}
				bool flag = data.ContainsKey("Decal_Alpha");
				this.hasLoadedModel = true;
				this.loadedModel = Object.Instantiate<GameObject>(Resources.Load<GameObject>(flag ? "Materials/Decal_Template_Alpha" : "Materials/Decal_Template_Masked"));
				this.loadedModel.transform.position = new Vector3(-10000f, -10000f, -10000f);
				this.loadedModel.hideFlags = HideFlags.HideAndDontSave;
				Object.DontDestroyOnLoad(this.loadedModel);
				this.loadedModel.GetComponent<BoxCollider>().size = new Vector3(num2, num, 1f);
				Decal component = this.loadedModel.transform.Find("Decal").GetComponent<Decal>();
				Material material = Object.Instantiate<Material>(component.material);
				material.name = "Decal_Deferred";
				material.hideFlags = HideFlags.DontSave;
				material.SetTexture("_MainTex", texture2D);
				component.material = material;
				component.lodBias = lodBias;
				component.transform.localScale = new Vector3(num, num2, 1f);
				MeshRenderer component2 = this.loadedModel.transform.Find("Mesh").GetComponent<MeshRenderer>();
				Material material2 = Object.Instantiate<Material>(component2.sharedMaterial);
				material2.name = "Decal_Forward";
				material2.hideFlags = HideFlags.DontSave;
				material2.SetTexture("_MainTex", texture2D);
				component2.sharedMaterial = material2;
				component2.transform.localScale = new Vector3(num2, num, 1f);
				this.useScale = true;
				this.chart = EObjectChart.IGNORE;
			}
			else
			{
				if (data.ContainsKey("Interactability"))
				{
					this.interactability = (EObjectInteractability)Enum.Parse(typeof(EObjectInteractability), data.GetString("Interactability", null), true);
					this.interactabilityRemote = data.ContainsKey("Interactability_Remote");
					this.interactabilityDelay = data.ParseFloat("Interactability_Delay", 0f);
					this.interactabilityReset = data.ParseFloat("Interactability_Reset", 0f);
					if (data.ContainsKey("Interactability_Hint"))
					{
						this.interactabilityHint = (EObjectInteractabilityHint)Enum.Parse(typeof(EObjectInteractabilityHint), data.GetString("Interactability_Hint", null), true);
					}
					if (this.interactability == EObjectInteractability.NOTE)
					{
						ushort num3 = data.ParseUInt16("Interactability_Text_Lines", 0);
						StringBuilder stringBuilder = new StringBuilder();
						for (ushort num4 = 0; num4 < num3; num4 += 1)
						{
							string text = localization.format("Interactability_Text_Line_" + num4.ToString());
							text = ItemTool.filterRarityRichText(text);
							RichTextUtil.replaceNewlineMarkup(ref text);
							stringBuilder.AppendLine(text);
						}
						this.interactabilityText = stringBuilder.ToString();
					}
					else
					{
						this.interactabilityText = localization.read("Interact");
						if (string.IsNullOrWhiteSpace(this.interactabilityText))
						{
							if (this.interactability == EObjectInteractability.QUEST)
							{
								Assets.reportError(this, "Interact text empty");
							}
						}
						else
						{
							this.interactabilityText = ItemTool.filterRarityRichText(this.interactabilityText);
							RichTextUtil.replaceNewlineMarkup(ref this.interactabilityText);
						}
					}
					if (data.ContainsKey("Interactability_Power"))
					{
						this.interactabilityPower = (EObjectInteractabilityPower)Enum.Parse(typeof(EObjectInteractabilityPower), data.GetString("Interactability_Power", null), true);
					}
					else
					{
						this.interactabilityPower = EObjectInteractabilityPower.NONE;
					}
					if (data.ContainsKey("Interactability_Editor"))
					{
						this.interactabilityEditor = (EObjectInteractabilityEditor)Enum.Parse(typeof(EObjectInteractabilityEditor), data.GetString("Interactability_Editor", null), true);
					}
					else
					{
						this.interactabilityEditor = EObjectInteractabilityEditor.NONE;
					}
					if (data.ContainsKey("Interactability_Nav"))
					{
						this.interactabilityNav = (EObjectInteractabilityNav)Enum.Parse(typeof(EObjectInteractabilityNav), data.GetString("Interactability_Nav", null), true);
					}
					else
					{
						this.interactabilityNav = EObjectInteractabilityNav.NONE;
					}
					this.interactabilityDrops = new ushort[(int)data.ParseUInt8("Interactability_Drops", 0)];
					byte b = 0;
					while ((int)b < this.interactabilityDrops.Length)
					{
						this.interactabilityDrops[(int)b] = data.ParseUInt16("Interactability_Drop_" + b.ToString(), 0);
						b += 1;
					}
					this.interactabilityRewardID = data.ParseUInt16("Interactability_Reward_ID", 0);
					this.interactabilityEffect = data.ParseGuidOrLegacyId("Interactability_Effect", out this.interactabilityEffectGuid);
					this.interactabilityConditions = new INPCCondition[(int)data.ParseUInt8("Interactability_Conditions", 0)];
					NPCTool.readConditions(data, localization, "Interactability_Condition_", this.interactabilityConditions, this);
					this.interactabilityRewards.Parse(data, localization, this, "Interactability_Rewards", "Interactability_Reward_");
					this.interactabilityResource = data.ParseUInt16("Interactability_Resource", 0);
					this.interactabilityResourceState = BitConverter.GetBytes(this.interactabilityResource);
				}
				else
				{
					this.interactability = EObjectInteractability.NONE;
					this.interactabilityPower = EObjectInteractabilityPower.NONE;
					this.interactabilityEditor = EObjectInteractabilityEditor.NONE;
				}
				if (this.interactability == EObjectInteractability.RUBBLE)
				{
					this.rubble = EObjectRubble.DESTROY;
					this.rubbleReset = data.ParseFloat("Interactability_Reset", 0f);
					this.rubbleHealth = data.ParseUInt16("Interactability_Health", 0);
					this.rubbleEffect = data.ParseGuidOrLegacyId("Interactability_Effect", out this.rubbleEffectGuid);
					this.rubbleFinale = data.ParseGuidOrLegacyId("Interactability_Finale", out this.rubbleFinaleGuid);
					this.rubbleRewardID = data.ParseUInt16("Interactability_Reward_ID", 0);
					this.rubbleBladeID = data.ParseUInt8("Interactability_Blade_ID", 0);
					this.rubbleRewardProbability = data.ParseFloat("Interactability_Reward_Probability", 1f);
					this.rubbleRewardsMin = data.ParseUInt8("Interactability_Rewards_Min", 1);
					this.rubbleRewardsMax = data.ParseUInt8("Interactability_Rewards_Max", 1);
					this.rubbleRewardXP = data.ParseUInt32("Interactability_Reward_XP", 0U);
					this.rubbleIsVulnerable = !data.ContainsKey("Interactability_Invulnerable");
					this.rubbleProofExplosion = data.ContainsKey("Interactability_Proof_Explosion");
				}
				else if (data.ContainsKey("Rubble"))
				{
					this.rubble = (EObjectRubble)Enum.Parse(typeof(EObjectRubble), data.GetString("Rubble", null), true);
					this.rubbleReset = data.ParseFloat("Rubble_Reset", 0f);
					this.rubbleHealth = data.ParseUInt16("Rubble_Health", 0);
					this.rubbleEffect = data.ParseGuidOrLegacyId("Rubble_Effect", out this.rubbleEffectGuid);
					this.rubbleFinale = data.ParseGuidOrLegacyId("Rubble_Finale", out this.rubbleFinaleGuid);
					this.rubbleRewardID = data.ParseUInt16("Rubble_Reward_ID", 0);
					this.rubbleBladeID = data.ParseUInt8("Rubble_Blade_ID", 0);
					this.rubbleRewardProbability = data.ParseFloat("Rubble_Reward_Probability", 1f);
					this.rubbleRewardsMin = data.ParseUInt8("Rubble_Rewards_Min", 1);
					this.rubbleRewardsMax = data.ParseUInt8("Rubble_Rewards_Max", 1);
					this.rubbleRewardXP = data.ParseUInt32("Rubble_Reward_XP", 0U);
					this.rubbleIsVulnerable = !data.ContainsKey("Rubble_Invulnerable");
					this.rubbleProofExplosion = data.ContainsKey("Rubble_Proof_Explosion");
					if (data.ContainsKey("Rubble_Editor"))
					{
						this.rubbleEditor = (EObjectRubbleEditor)Enum.Parse(typeof(EObjectRubbleEditor), data.GetString("Rubble_Editor", null), true);
					}
					else
					{
						this.rubbleEditor = EObjectRubbleEditor.ALIVE;
					}
				}
				if (data.ParseBool("Has_Clip_Prefab", true))
				{
					bundle.loadDeferred<GameObject>("Clip", out this.legacyServerModel, new LoadedAssetDeferredCallback<GameObject>(this.OnServerModelLoaded));
				}
				bundle.loadDeferred<GameObject>("Object", out this.clientModel, new LoadedAssetDeferredCallback<GameObject>(this.OnClientModelLoaded));
				bundle.loadDeferred<GameObject>("Nav", out this.navGameObject, new LoadedAssetDeferredCallback<GameObject>(this.onNavGameObjectLoaded));
				bundle.loadDeferred<GameObject>("Slots", out this.slotsGameObject, new LoadedAssetDeferredCallback<GameObject>(this.onSlotsGameObjectLoaded));
				bundle.loadDeferred<GameObject>("Triggers", out this.triggersGameObject, null);
				this.isSnowshoe = data.ContainsKey("Snowshoe");
				if (data.ContainsKey("Chart"))
				{
					this.chart = (EObjectChart)Enum.Parse(typeof(EObjectChart), data.GetString("Chart", null), true);
				}
				else
				{
					this.chart = EObjectChart.NONE;
				}
				this.isFuel = data.ContainsKey("Fuel");
				this.isRefill = data.ContainsKey("Refill");
				this.isSoft = data.ContainsKey("Soft");
				this.causesFallDamage = data.ParseBool("Causes_Fall_Damage", true);
				this.isCollisionImportant = (data.ContainsKey("Collision_Important") || this.type == EObjectType.LARGE);
				this.shouldExcludeFromCullingVolumes = data.ParseBool("Exclude_From_Culling_Volumes", false);
				if (this.isFuel || this.isRefill)
				{
					Assets.reportError(this, "is using the legacy fuel/water system");
				}
				data.ContainsKey("LOD");
				if (data.ContainsKey("Foliage"))
				{
					this.foliage = new AssetReference<FoliageInfoCollectionAsset>(new Guid(data.GetString("Foliage", null)));
				}
				this.useWaterHeightTransparentSort = data.ContainsKey("Use_Water_Height_Transparent_Sort");
				this.shouldAddNightLightScript = data.ContainsKey("Add_Night_Light_Script");
				this.shouldAddKillTriggers = data.ParseBool("Add_Kill_Triggers", false);
				this.allowStructures = data.ContainsKey("Allow_Structures");
				if (data.ContainsKey("Material_Palette"))
				{
					this.materialPalette = new AssetReference<MaterialPaletteAsset>(data.ParseGuid("Material_Palette", default(Guid)));
				}
				if (data.ContainsKey("Landmark_Quality"))
				{
					this.landmarkQuality = (EGraphicQuality)Enum.Parse(typeof(EGraphicQuality), data.GetString("Landmark_Quality", null), true);
					if (this.landmarkQuality < EGraphicQuality.LOW)
					{
						this.landmarkQuality = EGraphicQuality.LOW;
					}
				}
				else
				{
					this.landmarkQuality = EGraphicQuality.LOW;
				}
			}
			if (data.ContainsKey("Holiday_Restriction"))
			{
				this.holidayRestriction = (ENPCHoliday)Enum.Parse(typeof(ENPCHoliday), data.GetString("Holiday_Restriction", null), true);
				if (this.holidayRestriction == ENPCHoliday.NONE)
				{
					Assets.reportError(this, "has no holiday restriction, so value is ignored");
				}
			}
			else
			{
				this.holidayRestriction = ENPCHoliday.NONE;
			}
			this.christmasRedirect = data.readAssetReference("Christmas_Redirect");
			this.halloweenRedirect = data.readAssetReference("Halloween_Redirect");
			this.isGore = data.ParseBool("Is_Gore", false);
			this.shouldExcludeFromLevelBatching = data.ParseBool("Exclude_From_Level_Batching", false);
			this.shouldExcludeFromLevelBatching |= (this.type == EObjectType.NPC || this.type == EObjectType.DECAL);
			bool defaultValue = this.holidayRestriction > ENPCHoliday.NONE;
			this.ShouldExcludeFromSatelliteCapture = data.ParseBool("Exclude_From_Satellite_Capture", defaultValue);
			this.conditions = new INPCCondition[(int)data.ParseUInt8("Conditions", 0)];
			NPCTool.readConditions(data, localization, "Condition_", this.conditions, this);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0005C67B File Offset: 0x0005A87B
		[Obsolete("Removed shouldSend parameter")]
		public void applyInteractabilityConditions(Player player, bool shouldSend)
		{
			this.ApplyInteractabilityConditions(player);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0005C684 File Offset: 0x0005A884
		[Obsolete("Removed shouldSend parameter")]
		public void grantInteractabilityRewards(Player player, bool shouldSend)
		{
			this.GrantInteractabilityRewards(player);
		}

		// Token: 0x04000B5E RID: 2910
		protected string _objectName;

		// Token: 0x04000B5F RID: 2911
		public EObjectType type;

		// Token: 0x04000B60 RID: 2912
		private GameObject loadedModel;

		/// <summary>
		/// Prevents calling getOrLoad redundantly if asset does not exist.
		/// </summary>
		// Token: 0x04000B61 RID: 2913
		private bool hasLoadedModel;

		// Token: 0x04000B62 RID: 2914
		private IDeferredAsset<GameObject> clientModel;

		// Token: 0x04000B63 RID: 2915
		private IDeferredAsset<GameObject> legacyServerModel;

		// Token: 0x04000B64 RID: 2916
		public IDeferredAsset<GameObject> skyboxGameObject;

		// Token: 0x04000B65 RID: 2917
		public IDeferredAsset<GameObject> navGameObject;

		// Token: 0x04000B66 RID: 2918
		public IDeferredAsset<GameObject> slotsGameObject;

		// Token: 0x04000B67 RID: 2919
		public IDeferredAsset<GameObject> triggersGameObject;

		// Token: 0x04000B68 RID: 2920
		public bool isSnowshoe;

		// Token: 0x04000B69 RID: 2921
		public bool shouldExcludeFromCullingVolumes;

		// Token: 0x04000B6A RID: 2922
		public bool shouldExcludeFromLevelBatching;

		// Token: 0x04000B6C RID: 2924
		public EObjectChart chart;

		// Token: 0x04000B6D RID: 2925
		public bool isFuel;

		// Token: 0x04000B6E RID: 2926
		public bool isRefill;

		// Token: 0x04000B6F RID: 2927
		public bool isSoft;

		/// <summary>
		/// Should landing on this object inflict fall damage?
		/// </summary>
		// Token: 0x04000B70 RID: 2928
		public bool causesFallDamage;

		// Token: 0x04000B71 RID: 2929
		public bool isCollisionImportant;

		// Token: 0x04000B72 RID: 2930
		public bool useScale;

		// Token: 0x04000B73 RID: 2931
		public INPCCondition[] conditions;

		// Token: 0x04000B74 RID: 2932
		public EObjectInteractability interactability;

		// Token: 0x04000B75 RID: 2933
		public bool interactabilityRemote;

		// Token: 0x04000B76 RID: 2934
		public float interactabilityDelay;

		// Token: 0x04000B77 RID: 2935
		public EObjectInteractabilityHint interactabilityHint;

		// Token: 0x04000B78 RID: 2936
		public string interactabilityText;

		// Token: 0x04000B79 RID: 2937
		public EObjectInteractabilityPower interactabilityPower;

		// Token: 0x04000B7A RID: 2938
		public EObjectInteractabilityEditor interactabilityEditor;

		// Token: 0x04000B7B RID: 2939
		public EObjectInteractabilityNav interactabilityNav;

		// Token: 0x04000B7C RID: 2940
		public float interactabilityReset;

		// Token: 0x04000B7D RID: 2941
		public ushort interactabilityResource;

		// Token: 0x04000B7E RID: 2942
		private byte[] interactabilityResourceState;

		// Token: 0x04000B7F RID: 2943
		public ushort[] interactabilityDrops;

		// Token: 0x04000B80 RID: 2944
		public ushort interactabilityRewardID;

		// Token: 0x04000B81 RID: 2945
		public Guid interactabilityEffectGuid;

		// Token: 0x04000B82 RID: 2946
		[Obsolete]
		public ushort interactabilityEffect;

		// Token: 0x04000B83 RID: 2947
		public INPCCondition[] interactabilityConditions;

		// Token: 0x04000B84 RID: 2948
		protected NPCRewardsList interactabilityRewards;

		// Token: 0x04000B85 RID: 2949
		public EObjectRubble rubble;

		// Token: 0x04000B86 RID: 2950
		public float rubbleReset;

		// Token: 0x04000B87 RID: 2951
		public ushort rubbleHealth;

		/// <summary>
		/// Effect played when single segment is destroyed.
		/// </summary>
		// Token: 0x04000B88 RID: 2952
		public Guid rubbleEffectGuid;

		// Token: 0x04000B89 RID: 2953
		[Obsolete]
		public ushort rubbleEffect;

		/// <summary>
		/// Effect played when all segments are destroyed.
		/// </summary>
		// Token: 0x04000B8A RID: 2954
		public Guid rubbleFinaleGuid;

		// Token: 0x04000B8B RID: 2955
		[Obsolete]
		public ushort rubbleFinale;

		// Token: 0x04000B8C RID: 2956
		public EObjectRubbleEditor rubbleEditor;

		// Token: 0x04000B8D RID: 2957
		public ushort rubbleRewardID;

		/// <summary>
		/// Weapon must have matching blade ID to damage object.
		/// Both weapons and objects default to zero so they can be damaged by default.
		/// </summary>
		// Token: 0x04000B8E RID: 2958
		public byte rubbleBladeID;

		/// <summary>
		/// [0, 1] probability of dropping any rewards.
		/// </summary>
		// Token: 0x04000B8F RID: 2959
		public float rubbleRewardProbability;

		// Token: 0x04000B90 RID: 2960
		public byte rubbleRewardsMin;

		// Token: 0x04000B91 RID: 2961
		public byte rubbleRewardsMax;

		// Token: 0x04000B92 RID: 2962
		public uint rubbleRewardXP;

		// Token: 0x04000B93 RID: 2963
		public bool rubbleIsVulnerable;

		// Token: 0x04000B94 RID: 2964
		public bool rubbleProofExplosion;

		// Token: 0x04000B95 RID: 2965
		public AssetReference<FoliageInfoCollectionAsset> foliage;

		// Token: 0x04000B96 RID: 2966
		public bool useWaterHeightTransparentSort;

		// Token: 0x04000B97 RID: 2967
		public AssetReference<MaterialPaletteAsset> materialPalette;

		// Token: 0x04000B98 RID: 2968
		public EGraphicQuality landmarkQuality;

		// Token: 0x04000B99 RID: 2969
		public bool shouldAddNightLightScript;

		/// <summary>
		/// Should colliders in the Triggers GameObject with "Kill" name kill players?
		/// If Triggers GameObject is not set, searches Object instead.
		/// </summary>
		// Token: 0x04000B9A RID: 2970
		public bool shouldAddKillTriggers;

		// Token: 0x04000B9B RID: 2971
		public bool allowStructures;

		/// <summary>
		/// Should this object only be visible if gore is enabled?
		/// Allows pre-placed blood splatters to be hidden for younger players.
		/// </summary>
		// Token: 0x04000B9C RID: 2972
		public bool isGore;

		/// <summary>
		/// Object to use during the Christmas event instead.
		/// </summary>
		// Token: 0x04000B9E RID: 2974
		public AssetReference<ObjectAsset> christmasRedirect;

		/// <summary>
		/// Object to use during the Halloween event instead.
		/// </summary>
		// Token: 0x04000B9F RID: 2975
		public AssetReference<ObjectAsset> halloweenRedirect;

		// Token: 0x04000BA0 RID: 2976
		private static HashSet<ushort> tempAssociatedFlags = new HashSet<ushort>();

		// Token: 0x04000BA1 RID: 2977
		private static List<MeshCollider> navMCs = new List<MeshCollider>();
	}
}
