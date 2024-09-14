using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Framework.Devkit
{
	// Token: 0x02000116 RID: 278
	public class EffectVolume : LevelVolume<EffectVolume, EffectVolumeManager>
	{
		// Token: 0x06000728 RID: 1832 RVA: 0x0001A9EC File Offset: 0x00018BEC
		public override ISleekElement CreateMenu()
		{
			ISleekElement sleekElement = new EffectVolume.Menu(this);
			base.AppendBaseMenu(sleekElement);
			return sleekElement;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x0001AA08 File Offset: 0x00018C08
		public Guid EffectGuid
		{
			get
			{
				return this._effectGuid;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001AA10 File Offset: 0x00018C10
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001AA18 File Offset: 0x00018C18
		public ushort id
		{
			[Obsolete]
			get
			{
				return this._id;
			}
			[Obsolete]
			set
			{
				this._id = value;
				this.SyncEffect();
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001AA27 File Offset: 0x00018C27
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001AA2F File Offset: 0x00018C2F
		public float emissionMultiplier
		{
			get
			{
				return this._emissionMultiplier;
			}
			set
			{
				this._emissionMultiplier = value;
				if (this.effect != null)
				{
					this.applyEmission();
				}
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001AA4C File Offset: 0x00018C4C
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001AA54 File Offset: 0x00018C54
		public float audioRangeMultiplier
		{
			get
			{
				return this._audioRangeMultiplier;
			}
			set
			{
				this._audioRangeMultiplier = value;
				if (this.effect != null)
				{
					this.applyAudioRange();
				}
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001AA74 File Offset: 0x00018C74
		private void SyncEffect()
		{
			if (this.effect != null)
			{
				Object.Destroy(this.effect.gameObject);
				this.effect = null;
			}
			EffectAsset effectAsset = Assets.FindEffectAssetByGuidOrLegacyId(this._effectGuid, this._id);
			if (effectAsset != null && effectAsset.spawnOnDedicatedServer)
			{
				this.effect = Object.Instantiate<GameObject>(effectAsset.effect).transform;
				this.effect.name = "Effect";
				this.effect.transform.parent = base.transform;
				this.effect.transform.localPosition = new Vector3(0f, 0f, 0f);
				this.effect.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				this.effect.transform.localScale = new Vector3(1f, 1f, 1f);
				ParticleSystem component = this.effect.GetComponent<ParticleSystem>();
				if (component != null)
				{
					this.maxParticlesBase = component.main.maxParticles;
					this.rateOverTimeBase = component.emission.rateOverTimeMultiplier;
				}
				AudioSource component2 = this.effect.GetComponent<AudioSource>();
				if (component2 != null && component2.clip != null)
				{
					component2.time = Random.Range(0f, component2.clip.length);
				}
			}
			if (this.effect != null)
			{
				this.applyEmission();
				this.applyAudioRange();
			}
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001AC08 File Offset: 0x00018E08
		protected virtual void applyEmission()
		{
			if (this.effect == null)
			{
				return;
			}
			ParticleSystem component = this.effect.GetComponent<ParticleSystem>();
			if (component == null)
			{
				return;
			}
			component.main.maxParticles = (int)((float)this.maxParticlesBase * this.emissionMultiplier);
			component.emission.rateOverTimeMultiplier = this.rateOverTimeBase * this.emissionMultiplier;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001AC74 File Offset: 0x00018E74
		protected virtual void applyAudioRange()
		{
			if (this.effect == null)
			{
				return;
			}
			AudioSource component = this.effect.GetComponent<AudioSource>();
			if (component == null)
			{
				return;
			}
			component.maxDistance = this.audioRangeMultiplier;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001ACB4 File Offset: 0x00018EB4
		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			if (reader.containsKey("Emission"))
			{
				this._emissionMultiplier = reader.readValue<float>("Emission");
			}
			if (reader.containsKey("Audio_Range"))
			{
				this._audioRangeMultiplier = reader.readValue<float>("Audio_Range");
			}
			string text = reader.readValue("ID");
			if (ushort.TryParse(text, ref this._id))
			{
				this._effectGuid = Guid.Empty;
				this.SyncEffect();
				return;
			}
			if (Guid.TryParse(text, ref this._effectGuid))
			{
				this._id = 0;
				this.SyncEffect();
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001AD4C File Offset: 0x00018F4C
		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			if (!GuidExtension.IsEmpty(this._effectGuid))
			{
				writer.writeValue<Guid>("ID", this._effectGuid);
			}
			else
			{
				writer.writeValue<ushort>("ID", this._id);
			}
			writer.writeValue<float>("Emission", this.emissionMultiplier);
			writer.writeValue<float>("Audio_Range", this.audioRangeMultiplier);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001ADB3 File Offset: 0x00018FB3
		protected override void Awake()
		{
			this.supportsSphereShape = false;
			base.Awake();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001ADC2 File Offset: 0x00018FC2
		protected override void Start()
		{
			base.Start();
			this.effect = base.transform.Find("Effect");
		}

		// Token: 0x040002AD RID: 685
		[SerializeField]
		internal Guid _effectGuid;

		/// <summary>
		/// Kept because lots of modders have been using this script in Unity,
		/// so removing legacy effect id would break their content.
		/// </summary>
		// Token: 0x040002AE RID: 686
		[SerializeField]
		protected ushort _id;

		// Token: 0x040002AF RID: 687
		[SerializeField]
		protected int maxParticlesBase;

		// Token: 0x040002B0 RID: 688
		[SerializeField]
		protected float rateOverTimeBase;

		// Token: 0x040002B1 RID: 689
		[SerializeField]
		protected float _emissionMultiplier = 1f;

		// Token: 0x040002B2 RID: 690
		[SerializeField]
		protected float _audioRangeMultiplier = 1f;

		// Token: 0x040002B3 RID: 691
		protected Transform effect;

		// Token: 0x0200086B RID: 2155
		private class Menu : SleekWrapper
		{
			// Token: 0x0600481C RID: 18460 RVA: 0x001AED04 File Offset: 0x001ACF04
			public Menu(EffectVolume volume)
			{
				this.volume = volume;
				base.SizeOffset_X = 400f;
				base.SizeOffset_Y = 110f;
				ISleekField sleekField = Glazier.Get().CreateStringField();
				sleekField.SizeOffset_X = 200f;
				sleekField.SizeOffset_Y = 30f;
				if (GuidExtension.IsEmpty(volume._effectGuid))
				{
					sleekField.Text = volume._id.ToString();
				}
				else
				{
					sleekField.Text = volume._effectGuid.ToString("N");
				}
				sleekField.AddLabel("Effect ID", 1);
				sleekField.OnTextChanged += new Typed(this.OnIdChanged);
				base.AddChild(sleekField);
				ISleekFloat32Field sleekFloat32Field = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field.PositionOffset_Y = 40f;
				sleekFloat32Field.SizeOffset_X = 200f;
				sleekFloat32Field.SizeOffset_Y = 30f;
				sleekFloat32Field.Value = volume.emissionMultiplier;
				sleekFloat32Field.AddLabel("Emission Rate", 1);
				sleekFloat32Field.OnValueChanged += new TypedSingle(this.OnEmissionChanged);
				base.AddChild(sleekFloat32Field);
				ISleekFloat32Field sleekFloat32Field2 = Glazier.Get().CreateFloat32Field();
				sleekFloat32Field2.PositionOffset_Y = 80f;
				sleekFloat32Field2.SizeOffset_X = 200f;
				sleekFloat32Field2.SizeOffset_Y = 30f;
				sleekFloat32Field2.Value = volume.audioRangeMultiplier;
				sleekFloat32Field2.AddLabel("Audio Range", 1);
				sleekFloat32Field2.OnValueChanged += new TypedSingle(this.OnAudioRangeChanged);
				base.AddChild(sleekFloat32Field2);
			}

			// Token: 0x0600481D RID: 18461 RVA: 0x001AEE6C File Offset: 0x001AD06C
			private void OnIdChanged(ISleekField field, string effectIdString)
			{
				if (ushort.TryParse(effectIdString, ref this.volume._id))
				{
					this.volume._effectGuid = Guid.Empty;
					this.volume.SyncEffect();
					return;
				}
				if (Guid.TryParse(effectIdString, ref this.volume._effectGuid))
				{
					this.volume._id = 0;
					this.volume.SyncEffect();
					return;
				}
				this.volume._effectGuid = Guid.Empty;
				this.volume._id = 0;
				this.volume.SyncEffect();
			}

			// Token: 0x0600481E RID: 18462 RVA: 0x001AEEFA File Offset: 0x001AD0FA
			private void OnEmissionChanged(ISleekFloat32Field field, float value)
			{
				this.volume.emissionMultiplier = value;
			}

			// Token: 0x0600481F RID: 18463 RVA: 0x001AEF08 File Offset: 0x001AD108
			private void OnAudioRangeChanged(ISleekFloat32Field field, float value)
			{
				this.volume.audioRangeMultiplier = value;
			}

			// Token: 0x04003174 RID: 12660
			private EffectVolume volume;
		}
	}
}
