using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000341 RID: 833
	public class NPCRandomShortFlagReward : NPCShortFlagReward
	{
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001905 RID: 6405 RVA: 0x0005A34D File Offset: 0x0005854D
		// (set) Token: 0x06001906 RID: 6406 RVA: 0x0005A355 File Offset: 0x00058555
		public short minValue { get; protected set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001907 RID: 6407 RVA: 0x0005A35E File Offset: 0x0005855E
		// (set) Token: 0x06001908 RID: 6408 RVA: 0x0005A366 File Offset: 0x00058566
		public short maxValue { get; protected set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001909 RID: 6409 RVA: 0x0005A36F File Offset: 0x0005856F
		// (set) Token: 0x0600190A RID: 6410 RVA: 0x0005A385 File Offset: 0x00058585
		public override short value
		{
			get
			{
				return (short)Random.Range((int)this.minValue, (int)(this.maxValue + 1));
			}
			protected set
			{
			}
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0005A387 File Offset: 0x00058587
		public NPCRandomShortFlagReward(ushort newID, short newMinValue, short newMaxValue, ENPCModificationType newModificationType, string newText) : base(newID, 0, newModificationType, newText)
		{
			base.id = newID;
			this.minValue = newMinValue;
			this.maxValue = newMaxValue;
			base.modificationType = newModificationType;
		}
	}
}
