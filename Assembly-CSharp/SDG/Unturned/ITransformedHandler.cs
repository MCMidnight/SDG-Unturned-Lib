using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200041C RID: 1052
	public interface ITransformedHandler
	{
		// Token: 0x06001F2F RID: 7983
		void OnTransformed(Vector3 oldPosition, Quaternion oldRotation, Vector3 oldLocalScale, Vector3 newPosition, Quaternion newRotation, Vector3 newLocalScale, bool modifyRotation, bool modifyScale);
	}
}
