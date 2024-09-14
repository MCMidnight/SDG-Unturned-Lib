using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000011 RID: 17
	[StructLayout(2)]
	internal struct GuidBuffer
	{
		// Token: 0x0600003F RID: 63 RVA: 0x000030B7 File Offset: 0x000012B7
		public GuidBuffer(Guid GUID)
		{
			this = default(GuidBuffer);
			this.GUID = GUID;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000030C8 File Offset: 0x000012C8
		public unsafe void Read(byte[] source, int offset)
		{
			if (offset + 16 > source.Length)
			{
				throw new ArgumentException("Source buffer is too small!");
			}
			fixed (byte[] array = source)
			{
				byte* ptr;
				if (source == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (ulong* ptr2 = &this.buffer.FixedElementField)
				{
					ulong* ptr3 = ptr2;
					ulong* ptr4 = (ulong*)(ptr + offset);
					*ptr3 = *ptr4;
					ptr3[1] = ptr4[1];
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003124 File Offset: 0x00001324
		public unsafe void Write(byte[] destination, int offset)
		{
			if (offset + 16 > destination.Length)
			{
				throw new ArgumentException("Destination buffer is too small!");
			}
			fixed (byte[] array = destination)
			{
				byte* ptr;
				if (destination == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (ulong* ptr2 = &this.buffer.FixedElementField)
				{
					ulong* ptr3 = ptr2;
					ulong* ptr4 = (ulong*)(ptr + offset);
					*ptr4 = *ptr3;
					ptr4[1] = ptr3[1];
				}
			}
		}

		// Token: 0x04000028 RID: 40
		public static readonly byte[] GUID_BUFFER = new byte[16];

		// Token: 0x04000029 RID: 41
		[FixedBuffer(typeof(ulong), 2)]
		[FieldOffset(0)]
		private GuidBuffer.<buffer>e__FixedBuffer buffer;

		// Token: 0x0400002A RID: 42
		[FieldOffset(0)]
		public Guid GUID;

		// Token: 0x0200082D RID: 2093
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 16)]
		public struct <buffer>e__FixedBuffer
		{
			// Token: 0x0400311F RID: 12575
			public ulong FixedElementField;
		}
	}
}
