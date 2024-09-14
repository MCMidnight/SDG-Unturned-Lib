using System;
using System.Collections.Generic;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007DF RID: 2015
	public class UseableDetonator : Useable
	{
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004490 RID: 17552 RVA: 0x0018D747 File Offset: 0x0018B947
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x0018D75D File Offset: 0x0018B95D
		private bool isDetonatable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.33f;
			}
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0018D77C File Offset: 0x0018B97C
		private void plunge()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			base.player.animator.play("Use", false);
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0018D7CD File Offset: 0x0018B9CD
		[Obsolete]
		public void askPlunge(CSteamID steamID)
		{
			this.ReceivePlayPlunge();
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0018D7D5 File Offset: 0x0018B9D5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askPlunge")]
		public void ReceivePlayPlunge()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.plunge();
			}
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0018D7F0 File Offset: 0x0018B9F0
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isUseable)
			{
				if (base.channel.IsLocalPlayer)
				{
					for (int i = 0; i < this.charges.Count; i++)
					{
						InteractableCharge interactableCharge = this.charges[i];
						if (!(interactableCharge == null))
						{
							RaycastInfo info = new RaycastInfo(interactableCharge.transform);
							base.player.input.sendRaycast(info, ERaycastInfoUsage.Detonator);
						}
					}
					this.charges.Clear();
				}
				if (Provider.isServer)
				{
					this.charges.Clear();
					if (base.player.input.hasInputs())
					{
						int inputCount = base.player.input.getInputCount();
						for (int j = 0; j < inputCount; j++)
						{
							InputInfo input = base.player.input.getInput(false, ERaycastInfoUsage.Detonator);
							if (input != null && input.type == ERaycastInfoType.BARRICADE && !(input.transform == null) && input.transform.CompareTag("Barricade"))
							{
								InteractableCharge component = input.transform.GetComponent<InteractableCharge>();
								if (!(component == null) && OwnershipTool.checkToggle(base.channel.owner.playerID.steamID, component.owner, base.player.quests.groupID, component.group))
								{
									this.charges.Add(component);
								}
							}
						}
					}
				}
				base.player.equipment.isBusy = true;
				this.startedUse = Time.realtimeSinceStartup;
				this.isUsing = true;
				this.isDetonating = true;
				this.plunge();
				if (Provider.isServer)
				{
					base.player.life.markAggressive(false, true);
					UseableDetonator.SendPlayPlunge.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x0018D9E0 File Offset: 0x0018BBE0
		public override bool startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (base.channel.IsLocalPlayer && !this.isUsing && this.target != null)
			{
				if (this.target.isSelected)
				{
					this.target.deselect();
					this.charges.Remove(this.target);
				}
				else
				{
					this.target.select();
					this.charges.Add(this.target);
					if (this.charges.Count > 8)
					{
						if (this.charges[0] != null)
						{
							this.charges[0].deselect();
						}
						this.charges.RemoveAt(0);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x0018DAB8 File Offset: 0x0018BCB8
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.GetAnimationLength("Use", true);
			if (base.channel.IsLocalPlayer)
			{
				this.chargePoint = Vector3.zero;
				this.foundInRadius = new List<InteractableCharge>();
				this.chargesInRadius = new List<InteractableCharge>();
			}
			this.charges = new List<InteractableCharge>();
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0018DB30 File Offset: 0x0018BD30
		public override void dequip()
		{
			if (base.channel.IsLocalPlayer)
			{
				for (int i = 0; i < this.chargesInRadius.Count; i++)
				{
					this.chargesInRadius[i].unhighlight();
				}
			}
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x0018DB74 File Offset: 0x0018BD74
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isDetonating && this.isDetonatable)
			{
				if (this.charges.Count > 0)
				{
					if (simulation - this.lastExplosion > 1U)
					{
						this.lastExplosion = simulation;
						InteractableCharge interactableCharge = this.charges[0];
						this.charges.RemoveAt(0);
						if (interactableCharge != null)
						{
							interactableCharge.Detonate(base.player);
						}
					}
				}
				else
				{
					this.isDetonating = false;
				}
			}
			if (this.isUsing && this.isUseable && this.charges.Count == 0)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
			}
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x0018DC20 File Offset: 0x0018BE20
		public override void tick()
		{
			if (base.channel.IsLocalPlayer)
			{
				if ((base.transform.position - this.chargePoint).sqrMagnitude > 1f)
				{
					this.chargePoint = base.transform.position;
					this.foundInRadius.Clear();
					PowerTool.checkInteractables<InteractableCharge>(this.chargePoint, 64f, this.foundInRadius);
					for (int i = this.chargesInRadius.Count - 1; i >= 0; i--)
					{
						InteractableCharge interactableCharge = this.chargesInRadius[i];
						if (interactableCharge == null)
						{
							this.chargesInRadius.RemoveAtFast(i);
						}
						else if (!this.foundInRadius.Contains(interactableCharge))
						{
							interactableCharge.unhighlight();
							this.chargesInRadius.RemoveAtFast(i);
						}
					}
					for (int j = 0; j < this.foundInRadius.Count; j++)
					{
						InteractableCharge interactableCharge2 = this.foundInRadius[j];
						if (!(interactableCharge2 == null) && interactableCharge2.hasOwnership && !this.chargesInRadius.Contains(interactableCharge2))
						{
							interactableCharge2.highlight();
							this.chargesInRadius.Add(interactableCharge2);
						}
					}
				}
				InteractableCharge x = null;
				float num = 0.98f;
				for (int k = 0; k < this.chargesInRadius.Count; k++)
				{
					InteractableCharge interactableCharge3 = this.chargesInRadius[k];
					if (!(interactableCharge3 == null))
					{
						float num2 = Vector3.Dot((interactableCharge3.transform.position - MainCamera.instance.transform.position).normalized, MainCamera.instance.transform.forward);
						if (num2 > num)
						{
							x = interactableCharge3;
							num = num2;
						}
					}
				}
				if (x != this.target)
				{
					if (this.target != null)
					{
						this.target.untarget();
					}
					this.target = x;
					if (this.target != null)
					{
						this.target.target();
					}
				}
			}
		}

		// Token: 0x04002DE9 RID: 11753
		private float startedUse;

		// Token: 0x04002DEA RID: 11754
		private float useTime;

		// Token: 0x04002DEB RID: 11755
		private uint lastExplosion;

		// Token: 0x04002DEC RID: 11756
		private bool isUsing;

		// Token: 0x04002DED RID: 11757
		private bool isDetonating;

		// Token: 0x04002DEE RID: 11758
		private Vector3 chargePoint;

		// Token: 0x04002DEF RID: 11759
		private List<InteractableCharge> foundInRadius;

		// Token: 0x04002DF0 RID: 11760
		private List<InteractableCharge> chargesInRadius;

		// Token: 0x04002DF1 RID: 11761
		private List<InteractableCharge> charges;

		// Token: 0x04002DF2 RID: 11762
		private InteractableCharge target;

		// Token: 0x04002DF3 RID: 11763
		private static readonly ClientInstanceMethod SendPlayPlunge = ClientInstanceMethod.Get(typeof(UseableDetonator), "ReceivePlayPlunge");
	}
}
