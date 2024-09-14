using System;
using System.Collections;
using System.Collections.Generic;

namespace SDG.Unturned
{
	// Token: 0x02000369 RID: 873
	public struct SpawnTableRewardEnumerator : IEnumerable<ushort>, IEnumerable, IEnumerator<ushort>, IEnumerator, IDisposable
	{
		// Token: 0x06001A6E RID: 6766 RVA: 0x0005F929 File Offset: 0x0005DB29
		public SpawnTableRewardEnumerator(ushort tableID, int count)
		{
			this.tableID = tableID;
			this.assetID = 0;
			this.count = count;
			this.index = -1;
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001A6F RID: 6767 RVA: 0x0005F947 File Offset: 0x0005DB47
		public ushort Current
		{
			get
			{
				return this.assetID;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0005F94F File Offset: 0x0005DB4F
		object IEnumerator.Current
		{
			get
			{
				return this.assetID;
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0005F95C File Offset: 0x0005DB5C
		public void Dispose()
		{
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0005F95E File Offset: 0x0005DB5E
		public IEnumerator<ushort> GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0005F96C File Offset: 0x0005DB6C
		public bool MoveNext()
		{
			do
			{
				int num = this.index + 1;
				this.index = num;
				if (num >= this.count)
				{
					return false;
				}
				this.assetID = SpawnTableTool.ResolveLegacyId(this.tableID, EAssetType.ITEM, new Func<string>(this.OnGetSpawnTableErrorContext));
			}
			while (this.assetID == 0);
			return true;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0005F9C7 File Offset: 0x0005DBC7
		public void Reset()
		{
			this.index = -1;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0005F9D0 File Offset: 0x0005DBD0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0005F9DD File Offset: 0x0005DBDD
		private string OnGetSpawnTableErrorContext()
		{
			return "consumable item";
		}

		// Token: 0x04000C33 RID: 3123
		public ushort tableID;

		// Token: 0x04000C34 RID: 3124
		public ushort assetID;

		// Token: 0x04000C35 RID: 3125
		public int count;

		// Token: 0x04000C36 RID: 3126
		public int index;
	}
}
