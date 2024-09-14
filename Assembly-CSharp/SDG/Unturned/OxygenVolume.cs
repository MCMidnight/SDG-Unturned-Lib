using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FF RID: 1279
	public class OxygenVolume : LevelVolume<OxygenVolume, OxygenVolumeManager>
	{
		// Token: 0x0600281D RID: 10269 RVA: 0x000A985C File Offset: 0x000A7A5C
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new OxygenVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		/// <summary>
		/// If true oxygen is restored while in this volume, otherwise if false oxygen is depleted.
		/// </summary>
		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x0600281E RID: 10270 RVA: 0x000A9878 File Offset: 0x000A7A78
		// (set) Token: 0x0600281F RID: 10271 RVA: 0x000A9880 File Offset: 0x000A7A80
		public bool isBreathable
		{
			get
			{
				return this._isBreathable;
			}
			set
			{
				if (!base.enabled)
				{
					this._isBreathable = value;
					return;
				}
				base.GetVolumeManager().RemoveVolume(this);
				this._isBreathable = value;
				base.GetVolumeManager().AddVolume(this);
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000A98B1 File Offset: 0x000A7AB1
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.isBreathable = reader.readValue<bool>("Is_Breathable");
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000A98CB File Offset: 0x000A7ACB
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Is_Breathable", this.isBreathable);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000A98E5 File Offset: 0x000A7AE5
		protected override void Awake()
		{
			this.supportsFalloff = true;
			base.Awake();
		}

		// Token: 0x04001542 RID: 5442
		[SerializeField]
		private bool _isBreathable = true;

		// Token: 0x0200095F RID: 2399
		private class Menu : SleekWrapper
		{
			// Token: 0x06004B20 RID: 19232 RVA: 0x001B3690 File Offset: 0x001B1890
			public Menu(OxygenVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 40f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.isBreathable;
				sleekToggle.AddLabel("Is Breathable", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnHasOxygenToggled);
				base.AddChild(sleekToggle);
			}

			// Token: 0x06004B21 RID: 19233 RVA: 0x001B3712 File Offset: 0x001B1912
			private void OnHasOxygenToggled(ISleekToggle toggle, bool state)
			{
				this.volume.isBreathable = state;
			}

			// Token: 0x0400333B RID: 13115
			private OxygenVolume volume;
		}
	}
}
