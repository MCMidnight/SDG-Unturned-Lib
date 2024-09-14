using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C0 RID: 448
	public static class EPhysicsMaterial_NetEnum
	{
		// Token: 0x06000DE4 RID: 3556 RVA: 0x00030BD0 File Offset: 0x0002EDD0
		public static bool ReadEnum(this NetPakReader reader, out EPhysicsMaterial value)
		{
			uint num;
			bool result = reader.ReadBits(5, ref num);
			if (num <= 21U)
			{
				value = (EPhysicsMaterial)num;
				return result;
			}
			value = EPhysicsMaterial.NONE;
			return false;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00030BF8 File Offset: 0x0002EDF8
		public static bool WriteEnum(this NetPakWriter writer, EPhysicsMaterial value)
		{
			return writer.WriteBits((uint)value, 5);
		}
	}
}
