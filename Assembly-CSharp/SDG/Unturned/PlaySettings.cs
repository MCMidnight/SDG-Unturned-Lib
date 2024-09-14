using System;

namespace SDG.Unturned
{
	// Token: 0x020006E7 RID: 1767
	public class PlaySettings
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x0011414C File Offset: 0x0011234C
		public static void load()
		{
			if (ReadWrite.fileExists("/Play.dat", true))
			{
				Block block = ReadWrite.readBlock("/Play.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 1)
					{
						PlaySettings.connectHost = block.readString();
						PlaySettings.connectPort = block.readUInt16();
						PlaySettings.connectPassword = block.readString();
						if (b > 3 && b < 13)
						{
							block.readString();
						}
						PlaySettings.serversPassword = block.readString();
						PlaySettings.singleplayerMode = (EGameMode)block.readByte();
						if (b < 8)
						{
							PlaySettings.singleplayerMode = EGameMode.NORMAL;
						}
						if (b > 10 && b < 12)
						{
							block.readByte();
						}
						if (b < 7)
						{
							PlaySettings.singleplayerCheats = false;
						}
						else
						{
							PlaySettings.singleplayerCheats = block.readBoolean();
						}
						if (b > 4)
						{
							PlaySettings.singleplayerMap = block.readString();
							PlaySettings.editorMap = block.readString();
						}
						else
						{
							PlaySettings.singleplayerMap = "";
							PlaySettings.editorMap = "";
						}
						if (b > 10 && b < 12)
						{
							block.readString();
						}
						if (b > 5)
						{
							PlaySettings.isVR = block.readBoolean();
							if (b < 9)
							{
								PlaySettings.isVR = false;
							}
						}
						else
						{
							PlaySettings.isVR = false;
						}
						if (b < 10)
						{
							PlaySettings.singleplayerCategory = ESingleplayerMapCategory.OFFICIAL;
							return;
						}
						PlaySettings.singleplayerCategory = (ESingleplayerMapCategory)block.readByte();
						return;
					}
				}
			}
			PlaySettings.connectHost = "127.0.0.1";
			PlaySettings.connectPort = 27015;
			PlaySettings.connectPassword = "";
			PlaySettings.serversPassword = string.Empty;
			PlaySettings.singleplayerMode = EGameMode.NORMAL;
			PlaySettings.singleplayerCheats = false;
			PlaySettings.singleplayerMap = "";
			PlaySettings.editorMap = "";
			PlaySettings.singleplayerCategory = ESingleplayerMapCategory.OFFICIAL;
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x001142CC File Offset: 0x001124CC
		public static void save()
		{
			Block block = new Block();
			block.writeByte(13);
			block.writeString(PlaySettings.connectHost);
			block.writeUInt16(PlaySettings.connectPort);
			block.writeString(PlaySettings.connectPassword);
			block.writeString(PlaySettings.serversPassword);
			block.writeByte((byte)PlaySettings.singleplayerMode);
			block.writeBoolean(PlaySettings.singleplayerCheats);
			block.writeString(PlaySettings.singleplayerMap);
			block.writeString(PlaySettings.editorMap);
			block.writeBoolean(PlaySettings.isVR);
			block.writeByte((byte)PlaySettings.singleplayerCategory);
			ReadWrite.writeBlock("/Play.dat", true, block);
		}

		/// <summary>
		/// Version before named version constants were introduced. (2023-11-08)
		/// </summary>
		// Token: 0x040024B9 RID: 9401
		public const byte SAVEDATA_VERSION_INITIAL = 11;

		// Token: 0x040024BA RID: 9402
		public const byte SAVEDATA_VERSION_REMOVED_MATCHMAKING = 12;

		/// <summary>
		/// Moved into ServerListFilters.
		/// </summary>
		// Token: 0x040024BB RID: 9403
		public const byte SAVEDATA_VERSION_REMOVED_SERVER_NAME_FILTER = 13;

		// Token: 0x040024BC RID: 9404
		private const byte SAVEDATA_VERSION_NEWEST = 13;

		// Token: 0x040024BD RID: 9405
		public static readonly byte SAVEDATA_VERSION = 13;

		// Token: 0x040024BE RID: 9406
		public static string connectHost;

		// Token: 0x040024BF RID: 9407
		public static ushort connectPort;

		// Token: 0x040024C0 RID: 9408
		public static string connectPassword;

		// Token: 0x040024C1 RID: 9409
		public static string serversPassword;

		// Token: 0x040024C2 RID: 9410
		public static EGameMode singleplayerMode;

		// Token: 0x040024C3 RID: 9411
		public static bool singleplayerCheats;

		// Token: 0x040024C4 RID: 9412
		public static string singleplayerMap;

		// Token: 0x040024C5 RID: 9413
		public static string editorMap;

		// Token: 0x040024C6 RID: 9414
		public static bool isVR;

		// Token: 0x040024C7 RID: 9415
		public static ESingleplayerMapCategory singleplayerCategory;
	}
}
