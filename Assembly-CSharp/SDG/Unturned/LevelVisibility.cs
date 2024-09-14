using System;
using SDG.Framework.Devkit;

namespace SDG.Unturned
{
	// Token: 0x020004F6 RID: 1270
	public class LevelVisibility
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000A8079 File Offset: 0x000A6279
		// (set) Token: 0x060027DA RID: 10202 RVA: 0x000A8080 File Offset: 0x000A6280
		public static bool roadsVisible
		{
			get
			{
				return LevelVisibility._roadsVisible;
			}
			set
			{
				LevelVisibility._roadsVisible = value;
				LevelRoads.setEnabled(LevelVisibility.roadsVisible);
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060027DB RID: 10203 RVA: 0x000A8092 File Offset: 0x000A6292
		// (set) Token: 0x060027DC RID: 10204 RVA: 0x000A8099 File Offset: 0x000A6299
		public static bool navigationVisible
		{
			get
			{
				return LevelVisibility._navigationVisible;
			}
			set
			{
				LevelVisibility._navigationVisible = value;
				LevelNavigation.setEnabled(LevelVisibility.navigationVisible);
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060027DD RID: 10205 RVA: 0x000A80AB File Offset: 0x000A62AB
		// (set) Token: 0x060027DE RID: 10206 RVA: 0x000A80B7 File Offset: 0x000A62B7
		public static bool nodesVisible
		{
			get
			{
				return SpawnpointSystemV2.Get().IsVisible;
			}
			set
			{
				SpawnpointSystemV2.Get().IsVisible = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060027DF RID: 10207 RVA: 0x000A80C4 File Offset: 0x000A62C4
		// (set) Token: 0x060027E0 RID: 10208 RVA: 0x000A80CB File Offset: 0x000A62CB
		public static bool itemsVisible
		{
			get
			{
				return LevelVisibility._itemsVisible;
			}
			set
			{
				LevelVisibility._itemsVisible = value;
				LevelItems.setEnabled(LevelVisibility.itemsVisible);
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060027E1 RID: 10209 RVA: 0x000A80DD File Offset: 0x000A62DD
		// (set) Token: 0x060027E2 RID: 10210 RVA: 0x000A80E4 File Offset: 0x000A62E4
		public static bool playersVisible
		{
			get
			{
				return LevelVisibility._playersVisible;
			}
			set
			{
				LevelVisibility._playersVisible = value;
				LevelPlayers.setEnabled(LevelVisibility.playersVisible);
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060027E3 RID: 10211 RVA: 0x000A80F6 File Offset: 0x000A62F6
		// (set) Token: 0x060027E4 RID: 10212 RVA: 0x000A80FD File Offset: 0x000A62FD
		public static bool zombiesVisible
		{
			get
			{
				return LevelVisibility._zombiesVisible;
			}
			set
			{
				LevelVisibility._zombiesVisible = value;
				LevelZombies.setEnabled(LevelVisibility.zombiesVisible);
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060027E5 RID: 10213 RVA: 0x000A810F File Offset: 0x000A630F
		// (set) Token: 0x060027E6 RID: 10214 RVA: 0x000A8116 File Offset: 0x000A6316
		public static bool vehiclesVisible
		{
			get
			{
				return LevelVisibility._vehiclesVisible;
			}
			set
			{
				LevelVisibility._vehiclesVisible = value;
				LevelVehicles.setEnabled(LevelVisibility.vehiclesVisible);
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000A8128 File Offset: 0x000A6328
		// (set) Token: 0x060027E8 RID: 10216 RVA: 0x000A812F File Offset: 0x000A632F
		public static bool borderVisible
		{
			get
			{
				return LevelVisibility._borderVisible;
			}
			set
			{
				LevelVisibility._borderVisible = value;
				Level.setEnabled(LevelVisibility.borderVisible);
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000A8141 File Offset: 0x000A6341
		// (set) Token: 0x060027EA RID: 10218 RVA: 0x000A8148 File Offset: 0x000A6348
		public static bool animalsVisible
		{
			get
			{
				return LevelVisibility._animalsVisible;
			}
			set
			{
				LevelVisibility._animalsVisible = value;
				LevelAnimals.setEnabled(LevelVisibility.animalsVisible);
			}
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000A815C File Offset: 0x000A635C
		public static void load()
		{
			if (Level.isVR)
			{
				LevelVisibility.roadsVisible = false;
				LevelVisibility._navigationVisible = false;
				LevelVisibility._itemsVisible = false;
				LevelVisibility.playersVisible = false;
				LevelVisibility._zombiesVisible = false;
				LevelVisibility._vehiclesVisible = false;
				LevelVisibility.borderVisible = false;
				LevelVisibility._animalsVisible = false;
				return;
			}
			if (Level.isEditor)
			{
				if (ReadWrite.fileExists(Level.info.path + "/Level/Visibility.dat", false, false))
				{
					River river = new River(Level.info.path + "/Level/Visibility.dat", false);
					byte b = river.readByte();
					if (b > 0)
					{
						LevelVisibility.roadsVisible = river.readBoolean();
						LevelVisibility.navigationVisible = river.readBoolean();
						river.readBoolean();
						LevelVisibility.itemsVisible = river.readBoolean();
						LevelVisibility.playersVisible = river.readBoolean();
						LevelVisibility.zombiesVisible = river.readBoolean();
						LevelVisibility.vehiclesVisible = river.readBoolean();
						LevelVisibility.borderVisible = river.readBoolean();
						if (b > 1)
						{
							LevelVisibility.animalsVisible = river.readBoolean();
						}
						else
						{
							LevelVisibility._animalsVisible = true;
						}
						river.closeRiver();
						return;
					}
				}
				else
				{
					LevelVisibility._roadsVisible = true;
					LevelVisibility._navigationVisible = true;
					LevelVisibility._itemsVisible = true;
					LevelVisibility._playersVisible = true;
					LevelVisibility._zombiesVisible = true;
					LevelVisibility._vehiclesVisible = true;
					LevelVisibility._borderVisible = true;
					LevelVisibility._animalsVisible = true;
				}
			}
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A8298 File Offset: 0x000A6498
		public static void save()
		{
			River river = new River(Level.info.path + "/Level/Visibility.dat", false);
			river.writeByte(LevelVisibility.SAVEDATA_VERSION);
			river.writeBoolean(LevelVisibility.roadsVisible);
			river.writeBoolean(LevelVisibility.navigationVisible);
			river.writeBoolean(LevelVisibility.nodesVisible);
			river.writeBoolean(LevelVisibility.itemsVisible);
			river.writeBoolean(LevelVisibility.playersVisible);
			river.writeBoolean(LevelVisibility.zombiesVisible);
			river.writeBoolean(LevelVisibility.vehiclesVisible);
			river.writeBoolean(LevelVisibility.borderVisible);
			river.writeBoolean(LevelVisibility.animalsVisible);
			river.closeRiver();
		}

		// Token: 0x04001512 RID: 5394
		public static readonly byte SAVEDATA_VERSION = 2;

		// Token: 0x04001513 RID: 5395
		public static readonly byte OBJECT_REGIONS = 4;

		// Token: 0x04001514 RID: 5396
		private static bool _roadsVisible;

		// Token: 0x04001515 RID: 5397
		private static bool _navigationVisible;

		// Token: 0x04001516 RID: 5398
		private static bool _itemsVisible;

		// Token: 0x04001517 RID: 5399
		private static bool _playersVisible;

		// Token: 0x04001518 RID: 5400
		private static bool _zombiesVisible;

		// Token: 0x04001519 RID: 5401
		private static bool _vehiclesVisible;

		// Token: 0x0400151A RID: 5402
		private static bool _borderVisible;

		// Token: 0x0400151B RID: 5403
		private static bool _animalsVisible;
	}
}
