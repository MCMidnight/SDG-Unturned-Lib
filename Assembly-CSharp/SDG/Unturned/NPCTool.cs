using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000761 RID: 1889
	public class NPCTool
	{
		/// <summary>
		/// Was redirected to HolidayUtil but kept for plugin backwards compatibility.
		/// Refer to HolidayUtil for explanation of this weird situation.
		/// </summary>
		// Token: 0x06003DC8 RID: 15816 RVA: 0x00128E60 File Offset: 0x00127060
		public static ENPCHoliday getActiveHoliday()
		{
			return Provider.authorityHoliday;
		}

		/// <summary>
		/// Was redirected to HolidayUtil but kept for plugin backwards compatibility.
		/// Refer to HolidayUtil for explanation of this weird situation.
		/// </summary>
		// Token: 0x06003DC9 RID: 15817 RVA: 0x00128E67 File Offset: 0x00127067
		public static bool isHolidayActive(ENPCHoliday holiday)
		{
			return holiday == Provider.authorityHoliday;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00128E74 File Offset: 0x00127074
		public static bool doesLogicPass<T>(ENPCLogicType logicType, T a, T b) where T : IComparable
		{
			int num = a.CompareTo(b);
			switch (logicType)
			{
			case ENPCLogicType.LESS_THAN:
				return num < 0;
			case ENPCLogicType.LESS_THAN_OR_EQUAL_TO:
				return num <= 0;
			case ENPCLogicType.EQUAL:
				return num == 0;
			case ENPCLogicType.NOT_EQUAL:
				return num != 0;
			case ENPCLogicType.GREATER_THAN_OR_EQUAL_TO:
				return num >= 0;
			case ENPCLogicType.GREATER_THAN:
				return num > 0;
			default:
				return false;
			}
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00128EDC File Offset: 0x001270DC
		public static void readConditions(DatDictionary data, Local localization, string prefix, INPCCondition[] conditions, Asset assetContext)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				string text = prefix + i.ToString() + "_Type";
				if (!data.ContainsKey(text))
				{
					throw new NotSupportedException("Missing condition " + text);
				}
				ENPCConditionType enpcconditionType = data.ParseEnum<ENPCConditionType>(text, ENPCConditionType.NONE);
				if (enpcconditionType == ENPCConditionType.NONE)
				{
					Assets.reportError(assetContext, "{0} unknown type", text);
				}
				else
				{
					string text2 = localization.read(prefix + i.ToString());
					text2 = ItemTool.filterRarityRichText(text2);
					bool flag = data.ContainsKey(prefix + i.ToString() + "_Reset");
					ENPCLogicType defaultValue;
					if (enpcconditionType != ENPCConditionType.ITEM)
					{
						if (enpcconditionType != ENPCConditionType.HOLIDAY)
						{
							defaultValue = ENPCLogicType.NONE;
						}
						else
						{
							defaultValue = ENPCLogicType.EQUAL;
						}
					}
					else
					{
						defaultValue = ENPCLogicType.GREATER_THAN_OR_EQUAL_TO;
					}
					ENPCLogicType enpclogicType = data.ParseEnum<ENPCLogicType>(prefix + i.ToString() + "_Logic", defaultValue);
					INPCCondition inpccondition = null;
					switch (enpcconditionType)
					{
					case ENPCConditionType.EXPERIENCE:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Experience condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCExperienceCondition(data.ParseUInt32(prefix + i.ToString() + "_Value", 0U), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.REPUTATION:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Reputation condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCReputationCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					case ENPCConditionType.FLAG_BOOL:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Bool flag condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Bool flag condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCBoolFlagCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseBool(prefix + i.ToString() + "_Value", false), data.ContainsKey(prefix + i.ToString() + "_Allow_Unset"), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.FLAG_SHORT:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Short flag condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Short flag condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCShortFlagCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), data.ContainsKey(prefix + i.ToString() + "_Allow_Unset"), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.QUEST:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Quest condition " + prefix + i.ToString() + " missing _ID");
						}
						ENPCQuestStatus enpcquestStatus = ENPCQuestStatus.NONE;
						string text3 = prefix + i.ToString() + "_Status";
						if (!data.ContainsKey(text3))
						{
							Assets.reportError(assetContext, "Quest condition " + prefix + i.ToString() + " missing _Status");
						}
						else
						{
							enpcquestStatus = data.ParseEnum<ENPCQuestStatus>(text3, ENPCQuestStatus.NONE);
							if (enpcquestStatus == ENPCQuestStatus.NONE && flag)
							{
								Assets.reportError(assetContext, "Quest condition " + prefix + i.ToString() + " has Reset enabled with Status None (probably accidental)");
							}
						}
						Guid newQuestGuid;
						ushort newID;
						data.ParseGuidOrLegacyId(prefix + i.ToString() + "_ID", out newQuestGuid, out newID);
						inpccondition = new NPCQuestCondition(newQuestGuid, newID, enpcquestStatus, data.ContainsKey(prefix + i.ToString() + "_Ignore_NPC"), enpclogicType, text2, flag);
						break;
					}
					case ENPCConditionType.SKILLSET:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Skillset condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCSkillsetCondition(data.ParseEnum<EPlayerSkillset>(prefix + i.ToString() + "_Value", EPlayerSkillset.NONE), enpclogicType, text2);
						break;
					case ENPCConditionType.ITEM:
					{
						string text4 = prefix + i.ToString() + "_ID";
						if (!data.ContainsKey(text4))
						{
							Assets.reportError(assetContext, "Item condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Amount"))
						{
							Assets.reportError(assetContext, "Item condition " + prefix + i.ToString() + " missing _Amount");
						}
						if (flag && enpclogicType != ENPCLogicType.GREATER_THAN_OR_EQUAL_TO)
						{
							enpclogicType = ENPCLogicType.GREATER_THAN_OR_EQUAL_TO;
							Assets.reportError(assetContext, "Resetting item condition only compatible with >= comparison. If you have a use in mind feel free to email Nelson.");
						}
						Guid newItemGuid;
						ushort newID2;
						data.ParseGuidOrLegacyId(text4, out newItemGuid, out newID2);
						inpccondition = new NPCItemCondition(newItemGuid, newID2, data.ParseUInt16(prefix + i.ToString() + "_Amount", 0), enpclogicType, text2, flag);
						break;
					}
					case ENPCConditionType.KILLS_ZOMBIE:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Zombie kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Zombie kills condition " + prefix + i.ToString() + " missing _Value");
						}
						EZombieSpeciality newZombie = EZombieSpeciality.NONE;
						if (data.ContainsKey(prefix + i.ToString() + "_Zombie"))
						{
							newZombie = data.ParseEnum<EZombieSpeciality>(prefix + i.ToString() + "_Zombie", EZombieSpeciality.NONE);
						}
						else
						{
							Assets.reportError(assetContext, "Zombie kills condition " + prefix + i.ToString() + " missing _Zombie");
						}
						int newSpawnQuantity = 1;
						if (data.ContainsKey(prefix + i.ToString() + "_Spawn_Quantity"))
						{
							newSpawnQuantity = data.ParseInt32(prefix + i.ToString() + "_Spawn_Quantity", 0);
						}
						byte newNav = data.ParseUInt8(prefix + i.ToString() + "_Nav", byte.MaxValue);
						float newRadius = data.ParseFloat(prefix + i.ToString() + "_Radius", 512f);
						float newMinRadius = data.ParseFloat(prefix + i.ToString() + "_MinRadius", 0f);
						int newLevelTableUniqueId = data.ParseInt32(prefix + i.ToString() + "_LevelTableOverride", -1);
						inpccondition = new NPCZombieKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), newZombie, data.ContainsKey(prefix + i.ToString() + "_Spawn"), newSpawnQuantity, newNav, newRadius, newMinRadius, newLevelTableUniqueId, text2, flag);
						break;
					}
					case ENPCConditionType.KILLS_HORDE:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Horde kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Horde kills condition " + prefix + i.ToString() + " missing _Value");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Nav"))
						{
							Assets.reportError(assetContext, "Horde kills condition " + prefix + i.ToString() + " missing _Nav");
						}
						inpccondition = new NPCHordeKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), data.ParseUInt8(prefix + i.ToString() + "_Nav", 0), text2, flag);
						break;
					case ENPCConditionType.KILLS_ANIMAL:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Animal kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Animal kills condition " + prefix + i.ToString() + " missing _Value");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Animal"))
						{
							Assets.reportError(assetContext, "Animal kills condition " + prefix + i.ToString() + " missing _Animal");
						}
						inpccondition = new NPCAnimalKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), data.ParseUInt16(prefix + i.ToString() + "_Animal", 0), text2, flag);
						break;
					case ENPCConditionType.COMPARE_FLAGS:
						if (!data.ContainsKey(prefix + i.ToString() + "_A_ID"))
						{
							Assets.reportError(assetContext, "Compare flags condition " + prefix + i.ToString() + " missing _A_ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_B_ID"))
						{
							Assets.reportError(assetContext, "Compare flags condition " + prefix + i.ToString() + " missing _B_ID");
						}
						inpccondition = new NPCCompareFlagsCondition(data.ParseUInt16(prefix + i.ToString() + "_A_ID", 0), data.ParseUInt16(prefix + i.ToString() + "_B_ID", 0), data.ContainsKey(prefix + i.ToString() + "_Allow_A_Unset"), data.ContainsKey(prefix + i.ToString() + "_Allow_B_Unset"), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.TIME_OF_DAY:
						if (!data.ContainsKey(prefix + i.ToString() + "_Second"))
						{
							Assets.reportError(assetContext, "Time of day condition " + prefix + i.ToString() + " missing _Second");
						}
						inpccondition = new NPCTimeOfDayCondition(data.ParseInt32(prefix + i.ToString() + "_Second", 0), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.PLAYER_LIFE_HEALTH:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player life health condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerLifeHealthCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					case ENPCConditionType.PLAYER_LIFE_FOOD:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player life food condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerLifeFoodCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					case ENPCConditionType.PLAYER_LIFE_WATER:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player life water condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerLifeWaterCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					case ENPCConditionType.PLAYER_LIFE_VIRUS:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player life virus condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerLifeVirusCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					case ENPCConditionType.HOLIDAY:
					{
						ENPCHoliday enpcholiday = ENPCHoliday.NONE;
						if (data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							enpcholiday = data.ParseEnum<ENPCHoliday>(prefix + i.ToString() + "_Value", ENPCHoliday.NONE);
							if (enpcholiday == ENPCHoliday.NONE)
							{
								Assets.reportError(assetContext, "Holiday condition " + prefix + i.ToString() + " _Value is None");
							}
						}
						else
						{
							Assets.reportError(assetContext, "Holiday condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCHolidayCondition(enpcholiday, enpclogicType);
						break;
					}
					case ENPCConditionType.KILLS_PLAYER:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Player kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player kills condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), text2, flag);
						break;
					case ENPCConditionType.KILLS_OBJECT:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Object kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Object kills condition " + prefix + i.ToString() + " missing _Value");
						}
						Guid newObjectGuid;
						if (data.ContainsKey(prefix + i.ToString() + "_Object"))
						{
							newObjectGuid = data.ParseGuid(prefix + i.ToString() + "_Object", default(Guid));
						}
						else
						{
							newObjectGuid = default(Guid);
							Assets.reportError(assetContext, "Object kills condition " + prefix + i.ToString() + " missing _Object (GUID)");
						}
						byte newNav2 = data.ParseUInt8(prefix + i.ToString() + "_Nav", byte.MaxValue);
						inpccondition = new NPCObjectKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), newObjectGuid, newNav2, text2, flag);
						break;
					}
					case ENPCConditionType.CURRENCY:
					{
						string text5 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text5))
						{
							Assets.reportError(assetContext, "Currency condition " + prefix + i.ToString() + " missing _GUID");
						}
						string text6 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text6))
						{
							Assets.reportError(assetContext, "Currency condition " + prefix + i.ToString() + " missing _Value");
						}
						AssetReference<ItemCurrencyAsset> newCurrency = data.readAssetReference(text5);
						uint newValue = data.ParseUInt32(text6, 0U);
						inpccondition = new NPCCurrencyCondition(newCurrency, newValue, enpclogicType, text2, flag);
						break;
					}
					case ENPCConditionType.KILLS_TREE:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Tree kills condition " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Tree kills condition " + prefix + i.ToString() + " missing _Value");
						}
						Guid newTreeGuid;
						if (data.ContainsKey(prefix + i.ToString() + "_Tree"))
						{
							newTreeGuid = data.ParseGuid(prefix + i.ToString() + "_Tree", default(Guid));
						}
						else
						{
							newTreeGuid = default(Guid);
							Assets.reportError(assetContext, "Tree kills condition " + prefix + i.ToString() + " missing _Tree (GUID)");
						}
						inpccondition = new NPCTreeKillsCondition(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), newTreeGuid, text2, flag);
						break;
					}
					case ENPCConditionType.WEATHER_STATUS:
					{
						string text7 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text7))
						{
							Assets.reportError(assetContext, "Weather condition " + prefix + i.ToString() + " missing _GUID");
						}
						string text8 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text8))
						{
							Assets.reportError(assetContext, "Weather condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCWeatherStatusCondition(data.readAssetReference(text7), data.ParseEnum<ENPCWeatherStatus>(text8, ENPCWeatherStatus.Active), enpclogicType, text2);
						break;
					}
					case ENPCConditionType.WEATHER_BLEND_ALPHA:
					{
						string text9 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text9))
						{
							Assets.reportError(assetContext, "Weather condition " + prefix + i.ToString() + " missing _GUID");
						}
						string text10 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text10))
						{
							Assets.reportError(assetContext, "Weather condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCWeatherBlendAlphaCondition(data.readAssetReference(text9), data.ParseFloat(text10, 0f), enpclogicType, text2);
						break;
					}
					case ENPCConditionType.IS_FULL_MOON:
					{
						string text11 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text11))
						{
							Assets.reportError(assetContext, "Full moon condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCIsFullMoonCondition(data.ParseBool(text11, true), text2);
						break;
					}
					case ENPCConditionType.DATE_COUNTER:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Date counter condition " + prefix + i.ToString() + " missing _Value");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Divisor"))
						{
							Assets.reportError(assetContext, "Date counter condition " + prefix + i.ToString() + " missing _Divisor");
						}
						inpccondition = new NPCDateCounterCondition(data.ParseInt64(prefix + i.ToString() + "_Value", 0L), data.ParseInt64(prefix + i.ToString() + "_Divisor", 0L), enpclogicType, text2, flag);
						break;
					case ENPCConditionType.PLAYER_LIFE_STAMINA:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Player life stamina condition " + prefix + i.ToString() + " missing _Value");
						}
						inpccondition = new NPCPlayerLifeStaminaCondition(data.ParseInt32(prefix + i.ToString() + "_Value", 0), enpclogicType, text2);
						break;
					}
					if (inpccondition != null)
					{
						List<int> list = null;
						string text12;
						if (data.TryGetString(prefix + i.ToString() + "_UI_Requirements", out text12))
						{
							string[] array = text12.Split(',', 1);
							if (array == null || array.Length < 1)
							{
								Assets.reportError(assetContext, prefix + i.ToString() + " empty _UI_Requirements");
							}
							else
							{
								list = new List<int>(array.Length);
								foreach (string text13 in array)
								{
									int num;
									if (!int.TryParse(text13, ref num))
									{
										Assets.reportError(assetContext, string.Concat(new string[]
										{
											prefix,
											i.ToString(),
											" unable to parse _UI_Requirements index from \"",
											text13,
											"\""
										}));
									}
									else if (num < 0 || num >= conditions.Length)
									{
										Assets.reportError(assetContext, prefix + i.ToString() + string.Format(" UI requirement index {0} out of bounds", num));
									}
									else if (num == i)
									{
										Assets.reportError(assetContext, prefix + i.ToString() + " UI requirement depends on itself");
									}
									else
									{
										list.Add(num);
									}
								}
								if (list.Count < 1)
								{
									list = null;
								}
							}
						}
						inpccondition.uiRequirementIndices = list;
					}
					conditions[i] = inpccondition;
				}
			}
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0012A34C File Offset: 0x0012854C
		public static void readRewards(DatDictionary data, Local localization, string prefix, INPCReward[] rewards, Asset assetContext)
		{
			for (int i = 0; i < rewards.Length; i++)
			{
				string text = prefix + i.ToString() + "_Type";
				if (!data.ContainsKey(text))
				{
					throw new NotSupportedException("Missing " + text);
				}
				ENPCRewardType enpcrewardType = data.ParseEnum<ENPCRewardType>(text, ENPCRewardType.NONE);
				if (enpcrewardType == ENPCRewardType.NONE)
				{
					Assets.reportError(assetContext, "{0} unknown type", text);
				}
				else
				{
					string text2 = localization.read(prefix + i.ToString());
					text2 = ItemTool.filterRarityRichText(text2);
					INPCReward inpcreward = null;
					switch (enpcrewardType)
					{
					case ENPCRewardType.EXPERIENCE:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Experience reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCExperienceReward(data.ParseUInt32(prefix + i.ToString() + "_Value", 0U), text2);
						break;
					case ENPCRewardType.REPUTATION:
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Reputation reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCReputationReward(data.ParseInt32(prefix + i.ToString() + "_Value", 0), text2);
						break;
					case ENPCRewardType.FLAG_BOOL:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Bool flag reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Bool flag reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCBoolFlagReward(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseBool(prefix + i.ToString() + "_Value", false), text2);
						break;
					case ENPCRewardType.FLAG_SHORT:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Short flag reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Value"))
						{
							Assets.reportError(assetContext, "Short flag reward " + prefix + i.ToString() + " missing _Value");
						}
						string text3 = prefix + i.ToString() + "_Modification";
						if (!data.ContainsKey(text3))
						{
							Assets.reportError(assetContext, "Short flag reward " + prefix + i.ToString() + " missing _Modification");
						}
						inpcreward = new NPCShortFlagReward(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Value", 0), data.ParseEnum<ENPCModificationType>(text3, ENPCModificationType.NONE), text2);
						break;
					}
					case ENPCRewardType.FLAG_SHORT_RANDOM:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Random short flag reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Min_Value"))
						{
							Assets.reportError(assetContext, "Random short flag reward " + prefix + i.ToString() + " missing _Min_Value");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Max_Value"))
						{
							Assets.reportError(assetContext, "Random short flag reward " + prefix + i.ToString() + " missing _Max_Value");
						}
						string text4 = prefix + i.ToString() + "_Modification";
						if (!data.ContainsKey(text4))
						{
							Assets.reportError(assetContext, "Random short flag reward " + prefix + i.ToString() + " missing _Modification");
						}
						inpcreward = new NPCRandomShortFlagReward(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseInt16(prefix + i.ToString() + "_Min_Value", 0), data.ParseInt16(prefix + i.ToString() + "_Max_Value", 0), data.ParseEnum<ENPCModificationType>(text4, ENPCModificationType.NONE), text2);
						break;
					}
					case ENPCRewardType.QUEST:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Quest reward " + prefix + i.ToString() + " missing _ID");
						}
						Guid newQuestGuid;
						ushort newID;
						data.ParseGuidOrLegacyId(prefix + i.ToString() + "_ID", out newQuestGuid, out newID);
						inpcreward = new NPCQuestReward(newQuestGuid, newID, text2);
						break;
					}
					case ENPCRewardType.ITEM:
					{
						string text5 = prefix + i.ToString() + "_ID";
						if (!data.ContainsKey(text5))
						{
							Assets.reportError(assetContext, "Item reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Amount"))
						{
							Assets.reportError(assetContext, "Item reward " + prefix + i.ToString() + " missing _Amount");
						}
						Guid newItemGuid;
						ushort newID2;
						data.ParseGuidOrLegacyId(text5, out newItemGuid, out newID2);
						bool newShouldAutoEquip = data.ParseBool(prefix + i.ToString() + "_Auto_Equip", false);
						EItemOrigin origin = data.ParseEnum<EItemOrigin>(prefix + i.ToString() + "_Origin", EItemOrigin.CRAFT);
						inpcreward = new NPCItemReward(newItemGuid, newID2, data.ParseUInt8(prefix + i.ToString() + "_Amount", 0), newShouldAutoEquip, data.ParseInt32(prefix + i.ToString() + "_Sight", -1), data.ParseInt32(prefix + i.ToString() + "_Tactical", -1), data.ParseInt32(prefix + i.ToString() + "_Grip", -1), data.ParseInt32(prefix + i.ToString() + "_Barrel", -1), data.ParseInt32(prefix + i.ToString() + "_Magazine", -1), data.ParseInt32(prefix + i.ToString() + "_Ammo", -1), origin, text2);
						break;
					}
					case ENPCRewardType.ITEM_RANDOM:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Random item reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Amount"))
						{
							Assets.reportError(assetContext, "Random item reward " + prefix + i.ToString() + " missing _Amount");
						}
						bool newShouldAutoEquip2 = data.ParseBool(prefix + i.ToString() + "_Auto_Equip", false);
						EItemOrigin origin2 = data.ParseEnum<EItemOrigin>(prefix + i.ToString() + "_Origin", EItemOrigin.CRAFT);
						inpcreward = new NPCRandomItemReward(data.ParseUInt16(prefix + i.ToString() + "_ID", 0), data.ParseUInt8(prefix + i.ToString() + "_Amount", 0), newShouldAutoEquip2, origin2, text2);
						break;
					}
					case ENPCRewardType.ACHIEVEMENT:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Achievement reward " + prefix + i.ToString() + " missing _ID");
						}
						string @string = data.GetString(prefix + i.ToString() + "_ID", null);
						if (!Provider.statusData.Achievements.canBeGrantedByNPC(@string))
						{
							Assets.reportError(assetContext, "achievement \"{0}\" cannot be granted by NPCs", @string);
						}
						inpcreward = new NPCAchievementReward(@string, text2);
						break;
					}
					case ENPCRewardType.VEHICLE:
					{
						string text6 = prefix + i.ToString() + "_ID";
						if (!data.ContainsKey(text6))
						{
							Assets.reportError(assetContext, "Vehicle reward " + prefix + i.ToString() + " missing _ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Spawnpoint"))
						{
							Assets.reportError(assetContext, "Vehicle reward " + prefix + i.ToString() + " missing _Spawnpoint");
						}
						Guid newVehicleGuid;
						ushort newID3;
						data.ParseGuidOrLegacyId(text6, out newVehicleGuid, out newID3);
						Color32? newPaintColor = default(Color32?);
						Color32 color;
						if (data.TryParseColor32RGB(prefix + i.ToString() + "_PaintColor", out color))
						{
							newPaintColor = new Color32?(color);
						}
						inpcreward = new NPCVehicleReward(newVehicleGuid, newID3, data.GetString(prefix + i.ToString() + "_Spawnpoint", null), newPaintColor, text2);
						break;
					}
					case ENPCRewardType.TELEPORT:
						if (!data.ContainsKey(prefix + i.ToString() + "_Spawnpoint"))
						{
							Assets.reportError(assetContext, "Teleport reward " + prefix + i.ToString() + " missing _Spawnpoint");
						}
						inpcreward = new NPCTeleportReward(data.GetString(prefix + i.ToString() + "_Spawnpoint", null), text2);
						break;
					case ENPCRewardType.EVENT:
						if (!data.ContainsKey(prefix + i.ToString() + "_ID"))
						{
							Assets.reportError(assetContext, "Event reward " + prefix + i.ToString() + " missing _ID");
						}
						inpcreward = new NPCEventReward(data.GetString(prefix + i.ToString() + "_ID", null), text2);
						break;
					case ENPCRewardType.FLAG_MATH:
					{
						if (!data.ContainsKey(prefix + i.ToString() + "_A_ID"))
						{
							Assets.reportError(assetContext, "Math reward " + prefix + i.ToString() + " missing _A_ID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_B_ID") && !data.ContainsKey(prefix + i.ToString() + "_B_Value"))
						{
							Assets.reportError(assetContext, "Math reward " + prefix + i.ToString() + " missing _B_ID or _B_Value");
						}
						string text7 = prefix + i.ToString() + "_Operation";
						if (!data.ContainsKey(text7))
						{
							Assets.reportError(assetContext, "Math reward " + prefix + i.ToString() + " missing _Operation");
						}
						inpcreward = new NPCFlagMathReward(data.ParseUInt16(prefix + i.ToString() + "_A_ID", 0), data.ParseUInt16(prefix + i.ToString() + "_B_ID", 0), data.ParseInt16(prefix + i.ToString() + "_B_Value", 0), data.ParseEnum<ENPCOperationType>(text7, ENPCOperationType.NONE), text2);
						break;
					}
					case ENPCRewardType.CURRENCY:
					{
						string text8 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text8))
						{
							Assets.reportError(assetContext, "Currency reward " + prefix + i.ToString() + " missing _GUID");
						}
						string text9 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text9))
						{
							Assets.reportError(assetContext, "Currency reward " + prefix + i.ToString() + " missing _Value");
						}
						AssetReference<ItemCurrencyAsset> newCurrency = data.readAssetReference(text8);
						uint newValue = data.ParseUInt32(text9, 0U);
						inpcreward = new NPCCurrencyReward(newCurrency, newValue, text2);
						break;
					}
					case ENPCRewardType.HINT:
						if (string.IsNullOrEmpty(text2))
						{
							text2 = data.GetString(prefix + i.ToString() + "_Text", null);
						}
						inpcreward = new NPCHintReward(data.ParseFloat(prefix + i.ToString() + "_Duration", 2f), text2);
						break;
					case ENPCRewardType.PLAYER_SPAWNPOINT:
						inpcreward = new NPCPlayerSpawnpointReward(data.GetString(prefix + i.ToString() + "_ID", null), text2);
						break;
					case ENPCRewardType.PLAYER_LIFE_HEALTH:
					{
						string text10 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text10))
						{
							Assets.reportError(assetContext, "Player life health reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCPlayerLifeHealthReward(data.ParseInt32(text10, 0), text2);
						break;
					}
					case ENPCRewardType.PLAYER_LIFE_FOOD:
					{
						string text11 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text11))
						{
							Assets.reportError(assetContext, "Player life food reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCPlayerLifeFoodReward(data.ParseInt32(text11, 0), text2);
						break;
					}
					case ENPCRewardType.PLAYER_LIFE_WATER:
					{
						string text12 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text12))
						{
							Assets.reportError(assetContext, "Player life water reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCPlayerLifeWaterReward(data.ParseInt32(text12, 0), text2);
						break;
					}
					case ENPCRewardType.PLAYER_LIFE_VIRUS:
					{
						string text13 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text13))
						{
							Assets.reportError(assetContext, "Player life virus reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCPlayerLifeVirusReward(data.ParseInt32(text13, 0), text2);
						break;
					}
					case ENPCRewardType.REWARDS_LIST_ASSET:
					{
						string text14 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text14))
						{
							Assets.reportError(assetContext, "Rewards list asset reward " + prefix + i.ToString() + " missing _GUID");
						}
						inpcreward = new NPCRewardsListAssetReward(data.readAssetReference(text14), text2);
						break;
					}
					case ENPCRewardType.CUTSCENE_MODE:
					{
						string text15 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text15))
						{
							Assets.reportError(assetContext, "Cutscene mode reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCCutsceneModeReward(data.ParseBool(text15, false), text2);
						break;
					}
					case ENPCRewardType.PLAYER_LIFE_STAMINA:
					{
						string text16 = prefix + i.ToString() + "_Value";
						if (!data.ContainsKey(text16))
						{
							Assets.reportError(assetContext, "Player life stamina reward " + prefix + i.ToString() + " missing _Value");
						}
						inpcreward = new NPCPlayerLifeStaminaReward(data.ParseInt32(text16, 0), text2);
						break;
					}
					case ENPCRewardType.EFFECT:
					{
						string text17 = prefix + i.ToString() + "_GUID";
						if (!data.ContainsKey(text17))
						{
							Assets.reportError(assetContext, "Effect reward " + prefix + i.ToString() + " missing _GUID");
						}
						if (!data.ContainsKey(prefix + i.ToString() + "_Spawnpoint"))
						{
							Assets.reportError(assetContext, "Effect reward " + prefix + i.ToString() + " missing _Spawnpoint");
						}
						AssetReference<EffectAsset> newAssetRef = data.readAssetReference(text17);
						string string2 = data.GetString(prefix + i.ToString() + "_Spawnpoint", null);
						bool newIsReliable = data.ParseBool(prefix + i.ToString() + "_IsReliable", true);
						float newRelevantDistance = data.ParseFloat(prefix + i.ToString() + "_RelevantDistance", -1f);
						inpcreward = new NPCEffectReward(newAssetRef, string2, newIsReliable, newRelevantDistance, text2);
						break;
					}
					}
					if (inpcreward != null)
					{
						inpcreward.grantDelaySeconds = data.ParseFloat(prefix + i.ToString() + "_GrantDelaySeconds", -1f);
					}
					rewards[i] = inpcreward;
				}
			}
		}
	}
}
