using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020005B7 RID: 1463
	public class MenuConfigurationControls : MonoBehaviour
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x000D230E File Offset: 0x000D050E
		// (set) Token: 0x06002F98 RID: 12184 RVA: 0x000D2315 File Offset: 0x000D0515
		public static byte binding
		{
			get
			{
				return MenuConfigurationControls._binding;
			}
			set
			{
				MenuConfigurationControls._binding = value;
			}
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x000D231D File Offset: 0x000D051D
		private static void cancel()
		{
			MenuConfigurationControlsUI.cancel();
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x000D232E File Offset: 0x000D052E
		private static void bind(KeyCode key)
		{
			MenuConfigurationControlsUI.bind(key);
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x000D2340 File Offset: 0x000D0540
		private void Update()
		{
			if (MenuConfigurationControls.binding != 255)
			{
				if (Event.current.type == 4)
				{
					if (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Escape)
					{
						MenuConfigurationControls.cancel();
						return;
					}
					MenuConfigurationControls.bind(Event.current.keyCode);
					return;
				}
				else if (Event.current.type == null)
				{
					if (Event.current.button == 0)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse0);
						return;
					}
					if (Event.current.button == 1)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse1);
						return;
					}
					if (Event.current.button == 2)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse2);
						return;
					}
					if (Event.current.button == 3)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse3);
						return;
					}
					if (Event.current.button == 4)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse4);
						return;
					}
					if (Event.current.button == 5)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse5);
						return;
					}
					if (Event.current.button == 6)
					{
						MenuConfigurationControls.bind(KeyCode.Mouse6);
						return;
					}
				}
				else if (Event.current.shift)
				{
					MenuConfigurationControls.bind(KeyCode.LeftShift);
				}
			}
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x000D2466 File Offset: 0x000D0666
		private void Awake()
		{
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		// Token: 0x040019AE RID: 6574
		private static byte _binding;
	}
}
