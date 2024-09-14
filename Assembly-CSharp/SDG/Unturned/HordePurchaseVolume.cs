using System;
using SDG.Framework.IO.FormattedFiles;

namespace SDG.Unturned
{
	// Token: 0x020004CA RID: 1226
	public class HordePurchaseVolume : LevelVolume<HordePurchaseVolume, HordePurchaseVolumeManager>
	{
		// Token: 0x06002586 RID: 9606 RVA: 0x000954A0 File Offset: 0x000936A0
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new HordePurchaseVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000954BC File Offset: 0x000936BC
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Item_ID"))
			{
				this.id = reader.readValue<ushort>("Item_ID");
			}
			if (reader.containsKey("Cost"))
			{
				this.cost = reader.readValue<uint>("Cost");
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x0009550C File Offset: 0x0009370C
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<ushort>("Item_ID", this.id);
			writer.writeValue<uint>("Cost", this.cost);
		}

		// Token: 0x04001349 RID: 4937
		public ushort id;

		// Token: 0x0400134A RID: 4938
		public uint cost;

		// Token: 0x0200094D RID: 2381
		private class Menu : SleekWrapper
		{
			// Token: 0x06004AEC RID: 19180 RVA: 0x001B25D0 File Offset: 0x001B07D0
			public Menu(HordePurchaseVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 70f;
				ISleekUInt16Field sleekUInt16Field = Glazier.Get().CreateUInt16Field();
				sleekUInt16Field.SizeOffset_X = 200f;
				sleekUInt16Field.SizeOffset_Y = 30f;
				sleekUInt16Field.Value = volume.id;
				sleekUInt16Field.AddLabel("Item ID", 1);
				sleekUInt16Field.OnValueChanged += new TypedUInt16(this.OnIdChanged);
				base.AddChild(sleekUInt16Field);
				ISleekUInt32Field sleekUInt32Field = Glazier.Get().CreateUInt32Field();
				sleekUInt32Field.PositionOffset_Y = 40f;
				sleekUInt32Field.SizeOffset_X = 200f;
				sleekUInt32Field.SizeOffset_Y = 30f;
				sleekUInt32Field.Value = volume.cost;
				sleekUInt32Field.AddLabel("Cost", 1);
				sleekUInt32Field.OnValueChanged += new TypedUInt32(this.OnCostChanged);
				base.AddChild(sleekUInt32Field);
			}

			// Token: 0x06004AED RID: 19181 RVA: 0x001B26AF File Offset: 0x001B08AF
			private void OnIdChanged(ISleekUInt16Field field, ushort state)
			{
				this.volume.id = state;
			}

			// Token: 0x06004AEE RID: 19182 RVA: 0x001B26BD File Offset: 0x001B08BD
			private void OnCostChanged(ISleekUInt32Field field, uint state)
			{
				this.volume.cost = state;
			}

			// Token: 0x0400330F RID: 13071
			private HordePurchaseVolume volume;
		}
	}
}
