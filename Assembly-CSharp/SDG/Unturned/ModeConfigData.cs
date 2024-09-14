using System;

namespace SDG.Unturned
{
	// Token: 0x020006DA RID: 1754
	public class ModeConfigData
	{
		// Token: 0x06003AF0 RID: 15088 RVA: 0x0011325C File Offset: 0x0011145C
		public ModeConfigData(EGameMode mode)
		{
			this.Items = new ItemsConfigData(mode);
			this.Vehicles = new VehiclesConfigData(mode);
			this.Zombies = new ZombiesConfigData(mode);
			this.Animals = new AnimalsConfigData(mode);
			this.Barricades = new BarricadesConfigData(mode);
			this.Structures = new StructuresConfigData(mode);
			this.Players = new PlayersConfigData(mode);
			this.Objects = new ObjectConfigData(mode);
			this.Events = new EventsConfigData(mode);
			this.Gameplay = new GameplayConfigData(mode);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001132E7 File Offset: 0x001114E7
		public void InitSingleplayerDefaults()
		{
			this.Players.InitSingleplayerDefaults();
			this.Gameplay.InitSingleplayerDefaults();
		}

		// Token: 0x040023CD RID: 9165
		public ItemsConfigData Items;

		// Token: 0x040023CE RID: 9166
		public VehiclesConfigData Vehicles;

		// Token: 0x040023CF RID: 9167
		public ZombiesConfigData Zombies;

		// Token: 0x040023D0 RID: 9168
		public AnimalsConfigData Animals;

		// Token: 0x040023D1 RID: 9169
		public BarricadesConfigData Barricades;

		// Token: 0x040023D2 RID: 9170
		public StructuresConfigData Structures;

		// Token: 0x040023D3 RID: 9171
		public PlayersConfigData Players;

		// Token: 0x040023D4 RID: 9172
		public ObjectConfigData Objects;

		// Token: 0x040023D5 RID: 9173
		public EventsConfigData Events;

		// Token: 0x040023D6 RID: 9174
		public GameplayConfigData Gameplay;
	}
}
