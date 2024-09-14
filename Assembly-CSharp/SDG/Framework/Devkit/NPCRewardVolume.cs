using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000124 RID: 292
	public class NPCRewardVolume : LevelVolume<NPCRewardVolume, NPCRewardVolumeManager>
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x0001B85C File Offset: 0x00019A5C
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new NPCRewardVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001B878 File Offset: 0x00019A78
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.parsedAssetGuid = reader.readValue<Guid>("AssetGuid");
			this._assetGuid = this.parsedAssetGuid.ToString("N");
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001B8A8 File Offset: 0x00019AA8
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<Guid>("AssetGuid", this.parsedAssetGuid);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001B8C2 File Offset: 0x00019AC2
		protected override void Awake()
		{
			this.forceShouldAddCollider = true;
			base.Awake();
			Guid.TryParse(this._assetGuid, ref this.parsedAssetGuid);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001B8E4 File Offset: 0x00019AE4
		private void OnTriggerEnter(Collider other)
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (other.CompareTag("Player"))
			{
				Player player = DamageTool.getPlayer(other.transform);
				if (player != null && !GuidExtension.IsEmpty(this.parsedAssetGuid))
				{
					NPCRewardsAsset npcrewardsAsset = Assets.find(this.parsedAssetGuid) as NPCRewardsAsset;
					if (npcrewardsAsset != null)
					{
						if (npcrewardsAsset.AreConditionsMet(player))
						{
							npcrewardsAsset.ApplyConditions(player);
							npcrewardsAsset.GrantRewards(player);
							return;
						}
					}
					else
					{
						UnturnedLog.warn(string.Format("NPC reward volume unable to find asset ({0:N})", this.parsedAssetGuid));
					}
				}
			}
		}

		/// <summary>
		/// Nelson 2024-06-10: Changed this from guid to string because Unity serialization doesn't support guids
		/// and neither does the inspector. (e.g., couldn't duplicate reward volume without re-assigning guid)
		/// </summary>
		// Token: 0x040002C2 RID: 706
		[SerializeField]
		internal string _assetGuid;

		// Token: 0x040002C3 RID: 707
		private Guid parsedAssetGuid;

		// Token: 0x0200086D RID: 2157
		private class Menu : SleekWrapper
		{
			// Token: 0x06004826 RID: 18470 RVA: 0x001AF168 File Offset: 0x001AD368
			public Menu(NPCRewardVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 110f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				sleekField.Text = volume.parsedAssetGuid.ToString("N");
				sleekField.AddLabel("Asset GUID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdChanged);
				base.AddChild(sleekField);
				this.assetNameBox = Glazier.Get().CreateBox();
				this.assetNameBox.PositionOffset_Y = 40f;
				this.assetNameBox.SizeOffset_X = 200f;
				this.assetNameBox.SizeOffset_Y = 30f;
				this.assetNameBox.AddLabel("Asset", 1);
				base.AddChild(this.assetNameBox);
				this.SyncAssetName();
			}

			// Token: 0x06004827 RID: 18471 RVA: 0x001AF258 File Offset: 0x001AD458
			private void OnIdChanged(ISleekField field, string idString)
			{
				if (!Guid.TryParse(idString, ref this.volume.parsedAssetGuid))
				{
					this.volume.parsedAssetGuid = Guid.Empty;
				}
				this.volume._assetGuid = this.volume.parsedAssetGuid.ToString("N");
				this.SyncAssetName();
			}

			// Token: 0x06004828 RID: 18472 RVA: 0x001AF2B0 File Offset: 0x001AD4B0
			private void SyncAssetName()
			{
				NPCRewardsAsset npcrewardsAsset = Assets.find(this.volume.parsedAssetGuid) as NPCRewardsAsset;
				if (npcrewardsAsset != null)
				{
					this.assetNameBox.Text = npcrewardsAsset.FriendlyName;
					return;
				}
				this.assetNameBox.Text = "null";
			}

			// Token: 0x04003176 RID: 12662
			private ISleekBox assetNameBox;

			// Token: 0x04003177 RID: 12663
			private NPCRewardVolume volume;
		}
	}
}
