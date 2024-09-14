using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E9 RID: 2025
	public class UseableOptic : Useable
	{
		// Token: 0x06004591 RID: 17809 RVA: 0x0019FA4E File Offset: 0x0019DC4E
		public override bool startSecondary()
		{
			if (base.channel.IsLocalPlayer && !this.isZoomed && base.player.look.perspective == EPlayerPerspective.FIRST)
			{
				this.isZoomed = true;
				this.startZoom();
				return true;
			}
			return false;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x0019FA87 File Offset: 0x0019DC87
		public override void stopSecondary()
		{
			if (base.channel.IsLocalPlayer && this.isZoomed)
			{
				this.isZoomed = false;
				this.stopZoom();
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0019FAAC File Offset: 0x0019DCAC
		private void startZoom()
		{
			base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.up;
			base.player.animator.viewmodelSwayMultiplier = 0f;
			base.player.look.enableZoom(((ItemOpticAsset)base.player.equipment.asset).zoom);
			base.player.look.shouldUseZoomFactorForSensitivity = true;
			PlayerUI.updateBinoculars(true);
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x0019FB24 File Offset: 0x0019DD24
		private void stopZoom()
		{
			base.player.animator.viewmodelCameraLocalPositionOffset = Vector3.zero;
			base.player.animator.viewmodelSwayMultiplier = 1f;
			base.player.look.disableZoom();
			base.player.look.shouldUseZoomFactorForSensitivity = false;
			PlayerUI.updateBinoculars(false);
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x0019FB82 File Offset: 0x0019DD82
		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			if (this.isZoomed && newPerspective == EPlayerPerspective.THIRD)
			{
				this.stopZoom();
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x0019FB98 File Offset: 0x0019DD98
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			if (base.channel.IsLocalPlayer)
			{
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x0019FBF4 File Offset: 0x0019DDF4
		public override void dequip()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (this.isZoomed)
				{
					this.stopZoom();
				}
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Remove(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
		}

		// Token: 0x04002EF5 RID: 12021
		private bool isZoomed;
	}
}
