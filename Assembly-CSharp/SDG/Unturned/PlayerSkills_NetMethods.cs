using System;
using SDG.NetPak;

namespace SDG.Unturned
{
	// Token: 0x02000200 RID: 512
	[NetInvokableGeneratedClass(typeof(PlayerSkills))]
	public static class PlayerSkills_NetMethods
	{
		// Token: 0x06000FDD RID: 4061 RVA: 0x0003766C File Offset: 0x0003586C
		[NetInvokableGeneratedMethod("ReceiveExperience", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveExperience_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			uint newExperience;
			SystemNetPakReaderEx.ReadUInt32(reader, ref newExperience);
			playerSkills.ReceiveExperience(newExperience);
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x000376B8 File Offset: 0x000358B8
		[NetInvokableGeneratedMethod("ReceiveExperience", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveExperience_Write(NetPakWriter writer, uint newExperience)
		{
			SystemNetPakWriterEx.WriteUInt32(writer, newExperience);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x000376C4 File Offset: 0x000358C4
		[NetInvokableGeneratedMethod("ReceiveReputation", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveReputation_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			int newReputation;
			SystemNetPakReaderEx.ReadInt32(reader, ref newReputation);
			playerSkills.ReceiveReputation(newReputation);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00037710 File Offset: 0x00035910
		[NetInvokableGeneratedMethod("ReceiveReputation", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveReputation_Write(NetPakWriter writer, int newReputation)
		{
			SystemNetPakWriterEx.WriteInt32(writer, newReputation);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0003771C File Offset: 0x0003591C
		[NetInvokableGeneratedMethod("ReceiveBoost", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBoost_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			EPlayerBoost newBoost;
			reader.ReadEnum(out newBoost);
			playerSkills.ReceiveBoost(newBoost);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00037768 File Offset: 0x00035968
		[NetInvokableGeneratedMethod("ReceiveBoost", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBoost_Write(NetPakWriter writer, EPlayerBoost newBoost)
		{
			writer.WriteEnum(newBoost);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00037774 File Offset: 0x00035974
		[NetInvokableGeneratedMethod("ReceiveSingleSkillLevel", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveSingleSkillLevel_Read(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			byte speciality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref speciality);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			byte level;
			SystemNetPakReaderEx.ReadUInt8(reader, ref level);
			playerSkills.ReceiveSingleSkillLevel(speciality, index, level);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x000377D6 File Offset: 0x000359D6
		[NetInvokableGeneratedMethod("ReceiveSingleSkillLevel", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveSingleSkillLevel_Write(NetPakWriter writer, byte speciality, byte index, byte level)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, speciality);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			SystemNetPakWriterEx.WriteUInt8(writer, level);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000377F0 File Offset: 0x000359F0
		[NetInvokableGeneratedMethod("ReceiveUpgradeRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveUpgradeRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerSkills.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerSkills));
				return;
			}
			byte speciality;
			SystemNetPakReaderEx.ReadUInt8(reader, ref speciality);
			byte index;
			SystemNetPakReaderEx.ReadUInt8(reader, ref index);
			bool force;
			reader.ReadBit(ref force);
			playerSkills.ReceiveUpgradeRequest(speciality, index, force);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00037872 File Offset: 0x00035A72
		[NetInvokableGeneratedMethod("ReceiveUpgradeRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveUpgradeRequest_Write(NetPakWriter writer, byte speciality, byte index, bool force)
		{
			SystemNetPakWriterEx.WriteUInt8(writer, speciality);
			SystemNetPakWriterEx.WriteUInt8(writer, index);
			writer.WriteBit(force);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0003788C File Offset: 0x00035A8C
		[NetInvokableGeneratedMethod("ReceiveBoostRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveBoostRequest_Read(in ServerInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerSkills.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerSkills));
				return;
			}
			playerSkills.ReceiveBoostRequest();
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x000378EB File Offset: 0x00035AEB
		[NetInvokableGeneratedMethod("ReceiveBoostRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceiveBoostRequest_Write(NetPakWriter writer)
		{
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000378F0 File Offset: 0x00035AF0
		[NetInvokableGeneratedMethod("ReceivePurchaseRequest", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceivePurchaseRequest_Read(in ServerInvocationContext context)
		{
			NetPakReader reader = context.reader;
			NetId key;
			if (!reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			if (!context.IsOwnerOf(playerSkills.channel))
			{
				context.Kick(string.Format("not owner of {0}", playerSkills));
				return;
			}
			NetId volumeNetId;
			reader.ReadNetId(out volumeNetId);
			playerSkills.ReceivePurchaseRequest(volumeNetId);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003795C File Offset: 0x00035B5C
		[NetInvokableGeneratedMethod("ReceivePurchaseRequest", ENetInvokableGeneratedMethodPurpose.Write)]
		public static void ReceivePurchaseRequest_Write(NetPakWriter writer, NetId volumeNetId)
		{
			writer.WriteNetId(volumeNetId);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00037968 File Offset: 0x00035B68
		[NetInvokableGeneratedMethod("ReceiveMultipleSkillLevels", ENetInvokableGeneratedMethodPurpose.Read)]
		public static void ReceiveMultipleSkillLevels_Read(in ClientInvocationContext context)
		{
			NetId key;
			if (!context.reader.ReadNetId(out key))
			{
				return;
			}
			object obj = NetIdRegistry.Get(key);
			if (obj == null)
			{
				return;
			}
			PlayerSkills playerSkills = obj as PlayerSkills;
			if (playerSkills == null)
			{
				return;
			}
			playerSkills.ReceiveMultipleSkillLevels(context);
		}
	}
}
