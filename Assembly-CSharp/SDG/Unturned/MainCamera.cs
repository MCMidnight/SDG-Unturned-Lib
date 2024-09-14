using System;
using System.Collections;
using SDG.Framework.Rendering;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200043E RID: 1086
	public class MainCamera : MonoBehaviour
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x0007E301 File Offset: 0x0007C501
		// (set) Token: 0x060020BE RID: 8382 RVA: 0x0007E308 File Offset: 0x0007C508
		public static Camera instance
		{
			get
			{
				return MainCamera._instance;
			}
			protected set
			{
				if (MainCamera.instance != value)
				{
					MainCamera._instance = value;
					MainCamera.triggerInstanceChanged();
				}
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x0007E322 File Offset: 0x0007C522
		// (set) Token: 0x060020C0 RID: 8384 RVA: 0x0007E329 File Offset: 0x0007C529
		public static bool isAvailable
		{
			get
			{
				return MainCamera._isAvailable;
			}
			protected set
			{
				if (MainCamera.isAvailable != value)
				{
					MainCamera._isAvailable = value;
					MainCamera.triggerAvailabilityChanged();
				}
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060020C1 RID: 8385 RVA: 0x0007E340 File Offset: 0x0007C540
		// (remove) Token: 0x060020C2 RID: 8386 RVA: 0x0007E374 File Offset: 0x0007C574
		public static event MainCameraInstanceChangedHandler instanceChanged;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060020C3 RID: 8387 RVA: 0x0007E3A8 File Offset: 0x0007C5A8
		// (remove) Token: 0x060020C4 RID: 8388 RVA: 0x0007E3DC File Offset: 0x0007C5DC
		public static event MainCameraAvailabilityChangedHandler availabilityChanged;

		// Token: 0x060020C5 RID: 8389 RVA: 0x0007E40F File Offset: 0x0007C60F
		public IEnumerator activate()
		{
			yield return new WaitForEndOfFrame();
			MainCamera.isAvailable = true;
			yield break;
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x0007E417 File Offset: 0x0007C617
		protected static void triggerInstanceChanged()
		{
			MainCameraInstanceChangedHandler mainCameraInstanceChangedHandler = MainCamera.instanceChanged;
			if (mainCameraInstanceChangedHandler == null)
			{
				return;
			}
			mainCameraInstanceChangedHandler();
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x0007E428 File Offset: 0x0007C628
		protected static void triggerAvailabilityChanged()
		{
			MainCameraAvailabilityChangedHandler mainCameraAvailabilityChangedHandler = MainCamera.availabilityChanged;
			if (mainCameraAvailabilityChangedHandler == null)
			{
				return;
			}
			mainCameraAvailabilityChangedHandler();
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x0007E43C File Offset: 0x0007C63C
		public void Awake()
		{
			MainCamera.isAvailable = false;
			MainCamera.instance = base.transform.GetComponent<Camera>();
			MainCamera.instance.eventMask = 0;
			base.StartCoroutine(this.activate());
			UnturnedPostProcess.instance.setBaseCamera(MainCamera.instance);
			base.gameObject.GetOrAddComponent<GLRenderer>();
		}

		// Token: 0x04000FFA RID: 4090
		protected static Camera _instance;

		// Token: 0x04000FFB RID: 4091
		protected static bool _isAvailable;
	}
}
