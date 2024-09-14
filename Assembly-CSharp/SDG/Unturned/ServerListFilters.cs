using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x020006CD RID: 1741
	public class ServerListFilters
	{
		// Token: 0x06003A01 RID: 14849 RVA: 0x0010F644 File Offset: 0x0010D844
		public void GetLevels(List<LevelInfo> levels)
		{
			foreach (string filter in this.mapNames)
			{
				LevelInfo levelInfo = Level.FindLevelForServerFilterExact(filter);
				if (levelInfo != null)
				{
					levels.Add(levelInfo);
				}
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x0010F6A0 File Offset: 0x0010D8A0
		public string GetMapDisplayText()
		{
			string text = string.Empty;
			foreach (string text2 in this.mapNames)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				LevelInfo levelInfo = Level.FindLevelForServerFilterExact(text2);
				if (levelInfo != null)
				{
					text += levelInfo.getLocalizedName();
				}
				else
				{
					text += text2;
				}
			}
			return text;
		}

		/// <returns>True if level was added to the list of maps.</returns>
		// Token: 0x06003A03 RID: 14851 RVA: 0x0010F72C File Offset: 0x0010D92C
		public bool ToggleMap(LevelInfo levelInfo)
		{
			if (levelInfo == null)
			{
				return false;
			}
			string text = levelInfo.name.ToLower();
			if (!this.mapNames.Remove(text))
			{
				this.mapNames.Add(text);
				return true;
			}
			return false;
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x0010F768 File Offset: 0x0010D968
		public void ClearMaps()
		{
			this.mapNames.Clear();
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x0010F778 File Offset: 0x0010D978
		public void CopyFrom(ServerListFilters source)
		{
			this.presetName = source.presetName;
			this.presetId = source.presetId;
			this.serverName = source.serverName;
			this.mapNames = new HashSet<string>(source.mapNames);
			this.password = source.password;
			this.workshop = source.workshop;
			this.plugins = source.plugins;
			this.attendance = source.attendance;
			this.notFull = source.notFull;
			this.vacProtection = source.vacProtection;
			this.battlEyeProtection = source.battlEyeProtection;
			this.combat = source.combat;
			this.cheats = source.cheats;
			this.camera = source.camera;
			this.monetization = source.monetization;
			this.gold = source.gold;
			this.listSource = source.listSource;
			this.maxPing = source.maxPing;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x0010F864 File Offset: 0x0010DA64
		public void Read(byte version, Block block)
		{
			if (version >= 20)
			{
				int num = block.readInt32();
				for (int i = 0; i < num; i++)
				{
					string text = block.readString();
					if (!string.IsNullOrEmpty(text))
					{
						this.mapNames.Add(text);
					}
				}
			}
			else
			{
				string text2 = block.readString();
				if (!string.IsNullOrEmpty(text2))
				{
					this.mapNames.Add(text2);
				}
			}
			this.password = (EPassword)block.readByte();
			this.workshop = (EWorkshop)block.readByte();
			this.plugins = (EPlugins)block.readByte();
			this.attendance = (EAttendance)block.readByte();
			this.notFull = block.readBoolean();
			this.vacProtection = (EVACProtectionFilter)block.readByte();
			this.battlEyeProtection = (EBattlEyeProtectionFilter)block.readByte();
			this.combat = (ECombat)block.readByte();
			this.cheats = (ECheats)block.readByte();
			this.camera = (ECameraMode)block.readByte();
			this.monetization = (EServerMonetizationTag)block.readByte();
			this.gold = (EServerListGoldFilter)block.readByte();
			this.serverName = block.readString();
			this.listSource = (ESteamServerList)block.readByte();
			this.presetName = block.readString();
			this.presetId = block.readInt32();
			if (version >= 22)
			{
				this.maxPing = block.readInt32();
				if (version < 24 && this.maxPing == 200)
				{
					this.maxPing = 300;
					return;
				}
			}
			else
			{
				this.maxPing = 300;
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x0010F9C0 File Offset: 0x0010DBC0
		public void Write(Block block)
		{
			block.writeInt32(this.mapNames.Count);
			foreach (string value in this.mapNames)
			{
				block.writeString(value);
			}
			block.writeByte((byte)this.password);
			block.writeByte((byte)this.workshop);
			block.writeByte((byte)this.plugins);
			block.writeByte((byte)this.attendance);
			block.writeBoolean(this.notFull);
			block.writeByte((byte)this.vacProtection);
			block.writeByte((byte)this.battlEyeProtection);
			block.writeByte((byte)this.combat);
			block.writeByte((byte)this.cheats);
			block.writeByte((byte)this.camera);
			block.writeByte((byte)this.monetization);
			block.writeByte((byte)this.gold);
			block.writeString(this.serverName);
			block.writeByte((byte)this.listSource);
			block.writeString(this.presetName);
			block.writeInt32(this.presetId);
			block.writeInt32(this.maxPing);
		}

		// Token: 0x040022D5 RID: 8917
		public string presetName = string.Empty;

		/// <summary>
		/// Assigned when a named preset is created.
		/// 0 is the default and should be replaced by a preset when loaded.
		/// -1 indicates the preset was modified.
		/// -2 and below are the default presets.
		/// </summary>
		// Token: 0x040022D6 RID: 8918
		public int presetId;

		// Token: 0x040022D7 RID: 8919
		public string serverName = string.Empty;

		// Token: 0x040022D8 RID: 8920
		public HashSet<string> mapNames = new HashSet<string>();

		// Token: 0x040022D9 RID: 8921
		public EPassword password;

		// Token: 0x040022DA RID: 8922
		public EWorkshop workshop = EWorkshop.ANY;

		// Token: 0x040022DB RID: 8923
		public EPlugins plugins = EPlugins.ANY;

		// Token: 0x040022DC RID: 8924
		public EAttendance attendance = EAttendance.HasPlayers;

		/// <summary>
		/// If true, only servers with available player slots are shown.
		/// </summary>
		// Token: 0x040022DD RID: 8925
		public bool notFull = true;

		// Token: 0x040022DE RID: 8926
		public EVACProtectionFilter vacProtection;

		// Token: 0x040022DF RID: 8927
		public EBattlEyeProtectionFilter battlEyeProtection;

		// Token: 0x040022E0 RID: 8928
		public ECombat combat = ECombat.ANY;

		// Token: 0x040022E1 RID: 8929
		public ECheats cheats;

		// Token: 0x040022E2 RID: 8930
		public ECameraMode camera = ECameraMode.ANY;

		// Token: 0x040022E3 RID: 8931
		public EServerMonetizationTag monetization = EServerMonetizationTag.Any;

		// Token: 0x040022E4 RID: 8932
		public EServerListGoldFilter gold;

		// Token: 0x040022E5 RID: 8933
		public ESteamServerList listSource;

		/// <summary>
		/// If &gt;0, servers with ping higher than this will not be shown.
		/// </summary>
		// Token: 0x040022E6 RID: 8934
		public int maxPing = 300;
	}
}
