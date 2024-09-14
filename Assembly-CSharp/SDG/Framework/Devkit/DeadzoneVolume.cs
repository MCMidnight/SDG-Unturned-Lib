using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000109 RID: 265
	public class DeadzoneVolume : LevelVolume<DeadzoneVolume, DeadzoneVolumeManager>, IDeadzoneNode
	{
		// Token: 0x060006C6 RID: 1734 RVA: 0x00019D54 File Offset: 0x00017F54
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new DeadzoneVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00019D70 File Offset: 0x00017F70
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x00019D78 File Offset: 0x00017F78
		public EDeadzoneType DeadzoneType
		{
			get
			{
				return this._deadzoneType;
			}
			set
			{
				this._deadzoneType = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00019D81 File Offset: 0x00017F81
		// (set) Token: 0x060006CA RID: 1738 RVA: 0x00019D89 File Offset: 0x00017F89
		public float UnprotectedDamagePerSecond
		{
			get
			{
				return this._unprotectedDamagePerSecond;
			}
			set
			{
				this._unprotectedDamagePerSecond = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x00019D92 File Offset: 0x00017F92
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x00019D9A File Offset: 0x00017F9A
		public float ProtectedDamagePerSecond
		{
			get
			{
				return this._protectedDamagePerSecond;
			}
			set
			{
				this._protectedDamagePerSecond = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x00019DA3 File Offset: 0x00017FA3
		// (set) Token: 0x060006CE RID: 1742 RVA: 0x00019DAB File Offset: 0x00017FAB
		public float UnprotectedRadiationPerSecond
		{
			get
			{
				return this._unprotectedRadiationPerSecond;
			}
			set
			{
				this._unprotectedRadiationPerSecond = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00019DB4 File Offset: 0x00017FB4
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x00019DBC File Offset: 0x00017FBC
		public float MaskFilterDamagePerSecond
		{
			get
			{
				return this._maskFilterDamagePerSecond;
			}
			set
			{
				this._maskFilterDamagePerSecond = value;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00019DC8 File Offset: 0x00017FC8
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Deadzone_Type"))
			{
				this._deadzoneType = reader.readValue<EDeadzoneType>("Deadzone_Type");
			}
			else
			{
				this._deadzoneType = EDeadzoneType.DefaultRadiation;
			}
			this._unprotectedDamagePerSecond = reader.readValue<float>("UnprotectedDamagePerSecond");
			this._protectedDamagePerSecond = reader.readValue<float>("ProtectedDamagePerSecond");
			if (reader.containsKey("UnprotectedRadiationPerSecond"))
			{
				this._unprotectedRadiationPerSecond = reader.readValue<float>("UnprotectedRadiationPerSecond");
			}
			else
			{
				this._unprotectedRadiationPerSecond = 6.25f;
			}
			if (reader.containsKey("MaskFilterDamagePerSecond"))
			{
				this._maskFilterDamagePerSecond = reader.readValue<float>("MaskFilterDamagePerSecond");
				return;
			}
			this._maskFilterDamagePerSecond = 0.4f;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00019E7C File Offset: 0x0001807C
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<EDeadzoneType>("Deadzone_Type", this._deadzoneType);
			writer.writeValue<float>("UnprotectedDamagePerSecond", this._unprotectedDamagePerSecond);
			writer.writeValue<float>("ProtectedDamagePerSecond", this._protectedDamagePerSecond);
			writer.writeValue<float>("UnprotectedRadiationPerSecond", this._unprotectedRadiationPerSecond);
			writer.writeValue<float>("MaskFilterDamagePerSecond", this._maskFilterDamagePerSecond);
		}

		// Token: 0x0400028F RID: 655
		[SerializeField]
		private EDeadzoneType _deadzoneType;

		// Token: 0x04000290 RID: 656
		[SerializeField]
		private float _unprotectedDamagePerSecond;

		// Token: 0x04000291 RID: 657
		[SerializeField]
		private float _protectedDamagePerSecond;

		// Token: 0x04000292 RID: 658
		[SerializeField]
		private float _unprotectedRadiationPerSecond = 6.25f;

		// Token: 0x04000293 RID: 659
		[SerializeField]
		private float _maskFilterDamagePerSecond = 0.4f;

		// Token: 0x0200086A RID: 2154
		private class Menu : SleekWrapper
		{
			// Token: 0x06004816 RID: 18454 RVA: 0x001AEA40 File Offset: 0x001ACC40
			public Menu(DeadzoneVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				float num = 0f;
				SleekButtonState sleekButtonState = new SleekButtonState(new GUIContent[]
				{
					new GUIContent("Default Radiation"),
					new GUIContent("Full Suit Radiation")
				});
				sleekButtonState.PositionOffset_Y = num;
				sleekButtonState.SizeOffset_X = 200f;
				sleekButtonState.SizeOffset_Y = 30f;
				sleekButtonState.state = (int)volume.DeadzoneType;
				sleekButtonState.AddLabel("Deadzone Type", 1);
				SleekButtonState sleekButtonState2 = sleekButtonState;
				sleekButtonState2.onSwappedState = (SwappedState)Delegate.Combine(sleekButtonState2.onSwappedState, new SwappedState(this.OnSwappedState));
				base.AddChild(sleekButtonState);
				num += sleekButtonState.SizeOffset_Y + 10f;
				ISleekFloat32Field sleekFloat32Field = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field.PositionOffset_Y = num;
				sleekFloat32Field.SizeOffset_X = 200f;
				sleekFloat32Field.SizeOffset_Y = 30f;
				sleekFloat32Field.Value = volume.UnprotectedDamagePerSecond;
				sleekFloat32Field.AddLabel("Damage per Second (Unprotected)", 1);
				sleekFloat32Field.OnValueChanged += new TypedSingle(this.OnUnprotectedDamageChanged);
				base.AddChild(sleekFloat32Field);
				num += sleekFloat32Field.SizeOffset_Y + 10f;
				ISleekFloat32Field sleekFloat32Field2 = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field2.PositionOffset_Y = num;
				sleekFloat32Field2.SizeOffset_X = 200f;
				sleekFloat32Field2.SizeOffset_Y = 30f;
				sleekFloat32Field2.Value = volume.ProtectedDamagePerSecond;
				sleekFloat32Field2.AddLabel("Damage per Second (Protected)", 1);
				sleekFloat32Field2.OnValueChanged += new TypedSingle(this.OnProtectedDamageChanged);
				base.AddChild(sleekFloat32Field2);
				num += sleekFloat32Field2.SizeOffset_Y + 10f;
				ISleekFloat32Field sleekFloat32Field3 = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field3.PositionOffset_Y = num;
				sleekFloat32Field3.SizeOffset_X = 200f;
				sleekFloat32Field3.SizeOffset_Y = 30f;
				sleekFloat32Field3.Value = volume.UnprotectedRadiationPerSecond;
				sleekFloat32Field3.AddLabel("Radiation per Second", 1);
				sleekFloat32Field3.OnValueChanged += new TypedSingle(this.OnUnprotectedRadiationChanged);
				base.AddChild(sleekFloat32Field3);
				num += sleekFloat32Field3.SizeOffset_Y + 10f;
				ISleekFloat32Field sleekFloat32Field4 = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field4.PositionOffset_Y = num;
				sleekFloat32Field4.SizeOffset_X = 200f;
				sleekFloat32Field4.SizeOffset_Y = 30f;
				sleekFloat32Field4.Value = volume.MaskFilterDamagePerSecond;
				sleekFloat32Field4.AddLabel("Mask Filter Degradation per Second", 1);
				sleekFloat32Field4.OnValueChanged += new TypedSingle(this.OnMaskFilterDamageChanged);
				base.AddChild(sleekFloat32Field4);
				num += sleekFloat32Field4.SizeOffset_Y + 10f;
				base.SizeOffset_Y = num - 10f;
			}

			// Token: 0x06004817 RID: 18455 RVA: 0x001AECBD File Offset: 0x001ACEBD
			private void OnSwappedState(SleekButtonState button, int state)
			{
				this.volume.DeadzoneType = (EDeadzoneType)state;
			}

			// Token: 0x06004818 RID: 18456 RVA: 0x001AECCB File Offset: 0x001ACECB
			private void OnUnprotectedDamageChanged(ISleekFloat32Field field, float value)
			{
				this.volume.UnprotectedDamagePerSecond = value;
			}

			// Token: 0x06004819 RID: 18457 RVA: 0x001AECD9 File Offset: 0x001ACED9
			private void OnProtectedDamageChanged(ISleekFloat32Field field, float value)
			{
				this.volume.ProtectedDamagePerSecond = value;
			}

			// Token: 0x0600481A RID: 18458 RVA: 0x001AECE7 File Offset: 0x001ACEE7
			private void OnUnprotectedRadiationChanged(ISleekFloat32Field field, float value)
			{
				this.volume.UnprotectedRadiationPerSecond = value;
			}

			// Token: 0x0600481B RID: 18459 RVA: 0x001AECF5 File Offset: 0x001ACEF5
			private void OnMaskFilterDamageChanged(ISleekFloat32Field field, float value)
			{
				this.volume.MaskFilterDamagePerSecond = value;
			}

			// Token: 0x04003173 RID: 12659
			private DeadzoneVolume volume;
		}
	}
}
