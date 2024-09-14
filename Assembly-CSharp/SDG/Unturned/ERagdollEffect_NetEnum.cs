using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x020001C8 RID: 456
	public static class ERagdollEffect_NetEnum
	{
		// Token: 0x06000DF4 RID: 3572 RVA: 0x00030DB0 File Offset: 0x0002EFB0
		public static bool ReadEnum(this NetPakReader reader, out ERagdollEffect value)
		{
			uint num;
			bool result = reader.ReadBits(4, ref num);
			if (num <= 9U)
			{
				value = (ERagdollEffect)num;
				return result;
			}
			value = ERagdollEffect.NONE;
			return false;
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00030DD8 File Offset: 0x0002EFD8
		public static bool WriteEnum(this NetPakWriter writer, ERagdollEffect value)
		{
			return writer.WriteBits((uint)value, 4);
		}
	}
}
