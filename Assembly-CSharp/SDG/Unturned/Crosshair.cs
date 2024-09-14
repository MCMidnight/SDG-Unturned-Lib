using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020006FA RID: 1786
	public class Crosshair : SleekWrapper
	{
		// Token: 0x06003B2C RID: 15148 RVA: 0x00114842 File Offset: 0x00112A42
		public void SetGameWantsCenterDotVisible(bool isVisible)
		{
			this.gameWantsCenterDotVisible = isVisible;
			this.centerDotImage.IsVisible = (this.gameWantsCenterDotVisible && this.pluginAllowsCenterDotVisible);
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00114867 File Offset: 0x00112A67
		public void SetPluginAllowsCenterDotVisible(bool isVisible)
		{
			this.pluginAllowsCenterDotVisible = isVisible;
			this.centerDotImage.IsVisible = (this.gameWantsCenterDotVisible && this.pluginAllowsCenterDotVisible);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x0011488C File Offset: 0x00112A8C
		public void SetDirectionalArrowsVisible(bool isVisible)
		{
			this.isGunCrosshairVisible = isVisible;
			this.crosshairLeftImage.IsVisible = isVisible;
			this.crosshairRightImage.IsVisible = isVisible;
			this.crosshairDownImage.IsVisible = isVisible;
			this.crosshairUpImage.IsVisible = isVisible;
			this.isInterpolatedSpreadValid &= this.isGunCrosshairVisible;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x001148E4 File Offset: 0x00112AE4
		public void SynchronizeCustomColors()
		{
			Color crosshairColor = OptionsSettings.crosshairColor;
			this.centerDotImage.TintColor = crosshairColor;
			this.crosshairLeftImage.TintColor = crosshairColor;
			this.crosshairRightImage.TintColor = crosshairColor;
			this.crosshairDownImage.TintColor = crosshairColor;
			this.crosshairUpImage.TintColor = crosshairColor;
		}

		// Token: 0x06003B30 RID: 15152 RVA: 0x0011494C File Offset: 0x00112B4C
		public void SynchronizeImages()
		{
			if (OptionsSettings.crosshairShape == ECrosshairShape.Classic)
			{
				this.crosshairLeftImage.SizeOffset_X = 8f;
				this.crosshairLeftImage.Texture = this.icons.load<Texture>("Crosshair_Left_Square");
				this.crosshairRightImage.SizeOffset_X = 8f;
				this.crosshairRightImage.Texture = this.icons.load<Texture>("Crosshair_Right_Square");
				this.crosshairUpImage.SizeOffset_Y = 8f;
				this.crosshairUpImage.Texture = this.icons.load<Texture>("Crosshair_Up_Square");
				this.crosshairDownImage.SizeOffset_Y = 8f;
				this.crosshairDownImage.Texture = this.icons.load<Texture>("Crosshair_Down_Square");
			}
			else
			{
				this.crosshairLeftImage.SizeOffset_X = 16f;
				this.crosshairLeftImage.Texture = this.icons.load<Texture>("Crosshair_Left");
				this.crosshairRightImage.SizeOffset_X = 16f;
				this.crosshairRightImage.Texture = this.icons.load<Texture>("Crosshair_Right");
				this.crosshairUpImage.SizeOffset_Y = 16f;
				this.crosshairUpImage.Texture = this.icons.load<Texture>("Crosshair_Up");
				this.crosshairDownImage.SizeOffset_Y = 16f;
				this.crosshairDownImage.Texture = this.icons.load<Texture>("Crosshair_Down");
			}
			this.crosshairLeftImage.PositionOffset_X = -this.crosshairLeftImage.SizeOffset_X;
			this.crosshairUpImage.PositionOffset_Y = -this.crosshairUpImage.SizeOffset_Y;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x00114AF0 File Offset: 0x00112CF0
		public override void OnUpdate()
		{
			if (!this.isGunCrosshairVisible)
			{
				this.isInterpolatedSpreadValid = false;
				return;
			}
			UseableGun useableGun = Player.player.equipment.useable as UseableGun;
			if (useableGun == null)
			{
				this.isInterpolatedSpreadValid = false;
				return;
			}
			Camera instance = MainCamera.instance;
			if (instance == null)
			{
				this.isInterpolatedSpreadValid = false;
				return;
			}
			float fieldOfView = instance.fieldOfView;
			float num = 0.017453292f * fieldOfView * 0.5f;
			if (num < 0.001f)
			{
				this.isInterpolatedSpreadValid = false;
				return;
			}
			Vector2 vector2;
			if (Player.player.look.perspective == EPlayerPerspective.FIRST)
			{
				Quaternion rotation = Player.player.look.aim.rotation;
				Quaternion rhs = Quaternion.Euler(Player.player.animator.recoilViewmodelCameraRotation.currentPosition);
				Vector3 b = rotation * rhs * Vector3.forward;
				Vector2 vector = instance.WorldToViewportPoint(instance.transform.position + b);
				vector2 = base.ViewportToNormalizedPosition(vector);
				vector2.x += base.Parent.PositionScale_X;
				vector2.y += base.Parent.PositionScale_Y;
			}
			else
			{
				vector2 = new Vector2(0.5f, 0.5f);
			}
			float b2 = useableGun.CalculateSpreadAngleRadians();
			if (this.isInterpolatedSpreadValid)
			{
				this.interpolatedSpread = Mathf.Lerp(this.interpolatedSpread, b2, Time.deltaTime * 16f);
			}
			else
			{
				this.interpolatedSpread = b2;
				this.isInterpolatedSpreadValid = true;
			}
			float num2 = Mathf.Tan(num);
			float num3 = num2 * instance.aspect;
			float num4 = Mathf.Tan(this.interpolatedSpread);
			float num5 = num4 / num3 * 0.5f;
			float num6 = num4 / num2 * 0.5f;
			if (OptionsSettings.useStaticCrosshair)
			{
				num5 = Mathf.Lerp(0.0025f, 0.05f, OptionsSettings.staticCrosshairSize);
				num6 = num5 * instance.aspect;
			}
			this.crosshairLeftImage.PositionScale_X = vector2.x - num5;
			this.crosshairLeftImage.PositionScale_Y = vector2.y;
			this.crosshairRightImage.PositionScale_X = vector2.x + num5;
			this.crosshairRightImage.PositionScale_Y = vector2.y;
			this.crosshairUpImage.PositionScale_X = vector2.x;
			this.crosshairUpImage.PositionScale_Y = vector2.y - num6;
			this.crosshairDownImage.PositionScale_X = vector2.x;
			this.crosshairDownImage.PositionScale_Y = vector2.y + num6;
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x00114D68 File Offset: 0x00112F68
		public Crosshair(Bundle icons)
		{
			this.icons = icons;
			this.centerDotImage = Glazier.Get().CreateImage();
			this.centerDotImage.PositionOffset_X = -4f;
			this.centerDotImage.PositionOffset_Y = -4f;
			this.centerDotImage.PositionScale_X = 0.5f;
			this.centerDotImage.PositionScale_Y = 0.5f;
			this.centerDotImage.SizeOffset_X = 8f;
			this.centerDotImage.SizeOffset_Y = 8f;
			this.centerDotImage.Texture = icons.load<Texture>("Dot");
			base.AddChild(this.centerDotImage);
			this.gameWantsCenterDotVisible = true;
			this.crosshairLeftImage = Glazier.Get().CreateImage();
			this.crosshairLeftImage.PositionOffset_Y = -4f;
			this.crosshairLeftImage.PositionScale_X = 0.5f;
			this.crosshairLeftImage.PositionScale_Y = 0.5f;
			this.crosshairLeftImage.SizeOffset_Y = 8f;
			base.AddChild(this.crosshairLeftImage);
			this.crosshairLeftImage.IsVisible = false;
			this.crosshairRightImage = Glazier.Get().CreateImage();
			this.crosshairRightImage.PositionOffset_Y = -4f;
			this.crosshairRightImage.PositionScale_X = 0.5f;
			this.crosshairRightImage.PositionScale_Y = 0.5f;
			this.crosshairRightImage.SizeOffset_Y = 8f;
			base.AddChild(this.crosshairRightImage);
			this.crosshairRightImage.IsVisible = false;
			this.crosshairDownImage = Glazier.Get().CreateImage();
			this.crosshairDownImage.PositionOffset_X = -4f;
			this.crosshairDownImage.PositionScale_X = 0.5f;
			this.crosshairDownImage.PositionScale_Y = 0.5f;
			this.crosshairDownImage.SizeOffset_X = 8f;
			base.AddChild(this.crosshairDownImage);
			this.crosshairDownImage.IsVisible = false;
			this.crosshairUpImage = Glazier.Get().CreateImage();
			this.crosshairUpImage.PositionOffset_X = -4f;
			this.crosshairUpImage.PositionScale_X = 0.5f;
			this.crosshairUpImage.PositionScale_Y = 0.5f;
			this.crosshairUpImage.SizeOffset_X = 8f;
			base.AddChild(this.crosshairUpImage);
			this.crosshairUpImage.IsVisible = false;
			this.SynchronizeCustomColors();
			this.SynchronizeImages();
		}

		// Token: 0x04002513 RID: 9491
		private bool gameWantsCenterDotVisible;

		// Token: 0x04002514 RID: 9492
		private bool pluginAllowsCenterDotVisible;

		// Token: 0x04002515 RID: 9493
		private bool isGunCrosshairVisible;

		// Token: 0x04002516 RID: 9494
		private Bundle icons;

		// Token: 0x04002517 RID: 9495
		private ISleekImage crosshairLeftImage;

		// Token: 0x04002518 RID: 9496
		private ISleekImage crosshairRightImage;

		// Token: 0x04002519 RID: 9497
		private ISleekImage crosshairDownImage;

		// Token: 0x0400251A RID: 9498
		private ISleekImage crosshairUpImage;

		// Token: 0x0400251B RID: 9499
		private ISleekImage centerDotImage;

		/// <summary>
		/// Slightly interpolated copy of actual spread angle to smooth out sharp changes like crouch/prone.
		/// </summary>
		// Token: 0x0400251C RID: 9500
		private float interpolatedSpread;

		/// <summary>
		/// Allows interpolatedSpread to snap to target value when crosshair becomes visible.
		/// </summary>
		// Token: 0x0400251D RID: 9501
		private bool isInterpolatedSpreadValid;
	}
}
