using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000664 RID: 1636
	public class Skill
	{
		/// <summary>
		/// Get maximum level, or maxUnlockableLevel if set.
		/// </summary>
		/// <returns></returns>
		// Token: 0x060036A6 RID: 13990 RVA: 0x0010055E File Offset: 0x000FE75E
		public int GetClampedMaxUnlockableLevel()
		{
			if (this.maxUnlockableLevel <= -1)
			{
				return (int)this.max;
			}
			return Mathf.Min((int)this.max, this.maxUnlockableLevel);
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x00100581 File Offset: 0x000FE781
		public void setLevelToMax()
		{
			this.level = this.max;
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060036A8 RID: 13992 RVA: 0x0010058F File Offset: 0x000FE78F
		public float mastery
		{
			get
			{
				if (this.level == 0)
				{
					return 0f;
				}
				if (this.level >= this.max)
				{
					return 1f;
				}
				return (float)this.level / (float)this.max;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x001005C2 File Offset: 0x000FE7C2
		public uint cost
		{
			get
			{
				return MathfEx.RoundAndClampToUInt(this._cost * ((float)this.level * this.difficulty + 1f) * this.costMultiplier);
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x001005ED File Offset: 0x000FE7ED
		public Skill(byte newLevel, byte newMax, uint newCost, float newDifficulty)
		{
			this.level = newLevel;
			this.max = newMax;
			this._cost = newCost;
			this.difficulty = newDifficulty;
		}

		// Token: 0x04001FA0 RID: 8096
		public byte level;

		/// <summary>
		/// Vanilla maximum level.
		/// </summary>
		// Token: 0x04001FA1 RID: 8097
		public byte max;

		/// <summary>
		/// If set, maximum skill level attainable through gameplay.
		/// </summary>
		// Token: 0x04001FA2 RID: 8098
		public int maxUnlockableLevel = -1;

		/// <summary>
		/// Multiplier for XP upgrade cost.
		/// </summary>
		// Token: 0x04001FA3 RID: 8099
		public float costMultiplier = 1f;

		// Token: 0x04001FA4 RID: 8100
		private uint _cost;

		// Token: 0x04001FA5 RID: 8101
		private float difficulty;
	}
}
