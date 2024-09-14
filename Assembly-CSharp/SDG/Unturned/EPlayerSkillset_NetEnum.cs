using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C5 RID: 453
	public static class EPlayerSkillset_NetEnum
	{
		// Token: 0x06000DEE RID: 3566 RVA: 0x00030CFC File Offset: 0x0002EEFC
		public static bool ReadEnum(this NetPakReader reader, out EPlayerSkillset value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			if (num <= 10U)
			{
				value = (EPlayerSkillset)num;
				return result;
			}
			value = EPlayerSkillset.NONE;
			return false;
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00030D24 File Offset: 0x0002EF24
		public static bool WriteEnum(this NetPakWriter writer, EPlayerSkillset value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
