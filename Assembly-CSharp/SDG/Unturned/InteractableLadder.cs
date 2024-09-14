using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000456 RID: 1110
	public class InteractableLadder : Interactable
	{
		// Token: 0x06002189 RID: 8585 RVA: 0x00081496 File Offset: 0x0007F696
		public override bool checkUseable()
		{
			return true;
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0008149C File Offset: 0x0007F69C
		public override void use()
		{
			if (this.CanClimb(Player.player))
			{
				Vector3 normalized = (PlayerInteract.hit.point - Player.player.look.aim.position).normalized;
				PlayerStance.SendClimbRequest.Invoke(Player.player.stance.GetNetId(), ENetReliability.Reliable, normalized);
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000814FD File Offset: 0x0007F6FD
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = "";
			color = Color.white;
			if (this.CanClimb(Player.player))
			{
				message = EPlayerMessage.CLIMB;
				return true;
			}
			message = EPlayerMessage.NONE;
			return false;
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x00081528 File Offset: 0x0007F728
		private bool CanClimb(Player player)
		{
			if (player == null || player.stance.stance == EPlayerStance.CLIMB)
			{
				return false;
			}
			if (!player.stance.canCurrentStanceTransitionToClimbing)
			{
				return false;
			}
			if (!player.stance.isAllowedToStartClimbing)
			{
				return false;
			}
			Vector3 normalized = (PlayerInteract.hit.point - player.look.aim.position).normalized;
			RaycastHit raycastHit;
			Physics.SphereCast(new Ray(player.look.aim.position, normalized), PlayerStance.RADIUS, out raycastHit, 4f, RayMasks.LADDER_INTERACT);
			if (raycastHit.collider == null || !raycastHit.collider.CompareTag("Ladder"))
			{
				return false;
			}
			RaycastHit raycastHit2;
			Physics.Raycast(new Ray(player.look.aim.position, normalized), out raycastHit2, 4f, RayMasks.LADDER_INTERACT);
			if (raycastHit2.collider == null || !raycastHit2.collider.CompareTag("Ladder"))
			{
				return false;
			}
			if (Mathf.Abs(Vector3.Dot(raycastHit2.normal, raycastHit2.collider.transform.up)) <= 0.9f)
			{
				return false;
			}
			if (Mathf.Abs(Vector3.Dot(Vector3.up, raycastHit2.collider.transform.up)) > 0.1f)
			{
				return false;
			}
			Vector3 vector = new Vector3(raycastHit2.collider.transform.position.x, raycastHit2.point.y - 0.5f - 0.5f - 0.1f, raycastHit2.collider.transform.position.z) + raycastHit2.normal * 0.65f;
			float num = PlayerMovement.HEIGHT_STAND + 0.1f + 0.5f;
			Vector3 end = vector + new Vector3(0f, num * 0.5f, 0f);
			RaycastHit raycastHit3;
			return !Physics.Linecast(raycastHit2.point, end, out raycastHit3, RayMasks.BLOCK_STANCE, QueryTriggerInteraction.Collide) && PlayerStance.hasHeightClearanceAtPosition(vector, num);
		}
	}
}
