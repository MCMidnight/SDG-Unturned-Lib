using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000107 RID: 263
	public class AmbianceVolume : LevelVolume<AmbianceVolume, AmbianceVolumeManager>, IAmbianceNode
	{
		// Token: 0x060006B1 RID: 1713 RVA: 0x00019A54 File Offset: 0x00017C54
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new AmbianceVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00019A70 File Offset: 0x00017C70
		public Guid EffectGuid
		{
			get
			{
				return this._effectGuid;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00019A78 File Offset: 0x00017C78
		// (set) Token: 0x060006B4 RID: 1716 RVA: 0x00019A80 File Offset: 0x00017C80
		public ushort id
		{
			[Obsolete]
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00019A89 File Offset: 0x00017C89
		// (set) Token: 0x060006B6 RID: 1718 RVA: 0x00019A91 File Offset: 0x00017C91
		public bool noWater
		{
			get
			{
				return this._noWater;
			}
			set
			{
				this._noWater = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00019A9A File Offset: 0x00017C9A
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x00019AA2 File Offset: 0x00017CA2
		public bool noLighting
		{
			get
			{
				return this._noLighting;
			}
			set
			{
				this._noLighting = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00019AAB File Offset: 0x00017CAB
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x00019AB3 File Offset: 0x00017CB3
		public bool overrideFog
		{
			get
			{
				return this._overrideFog;
			}
			set
			{
				this._overrideFog = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00019ABC File Offset: 0x00017CBC
		// (set) Token: 0x060006BC RID: 1724 RVA: 0x00019AC4 File Offset: 0x00017CC4
		public Color fogColor
		{
			get
			{
				return this._fogColor;
			}
			set
			{
				this._fogColor = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00019ACD File Offset: 0x00017CCD
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x00019AD5 File Offset: 0x00017CD5
		public float fogIntensity
		{
			get
			{
				return this._fogIntensity;
			}
			set
			{
				this._fogIntensity = value;
			}
		}

		/// <summary>
		/// Used by lighting to get the currently active effect.
		/// </summary>
		// Token: 0x060006BF RID: 1727 RVA: 0x00019ADE File Offset: 0x00017CDE
		public EffectAsset GetEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._effectGuid, this._id);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00019AF4 File Offset: 0x00017CF4
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			string text = reader.readValue("Ambiance_ID");
			if (ushort.TryParse(text, ref this._id))
			{
				this._effectGuid = Guid.Empty;
			}
			else if (Guid.TryParse(text, ref this._effectGuid))
			{
				this._id = 0;
			}
			this.noWater = reader.readValue<bool>("No_Water");
			this.noLighting = reader.readValue<bool>("No_Lighting");
			if (reader.containsKey("Weather_Mask"))
			{
				this.weatherMask = reader.readValue<uint>("Weather_Mask");
			}
			else
			{
				this.weatherMask = uint.MaxValue;
				if (reader.containsKey("Can_Rain") && !reader.readValue<bool>("Can_Rain"))
				{
					this.weatherMask &= 4294967294U;
				}
				if (reader.containsKey("Can_Snow") && !reader.readValue<bool>("Can_Snow"))
				{
					this.weatherMask &= 4294967293U;
				}
			}
			this.overrideFog = reader.readValue<bool>("Override_Fog");
			this.fogColor = reader.readValue<Color>("Fog_Color");
			if (reader.containsKey("Fog_Intensity"))
			{
				this.fogIntensity = reader.readValue<float>("Fog_Intensity");
			}
			else
			{
				float value = reader.readValue<float>("Fog_Height");
				this.fogIntensity = Mathf.InverseLerp(-1024f, 1024f, value);
			}
			this.overrideAtmosphericFog = reader.readValue<bool>("Override_Atmospheric_Fog");
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00019C54 File Offset: 0x00017E54
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			if (!GuidExtension.IsEmpty(this._effectGuid))
			{
				writer.writeValue<Guid>("Ambiance_ID", this._effectGuid);
			}
			else
			{
				writer.writeValue<ushort>("Ambiance_ID", this._id);
			}
			writer.writeValue<bool>("No_Water", this.noWater);
			writer.writeValue<bool>("No_Lighting", this.noLighting);
			writer.writeValue<uint>("Weather_Mask", this.weatherMask);
			writer.writeValue<bool>("Override_Fog", this.overrideFog);
			writer.writeValue<Color>("Fog_Color", this.fogColor);
			writer.writeValue<float>("Fog_Intensity", this.fogIntensity);
			writer.writeValue<bool>("Override_Atmospheric_Fog", this.overrideAtmosphericFog);
		}

		// Token: 0x04000286 RID: 646
		[SerializeField]
		internal Guid _effectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x04000287 RID: 647
		[SerializeField]
		protected ushort _id;

		// Token: 0x04000288 RID: 648
		[SerializeField]
		protected bool _noWater;

		// Token: 0x04000289 RID: 649
		[SerializeField]
		protected bool _noLighting;

		/// <summary>
		/// If per-weather mask AND is non zero the weather will blend in.
		/// </summary>
		// Token: 0x0400028A RID: 650
		[SerializeField]
		public uint weatherMask = uint.MaxValue;

		// Token: 0x0400028B RID: 651
		[SerializeField]
		protected bool _overrideFog;

		// Token: 0x0400028C RID: 652
		[SerializeField]
		protected Color _fogColor = Color.white;

		// Token: 0x0400028D RID: 653
		[SerializeField]
		protected float _fogIntensity;

		// Token: 0x0400028E RID: 654
		[SerializeField]
		public bool overrideAtmosphericFog;

		// Token: 0x02000869 RID: 2153
		private class Menu : SleekWrapper
		{
			// Token: 0x0600480D RID: 18445 RVA: 0x001AE618 File Offset: 0x001AC818
			public Menu(AmbianceVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				if (GuidExtension.IsEmpty(volume._effectGuid))
				{
					sleekField.Text = volume._id.ToString();
				}
				else
				{
					sleekField.Text = volume._effectGuid.ToString("N");
				}
				sleekField.AddLabel("Effect ID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdChanged);
				base.AddChild(sleekField);
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.PositionOffset_Y = 40f;
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.noWater;
				sleekToggle.AddLabel("No Water", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnNoWaterToggled);
				base.AddChild(sleekToggle);
				ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
				sleekToggle2.PositionOffset_Y = 80f;
				sleekToggle2.SizeOffset_X = 40f;
				sleekToggle2.SizeOffset_Y = 40f;
				sleekToggle2.Value = volume.noLighting;
				sleekToggle2.AddLabel("No Lighting", 1);
				sleekToggle2.OnValueChanged += new Toggled(this.OnNoLightingToggled);
				base.AddChild(sleekToggle2);
				ISleekUInt32Field sleekUInt32Field = Glazier.Get().CreateUInt32Field();
				sleekUInt32Field.PositionOffset_Y = 120f;
				sleekUInt32Field.SizeOffset_X = 200f;
				sleekUInt32Field.SizeOffset_Y = 30f;
				sleekUInt32Field.Value = volume.weatherMask;
				sleekUInt32Field.AddLabel("Weather Mask", 1);
				sleekUInt32Field.OnValueChanged += new TypedUInt32(this.OnWeatherMaskChanged);
				base.AddChild(sleekUInt32Field);
				ISleekToggle sleekToggle3 = Glazier.Get().CreateToggle();
				sleekToggle3.PositionOffset_Y = 150f;
				sleekToggle3.SizeOffset_X = 40f;
				sleekToggle3.SizeOffset_Y = 40f;
				sleekToggle3.Value = volume.overrideFog;
				sleekToggle3.AddLabel("Override Fog", 1);
				sleekToggle3.OnValueChanged += new Toggled(this.OnOverrideFogToggled);
				base.AddChild(sleekToggle3);
				SleekColorPicker sleekColorPicker = new SleekColorPicker();
				sleekColorPicker.PositionOffset_Y = 190f;
				sleekColorPicker.state = volume.fogColor;
				SleekColorPicker sleekColorPicker2 = sleekColorPicker;
				sleekColorPicker2.onColorPicked = (ColorPicked)Delegate.Combine(sleekColorPicker2.onColorPicked, new ColorPicked(this.OnFogColorPicked));
				base.AddChild(sleekColorPicker);
				ISleekFloat32Field sleekFloat32Field = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field.PositionOffset_Y = 200f + sleekColorPicker.SizeOffset_Y;
				sleekFloat32Field.SizeOffset_X = 200f;
				sleekFloat32Field.SizeOffset_Y = 30f;
				sleekFloat32Field.Value = volume.fogIntensity;
				sleekFloat32Field.AddLabel("Fog Intensity", 1);
				sleekFloat32Field.OnValueChanged += new TypedSingle(this.OnFogIntensityChanged);
				base.AddChild(sleekFloat32Field);
				ISleekToggle sleekToggle4 = Glazier.Get().CreateToggle();
				sleekToggle4.PositionOffset_Y = sleekFloat32Field.PositionOffset_Y + 40f;
				sleekToggle4.SizeOffset_X = 40f;
				sleekToggle4.SizeOffset_Y = 40f;
				sleekToggle4.Value = volume.overrideAtmosphericFog;
				sleekToggle4.AddLabel("Override Atmospheric Fog", 1);
				sleekToggle4.OnValueChanged += new Toggled(this.OnOverrideAtmosphericFogToggled);
				base.AddChild(sleekToggle4);
				base.SizeOffset_Y = sleekToggle4.PositionOffset_Y + 40f;
			}

			// Token: 0x0600480E RID: 18446 RVA: 0x001AE970 File Offset: 0x001ACB70
			private void OnIdChanged(ISleekField field, string effectIdString)
			{
				if (ushort.TryParse(effectIdString, ref this.volume._id))
				{
					this.volume._effectGuid = Guid.Empty;
					return;
				}
				if (Guid.TryParse(effectIdString, ref this.volume._effectGuid))
				{
					this.volume._id = 0;
					return;
				}
				this.volume._effectGuid = Guid.Empty;
				this.volume._id = 0;
			}

			// Token: 0x0600480F RID: 18447 RVA: 0x001AE9DD File Offset: 0x001ACBDD
			private void OnNoWaterToggled(ISleekToggle toggle, bool noWater)
			{
				this.volume.noWater = noWater;
			}

			// Token: 0x06004810 RID: 18448 RVA: 0x001AE9EB File Offset: 0x001ACBEB
			private void OnNoLightingToggled(ISleekToggle toggle, bool noLighting)
			{
				this.volume.noLighting = noLighting;
			}

			// Token: 0x06004811 RID: 18449 RVA: 0x001AE9F9 File Offset: 0x001ACBF9
			private void OnWeatherMaskChanged(ISleekUInt32Field field, uint mask)
			{
				this.volume.weatherMask = mask;
			}

			// Token: 0x06004812 RID: 18450 RVA: 0x001AEA07 File Offset: 0x001ACC07
			private void OnOverrideFogToggled(ISleekToggle toggle, bool overrideFog)
			{
				this.volume.overrideFog = overrideFog;
			}

			// Token: 0x06004813 RID: 18451 RVA: 0x001AEA15 File Offset: 0x001ACC15
			private void OnFogColorPicked(SleekColorPicker picker, Color color)
			{
				this.volume.fogColor = color;
			}

			// Token: 0x06004814 RID: 18452 RVA: 0x001AEA23 File Offset: 0x001ACC23
			private void OnFogIntensityChanged(ISleekFloat32Field field, float value)
			{
				this.volume.fogIntensity = value;
			}

			// Token: 0x06004815 RID: 18453 RVA: 0x001AEA31 File Offset: 0x001ACC31
			private void OnOverrideAtmosphericFogToggled(ISleekToggle toggle, bool overrideAtmosphericFog)
			{
				this.volume.overrideAtmosphericFog = overrideAtmosphericFog;
			}

			// Token: 0x04003172 RID: 12658
			private AmbianceVolume volume;
		}
	}
}
