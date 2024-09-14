using System;

namespace SDG.Unturned
{
	// Token: 0x020005EB RID: 1515
	public enum EPlayerMessage
	{
		// Token: 0x04001AA1 RID: 6817
		NONE,
		// Token: 0x04001AA2 RID: 6818
		SPACE,
		// Token: 0x04001AA3 RID: 6819
		ITEM,
		// Token: 0x04001AA4 RID: 6820
		VEHICLE_ENTER,
		// Token: 0x04001AA5 RID: 6821
		VEHICLE_EXIT,
		// Token: 0x04001AA6 RID: 6822
		VEHICLE_SWAP,
		// Token: 0x04001AA7 RID: 6823
		RELOAD,
		// Token: 0x04001AA8 RID: 6824
		SAFETY,
		// Token: 0x04001AA9 RID: 6825
		LIGHT,
		// Token: 0x04001AAA RID: 6826
		LASER,
		// Token: 0x04001AAB RID: 6827
		RANGEFINDER,
		// Token: 0x04001AAC RID: 6828
		ENEMY,
		// Token: 0x04001AAD RID: 6829
		DOOR_OPEN,
		// Token: 0x04001AAE RID: 6830
		DOOR_CLOSE,
		// Token: 0x04001AAF RID: 6831
		LOCKED,
		// Token: 0x04001AB0 RID: 6832
		BLOCKED,
		// Token: 0x04001AB1 RID: 6833
		PILLAR,
		// Token: 0x04001AB2 RID: 6834
		POST,
		// Token: 0x04001AB3 RID: 6835
		ROOF,
		// Token: 0x04001AB4 RID: 6836
		WALL,
		// Token: 0x04001AB5 RID: 6837
		CORNER,
		// Token: 0x04001AB6 RID: 6838
		GROUND,
		// Token: 0x04001AB7 RID: 6839
		DOORWAY,
		// Token: 0x04001AB8 RID: 6840
		GARAGE,
		// Token: 0x04001AB9 RID: 6841
		WINDOW,
		// Token: 0x04001ABA RID: 6842
		BED_ON,
		// Token: 0x04001ABB RID: 6843
		BED_OFF,
		// Token: 0x04001ABC RID: 6844
		BED_CLAIMED,
		// Token: 0x04001ABD RID: 6845
		BOUNDS,
		// Token: 0x04001ABE RID: 6846
		EXPERIENCE,
		// Token: 0x04001ABF RID: 6847
		STORAGE,
		// Token: 0x04001AC0 RID: 6848
		FARM,
		// Token: 0x04001AC1 RID: 6849
		GROW,
		// Token: 0x04001AC2 RID: 6850
		SOIL,
		// Token: 0x04001AC3 RID: 6851
		FIRE_ON,
		// Token: 0x04001AC4 RID: 6852
		FIRE_OFF,
		// Token: 0x04001AC5 RID: 6853
		FORAGE,
		// Token: 0x04001AC6 RID: 6854
		GENERATOR_ON,
		// Token: 0x04001AC7 RID: 6855
		GENERATOR_OFF,
		// Token: 0x04001AC8 RID: 6856
		SPOT_ON,
		// Token: 0x04001AC9 RID: 6857
		SPOT_OFF,
		// Token: 0x04001ACA RID: 6858
		EMPTY,
		// Token: 0x04001ACB RID: 6859
		FULL,
		// Token: 0x04001ACC RID: 6860
		MOON_ON,
		// Token: 0x04001ACD RID: 6861
		MOON_OFF,
		// Token: 0x04001ACE RID: 6862
		SAFEZONE_ON,
		// Token: 0x04001ACF RID: 6863
		SAFEZONE_OFF,
		// Token: 0x04001AD0 RID: 6864
		PURCHASE,
		// Token: 0x04001AD1 RID: 6865
		WAVE_ON,
		// Token: 0x04001AD2 RID: 6866
		WAVE_OFF,
		// Token: 0x04001AD3 RID: 6867
		POWER,
		// Token: 0x04001AD4 RID: 6868
		USE,
		// Token: 0x04001AD5 RID: 6869
		SALVAGE,
		// Token: 0x04001AD6 RID: 6870
		TUTORIAL_MOVE,
		// Token: 0x04001AD7 RID: 6871
		TUTORIAL_LOOK,
		// Token: 0x04001AD8 RID: 6872
		TUTORIAL_JUMP,
		// Token: 0x04001AD9 RID: 6873
		TUTORIAL_PERSPECTIVE,
		// Token: 0x04001ADA RID: 6874
		TUTORIAL_RUN,
		// Token: 0x04001ADB RID: 6875
		TUTORIAL_INVENTORY,
		// Token: 0x04001ADC RID: 6876
		TUTORIAL_SURVIVAL,
		// Token: 0x04001ADD RID: 6877
		TUTORIAL_GUN,
		// Token: 0x04001ADE RID: 6878
		TUTORIAL_LADDER,
		// Token: 0x04001ADF RID: 6879
		TUTORIAL_CRAFT,
		// Token: 0x04001AE0 RID: 6880
		TUTORIAL_SKILLS,
		// Token: 0x04001AE1 RID: 6881
		TUTORIAL_SWIM,
		// Token: 0x04001AE2 RID: 6882
		TUTORIAL_MEDICAL,
		// Token: 0x04001AE3 RID: 6883
		TUTORIAL_VEHICLE,
		// Token: 0x04001AE4 RID: 6884
		TUTORIAL_CROUCH,
		// Token: 0x04001AE5 RID: 6885
		TUTORIAL_PRONE,
		// Token: 0x04001AE6 RID: 6886
		TUTORIAL_EDUCATED,
		// Token: 0x04001AE7 RID: 6887
		TUTORIAL_HARVEST,
		// Token: 0x04001AE8 RID: 6888
		TUTORIAL_FISH,
		// Token: 0x04001AE9 RID: 6889
		TUTORIAL_BUILD,
		// Token: 0x04001AEA RID: 6890
		TUTORIAL_HORN,
		// Token: 0x04001AEB RID: 6891
		TUTORIAL_LIGHTS,
		// Token: 0x04001AEC RID: 6892
		TUTORIAL_SIRENS,
		// Token: 0x04001AED RID: 6893
		TUTORIAL_FARM,
		// Token: 0x04001AEE RID: 6894
		TUTORIAL_POWER,
		// Token: 0x04001AEF RID: 6895
		TUTORIAL_FIRE,
		// Token: 0x04001AF0 RID: 6896
		CLAIM,
		// Token: 0x04001AF1 RID: 6897
		DEADZONE_ON,
		// Token: 0x04001AF2 RID: 6898
		DEADZONE_OFF,
		// Token: 0x04001AF3 RID: 6899
		UNDERWATER,
		// Token: 0x04001AF4 RID: 6900
		NAV,
		// Token: 0x04001AF5 RID: 6901
		SPAWN,
		// Token: 0x04001AF6 RID: 6902
		MOBILE,
		// Token: 0x04001AF7 RID: 6903
		OIL,
		// Token: 0x04001AF8 RID: 6904
		VOLUME_WATER,
		// Token: 0x04001AF9 RID: 6905
		VOLUME_FUEL,
		// Token: 0x04001AFA RID: 6906
		BUSY,
		// Token: 0x04001AFB RID: 6907
		TRAPDOOR,
		// Token: 0x04001AFC RID: 6908
		FUEL,
		// Token: 0x04001AFD RID: 6909
		CLEAN,
		// Token: 0x04001AFE RID: 6910
		SALTY,
		// Token: 0x04001AFF RID: 6911
		DIRTY,
		// Token: 0x04001B00 RID: 6912
		TALK,
		// Token: 0x04001B01 RID: 6913
		REPUTATION,
		// Token: 0x04001B02 RID: 6914
		CONDITION,
		/// <summary>
		/// Poorly named. Specific to InteractableObjectQuest.
		/// </summary>
		// Token: 0x04001B03 RID: 6915
		INTERACT,
		// Token: 0x04001B04 RID: 6916
		SAFEZONE,
		// Token: 0x04001B05 RID: 6917
		BAYONET,
		// Token: 0x04001B06 RID: 6918
		VEHICLE_LOCKED,
		// Token: 0x04001B07 RID: 6919
		VEHICLE_UNLOCKED,
		/// <summary>
		/// Directly uses input string for custom message popups.
		/// </summary>
		// Token: 0x04001B08 RID: 6920
		NPC_CUSTOM,
		/// <summary>
		/// Player cannot build on a vehicle with occupied seats.
		/// </summary>
		// Token: 0x04001B09 RID: 6921
		BUILD_ON_OCCUPIED_VEHICLE,
		/// <summary>
		/// Horde beacon cannot be built here.
		/// </summary>
		// Token: 0x04001B0A RID: 6922
		NOT_ALLOWED_HERE,
		/// <summary>
		/// Item type is not allowed on vehicles.
		/// </summary>
		// Token: 0x04001B0B RID: 6923
		CANNOT_BUILD_ON_VEHICLE,
		/// <summary>
		/// Item must be placed closer to vehicle hull.
		/// </summary>
		// Token: 0x04001B0C RID: 6924
		TOO_FAR_FROM_HULL,
		/// <summary>
		/// Player cannot build while seated in a vehicle because some vehicles are abusable to stick the camera through a wall.
		/// </summary>
		// Token: 0x04001B0D RID: 6925
		CANNOT_BUILD_WHILE_SEATED,
		/// <summary>
		/// Interacting with ladder.
		/// </summary>
		// Token: 0x04001B0E RID: 6926
		CLIMB,
		/// <summary>
		/// Popup when equipping housing planner "press T to show items"
		/// </summary>
		// Token: 0x04001B0F RID: 6927
		HOUSING_PLANNER_TUTORIAL,
		/// <summary>
		/// Popup when structure is blocked by something named we can format into the message.
		/// </summary>
		// Token: 0x04001B10 RID: 6928
		PLACEMENT_OBSTRUCTED_BY,
		/// <summary>
		/// Notice that freeform buildables are blocked by <see cref="F:SDG.Unturned.GameplayConfigData.Allow_Freeform_Buildables">Allow_Freeform_Buildables</see>.
		/// </summary>
		// Token: 0x04001B11 RID: 6929
		FREEFORM_BUILDABLE_NOT_ALLOWED,
		/// <summary>
		/// Popup when structure is blocked by terrain.
		/// </summary>
		// Token: 0x04001B12 RID: 6930
		PLACEMENT_OBSTRUCTED_BY_GROUND,
		/// <summary>
		/// Vehicle doesn't support spray paints.
		/// </summary>
		// Token: 0x04001B13 RID: 6931
		NOT_PAINTABLE
	}
}
