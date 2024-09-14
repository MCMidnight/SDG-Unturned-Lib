using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000402 RID: 1026
	public class EditorMovement : MonoBehaviour
	{
		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x0006E4BD File Offset: 0x0006C6BD
		public static bool isMoving
		{
			get
			{
				return EditorMovement._isMoving;
			}
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0006E4C4 File Offset: 0x0006C6C4
		private void Update()
		{
			if (EditorInteract.isFlying && Level.isEditor)
			{
				if (InputEx.GetKey(ControlsSettings.left))
				{
					this.input.x = -1f;
				}
				else if (InputEx.GetKey(ControlsSettings.right))
				{
					this.input.x = 1f;
				}
				else
				{
					this.input.x = 0f;
				}
				if (InputEx.GetKey(ControlsSettings.up))
				{
					this.input.z = 1f;
				}
				else if (InputEx.GetKey(ControlsSettings.down))
				{
					this.input.z = -1f;
				}
				else
				{
					this.input.z = 0f;
				}
				EditorMovement._isMoving = (this.input.x != 0f || this.input.z != 0f);
				this.speed = Mathf.Clamp(this.speed + Input.GetAxis("mouse_z") * 0.2f * this.speed, 0.5f, 2048f);
				float d = 0f;
				if (InputEx.GetKey(ControlsSettings.ascend))
				{
					d = 1f;
				}
				else if (InputEx.GetKey(ControlsSettings.descend))
				{
					d = -1f;
				}
				base.transform.position += MainCamera.instance.transform.rotation * this.input * this.speed * Time.deltaTime + Vector3.up * d * Time.deltaTime * this.speed;
			}
		}

		// Token: 0x04000E88 RID: 3720
		private static bool _isMoving;

		// Token: 0x04000E89 RID: 3721
		private float speed = 32f;

		// Token: 0x04000E8A RID: 3722
		private Vector3 input;
	}
}
