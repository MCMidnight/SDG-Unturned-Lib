using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012C RID: 300
	public class TeleporterEntranceVolume : LevelVolume<TeleporterEntranceVolume, TeleporterEntranceVolumeManager>
	{
		// Token: 0x060007A8 RID: 1960 RVA: 0x0001BEF8 File Offset: 0x0001A0F8
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new TeleporterEntranceVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001BF14 File Offset: 0x0001A114
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.pairId = reader.readValue<string>("Pair_Id");
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001BF2E File Offset: 0x0001A12E
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue("Pair_Id", this.pairId);
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001BF48 File Offset: 0x0001A148
		public void OnTriggerEnter(Collider other)
		{
			TeleporterExitVolume teleporterExitVolume = VolumeManager<TeleporterExitVolume, TeleporterExitVolumeManager>.Get().FindExitVolume(this.pairId);
			if (teleporterExitVolume != null)
			{
				PlayerMovement component = other.gameObject.GetComponent<PlayerMovement>();
				if (component != null && component.CanEnterTeleporter)
				{
					component.EnterTeleporterVolume(this, teleporterExitVolume);
				}
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001BF94 File Offset: 0x0001A194
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
		}

		// Token: 0x040002CA RID: 714
		[SerializeField]
		public string pairId;

		// Token: 0x02000872 RID: 2162
		private class Menu : SleekWrapper
		{
			// Token: 0x06004838 RID: 18488 RVA: 0x001AF5D4 File Offset: 0x001AD7D4
			public Menu(TeleporterEntranceVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 30f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				sleekField.Text = volume.pairId;
				sleekField.AddLabel("Pair ID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdChanged);
				base.AddChild(sleekField);
			}

			// Token: 0x06004839 RID: 18489 RVA: 0x001AF656 File Offset: 0x001AD856
			private void OnIdChanged(ISleekField field, string id)
			{
				this.volume.pairId = id;
			}

			// Token: 0x04003180 RID: 12672
			private TeleporterEntranceVolume volume;
		}
	}
}
