using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000126 RID: 294
	public class PlayerClipVolume : LevelVolume<PlayerClipVolume, PlayerClipVolumeManager>
	{
		// Token: 0x06000787 RID: 1927 RVA: 0x0001B9AC File Offset: 0x00019BAC
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new PlayerClipVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0001B9C8 File Offset: 0x00019BC8
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x0001B9D0 File Offset: 0x00019BD0
		public bool blockPlayer
		{
			get
			{
				return this._blockPlayer;
			}
			set
			{
				this._blockPlayer = value;
				if (!Level.isEditor)
				{
					this.volumeCollider.enabled = value;
				}
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001B9EC File Offset: 0x00019BEC
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Block_Player"))
			{
				this.blockPlayer = reader.readValue<bool>("Block_Player");
				return;
			}
			this.blockPlayer = true;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001BA1B File Offset: 0x00019C1B
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Block_Player", this.blockPlayer);
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001BA35 File Offset: 0x00019C35
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
			this.volumeCollider.isTrigger = false;
			base.gameObject.layer = 21;
		}

		// Token: 0x040002C4 RID: 708
		[SerializeField]
		protected bool _blockPlayer = true;

		// Token: 0x0200086E RID: 2158
		private class Menu : SleekWrapper
		{
			// Token: 0x06004829 RID: 18473 RVA: 0x001AF2F8 File Offset: 0x001AD4F8
			public Menu(PlayerClipVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 40f;
				ISleekToggle sleekToggle = Glazier.Get().CreateToggle();
				sleekToggle.SizeOffset_X = 40f;
				sleekToggle.SizeOffset_Y = 40f;
				sleekToggle.Value = volume.blockPlayer;
				sleekToggle.AddLabel("Block Player", 1);
				sleekToggle.OnValueChanged += new Toggled(this.OnBlockPlayerToggled);
				base.AddChild(sleekToggle);
			}

			// Token: 0x0600482A RID: 18474 RVA: 0x001AF37A File Offset: 0x001AD57A
			private void OnBlockPlayerToggled(ISleekToggle toggle, bool state)
			{
				this.volume.blockPlayer = state;
			}

			// Token: 0x04003178 RID: 12664
			private PlayerClipVolume volume;
		}
	}
}
