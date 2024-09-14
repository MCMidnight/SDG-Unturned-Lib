using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Foliage;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004EE RID: 1262
	public class LevelObject
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x000A3ADD File Offset: 0x000A1CDD
		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		/// <summary>
		/// Transform created to preserve objects whose assets failed to load.
		/// Separate from default transform to avoid messing with old behavior when transform is null.
		/// </summary>
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x000A3AE5 File Offset: 0x000A1CE5
		// (set) Token: 0x06002744 RID: 10052 RVA: 0x000A3AED File Offset: 0x000A1CED
		public Transform placeholderTransform { get; protected set; }

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x000A3AF6 File Offset: 0x000A1CF6
		public Transform skybox
		{
			get
			{
				return this._skybox;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002746 RID: 10054 RVA: 0x000A3AFE File Offset: 0x000A1CFE
		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x000A3B06 File Offset: 0x000A1D06
		public Guid GUID
		{
			get
			{
				return this._GUID;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000A3B0E File Offset: 0x000A1D0E
		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000A3B16 File Offset: 0x000A1D16
		public ObjectAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x000A3B1E File Offset: 0x000A1D1E
		public InteractableObject interactable
		{
			get
			{
				return this._interactableObj;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x000A3B26 File Offset: 0x000A1D26
		public InteractableObjectRubble rubble
		{
			get
			{
				return this._rubble;
			}
		}

		/// <summary>
		/// Can this object's rubble be damaged?
		/// Allows holiday restrictions to be taken into account. (Otherwise holiday presents could be destroyed out of season.)
		/// </summary>
		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x000A3B2E File Offset: 0x000A1D2E
		public bool canDamageRubble
		{
			get
			{
				return this.areConditionsMet;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000A3B36 File Offset: 0x000A1D36
		// (set) Token: 0x0600274E RID: 10062 RVA: 0x000A3B3E File Offset: 0x000A1D3E
		public ELevelObjectPlacementOrigin placementOrigin { get; protected set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600274F RID: 10063 RVA: 0x000A3B47 File Offset: 0x000A1D47
		// (set) Token: 0x06002750 RID: 10064 RVA: 0x000A3B4F File Offset: 0x000A1D4F
		[Obsolete]
		public bool isCollisionEnabled { get; private set; }

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000A3B58 File Offset: 0x000A1D58
		// (set) Token: 0x06002752 RID: 10066 RVA: 0x000A3B60 File Offset: 0x000A1D60
		[Obsolete]
		public bool isVisualEnabled { get; private set; }

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x000A3B69 File Offset: 0x000A1D69
		// (set) Token: 0x06002754 RID: 10068 RVA: 0x000A3B71 File Offset: 0x000A1D71
		[Obsolete]
		public bool isSkyboxEnabled { get; private set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000A3B7A File Offset: 0x000A1D7A
		public bool isLandmarkQualityMet
		{
			get
			{
				ObjectAsset asset = this.asset;
				return false;
			}
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000A3B84 File Offset: 0x000A1D84
		internal void SetIsActiveInRegion(bool isActive)
		{
			if (this.isActiveInRegion != isActive)
			{
				this.isActiveInRegion = isActive;
				this.UpdateActiveAndRenderersEnabled();
			}
		}

		/// <summary>
		/// Object activation is time-sliced, so this does not necessarily match whether the region is active.
		/// </summary>
		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002757 RID: 10071 RVA: 0x000A3B9C File Offset: 0x000A1D9C
		// (set) Token: 0x06002758 RID: 10072 RVA: 0x000A3BA4 File Offset: 0x000A1DA4
		internal bool isActiveInRegion { get; private set; }

		// Token: 0x06002759 RID: 10073 RVA: 0x000A3BAD File Offset: 0x000A1DAD
		internal void SetIsVisibleInCullingVolume(bool isVisible)
		{
			if (this.isVisibleInCullingVolume != isVisible)
			{
				this.isVisibleInCullingVolume = isVisible;
				this.UpdateActiveAndRenderersEnabled();
			}
		}

		/// <summary>
		/// Defaults to true because most objects are not inside a culling volume. 
		/// </summary>
		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x0600275A RID: 10074 RVA: 0x000A3BC5 File Offset: 0x000A1DC5
		// (set) Token: 0x0600275B RID: 10075 RVA: 0x000A3BCD File Offset: 0x000A1DCD
		internal bool isVisibleInCullingVolume { get; private set; } = true;

		// Token: 0x0600275C RID: 10076 RVA: 0x000A3BD6 File Offset: 0x000A1DD6
		internal void SetIsActiveOverrideForSatelliteCapture(bool isActive)
		{
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(isActive);
			}
			if (this.skybox != null)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x000A3C16 File Offset: 0x000A1E16
		public void destroy()
		{
			if (this.transform)
			{
				Object.Destroy(this.transform.gameObject);
			}
			if (this.skybox)
			{
				Object.Destroy(this.skybox.gameObject);
			}
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000A3C54 File Offset: 0x000A1E54
		internal void ReapplyMaterialOverrides()
		{
			Material materialOverride = this.GetMaterialOverride();
			if (materialOverride == null)
			{
				return;
			}
			if (this.skybox != null)
			{
				this.renderers.Clear();
				this.skybox.GetComponentsInChildren<Renderer>(true, this.renderers);
				foreach (Renderer renderer in this.renderers)
				{
					renderer.sharedMaterial = materialOverride;
				}
			}
			this.renderers.Clear();
			this.transform.GetComponentsInChildren<Renderer>(true, this.renderers);
			foreach (Renderer renderer2 in this.renderers)
			{
				renderer2.sharedMaterial = materialOverride;
			}
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000A3D40 File Offset: 0x000A1F40
		internal void ReapplyOwnedCullingVolumeAllowed()
		{
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000A3D44 File Offset: 0x000A1F44
		private Material GetMaterialOverride()
		{
			Material material = null;
			AssetReference<MaterialPaletteAsset> materialPalette = this.customMaterialOverride;
			if (!materialPalette.isValid)
			{
				materialPalette = this.asset.materialPalette;
			}
			if (materialPalette.isValid)
			{
				MaterialPaletteAsset materialPaletteAsset = Assets.find<MaterialPaletteAsset>(materialPalette);
				if (materialPaletteAsset != null && materialPaletteAsset.materials != null && materialPaletteAsset.materials.Count > 0)
				{
					int num;
					if (this.materialIndexOverride == -1)
					{
						Random.State state = Random.state;
						Random.InitState((int)this.instanceID);
						num = Random.Range(0, materialPaletteAsset.materials.Count);
						Random.state = state;
					}
					else
					{
						num = Mathf.Clamp(this.materialIndexOverride, 0, materialPaletteAsset.materials.Count - 1);
					}
					material = Assets.load<Material>(materialPaletteAsset.materials[num]);
					if (material == null)
					{
						string text = "Object \"{0}\" with palette \"{1}\" has invalid material at index {2}";
						ObjectAsset asset = this.asset;
						UnturnedLog.warn(string.Format(text, (asset != null) ? asset.FriendlyName : null, materialPaletteAsset.FriendlyName, num));
					}
				}
				else
				{
					string text2 = (this._transform != null) ? this._transform.position.ToString() : "(no transform)";
					string text3 = "Object \"{0}\" at {1} has invalid material palette {2}";
					ObjectAsset asset2 = this.asset;
					UnturnedLog.warn(string.Format(text3, (asset2 != null) ? asset2.FriendlyName : null, text2, materialPalette));
				}
			}
			return material;
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000A3E98 File Offset: 0x000A2098
		private void updateConditions()
		{
			if (this.asset == null)
			{
				return;
			}
			bool flag = true;
			if (flag && this.asset.holidayRestriction != ENPCHoliday.NONE)
			{
				flag = HolidayUtil.isHolidayActive(this.asset.holidayRestriction);
			}
			if (this.areConditionsMet != flag || !this.haveConditionsBeenChecked)
			{
				this.areConditionsMet = flag;
				this.haveConditionsBeenChecked = true;
				this.UpdateActiveAndRenderersEnabled();
			}
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000A3EF6 File Offset: 0x000A20F6
		private void onExternalConditionsUpdated()
		{
			this.updateConditions();
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000A3EFE File Offset: 0x000A20FE
		private void OnLocalPlayerQuestsChanged(ushort id)
		{
			this.updateConditions();
		}

		/// <summary>
		/// Used if the object asset has weather blend alpha conditions.
		/// </summary>
		// Token: 0x06002764 RID: 10084 RVA: 0x000A3F06 File Offset: 0x000A2106
		private void OnWeatherBlendAlphaChanged(WeatherAssetBase weatherAsset, float blendAlpha)
		{
			this.updateConditions();
		}

		/// <summary>
		/// Used if the object asset has weather status conditions.
		/// </summary>
		// Token: 0x06002765 RID: 10085 RVA: 0x000A3F0E File Offset: 0x000A210E
		private void OnWeatherStatusChanged(WeatherAssetBase weatherAsset, EWeatherStatusChange statusChange)
		{
			this.updateConditions();
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x000A3F16 File Offset: 0x000A2116
		private void onFlagsUpdated()
		{
			this.updateConditions();
		}

		/// <summary>
		/// Callback when an individual quest flag changes for the local player.
		/// Refreshes visibility conditions if the flag was relevant to this object.
		/// </summary>
		// Token: 0x06002767 RID: 10087 RVA: 0x000A3F1E File Offset: 0x000A211E
		private void onFlagUpdated(ushort id)
		{
			if (this.associatedFlags != null && this.associatedFlags.Contains(id))
			{
				this.updateConditions();
			}
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000A3F3C File Offset: 0x000A213C
		private void onPlayerCreated(Player player)
		{
			if (player.channel.IsLocalPlayer)
			{
				Player.onPlayerCreated = (PlayerCreated)Delegate.Remove(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
				bool flag = false;
				bool flag2 = false;
				foreach (INPCCondition inpccondition in this.asset.conditions)
				{
					if (inpccondition is NPCTimeOfDayCondition || inpccondition is NPCIsFullMoonCondition || inpccondition is NPCDateCounterCondition)
					{
						flag = true;
					}
					else if (inpccondition is NPCQuestCondition)
					{
						flag2 = true;
					}
				}
				foreach (INPCCondition inpccondition2 in this.asset.conditions)
				{
					NPCWeatherBlendAlphaCondition npcweatherBlendAlphaCondition = inpccondition2 as NPCWeatherBlendAlphaCondition;
					if (npcweatherBlendAlphaCondition != null)
					{
						WeatherEventListenerManager.AddBlendAlphaListener(npcweatherBlendAlphaCondition.weather.GUID, new WeatherBlendAlphaChangedListener(this.OnWeatherBlendAlphaChanged));
					}
					else
					{
						NPCWeatherStatusCondition npcweatherStatusCondition = inpccondition2 as NPCWeatherStatusCondition;
						if (npcweatherStatusCondition != null)
						{
							WeatherEventListenerManager.AddStatusListener(npcweatherStatusCondition.weather.GUID, new WeatherStatusChangedListener(this.OnWeatherStatusChanged));
						}
					}
				}
				if (flag)
				{
					PlayerQuests quests = Player.player.quests;
					quests.onExternalConditionsUpdated = (ExternalConditionsUpdated)Delegate.Combine(quests.onExternalConditionsUpdated, new ExternalConditionsUpdated(this.onExternalConditionsUpdated));
				}
				PlayerQuests quests2 = Player.player.quests;
				quests2.onFlagsUpdated = (FlagsUpdated)Delegate.Combine(quests2.onFlagsUpdated, new FlagsUpdated(this.onFlagsUpdated));
				this.associatedFlags = this.asset.GetConditionAssociatedFlags();
				if (this.associatedFlags != null)
				{
					PlayerQuests quests3 = Player.player.quests;
					quests3.onFlagUpdated = (FlagUpdated)Delegate.Combine(quests3.onFlagUpdated, new FlagUpdated(this.onFlagUpdated));
				}
				if (flag2)
				{
					Player.player.quests.OnLocalPlayerQuestsChanged += new Action<ushort>(this.OnLocalPlayerQuestsChanged);
				}
				this.updateConditions();
			}
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000A4104 File Offset: 0x000A2304
		private void LoadAsset()
		{
			if (!Assets.shouldLoadAnyAssets)
			{
				this._asset = null;
				return;
			}
			if (!(this.GUID == Guid.Empty))
			{
				this._asset = Assets.find<ObjectAsset>(new AssetReference<ObjectAsset>(this.GUID));
				if (this.asset == null)
				{
					ClientAssetIntegrity.ServerAddKnownMissingAsset(this.GUID, "Object");
					this._asset = (Assets.find(EAssetType.OBJECT, this.id) as ObjectAsset);
					if (this.asset != null)
					{
						UnturnedLog.info(string.Format("Unable to find object for GUID {0:N} found by legacy ID {1}, updating to {2:N} \"{3}\"", new object[]
						{
							this.GUID,
							this.id,
							this.asset.GUID,
							this.asset.FriendlyName
						}));
						this._GUID = this.asset.GUID;
						return;
					}
					UnturnedLog.warn(string.Format("Unable to find object for GUID {0:N}, nor by legacy ID {1}", this.GUID, this.id));
				}
				return;
			}
			this._asset = (Assets.find(EAssetType.OBJECT, this.id) as ObjectAsset);
			if (this.asset != null)
			{
				UnturnedLog.info("Object without GUID loaded by legacy ID {0}, updating to {1} \"{2}\"", new object[]
				{
					this.asset.id,
					this.asset.GUID,
					this.asset.FriendlyName
				});
				this._GUID = this.asset.GUID;
				return;
			}
			UnturnedLog.warn("Unable to find object by legacy ID {0}", new object[]
			{
				this.id
			});
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000A42A8 File Offset: 0x000A24A8
		[Obsolete]
		public LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, string newName, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID) : this(newPoint, newRotation, newScale, newID, newName, newGUID, newPlacementOrigin, newInstanceID, AssetReference<MaterialPaletteAsset>.invalid, -1, false)
		{
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000A42D0 File Offset: 0x000A24D0
		[Obsolete]
		public LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, string newName, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID, AssetReference<MaterialPaletteAsset> customMaterialOverride, int materialIndexOverride, bool newIsHierarchyItem) : this(newPoint, newRotation, newScale, newID, newGUID, newPlacementOrigin, newInstanceID, customMaterialOverride, materialIndexOverride, null, NetId.INVALID)
		{
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000A42F8 File Offset: 0x000A24F8
		[Obsolete]
		internal LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID, AssetReference<MaterialPaletteAsset> customMaterialOverride, int materialIndexOverride, DevkitHierarchyWorldObject devkitOwner, NetId netId) : this(newPoint, newRotation, newScale, newID, newGUID, newPlacementOrigin, newInstanceID, customMaterialOverride, materialIndexOverride, netId, true)
		{
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000A4320 File Offset: 0x000A2520
		internal LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID, AssetReference<MaterialPaletteAsset> customMaterialOverride, int materialIndexOverride, NetId netId, bool isOwnedCullingVolumeAllowed)
		{
			this._id = newID;
			this._GUID = newGUID;
			this._instanceID = newInstanceID;
			this.placementOrigin = newPlacementOrigin;
			this.customMaterialOverride = customMaterialOverride;
			this.materialIndexOverride = materialIndexOverride;
			this.isOwnedCullingVolumeAllowed = isOwnedCullingVolumeAllowed;
			this.LoadAsset();
			if (this.asset == null)
			{
				if (LevelObjects.preserveMissingAssets)
				{
					this.placeholderTransform = new GameObject().transform;
					this.placeholderTransform.position = newPoint;
					this.placeholderTransform.rotation = newRotation;
					this.placeholderTransform.localScale = newScale;
				}
				return;
			}
			this.state = this.asset.getState();
			this.areConditionsMet = true;
			this.haveConditionsBeenChecked = false;
			GameObject orLoadModel = this.asset.GetOrLoadModel();
			if (orLoadModel != null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(orLoadModel, newPoint, newRotation);
				this._transform = gameObject.transform;
				gameObject.name = this.asset.name;
				NetIdRegistry.AssignTransform(netId, this._transform);
				this.isDecal = this.transform.Find("Decal");
				if (this.isDecal)
				{
					gameObject.hideFlags = HideFlags.None;
				}
				if (this.asset.useScale)
				{
					this.transform.localScale = newScale;
				}
			}
			this.renderers = null;
			if (this.transform != null)
			{
				if (this.isDecal && !Level.isEditor && this.asset.interactability == EObjectInteractability.NONE && this.asset.rubble == EObjectRubble.NONE)
				{
					Collider component = this.transform.GetComponent<Collider>();
					if (component != null)
					{
						Object.Destroy(component);
					}
				}
				if (Level.isEditor)
				{
					Rigidbody orAddComponent = this.transform.GetOrAddComponent<Rigidbody>();
					orAddComponent.useGravity = false;
					orAddComponent.isKinematic = true;
				}
				else
				{
					Rigidbody component2 = this.transform.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						Object.Destroy(component2);
					}
					if (this.asset.type == EObjectType.SMALL && this.asset.interactability == EObjectInteractability.NONE && this.asset.rubble == EObjectRubble.NONE)
					{
						Collider component3 = this.transform.GetComponent<Collider>();
						if (component3 != null)
						{
							Object.Destroy(component3);
						}
					}
				}
				if ((Level.isEditor || Provider.isServer) && this.asset.type != EObjectType.SMALL)
				{
					IDeferredAsset<GameObject> navGameObject = this.asset.navGameObject;
					GameObject gameObject2 = (navGameObject != null) ? navGameObject.getOrLoad() : null;
					if (gameObject2 != null)
					{
						Transform transform = Object.Instantiate<GameObject>(gameObject2).transform;
						transform.name = "Nav";
						transform.parent = this.transform;
						transform.localPosition = Vector3.zero;
						transform.localRotation = Quaternion.identity;
						transform.localScale = Vector3.one;
						if (Level.isEditor)
						{
							Rigidbody orAddComponent2 = transform.GetOrAddComponent<Rigidbody>();
							orAddComponent2.useGravity = false;
							orAddComponent2.isKinematic = true;
						}
						else
						{
							LevelObject.reuseableRigidbodyList.Clear();
							transform.GetComponentsInChildren<Rigidbody>(LevelObject.reuseableRigidbodyList);
							foreach (Rigidbody obj in LevelObject.reuseableRigidbodyList)
							{
								Object.Destroy(obj);
							}
						}
					}
				}
				if (Provider.isServer)
				{
					IDeferredAsset<GameObject> triggersGameObject = this.asset.triggersGameObject;
					GameObject gameObject3 = (triggersGameObject != null) ? triggersGameObject.getOrLoad() : null;
					Transform transform3;
					if (gameObject3 != null)
					{
						Transform transform2 = Object.Instantiate<GameObject>(gameObject3).transform;
						transform2.name = "Triggers";
						transform2.parent = this.transform;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.localScale = Vector3.one;
						transform3 = transform2;
					}
					else
					{
						transform3 = this.transform;
					}
					if (this.asset.shouldAddKillTriggers)
					{
						foreach (object obj2 in transform3)
						{
							Transform transform4 = (Transform)obj2;
							if (transform4.name.Equals("Kill", 5))
							{
								transform4.tag = "Trap";
								transform4.gameObject.layer = 30;
								transform4.gameObject.AddComponent<Barrier>();
							}
						}
					}
				}
				if (this.asset.type != EObjectType.SMALL)
				{
					if (Level.isEditor)
					{
						Transform transform5 = this.transform.Find("Block");
						if (transform5 != null && this.transform.GetComponent<Collider>() == null)
						{
							BoxCollider component4 = transform5.GetComponent<BoxCollider>();
							if (component4 != null)
							{
								BoxCollider boxCollider = this.transform.gameObject.AddComponent<BoxCollider>();
								boxCollider.center = component4.center;
								boxCollider.size = component4.size;
							}
						}
					}
					else if (Provider.isClient)
					{
						IDeferredAsset<GameObject> slotsGameObject = this.asset.slotsGameObject;
						GameObject gameObject4 = (slotsGameObject != null) ? slotsGameObject.getOrLoad() : null;
						if (gameObject4 != null)
						{
							Transform transform6 = Object.Instantiate<GameObject>(gameObject4).transform;
							transform6.name = "Slots";
							transform6.parent = this.transform;
							transform6.localPosition = Vector3.zero;
							transform6.localRotation = Quaternion.identity;
							transform6.localScale = Vector3.one;
							LevelObject.reuseableRigidbodyList.Clear();
							transform6.GetComponentsInChildren<Rigidbody>(LevelObject.reuseableRigidbodyList);
							foreach (Rigidbody obj3 in LevelObject.reuseableRigidbodyList)
							{
								Object.Destroy(obj3);
							}
						}
					}
				}
				if (this.asset.interactability != EObjectInteractability.NONE)
				{
					if (this.asset.interactability == EObjectInteractability.BINARY_STATE)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectBinaryState>();
					}
					else if (this.asset.interactability == EObjectInteractability.DROPPER)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectDropper>();
					}
					else if (this.asset.interactability == EObjectInteractability.NOTE)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectNote>();
					}
					else if (this.asset.interactability == EObjectInteractability.WATER || this.asset.interactability == EObjectInteractability.FUEL)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectResource>();
					}
					else if (this.asset.interactability == EObjectInteractability.NPC)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectNPC>();
					}
					else if (this.asset.interactability == EObjectInteractability.QUEST)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectQuest>();
					}
					if (this.interactable != null)
					{
						this.interactable.updateState(this.asset, this.state);
					}
				}
				if (this.asset.rubble != EObjectRubble.NONE)
				{
					if (this.asset.rubble == EObjectRubble.DESTROY)
					{
						this._rubble = this.transform.gameObject.AddComponent<InteractableObjectRubble>();
					}
					if (this.rubble != null)
					{
						this.rubble.updateState(this.asset, this.state);
					}
					if (this.asset.rubbleEditor == EObjectRubbleEditor.DEAD && Level.isEditor)
					{
						Transform transform7 = this.transform.Find("Editor");
						if (transform7 != null)
						{
							transform7.gameObject.SetActive(true);
						}
					}
				}
				bool flag = false;
				if (this.asset.conditions != null && this.asset.conditions.Length != 0)
				{
					bool isEditor = Level.isEditor;
				}
				if (!flag && (this.asset.holidayRestriction != ENPCHoliday.NONE || this.asset.isGore) && !Level.isEditor)
				{
					this.areConditionsMet = false;
					this.updateConditions();
				}
				if (this.asset.foliage.isValid)
				{
					FoliageSurfaceComponent foliageSurfaceComponent = this.transform.gameObject.AddComponent<FoliageSurfaceComponent>();
					foliageSurfaceComponent.foliage = this.asset.foliage;
					foliageSurfaceComponent.surfaceCollider = this.transform.gameObject.GetComponent<Collider>();
				}
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000A4B24 File Offset: 0x000A2D24
		internal void UpdateActiveAndRenderersEnabled()
		{
			this.isCollisionEnabled = this.isActiveInRegion;
			this.isVisualEnabled = this.isActiveInRegion;
			bool active;
			if (this.isDecal || (this.asset != null && this.asset.type == EObjectType.NPC))
			{
				if (this.isActiveInRegion)
				{
					bool isVisibleInCullingVolume = this.isVisibleInCullingVolume;
				}
				active = this.areConditionsMet;
			}
			else
			{
				bool flag = (this.asset != null && this.asset.isCollisionImportant && Provider.isServer) || true;
				active = ((this.isActiveInRegion || flag) && this.areConditionsMet);
				if (this.isActiveInRegion && (this.isVisibleInCullingVolume || LevelObject.disableCullingVolumes))
				{
					bool flag2 = this.areConditionsMet;
				}
			}
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(active);
			}
			this.UpdateSkyboxActive();
		}

		/// <summary>
		/// Separate from UpdateActiveAndRenderersEnabled so graphics settings can call it.
		/// </summary>
		// Token: 0x0600276F RID: 10095 RVA: 0x000A4BFF File Offset: 0x000A2DFF
		internal void UpdateSkyboxActive()
		{
			this.isSkyboxEnabled = !this.isActiveInRegion;
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x000A4C10 File Offset: 0x000A2E10
		[Obsolete]
		public string name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000A4C13 File Offset: 0x000A2E13
		[Obsolete("Replaced by SetIsActiveInRegion(true)")]
		public void enableCollision()
		{
			this.SetIsActiveInRegion(true);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000A4C1C File Offset: 0x000A2E1C
		[Obsolete("Replaced by SetIsActiveInRegion(true)")]
		public void enableVisual()
		{
			this.SetIsActiveInRegion(true);
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000A4C25 File Offset: 0x000A2E25
		[Obsolete("Replaced by SetIsActiveInRegion(false)")]
		public void enableSkybox()
		{
			this.SetIsActiveInRegion(false);
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000A4C2E File Offset: 0x000A2E2E
		[Obsolete("Replaced by SetIsActiveInRegion(false)")]
		public void disableCollision()
		{
			this.SetIsActiveInRegion(false);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000A4C37 File Offset: 0x000A2E37
		[Obsolete("Replaced by SetIsActiveInRegion(false)")]
		public void disableVisual()
		{
			this.SetIsActiveInRegion(false);
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000A4C40 File Offset: 0x000A2E40
		[Obsolete("Replaced by SetIsActiveInRegion(true)")]
		public void disableSkybox()
		{
			this.SetIsActiveInRegion(true);
		}

		// Token: 0x040014D2 RID: 5330
		private static List<Rigidbody> reuseableRigidbodyList = new List<Rigidbody>();

		/// <summary>
		/// If true, object is within a culling volume.
		/// Name is old and not very specific, but not changing because it's public.
		/// </summary>
		// Token: 0x040014D3 RID: 5331
		public bool isSpeciallyCulled;

		// Token: 0x040014D4 RID: 5332
		private bool isDecal;

		// Token: 0x040014D5 RID: 5333
		private Transform _transform;

		// Token: 0x040014D7 RID: 5335
		private Transform _skybox;

		// Token: 0x040014D8 RID: 5336
		private List<Renderer> renderers;

		// Token: 0x040014D9 RID: 5337
		private ushort _id;

		// Token: 0x040014DA RID: 5338
		private Guid _GUID;

		// Token: 0x040014DB RID: 5339
		private uint _instanceID;

		// Token: 0x040014DC RID: 5340
		internal AssetReference<MaterialPaletteAsset> customMaterialOverride;

		// Token: 0x040014DD RID: 5341
		internal int materialIndexOverride = -1;

		/// <summary>
		/// If true, <see cref="F:SDG.Unturned.LevelObject.ownedCullingVolume" /> can be instantiated. Defaults to true.
		/// Enables mappers to remove culling volumes embedded in objects if they're causing issues.
		/// </summary>
		// Token: 0x040014DE RID: 5342
		internal bool isOwnedCullingVolumeAllowed;

		// Token: 0x040014DF RID: 5343
		public byte[] state;

		// Token: 0x040014E0 RID: 5344
		private ObjectAsset _asset;

		// Token: 0x040014E1 RID: 5345
		private InteractableObject _interactableObj;

		// Token: 0x040014E2 RID: 5346
		private InteractableObjectRubble _rubble;

		// Token: 0x040014E7 RID: 5351
		private bool areConditionsMet;

		// Token: 0x040014E8 RID: 5352
		private bool haveConditionsBeenChecked;

		// Token: 0x040014EB RID: 5355
		private HashSet<ushort> associatedFlags;

		// Token: 0x040014EC RID: 5356
		private static CommandLineFlag disableCullingVolumes = new CommandLineFlag(false, "-DisableCullingVolumes");
	}
}
