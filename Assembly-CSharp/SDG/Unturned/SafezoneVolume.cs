using System;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	// Token: 0x0200050D RID: 1293
	public class SafezoneVolume : LevelVolume<SafezoneVolume, SafezoneVolumeManager>
	{
		// Token: 0x06002893 RID: 10387 RVA: 0x000ACD60 File Offset: 0x000AAF60
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new SafezoneVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000ACD7C File Offset: 0x000AAF7C
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("No_Weapons"))
			{
				this.noWeapons = reader.readValue<bool>("No_Weapons");
			}
			if (reader.containsKey("No_Buildables"))
			{
				this.noBuildables = reader.readValue<bool>("No_Buildables");
			}
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000ACDCC File Offset: 0x000AAFCC
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("No_Weapons", this.noWeapons);
			writer.writeValue<bool>("No_Buildables", this.noBuildables);
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000ACDF7 File Offset: 0x000AAFF7
		protected override void Start()
		{
			base.Start();
			this.backwardsCompatibilityNode = new SafezoneNode(base.transform.position, SafezoneNode.CalculateNormalizedRadiusFromRadius(base.GetSphereRadius()), false, this.noWeapons, this.noBuildables);
		}

		// Token: 0x04001598 RID: 5528
		public bool noWeapons = true;

		// Token: 0x04001599 RID: 5529
		public bool noBuildables = true;

		// Token: 0x0400159A RID: 5530
		internal SafezoneNode backwardsCompatibilityNode;

		// Token: 0x02000960 RID: 2400
		private class Menu : SleekWrapper
		{
			// Token: 0x06004B22 RID: 19234 RVA: 0x001B3720 File Offset: 0x001B1920
			public Menu(SafezoneVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 90f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.noWeapons;
				sleekToggle.AddLabel("No Weapons", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnWeaponsToggled);
				base.AddChild(sleekToggle);
				ISleekToggle sleekToggle2 = Glazier.Get().CreateToggle();
				sleekToggle2.PositionOffset_Y = 50f;
				sleekToggle2.SizeOffset_X = 40f;
				sleekToggle2.SizeOffset_Y = 40f;
				sleekToggle2.Value = volume.noBuildables;
				sleekToggle2.AddLabel("No Buildables", 1);
				sleekToggle2.OnValueChanged += new Toggled(this.OnBuildablesToggled);
				base.AddChild(sleekToggle2);
			}

			// Token: 0x06004B23 RID: 19235 RVA: 0x001B37FF File Offset: 0x001B19FF
			private void OnWeaponsToggled(ISleekToggle toggle, bool state)
			{
				this.volume.noWeapons = state;
			}

			// Token: 0x06004B24 RID: 19236 RVA: 0x001B380D File Offset: 0x001B1A0D
			private void OnBuildablesToggled(ISleekToggle toggle, bool state)
			{
				this.volume.noBuildables = state;
			}

			// Token: 0x0400333C RID: 13116
			private SafezoneVolume volume;
		}
	}
}
