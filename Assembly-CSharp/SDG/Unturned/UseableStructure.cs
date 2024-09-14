using System;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007EB RID: 2027
	public class UseableStructure : Useable
	{
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x001A06E0 File Offset: 0x0019E8E0
		public ItemStructureAsset equippedStructureAsset
		{
			get
			{
				return base.player.equipment.asset as ItemStructureAsset;
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x001A06F7 File Offset: 0x0019E8F7
		[Obsolete]
		public void askStructure(CSteamID steamID, Vector3 newPoint, float newAngle)
		{
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x001A06FC File Offset: 0x0019E8FC
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10)]
		public void ReceiveBuildStructure(in ServerInvocationContext context, Vector3 newPoint, float newAngle)
		{
			if (this.hasServerReceivedBuildRequest)
			{
				return;
			}
			this.hasServerReceivedBuildRequest = true;
			if ((newPoint - base.player.look.aim.position).sqrMagnitude < 256f)
			{
				this.serverPlacementPosition = newPoint;
				this.serverPlacementYaw = newAngle;
				if (!UseableHousingUtils.IsPendingPositionValid(base.player, this.serverPlacementPosition))
				{
					this.isServerBuildRequestInitiallyApproved = false;
					return;
				}
				string text = null;
				if (UseableHousingUtils.ValidatePendingPlacement(this.equippedStructureAsset, ref this.serverPlacementPosition, this.serverPlacementYaw, ref text) == EHousingPlacementResult.Success)
				{
					this.isServerBuildRequestInitiallyApproved = true;
					return;
				}
				this.isServerBuildRequestInitiallyApproved = false;
			}
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x001A0798 File Offset: 0x0019E998
		[Obsolete]
		public void askConstruct(CSteamID steamID)
		{
			this.ReceivePlayConstruct();
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x001A07A0 File Offset: 0x0019E9A0
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askConstruct")]
		public void ReceivePlayConstruct()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.PlayUseAnimation();
			}
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x001A07BC File Offset: 0x0019E9BC
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.player.movement.getVehicle() != null)
			{
				return false;
			}
			if (this.isServerBuildRequestInitiallyApproved)
			{
				if (base.channel.IsLocalPlayer)
				{
					UseableStructure.SendBuildStructure.Invoke(base.GetNetId(), ENetReliability.Reliable, this.pendingPlacementPosition, this.pendingPlacementYaw + this.customRotationOffset);
				}
				base.player.equipment.isBusy = true;
				this.PlayUseAnimation();
				if (Provider.isServer)
				{
					UseableStructure.SendPlayConstruct.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
			}
			else if (this.hasServerReceivedBuildRequest)
			{
				base.player.equipment.dequip();
			}
			return true;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x001A0888 File Offset: 0x0019EA88
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.equippedStructureAsset.construct == EConstruct.FLOOR_POLY || this.equippedStructureAsset.construct == EConstruct.ROOF_POLY)
				{
					return false;
				}
				float num;
				if (this.equippedStructureAsset.construct == EConstruct.FLOOR || this.equippedStructureAsset.construct == EConstruct.ROOF)
				{
					num = 90f;
				}
				else if (this.equippedStructureAsset.construct == EConstruct.RAMPART || this.equippedStructureAsset.construct == EConstruct.WALL)
				{
					num = 180f;
				}
				else
				{
					num = 30f;
				}
				if (InputEx.GetKey(KeyCode.LeftShift))
				{
					num *= -1f;
				}
				this.customRotationOffset += num;
			}
			return true;
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x001A0948 File Offset: 0x0019EB48
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useAnimationDuration = base.player.animator.GetAnimationLength("Use", true);
			if (base.channel.IsLocalPlayer)
			{
				this.isPlacementPreviewValid = false;
				this.placementPreviewTransform = UseableHousingUtils.InstantiatePlacementPreview(this.equippedStructureAsset);
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x001A09AC File Offset: 0x0019EBAC
		public override void dequip()
		{
			if (base.channel.IsLocalPlayer && this.placementPreviewTransform != null)
			{
				Object.Destroy(this.placementPreviewTransform.gameObject);
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x001A09DC File Offset: 0x0019EBDC
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUseAnimationPlaying && this.HasFinishedUseAnimation)
			{
				base.player.equipment.isBusy = false;
				if (Provider.isServer)
				{
					if (!UseableHousingUtils.IsPendingPositionValid(base.player, this.serverPlacementPosition))
					{
						base.player.equipment.dequip();
						return;
					}
					string empty = string.Empty;
					if (UseableHousingUtils.ValidatePendingPlacement(this.equippedStructureAsset, ref this.serverPlacementPosition, this.serverPlacementYaw, ref empty) != EHousingPlacementResult.Success)
					{
						base.player.equipment.dequip();
						return;
					}
					ItemStructureAsset equippedStructureAsset = this.equippedStructureAsset;
					bool flag = false;
					if (equippedStructureAsset != null)
					{
						base.player.sendStat(EPlayerStat.FOUND_BUILDABLES);
						flag = StructureManager.dropStructure(new Structure(equippedStructureAsset, equippedStructureAsset.health), this.serverPlacementPosition, 0f, this.serverPlacementYaw, 0f, base.channel.owner.playerID.steamID.m_SteamID, base.player.quests.groupID.m_SteamID);
					}
					if (flag)
					{
						base.player.equipment.use();
						return;
					}
					base.player.equipment.dequip();
				}
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x001A0B04 File Offset: 0x0019ED04
		public override void tick()
		{
			if (this.isWaitingForSoundTrigger && this.HasReachedSoundTrigger)
			{
				this.isWaitingForSoundTrigger = false;
				if (Provider.isServer)
				{
					AlertTool.alert(base.transform.position, 8f);
				}
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.placementPreviewTransform == null)
				{
					return;
				}
				if (!this.isUseAnimationPlaying)
				{
					bool flag = this.UpdatePendingPlacement();
					if (this.isPlacementPreviewValid != flag)
					{
						this.isPlacementPreviewValid = flag;
						HighlighterTool.help(this.placementPreviewTransform, this.isPlacementPreviewValid);
					}
				}
				float num = Glazier.Get().ShouldGameProcessInput ? Input.GetAxis("mouse_z") : 0f;
				this.foundationPositionOffset = Mathf.Clamp(this.foundationPositionOffset + num * 0.05f, -1f, 1f);
				this.animatedRotationOffset = Mathf.Lerp(this.animatedRotationOffset, this.customRotationOffset, 8f * Time.deltaTime);
				this.placementPreviewTransform.position = this.pendingPlacementPosition;
				this.placementPreviewTransform.rotation = Quaternion.Euler(-90f, this.pendingPlacementYaw + this.animatedRotationOffset, 0f);
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x001A0C2E File Offset: 0x0019EE2E
		private void PlayUseAnimation()
		{
			this.useAnimationStartTime = Time.timeAsDouble;
			this.isUseAnimationPlaying = true;
			this.isWaitingForSoundTrigger = true;
			base.player.animator.play("Use", false);
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x001A0C60 File Offset: 0x0019EE60
		private bool UpdatePendingPlacement()
		{
			return UseableHousingUtils.FindPlacement(this.equippedStructureAsset, base.player, this.customRotationOffset, this.foundationPositionOffset, out this.pendingPlacementPosition, out this.pendingPlacementYaw) && UseableHousingUtils.IsPendingPositionValid(base.player, this.pendingPlacementPosition);
		}

		/// <summary>
		/// Whether enough time has passed for "Use" animation to finish.
		/// </summary>
		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x001A0CB0 File Offset: 0x0019EEB0
		private bool HasFinishedUseAnimation
		{
			get
			{
				return Time.timeAsDouble - this.useAnimationStartTime > (double)this.useAnimationDuration;
			}
		}

		/// <summary>
		/// Whether animation has reached the time when placement sound should play.
		/// </summary>
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x001A0CC7 File Offset: 0x0019EEC7
		private bool HasReachedSoundTrigger
		{
			get
			{
				return Time.timeAsDouble - this.useAnimationStartTime > (double)(this.useAnimationDuration * 0.8f);
			}
		}

		// Token: 0x04002EFE RID: 12030
		private static readonly ServerInstanceMethod<Vector3, float> SendBuildStructure = ServerInstanceMethod<Vector3, float>.Get(typeof(UseableStructure), "ReceiveBuildStructure");

		// Token: 0x04002EFF RID: 12031
		private static readonly ClientInstanceMethod SendPlayConstruct = ClientInstanceMethod.Get(typeof(UseableStructure), "ReceivePlayConstruct");

		/// <summary>
		/// Stripped-down version of structure prefab for previewing where the structure will be spawned.
		/// </summary>
		// Token: 0x04002F00 RID: 12032
		private Transform placementPreviewTransform;

		/// <summary>
		/// Whether preview object is currently highlighted positively.
		/// </summary>
		// Token: 0x04002F01 RID: 12033
		private bool isPlacementPreviewValid;

		/// <summary>
		/// Time when "Use" animation clip started playing in seconds.
		/// </summary>
		// Token: 0x04002F02 RID: 12034
		private double useAnimationStartTime;

		/// <summary>
		/// Length of "Use" animation clip in seconds.
		/// </summary>
		// Token: 0x04002F03 RID: 12035
		private float useAnimationDuration;

		/// <summary>
		/// True when animation starts playing, false after placement sound is played.
		/// </summary>
		// Token: 0x04002F04 RID: 12036
		private bool isWaitingForSoundTrigger;

		/// <summary>
		/// Whether the "Use" animation clip started playing.
		/// </summary>
		// Token: 0x04002F05 RID: 12037
		private bool isUseAnimationPlaying;

		/// <summary>
		/// If running as server, whether ReceiveBuildStructure has been called yet.
		/// </summary>
		// Token: 0x04002F06 RID: 12038
		private bool hasServerReceivedBuildRequest;

		/// <summary>
		/// Whether basic range and claim checks passed.
		/// </summary>
		// Token: 0x04002F07 RID: 12039
		private bool isServerBuildRequestInitiallyApproved;

		/// <summary>
		/// Position the item should be spawned at.
		/// </summary>
		// Token: 0x04002F08 RID: 12040
		private Vector3 pendingPlacementPosition;

		/// <summary>
		/// Rotation the item should be spawned at.
		/// </summary>
		// Token: 0x04002F09 RID: 12041
		private float pendingPlacementYaw;

		/// <summary>
		/// Interpolated toward customRotationOffset.
		/// </summary>
		// Token: 0x04002F0A RID: 12042
		private float animatedRotationOffset;

		/// <summary>
		/// Allows players to flip walls.
		/// </summary>
		// Token: 0x04002F0B RID: 12043
		private float customRotationOffset;

		/// <summary>
		/// Vertical offset using scroll wheel.
		/// </summary>
		// Token: 0x04002F0C RID: 12044
		private float foundationPositionOffset;

		// Token: 0x04002F0D RID: 12045
		private Vector3 serverPlacementPosition;

		// Token: 0x04002F0E RID: 12046
		private float serverPlacementYaw;
	}
}
