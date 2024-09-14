using System;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000286 RID: 646
	public class AnimalAsset : Asset
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001311 RID: 4881 RVA: 0x00046057 File Offset: 0x00044257
		public string animalName
		{
			get
			{
				return this._animalName;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x0004605F File Offset: 0x0004425F
		public override string FriendlyName
		{
			get
			{
				return this._animalName;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001313 RID: 4883 RVA: 0x00046067 File Offset: 0x00044267
		public GameObject client
		{
			get
			{
				return this._client;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x0004606F File Offset: 0x0004426F
		public GameObject server
		{
			get
			{
				return this._server;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001315 RID: 4885 RVA: 0x00046077 File Offset: 0x00044277
		public GameObject dedicated
		{
			get
			{
				return this._dedicated;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x0004607F File Offset: 0x0004427F
		public GameObject ragdoll
		{
			get
			{
				return this._ragdoll;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001317 RID: 4887 RVA: 0x00046087 File Offset: 0x00044287
		public float speedRun
		{
			get
			{
				return this._speedRun;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x0004608F File Offset: 0x0004428F
		public float speedWalk
		{
			get
			{
				return this._speedWalk;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001319 RID: 4889 RVA: 0x00046097 File Offset: 0x00044297
		public EAnimalBehaviour behaviour
		{
			get
			{
				return this._behaviour;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600131A RID: 4890 RVA: 0x0004609F File Offset: 0x0004429F
		public ushort health
		{
			get
			{
				return this._health;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600131B RID: 4891 RVA: 0x000460A7 File Offset: 0x000442A7
		public uint rewardXP
		{
			get
			{
				return this._rewardXP;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600131C RID: 4892 RVA: 0x000460AF File Offset: 0x000442AF
		public float regen
		{
			get
			{
				return this._regen;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x000460B7 File Offset: 0x000442B7
		public byte damage
		{
			get
			{
				return this._damage;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600131E RID: 4894 RVA: 0x000460BF File Offset: 0x000442BF
		public ushort meat
		{
			get
			{
				return this._meat;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x000460C7 File Offset: 0x000442C7
		public ushort pelt
		{
			get
			{
				return this._pelt;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001320 RID: 4896 RVA: 0x000460CF File Offset: 0x000442CF
		public byte rewardMin
		{
			get
			{
				return this._rewardMin;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001321 RID: 4897 RVA: 0x000460D7 File Offset: 0x000442D7
		public byte rewardMax
		{
			get
			{
				return this._rewardMax;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x000460DF File Offset: 0x000442DF
		public ushort rewardID
		{
			get
			{
				return this._rewardID;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001323 RID: 4899 RVA: 0x000460E7 File Offset: 0x000442E7
		public AudioClip[] roars
		{
			get
			{
				return this._roars;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001324 RID: 4900 RVA: 0x000460EF File Offset: 0x000442EF
		public AudioClip[] panics
		{
			get
			{
				return this._panics;
			}
		}

		/// <summary>
		/// Number of Attack_# animations.
		/// </summary>
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06001325 RID: 4901 RVA: 0x000460F7 File Offset: 0x000442F7
		// (set) Token: 0x06001326 RID: 4902 RVA: 0x000460FF File Offset: 0x000442FF
		public int attackAnimVariantsCount { get; protected set; }

		/// <summary>
		/// Number of Eat_# animations.
		/// </summary>
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x00046108 File Offset: 0x00044308
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x00046110 File Offset: 0x00044310
		public int eatAnimVariantsCount { get; protected set; }

		/// <summary>
		/// Number of Glance_# animations.
		/// </summary>
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x00046119 File Offset: 0x00044319
		// (set) Token: 0x0600132A RID: 4906 RVA: 0x00046121 File Offset: 0x00044321
		public int glanceAnimVariantsCount { get; protected set; }

		/// <summary>
		/// Number of Startle_# animations.
		/// </summary>
		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600132B RID: 4907 RVA: 0x0004612A File Offset: 0x0004432A
		// (set) Token: 0x0600132C RID: 4908 RVA: 0x00046132 File Offset: 0x00044332
		public int startleAnimVariantsCount { get; protected set; }

		/// <summary>
		/// Maximum distance on the XZ plane.
		/// </summary>
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600132D RID: 4909 RVA: 0x0004613B File Offset: 0x0004433B
		// (set) Token: 0x0600132E RID: 4910 RVA: 0x00046143 File Offset: 0x00044343
		public float horizontalAttackRangeSquared { get; protected set; }

		/// <summary>
		/// Maximum distance on the XZ plane when attacking vehicles.
		/// </summary>
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600132F RID: 4911 RVA: 0x0004614C File Offset: 0x0004434C
		// (set) Token: 0x06001330 RID: 4912 RVA: 0x00046154 File Offset: 0x00044354
		public float horizontalVehicleAttackRangeSquared { get; protected set; }

		/// <summary>
		/// Maximum distance on the Y axis.
		/// </summary>
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06001331 RID: 4913 RVA: 0x0004615D File Offset: 0x0004435D
		// (set) Token: 0x06001332 RID: 4914 RVA: 0x00046165 File Offset: 0x00044365
		public float verticalAttackRange { get; protected set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06001333 RID: 4915 RVA: 0x0004616E File Offset: 0x0004436E
		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.ANIMAL;
			}
		}

		/// <summary>
		/// Temporary until something better makes sense? For Spyjack.
		/// </summary>
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06001334 RID: 4916 RVA: 0x00046171 File Offset: 0x00044371
		// (set) Token: 0x06001335 RID: 4917 RVA: 0x00046179 File Offset: 0x00044379
		public bool shouldPlayAnimsOnDedicatedServer { get; private set; }

		// Token: 0x06001336 RID: 4918 RVA: 0x00046184 File Offset: 0x00044384
		protected void validateAnimations(GameObject root)
		{
			Transform transform = root.transform.Find("Character");
			Animation animation = (transform != null) ? transform.GetComponent<Animation>() : null;
			if (animation == null)
			{
				Assets.reportError(this, "{0} missing Animation component on Character", root);
				return;
			}
			base.validateAnimation(animation, "Idle");
			base.validateAnimation(animation, "Walk");
			base.validateAnimation(animation, "Run");
			if (this.attackAnimVariantsCount > 1)
			{
				for (int i = 0; i < this.attackAnimVariantsCount; i++)
				{
					base.validateAnimation(animation, "Attack_" + i.ToString());
				}
			}
			if (this.eatAnimVariantsCount == 1)
			{
				base.validateAnimation(animation, "Eat");
			}
			else
			{
				for (int j = 0; j < this.eatAnimVariantsCount; j++)
				{
					base.validateAnimation(animation, "Eat_" + j.ToString());
				}
			}
			for (int k = 0; k < this.glanceAnimVariantsCount; k++)
			{
				base.validateAnimation(animation, "Glance_" + k.ToString());
			}
			if (this.startleAnimVariantsCount == 1)
			{
				base.validateAnimation(animation, "Startle");
				return;
			}
			for (int l = 0; l < this.startleAnimVariantsCount; l++)
			{
				base.validateAnimation(animation, "Startle_" + l.ToString());
			}
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x000462C8 File Offset: 0x000444C8
		public override void PopulateAsset(Bundle bundle, DatDictionary data, Local localization)
		{
			base.PopulateAsset(bundle, data, localization);
			if (this.id < 50 && !base.OriginAllowsVanillaLegacyId && !data.ContainsKey("Bypass_ID_Limit"))
			{
				throw new NotSupportedException("ID < 50");
			}
			this._animalName = localization.format("Name");
			this._client = bundle.load<GameObject>("Animal_Client");
			this._server = bundle.load<GameObject>("Animal_Server");
			this._dedicated = bundle.load<GameObject>("Animal_Dedicated");
			this._ragdoll = bundle.load<GameObject>("Ragdoll");
			if (this.client == null)
			{
				throw new NotSupportedException("missing \"Animal_Client\" GameObject");
			}
			if (Assets.shouldValidateAssets)
			{
				this.validateAnimations(this.client);
			}
			if (this.server == null)
			{
				throw new NotSupportedException("missing \"Animal_Server\" GameObject");
			}
			if (Assets.shouldValidateAssets)
			{
				this.validateAnimations(this.server);
			}
			if (this.dedicated == null)
			{
				throw new NotSupportedException("missing \"Animal_Dedicated\" GameObject");
			}
			if (this.ragdoll == null)
			{
				Assets.reportError(this, "missing 'Ragdoll' GameObject. Highly recommended to fix.");
			}
			this._speedRun = data.ParseFloat("Speed_Run", 0f);
			this._speedWalk = data.ParseFloat("Speed_Walk", 0f);
			this._behaviour = (EAnimalBehaviour)Enum.Parse(typeof(EAnimalBehaviour), data.GetString("Behaviour", null), true);
			this._health = data.ParseUInt16("Health", 0);
			this._regen = data.ParseFloat("Regen", 0f);
			if (!data.ContainsKey("Regen"))
			{
				this._regen = 10f;
			}
			this._damage = data.ParseUInt8("Damage", 0);
			this._meat = data.ParseUInt16("Meat", 0);
			this._pelt = data.ParseUInt16("Pelt", 0);
			this._rewardID = data.ParseUInt16("Reward_ID", 0);
			if (data.ContainsKey("Reward_Min"))
			{
				this._rewardMin = data.ParseUInt8("Reward_Min", 0);
			}
			else
			{
				this._rewardMin = 3;
			}
			if (data.ContainsKey("Reward_Max"))
			{
				this._rewardMax = data.ParseUInt8("Reward_Max", 0);
			}
			else
			{
				this._rewardMax = 4;
			}
			this._roars = new AudioClip[(int)data.ParseUInt8("Roars", 0)];
			byte b = 0;
			while ((int)b < this.roars.Length)
			{
				this.roars[(int)b] = bundle.load<AudioClip>("Roar_" + b.ToString());
				b += 1;
			}
			this._panics = new AudioClip[(int)data.ParseUInt8("Panics", 0)];
			byte b2 = 0;
			while ((int)b2 < this.panics.Length)
			{
				this.panics[(int)b2] = bundle.load<AudioClip>("Panic_" + b2.ToString());
				b2 += 1;
			}
			this.attackAnimVariantsCount = data.ParseInt32("Attack_Anim_Variants", 1);
			this.eatAnimVariantsCount = data.ParseInt32("Eat_Anim_Variants", 1);
			this.glanceAnimVariantsCount = data.ParseInt32("Glance_Anim_Variants", 2);
			this.startleAnimVariantsCount = data.ParseInt32("Startle_Anim_Variants", 1);
			this.horizontalAttackRangeSquared = MathfEx.Square(data.ParseFloat("Horizontal_Attack_Range", 2.25f));
			this.horizontalVehicleAttackRangeSquared = MathfEx.Square(data.ParseFloat("Horizontal_Vehicle_Attack_Range", 4.4f));
			this.verticalAttackRange = data.ParseFloat("Vertical_Attack_Range", 2f);
			this.attackInterval = data.ParseFloat("Attack_Interval", 1f);
			this.shouldPlayAnimsOnDedicatedServer = data.ParseBool("Should_Play_Anims_On_Dedicated_Server", false);
			this._rewardXP = data.ParseUInt32("Reward_XP", 0U);
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0004667E File Offset: 0x0004487E
		internal string OnGetRewardSpawnTableErrorContext()
		{
			return this.FriendlyName + " reward";
		}

		// Token: 0x0400065B RID: 1627
		protected string _animalName;

		// Token: 0x0400065C RID: 1628
		protected GameObject _client;

		// Token: 0x0400065D RID: 1629
		protected GameObject _server;

		// Token: 0x0400065E RID: 1630
		protected GameObject _dedicated;

		// Token: 0x0400065F RID: 1631
		protected GameObject _ragdoll;

		// Token: 0x04000660 RID: 1632
		protected float _speedRun;

		// Token: 0x04000661 RID: 1633
		protected float _speedWalk;

		// Token: 0x04000662 RID: 1634
		private EAnimalBehaviour _behaviour;

		// Token: 0x04000663 RID: 1635
		protected ushort _health;

		// Token: 0x04000664 RID: 1636
		protected uint _rewardXP;

		// Token: 0x04000665 RID: 1637
		protected float _regen;

		// Token: 0x04000666 RID: 1638
		protected byte _damage;

		// Token: 0x04000667 RID: 1639
		protected ushort _meat;

		// Token: 0x04000668 RID: 1640
		protected ushort _pelt;

		// Token: 0x04000669 RID: 1641
		private byte _rewardMin;

		// Token: 0x0400066A RID: 1642
		private byte _rewardMax;

		// Token: 0x0400066B RID: 1643
		private ushort _rewardID;

		// Token: 0x0400066C RID: 1644
		protected AudioClip[] _roars;

		// Token: 0x0400066D RID: 1645
		protected AudioClip[] _panics;

		/// <summary>
		/// Minimum seconds between attacks.
		/// </summary>
		// Token: 0x04000675 RID: 1653
		public float attackInterval;
	}
}
