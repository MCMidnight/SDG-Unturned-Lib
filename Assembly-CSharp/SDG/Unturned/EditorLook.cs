using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000401 RID: 1025
	public class EditorLook : MonoBehaviour
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x0006E2B1 File Offset: 0x0006C4B1
		public static float pitch
		{
			get
			{
				return EditorLook._pitch;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x0006E2B8 File Offset: 0x0006C4B8
		public static float yaw
		{
			get
			{
				return EditorLook._yaw;
			}
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0006E2C0 File Offset: 0x0006C4C0
		private void Update()
		{
			if (EditorInteract.isFlying && Level.isEditor)
			{
				MainCamera.instance.fieldOfView = Mathf.Lerp(MainCamera.instance.fieldOfView, OptionsSettings.DesiredVerticalFieldOfView + (float)((EditorMovement.isMoving && InputEx.GetKey(ControlsSettings.modify)) ? 10 : 0), 8f * Time.deltaTime);
				this.highlightCamera.fieldOfView = MainCamera.instance.fieldOfView;
				EditorLook._yaw += ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_x");
				if (ControlsSettings.invert)
				{
					EditorLook._pitch += ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_y");
				}
				else
				{
					EditorLook._pitch -= ControlsSettings.mouseAimSensitivity * Input.GetAxis("mouse_y");
				}
				if (EditorLook.pitch > 90f)
				{
					EditorLook._pitch = 90f;
				}
				else if (EditorLook.pitch < -90f)
				{
					EditorLook._pitch = -90f;
				}
				MainCamera.instance.transform.localRotation = Quaternion.Euler(EditorLook.pitch, 0f, 0f);
				base.transform.rotation = Quaternion.Euler(0f, EditorLook.yaw, 0f);
			}
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0006E404 File Offset: 0x0006C604
		private void Start()
		{
			MainCamera.instance.fieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
			this.highlightCamera = MainCamera.instance.transform.Find("HighlightCamera").GetComponent<Camera>();
			this.highlightCamera.fieldOfView = OptionsSettings.DesiredVerticalFieldOfView;
			this.highlightCamera.eventMask = 0;
			EditorLook._pitch = MainCamera.instance.transform.localRotation.eulerAngles.x;
			if (EditorLook.pitch > 90f)
			{
				EditorLook._pitch = -360f + EditorLook.pitch;
			}
			EditorLook._yaw = base.transform.rotation.eulerAngles.y;
		}

		// Token: 0x04000E85 RID: 3717
		private static float _pitch;

		// Token: 0x04000E86 RID: 3718
		private static float _yaw;

		// Token: 0x04000E87 RID: 3719
		private Camera highlightCamera;
	}
}
