﻿using System;

namespace SDG.Unturned
{
	/// <summary>
	/// Almost every menu has a container element for its contents which spans the entire screen. This element is then
	/// animated into and out of view. In the IMGUI implementation this was fine because containers off-screen were not
	/// processed, but with uGUI they were still considered active. To solve the uGUI performance overhead this class
	/// was introduced to disable visibility after animating out of view.
	/// </summary>
	// Token: 0x02000714 RID: 1812
	public class SleekFullscreenBox : SleekWrapper
	{
		// Token: 0x06003BE5 RID: 15333 RVA: 0x00119647 File Offset: 0x00117847
		public void AnimateIntoView()
		{
			base.IsVisible = true;
			base.AnimatePositionScale(0f, 0f, 1, 20f);
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00119666 File Offset: 0x00117866
		public void AnimateOutOfView(float x, float y)
		{
			base.IsVisible = true;
			base.AnimatePositionScale(x, y, 1, 20f);
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x00119680 File Offset: 0x00117880
		public override void OnUpdate()
		{
			if (!base.IsAnimatingTransform)
			{
				float positionScale_X = base.PositionScale_X;
				float positionScale_Y = base.PositionScale_Y;
				if (positionScale_X > 0.999f || positionScale_Y > 0.999f || positionScale_X + 1f < 0.001f || positionScale_Y + 1f < 0.001f)
				{
					base.IsVisible = false;
				}
			}
		}
	}
}
