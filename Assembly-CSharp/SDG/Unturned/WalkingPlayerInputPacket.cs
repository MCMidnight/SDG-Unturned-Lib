using System;
using SDG.NetPak;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000623 RID: 1571
	public class WalkingPlayerInputPacket : PlayerInputPacket
	{
		// Token: 0x060032A7 RID: 12967 RVA: 0x000E5010 File Offset: 0x000E3210
		public override void read(SteamChannel channel, NetPakReader reader)
		{
			base.read(channel, reader);
			SystemNetPakReaderEx.ReadUInt8(reader, ref this.analog);
			UnityNetPakReaderEx.ReadClampedVector3(reader, ref this.clientPosition, 13, 7);
			SystemNetPakReaderEx.ReadFloat(reader, ref this.yaw);
			SystemNetPakReaderEx.ReadFloat(reader, ref this.pitch);
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x000E505C File Offset: 0x000E325C
		public override void write(NetPakWriter writer)
		{
			base.write(writer);
			SystemNetPakWriterEx.WriteUInt8(writer, this.analog);
			UnityNetPakWriterEx.WriteClampedVector3(writer, this.clientPosition, 13, 7);
			SystemNetPakWriterEx.WriteFloat(writer, this.yaw);
			SystemNetPakWriterEx.WriteFloat(writer, this.pitch);
		}

		// Token: 0x04001D1B RID: 7451
		public byte analog;

		/// <summary>
		/// Resulting transform.position immediately after movement.simulate was called.
		/// </summary>
		// Token: 0x04001D1C RID: 7452
		public Vector3 clientPosition;

		// Token: 0x04001D1D RID: 7453
		public float yaw;

		// Token: 0x04001D1E RID: 7454
		public float pitch;
	}
}
