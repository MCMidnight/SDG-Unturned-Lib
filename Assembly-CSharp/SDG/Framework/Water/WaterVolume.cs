using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Water
{
	// Token: 0x02000079 RID: 121
	public class WaterVolume : LevelVolume<WaterVolume, WaterVolumeManager>
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new WaterVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000BBEC File Offset: 0x00009DEC
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x0000BBF4 File Offset: 0x00009DF4
		public override ELevelVolumeShape Shape
		{
			get
			{
				return base.Shape;
			}
			set
			{
				base.Shape = value;
				this.SyncWaterPlaneActive();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000BC03 File Offset: 0x00009E03
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0000BC0B File Offset: 0x00009E0B
		public bool isSurfaceVisible
		{
			get
			{
				return this._isSurfaceVisible;
			}
			set
			{
				this._isSurfaceVisible = value;
				this.SyncWaterPlaneActive();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000BC1A File Offset: 0x00009E1A
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000BC22 File Offset: 0x00009E22
		public bool isReflectionVisible
		{
			get
			{
				return this._isReflectionVisible;
			}
			set
			{
				this._isReflectionVisible = value;
				this.SyncPlanarReflectionEnabled();
			}
		}

		/// <summary>
		/// If true rain will be occluded below the surface on the Y axis.
		/// </summary>
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000BC31 File Offset: 0x00009E31
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000BC39 File Offset: 0x00009E39
		public bool isSeaLevel
		{
			get
			{
				return this._isSeaLevel;
			}
			set
			{
				this._isSeaLevel = value;
				if (this.isSeaLevel)
				{
					WaterVolumeManager.seaLevelVolume = this;
				}
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000BC50 File Offset: 0x00009E50
		public override void UpdateEditorVisibility(ELevelVolumeVisibility visibility)
		{
			base.UpdateEditorVisibility(visibility);
			this._editorIsSufaceVisible = (visibility != ELevelVolumeVisibility.Solid && LevelLighting.EditorWantsWaterSurface);
			this.SyncWaterPlaneActive();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000BC74 File Offset: 0x00009E74
		internal void SyncWaterQuality()
		{
			if (this.sharedMaterial == null)
			{
				return;
			}
			EGraphicQuality waterQuality = GraphicsSettings.waterQuality;
			if (waterQuality == EGraphicQuality.LOW)
			{
				this.sharedMaterial.shader.maximumLOD = 201;
				return;
			}
			if (waterQuality == EGraphicQuality.MEDIUM)
			{
				this.sharedMaterial.shader.maximumLOD = 301;
				return;
			}
			if (waterQuality == EGraphicQuality.HIGH || waterQuality == EGraphicQuality.ULTRA)
			{
				this.sharedMaterial.shader.maximumLOD = 501;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		internal void SyncPlanarReflectionEnabled()
		{
			if (this.planarReflection != null)
			{
				this.planarReflection.enabled = (this.isReflectionVisible && GraphicsSettings.waterQuality == EGraphicQuality.ULTRA);
				if (this.sharedMaterial != null && !this.planarReflection.enabled)
				{
					this.sharedMaterial.DisableKeyword("WATER_REFLECTIVE");
					this.sharedMaterial.EnableKeyword("WATER_SIMPLE");
					this.sharedMaterial.SetTexture(this.planarReflection.reflectionSampler, null);
				}
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000BD73 File Offset: 0x00009F73
		private void SyncWaterPlaneActive()
		{
			if (this.waterPlane != null)
			{
				this.waterPlane.SetActive(this.isSurfaceVisible && this._editorIsSufaceVisible && this.Shape == ELevelVolumeShape.Box);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000BDAC File Offset: 0x00009FAC
		protected void createWaterPlanes()
		{
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BDBC File Offset: 0x00009FBC
		public void beginCollision(Collider collider)
		{
			if (collider == null)
			{
				return;
			}
			IWaterVolumeInteractionHandler component = collider.gameObject.GetComponent<IWaterVolumeInteractionHandler>();
			if (component != null)
			{
				component.waterBeginCollision(this);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BDEC File Offset: 0x00009FEC
		public void endCollision(Collider collider)
		{
			if (collider == null)
			{
				return;
			}
			IWaterVolumeInteractionHandler component = collider.gameObject.GetComponent<IWaterVolumeInteractionHandler>();
			if (component != null)
			{
				component.waterEndCollision(this);
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BE1C File Offset: 0x0000A01C
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.isSurfaceVisible = reader.readValue<bool>("Is_Surface_Visible");
			this.isReflectionVisible = reader.readValue<bool>("Is_Reflection_Visible");
			this.isSeaLevel = reader.readValue<bool>("Is_Sea_Level");
			this.waterType = reader.readValue<ERefillWaterType>("Water_Type");
			this.createWaterPlanes();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BE7C File Offset: 0x0000A07C
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Is_Surface_Visible", this.isSurfaceVisible);
			writer.writeValue<bool>("Is_Reflection_Visible", this.isReflectionVisible);
			writer.writeValue<bool>("Is_Sea_Level", this.isSeaLevel);
			writer.writeValue<ERefillWaterType>("Water_Type", this.waterType);
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000BED4 File Offset: 0x0000A0D4
		public override bool ShouldSave
		{
			get
			{
				return !this.isManagedByLighting;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000BEDF File Offset: 0x0000A0DF
		public override bool CanBeSelected
		{
			get
			{
				return !this.isManagedByLighting;
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BEEA File Offset: 0x0000A0EA
		public void OnTriggerEnter(Collider other)
		{
			this.beginCollision(other);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		public void OnTriggerExit(Collider other)
		{
			this.endCollision(other);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000BEFC File Offset: 0x0000A0FC
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BF0B File Offset: 0x0000A10B
		protected override void Start()
		{
			base.Start();
			this.createWaterPlanes();
		}

		// Token: 0x04000144 RID: 324
		public static readonly int WATER_SURFACE_TILE_SIZE = 1024;

		// Token: 0x04000145 RID: 325
		public GameObject waterPlane;

		/// <summary>
		/// All water tiles and the planar reflection component reference this material.
		/// </summary>
		// Token: 0x04000146 RID: 326
		public Material sharedMaterial;

		// Token: 0x04000147 RID: 327
		public PlanarReflection planarReflection;

		// Token: 0x04000148 RID: 328
		[SerializeField]
		protected bool _isSurfaceVisible = true;

		// Token: 0x04000149 RID: 329
		protected bool _editorIsSufaceVisible = true;

		// Token: 0x0400014A RID: 330
		[SerializeField]
		protected bool _isReflectionVisible;

		// Token: 0x0400014B RID: 331
		[SerializeField]
		protected bool _isSeaLevel;

		/// <summary>
		/// Flag for legacy sea level.
		/// </summary>
		// Token: 0x0400014C RID: 332
		internal bool isManagedByLighting;

		// Token: 0x0400014D RID: 333
		public ERefillWaterType waterType = ERefillWaterType.SALTY;

		// Token: 0x02000852 RID: 2130
		private class Menu : SleekWrapper
		{
			// Token: 0x060047BC RID: 18364 RVA: 0x001ADCAC File Offset: 0x001ABEAC
			public Menu(WaterVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 150f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.isSurfaceVisible;
				sleekToggle.AddLabel("Surface Visible", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnIsSurfaceVisibleToggled);
				base.AddChild(sleekToggle);
				ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
				sleekToggle2.PositionOffset_Y = 40f;
				sleekToggle2.SizeOffset_X = 40f;
				sleekToggle2.SizeOffset_Y = 40f;
				sleekToggle2.Value = volume.isReflectionVisible;
				sleekToggle2.AddLabel("Reflection Visible", 1);
				sleekToggle2.OnValueChanged += new Toggled(this.OnIsReflectionVisibleToggled);
				base.AddChild(sleekToggle2);
				ISleekToggle sleekToggle3 = Glazier.Get().CreateToggle();
				sleekToggle3.PositionOffset_Y = 80f;
				sleekToggle3.SizeOffset_X = 40f;
				sleekToggle3.SizeOffset_Y = 40f;
				sleekToggle3.Value = volume.isSeaLevel;
				sleekToggle3.AddLabel("Sea Level", 1);
				sleekToggle3.OnValueChanged += new Toggled(this.OnIsSeaLevelToggled);
				base.AddChild(sleekToggle3);
				SleekButtonState sleekButtonState = new SleekButtonState(new GUIContent[]
				{
					new GUIContent("Clean"),
					new GUIContent("Salty"),
					new GUIContent("Dirty")
				});
				sleekButtonState.PositionOffset_Y = 120f;
				sleekButtonState.SizeOffset_X = 200f;
				sleekButtonState.SizeOffset_Y = 30f;
				sleekButtonState.AddLabel("Refill Type", 1);
				sleekButtonState.state = volume.waterType - ERefillWaterType.CLEAN;
				SleekButtonState sleekButtonState2 = sleekButtonState;
				sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedWaterType));
				base.AddChild(sleekButtonState);
			}

			// Token: 0x060047BD RID: 18365 RVA: 0x001ADE7F File Offset: 0x001AC07F
			private void OnIsSurfaceVisibleToggled(ISleekToggle toggle, bool state)
			{
				this.volume.isSurfaceVisible = state;
			}

			// Token: 0x060047BE RID: 18366 RVA: 0x001ADE8D File Offset: 0x001AC08D
			private void OnIsReflectionVisibleToggled(ISleekToggle toggle, bool state)
			{
				this.volume.isReflectionVisible = state;
			}

			// Token: 0x060047BF RID: 18367 RVA: 0x001ADE9B File Offset: 0x001AC09B
			private void OnIsSeaLevelToggled(ISleekToggle toggle, bool state)
			{
				this.volume.isSeaLevel = state;
			}

			// Token: 0x060047C0 RID: 18368 RVA: 0x001ADEA9 File Offset: 0x001AC0A9
			private void OnSwappedWaterType(SleekButtonState button, int state)
			{
				this.volume.waterType = state + ERefillWaterType.CLEAN;
			}

			// Token: 0x0400314D RID: 12621
			private WaterVolume volume;
		}
	}
}
