using System;

namespace SDG.Framework.Foliage
{
	// Token: 0x020000F3 RID: 243
	public class FoliageSettings
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00016BDC File Offset: 0x00014DDC
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00016BE3 File Offset: 0x00014DE3
		public static bool enabled
		{
			get
			{
				return FoliageSettings._enabled;
			}
			set
			{
				FoliageSettings._enabled = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00016BEB File Offset: 0x00014DEB
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00016BF2 File Offset: 0x00014DF2
		public static bool drawFocus
		{
			get
			{
				return FoliageSettings._drawFocus;
			}
			set
			{
				FoliageSettings._drawFocus = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00016BFA File Offset: 0x00014DFA
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x00016C01 File Offset: 0x00014E01
		public static int drawDistance
		{
			get
			{
				return FoliageSettings._drawDistance;
			}
			set
			{
				FoliageSettings._drawDistance = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00016C09 File Offset: 0x00014E09
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00016C10 File Offset: 0x00014E10
		public static int drawFocusDistance
		{
			get
			{
				return FoliageSettings._drawFocusDistance;
			}
			set
			{
				FoliageSettings._drawFocusDistance = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00016C18 File Offset: 0x00014E18
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x00016C1F File Offset: 0x00014E1F
		public static float instanceDensity
		{
			get
			{
				return FoliageSettings._instanceDensity;
			}
			set
			{
				FoliageSettings._instanceDensity = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00016C27 File Offset: 0x00014E27
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x00016C2E File Offset: 0x00014E2E
		public static bool forceInstancingOff
		{
			get
			{
				return FoliageSettings._forceInstancingOff;
			}
			set
			{
				FoliageSettings._forceInstancingOff = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00016C36 File Offset: 0x00014E36
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x00016C3D File Offset: 0x00014E3D
		public static float focusDistance
		{
			get
			{
				return FoliageSettings._focusDistance;
			}
			set
			{
				FoliageSettings._focusDistance = value;
			}
		}

		// Token: 0x04000239 RID: 569
		private static bool _enabled;

		// Token: 0x0400023A RID: 570
		private static bool _drawFocus;

		// Token: 0x0400023B RID: 571
		private static int _drawDistance;

		// Token: 0x0400023C RID: 572
		private static int _drawFocusDistance;

		// Token: 0x0400023D RID: 573
		private static float _instanceDensity;

		// Token: 0x0400023E RID: 574
		private static bool _forceInstancingOff;

		// Token: 0x0400023F RID: 575
		private static float _focusDistance;
	}
}
