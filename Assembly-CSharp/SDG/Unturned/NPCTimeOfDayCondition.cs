using System;

namespace SDG.Unturned
{
	// Token: 0x0200034A RID: 842
	public class NPCTimeOfDayCondition : NPCLogicCondition
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x0600193A RID: 6458 RVA: 0x0005AA61 File Offset: 0x00058C61
		// (set) Token: 0x0600193B RID: 6459 RVA: 0x0005AA69 File Offset: 0x00058C69
		public int second { get; protected set; }

		// Token: 0x0600193C RID: 6460 RVA: 0x0005AA74 File Offset: 0x00058C74
		public override bool isConditionMet(Player player)
		{
			float num;
			if (LightingManager.day < LevelLighting.bias)
			{
				num = LightingManager.day / LevelLighting.bias;
				num /= 2f;
			}
			else
			{
				num = (LightingManager.day - LevelLighting.bias) / (1f - LevelLighting.bias);
				num = 0.5f + num / 2f;
			}
			num += 0.25f;
			if (num >= 1f)
			{
				num -= 1f;
			}
			int a = (int)(num * 86400f);
			return base.doesLogicPass<int>(a, this.second);
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x0005AAF8 File Offset: 0x00058CF8
		public override string formatCondition(Player player)
		{
			if (string.IsNullOrEmpty(this.text))
			{
				return null;
			}
			int num = this.second / 3600;
			int num2 = this.second / 60 - num * 60;
			int num3 = this.second - num * 3600 - num2 * 60;
			string text = string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, num3);
			return string.Format(this.text, text);
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x0005AB6D File Offset: 0x00058D6D
		public NPCTimeOfDayCondition(int newSecond, ENPCLogicType newLogicType, string newText, bool newShouldReset) : base(newLogicType, newText, newShouldReset)
		{
			this.second = newSecond;
		}
	}
}
