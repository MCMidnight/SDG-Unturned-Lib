using System;

namespace SDG.Unturned
{
	// Token: 0x0200035B RID: 859
	public struct PhysicsMaterialNetId : IEquatable<PhysicsMaterialNetId>
	{
		// Token: 0x060019F4 RID: 6644 RVA: 0x0005D73A File Offset: 0x0005B93A
		public PhysicsMaterialNetId(uint id)
		{
			this.id = id;
		}

		/// <summary>
		/// Zero is treated as unset.
		/// </summary>
		// Token: 0x060019F5 RID: 6645 RVA: 0x0005D743 File Offset: 0x0005B943
		public bool IsNull()
		{
			return this.id == 0U;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x0005D74E File Offset: 0x0005B94E
		public void Clear()
		{
			this.id = 0U;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x0005D757 File Offset: 0x0005B957
		public override bool Equals(object obj)
		{
			return obj is PhysicsMaterialNetId && this.id == ((PhysicsMaterialNetId)obj).id;
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x0005D776 File Offset: 0x0005B976
		public bool Equals(PhysicsMaterialNetId other)
		{
			return this.id == other.id;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x0005D786 File Offset: 0x0005B986
		public override int GetHashCode()
		{
			return this.id.GetHashCode();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0005D793 File Offset: 0x0005B993
		public override string ToString()
		{
			return this.id.ToString("X2");
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005D7A5 File Offset: 0x0005B9A5
		public static bool operator ==(PhysicsMaterialNetId lhs, PhysicsMaterialNetId rhs)
		{
			return lhs.id == rhs.id;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x0005D7B5 File Offset: 0x0005B9B5
		public static bool operator !=(PhysicsMaterialNetId lhs, PhysicsMaterialNetId rhs)
		{
			return lhs.id != rhs.id;
		}

		// Token: 0x04000BE1 RID: 3041
		public static readonly PhysicsMaterialNetId NULL = new PhysicsMaterialNetId(0U);

		// Token: 0x04000BE2 RID: 3042
		public uint id;
	}
}
