using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x0200012D RID: 301
	public class TeleporterExitVolume : LevelVolume<TeleporterExitVolume, TeleporterExitVolumeManager>
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0001BFAC File Offset: 0x0001A1AC
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new TeleporterExitVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
		public string id
		{
			get
			{
				return this._id;
			}
			set
			{
				if (!string.Equals(this._id, value))
				{
					base.GetVolumeManager().RemoveVolumeFromIdDictionary(this);
					this._id = value;
					base.GetVolumeManager().AddVolumeToIdDictionary(this);
				}
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001BFFF File Offset: 0x0001A1FF
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.id = reader.readValue<string>("Id");
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001C019 File Offset: 0x0001A219
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue("Id", this.id);
		}

		// Token: 0x040002CB RID: 715
		[SerializeField]
		private string _id;

		// Token: 0x02000873 RID: 2163
		private class Menu : SleekWrapper
		{
			// Token: 0x0600483A RID: 18490 RVA: 0x001AF664 File Offset: 0x001AD864
			public Menu(TeleporterExitVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 30f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				sleekField.Text = volume.id;
				sleekField.AddLabel("ID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdChanged);
				base.AddChild(sleekField);
			}

			// Token: 0x0600483B RID: 18491 RVA: 0x001AF6E6 File Offset: 0x001AD8E6
			private void OnIdChanged(ISleekField field, string id)
			{
				this.volume.id = id;
			}

			// Token: 0x04003181 RID: 12673
			private TeleporterExitVolume volume;
		}
	}
}
