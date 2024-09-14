using System;
using System.Collections.Generic;
using SDG.NetTransport;

namespace SDG.Unturned
{
	// Token: 0x02000263 RID: 611
	public class PooledTransportConnectionList : List<ITransportConnection>
	{
		// Token: 0x0600125D RID: 4701 RVA: 0x0003EDFA File Offset: 0x0003CFFA
		internal PooledTransportConnectionList(int capacity) : base(capacity)
		{
		}
	}
}
