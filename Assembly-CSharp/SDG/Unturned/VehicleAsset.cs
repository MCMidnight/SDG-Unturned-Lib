using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200037C RID: 892
	public class VehicleAsset : Asset, ISkinableAsset
	{
		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x00060596 File Offset: 0x0005E796
		public bool shouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x0006059E File Offset: 0x0005E79E
		internal override bool ShouldVerifyHash
		{
			get
			{
				return this._shouldVerifyHash;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x000605A6 File Offset: 0x0005E7A6
		public string vehicleName
		{
			get
			{
				return this._vehicleName;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x000605AE File Offset: 0x0005E7AE
		public override string FriendlyName
		{
			get
			{
				return this._vehicleName;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001ABE RID: 6846 RVA: 0x000605B6 File Offset: 0x0005E7B6
		public float size2_z
		{
			get
			{
				return this._size2_z;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x000605BE File Offset: 0x0005E7BE
		public string sharedSkinName
		{
			get
			{
				return this._sharedSkinName;
			}
		}

		/// <summary>
		/// Please refer to: <seealso cref="M:SDG.Unturned.VehicleAsset.FindSharedSkinVehicleAsset" />
		/// </summary>
		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x000605C6 File Offset: 0x0005E7C6
		public Guid SharedSkinLookupGuid
		{
			get
			{
				return this._sharedSkinLookupGuid;
			}
		}

		/// <summary>
		/// Please refer to: <seealso cref="M:SDG.Unturned.VehicleAsset.FindSharedSkinVehicleAsset" />
		/// </summary>
		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x000605CE File Offset: 0x0005E7CE
		[Obsolete]
		public ushort sharedSkinLookupID
		{
			get
			{
				return this._sharedSkinLookupID;
			}
		}

		/// <summary>
		/// Supports redirects by VehicleRedirectorAsset.
		///
		/// "Shared Skins" were implemented when there were several asset variants of each vehicle. For example,
		/// Off_Roader_Orange, Off_Roader_Purple, Off_Roader_Green, etc. Each vehicle had their "shared skin" set to
		/// the same ID, and the skin asset had its target ID set to the shared ID. This isn't as necessary after
		/// merging vanilla vehicle variants, but some mods may rely on it, and it needed GUID support now that the
		/// target vehicle might not have a legacy ID.
		/// </summary>
		// Token: 0x06001AC2 RID: 6850 RVA: 0x000605D8 File Offset: 0x0005E7D8
		public VehicleAsset FindSharedSkinVehicleAsset()
		{
			Asset asset = Assets.FindBaseVehicleAssetByGuidOrLegacyId(this._sharedSkinLookupGuid, this._sharedSkinLookupID);
			VehicleRedirectorAsset vehicleRedirectorAsset = asset as VehicleRedirectorAsset;
			if (vehicleRedirectorAsset != null)
			{
				asset = vehicleRedirectorAsset.TargetVehicle.Find();
			}
			return asset as VehicleAsset;
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x00060616 File Offset: 0x0005E816
		public EEngine engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x06001AC4 RID: 6852 RVA: 0x0006061E File Offset: 0x0005E81E
		public EItemRarity rarity
		{
			get
			{
				return this._rarity;
			}
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00060628 File Offset: 0x0005E828
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

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00060698 File Offset: 0x0005E898
		protected void onModelLoaded(GameObject asset)
		{
			Transform transform = asset.transform.Find("Seats");
			if (transform == null)
			{
				Assets.reportError(this, "missing 'Seats' Transform");
			}
			else if (transform.childCount < 1)
			{
				Assets.reportError(this, "empty 'Seats' Transform has zero children");
			}
			Rigidbody component = asset.GetComponent<Rigidbody>();
			if (component == null)
			{
				Assets.reportError(this, "missing root Rigidbody");
			}
			else if (this.physicsProfileRef.isNull && MathfEx.IsNearlyEqual(component.mass, 1f, 0.01f))
			{
				bool flag = true;
				Transform transform2 = asset.transform.Find("Tires");
				if (transform2 != null)
				{
					for (int i = 0; i < transform2.childCount; i++)
					{
						Transform child = transform2.GetChild(i);
						if (!(child == null))
						{
							WheelCollider component2 = child.GetComponent<WheelCollider>();
							if (!(component2 == null) && !MathfEx.IsNearlyEqual(component2.mass, 1f, 0.01f))
							{
								flag = false;
								break;
							}
						}
					}
				}
				if (flag)
				{
					switch (this.engine)
					{
					case EEngine.CAR:
						this.physicsProfileRef = VehiclePhysicsProfileAsset.defaultProfile_Car;
						break;
					case EEngine.PLANE:
						this.physicsProfileRef = VehiclePhysicsProfileAsset.defaultProfile_Plane;
						break;
					case EEngine.HELICOPTER:
						this.physicsProfileRef = VehiclePhysicsProfileAsset.defaultProfile_Helicopter;
						break;
					case EEngine.BOAT:
						this.physicsProfileRef = VehiclePhysicsProfileAsset.defaultProfile_Boat;
						break;
					}
				}
			}
			asset.SetTagIfUntaggedRecursively("Vehicle");
			if (this.wheelConfiguration == null)
			{
				this.BuildAutomaticWheelConfiguration(asset);
			}
		}

		/// <summary>
		/// Clip.prefab
		/// </summary>
		// Token: 0x06001AC7 RID: 6855 RVA: 0x00060819 File Offset: 0x0005EA19
		protected void OnServerModelLoaded(GameObject asset)
		{
			if (asset == null)
			{
				Assets.reportError(this, "missing \"Clip\" GameObject, loading \"Vehicle\" GameObject instead");
				return;
			}
			this._hasHeadlights = true;
			this._hasSirens = true;
			this._hasHook = true;
			this.onModelLoaded(asset);
		}

		/// <summary>
		/// Vehicle.prefab
		/// </summary>
		// Token: 0x06001AC8 RID: 6856 RVA: 0x0006084C File Offset: 0x0005EA4C
		protected void OnClientModelLoaded(GameObject asset)
		{
			if (asset == null)
			{
				Assets.reportError(this, "missing \"Vehicle\" GameObject");
				return;
			}
			AssetValidation.searchGameObjectForErrors(this, asset);
			this._hasHeadlights = (asset.transform.Find("Headlights") != null);
			this._hasSirens = (asset.transform.Find("Sirens") != null);
			this._hasHook = (asset.transform.Find("Hook") != null);
			if (this._pitchIdle < 0f)
			{
				this._pitchIdle = 0.5f;
				AudioSource component = asset.GetComponent<AudioSource>();
				if (component != null)
				{
					AudioClip clip = component.clip;
					if (clip != null)
					{
						if (clip.name == "Engine_Large")
						{
							this._pitchIdle = 0.625f;
						}
						else if (clip.name == "Engine_Small")
						{
							this._pitchIdle = 0.75f;
						}
					}
				}
			}
			if (this._pitchDrive < 0f)
			{
				if (this.engine == EEngine.HELICOPTER)
				{
					this._pitchDrive = 0.03f;
				}
				else if (this.engine == EEngine.BLIMP)
				{
					this._pitchDrive = 0.1f;
				}
				else
				{
					this._pitchDrive = 0.05f;
					AudioSource component2 = asset.GetComponent<AudioSource>();
					if (component2 != null)
					{
						AudioClip clip2 = component2.clip;
						if (clip2 != null)
						{
							if (clip2.name == "Engine_Large")
							{
								this._pitchDrive = 0.025f;
							}
							else if (clip2.name == "Engine_Small")
							{
								this._pitchDrive = 0.075f;
							}
						}
					}
				}
			}
			this.onModelLoaded(asset);
			ServerPrefabUtil.RemoveClientComponents(asset);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x000609F0 File Offset: 0x0005EBF0
		public void DebugDumpWheelConfigurationToStringBuilder(StringBuilder output)
		{
			output.Append(this.vehicleName);
			if (this.wheelConfiguration == null || this.wheelConfiguration.Length < 1)
			{
				output.AppendLine(" wheel configuration(s): N/A");
				return;
			}
			output.AppendLine(" wheel configuration(s):");
			for (int i = 0; i < this.wheelConfiguration.Length; i++)
			{
				output.Append(i);
				output.AppendLine(":");
				VehicleWheelConfiguration vehicleWheelConfiguration = this.wheelConfiguration[i];
				output.Append("Wheel collider path: \"");
				output.Append(vehicleWheelConfiguration.wheelColliderPath);
				output.AppendLine("\"");
				output.Append("Is collider steered: ");
				output.Append(vehicleWheelConfiguration.isColliderSteered);
				output.AppendLine();
				output.Append("Is collider powered: ");
				output.Append(vehicleWheelConfiguration.isColliderPowered);
				output.AppendLine();
				output.Append("Model path: \"");
				output.Append(vehicleWheelConfiguration.modelPath);
				output.AppendLine("\"");
				output.Append("Is model steered: ");
				output.Append(vehicleWheelConfiguration.isModelSteered);
				output.AppendLine();
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00060B18 File Offset: 0x0005ED18
		public string DebugDumpWheelConfigurationToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.DebugDumpWheelConfigurationToStringBuilder(stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00060B38 File Offset: 0x0005ED38
		private void LogWheelConfigurationDatConversion()
		{
			string message;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (DatWriter datWriter = new DatWriter(stringWriter))
				{
					datWriter.WriteListStart("WheelConfigurations");
					foreach (VehicleWheelConfiguration vehicleWheelConfiguration in this.wheelConfiguration)
					{
						datWriter.WriteDictionaryStart();
						datWriter.WriteKeyValue("WheelColliderPath", vehicleWheelConfiguration.wheelColliderPath, null);
						datWriter.WriteKeyValue("IsColliderSteered", vehicleWheelConfiguration.isColliderSteered, null);
						datWriter.WriteKeyValue("IsColliderPowered", vehicleWheelConfiguration.isColliderPowered, null);
						datWriter.WriteKeyValue("ModelPath", vehicleWheelConfiguration.modelPath, null);
						datWriter.WriteKeyValue("IsModelSteered", vehicleWheelConfiguration.isModelSteered, null);
						datWriter.WriteDictionaryEnd();
					}
					datWriter.WriteListEnd();
					message = stringWriter.ToString();
				}
			}
			UnturnedLog.info("Converted \"" + this.FriendlyName + "\" wheel configuration:");
			UnturnedLog.info(message);
		}

		/// <summary>
		/// Nelson 2024-02-28: Prior to the VehicleWheelConfiguration class, most of the wheel configuration was
		/// inferred during InteractableVehicle initialization from the children of the "Tires" and "Wheels" transforms.
		/// Confusingly, "Tires" only contains WheelColliders and "Wheels" only contains the visual models. Rather than
		/// keeping the old behavior in InteractableVehicle alongside the newer more configurable one, we match the old
		/// behavior here to generate an equivalent configuration.
		///
		/// Note that <see cref="P:SDG.Unturned.VehicleAsset.steeringTireIndices" /> must be initialized before this is called (by loading model).
		/// </summary>
		// Token: 0x06001ACC RID: 6860 RVA: 0x00060C48 File Offset: 0x0005EE48
		private void BuildAutomaticWheelConfiguration(GameObject vehicleGameObject)
		{
			Transform transform = vehicleGameObject.transform;
			List<VehicleWheelConfiguration> list = new List<VehicleWheelConfiguration>();
			Transform transform2 = transform.Find("Tires");
			if (transform2 != null)
			{
				for (int i = 0; i < transform2.childCount; i++)
				{
					string text = "Tire_" + i.ToString();
					Transform transform3 = transform2.Find(text);
					if (transform3 == null)
					{
						Assets.reportError(this, "missing \"{0}\" Transform", text);
					}
					else if (transform3.GetComponent<WheelCollider>() == null)
					{
						Assets.reportError(this, "missing \"{0}\" WheelCollider", text);
					}
					else
					{
						list.Add(new VehicleWheelConfiguration
						{
							wasAutomaticallyGenerated = true,
							wheelColliderPath = "Tires/" + text,
							isColliderSteered = (i < 2),
							isColliderPowered = (i >= transform2.childCount - 2)
						});
					}
				}
			}
			Transform transform4 = transform.Find("Wheels");
			if (transform4 != null)
			{
				foreach (VehicleWheelConfiguration vehicleWheelConfiguration in list)
				{
					Transform transform5 = transform.Find(vehicleWheelConfiguration.wheelColliderPath);
					if (!(transform5 == null))
					{
						int num = -1;
						float num2 = 16f;
						for (int j = 0; j < transform4.childCount; j++)
						{
							Transform child = transform4.GetChild(j);
							float sqrMagnitude = (transform5.position - child.position).sqrMagnitude;
							if (sqrMagnitude < num2)
							{
								num = j;
								num2 = sqrMagnitude;
							}
						}
						if (num != -1)
						{
							Transform transform6 = transform4.GetChild(num);
							if (transform6.childCount < 1)
							{
								Transform transform7 = transform.FindChildRecursive("Wheel_" + num.ToString());
								if (transform7 != null)
								{
									transform6 = transform7;
								}
							}
							string text2 = transform6.name;
							Transform parent = transform6.parent;
							while (parent != transform)
							{
								text2 = parent.name + "/" + text2;
								parent = parent.parent;
							}
							vehicleWheelConfiguration.modelPath = text2;
						}
					}
				}
				foreach (object obj in transform4)
				{
					Transform transform8 = (Transform)obj;
					if (transform8.childCount >= 1)
					{
						bool flag = false;
						foreach (VehicleWheelConfiguration vehicleWheelConfiguration2 in list)
						{
							if (!string.IsNullOrEmpty(vehicleWheelConfiguration2.modelPath))
							{
								Transform x = transform.Find(vehicleWheelConfiguration2.modelPath);
								if (!(x == null) && x == transform8)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							list.Add(new VehicleWheelConfiguration
							{
								wasAutomaticallyGenerated = true,
								modelPath = "Wheels/" + transform8.name
							});
						}
					}
				}
				if (this.steeringTireIndices != null)
				{
					foreach (int num3 in this.steeringTireIndices)
					{
						string text3 = "Wheel_" + num3.ToString();
						Transform transform9 = transform4.Find(text3);
						if (transform9 == null)
						{
							transform9 = transform.FindChildRecursive(text3);
							if (transform9 == null && num3 < transform4.childCount)
							{
								transform9 = transform4.GetChild(num3);
							}
						}
						if (!(transform9 == null))
						{
							VehicleWheelConfiguration vehicleWheelConfiguration3 = null;
							foreach (VehicleWheelConfiguration vehicleWheelConfiguration4 in list)
							{
								if (!string.IsNullOrEmpty(vehicleWheelConfiguration4.modelPath))
								{
									Transform x2 = transform.Find(vehicleWheelConfiguration4.modelPath);
									if (!(x2 == null) && x2 == transform9)
									{
										vehicleWheelConfiguration3 = vehicleWheelConfiguration4;
										break;
									}
								}
							}
							if (vehicleWheelConfiguration3 != null)
							{
								vehicleWheelConfiguration3.isModelSteered = true;
							}
							else
							{
								Assets.reportError(this, "unable to match physical tire with steering tire model {0}", num3);
							}
						}
					}
				}
			}
			this.wheelConfiguration = list.ToArray();
			if (VehicleAsset.clLogWheelConfiguration)
			{
				this.LogWheelConfigurationDatConversion();
			}
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x000610F4 File Offset: 0x0005F2F4
		public AudioClip ignition
		{
			get
			{
				return this._ignition;
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001ACE RID: 6862 RVA: 0x000610FC File Offset: 0x0005F2FC
		public AudioClip horn
		{
			get
			{
				return this._horn;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x00061104 File Offset: 0x0005F304
		// (set) Token: 0x06001AD0 RID: 6864 RVA: 0x0006110C File Offset: 0x0005F30C
		public bool hasHorn { get; protected set; }

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00061115 File Offset: 0x0005F315
		public float pitchIdle
		{
			get
			{
				return this._pitchIdle;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x0006111D File Offset: 0x0005F31D
		public float pitchDrive
		{
			get
			{
				return this._pitchDrive;
			}
		}

		/// <summary>
		/// Maximum (negative) velocity to aim for while accelerating backward.
		/// </summary>
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x00061125 File Offset: 0x0005F325
		// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x0006112D File Offset: 0x0005F32D
		public float TargetReverseVelocity { get; private set; }

		/// <summary>
		/// Maximum speed to aim for while accelerating backward.
		/// </summary>
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x00061136 File Offset: 0x0005F336
		public float TargetReverseSpeed
		{
			get
			{
				return Mathf.Abs(this.TargetReverseVelocity);
			}
		}

		/// <summary>
		/// Maximum velocity to aim for while accelerating forward.
		/// </summary>
		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001AD6 RID: 6870 RVA: 0x00061143 File Offset: 0x0005F343
		// (set) Token: 0x06001AD7 RID: 6871 RVA: 0x0006114B File Offset: 0x0005F34B
		public float TargetForwardVelocity { get; private set; }

		/// <summary>
		/// Maximum speed to aim for while accelerating forward.
		/// </summary>
		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001AD8 RID: 6872 RVA: 0x00061154 File Offset: 0x0005F354
		public float TargetForwardSpeed
		{
			get
			{
				return Mathf.Abs(this.TargetForwardVelocity);
			}
		}

		/// <summary>
		/// Steering angle range at zero speed.
		/// </summary>
		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x00061161 File Offset: 0x0005F361
		public float steerMin
		{
			get
			{
				return this._steerMin;
			}
		}

		/// <summary>
		/// Steering angle range at target maximum speed (for the current forward/backward direction).
		/// </summary>
		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00061169 File Offset: 0x0005F369
		public float steerMax
		{
			get
			{
				return this._steerMax;
			}
		}

		/// <summary>
		/// Steering angle rotation change in degrees per second.
		/// </summary>
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x00061171 File Offset: 0x0005F371
		// (set) Token: 0x06001ADC RID: 6876 RVA: 0x00061179 File Offset: 0x0005F379
		public float SteeringAngleTurnSpeed { get; private set; }

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00061182 File Offset: 0x0005F382
		public float brake
		{
			get
			{
				return this._brake;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0006118A File Offset: 0x0005F38A
		public float lift
		{
			get
			{
				return this._lift;
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x00061192 File Offset: 0x0005F392
		public ushort fuelMin
		{
			get
			{
				return this._fuelMin;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x0006119A File Offset: 0x0005F39A
		public ushort fuelMax
		{
			get
			{
				return this._fuelMax;
			}
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x000611A2 File Offset: 0x0005F3A2
		public ushort fuel
		{
			get
			{
				return this._fuel;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x000611AA File Offset: 0x0005F3AA
		public ushort healthMin
		{
			get
			{
				return this._healthMin;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x000611B2 File Offset: 0x0005F3B2
		public ushort healthMax
		{
			get
			{
				return this._healthMax;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001AE4 RID: 6884 RVA: 0x000611BA File Offset: 0x0005F3BA
		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x000611C2 File Offset: 0x0005F3C2
		public Guid ExplosionEffectGuid
		{
			get
			{
				return this._explosionEffectGuid;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001AE6 RID: 6886 RVA: 0x000611CA File Offset: 0x0005F3CA
		public ushort explosion
		{
			[Obsolete]
			get
			{
				return this._explosion;
			}
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x000611D2 File Offset: 0x0005F3D2
		public bool IsExplosionEffectRefNull()
		{
			return this._explosion == 0 && GuidExtension.IsEmpty(this._explosionEffectGuid);
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x000611E9 File Offset: 0x0005F3E9
		public EffectAsset FindExplosionEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._explosionEffectGuid, this._explosion);
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000611FC File Offset: 0x0005F3FC
		// (set) Token: 0x06001AEA RID: 6890 RVA: 0x00061204 File Offset: 0x0005F404
		public Vector3 minExplosionForce { get; set; }

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x0006120D File Offset: 0x0005F40D
		// (set) Token: 0x06001AEC RID: 6892 RVA: 0x00061215 File Offset: 0x0005F415
		public Vector3 maxExplosionForce { get; set; }

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0006121E File Offset: 0x0005F41E
		[Obsolete("Separated into ShouldExplosionCauseDamage and ShouldExplosionBurnMaterials.")]
		public bool isExplosive
		{
			get
			{
				return !this.IsExplosionEffectRefNull();
			}
		}

		/// <summary>
		/// If true, explosion will damage nearby entities and kill passengers.
		/// </summary>
		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x00061229 File Offset: 0x0005F429
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x00061231 File Offset: 0x0005F431
		public bool ShouldExplosionCauseDamage { get; protected set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0006123A File Offset: 0x0005F43A
		// (set) Token: 0x06001AF1 RID: 6897 RVA: 0x00061242 File Offset: 0x0005F442
		public bool ShouldExplosionBurnMaterials { get; protected set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x0006124B File Offset: 0x0005F44B
		public bool hasHeadlights
		{
			get
			{
				return this._hasHeadlights;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00061253 File Offset: 0x0005F453
		public bool hasSirens
		{
			get
			{
				return this._hasSirens;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x0006125B File Offset: 0x0005F45B
		public bool hasHook
		{
			get
			{
				return this._hasHook;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x00061263 File Offset: 0x0005F463
		public bool hasZip
		{
			get
			{
				return this._hasZip;
			}
		}

		/// <summary>
		/// When true the bicycle animation is used and extra speed is stamina powered.
		/// Bad way to implement it.
		/// </summary>
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0006126B File Offset: 0x0005F46B
		public bool hasBicycle
		{
			get
			{
				return this._hasBicycle;
			}
		}

		/// <summary>
		/// Can this vehicle ever spawn with a charged battery?
		/// Uses game mode battery stats when true, or overrides by preventing battery spawn when false.
		/// </summary>
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x00061273 File Offset: 0x0005F473
		// (set) Token: 0x06001AF8 RID: 6904 RVA: 0x0006127B File Offset: 0x0005F47B
		public bool canSpawnWithBattery { get; protected set; }

		/// <summary>
		/// Battery charge when first spawning in is multiplied by this [0, 1] number.
		/// </summary>
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x00061284 File Offset: 0x0005F484
		// (set) Token: 0x06001AFA RID: 6906 RVA: 0x0006128C File Offset: 0x0005F48C
		public float batterySpawnChargeMultiplier { get; protected set; }

		/// <summary>
		/// Battery decrease per second.
		/// </summary>
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x00061295 File Offset: 0x0005F495
		// (set) Token: 0x06001AFC RID: 6908 RVA: 0x0006129D File Offset: 0x0005F49D
		public float batteryBurnRate { get; protected set; }

		/// <summary>
		/// Battery increase per second.
		/// </summary>
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000612A6 File Offset: 0x0005F4A6
		// (set) Token: 0x06001AFE RID: 6910 RVA: 0x000612AE File Offset: 0x0005F4AE
		public float batteryChargeRate { get; protected set; }

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x000612B7 File Offset: 0x0005F4B7
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x000612BF File Offset: 0x0005F4BF
		public EBatteryMode batteryDriving { get; protected set; }

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x000612C8 File Offset: 0x0005F4C8
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x000612D0 File Offset: 0x0005F4D0
		public EBatteryMode batteryEmpty { get; protected set; }

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x000612D9 File Offset: 0x0005F4D9
		// (set) Token: 0x06001B04 RID: 6916 RVA: 0x000612E1 File Offset: 0x0005F4E1
		public EBatteryMode batteryHeadlights { get; protected set; }

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001B05 RID: 6917 RVA: 0x000612EA File Offset: 0x0005F4EA
		// (set) Token: 0x06001B06 RID: 6918 RVA: 0x000612F2 File Offset: 0x0005F4F2
		public EBatteryMode batterySirens { get; protected set; }

		/// <summary>
		/// Battery item given to the player when a specific battery hasn't been manually
		/// installed yet. Defaults to the vanilla car battery (098b13be34a7411db7736b7f866ada69).
		/// </summary>
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x000612FB File Offset: 0x0005F4FB
		// (set) Token: 0x06001B08 RID: 6920 RVA: 0x00061303 File Offset: 0x0005F503
		public Guid defaultBatteryGuid { get; protected set; }

		/// <summary>
		/// Fuel decrease per second.
		/// </summary>
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x0006130C File Offset: 0x0005F50C
		// (set) Token: 0x06001B0A RID: 6922 RVA: 0x00061314 File Offset: 0x0005F514
		public float fuelBurnRate { get; protected set; }

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001B0B RID: 6923 RVA: 0x0006131D File Offset: 0x0005F51D
		// (set) Token: 0x06001B0C RID: 6924 RVA: 0x00061325 File Offset: 0x0005F525
		public bool isReclined { get; protected set; }

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001B0D RID: 6925 RVA: 0x0006132E File Offset: 0x0005F52E
		public bool hasLockMouse
		{
			get
			{
				return this._hasLockMouse;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001B0E RID: 6926 RVA: 0x00061336 File Offset: 0x0005F536
		public bool hasTraction
		{
			get
			{
				return this._hasTraction;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001B0F RID: 6927 RVA: 0x0006133E File Offset: 0x0005F53E
		public bool hasSleds
		{
			get
			{
				return this._hasSleds;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001B10 RID: 6928 RVA: 0x00061346 File Offset: 0x0005F546
		public float exit
		{
			get
			{
				return this._exit;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0006134E File Offset: 0x0005F54E
		public float sqrDelta
		{
			get
			{
				return this._sqrDelta;
			}
		}

		/// <summary>
		/// Client sends physics simulation results to server. If upward (+Y) speed exceeds this, mark the move invalid.
		/// </summary>
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x00061356 File Offset: 0x0005F556
		// (set) Token: 0x06001B13 RID: 6931 RVA: 0x0006135E File Offset: 0x0005F55E
		public float validSpeedUp { get; protected set; }

		/// <summary>
		/// Client sends physics simulation results to server. If downward (-Y) speed exceeds this, mark the move invalid.
		/// </summary>
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x00061367 File Offset: 0x0005F567
		// (set) Token: 0x06001B15 RID: 6933 RVA: 0x0006136F File Offset: 0x0005F56F
		public float validSpeedDown { get; protected set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x00061378 File Offset: 0x0005F578
		public float camFollowDistance
		{
			get
			{
				return this._camFollowDistance;
			}
		}

		/// <summary>
		/// Vertical first-person view translation.
		/// </summary>
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00061380 File Offset: 0x0005F580
		// (set) Token: 0x06001B18 RID: 6936 RVA: 0x00061388 File Offset: 0x0005F588
		public float camDriverOffset { get; protected set; }

		/// <summary>
		/// Vertical first-person view translation.
		/// </summary>
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00061391 File Offset: 0x0005F591
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x00061399 File Offset: 0x0005F599
		public float camPassengerOffset { get; protected set; }

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06001B1B RID: 6939 RVA: 0x000613A2 File Offset: 0x0005F5A2
		public float bumperMultiplier
		{
			get
			{
				return this._bumperMultiplier;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001B1C RID: 6940 RVA: 0x000613AA File Offset: 0x0005F5AA
		public float passengerExplosionArmor
		{
			get
			{
				return this._passengerExplosionArmor;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001B1D RID: 6941 RVA: 0x000613B2 File Offset: 0x0005F5B2
		public TurretInfo[] turrets
		{
			get
			{
				return this._turrets;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001B1E RID: 6942 RVA: 0x000613BA File Offset: 0x0005F5BA
		public Texture albedoBase
		{
			get
			{
				return this._albedoBase;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000613C2 File Offset: 0x0005F5C2
		public Texture metallicBase
		{
			get
			{
				return this._metallicBase;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x000613CA File Offset: 0x0005F5CA
		public Texture emissionBase
		{
			get
			{
				return this._emissionBase;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x000613D2 File Offset: 0x0005F5D2
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x000613DA File Offset: 0x0005F5DA
		public bool CanDecay { get; private set; }

		/// <summary>
		/// Can this vehicle be repaired by a seated player?
		/// </summary>
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000613E3 File Offset: 0x0005F5E3
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x000613EB File Offset: 0x0005F5EB
		public bool canRepairWhileSeated { get; protected set; }

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000613F4 File Offset: 0x0005F5F4
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x000613FC File Offset: 0x0005F5FC
		public float childExplosionArmorMultiplier { get; protected set; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x00061405 File Offset: 0x0005F605
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x0006140D File Offset: 0x0005F60D
		public float airTurnResponsiveness { get; protected set; }

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x00061416 File Offset: 0x0005F616
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x0006141E File Offset: 0x0005F61E
		public float airSteerMin { get; protected set; }

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001B2B RID: 6955 RVA: 0x00061427 File Offset: 0x0005F627
		// (set) Token: 0x06001B2C RID: 6956 RVA: 0x0006142F File Offset: 0x0005F62F
		public float airSteerMax { get; protected set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x00061438 File Offset: 0x0005F638
		// (set) Token: 0x06001B2E RID: 6958 RVA: 0x00061440 File Offset: 0x0005F640
		public float bicycleAnimSpeed { get; protected set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001B2F RID: 6959 RVA: 0x00061449 File Offset: 0x0005F649
		// (set) Token: 0x06001B30 RID: 6960 RVA: 0x00061451 File Offset: 0x0005F651
		public float trainTrackOffset { get; protected set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0006145A File Offset: 0x0005F65A
		// (set) Token: 0x06001B32 RID: 6962 RVA: 0x00061462 File Offset: 0x0005F662
		public float trainWheelOffset { get; protected set; }

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x0006146B File Offset: 0x0005F66B
		// (set) Token: 0x06001B34 RID: 6964 RVA: 0x00061473 File Offset: 0x0005F673
		public float trainCarLength { get; protected set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001B35 RID: 6965 RVA: 0x0006147C File Offset: 0x0005F67C
		// (set) Token: 0x06001B36 RID: 6966 RVA: 0x00061484 File Offset: 0x0005F684
		public float staminaBoost { get; protected set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0006148D File Offset: 0x0005F68D
		// (set) Token: 0x06001B38 RID: 6968 RVA: 0x00061495 File Offset: 0x0005F695
		public bool useStaminaBoost { get; protected set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x0006149E File Offset: 0x0005F69E
		// (set) Token: 0x06001B3A RID: 6970 RVA: 0x000614A6 File Offset: 0x0005F6A6
		public bool isStaminaPowered { get; protected set; }

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x000614AF File Offset: 0x0005F6AF
		// (set) Token: 0x06001B3C RID: 6972 RVA: 0x000614B7 File Offset: 0x0005F6B7
		public bool isBatteryPowered { get; protected set; }

		/// <summary>
		/// Can mobile barricades e.g. bed or sentry guns be placed on this vehicle?
		/// </summary>
		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001B3D RID: 6973 RVA: 0x000614C0 File Offset: 0x0005F6C0
		[Obsolete("Replaced by BuildablePlacementRule")]
		public bool supportsMobileBuildables
		{
			get
			{
				return this.BuildablePlacementRule == EVehicleBuildablePlacementRule.AlwaysAllow;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001B3E RID: 6974 RVA: 0x000614CB File Offset: 0x0005F6CB
		// (set) Token: 0x06001B3F RID: 6975 RVA: 0x000614D3 File Offset: 0x0005F6D3
		public EVehicleBuildablePlacementRule BuildablePlacementRule { get; protected set; }

		/// <summary>
		/// Should capsule colliders be added to seat transforms?
		/// Useful to prevent bikes from leaning into walls.
		/// </summary>
		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001B40 RID: 6976 RVA: 0x000614DC File Offset: 0x0005F6DC
		// (set) Token: 0x06001B41 RID: 6977 RVA: 0x000614E4 File Offset: 0x0005F6E4
		public bool shouldSpawnSeatCapsules { get; protected set; }

		/// <summary>
		/// Can players lock the vehicle to their clan/group?
		/// True by default, but mods want to be able to disable.
		/// </summary>
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x000614ED File Offset: 0x0005F6ED
		// (set) Token: 0x06001B43 RID: 6979 RVA: 0x000614F5 File Offset: 0x0005F6F5
		public bool canBeLocked { get; protected set; }

		/// <summary>
		/// Can players steal the battery?
		/// </summary>
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001B44 RID: 6980 RVA: 0x000614FE File Offset: 0x0005F6FE
		// (set) Token: 0x06001B45 RID: 6981 RVA: 0x00061506 File Offset: 0x0005F706
		public bool canStealBattery { get; protected set; }

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x0006150F File Offset: 0x0005F70F
		// (set) Token: 0x06001B47 RID: 6983 RVA: 0x00061517 File Offset: 0x0005F717
		public byte trunkStorage_X { get; set; }

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x00061520 File Offset: 0x0005F720
		// (set) Token: 0x06001B49 RID: 6985 RVA: 0x00061528 File Offset: 0x0005F728
		public byte trunkStorage_Y { get; set; }

		/// <summary>
		/// Spawn table to drop items from on death.
		/// </summary>
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001B4A RID: 6986 RVA: 0x00061531 File Offset: 0x0005F731
		// (set) Token: 0x06001B4B RID: 6987 RVA: 0x00061539 File Offset: 0x0005F739
		public ushort dropsTableId { get; protected set; }

		/// <summary>
		/// Minimum number of items to drop on death.
		/// </summary>
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x00061542 File Offset: 0x0005F742
		// (set) Token: 0x06001B4D RID: 6989 RVA: 0x0006154A File Offset: 0x0005F74A
		public byte dropsMin { get; protected set; }

		/// <summary>
		/// Maximum number of items to drop on death.
		/// </summary>
		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x00061553 File Offset: 0x0005F753
		// (set) Token: 0x06001B4F RID: 6991 RVA: 0x0006155B File Offset: 0x0005F75B
		public byte dropsMax { get; protected set; }

		/// <summary>
		/// Item ID of compatible tire.
		/// </summary>
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001B50 RID: 6992 RVA: 0x00061564 File Offset: 0x0005F764
		// (set) Token: 0x06001B51 RID: 6993 RVA: 0x0006156C File Offset: 0x0005F76C
		public ushort tireID { get; protected set; }

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001B52 RID: 6994 RVA: 0x00061575 File Offset: 0x0005F775
		internal bool UsesEngineRpmAndGears
		{
			get
			{
				return this.forwardGearRatios != null && this.forwardGearRatios.Length != 0;
			}
		}

		/// <summary>
		/// If this and UsesEngineRpmAndGears are true, HUD will show RPM and gear number.
		/// </summary>
		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x0006158B File Offset: 0x0005F78B
		// (set) Token: 0x06001B54 RID: 6996 RVA: 0x00061593 File Offset: 0x0005F793
		public bool AllowsEngineRpmAndGearsInHud { get; protected set; }

		/// <summary>
		/// When engine RPM dips below this value shift to the next lower gear if available.
		/// </summary>
		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001B55 RID: 6997 RVA: 0x0006159C File Offset: 0x0005F79C
		// (set) Token: 0x06001B56 RID: 6998 RVA: 0x000615A4 File Offset: 0x0005F7A4
		public float GearShiftDownThresholdRpm { get; private set; }

		/// <summary>
		/// When engine RPM exceeds this value shift to the next higher gear if available.
		/// </summary>
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001B57 RID: 6999 RVA: 0x000615AD File Offset: 0x0005F7AD
		// (set) Token: 0x06001B58 RID: 7000 RVA: 0x000615B5 File Offset: 0x0005F7B5
		public float GearShiftUpThresholdRpm { get; private set; }

		/// <summary>
		/// How long after changing gears before throttle is engaged again.
		/// </summary>
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x000615BE File Offset: 0x0005F7BE
		// (set) Token: 0x06001B5A RID: 7002 RVA: 0x000615C6 File Offset: 0x0005F7C6
		public float GearShiftDuration { get; private set; }

		/// <summary>
		/// How long between changing gears to allow another automatic gear change.
		/// </summary>
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x000615CF File Offset: 0x0005F7CF
		// (set) Token: 0x06001B5C RID: 7004 RVA: 0x000615D7 File Offset: 0x0005F7D7
		public float GearShiftInterval { get; private set; }

		/// <summary>
		/// Minimum engine RPM.
		/// </summary>
		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001B5D RID: 7005 RVA: 0x000615E0 File Offset: 0x0005F7E0
		// (set) Token: 0x06001B5E RID: 7006 RVA: 0x000615E8 File Offset: 0x0005F7E8
		public float EngineIdleRpm { get; private set; }

		/// <summary>
		/// Maximum engine RPM.
		/// </summary>
		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000615F1 File Offset: 0x0005F7F1
		// (set) Token: 0x06001B60 RID: 7008 RVA: 0x000615F9 File Offset: 0x0005F7F9
		public float EngineMaxRpm { get; private set; }

		/// <summary>
		/// How quickly RPM can increase in RPM/s.
		/// e.g., 1000 will take 2 seconds to go from 2000 to 4000 RPM.
		/// </summary>
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001B61 RID: 7009 RVA: 0x00061602 File Offset: 0x0005F802
		// (set) Token: 0x06001B62 RID: 7010 RVA: 0x0006160A File Offset: 0x0005F80A
		public float EngineRpmIncreaseRate { get; private set; }

		/// <summary>
		/// How quickly RPM can decrease in RPM/s.
		/// e.g., 1000 will take 2 seconds to go from 4000 to 2000 RPM.
		/// </summary>
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001B63 RID: 7011 RVA: 0x00061613 File Offset: 0x0005F813
		// (set) Token: 0x06001B64 RID: 7012 RVA: 0x0006161B File Offset: 0x0005F81B
		public float EngineRpmDecreaseRate { get; private set; }

		/// <summary>
		/// Maximum torque (multiplied by output of torque curve).
		/// </summary>
		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001B65 RID: 7013 RVA: 0x00061624 File Offset: 0x0005F824
		// (set) Token: 0x06001B66 RID: 7014 RVA: 0x0006162C File Offset: 0x0005F82C
		public float EngineMaxTorque { get; private set; }

		/// <summary>
		/// Was a center of mass specified in the .dat?
		/// </summary>
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001B67 RID: 7015 RVA: 0x00061635 File Offset: 0x0005F835
		// (set) Token: 0x06001B68 RID: 7016 RVA: 0x0006163D File Offset: 0x0005F83D
		public bool hasCenterOfMassOverride { get; protected set; }

		/// <summary>
		/// If hasCenterOfMassOverride, use this value.
		/// </summary>
		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001B69 RID: 7017 RVA: 0x00061646 File Offset: 0x0005F846
		// (set) Token: 0x06001B6A RID: 7018 RVA: 0x0006164E File Offset: 0x0005F84E
		public Vector3 centerOfMass { get; protected set; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x00061657 File Offset: 0x0005F857
		// (set) Token: 0x06001B6C RID: 7020 RVA: 0x0006165F File Offset: 0x0005F85F
		public float carjackForceMultiplier { get; protected set; }

		/// <summary>
		/// Multiplier for otherwise not-yet-configurable plane/heli/boat forces.
		/// Nelson 2024-03-06: Required for increasing mass of vehicles without significantly messing with behavior.
		/// </summary>
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001B6D RID: 7021 RVA: 0x00061668 File Offset: 0x0005F868
		// (set) Token: 0x06001B6E RID: 7022 RVA: 0x00061670 File Offset: 0x0005F870
		public float engineForceMultiplier { get; protected set; }

		/// <summary>
		/// If set, override the wheel collider mass with this value.
		/// </summary>
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001B6F RID: 7023 RVA: 0x00061679 File Offset: 0x0005F879
		// (set) Token: 0x06001B70 RID: 7024 RVA: 0x00061681 File Offset: 0x0005F881
		public float? wheelColliderMassOverride { get; protected set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x0006168A File Offset: 0x0005F88A
		// (set) Token: 0x06001B72 RID: 7026 RVA: 0x00061692 File Offset: 0x0005F892
		public AssetReference<VehiclePhysicsProfileAsset> physicsProfileRef { get; protected set; }

		/// <summary>
		/// Null if vehicle doesn't support paint color.
		/// </summary>
		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001B73 RID: 7027 RVA: 0x0006169B File Offset: 0x0005F89B
		// (set) Token: 0x06001B74 RID: 7028 RVA: 0x000616A3 File Offset: 0x0005F8A3
		public PaintableVehicleSection[] PaintableVehicleSections { get; protected set; }

		/// <summary>
		/// Null if vehicle doesn't support paint color.
		/// </summary>
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001B75 RID: 7029 RVA: 0x000616AC File Offset: 0x0005F8AC
		// (set) Token: 0x06001B76 RID: 7030 RVA: 0x000616B4 File Offset: 0x0005F8B4
		public List<Color32> DefaultPaintColors { get; protected set; }

		/// <summary>
		/// Pick a random paint color according to <see cref="F:SDG.Unturned.VehicleAsset.defaultPaintColorMode" />. Null if unsupported or not configured.
		/// </summary>
		// Token: 0x06001B77 RID: 7031 RVA: 0x000616C0 File Offset: 0x0005F8C0
		public Color32? GetRandomDefaultPaintColor()
		{
			if (this.defaultPaintColorMode == EVehicleDefaultPaintColorMode.List)
			{
				if (this.DefaultPaintColors != null && this.DefaultPaintColors.Count > 0)
				{
					return new Color32?(this.DefaultPaintColors.RandomOrDefault<Color32>());
				}
			}
			else if (this.defaultPaintColorMode == EVehicleDefaultPaintColorMode.RandomHueOrGrayscale && this.randomPaintColorConfiguration != null)
			{
				if (Random.value < this.randomPaintColorConfiguration.grayscaleChance)
				{
					float num = Random.Range(this.randomPaintColorConfiguration.minValue, this.randomPaintColorConfiguration.maxValue);
					return new Color32?(new Color(num, num, num, 1f));
				}
				float value = Random.value;
				float s = Random.Range(this.randomPaintColorConfiguration.minSaturation, this.randomPaintColorConfiguration.maxSaturation);
				float v = Random.Range(this.randomPaintColorConfiguration.minValue, this.randomPaintColorConfiguration.maxValue);
				return new Color32?(Color.HSVToRGB(value, s, v));
			}
			return default(Color32?);
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x000617B6 File Offset: 0x0005F9B6
		public bool SupportsPaintColor
		{
			get
			{
				return this.PaintableVehicleSections != null && this.PaintableVehicleSections.Length != 0;
			}
		}

		/// <summary>
		/// If true, Vehicle Paint items can be used on this vehicle.
		/// Always false if <see cref="P:SDG.Unturned.VehicleAsset.SupportsPaintColor" /> is false.
		///
		/// Certain vehicles may support paint colors without also being paintable by players. For example, the creator
		/// of a vehicle may want to use color variants without also allowing players to make it bright pink.
		/// </summary>
		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001B79 RID: 7033 RVA: 0x000617CC File Offset: 0x0005F9CC
		// (set) Token: 0x06001B7A RID: 7034 RVA: 0x000617D4 File Offset: 0x0005F9D4
		public bool IsPaintable { get; protected set; }

		/// <summary>
		/// Returns reverseGearRatio for negative gears, actual value for valid gear number, otherwise zero.
		/// Exposed for plugin use.
		/// </summary>
		// Token: 0x06001B7B RID: 7035 RVA: 0x000617E0 File Offset: 0x0005F9E0
		public float GetEngineGearRatio(int gearNumber)
		{
			if (gearNumber < 0)
			{
				return this.reverseGearRatio;
			}
			int num = gearNumber - 1;
			if (this.forwardGearRatios != null && num >= 0 && num < this.forwardGearRatios.Length)
			{
				return this.forwardGearRatios[num];
			}
			return 0f;
		}

		/// <summary>
		/// Get number of reverse gear ratios.
		/// Exposed for plugin use.
		/// </summary>
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x00061821 File Offset: 0x0005FA21
		public int ReverseGearsCount
		{
			get
			{
				return 1;
			}
		}

		/// <summary>
		/// Get number of forward gear ratios.
		/// Exposed for plugin use.
		/// </summary>
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001B7D RID: 7037 RVA: 0x00061824 File Offset: 0x0005FA24
		public int ForwardGearsCount
		{
			get
			{
				float[] array = this.forwardGearRatios;
				if (array == null)
				{
					return 0;
				}
				return array.Length;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x00061834 File Offset: 0x0005FA34
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.VEHICLE;
			}
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x00061838 File Offset: 0x0005FA38
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			this._vehicleName = localization.format("Name");
			this._pitchIdle = data.ParseFloat("Pitch_Idle", -1f);
			this._pitchDrive = data.ParseFloat("Pitch_Drive", -1f);
			this._engine = data.ParseEnum<EEngine>("Engine", EEngine.CAR);
			this.physicsProfileRef = data.readAssetReference("Physics_Profile");
			int defaultValue = (this.engine == EEngine.CAR) ? 2 : 1;
			this._hasCrawler = data.ContainsKey("Crawler");
			if (this.hasCrawler)
			{
				defaultValue = 0;
			}
			this.numSteeringTires = data.ParseInt32("Num_Steering_Tires", defaultValue);
			this.steeringTireIndices = new int[this.numSteeringTires];
			for (int i = 0; i < this.numSteeringTires; i++)
			{
				this.steeringTireIndices[i] = data.ParseInt32("Steering_Tire_" + i.ToString(), i);
			}
			if (data.ParseBool("Has_Clip_Prefab", true))
			{
				bundle.loadDeferred<GameObject>("Clip", out this.legacyServerModel, new LoadedAssetDeferredCallback<GameObject>(this.OnServerModelLoaded));
			}
			bundle.loadDeferred<GameObject>("Vehicle", out this.clientModel, new LoadedAssetDeferredCallback<GameObject>(this.OnClientModelLoaded));
			this._size2_z = data.ParseFloat("Size2_Z", 0f);
			this._sharedSkinName = data.GetString("Shared_Skin_Name", null);
			if (data.ContainsKey("Shared_Skin_Lookup_ID"))
			{
				this._sharedSkinLookupID = data.ParseGuidOrLegacyId("Shared_Skin_Lookup_ID", out this._sharedSkinLookupGuid);
			}
			else
			{
				this._sharedSkinLookupGuid = this.GUID;
				this._sharedSkinLookupID = this.id;
			}
			if (data.ContainsKey("Rarity"))
			{
				this._rarity = (EItemRarity)Enum.Parse(typeof(EItemRarity), data.GetString("Rarity", null), true);
			}
			else
			{
				this._rarity = EItemRarity.COMMON;
			}
			this._hasZip = data.ContainsKey("Zip");
			this._hasBicycle = data.ContainsKey("Bicycle");
			this.isReclined = data.ContainsKey("Reclined");
			this._hasLockMouse = data.ContainsKey("LockMouse");
			this._hasTraction = data.ContainsKey("Traction");
			this._hasSleds = data.ContainsKey("Sleds");
			this.canSpawnWithBattery = !data.ContainsKey("Cannot_Spawn_With_Battery");
			if (data.ContainsKey("Battery_Spawn_Charge_Multiplier"))
			{
				this.batterySpawnChargeMultiplier = data.ParseFloat("Battery_Spawn_Charge_Multiplier", 0f);
			}
			else
			{
				this.batterySpawnChargeMultiplier = 1f;
			}
			if (data.ContainsKey("Battery_Burn_Rate"))
			{
				this.batteryBurnRate = data.ParseFloat("Battery_Burn_Rate", 0f);
			}
			else
			{
				this.batteryBurnRate = 20f;
			}
			if (data.ContainsKey("Battery_Charge_Rate"))
			{
				this.batteryChargeRate = data.ParseFloat("Battery_Charge_Rate", 0f);
			}
			else
			{
				this.batteryChargeRate = 20f;
			}
			this.batteryDriving = data.ParseEnum<EBatteryMode>("BatteryMode_Driving", EBatteryMode.Charge);
			this.batteryEmpty = data.ParseEnum<EBatteryMode>("BatteryMode_Empty", EBatteryMode.None);
			this.batteryHeadlights = data.ParseEnum<EBatteryMode>("BatteryMode_Headlights", EBatteryMode.Burn);
			this.batterySirens = data.ParseEnum<EBatteryMode>("BatteryMode_Sirens", EBatteryMode.Burn);
			this.defaultBatteryGuid = data.ParseGuid("Default_Battery", VehicleAsset.VANILLA_BATTERY_ITEM);
			float defaultValue2 = (this.engine == EEngine.CAR) ? 2.05f : 4.2f;
			this.fuelBurnRate = data.ParseFloat("Fuel_Burn_Rate", defaultValue2);
			this._ignition = base.LoadRedirectableAsset<AudioClip>(bundle, "Ignition", data, "IgnitionAudioClip");
			this._horn = base.LoadRedirectableAsset<AudioClip>(bundle, "Horn", data, "HornAudioClip");
			this.hasHorn = data.ParseBool("Has_Horn", this._horn != null);
			this.TargetReverseVelocity = data.ParseFloat("Speed_Min", 0f);
			this.TargetForwardVelocity = data.ParseFloat("Speed_Max", 0f);
			if (this.engine != EEngine.TRAIN)
			{
				this.TargetForwardVelocity *= 1.25f;
			}
			this._steerMin = data.ParseFloat("Steer_Min", 0f);
			this._steerMax = data.ParseFloat("Steer_Max", 0f) * 0.75f;
			this.SteeringAngleTurnSpeed = data.ParseFloat("Steering_Angle_Turn_Speed", this._steerMax * 5f);
			this.steeringLeaningForceMultiplier = data.ParseFloat("Steering_LeaningForceMultiplier", -1f);
			this._brake = data.ParseFloat("Brake", 0f);
			this._lift = data.ParseFloat("Lift", 0f);
			this._fuelMin = data.ParseUInt16("Fuel_Min", 0);
			this._fuelMax = data.ParseUInt16("Fuel_Max", 0);
			this._fuel = data.ParseUInt16("Fuel", 0);
			this._healthMin = data.ParseUInt16("Health_Min", 0);
			this._healthMax = data.ParseUInt16("Health_Max", 0);
			this._health = data.ParseUInt16("Health", 0);
			this._explosion = data.ParseGuidOrLegacyId("Explosion", out this._explosionEffectGuid);
			bool defaultValue3 = !this.IsExplosionEffectRefNull();
			this.ShouldExplosionCauseDamage = data.ParseBool("ShouldExplosionCauseDamage", defaultValue3);
			this.ShouldExplosionBurnMaterials = data.ParseBool("ShouldExplosionBurnMaterials", defaultValue3);
			float num = data.ParseFloat("Explosion_Force_Multiplier", 1f);
			Vector3 a;
			if (data.TryParseVector3("Explosion_Min_Force", out a))
			{
				this.minExplosionForce = a * num;
			}
			else if (data.ContainsKey("Explosion_Min_Force_Y"))
			{
				this.minExplosionForce = data.LegacyParseVector3("Explosion_Min_Force") * num;
			}
			else
			{
				this.minExplosionForce = new Vector3(0f, 1024f * num, 0f);
			}
			Vector3 a2;
			if (data.TryParseVector3("Explosion_Max_Force", out a2))
			{
				this.maxExplosionForce = a2 * num;
			}
			else if (data.ContainsKey("Explosion_Max_Force_Y"))
			{
				this.maxExplosionForce = data.LegacyParseVector3("Explosion_Max_Force") * num;
			}
			else
			{
				this.maxExplosionForce = new Vector3(0f, 1024f * num, 0f);
			}
			if (data.ContainsKey("Exit"))
			{
				this._exit = data.ParseFloat("Exit", 0f);
			}
			else
			{
				this._exit = 2f;
			}
			if (data.ContainsKey("Cam_Follow_Distance"))
			{
				this._camFollowDistance = data.ParseFloat("Cam_Follow_Distance", 0f);
			}
			else
			{
				this._camFollowDistance = 5.5f;
			}
			this.camDriverOffset = data.ParseFloat("Cam_Driver_Offset", 0f);
			this.camPassengerOffset = data.ParseFloat("Cam_Passenger_Offset", 0f);
			if (data.ContainsKey("Bumper_Multiplier"))
			{
				this._bumperMultiplier = data.ParseFloat("Bumper_Multiplier", 0f);
			}
			else
			{
				this._bumperMultiplier = 1f;
			}
			if (data.ContainsKey("Passenger_Explosion_Armor"))
			{
				this._passengerExplosionArmor = data.ParseFloat("Passenger_Explosion_Armor", 0f);
			}
			else
			{
				this._passengerExplosionArmor = 1f;
			}
			if (this.engine == EEngine.HELICOPTER || this.engine == EEngine.BLIMP)
			{
				this._sqrDelta = MathfEx.Square(this.TargetForwardVelocity * 0.125f);
			}
			else
			{
				this._sqrDelta = MathfEx.Square(this.TargetForwardVelocity * 0.1f);
			}
			if (data.ContainsKey("Valid_Speed_Horizontal"))
			{
				float x = data.ParseFloat("Valid_Speed_Horizontal", 0f) * PlayerInput.RATE;
				this._sqrDelta = MathfEx.Square(x);
			}
			EEngine engine = this.engine;
			float defaultValue4;
			float defaultValue5;
			if (engine != EEngine.CAR)
			{
				if (engine != EEngine.BOAT)
				{
					defaultValue4 = 100f;
					defaultValue5 = 100f;
				}
				else
				{
					defaultValue4 = 3.25f;
					defaultValue5 = 25f;
				}
			}
			else
			{
				defaultValue4 = 12.5f;
				defaultValue5 = 25f;
			}
			this.validSpeedUp = data.ParseFloat("Valid_Speed_Up", defaultValue4);
			this.validSpeedDown = data.ParseFloat("Valid_Speed_Down", defaultValue5);
			this._turrets = new TurretInfo[(int)data.ParseUInt8("Turrets", 0)];
			byte b = 0;
			while ((int)b < this.turrets.Length)
			{
				TurretInfo turretInfo = new TurretInfo();
				turretInfo.seatIndex = data.ParseUInt8("Turret_" + b.ToString() + "_Seat_Index", 0);
				turretInfo.itemID = data.ParseUInt16("Turret_" + b.ToString() + "_Item_ID", 0);
				turretInfo.yawMin = data.ParseFloat("Turret_" + b.ToString() + "_Yaw_Min", 0f);
				turretInfo.yawMax = data.ParseFloat("Turret_" + b.ToString() + "_Yaw_Max", 0f);
				turretInfo.pitchMin = data.ParseFloat("Turret_" + b.ToString() + "_Pitch_Min", 0f);
				turretInfo.pitchMax = data.ParseFloat("Turret_" + b.ToString() + "_Pitch_Max", 0f);
				turretInfo.useAimCamera = !data.ContainsKey("Turret_" + b.ToString() + "_Ignore_Aim_Camera");
				this._turrets[(int)b] = turretInfo;
				b += 1;
			}
			this.isVulnerable = !data.ContainsKey("Invulnerable");
			this.isVulnerableToExplosions = !data.ContainsKey("Explosions_Invulnerable");
			this.isVulnerableToEnvironment = !data.ContainsKey("Environment_Invulnerable");
			this.isVulnerableToBumper = !data.ContainsKey("Bumper_Invulnerable");
			this.canTiresBeDamaged = !data.ContainsKey("Tires_Invulnerable");
			this.canRepairWhileSeated = data.ParseBool("Can_Repair_While_Seated", false);
			this.childExplosionArmorMultiplier = data.ParseFloat("Child_Explosion_Armor_Multiplier", 0.2f);
			if (data.ContainsKey("Air_Turn_Responsiveness"))
			{
				this.airTurnResponsiveness = data.ParseFloat("Air_Turn_Responsiveness", 0f);
			}
			else
			{
				this.airTurnResponsiveness = 2f;
			}
			if (data.ContainsKey("Air_Steer_Min"))
			{
				this.airSteerMin = data.ParseFloat("Air_Steer_Min", 0f);
			}
			else
			{
				this.airSteerMin = this.steerMin;
			}
			if (data.ContainsKey("Air_Steer_Max"))
			{
				this.airSteerMax = data.ParseFloat("Air_Steer_Max", 0f);
			}
			else
			{
				this.airSteerMax = this.steerMax;
			}
			this.bicycleAnimSpeed = data.ParseFloat("Bicycle_Anim_Speed", 0f);
			this.staminaBoost = data.ParseFloat("Stamina_Boost", 0f);
			this.useStaminaBoost = data.ContainsKey("Stamina_Boost");
			this.isStaminaPowered = data.ContainsKey("Stamina_Powered");
			this.isBatteryPowered = data.ContainsKey("Battery_Powered");
			EVehicleBuildablePlacementRule buildablePlacementRule;
			if (data.TryParseEnum<EVehicleBuildablePlacementRule>("Buildable_Placement_Rule", out buildablePlacementRule))
			{
				this.BuildablePlacementRule = buildablePlacementRule;
			}
			else if (data.ContainsKey("Supports_Mobile_Buildables"))
			{
				this.BuildablePlacementRule = EVehicleBuildablePlacementRule.AlwaysAllow;
			}
			else
			{
				this.BuildablePlacementRule = EVehicleBuildablePlacementRule.None;
			}
			this.shouldSpawnSeatCapsules = data.ParseBool("Should_Spawn_Seat_Capsules", false);
			this.canBeLocked = data.ParseBool("Can_Be_Locked", true);
			this.canStealBattery = data.ParseBool("Can_Steal_Battery", true);
			this.trunkStorage_X = data.ParseUInt8("Trunk_Storage_X", 0);
			this.trunkStorage_Y = data.ParseUInt8("Trunk_Storage_Y", 0);
			this.dropsTableId = data.ParseUInt16("Drops_Table_ID", 962);
			this.dropsMin = data.ParseUInt8("Drops_Min", 3);
			this.dropsMax = data.ParseUInt8("Drops_Max", 7);
			this.tireID = data.ParseUInt16("Tire_ID", 1451);
			this.hasCenterOfMassOverride = data.ParseBool("Override_Center_Of_Mass", false);
			if (this.hasCenterOfMassOverride)
			{
				this.centerOfMass = data.LegacyParseVector3("Center_Of_Mass");
			}
			this.carjackForceMultiplier = data.ParseFloat("Carjack_Force_Multiplier", 1f);
			this.engineForceMultiplier = data.ParseFloat("Engine_Force_Multiplier", 1f);
			if (data.ContainsKey("Wheel_Collider_Mass_Override"))
			{
				this.wheelColliderMassOverride = new float?(data.ParseFloat("Wheel_Collider_Mass_Override", 1f));
			}
			else
			{
				this.wheelColliderMassOverride = default(float?);
			}
			this.trainTrackOffset = data.ParseFloat("Train_Track_Offset", 0f);
			this.trainWheelOffset = data.ParseFloat("Train_Wheel_Offset", 0f);
			this.trainCarLength = data.ParseFloat("Train_Car_Length", 0f);
			this._shouldVerifyHash = !data.ContainsKey("Bypass_Hash_Verification");
			this.CanDecay = (this.engine != EEngine.TRAIN && (this.isVulnerable | this.isVulnerableToExplosions | this.isVulnerableToEnvironment | this.isVulnerableToBumper));
			this.PaintableVehicleSections = data.ParseArrayOfStructs<PaintableVehicleSection>("PaintableSections", default(PaintableVehicleSection));
			if (this.SupportsPaintColor)
			{
				this.IsPaintable = data.ParseBool("IsPaintable", true);
			}
			else
			{
				this.IsPaintable = false;
			}
			DatList datList;
			if (data.TryGetList("AdditionalTransparentSections", out datList))
			{
				List<string> list = new List<string>(datList.Count);
				foreach (IDatNode datNode in datList)
				{
					DatValue datValue = datNode as DatValue;
					if (datValue != null)
					{
						list.Add(datValue.value);
					}
					else
					{
						Assets.reportError(this, "unable to parse additional transparent section " + datNode.DebugDumpToString());
					}
				}
				if (!list.IsEmpty<string>())
				{
					this.extraTransparentSections = list.ToArray();
				}
			}
			DatList datList2;
			bool flag = data.TryGetList("DefaultPaintColors", out datList2);
			this.defaultPaintColorMode = data.ParseEnum<EVehicleDefaultPaintColorMode>("DefaultPaintColor_Mode", flag ? EVehicleDefaultPaintColorMode.List : EVehicleDefaultPaintColorMode.None);
			if (this.defaultPaintColorMode == EVehicleDefaultPaintColorMode.List)
			{
				this.DefaultPaintColors = new List<Color32>(datList2.Count);
				using (List<IDatNode>.Enumerator enumerator = datList2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDatNode datNode2 = enumerator.Current;
						DatValue datValue2 = datNode2 as DatValue;
						Color32 color;
						if (datValue2 != null && datValue2.TryParseColor32RGB(out color))
						{
							this.DefaultPaintColors.Add(color);
						}
					}
					goto IL_E4B;
				}
			}
			if (this.defaultPaintColorMode == EVehicleDefaultPaintColorMode.RandomHueOrGrayscale)
			{
				this.randomPaintColorConfiguration = new VehicleRandomPaintColorConfiguration();
				DatDictionary node;
				if (data.TryGetDictionary("DefaultPaintColor_Configuration", out node))
				{
					if (!this.randomPaintColorConfiguration.TryParse(node))
					{
						Assets.reportError(this, "unable to parse DefaultPaintColor_Configuration");
					}
				}
				else
				{
					Assets.reportError(this, "missing DefaultPaintColor_Configuration");
				}
			}
			IL_E4B:
			this.wheelBalancingForceMultiplier = data.ParseFloat("WheelBalancing_ForceMultiplier", -1f);
			this.wheelBalancingUprightExponent = data.ParseFloat("WheelBalancing_UprightExponent", 1.5f);
			this.rollAngularVelocityDamping = data.ParseFloat("RollAngularVelocityDamping", -1f);
			DatList datList3;
			if (data.TryGetList("WheelConfigurations", out datList3))
			{
				List<VehicleWheelConfiguration> list2 = new List<VehicleWheelConfiguration>();
				List<int> list3 = new List<int>();
				List<int> list4 = new List<int>();
				foreach (IDatNode datNode3 in datList3)
				{
					VehicleWheelConfiguration vehicleWheelConfiguration = new VehicleWheelConfiguration();
					if (vehicleWheelConfiguration.TryParse(datNode3))
					{
						if (vehicleWheelConfiguration.modelUseColliderPose)
						{
							int count = list2.Count;
							list3.Add(count);
						}
						if (vehicleWheelConfiguration.isColliderPowered)
						{
							int count2 = list2.Count;
							list4.Add(count2);
						}
						list2.Add(vehicleWheelConfiguration);
					}
					else
					{
						Assets.reportError("Unable to parse entry in WheelConfigurations list: " + datNode3.DebugDumpToString());
					}
				}
				this.wheelConfiguration = list2.ToArray();
				if (list3.Count > 0)
				{
					this.replicatedWheelIndices = list3.ToArray();
				}
				if (list4.Count > 0)
				{
					this.poweredWheelIndices = list4.ToArray();
				}
			}
			this.reverseGearRatio = data.ParseFloat("ReverseGearRatio", 1f);
			DatList datList4;
			if (data.TryGetList("ForwardGearRatios", out datList4))
			{
				List<float> list5 = new List<float>();
				foreach (IDatNode datNode4 in datList4)
				{
					DatValue datValue3 = datNode4 as DatValue;
					float num2;
					if (datValue3 != null && datValue3.TryParseFloat(out num2))
					{
						list5.Add(num2);
					}
				}
				if (list5.Count > 0)
				{
					this.forwardGearRatios = list5.ToArray();
					this.AllowsEngineRpmAndGearsInHud = data.ParseBool("GearShift_VisibleInHUD", true);
				}
			}
			this.GearShiftDownThresholdRpm = data.ParseFloat("GearShift_DownThresholdRPM", 1500f);
			this.GearShiftUpThresholdRpm = data.ParseFloat("GearShift_UpThresholdRPM", 5500f);
			this.GearShiftDuration = data.ParseFloat("GearShift_Duration", 0.5f);
			this.GearShiftInterval = data.ParseFloat("GearShift_Interval", 1f);
			this.EngineIdleRpm = data.ParseFloat("EngineIdleRPM", 1000f);
			this.EngineMaxRpm = data.ParseFloat("EngineMaxRPM", 7000f);
			this.EngineRpmIncreaseRate = data.ParseFloat("EngineRPM_IncreaseRate", 10000f);
			this.EngineRpmDecreaseRate = data.ParseFloat("EngineRPM_DecreaseRate", 10000f);
			this.EngineMaxTorque = data.ParseFloat("EngineMaxTorque", 1f);
			this.engineSoundType = data.ParseEnum<EVehicleEngineSoundType>("EngineSound_Type", EVehicleEngineSoundType.Legacy);
			if (this.engineSoundType == EVehicleEngineSoundType.EngineRPMSimple)
			{
				this.engineSoundConfiguration = new RpmEngineSoundConfiguration();
				DatDictionary node2;
				if (data.TryGetDictionary("EngineSound", out node2))
				{
					this.engineSoundConfiguration.TryParse(node2);
				}
			}
			if (this.UsesEngineRpmAndGears && Assets.shouldValidateAssets)
			{
				GameObject orLoadModel = this.GetOrLoadModel();
				if (orLoadModel != null && orLoadModel.GetComponent<EngineCurvesComponent>() == null)
				{
					Assets.reportError(this, "needs EngineCurvesComponent on vehicle prefab for engine RPM and gearbox to work properly");
				}
			}
		}

		/// <summary>
		/// Number of tire visuals to rotate with steering wheel.
		/// </summary>
		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x000629E8 File Offset: 0x00060BE8
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x000629F0 File Offset: 0x00060BF0
		[Obsolete("Replaced by VehicleWheelConfiguration. Only used for backwards compatibility.")]
		public int numSteeringTires { get; protected set; }

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x000629F9 File Offset: 0x00060BF9
		// (set) Token: 0x06001B83 RID: 7043 RVA: 0x00062A01 File Offset: 0x00060C01
		[Obsolete("Replaced by VehicleWheelConfiguration. Only used for backwards compatibility.")]
		public int[] steeringTireIndices { get; protected set; }

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00062A0A File Offset: 0x00060C0A
		[Obsolete("Replaced by VehicleWheelConfiguration. Only used for backwards compatibility.")]
		public bool hasCrawler
		{
			get
			{
				return this._hasCrawler;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x00062A12 File Offset: 0x00060C12
		[Obsolete("Renamed to TargetReverseVelocity.")]
		public float speedMin
		{
			get
			{
				return this.TargetReverseVelocity;
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06001B86 RID: 7046 RVA: 0x00062A1A File Offset: 0x00060C1A
		[Obsolete("Renamed to TargetForwardVelocity.")]
		public float speedMax
		{
			get
			{
				return this.TargetForwardVelocity;
			}
		}

		// Token: 0x04000C74 RID: 3188
		protected bool _shouldVerifyHash;

		// Token: 0x04000C75 RID: 3189
		protected string _vehicleName;

		// Token: 0x04000C76 RID: 3190
		protected float _size2_z;

		// Token: 0x04000C77 RID: 3191
		protected string _sharedSkinName;

		// Token: 0x04000C78 RID: 3192
		private Guid _sharedSkinLookupGuid;

		// Token: 0x04000C79 RID: 3193
		protected ushort _sharedSkinLookupID;

		// Token: 0x04000C7A RID: 3194
		protected EEngine _engine;

		// Token: 0x04000C7B RID: 3195
		protected EItemRarity _rarity;

		// Token: 0x04000C7C RID: 3196
		private GameObject loadedModel;

		/// <summary>
		/// Prevents calling getOrLoad redundantly if asset does not exist.
		/// </summary>
		// Token: 0x04000C7D RID: 3197
		private bool hasLoadedModel;

		// Token: 0x04000C7E RID: 3198
		private IDeferredAsset<GameObject> clientModel;

		// Token: 0x04000C7F RID: 3199
		private IDeferredAsset<GameObject> legacyServerModel;

		// Token: 0x04000C80 RID: 3200
		protected AudioClip _ignition;

		// Token: 0x04000C81 RID: 3201
		protected AudioClip _horn;

		// Token: 0x04000C83 RID: 3203
		protected float _pitchIdle;

		// Token: 0x04000C84 RID: 3204
		protected float _pitchDrive;

		// Token: 0x04000C85 RID: 3205
		internal EVehicleEngineSoundType engineSoundType;

		// Token: 0x04000C86 RID: 3206
		internal RpmEngineSoundConfiguration engineSoundConfiguration;

		// Token: 0x04000C89 RID: 3209
		protected float _steerMin;

		// Token: 0x04000C8A RID: 3210
		protected float _steerMax;

		/// <summary>
		/// Torque on Z axis applied according to steering input for bikes and motorcycles.
		/// </summary>
		// Token: 0x04000C8C RID: 3212
		internal float steeringLeaningForceMultiplier;

		// Token: 0x04000C8D RID: 3213
		protected float _brake;

		// Token: 0x04000C8E RID: 3214
		protected float _lift;

		// Token: 0x04000C8F RID: 3215
		protected ushort _fuelMin;

		// Token: 0x04000C90 RID: 3216
		protected ushort _fuelMax;

		// Token: 0x04000C91 RID: 3217
		protected ushort _fuel;

		// Token: 0x04000C92 RID: 3218
		protected ushort _healthMin;

		// Token: 0x04000C93 RID: 3219
		protected ushort _healthMax;

		// Token: 0x04000C94 RID: 3220
		protected ushort _health;

		// Token: 0x04000C95 RID: 3221
		private Guid _explosionEffectGuid;

		// Token: 0x04000C96 RID: 3222
		protected ushort _explosion;

		// Token: 0x04000C9B RID: 3227
		protected bool _hasHeadlights;

		// Token: 0x04000C9C RID: 3228
		protected bool _hasSirens;

		// Token: 0x04000C9D RID: 3229
		protected bool _hasHook;

		// Token: 0x04000C9E RID: 3230
		protected bool _hasZip;

		// Token: 0x04000C9F RID: 3231
		protected bool _hasBicycle;

		// Token: 0x04000CAB RID: 3243
		protected bool _hasLockMouse;

		// Token: 0x04000CAC RID: 3244
		protected bool _hasTraction;

		// Token: 0x04000CAD RID: 3245
		protected bool _hasSleds;

		// Token: 0x04000CAE RID: 3246
		protected float _exit;

		// Token: 0x04000CAF RID: 3247
		protected float _sqrDelta;

		// Token: 0x04000CB2 RID: 3250
		protected float _camFollowDistance;

		// Token: 0x04000CB5 RID: 3253
		protected float _bumperMultiplier;

		// Token: 0x04000CB6 RID: 3254
		protected float _passengerExplosionArmor;

		// Token: 0x04000CB7 RID: 3255
		protected TurretInfo[] _turrets;

		// Token: 0x04000CB8 RID: 3256
		protected Texture2D _albedoBase;

		// Token: 0x04000CB9 RID: 3257
		protected Texture2D _metallicBase;

		// Token: 0x04000CBA RID: 3258
		protected Texture2D _emissionBase;

		/// <summary>
		/// To non-explosions.
		/// </summary>
		// Token: 0x04000CBB RID: 3259
		public bool isVulnerable;

		// Token: 0x04000CBC RID: 3260
		public bool isVulnerableToExplosions;

		/// <summary>
		/// Mega zombie rocks, zombies, animals.
		/// </summary>
		// Token: 0x04000CBD RID: 3261
		public bool isVulnerableToEnvironment;

		/// <summary>
		/// Crashing into stuff.
		/// </summary>
		// Token: 0x04000CBE RID: 3262
		public bool isVulnerableToBumper;

		// Token: 0x04000CBF RID: 3263
		public bool canTiresBeDamaged;

		/// <summary>
		/// If greater than zero, torque is applied on the local Z axis multiplied by this factor.
		/// Note that <see cref="F:SDG.Unturned.VehicleAsset.rollAngularVelocityDamping" /> is critical for damping this force.
		/// </summary>
		// Token: 0x04000CD8 RID: 3288
		internal float wheelBalancingForceMultiplier = -1f;

		/// <summary>
		/// Exponent on the [0, 1] factor representing how aligned the vehicle is with the ground up vector.
		/// </summary>
		// Token: 0x04000CD9 RID: 3289
		internal float wheelBalancingUprightExponent;

		/// <summary>
		/// If greater than zero, an acceleration is applied to angular velocity on Z axis toward zero.
		/// </summary>
		// Token: 0x04000CDA RID: 3290
		internal float rollAngularVelocityDamping = -1f;

		// Token: 0x04000CDB RID: 3291
		internal VehicleWheelConfiguration[] wheelConfiguration;

		/// <summary>
		/// Indices of wheels using replicated collider pose (if any).
		/// Null if not configured or no wheels using this feature.
		/// Allows client and server to replicate only the suspension value without other context.
		/// </summary>
		// Token: 0x04000CDC RID: 3292
		internal int[] replicatedWheelIndices;

		/// <summary>
		/// Indices of wheels with motor torque applied (if any).
		/// Used for engine RPM calculation.
		/// </summary>
		// Token: 0x04000CDD RID: 3293
		internal int[] poweredWheelIndices;

		// Token: 0x04000CDE RID: 3294
		internal float reverseGearRatio;

		// Token: 0x04000CDF RID: 3295
		internal float[] forwardGearRatios;

		/// <summary>
		/// List of transforms to register with DynamicWaterTransparentSort.
		/// </summary>
		// Token: 0x04000CF1 RID: 3313
		internal string[] extraTransparentSections;

		// Token: 0x04000CF2 RID: 3314
		internal EVehicleDefaultPaintColorMode defaultPaintColorMode;

		/// <summary>
		/// Null if <see cref="F:SDG.Unturned.VehicleAsset.defaultPaintColorMode" /> isn't <see cref="F:SDG.Unturned.EVehicleDefaultPaintColorMode.RandomHueOrGrayscale" />.
		/// </summary>
		// Token: 0x04000CF4 RID: 3316
		internal VehicleRandomPaintColorConfiguration randomPaintColorConfiguration;

		// Token: 0x04000CF6 RID: 3318
		private static readonly Guid VANILLA_BATTERY_ITEM = new Guid("098b13be34a7411db7736b7f866ada69");

		// Token: 0x04000CF9 RID: 3321
		protected bool _hasCrawler;

		// Token: 0x04000CFA RID: 3322
		private static CommandLineFlag clLogWheelConfiguration = new CommandLineFlag(false, "-LogVehicleWheelConfigurations");
	}
}
