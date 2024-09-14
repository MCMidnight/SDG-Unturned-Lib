using System;

namespace SDG.Unturned
{
	// Token: 0x0200023C RID: 572
	public struct NetId : IEquatable<NetId>
	{
		// Token: 0x060011B8 RID: 4536 RVA: 0x0003CE32 File Offset: 0x0003B032
		public NetId(uint id)
		{
			this.id = id;
		}

		/// <summary>
		/// Zero is treated as unset.
		/// </summary>
		// Token: 0x060011B9 RID: 4537 RVA: 0x0003CE3B File Offset: 0x0003B03B
		public bool IsNull()
		{
			return this.id == 0U;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0003CE46 File Offset: 0x0003B046
		public void Clear()
		{
			this.id = 0U;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0003CE4F File Offset: 0x0003B04F
		public override bool Equals(object obj)
		{
			return obj is NetId && this.id == ((NetId)obj).id;
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x0003CE6E File Offset: 0x0003B06E
		public bool Equals(NetId other)
		{
			return this.id == other.id;
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0003CE7E File Offset: 0x0003B07E
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0003CE8B File Offset: 0x0003B08B
		public override string ToString()
		{
			return this.id.ToString("X8");
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x0003CE9D File Offset: 0x0003B09D
		public static bool operator ==(NetId lhs, NetId rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x0003CEAD File Offset: 0x0003B0AD
		public static bool operator !=(NetId lhs, NetId rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
		public static NetId operator +(NetId lhs, uint rhs)
		{
			return new NetId(lhs.id + rhs);
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0003CECF File Offset: 0x0003B0CF
		public static NetId operator ++(NetId value)
		{
			return new NetId(value.id + 1U);
		}

		// Token: 0x04000575 RID: 1397
		public static readonly NetId INVALID = new NetId(0U);

		// Token: 0x04000576 RID: 1398
		public uint id;
	}
}
