using System;
using SDG.NetTransport;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x0200046E RID: 1134
	public class InteractableStereo : Interactable
	{
		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x00087B83 File Offset: 0x00085D83
		// (set) Token: 0x060022B7 RID: 8887 RVA: 0x00087B8B File Offset: 0x00085D8B
		public float volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				this._volume = Mathf.Clamp01(value);
				if (this.audioSource != null)
				{
					this.audioSource.volume = this._volume;
				}
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x00087BB8 File Offset: 0x00085DB8
		// (set) Token: 0x060022B9 RID: 8889 RVA: 0x00087BCC File Offset: 0x00085DCC
		public byte compressedVolume
		{
			get
			{
				return (byte)Mathf.RoundToInt(this.volume * 100f);
			}
			set
			{
				this.volume = Mathf.Clamp01((float)value / 100f);
			}
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x00087BE4 File Offset: 0x00085DE4
		public void updateTrack(Guid newTrack)
		{
			this.track.GUID = newTrack;
			if (this.audioSource != null)
			{
				this.audioSource.clip = null;
				this.audioSource.loop = false;
				StereoSongAsset stereoSongAsset = Assets.Find_UseDefaultAssetMapping<StereoSongAsset>(this.track);
				if (stereoSongAsset != null)
				{
					if (stereoSongAsset.songMbRef.isValid)
					{
						this.audioSource.clip = stereoSongAsset.songMbRef.loadAsset(true);
					}
					else if (stereoSongAsset.songContentRef.isValid)
					{
						this.audioSource.clip = Assets.load<AudioClip>(stereoSongAsset.songContentRef);
					}
					this.audioSource.loop = stereoSongAsset.isLoop;
				}
				if (this.audioSource.clip != null)
				{
					this.audioSource.Play();
					return;
				}
				this.audioSource.Stop();
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00087CBC File Offset: 0x00085EBC
		public override void updateState(Asset asset, byte[] state)
		{
			GuidBuffer guidBuffer = default(GuidBuffer);
			guidBuffer.Read(state, 0);
			this.updateTrack(guidBuffer.GUID);
			this.compressedVolume = state[16];
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00087CF0 File Offset: 0x00085EF0
		public override void use()
		{
			PlayerUI.instance.boomboxUI.open(this);
			PlayerLifeUI.close();
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00087D07 File Offset: 0x00085F07
		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.USE;
			text = "";
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x00087D2C File Offset: 0x00085F2C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveTrack(Guid newTrack)
		{
			this.updateTrack(newTrack);
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00087D35 File Offset: 0x00085F35
		public void ClientSetTrack(Guid newTrack)
		{
			InteractableStereo.SendTrackRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, newTrack);
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x00087D4C File Offset: 0x00085F4C
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 2)]
		public void ReceiveTrackRequest(in ServerInvocationContext context, Guid newTrack)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out region))
			{
				return;
			}
			BarricadeManager.ServerSetStereoTrackInternal(this, x, y, plant, region, newTrack);
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00087DC6 File Offset: 0x00085FC6
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, deferMode = ENetInvocationDeferMode.Queue)]
		public void ReceiveChangeVolume(byte newVolume)
		{
			this.compressedVolume = newVolume;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00087DCF File Offset: 0x00085FCF
		public void ClientSetVolume(byte newVolume)
		{
			InteractableStereo.SendChangeVolumeRequest.Invoke(base.GetNetId(), ENetReliability.Unreliable, newVolume);
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00087DE4 File Offset: 0x00085FE4
		[SteamCall(ESteamCallValidation.SERVERSIDE, ratelimitHz = 8)]
		public void ReceiveChangeVolumeRequest(in ServerInvocationContext context, byte newVolume)
		{
			Player player = context.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (player.life.isDead)
			{
				return;
			}
			if ((base.transform.position - player.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(base.transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			newVolume = MathfEx.Min(newVolume, 100);
			InteractableStereo.SendChangeVolume.InvokeAndLoopback(base.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), newVolume);
			barricadeRegion.FindBarricadeByRootFast(base.transform).serversideData.barricade.state[16] = newVolume;
		}

		// Token: 0x0400112E RID: 4398
		protected float _volume;

		// Token: 0x0400112F RID: 4399
		public AssetReference<StereoSongAsset> track;

		// Token: 0x04001130 RID: 4400
		public AudioSource audioSource;

		// Token: 0x04001131 RID: 4401
		internal static readonly ClientInstanceMethod<Guid> SendTrack = ClientInstanceMethod<Guid>.Get(typeof(InteractableStereo), "ReceiveTrack");

		// Token: 0x04001132 RID: 4402
		private static readonly ServerInstanceMethod<Guid> SendTrackRequest = ServerInstanceMethod<Guid>.Get(typeof(InteractableStereo), "ReceiveTrackRequest");

		// Token: 0x04001133 RID: 4403
		private static readonly ClientInstanceMethod<byte> SendChangeVolume = ClientInstanceMethod<byte>.Get(typeof(InteractableStereo), "ReceiveChangeVolume");

		// Token: 0x04001134 RID: 4404
		private static readonly ServerInstanceMethod<byte> SendChangeVolumeRequest = ServerInstanceMethod<byte>.Get(typeof(InteractableStereo), "ReceiveChangeVolumeRequest");
	}
}
