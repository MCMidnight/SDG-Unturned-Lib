using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020004FB RID: 1275
	public class LocationNode : Node
	{
		// Token: 0x0600280C RID: 10252 RVA: 0x000A9564 File Offset: 0x000A7764
		public LocationNode(Vector3 newPoint) : this(newPoint, "")
		{
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000A9572 File Offset: 0x000A7772
		public LocationNode(Vector3 newPoint, string newName)
		{
			this._point = newPoint;
			this.name = newName;
			this._type = ENodeType.LOCATION;
		}

		// Token: 0x04001537 RID: 5431
		public string name;
	}
}
