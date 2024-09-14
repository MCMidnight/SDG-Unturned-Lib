using System;

namespace SDG.Unturned
{
	// Token: 0x0200048C RID: 1164
	public class Blueprint
	{
		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x00091866 File Offset: 0x0008FA66
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x0009186E File Offset: 0x0008FA6E
		public ItemAsset sourceItem { get; protected set; }

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x00091877 File Offset: 0x0008FA77
		public byte id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06002469 RID: 9321 RVA: 0x0009187F File Offset: 0x0008FA7F
		public EBlueprintType type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x00091887 File Offset: 0x0008FA87
		public BlueprintSupply[] supplies
		{
			get
			{
				return this._supplies;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x0600246B RID: 9323 RVA: 0x0009188F File Offset: 0x0008FA8F
		public BlueprintOutput[] outputs
		{
			get
			{
				return this._outputs;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x00091897 File Offset: 0x0008FA97
		public ushort tool
		{
			get
			{
				return this._tool;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x0600246D RID: 9325 RVA: 0x0009189F File Offset: 0x0008FA9F
		public bool toolCritical
		{
			get
			{
				return this._toolCritical;
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x000918A7 File Offset: 0x0008FAA7
		public Guid BuildEffectGuid
		{
			get
			{
				return this._buildEffectGuid;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x0600246F RID: 9327 RVA: 0x000918AF File Offset: 0x0008FAAF
		public ushort build
		{
			[Obsolete]
			get
			{
				return this._build;
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000918B7 File Offset: 0x0008FAB7
		public EffectAsset FindBuildEffectAsset()
		{
			return Assets.FindEffectAssetByGuidOrLegacyId(this._buildEffectGuid, this._build);
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000918CA File Offset: 0x0008FACA
		public byte level
		{
			get
			{
				return this._level;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06002472 RID: 9330 RVA: 0x000918D2 File Offset: 0x0008FAD2
		public EBlueprintSkill skill
		{
			get
			{
				return this._skill;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06002473 RID: 9331 RVA: 0x000918DA File Offset: 0x0008FADA
		public bool transferState
		{
			get
			{
				return this._transferState;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000918E2 File Offset: 0x0008FAE2
		// (set) Token: 0x06002475 RID: 9333 RVA: 0x000918EA File Offset: 0x0008FAEA
		public string map { get; private set; }

		/// <summary>
		/// Must match conditions to craft.
		/// </summary>
		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06002476 RID: 9334 RVA: 0x000918F3 File Offset: 0x0008FAF3
		// (set) Token: 0x06002477 RID: 9335 RVA: 0x000918FB File Offset: 0x0008FAFB
		public INPCCondition[] questConditions { get; protected set; }

		/// <summary>
		/// Extra rewards given after crafting. Not displayed.
		/// </summary>
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x00091904 File Offset: 0x0008FB04
		public INPCReward[] questRewards
		{
			get
			{
				return this.questRewardsList.rewards;
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x00091914 File Offset: 0x0008FB14
		public bool areConditionsMet(Player player)
		{
			if (this.questConditions != null)
			{
				for (int i = 0; i < this.questConditions.Length; i++)
				{
					if (!this.questConditions[i].isConditionMet(player))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x00091950 File Offset: 0x0008FB50
		public void ApplyConditions(Player player)
		{
			if (this.questConditions != null)
			{
				for (int i = 0; i < this.questConditions.Length; i++)
				{
					this.questConditions[i].ApplyCondition(player);
				}
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x00091986 File Offset: 0x0008FB86
		public void GrantRewards(Player player)
		{
			this.questRewardsList.Grant(player);
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x00091994 File Offset: 0x0008FB94
		internal bool IsOutputFreeformBuildable
		{
			get
			{
				if (this._outputs == null || this._outputs.Length < 1)
				{
					return false;
				}
				foreach (BlueprintOutput blueprintOutput in this._outputs)
				{
					ItemBarricadeAsset itemBarricadeAsset = Assets.find(EAssetType.ITEM, blueprintOutput.id) as ItemBarricadeAsset;
					if (itemBarricadeAsset != null && itemBarricadeAsset.build == EBuild.FREEFORM)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000919F4 File Offset: 0x0008FBF4
		public Blueprint(ItemAsset newSourceItem, byte newID, EBlueprintType newType, BlueprintSupply[] newSupplies, BlueprintOutput[] newOutputs, ushort newTool, bool newToolCritical, ushort newBuild, byte newLevel, EBlueprintSkill newSkill, bool newTransferState, string newMap, INPCCondition[] newQuestConditions, NPCRewardsList newQuestRewardsList) : this(newSourceItem, newID, newType, newSupplies, newOutputs, newTool, newToolCritical, newBuild, default(Guid), newLevel, newSkill, newTransferState, newMap, newQuestConditions, newQuestRewardsList)
		{
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x00091A2C File Offset: 0x0008FC2C
		public Blueprint(ItemAsset newSourceItem, byte newID, EBlueprintType newType, BlueprintSupply[] newSupplies, BlueprintOutput[] newOutputs, ushort newTool, bool newToolCritical, ushort newBuild, Guid newBuildEffectGuid, byte newLevel, EBlueprintSkill newSkill, bool newTransferState, string newMap, INPCCondition[] newQuestConditions, NPCRewardsList newQuestRewardsList)
		{
			this.sourceItem = newSourceItem;
			this._id = newID;
			this._type = newType;
			this._supplies = newSupplies;
			this._outputs = newOutputs;
			this._tool = newTool;
			this._toolCritical = newToolCritical;
			this._buildEffectGuid = newBuildEffectGuid;
			this._build = newBuild;
			this._level = newLevel;
			this._skill = newSkill;
			this._transferState = newTransferState;
			this.map = newMap;
			this.questConditions = newQuestConditions;
			this.questRewardsList = newQuestRewardsList;
			this.hasSupplies = false;
			this.hasTool = false;
			this.tools = 0;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x00091AD0 File Offset: 0x0008FCD0
		public override string ToString()
		{
			string text = string.Empty;
			text += this.type.ToString();
			text += ": ";
			byte b = 0;
			while ((int)b < this.supplies.Length)
			{
				if (b > 0)
				{
					text += " + ";
				}
				text += this.supplies[(int)b].id.ToString();
				text += "x";
				text += this.supplies[(int)b].amount.ToString();
				b += 1;
			}
			text += " = ";
			byte b2 = 0;
			while ((int)b2 < this.outputs.Length)
			{
				if (b2 > 0)
				{
					text += " + ";
				}
				text += this.outputs[(int)b2].id.ToString();
				text += "x";
				text += this.outputs[(int)b2].amount.ToString();
				b2 += 1;
			}
			return text;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x00091BE8 File Offset: 0x0008FDE8
		[Obsolete("Removed shouldSend parameter")]
		public void applyConditions(Player player, bool shouldSend)
		{
			this.ApplyConditions(player);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x00091BF1 File Offset: 0x0008FDF1
		[Obsolete("Removed shouldSend parameter")]
		public void grantRewards(Player player, bool shouldSend)
		{
			this.GrantRewards(player);
		}

		// Token: 0x04001259 RID: 4697
		private byte _id;

		// Token: 0x0400125A RID: 4698
		private EBlueprintType _type;

		// Token: 0x0400125B RID: 4699
		private BlueprintSupply[] _supplies;

		// Token: 0x0400125C RID: 4700
		private BlueprintOutput[] _outputs;

		// Token: 0x0400125D RID: 4701
		private ushort _tool;

		// Token: 0x0400125E RID: 4702
		private bool _toolCritical;

		// Token: 0x0400125F RID: 4703
		private Guid _buildEffectGuid;

		// Token: 0x04001260 RID: 4704
		private ushort _build;

		// Token: 0x04001261 RID: 4705
		private byte _level;

		// Token: 0x04001262 RID: 4706
		private EBlueprintSkill _skill;

		// Token: 0x04001263 RID: 4707
		private bool _transferState;

		// Token: 0x04001265 RID: 4709
		public bool hasSupplies;

		// Token: 0x04001266 RID: 4710
		public bool hasTool;

		// Token: 0x04001267 RID: 4711
		public bool hasItem;

		// Token: 0x04001268 RID: 4712
		public bool hasSkills;

		// Token: 0x04001269 RID: 4713
		public ushort tools;

		// Token: 0x0400126A RID: 4714
		public ushort products;

		// Token: 0x0400126B RID: 4715
		public ushort items;

		// Token: 0x0400126D RID: 4717
		protected NPCRewardsList questRewardsList;

		/// <summary>
		/// 2023-05-27: requested by Renaxon because some Arid blueprints are debug-only and
		/// should not be visible when players search by name. (the 3.23.7.0 update made
		/// non-craftable blueprints searchable for Buak)
		/// </summary>
		// Token: 0x0400126E RID: 4718
		public bool canBeVisibleWhenSearchedWithoutRequiredItems = true;
	}
}
