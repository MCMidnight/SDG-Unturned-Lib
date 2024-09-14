using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x0200035C RID: 860
	public static class PhysicsMaterialNetIdPakEx
	{
		// Token: 0x060019FE RID: 6654 RVA: 0x0005D7D5 File Offset: 0x0005B9D5
		public static bool ReadPhysicsMaterialNetId(this NetPakReader reader, out PhysicsMaterialNetId value)
		{
			return reader.ReadBits(PhysicsMaterialNetTable.idBitCount, ref value.id);
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0005D7E8 File Offset: 0x0005B9E8
		public static bool WritePhysicsMaterialNetId(this NetPakWriter writer, PhysicsMaterialNetId value)
		{
			return writer.WriteBits(value.id, PhysicsMaterialNetTable.idBitCount);
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005D7FC File Offset: 0x0005B9FC
		public static bool ReadPhysicsMaterialName(this NetPakReader reader, out string materialName)
		{
			PhysicsMaterialNetId netId;
			bool result = reader.ReadPhysicsMaterialNetId(out netId);
			materialName = PhysicsMaterialNetTable.GetMaterialName(netId);
			return result;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005D81C File Offset: 0x0005BA1C
		public static bool WritePhysicsMaterialName(this NetPakWriter writer, string materialName)
		{
			PhysicsMaterialNetId netId = PhysicsMaterialNetTable.GetNetId(materialName);
			return writer.WritePhysicsMaterialNetId(netId);
		}
	}
}
