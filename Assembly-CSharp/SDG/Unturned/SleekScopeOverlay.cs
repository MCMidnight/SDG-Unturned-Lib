using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200072B RID: 1835
	public class SleekScopeOverlay : SleekWrapper
	{
		/// <summary>
		/// Calculate angle in radians the player would need to offset their aim upward
		/// to hit a target a certain distance away.
		/// </summary>
		// Token: 0x06003C7E RID: 15486 RVA: 0x0011DB51 File Offset: 0x0011BD51
		internal static float CalcAngle(float speed, float distance, float gravity)
		{
			return Mathf.Asin(distance * gravity / (speed * speed)) * 0.5f;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x0011DB68 File Offset: 0x0011BD68
		public override void OnUpdate()
		{
			base.OnUpdate();
			UseableGun useableGun = Player.player.equipment.useable as UseableGun;
			if (useableGun == null || useableGun.equippedGunAsset.projectile != null || useableGun.firstAttachments == null || useableGun.firstAttachments.sightAsset == null || useableGun.firstAttachments.sightAsset.distanceMarkers == null || useableGun.firstAttachments.sightAsset.distanceMarkers.Count < 1)
			{
				this.DisableDistanceMarkers();
				return;
			}
			if (Player.player == null || Player.player.look == null || Player.player.look.mainCameraZoomFactor <= 0f)
			{
				this.DisableDistanceMarkers();
				return;
			}
			float num = OptionsSettings.GetZoomBaseFieldOfView() / Player.player.look.mainCameraZoomFactor;
			float num2 = 0.017453292f * num;
			if (num2 < 0.001f)
			{
				this.DisableDistanceMarkers();
				return;
			}
			if (this.currentSightAsset != useableGun.firstAttachments.sightAsset)
			{
				this.EnableDistanceMarkersForSight(useableGun.firstAttachments.sightAsset);
			}
			float muzzleVelocity = useableGun.equippedGunAsset.muzzleVelocity;
			float gravity = useableGun.CalculateBulletGravity();
			foreach (SleekScopeOverlay.DistanceMarker distanceMarker in this.distanceMarkers)
			{
				if (!distanceMarker.isEnabled)
				{
					break;
				}
				float num3 = Mathf.Abs(SleekScopeOverlay.CalcAngle(muzzleVelocity, distanceMarker.distance, gravity)) / num2;
				distanceMarker.horizontalLine.PositionScale_Y = 0.5f + num3;
				distanceMarker.distanceLabel.PositionScale_Y = distanceMarker.horizontalLine.PositionScale_Y;
				distanceMarker.SetIsVisible(num3 > 0.01f && num3 < 0.5f);
			}
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0011DD48 File Offset: 0x0011BF48
		public override void OnDestroy()
		{
			base.OnDestroy();
			OptionsSettings.OnUnitSystemChanged -= new Action(this.SyncMarkerLabels);
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0011DD64 File Offset: 0x0011BF64
		public SleekScopeOverlay()
		{
			this.scopeFrame = Glazier.Get().CreateConstraintFrame();
			this.scopeFrame.SizeScale_X = 1f;
			this.scopeFrame.SizeScale_Y = 1f;
			this.scopeFrame.Constraint = 1;
			base.AddChild(this.scopeFrame);
			this.scopeOverlay = Glazier.Get().CreateImage((Texture2D)Resources.Load("Overlay/Scope"));
			this.scopeOverlay.PositionScale_X = 0.1f;
			this.scopeOverlay.PositionScale_Y = 0.1f;
			this.scopeOverlay.SizeScale_X = 0.8f;
			this.scopeOverlay.SizeScale_Y = 0.8f;
			this.scopeFrame.AddChild(this.scopeOverlay);
			this.scopeLeftOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			this.scopeLeftOverlay.PositionOffset_X = 1f;
			this.scopeLeftOverlay.PositionScale_X = -10f;
			this.scopeLeftOverlay.SizeScale_X = 10f;
			this.scopeLeftOverlay.SizeScale_Y = 1f;
			this.scopeLeftOverlay.TintColor = Color.black;
			this.scopeOverlay.AddChild(this.scopeLeftOverlay);
			this.scopeRightOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			this.scopeRightOverlay.PositionOffset_X = -1f;
			this.scopeRightOverlay.PositionScale_X = 1f;
			this.scopeRightOverlay.SizeScale_X = 10f;
			this.scopeRightOverlay.SizeScale_Y = 1f;
			this.scopeRightOverlay.TintColor = Color.black;
			this.scopeOverlay.AddChild(this.scopeRightOverlay);
			this.scopeUpOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			this.scopeUpOverlay.PositionOffset_Y = 1f;
			this.scopeUpOverlay.PositionScale_X = -10f;
			this.scopeUpOverlay.PositionScale_Y = -10f;
			this.scopeUpOverlay.SizeScale_X = 21f;
			this.scopeUpOverlay.SizeScale_Y = 10f;
			this.scopeUpOverlay.TintColor = Color.black;
			this.scopeOverlay.AddChild(this.scopeUpOverlay);
			this.scopeDownOverlay = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
			this.scopeDownOverlay.PositionOffset_Y = -1f;
			this.scopeDownOverlay.PositionScale_X = -10f;
			this.scopeDownOverlay.PositionScale_Y = 1f;
			this.scopeDownOverlay.SizeScale_X = 21f;
			this.scopeDownOverlay.SizeScale_Y = 10f;
			this.scopeDownOverlay.TintColor = Color.black;
			this.scopeOverlay.AddChild(this.scopeDownOverlay);
			this.scopeImage = Glazier.Get().CreateImage();
			this.scopeImage.SizeScale_X = 1f;
			this.scopeImage.SizeScale_Y = 1f;
			this.scopeOverlay.AddChild(this.scopeImage);
			OptionsSettings.OnUnitSystemChanged += new Action(this.SyncMarkerLabels);
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x0011E0AC File Offset: 0x0011C2AC
		private void SyncMarkerLabels()
		{
			foreach (SleekScopeOverlay.DistanceMarker distanceMarker in this.distanceMarkers)
			{
				if (!distanceMarker.isEnabled)
				{
					break;
				}
				if (OptionsSettings.metric)
				{
					distanceMarker.distanceLabel.Text = string.Format("{0} m", distanceMarker.distance);
				}
				else
				{
					distanceMarker.distanceLabel.Text = string.Format("{0} yd", Mathf.RoundToInt(MeasurementTool.MtoYd(distanceMarker.distance)));
				}
			}
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x0011E158 File Offset: 0x0011C358
		private void DisableDistanceMarkers()
		{
			this.currentSightAsset = null;
			for (int i = 0; i < this.distanceMarkers.Count; i++)
			{
				SleekScopeOverlay.DistanceMarker distanceMarker = this.distanceMarkers[i];
				if (distanceMarker.isEnabled)
				{
					distanceMarker.isEnabled = false;
					distanceMarker.SetIsVisible(false);
				}
			}
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x0011E1A8 File Offset: 0x0011C3A8
		private void EnableDistanceMarkersForSight(ItemSightAsset newSightAsset)
		{
			this.currentSightAsset = newSightAsset;
			for (int i = 0; i < this.currentSightAsset.distanceMarkers.Count; i++)
			{
				ItemSightAsset.DistanceMarker distanceMarker = this.currentSightAsset.distanceMarkers[i];
				SleekScopeOverlay.DistanceMarker distanceMarker2;
				if (i < this.distanceMarkers.Count)
				{
					distanceMarker2 = this.distanceMarkers[i];
				}
				else
				{
					distanceMarker2 = new SleekScopeOverlay.DistanceMarker();
					distanceMarker2.isVisible = true;
					distanceMarker2.horizontalLine = Glazier.Get().CreateImage(GlazierResources.PixelTexture);
					distanceMarker2.horizontalLine.SizeOffset_Y = 1f;
					this.scopeFrame.AddChild(distanceMarker2.horizontalLine);
					distanceMarker2.distanceLabel = Glazier.Get().CreateLabel();
					distanceMarker2.distanceLabel.PositionOffset_Y = -25f;
					distanceMarker2.distanceLabel.SizeOffset_X = 200f;
					distanceMarker2.distanceLabel.SizeOffset_Y = 50f;
					distanceMarker2.distanceLabel.FontStyle = 1;
					this.scopeFrame.AddChild(distanceMarker2.distanceLabel);
					this.distanceMarkers.Add(distanceMarker2);
				}
				distanceMarker2.horizontalLine.SizeScale_X = distanceMarker.lineWidth;
				if (distanceMarker.side == ItemSightAsset.DistanceMarker.ESide.Right)
				{
					distanceMarker2.horizontalLine.PositionScale_X = 0.5f + distanceMarker.lineOffset;
					distanceMarker2.distanceLabel.PositionScale_X = 0.5f + distanceMarker.lineOffset + distanceMarker.lineWidth;
					distanceMarker2.distanceLabel.PositionOffset_X = 0f;
					distanceMarker2.distanceLabel.TextAlignment = 3;
				}
				else
				{
					distanceMarker2.horizontalLine.PositionScale_X = 0.5f - distanceMarker.lineOffset - distanceMarker.lineWidth;
					distanceMarker2.distanceLabel.PositionScale_X = 0.5f - distanceMarker.lineOffset - distanceMarker.lineWidth;
					distanceMarker2.distanceLabel.PositionOffset_X = -distanceMarker2.distanceLabel.SizeOffset_X;
					distanceMarker2.distanceLabel.TextAlignment = 5;
				}
				distanceMarker2.distance = distanceMarker.distance;
				distanceMarker2.isEnabled = true;
				distanceMarker2.hasLabel = distanceMarker.hasLabel;
				distanceMarker2.horizontalLine.TintColor = distanceMarker.color;
				distanceMarker2.distanceLabel.TextColor = distanceMarker.color;
				distanceMarker2.SyncIsVisible();
			}
			for (int j = this.currentSightAsset.distanceMarkers.Count; j < this.distanceMarkers.Count; j++)
			{
				SleekScopeOverlay.DistanceMarker distanceMarker3 = this.distanceMarkers[j];
				if (distanceMarker3.isEnabled)
				{
					distanceMarker3.isEnabled = false;
					distanceMarker3.SetIsVisible(false);
				}
			}
			this.SyncMarkerLabels();
		}

		// Token: 0x040025D0 RID: 9680
		private ISleekConstraintFrame scopeFrame;

		// Token: 0x040025D1 RID: 9681
		public ISleekImage scopeImage;

		// Token: 0x040025D2 RID: 9682
		private ISleekImage scopeOverlay;

		// Token: 0x040025D3 RID: 9683
		private ISleekImage scopeLeftOverlay;

		// Token: 0x040025D4 RID: 9684
		private ISleekImage scopeRightOverlay;

		// Token: 0x040025D5 RID: 9685
		private ISleekImage scopeUpOverlay;

		// Token: 0x040025D6 RID: 9686
		private ISleekImage scopeDownOverlay;

		// Token: 0x040025D7 RID: 9687
		private ItemSightAsset currentSightAsset;

		// Token: 0x040025D8 RID: 9688
		private List<SleekScopeOverlay.DistanceMarker> distanceMarkers = new List<SleekScopeOverlay.DistanceMarker>();

		// Token: 0x020009F2 RID: 2546
		private class DistanceMarker
		{
			/// <summary>
			/// Separate from isEnabled to hide markers when they are outside the scope.
			/// </summary>
			// Token: 0x06004CF2 RID: 19698 RVA: 0x001B833C File Offset: 0x001B653C
			public void SetIsVisible(bool isVisible)
			{
				if (this.isVisible != isVisible)
				{
					this.isVisible = isVisible;
					this.SyncIsVisible();
				}
			}

			/// <summary>
			/// Used to sync hasLabel visibility.
			/// </summary>
			// Token: 0x06004CF3 RID: 19699 RVA: 0x001B8354 File Offset: 0x001B6554
			public void SyncIsVisible()
			{
				this.horizontalLine.IsVisible = this.isVisible;
				this.distanceLabel.IsVisible = (this.isVisible && this.hasLabel);
			}

			// Token: 0x040034BE RID: 13502
			public bool isEnabled;

			// Token: 0x040034BF RID: 13503
			public bool isVisible;

			// Token: 0x040034C0 RID: 13504
			public float distance;

			// Token: 0x040034C1 RID: 13505
			public ISleekImage horizontalLine;

			// Token: 0x040034C2 RID: 13506
			public ISleekLabel distanceLabel;

			// Token: 0x040034C3 RID: 13507
			public bool hasLabel;
		}
	}
}
