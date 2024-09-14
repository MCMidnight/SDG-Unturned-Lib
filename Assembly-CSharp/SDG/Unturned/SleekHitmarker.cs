using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000715 RID: 1813
	public class SleekHitmarker : SleekWrapper
	{
		// Token: 0x06003BE9 RID: 15337 RVA: 0x001196E0 File Offset: 0x001178E0
		public void SetStyle(EPlayerHit hit)
		{
			Texture2D texture;
			Color color;
			switch (hit)
			{
			case EPlayerHit.NONE:
				return;
			case EPlayerHit.ENTITIY:
				texture = SleekHitmarker.hitEntityTexture;
				color = OptionsSettings.hitmarkerColor;
				break;
			case EPlayerHit.CRITICAL:
				texture = SleekHitmarker.hitEntityTexture;
				color = OptionsSettings.criticalHitmarkerColor;
				break;
			case EPlayerHit.BUILD:
				texture = SleekHitmarker.hitBuildTexture;
				color = OptionsSettings.hitmarkerColor;
				break;
			case EPlayerHit.GHOST:
				texture = SleekHitmarker.hitGhostTexture;
				color = OptionsSettings.hitmarkerColor;
				break;
			default:
				return;
			}
			this.neImage.Texture = texture;
			this.neImage.TintColor = color;
			this.seImage.Texture = texture;
			this.seImage.TintColor = color;
			this.swImage.Texture = texture;
			this.swImage.TintColor = color;
			this.nwImage.Texture = texture;
			this.nwImage.TintColor = color;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x001197CC File Offset: 0x001179CC
		public void PlayAnimation()
		{
			float num = Random.Range(-3f, 3f);
			this.neImage.RotationAngle = num;
			this.seImage.RotationAngle = 90f + num;
			this.swImage.RotationAngle = 180f + num;
			this.nwImage.RotationAngle = 270f + num;
			float f = (num - 45f) * 0.017453292f;
			float num2 = Mathf.Cos(f);
			float num3 = Mathf.Sin(f);
			float num4 = -num3;
			float num5 = num2;
			float num6 = -num5;
			float num7 = num4;
			float num8 = -num7;
			float num9 = num6;
			this.neImage.PositionScale_X = 0.5f + num2 * 0.1f;
			this.neImage.PositionScale_Y = 0.5f + num3 * 0.1f;
			this.seImage.PositionScale_X = 0.5f + num4 * 0.1f;
			this.seImage.PositionScale_Y = 0.5f + num5 * 0.1f;
			this.swImage.PositionScale_X = 0.5f + num6 * 0.1f;
			this.swImage.PositionScale_Y = 0.5f + num7 * 0.1f;
			this.nwImage.PositionScale_X = 0.5f + num8 * 0.1f;
			this.nwImage.PositionScale_Y = 0.5f + num9 * 0.1f;
			this.neImage.PositionOffset_X = -8f;
			this.neImage.PositionOffset_Y = -8f;
			this.seImage.PositionOffset_X = -8f;
			this.seImage.PositionOffset_Y = -8f;
			this.swImage.PositionOffset_X = -8f;
			this.swImage.PositionOffset_Y = -8f;
			this.nwImage.PositionOffset_X = -8f;
			this.nwImage.PositionOffset_Y = -8f;
			this.neImage.AnimatePositionScale(0.5f + num2 * 0.5f, 0.5f + num3 * 0.5f, 0, PlayerUI.HIT_TIME);
			this.seImage.AnimatePositionScale(0.5f + num4 * 0.5f, 0.5f + num5 * 0.5f, 0, PlayerUI.HIT_TIME);
			this.swImage.AnimatePositionScale(0.5f + num6 * 0.5f, 0.5f + num7 * 0.5f, 0, PlayerUI.HIT_TIME);
			this.nwImage.AnimatePositionScale(0.5f + num8 * 0.5f, 0.5f + num9 * 0.5f, 0, PlayerUI.HIT_TIME);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00119A54 File Offset: 0x00117C54
		public void ApplyClassicPositions()
		{
			this.neImage.RotationAngle = 0f;
			this.seImage.RotationAngle = 90f;
			this.swImage.RotationAngle = 180f;
			this.nwImage.RotationAngle = 270f;
			this.neImage.PositionScale_X = 0.5f;
			this.neImage.PositionScale_Y = 0.5f;
			this.seImage.PositionScale_X = 0.5f;
			this.seImage.PositionScale_Y = 0.5f;
			this.swImage.PositionScale_X = 0.5f;
			this.swImage.PositionScale_Y = 0.5f;
			this.nwImage.PositionScale_X = 0.5f;
			this.nwImage.PositionScale_Y = 0.5f;
			this.neImage.PositionOffset_X = 8f;
			this.neImage.PositionOffset_Y = -24f;
			this.seImage.PositionOffset_X = 8f;
			this.seImage.PositionOffset_Y = 8f;
			this.swImage.PositionOffset_X = -24f;
			this.swImage.PositionOffset_Y = 8f;
			this.nwImage.PositionOffset_X = -24f;
			this.nwImage.PositionOffset_Y = -24f;
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x00119BA4 File Offset: 0x00117DA4
		public SleekHitmarker()
		{
			this.neImage = Glazier.Get().CreateImage();
			this.neImage.PositionOffset_X = -8f;
			this.neImage.PositionOffset_Y = -8f;
			this.neImage.PositionScale_X = 0.5f;
			this.neImage.PositionScale_Y = 0.5f;
			this.neImage.SizeOffset_X = 16f;
			this.neImage.SizeOffset_Y = 16f;
			this.neImage.CanRotate = true;
			base.AddChild(this.neImage);
			this.seImage = Glazier.Get().CreateImage();
			this.seImage.PositionOffset_X = -8f;
			this.seImage.PositionOffset_Y = -8f;
			this.seImage.PositionScale_X = 0.5f;
			this.seImage.PositionScale_Y = 0.5f;
			this.seImage.SizeOffset_X = 16f;
			this.seImage.SizeOffset_Y = 16f;
			this.seImage.RotationAngle = 90f;
			this.seImage.CanRotate = true;
			base.AddChild(this.seImage);
			this.swImage = Glazier.Get().CreateImage();
			this.swImage.PositionOffset_X = -8f;
			this.swImage.PositionOffset_Y = -8f;
			this.swImage.PositionScale_X = 0.5f;
			this.swImage.PositionScale_Y = 0.5f;
			this.swImage.SizeOffset_X = 16f;
			this.swImage.SizeOffset_Y = 16f;
			this.swImage.RotationAngle = 180f;
			this.swImage.CanRotate = true;
			base.AddChild(this.swImage);
			this.nwImage = Glazier.Get().CreateImage();
			this.nwImage.PositionOffset_X = -8f;
			this.nwImage.PositionOffset_Y = -8f;
			this.nwImage.PositionScale_X = 0.5f;
			this.nwImage.PositionScale_Y = 0.5f;
			this.nwImage.SizeOffset_X = 16f;
			this.nwImage.SizeOffset_Y = 16f;
			this.nwImage.RotationAngle = 270f;
			this.nwImage.CanRotate = true;
			base.AddChild(this.nwImage);
		}

		// Token: 0x0400257A RID: 9594
		private const int IMAGE_SIZE = 16;

		// Token: 0x0400257B RID: 9595
		private const int HALF_IMAGE_SIZE = 8;

		// Token: 0x0400257C RID: 9596
		private ISleekImage neImage;

		// Token: 0x0400257D RID: 9597
		private ISleekImage seImage;

		// Token: 0x0400257E RID: 9598
		private ISleekImage swImage;

		// Token: 0x0400257F RID: 9599
		private ISleekImage nwImage;

		// Token: 0x04002580 RID: 9600
		private static StaticResourceRef<Texture2D> hitEntityTexture = new StaticResourceRef<Texture2D>("Bundles/Textures/Player/Icons/PlayerLife/Hit_Entity");

		// Token: 0x04002581 RID: 9601
		private static StaticResourceRef<Texture2D> hitBuildTexture = new StaticResourceRef<Texture2D>("Bundles/Textures/Player/Icons/PlayerLife/Hit_Build");

		// Token: 0x04002582 RID: 9602
		private static StaticResourceRef<Texture2D> hitGhostTexture = new StaticResourceRef<Texture2D>("Bundles/Textures/Player/Icons/PlayerLife/Hit_Ghost");
	}
}
