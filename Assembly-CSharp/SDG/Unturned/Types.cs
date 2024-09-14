using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006B9 RID: 1721
	public class Types
	{
		// Token: 0x0400221B RID: 8731
		public static readonly Type STRING_TYPE = typeof(string);

		// Token: 0x0400221C RID: 8732
		public static readonly Type STRING_ARRAY_TYPE = typeof(string[]);

		// Token: 0x0400221D RID: 8733
		public static readonly Type BOOLEAN_TYPE = typeof(bool);

		// Token: 0x0400221E RID: 8734
		public static readonly Type BOOLEAN_ARRAY_TYPE = typeof(bool[]);

		// Token: 0x0400221F RID: 8735
		public static readonly Type BYTE_ARRAY_TYPE = typeof(byte[]);

		// Token: 0x04002220 RID: 8736
		public static readonly Type BYTE_TYPE = typeof(byte);

		// Token: 0x04002221 RID: 8737
		public static readonly Type INT16_TYPE = typeof(short);

		// Token: 0x04002222 RID: 8738
		public static readonly Type UINT16_TYPE = typeof(ushort);

		// Token: 0x04002223 RID: 8739
		public static readonly Type INT32_ARRAY_TYPE = typeof(int[]);

		// Token: 0x04002224 RID: 8740
		public static readonly Type INT32_TYPE = typeof(int);

		// Token: 0x04002225 RID: 8741
		public static readonly Type UINT32_TYPE = typeof(uint);

		// Token: 0x04002226 RID: 8742
		public static readonly Type SINGLE_TYPE = typeof(float);

		// Token: 0x04002227 RID: 8743
		public static readonly Type INT64_TYPE = typeof(long);

		// Token: 0x04002228 RID: 8744
		public static readonly Type UINT64_ARRAY_TYPE = typeof(ulong[]);

		// Token: 0x04002229 RID: 8745
		public static readonly Type UINT64_TYPE = typeof(ulong);

		// Token: 0x0400222A RID: 8746
		public static readonly Type STEAM_ID_TYPE = typeof(CSteamID);

		// Token: 0x0400222B RID: 8747
		public static readonly Type GUID_TYPE = typeof(Guid);

		// Token: 0x0400222C RID: 8748
		public static readonly Type VECTOR3_TYPE = typeof(Vector3);

		// Token: 0x0400222D RID: 8749
		public static readonly Type COLOR_TYPE = typeof(Color);

		/// <summary>
		/// Not originally supported by networking. Added temporarily during netpak rewrite because the quaternion
		/// compression is so much better for vehicles than three byte Euler rotation.
		/// </summary>
		// Token: 0x0400222E RID: 8750
		public static readonly Type QUATERNION_TYPE = typeof(Quaternion);

		// Token: 0x0400222F RID: 8751
		public static readonly byte[] SHIFTS = new byte[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			64,
			128
		};
	}
}
