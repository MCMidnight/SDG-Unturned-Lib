using System;
using UnityEngine;

namespace SDG.Unturned
{
	/// <summary>
	/// Seamlessly teleports player to an equivalent position at the destination upon contact.
	/// </summary>
	// Token: 0x020005C4 RID: 1476
	[AddComponentMenu("Unturned/Collision Teleporter")]
	public class CollisionTeleporter : MonoBehaviour
	{
		// Token: 0x06002FDC RID: 12252 RVA: 0x000D3930 File Offset: 0x000D1B30
		private void OnTriggerEnter(Collider other)
		{
			if (this.DestinationTransform != null)
			{
				PlayerMovement component = other.gameObject.GetComponent<PlayerMovement>();
				if (component != null && component.CanEnterTeleporter)
				{
					component.EnterCollisionTeleporter(this);
				}
			}
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x000D3970 File Offset: 0x000D1B70
		private void OnDrawGizmos()
		{
			if (this.DestinationTransform == null)
			{
				return;
			}
			BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
			if (component == null)
			{
				return;
			}
			Gizmos.color = this.GizmoColor;
			Gizmos.DrawLine(base.transform.position, this.DestinationTransform.position);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 0f, 1f));
			Gizmos.DrawLine(new Vector3(0f, 0f, 1f), new Vector3(-0.25f, 0f, 0.75f));
			Gizmos.DrawLine(new Vector3(0f, 0f, 1f), new Vector3(0.25f, 0f, 0.75f));
			Gizmos.matrix = this.DestinationTransform.localToWorldMatrix;
			Gizmos.DrawWireCube(component.center, component.size);
			Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 0f, 1f));
			Gizmos.DrawLine(new Vector3(0f, 0f, 1f), new Vector3(-0.25f, 0f, 0.75f));
			Gizmos.DrawLine(new Vector3(0f, 0f, 1f), new Vector3(0.25f, 0f, 0.75f));
		}

		/// <summary>
		/// Target position and rotation.
		/// </summary>
		// Token: 0x040019D7 RID: 6615
		public Transform DestinationTransform;

		/// <summary>
		/// Only used in the Unity editor for visualization.
		/// </summary>
		// Token: 0x040019D8 RID: 6616
		public Color GizmoColor = Color.blue;
	}
}
