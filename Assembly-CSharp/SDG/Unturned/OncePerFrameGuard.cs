using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// True once per frame, false otherwise.
	/// </summary>
	// Token: 0x0200080A RID: 2058
	public struct OncePerFrameGuard
	{
		// Token: 0x06004672 RID: 18034 RVA: 0x001A3FD8 File Offset: 0x001A21D8
		public bool Consume()
		{
			int frameCount = Time.frameCount;
			if (frameCount > this.consumedFrameNumber)
			{
				this.consumedFrameNumber = frameCount;
				return true;
			}
			return false;
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004673 RID: 18035 RVA: 0x001A3FFE File Offset: 0x001A21FE
		public bool HasBeenConsumed
		{
			get
			{
				return Time.frameCount == this.consumedFrameNumber;
			}
		}

		// Token: 0x04002F8C RID: 12172
		private int consumedFrameNumber;
	}
}
