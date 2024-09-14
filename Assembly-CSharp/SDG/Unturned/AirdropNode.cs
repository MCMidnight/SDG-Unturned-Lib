using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004A5 RID: 1189
	public class AirdropNode : Node
	{
		// Token: 0x060024EE RID: 9454 RVA: 0x000931F5 File Offset: 0x000913F5
		public AirdropNode(Vector3 newPoint) : this(newPoint, 0)
		{
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x000931FF File Offset: 0x000913FF
		public AirdropNode(Vector3 newPoint, ushort newID)
		{
			this._point = newPoint;
			this.id = newID;
			this._type = ENodeType.AIRDROP;
		}

		// Token: 0x040012C3 RID: 4803
		public ushort id;
	}
}
