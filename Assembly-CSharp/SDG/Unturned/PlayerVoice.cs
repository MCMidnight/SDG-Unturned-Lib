using System;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x02000662 RID: 1634
	public class PlayerVoice : PlayerCaller
	{
		/// <summary>
		/// Is this player broadcasting their voice?
		/// Used in the menus to show an indicator who's talking.
		/// Locally set when recording starts/stops, and remotely when voice data starts/stops being received.
		/// </summary>
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000FEA6B File Offset: 0x000FCC6B
		// (set) Token: 0x0600366F RID: 13935 RVA: 0x000FEA73 File Offset: 0x000FCC73
		public bool isTalking
		{
			get
			{
				return this._isTalking;
			}
			private set
			{
				if (this._isTalking == value)
				{
					return;
				}
				this._isTalking = value;
				Talked talked = this.onTalkingChanged;
				if (talked == null)
				{
					return;
				}
				talked(value);
			}
		}

		/// <summary>
		/// Broadcasts after isTalking changes.
		/// </summary>
		// Token: 0x140000CD RID: 205
		// (add) Token: 0x06003670 RID: 13936 RVA: 0x000FEA98 File Offset: 0x000FCC98
		// (remove) Token: 0x06003671 RID: 13937 RVA: 0x000FEAD0 File Offset: 0x000FCCD0
		public event Talked onTalkingChanged;

		/// <summary>
		/// Can this player currently hear global (radio) voice chat?
		/// </summary>
		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x000FEB05 File Offset: 0x000FCD05
		public bool canHearRadio
		{
			get
			{
				return this.hasUseableWalkieTalkie || this.hasEarpiece;
			}
		}

		/// <summary>
		/// Is the player wearing an earpiece?
		/// Allows global (radio) voice chat to be heard without equipping the walkie-talkie item.
		/// </summary>
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x000FEB17 File Offset: 0x000FCD17
		public bool hasEarpiece
		{
			get
			{
				return base.player.clothing != null && base.player.clothing.maskAsset != null && base.player.clothing.maskAsset.isEarpiece;
			}
		}

		/// <summary>
		/// Only used by plugins.
		/// Called on server to allow plugins to override the default area and walkie-talkie voice channels.
		/// </summary>
		// Token: 0x140000CE RID: 206
		// (add) Token: 0x06003674 RID: 13940 RVA: 0x000FEB58 File Offset: 0x000FCD58
		// (remove) Token: 0x06003675 RID: 13941 RVA: 0x000FEB8C File Offset: 0x000FCD8C
		public static event PlayerVoice.RelayVoiceHandler onRelayVoice;

		/// <summary>
		/// Default culling handler when speaking over walkie-talkie.
		/// </summary>
		// Token: 0x06003676 RID: 13942 RVA: 0x000FEBBF File Offset: 0x000FCDBF
		public static bool handleRelayVoiceCulling_RadioFrequency(PlayerVoice speaker, PlayerVoice listener)
		{
			return listener.canHearRadio && speaker.player.quests.radioFrequency == listener.player.quests.radioFrequency;
		}

		/// <summary>
		/// Default culling handler when speaking in proximity.
		/// </summary>
		// Token: 0x06003677 RID: 13943 RVA: 0x000FEBF0 File Offset: 0x000FCDF0
		public static bool handleRelayVoiceCulling_Proximity(PlayerVoice speaker, PlayerVoice listener)
		{
			return (speaker.transform.position - listener.transform.position).sqrMagnitude < 16384f;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x000FEC2A File Offset: 0x000FCE2A
		public bool GetAllowTalkingWhileDead()
		{
			return this.allowTalkingWhileDead;
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x000FEC32 File Offset: 0x000FCE32
		public bool GetCustomAllowTalking()
		{
			return this.customAllowTalking;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x000FEC3C File Offset: 0x000FCE3C
		public void ServerSetPermissions(bool allowTalkingWhileDead, bool customAllowTalking)
		{
			if (this.allowTalkingWhileDead == allowTalkingWhileDead && this.customAllowTalking == customAllowTalking)
			{
				return;
			}
			this.allowTalkingWhileDead = allowTalkingWhileDead;
			this.customAllowTalking = customAllowTalking;
			PlayerVoice.SendPermissions.Invoke(base.GetNetId(), ENetReliability.Reliable, base.channel.GetOwnerTransportConnection(), allowTalkingWhileDead, customAllowTalking);
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000FEC88 File Offset: 0x000FCE88
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceivePermissions(bool allowTalkingWhileDead, bool customAllowTalking)
		{
			this.allowTalkingWhileDead = allowTalkingWhileDead;
			this.customAllowTalking = customAllowTalking;
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x000FEC98 File Offset: 0x000FCE98
		[Obsolete]
		public void askVoiceChat(byte[] packet)
		{
		}

		/// <summary>
		/// Called by owner to relay voice data to clients.
		/// Not using rate limit attribute because it internally tracks bytes per second.
		/// </summary>
		// Token: 0x0600367D RID: 13949 RVA: 0x000FEC9C File Offset: 0x000FCE9C
		[SteamCall(ESteamCallValidation.ONLY_FROM_OWNER)]
		public void ReceiveVoiceChatRelay(in ServerInvocationContext context)
		{
			if (base.player.life.isDead && !this.allowTalkingWhileDead)
			{
				return;
			}
			if (!this.customAllowTalking)
			{
				return;
			}
			if (!this.allowVoiceChat)
			{
				return;
			}
			NetPakReader reader = context.reader;
			ushort compressedSize;
			SystemNetPakReaderEx.ReadUInt16(reader, ref compressedSize);
			bool flag;
			reader.ReadBit(ref flag);
			byte[] source;
			int sourceOffset;
			if (!reader.ReadBytesPtr((int)compressedSize, ref source, ref sourceOffset))
			{
				return;
			}
			if (compressedSize < 1)
			{
				return;
			}
			float num = Time.realtimeSinceStartup - this.lastAskVoiceRealtime;
			if (num > 2f)
			{
				this.recentVoiceCalls = 1U;
				this.recentVoiceBytes = (uint)compressedSize;
				this.recentVoiceDuration = PlayerVoice.RECORDING_POLL_INTERVAL;
			}
			else
			{
				this.recentVoiceCalls += 1U;
				this.recentVoiceBytes += (uint)compressedSize;
				this.recentVoiceDuration += num;
			}
			this.lastAskVoiceRealtime = Time.realtimeSinceStartup;
			if (this.recentVoiceCalls >= PlayerVoice.EXPECTED_ASKVOICE_PER_SECOND || this.recentVoiceBytes > PlayerVoice.EXPECTED_BYTES_PER_SECOND || this.recentVoiceDuration > 1f)
			{
				float num2 = this.recentVoiceCalls / this.recentVoiceDuration;
				float num3 = this.recentVoiceBytes / this.recentVoiceDuration;
				if (num2 >= PlayerVoice.EXPECTED_ASKVOICE_PER_SECOND || num3 > PlayerVoice.EXPECTED_BYTES_PER_SECOND)
				{
					return;
				}
			}
			bool shouldBroadcastOverRadio;
			bool flag2;
			if (flag)
			{
				if (this.hasUseableWalkieTalkie)
				{
					flag2 = true;
					shouldBroadcastOverRadio = true;
				}
				else
				{
					flag2 = false;
					shouldBroadcastOverRadio = false;
				}
			}
			else
			{
				flag2 = true;
				shouldBroadcastOverRadio = false;
			}
			PlayerVoice.RelayVoiceCullingHandler cullingHandler = null;
			PlayerVoice.RelayVoiceHandler relayVoiceHandler = PlayerVoice.onRelayVoice;
			if (relayVoiceHandler != null)
			{
				relayVoiceHandler(this, flag, ref flag2, ref shouldBroadcastOverRadio, ref cullingHandler);
			}
			if (!flag2)
			{
				return;
			}
			if (cullingHandler == null)
			{
				cullingHandler = (shouldBroadcastOverRadio ? new PlayerVoice.RelayVoiceCullingHandler(PlayerVoice.handleRelayVoiceCulling_RadioFrequency) : new PlayerVoice.RelayVoiceCullingHandler(PlayerVoice.handleRelayVoiceCulling_Proximity));
			}
			PlayerVoice.SendPlayVoiceChat.Invoke(base.GetNetId(), ENetReliability.Unreliable, Provider.GatherRemoteClientConnectionsMatchingPredicate((SteamPlayer potentialRecipient) => potentialRecipient != null && !(potentialRecipient.player == null) && !(potentialRecipient.player.voice == null) && potentialRecipient != this.channel.owner && cullingHandler(this, potentialRecipient.player.voice)), delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt16(writer, compressedSize);
				writer.WriteBit(shouldBroadcastOverRadio);
				writer.WriteBytes(source, sourceOffset, (int)compressedSize);
			});
		}

		/// <summary>
		/// Called by server to relay voice data from clients.
		/// </summary>
		// Token: 0x0600367E RID: 13950 RVA: 0x000FEEA4 File Offset: 0x000FD0A4
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public void ReceivePlayVoiceChat(in ClientInvocationContext context)
		{
			if (!OptionsSettings.chatVoiceIn || base.channel.owner.isVoiceChatLocallyMuted)
			{
				return;
			}
			if (this.audioData == null || this.audioSource == null || this.audioSource.clip == null)
			{
				return;
			}
			NetPakReader reader = context.reader;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			bool wantsToUseWalkieTalkie;
			reader.ReadBit(ref wantsToUseWalkieTalkie);
			if (!reader.ReadBytes(PlayerVoice.compressedVoiceBuffer, (int)num))
			{
				return;
			}
			this.AppendVoiceData(PlayerVoice.compressedVoiceBuffer, (uint)num, wantsToUseWalkieTalkie);
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x000FEF28 File Offset: 0x000FD128
		private void AppendVoiceData(byte[] compressedBuffer, uint compressedSize, bool wantsToUseWalkieTalkie)
		{
			this.playbackUsingWalkieTalkie = wantsToUseWalkieTalkie;
			uint num;
			if (SteamUser.DecompressVoice(compressedBuffer, compressedSize, PlayerVoice.DECOMPRESSED_VOICE_BUFFER, (uint)PlayerVoice.DECOMPRESSED_VOICE_BUFFER.Length, out num, this.steamOptimalSampleRate) != EVoiceResult.k_EVoiceResultOK || num < 1U)
			{
				return;
			}
			int num2 = this.audioDataWriteIndex;
			float num3 = 0f;
			for (uint num4 = 0U; num4 < num; num4 += 2U)
			{
				int num5 = (int)PlayerVoice.DECOMPRESSED_VOICE_BUFFER[(int)num4];
				byte b = PlayerVoice.DECOMPRESSED_VOICE_BUFFER[(int)(num4 + 1U)];
				float num6 = (float)((short)(num5 | (int)b << 8)) / 32767f;
				num3 = Mathf.Max(Mathf.Abs(num6), num3);
				this.audioData[this.audioDataWriteIndex] = num6;
				this.audioDataWriteIndex = (this.audioDataWriteIndex + 1) % this.audioData.Length;
			}
			if (num3 != 0f)
			{
				float num7 = Mathf.Min(1f / num3, 8f);
				for (uint num8 = 0U; num8 < num; num8 += 2U)
				{
					this.audioData[num2] *= num7;
					num2 = (num2 + 1) % this.audioData.Length;
				}
			}
			int num9 = this.audioDataWriteIndex;
			for (int i = 0; i < this.zeroSamples; i++)
			{
				this.audioData[num9] = 0f;
				num9 = (num9 + 1) % this.audioData.Length;
			}
			this.audioClip.SetData(this.audioData, 0);
			float num10 = num / 2U * this.secondsPerSample;
			if (!this.isPlayingVoiceData && !this.hasPendingVoiceData)
			{
				this.hasPendingVoiceData = true;
				this.pendingPlaybackDelay = PlayerVoice.PLAYBACK_DELAY;
				this.availablePlaybackTime = PlayerVoice.SILENCE_DURATION;
			}
			this.availablePlaybackTime += num10;
		}

		/// <summary>
		/// If true, SteamUser.StartVoiceRecording has been called without a corresponding call to
		/// SteamUser.StopVoiceRecording yet.
		/// </summary>
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x000FF0AF File Offset: 0x000FD2AF
		// (set) Token: 0x06003681 RID: 13953 RVA: 0x000FF0B7 File Offset: 0x000FD2B7
		private bool SteamIsRecording
		{
			get
			{
				return this._isSteamRecording;
			}
			set
			{
				if (this._isSteamRecording != value)
				{
					this._isSteamRecording = value;
					if (this._isSteamRecording)
					{
						SteamUser.StartVoiceRecording();
						return;
					}
					SteamUser.StopVoiceRecording();
				}
			}
		}

		/// <summary>
		/// Set by updateInput based on whether voice is enabled, key is held, is alive, etc.
		/// Reset to false during OnDestroy to stop recording.
		/// </summary>
		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003682 RID: 13954 RVA: 0x000FF0DC File Offset: 0x000FD2DC
		// (set) Token: 0x06003683 RID: 13955 RVA: 0x000FF0E4 File Offset: 0x000FD2E4
		private bool inputWantsToRecord
		{
			get
			{
				return this._inputWantsToRecord;
			}
			set
			{
				if (this._inputWantsToRecord == value)
				{
					return;
				}
				this._inputWantsToRecord = value;
				if (this._inputWantsToRecord)
				{
					this.pollRecordingTimer = 0f;
				}
				this.SynchronizeSteamIsRecording();
				SteamFriends.SetInGameVoiceSpeaking(Provider.user, this.inputWantsToRecord);
				if (!this.canEverPlayback)
				{
					if (this.hasUseableWalkieTalkie)
					{
						this.playWalkieTalkieSound();
					}
					this.isTalking = this.inputWantsToRecord;
				}
			}
		}

		/// <summary>
		/// Called during Update on owner client to start/stop recording.
		/// </summary>
		// Token: 0x06003684 RID: 13956 RVA: 0x000FF150 File Offset: 0x000FD350
		private void updateInput()
		{
			bool flag = OptionsSettings.chatVoiceIn && OptionsSettings.chatVoiceOut;
			bool flag2 = base.player.life.IsAlive || this.allowTalkingWhileDead;
			bool flag3 = flag && flag2 && this.customAllowTalking;
			if (ControlsSettings.voiceMode == EControlMode.HOLD)
			{
				bool key = InputEx.GetKey(ControlsSettings.voice);
				this.inputWantsToRecord = (flag3 && key);
				this.inputToggleState = false;
				return;
			}
			if (ControlsSettings.voiceMode == EControlMode.TOGGLE)
			{
				if (InputEx.GetKeyDown(ControlsSettings.voice))
				{
					this.inputToggleState = !this.inputToggleState;
				}
				this.inputToggleState = (this.inputToggleState && flag3);
				this.inputWantsToRecord = this.inputToggleState;
			}
		}

		/// <summary>
		/// Called during Update on owner client to record voice data.
		/// </summary>
		// Token: 0x06003685 RID: 13957 RVA: 0x000FF1F8 File Offset: 0x000FD3F8
		private void updateRecording()
		{
			this.pollRecordingTimer += Time.unscaledDeltaTime;
			if (this.pollRecordingTimer < PlayerVoice.RECORDING_POLL_INTERVAL)
			{
				return;
			}
			this.pollRecordingTimer = 0f;
			uint num;
			EVoiceResult availableVoice = SteamUser.GetAvailableVoice(out num);
			if (availableVoice != EVoiceResult.k_EVoiceResultOK && availableVoice != EVoiceResult.k_EVoiceResultNoData && availableVoice != EVoiceResult.k_EVoiceResultNotRecording)
			{
				UnturnedLog.error("GetAvailableVoice result: " + availableVoice.ToString());
			}
			if (availableVoice != EVoiceResult.k_EVoiceResultOK || num < 1U)
			{
				return;
			}
			if ((ulong)num > (ulong)((long)PlayerVoice.compressedVoiceBuffer.Length))
			{
				UnturnedLog.info(string.Format("Resizing compressed voice buffer ({0}) to fit available size ({1})", PlayerVoice.compressedVoiceBuffer.Length, num));
				PlayerVoice.compressedVoiceBuffer = new byte[num];
			}
			uint compressedSize;
			EVoiceResult voice = SteamUser.GetVoice(true, PlayerVoice.compressedVoiceBuffer, num, out compressedSize);
			if (voice != EVoiceResult.k_EVoiceResultOK && voice != EVoiceResult.k_EVoiceResultNoData)
			{
				UnturnedLog.error("GetVoice result: " + voice.ToString());
			}
			if (voice != EVoiceResult.k_EVoiceResultOK || compressedSize < 1U)
			{
				return;
			}
			if (!this._inputWantsToRecord)
			{
				return;
			}
			if (Provider.isServer)
			{
				this.AppendVoiceData(PlayerVoice.compressedVoiceBuffer, compressedSize, this.hasUseableWalkieTalkie);
				return;
			}
			PlayerVoice.SendVoiceChatRelay.Invoke(base.GetNetId(), ENetReliability.Unreliable, delegate(NetPakWriter writer)
			{
				ushort num2 = (ushort)compressedSize;
				SystemNetPakWriterEx.WriteUInt16(writer, num2);
				writer.WriteBit(this.hasUseableWalkieTalkie);
				writer.WriteBytes(PlayerVoice.compressedVoiceBuffer, (int)num2);
			});
		}

		/// <summary>
		/// Play walkie-talkie squawk at our position.
		/// </summary>
		// Token: 0x06003686 RID: 13958 RVA: 0x000FF334 File Offset: 0x000FD534
		private void playWalkieTalkieSound()
		{
		}

		/// <summary>
		/// Start and stop playback of received audio stream.
		/// </summary>
		// Token: 0x06003687 RID: 13959 RVA: 0x000FF338 File Offset: 0x000FD538
		private void updatePlayback()
		{
			if (this.audioSource == null)
			{
				return;
			}
			if (this.playbackUsingWalkieTalkie)
			{
				this.audioSource.spatialBlend = 0f;
			}
			else
			{
				this.audioSource.spatialBlend = 1f;
			}
			if (this.isPlayingVoiceData)
			{
				this.availablePlaybackTime -= Time.deltaTime;
				if (this.availablePlaybackTime <= 0f)
				{
					this.audioSource.Stop();
					this.audioSource.time = 0f;
					this.audioDataWriteIndex = 0;
					this.isPlayingVoiceData = false;
					this.hasPendingVoiceData = false;
					if (this.playbackUsingWalkieTalkie)
					{
						this.playWalkieTalkieSound();
					}
					this.isTalking = false;
					return;
				}
			}
			else if (this.hasPendingVoiceData)
			{
				this.pendingPlaybackDelay -= Time.deltaTime;
				if (this.pendingPlaybackDelay <= 0f)
				{
					this.isPlayingVoiceData = true;
					if (this.playbackUsingWalkieTalkie)
					{
						this.playWalkieTalkieSound();
					}
					this.audioSource.Play();
					this.isTalking = true;
				}
			}
		}

		/// <summary>
		/// Will this component ever need to record voice data?
		/// </summary>
		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06003688 RID: 13960 RVA: 0x000FF43C File Offset: 0x000FD63C
		private bool canEverRecord
		{
			get
			{
				return base.channel.IsLocalPlayer && !Provider.isServer;
			}
		}

		/// <summary>
		/// Will this component ever need to play voice data?
		/// In release builds this is only true for remote clients, but in debug we may want to locally listen.
		/// </summary>
		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06003689 RID: 13961 RVA: 0x000FF455 File Offset: 0x000FD655
		private bool canEverPlayback
		{
			get
			{
				return !base.channel.IsLocalPlayer;
			}
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x000FF465 File Offset: 0x000FD665
		private void Update()
		{
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x000FF468 File Offset: 0x000FD668
		internal void InitializePlayer()
		{
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x000FF475 File Offset: 0x000FD675
		private void OnDestroy()
		{
			this.isBeingDestroyed = true;
			if (this.canEverRecord)
			{
				OptionsSettings.OnVoiceAlwaysRecordingChanged -= new Action(this.SynchronizeSteamIsRecording);
				this.inputWantsToRecord = false;
			}
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x000FF4A0 File Offset: 0x000FD6A0
		private void SynchronizeSteamIsRecording()
		{
			bool flag = OptionsSettings.chatVoiceIn && OptionsSettings.chatVoiceOut;
			this.SteamIsRecording = (flag && !this.isBeingDestroyed && (this.inputWantsToRecord || OptionsSettings.VoiceAlwaysRecording));
		}

		/// <summary>
		/// Speaker writes compressed audio to this buffer.
		/// Listener copies network buffer here for decompression.
		/// </summary>
		// Token: 0x04001F61 RID: 8033
		private static byte[] compressedVoiceBuffer = new byte[8000];

		/// <summary>
		/// Listener writes decompressed PCM data to this buffer.
		/// </summary>
		// Token: 0x04001F62 RID: 8034
		private static readonly byte[] DECOMPRESSED_VOICE_BUFFER = new byte[22000];

		/// <summary>
		/// Seconds interval to wait between asking recording subsystem for voice data.
		/// Rather than polling every frame we wait until data has accumulated to send.
		/// </summary>
		// Token: 0x04001F63 RID: 8035
		private static readonly float RECORDING_POLL_INTERVAL = 0.05f;

		/// <summary>
		/// Seconds to wait before playing back newly received data.
		/// Allows a few samples to buffer up so that we don't stutter as more arrive.
		/// </summary>
		// Token: 0x04001F64 RID: 8036
		private static readonly float PLAYBACK_DELAY = 0.2f;

		/// <summary>
		/// Seconds to wait after playback before stopping audio source.
		/// We zero this portion of the clip to prevent pops.
		/// </summary>
		// Token: 0x04001F65 RID: 8037
		private static readonly float SILENCE_DURATION = 0.1f;

		/// <summary>
		/// Max calls to askVoice server will allow per second before blocking their voice data.
		/// Prevents spamming many tiny requests bogging down server output.
		/// </summary>
		// Token: 0x04001F66 RID: 8038
		private static readonly uint EXPECTED_ASKVOICE_PER_SECOND = (uint)(1f / PlayerVoice.RECORDING_POLL_INTERVAL) + 3U;

		/// <summary>
		/// Max compressed bytes server will allow per second before blocking their voice data.
		/// When logging compressed size they averaged 3000-5000 per second, so this affords some wiggle-room.
		/// </summary>
		// Token: 0x04001F67 RID: 8039
		private static readonly uint EXPECTED_BYTES_PER_SECOND = 7000U;

		// Token: 0x04001F68 RID: 8040
		[Obsolete("Replaced by ServerSetPermissions which is replicated to owner.")]
		public bool allowVoiceChat = true;

		/// <summary>
		/// Internal value managed by isTalking.
		/// </summary>
		// Token: 0x04001F69 RID: 8041
		private bool _isTalking;

		/// <summary>
		/// Is a UseableWalkieTalkie currently equipped?
		/// Set by useable's equip and dequip events.
		/// </summary>
		// Token: 0x04001F6B RID: 8043
		public bool hasUseableWalkieTalkie;

		/// <summary>
		/// Was the most recent voice data we received sent using walkie talkie?
		/// </summary>
		// Token: 0x04001F6C RID: 8044
		private bool playbackUsingWalkieTalkie;

		/// <summary>
		/// Has voice data recently been received, but we're waiting slightly to begin playback?
		/// Important to give clip a chance to buffer up so that we don't stutter as more samples arrive.
		/// </summary>
		// Token: 0x04001F6D RID: 8045
		private bool hasPendingVoiceData;

		/// <summary>
		/// AudioSource.isPlaying is not trustworthy.
		/// </summary>
		// Token: 0x04001F6E RID: 8046
		private bool isPlayingVoiceData;

		/// <summary>
		/// Timer counting down to begin playback of recently received voice data.
		/// We use a timer rather than availableSamples.Count because a very short phrase could be less than threshold.
		/// </summary>
		// Token: 0x04001F6F RID: 8047
		private float pendingPlaybackDelay;

		/// <summary>
		/// Timer counting down to end playback.
		/// </summary>
		// Token: 0x04001F70 RID: 8048
		private float availablePlaybackTime;

		/// <summary>
		/// Accumulated realtime since we last polled data from voice subsystem.
		/// </summary>
		// Token: 0x04001F71 RID: 8049
		private float pollRecordingTimer;

		/// <summary>
		/// Last time askVoiceChat was invoked over network.
		/// </summary>
		// Token: 0x04001F72 RID: 8050
		private float lastAskVoiceRealtime;

		/// <summary>
		/// Number of times askVoiceChat has been called recently, to prevent calling it many times
		/// with tiny durations getting server to relay many packets to clients.
		/// </summary>
		// Token: 0x04001F73 RID: 8051
		private uint recentVoiceCalls;

		/// <summary>
		/// Total of recent compressed voice payload lengths.
		/// </summary>
		// Token: 0x04001F74 RID: 8052
		private uint recentVoiceBytes;

		/// <summary>
		/// Realtime since this recent conversation began.
		/// </summary>
		// Token: 0x04001F75 RID: 8053
		private float recentVoiceDuration;

		// Token: 0x04001F77 RID: 8055
		private static readonly ClientInstanceMethod<bool, bool> SendPermissions = ClientInstanceMethod<bool, bool>.Get(typeof(PlayerVoice), "ReceivePermissions");

		// Token: 0x04001F78 RID: 8056
		private static readonly ServerInstanceMethod SendVoiceChatRelay = ServerInstanceMethod.Get(typeof(PlayerVoice), "ReceiveVoiceChatRelay");

		// Token: 0x04001F79 RID: 8057
		private static ClientInstanceMethod SendPlayVoiceChat = ClientInstanceMethod.Get(typeof(PlayerVoice), "ReceivePlayVoiceChat");

		/// <summary>
		/// Set to true during OnDestroy to make sure we don't start recording again.
		/// </summary>
		// Token: 0x04001F7A RID: 8058
		private bool isBeingDestroyed;

		// Token: 0x04001F7B RID: 8059
		private bool _isSteamRecording;

		/// <summary>
		/// If true, voice toggle is in ON mode.
		/// </summary>
		// Token: 0x04001F7C RID: 8060
		private bool inputToggleState;

		/// <summary>
		/// Internal value managed by inputWantsToRecord.
		/// </summary>
		// Token: 0x04001F7D RID: 8061
		private bool _inputWantsToRecord;

		// Token: 0x04001F7E RID: 8062
		private static StaticResourceRef<AudioClip> radioClip = new StaticResourceRef<AudioClip>("Sounds/General/Radio");

		/// <summary>
		/// Player's voice audio source cached during Start.
		/// </summary>
		// Token: 0x04001F7F RID: 8063
		private AudioSource audioSource;

		/// <summary>
		/// Looping voice audio clip.
		/// </summary>
		// Token: 0x04001F80 RID: 8064
		private AudioClip audioClip;

		/// <summary>
		/// Playback buffer.
		/// </summary>
		// Token: 0x04001F81 RID: 8065
		private float[] audioData;

		// Token: 0x04001F82 RID: 8066
		private int audioDataWriteIndex;

		/// <summary>
		/// Steam does less work on the main thread if we request samples at the native decompresser sample rate,
		/// so the re-sampling can be done on the Unity audio thread instead.
		/// </summary>
		// Token: 0x04001F83 RID: 8067
		private uint steamOptimalSampleRate;

		/// <summary>
		/// 1 / frequency
		/// </summary>
		// Token: 0x04001F84 RID: 8068
		private float secondsPerSample;

		/// <summary>
		/// Number of samples to zero after writing new audio data.
		/// </summary>
		// Token: 0x04001F85 RID: 8069
		private int zeroSamples;

		// Token: 0x04001F86 RID: 8070
		private bool allowTalkingWhileDead;

		// Token: 0x04001F87 RID: 8071
		private bool customAllowTalking = true;

		// Token: 0x020009AE RID: 2478
		// (Invoke) Token: 0x06004C06 RID: 19462
		public delegate void RelayVoiceHandler(PlayerVoice speaker, bool wantsToUseWalkieTalkie, ref bool shouldAllow, ref bool shouldBroadcastOverRadio, ref PlayerVoice.RelayVoiceCullingHandler cullingHandler);

		// Token: 0x020009AF RID: 2479
		// (Invoke) Token: 0x06004C0A RID: 19466
		public delegate bool RelayVoiceCullingHandler(PlayerVoice speaker, PlayerVoice listener);
	}
}
