using System;
using SDG.Framework.Water;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020007E1 RID: 2017
	public class UseableFisher : Useable
	{
		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x0018E07C File Offset: 0x0018C27C
		public override bool isUseableShowingMenu
		{
			get
			{
				return this.castStrengthBox != null && this.castStrengthBox.IsVisible;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060044A7 RID: 17575 RVA: 0x0018E093 File Offset: 0x0018C293
		private bool isCastable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedCast > this.castTime;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0018E0A9 File Offset: 0x0018C2A9
		private bool isReelable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedReel > this.reelTime;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x060044A9 RID: 17577 RVA: 0x0018E0BF File Offset: 0x0018C2BF
		private bool isBobable
		{
			get
			{
				if (!this.isCasting)
				{
					return Time.realtimeSinceStartup - this.startedReel > this.reelTime * 0.75f;
				}
				return Time.realtimeSinceStartup - this.startedCast > this.castTime * 0.45f;
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x0018E0FE File Offset: 0x0018C2FE
		private void reel()
		{
			base.player.animator.play("Reel", false);
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0018E116 File Offset: 0x0018C316
		private void startStrength()
		{
			PlayerLifeUI.close();
			if (this.castStrengthBox != null)
			{
				this.castStrengthBox.IsVisible = true;
			}
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x0018E131 File Offset: 0x0018C331
		private void stopStrength()
		{
			PlayerLifeUI.open();
			if (this.castStrengthBox != null)
			{
				this.castStrengthBox.IsVisible = false;
			}
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x0018E14C File Offset: 0x0018C34C
		[Obsolete]
		public void askCatch(CSteamID steamID)
		{
			this.ReceiveCatch();
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x0018E154 File Offset: 0x0018C354
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER, ratelimitHz = 10, legacyName = "askCatch")]
		public void ReceiveCatch()
		{
			if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 2.4f || (this.hasLuckReset && Time.realtimeSinceStartup - this.lastLuck < 1f))
			{
				this.isCatch = true;
			}
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x0018E192 File Offset: 0x0018C392
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceiveLuckTime(float NewLuckTime)
		{
			this.luckTime = NewLuckTime;
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x0018E19B File Offset: 0x0018C39B
		[Obsolete]
		public void askReel(CSteamID steamID)
		{
			this.ReceivePlayReel();
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0018E1A3 File Offset: 0x0018C3A3
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askReel")]
		public void ReceivePlayReel()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.reel();
			}
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0018E1BD File Offset: 0x0018C3BD
		private void cast()
		{
			base.player.animator.play("Cast", false);
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x0018E1D5 File Offset: 0x0018C3D5
		[Obsolete]
		public void askCast(CSteamID steamID)
		{
			this.ReceivePlayCast();
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0018E1DD File Offset: 0x0018C3DD
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "askCast")]
		public void ReceivePlayCast()
		{
			if (base.player.equipment.IsEquipAnimationFinished)
			{
				this.cast();
			}
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0018E1F8 File Offset: 0x0018C3F8
		public override bool startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return false;
			}
			if (this.isFishing)
			{
				this.isFishing = false;
				base.player.equipment.isBusy = true;
				this.startedReel = Time.realtimeSinceStartup;
				this.isReeling = true;
				if (base.channel.IsLocalPlayer)
				{
					this.isBobbing = true;
					if (this.bobberTransform != null && !this.isLuring && Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f && Time.realtimeSinceStartup - this.lastLuck < this.luckTime)
					{
						UseableFisher.SendCatch.Invoke(base.GetNetId(), ENetReliability.Reliable);
					}
				}
				this.reel();
				if (Provider.isServer)
				{
					if (this.isCatch)
					{
						this.isCatch = false;
						ItemFisherAsset itemFisherAsset = (ItemFisherAsset)base.player.equipment.asset;
						ushort num = SpawnTableTool.ResolveLegacyId(itemFisherAsset.rewardID, EAssetType.ITEM, new Func<string>(this.OnGetRewardErrorContext));
						if (num != 0)
						{
							base.player.inventory.forceAddItem(new Item(num, EItemOrigin.NATURE), false);
						}
						base.player.sendStat(EPlayerStat.FOUND_FISHES);
						int num2 = Random.Range(itemFisherAsset.rewardExperienceMin, itemFisherAsset.rewardExperienceMax + 1);
						if (num2 > 0)
						{
							base.player.skills.askPay((uint)num2);
						}
						itemFisherAsset.rewardsList.Grant(base.player);
					}
					UseableFisher.SendPlayReel.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
					AlertTool.alert(base.transform.position, 8f);
				}
			}
			else
			{
				this.isStrengthening = true;
				this.strengthTime = 0U;
				this.strengthMultiplier = 0f;
				if (base.channel.IsLocalPlayer)
				{
					this.startStrength();
				}
			}
			return true;
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0018E3CC File Offset: 0x0018C5CC
		public override void stopPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (!this.isStrengthening)
			{
				return;
			}
			this.isStrengthening = false;
			if (base.channel.IsLocalPlayer)
			{
				this.stopStrength();
			}
			this.isFishing = true;
			base.player.equipment.isBusy = true;
			this.startedCast = Time.realtimeSinceStartup;
			this.isCasting = true;
			if (base.channel.IsLocalPlayer)
			{
				this.isBobbing = true;
			}
			this.resetLuck();
			this.hasLuckReset = false;
			this.cast();
			if (Provider.isServer)
			{
				UseableFisher.SendPlayCast.Invoke(base.GetNetId(), ENetReliability.Unreliable, base.channel.GatherRemoteClientConnectionsExcludingOwner());
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x0018E498 File Offset: 0x0018C698
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.castTime = base.player.animator.GetAnimationLength("Cast", true);
			this.reelTime = base.player.animator.GetAnimationLength("Reel", true);
			if (base.channel.IsLocalPlayer)
			{
				this.firstHook = base.player.equipment.firstModel.Find("Hook");
				this.thirdHook = base.player.equipment.thirdModel.Find("Hook");
				this.firstLine = (LineRenderer)base.player.equipment.firstModel.Find("Line").GetComponent<Renderer>();
				this.firstLine.tag = "Viewmodel";
				this.firstLine.gameObject.layer = 11;
				this.firstLine.gameObject.SetActive(true);
				this.thirdLine = (LineRenderer)base.player.equipment.thirdModel.Find("Line").GetComponent<Renderer>();
				this.thirdLine.gameObject.SetActive(true);
				this.castStrengthBox = Glazier.Get().CreateBox();
				this.castStrengthBox.PositionOffset_X = -20f;
				this.castStrengthBox.PositionOffset_Y = -110f;
				this.castStrengthBox.PositionScale_X = 0.5f;
				this.castStrengthBox.PositionScale_Y = 0.5f;
				this.castStrengthBox.SizeOffset_X = 40f;
				this.castStrengthBox.SizeOffset_Y = 220f;
				PlayerUI.container.AddChild(this.castStrengthBox);
				this.castStrengthBox.IsVisible = false;
				this.castStrengthArea = Glazier.Get().CreateFrame();
				this.castStrengthArea.PositionOffset_X = 10f;
				this.castStrengthArea.PositionOffset_Y = 10f;
				this.castStrengthArea.SizeOffset_X = -20f;
				this.castStrengthArea.SizeOffset_Y = -20f;
				this.castStrengthArea.SizeScale_X = 1f;
				this.castStrengthArea.SizeScale_Y = 1f;
				this.castStrengthBox.AddChild(this.castStrengthArea);
				this.castStrengthBar = Glazier.Get().CreateImage();
				this.castStrengthBar.SizeScale_X = 1f;
				this.castStrengthBar.SizeScale_Y = 1f;
				this.castStrengthBar.Texture = GlazierResources.PixelTexture;
				this.castStrengthArea.AddChild(this.castStrengthBar);
			}
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x0018E740 File Offset: 0x0018C940
		public override void dequip()
		{
			if (base.channel.IsLocalPlayer)
			{
				if (this.bobberTransform != null)
				{
					Object.Destroy(this.bobberTransform.gameObject);
				}
				if (this.castStrengthBox != null)
				{
					PlayerUI.container.RemoveChild(this.castStrengthBox);
				}
				if (this.isStrengthening)
				{
					PlayerLifeUI.open();
				}
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0018E7A0 File Offset: 0x0018C9A0
		public override void tock(uint clock)
		{
			if (!this.isStrengthening)
			{
				return;
			}
			this.strengthTime += 1U;
			uint num = (uint)(100 + base.player.skills.skills[2][4].level * 20);
			this.strengthMultiplier = 1f - Mathf.Abs(Mathf.Sin((this.strengthTime + num / 2U) % num / num * 3.1415927f));
			this.strengthMultiplier *= this.strengthMultiplier;
			if (base.channel.IsLocalPlayer && this.castStrengthBar != null)
			{
				this.castStrengthBar.PositionScale_Y = 1f - this.strengthMultiplier;
				this.castStrengthBar.SizeScale_Y = this.strengthMultiplier;
				this.castStrengthBar.TintColor = ItemTool.getQualityColor(this.strengthMultiplier);
			}
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x0018E880 File Offset: 0x0018CA80
		public override void tick()
		{
			if (!base.player.equipment.IsEquipAnimationFinished)
			{
				return;
			}
			if (base.channel.IsLocalPlayer)
			{
				if (this.isBobable && this.isBobbing)
				{
					if (this.isCasting)
					{
						Vector3 vector = base.player.look.aim.position;
						Vector3 forward = base.player.look.aim.forward;
						RaycastHit raycastHit;
						if (Physics.Raycast(new Ray(vector, forward), out raycastHit, 1.5f, RayMasks.DAMAGE_SERVER))
						{
							vector += forward * (raycastHit.distance - 0.5f);
						}
						else
						{
							vector += forward;
						}
						this.bobberTransform = Object.Instantiate<GameObject>(Resources.Load<GameObject>("Fishers/Bob")).transform;
						this.bobberTransform.name = "Bob";
						this.bobberTransform.position = vector;
						this.bobberRigidbody = this.bobberTransform.GetComponent<Rigidbody>();
						if (this.bobberRigidbody != null)
						{
							this.bobberRigidbody.AddForce(forward * Mathf.Lerp(500f, 1000f, this.strengthMultiplier));
							this.bobberRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
						}
						this.isBobbing = false;
						this.isLuring = true;
					}
					else if (this.isReeling && this.bobberTransform != null)
					{
						Object.Destroy(this.bobberTransform.gameObject);
					}
				}
				if (this.bobberTransform != null)
				{
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						Vector3 position = MainCamera.instance.WorldToViewportPoint(this.bobberTransform.position);
						Vector3 position2 = base.player.animator.viewmodelCamera.ViewportToWorldPoint(position);
						this.firstLine.SetPosition(0, this.firstHook.position);
						this.firstLine.SetPosition(1, position2);
						return;
					}
					this.thirdLine.SetPosition(0, this.thirdHook.position);
					this.thirdLine.SetPosition(1, this.bobberTransform.position);
					return;
				}
				else
				{
					if (base.player.look.perspective == EPlayerPerspective.FIRST)
					{
						this.firstLine.SetPosition(0, Vector3.zero);
						this.firstLine.SetPosition(1, Vector3.zero);
						return;
					}
					this.thirdLine.SetPosition(0, Vector3.zero);
					this.thirdLine.SetPosition(1, Vector3.zero);
				}
			}
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x0018EAF8 File Offset: 0x0018CCF8
		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isCasting && this.isCastable)
			{
				base.player.equipment.isBusy = false;
				this.isCasting = false;
			}
			else if (this.isReeling && this.isReelable)
			{
				base.player.equipment.isBusy = false;
				this.isReeling = false;
			}
			if (!base.channel.IsLocalPlayer && Time.realtimeSinceStartup - this.lastLuck > this.luckTime && !this.isReeling)
			{
				this.resetLuck();
				this.hasLuckReset = true;
			}
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x0018EB90 File Offset: 0x0018CD90
		private void resetLuck()
		{
			this.lastLuck = Time.realtimeSinceStartup;
			if (Provider.isServer)
			{
				this.luckTime = Random.Range(50.2f, 60.2f) - this.strengthMultiplier * 33.5f;
				UseableFisher.SendLuckTime.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), this.luckTime);
			}
			this.hasSplashed = false;
			this.hasTugged = false;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x0018EC04 File Offset: 0x0018CE04
		private void Update()
		{
			if (this.bobberTransform != null && this.bobberRigidbody != null)
			{
				if (this.isLuring)
				{
					bool flag;
					float num;
					WaterUtility.getUnderwaterInfo(this.bobberTransform.position, out flag, out num);
					if (flag && this.bobberTransform.position.y < num - 4f)
					{
						this.bobberRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
						this.bobberRigidbody.useGravity = false;
						this.bobberRigidbody.isKinematic = true;
						this.water = this.bobberTransform.position;
						this.water.y = num;
						this.isLuring = false;
						return;
					}
				}
				else
				{
					if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime)
					{
						if (!this.isReeling)
						{
							this.resetLuck();
							this.hasLuckReset = true;
						}
					}
					else if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f)
					{
						if (!this.hasTugged)
						{
							this.hasTugged = true;
							base.player.playSound(((ItemFisherAsset)base.player.equipment.asset).tug);
							base.player.animator.play("Tug", false);
						}
					}
					else if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 2.4f && !this.hasSplashed)
					{
						this.hasSplashed = true;
						Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Fishers/Splash"))).transform;
						transform.name = "Splash";
						EffectManager.RegisterDebris(transform.gameObject);
						transform.position = this.water;
						transform.rotation = Quaternion.Euler(-90f, Random.Range(0f, 360f), 0f);
						Object.Destroy(transform.gameObject, 8f);
					}
					if (Time.realtimeSinceStartup - this.lastLuck > this.luckTime - 1.4f)
					{
						this.bobberRigidbody.MovePosition(Vector3.Lerp(this.bobberTransform.position, this.water + Vector3.down * 4f + Vector3.left * Random.Range(-4f, 4f) + Vector3.forward * Random.Range(-4f, 4f), 4f * Time.deltaTime));
						return;
					}
					this.bobberRigidbody.MovePosition(Vector3.Lerp(this.bobberTransform.position, this.water + Vector3.up * Mathf.Sin(Time.time) * 0.25f, 4f * Time.deltaTime));
				}
			}
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x0018EED5 File Offset: 0x0018D0D5
		private string OnGetRewardErrorContext()
		{
			string text = "fishing ";
			ItemAsset asset = base.player.equipment.asset;
			return text + ((asset != null) ? asset.FriendlyName : null) + " reward";
		}

		// Token: 0x04002DF8 RID: 11768
		private float startedCast;

		// Token: 0x04002DF9 RID: 11769
		private float startedReel;

		// Token: 0x04002DFA RID: 11770
		private float castTime;

		// Token: 0x04002DFB RID: 11771
		private float reelTime;

		// Token: 0x04002DFC RID: 11772
		private bool isStrengthening;

		// Token: 0x04002DFD RID: 11773
		private bool isCasting;

		// Token: 0x04002DFE RID: 11774
		private bool isReeling;

		// Token: 0x04002DFF RID: 11775
		private bool isFishing;

		// Token: 0x04002E00 RID: 11776
		private bool isBobbing;

		// Token: 0x04002E01 RID: 11777
		private bool isLuring;

		// Token: 0x04002E02 RID: 11778
		private bool isCatch;

		// Token: 0x04002E03 RID: 11779
		private Transform bobberTransform;

		// Token: 0x04002E04 RID: 11780
		private Rigidbody bobberRigidbody;

		// Token: 0x04002E05 RID: 11781
		private Transform firstHook;

		// Token: 0x04002E06 RID: 11782
		private Transform thirdHook;

		// Token: 0x04002E07 RID: 11783
		private LineRenderer firstLine;

		// Token: 0x04002E08 RID: 11784
		private LineRenderer thirdLine;

		// Token: 0x04002E09 RID: 11785
		private Vector3 water;

		// Token: 0x04002E0A RID: 11786
		private uint strengthTime;

		// Token: 0x04002E0B RID: 11787
		private float strengthMultiplier;

		// Token: 0x04002E0C RID: 11788
		private float lastLuck;

		// Token: 0x04002E0D RID: 11789
		private float luckTime;

		// Token: 0x04002E0E RID: 11790
		private bool hasLuckReset;

		// Token: 0x04002E0F RID: 11791
		private bool hasSplashed;

		// Token: 0x04002E10 RID: 11792
		private bool hasTugged;

		// Token: 0x04002E11 RID: 11793
		private ISleekBox castStrengthBox;

		// Token: 0x04002E12 RID: 11794
		private ISleekElement castStrengthArea;

		// Token: 0x04002E13 RID: 11795
		private ISleekImage castStrengthBar;

		// Token: 0x04002E14 RID: 11796
		private static readonly ServerInstanceMethod SendCatch = ServerInstanceMethod.Get(typeof(UseableFisher), "ReceiveCatch");

		// Token: 0x04002E15 RID: 11797
		private static readonly ClientInstanceMethod<float> SendLuckTime = ClientInstanceMethod<float>.Get(typeof(UseableFisher), "ReceiveLuckTime");

		// Token: 0x04002E16 RID: 11798
		private static readonly ClientInstanceMethod SendPlayReel = ClientInstanceMethod.Get(typeof(UseableFisher), "ReceivePlayReel");

		// Token: 0x04002E17 RID: 11799
		private static readonly ClientInstanceMethod SendPlayCast = ClientInstanceMethod.Get(typeof(UseableFisher), "ReceivePlayCast");
	}
}
