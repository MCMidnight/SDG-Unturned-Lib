using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C9 RID: 457
	public static class ERaycastInfoType_NetEnum
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00030DF0 File Offset: 0x0002EFF0
		public static bool ReadEnum(this NetPakReader reader, out ERaycastInfoType value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			if (num <= 9U)
			{
				value = (ERaycastInfoType)num;
				return result;
			}
			value = ERaycastInfoType.NONE;
			return false;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00030E18 File Offset: 0x0002F018
		public static bool WriteEnum(this NetPakWriter writer, ERaycastInfoType value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
