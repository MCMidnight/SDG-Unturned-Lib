using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003F8 RID: 1016
	public static class DecalSystem
	{
		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0006D79C File Offset: 0x0006B99C
		// (set) Token: 0x06001E0D RID: 7693 RVA: 0x0006D7A4 File Offset: 0x0006B9A4
		public static bool IsVisible
		{
			get
			{
				return DecalSystem._isVisible;
			}
			set
			{
				if (DecalSystem._isVisible != value)
				{
					DecalSystem._isVisible = value;
					ConvenientSavedata.get().write("Visibility_Decals", value);
					if (Level.isEditor)
					{
						foreach (Decal decal in DecalSystem.decalsDiffuse)
						{
							decal.UpdateEditorVisibility();
						}
					}
				}
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x0006D818 File Offset: 0x0006BA18
		public static HashSet<Decal> decalsDiffuse
		{
			get
			{
				return DecalSystem._decalsDiffuse;
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0006D81F File Offset: 0x0006BA1F
		public static void add(Decal decal)
		{
			if (decal == null)
			{
				return;
			}
			if (decal.material == null)
			{
				return;
			}
			DecalSystem.remove(decal);
			if (decal.type == EDecalType.DIFFUSE)
			{
				DecalSystem.decalsDiffuse.Add(decal);
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x0006D854 File Offset: 0x0006BA54
		public static void remove(Decal decal)
		{
			if (decal == null)
			{
				return;
			}
			if (decal.type == EDecalType.DIFFUSE)
			{
				DecalSystem.decalsDiffuse.Remove(decal);
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x0006D874 File Offset: 0x0006BA74
		static DecalSystem()
		{
			bool isVisible;
			if (ConvenientSavedata.get().read("Visibility_Decals", out isVisible))
			{
				DecalSystem._isVisible = isVisible;
			}
			else
			{
				DecalSystem._isVisible = true;
			}
			TimeUtility.updated += DecalSystem.OnUpdateGizmos;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x0006D8C0 File Offset: 0x0006BAC0
		private static void OnUpdateGizmos()
		{
			if (!DecalSystem._isVisible || !Level.isEditor)
			{
				return;
			}
			Camera instance = MainCamera.instance;
			if (instance == null)
			{
				return;
			}
			RuntimeGizmos runtimeGizmos = RuntimeGizmos.Get();
			float num = 128f + GraphicsSettings.normalizedDrawDistance * 128f;
			foreach (Decal decal in DecalSystem.decalsDiffuse)
			{
				if (!(decal.material == null))
				{
					float num2 = num * decal.lodBias;
					float num3 = num2 * num2;
					if ((decal.transform.position - instance.transform.position).sqrMagnitude <= num3)
					{
						Color color = decal.isSelected ? Color.yellow : Color.red;
						runtimeGizmos.Box(decal.transform.localToWorldMatrix, Vector3.one, color, 0f, EGizmoLayer.World);
					}
				}
			}
		}

		// Token: 0x04000E67 RID: 3687
		private static bool _isVisible;

		// Token: 0x04000E68 RID: 3688
		private static HashSet<Decal> _decalsDiffuse = new HashSet<Decal>();
	}
}
