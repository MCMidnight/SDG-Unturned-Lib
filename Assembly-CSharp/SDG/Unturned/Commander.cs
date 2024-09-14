using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x020003A3 RID: 931
	public class Commander
	{
		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001CDC RID: 7388 RVA: 0x00067030 File Offset: 0x00065230
		// (set) Token: 0x06001CDD RID: 7389 RVA: 0x00067037 File Offset: 0x00065237
		public static List<Command> commands { get; set; }

		// Token: 0x06001CDE RID: 7390 RVA: 0x00067040 File Offset: 0x00065240
		public static void register(Command command)
		{
			int num = Commander.commands.BinarySearch(command);
			if (num < 0)
			{
				num = ~num;
			}
			Commander.commands.Insert(num, command);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0006706C File Offset: 0x0006526C
		public static void deregister(Command command)
		{
			Commander.commands.Remove(command);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0006707C File Offset: 0x0006527C
		public static bool execute(CSteamID executorID, string command)
		{
			try
			{
				string method = command;
				string parameter = "";
				int num = command.IndexOf(' ');
				if (num != -1)
				{
					method = command.Substring(0, num);
					parameter = command.Substring(num + 1, command.Length - num - 1);
				}
				for (int i = 0; i < Commander.commands.Count; i++)
				{
					if (Commander.commands[i].check(executorID, method, parameter))
					{
						return true;
					}
				}
			}
			catch (Exception e)
			{
				UnturnedLog.exception(e, "Caught exception while executing command string \"{0}\"", new object[]
				{
					command
				});
			}
			return false;
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06001CE1 RID: 7393 RVA: 0x00067118 File Offset: 0x00065318
		// (remove) Token: 0x06001CE2 RID: 7394 RVA: 0x0006714C File Offset: 0x0006534C
		public static event Commander.ServerUnityEventPermissionHandler onCheckUnityEventPermissions;

		/// <summary>
		/// Allows Unity events to execute commands from the server.
		/// Messenger context is logged to help track down abusive assets.
		/// </summary>
		// Token: 0x06001CE3 RID: 7395 RVA: 0x00067180 File Offset: 0x00065380
		public static void execute_UnityEvent(string command, ServerTextChatMessenger messenger)
		{
			if (messenger == null)
			{
				throw new ArgumentNullException("messenger");
			}
			if (!Provider.configData.UnityEvents.Allow_Server_Commands)
			{
				return;
			}
			bool flag = true;
			Commander.ServerUnityEventPermissionHandler serverUnityEventPermissionHandler = Commander.onCheckUnityEventPermissions;
			if (serverUnityEventPermissionHandler != null)
			{
				serverUnityEventPermissionHandler(messenger, command, ref flag);
			}
			UnturnedLog.info("UnityEventCmd {0}: '{1}' Allow: {2}", new object[]
			{
				messenger.gameObject.GetSceneHierarchyPath(),
				command,
				flag
			});
			if (flag)
			{
				Commander.execute(CSteamID.Nil, command);
			}
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x00067204 File Offset: 0x00065404
		public static void init()
		{
			Commander.commands = new List<Command>();
			Commander.register(new CommandModules(Localization.read("/Server/ServerCommandModules.dat")));
			Commander.register(new CommandReload(Localization.read("/Server/ServerCommandReload.dat")));
			Commander.register(new CommandHelp(Localization.read("/Server/ServerCommandHelp.dat")));
			Commander.register(new CommandName(Localization.read("/Server/ServerCommandName.dat")));
			Commander.register(new CommandPort(Localization.read("/Server/ServerCommandPort.dat")));
			Commander.register(new CommandPassword(Localization.read("/Server/ServerCommandPassword.dat")));
			Commander.register(new CommandMaxPlayers(Localization.read("/Server/ServerCommandMaxPlayers.dat")));
			Commander.register(new CommandQueue(Localization.read("/Server/ServerCommandQueue.dat")));
			Commander.register(new CommandMap(Localization.read("/Server/ServerCommandMap.dat")));
			Commander.register(new CommandPvE(Localization.read("/Server/ServerCommandPvE.dat")));
			Commander.register(new CommandWhitelisted(Localization.read("/Server/ServerCommandWhitelisted.dat")));
			Commander.register(new CommandCheats(Localization.read("/Server/ServerCommandCheats.dat")));
			Commander.register(new CommandHideAdmins(Localization.read("/Server/ServerCommandHideAdmins.dat")));
			Commander.register(new CommandEffectUI(Localization.read("/Server/ServerCommandEffectUI.dat")));
			Commander.register(new CommandSync(Localization.read("/Server/ServerCommandSync.dat")));
			Commander.register(new CommandFilter(Localization.read("/Server/ServerCommandFilter.dat")));
			Commander.register(new CommandVotify(Localization.read("/Server/ServerCommandVotify.dat")));
			Commander.register(new CommandMode(Localization.read("/Server/ServerCommandMode.dat")));
			Commander.register(new CommandGameMode(Localization.read("/Server/ServerCommandGameMode.dat")));
			Commander.register(new CommandGold(Localization.read("/Server/ServerCommandGold.dat")));
			Commander.register(new CommandCamera(Localization.read("/Server/ServerCommandCamera.dat")));
			Commander.register(new CommandCycle(Localization.read("/Server/ServerCommandCycle.dat")));
			Commander.register(new CommandTime(Localization.read("/Server/ServerCommandTime.dat")));
			Commander.register(new CommandDay(Localization.read("/Server/ServerCommandDay.dat")));
			Commander.register(new CommandNight(Localization.read("/Server/ServerCommandNight.dat")));
			Commander.register(new CommandWeather(Localization.read("/Server/ServerCommandWeather.dat")));
			Commander.register(new CommandAirdrop(Localization.read("/Server/ServerCommandAirdrop.dat")));
			Commander.register(new CommandKick(Localization.read("/Server/ServerCommandKick.dat")));
			Commander.register(new CommandSpy(Localization.read("/Server/ServerCommandSpy.dat")));
			Commander.register(new CommandBan(Localization.read("/Server/ServerCommandBan.dat")));
			Commander.register(new CommandUnban(Localization.read("/Server/ServerCommandUnban.dat")));
			Commander.register(new CommandBans(Localization.read("/Server/ServerCommandBans.dat")));
			Commander.register(new CommandAdmin(Localization.read("/Server/ServerCommandAdmin.dat")));
			Commander.register(new CommandUnadmin(Localization.read("/Server/ServerCommandUnadmin.dat")));
			Commander.register(new CommandAdmins(Localization.read("/Server/ServerCommandAdmins.dat")));
			Commander.register(new CommandOwner(Localization.read("/Server/ServerCommandOwner.dat")));
			Commander.register(new CommandPermit(Localization.read("/Server/ServerCommandPermit.dat")));
			Commander.register(new CommandUnpermit(Localization.read("/Server/ServerCommandUnpermit.dat")));
			Commander.register(new CommandPermits(Localization.read("/Server/ServerCommandPermits.dat")));
			Commander.register(new CommandPlayers(Localization.read("/Server/ServerCommandPlayers.dat")));
			Commander.register(new CommandSay(Localization.read("/Server/ServerCommandSay.dat")));
			Commander.register(new CommandWelcome(Localization.read("/Server/ServerCommandWelcome.dat")));
			Commander.register(new CommandSlay(Localization.read("/Server/ServerCommandSlay.dat")));
			Commander.register(new CommandKill(Localization.read("/Server/ServerCommandKill.dat")));
			Commander.register(new CommandGive(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandUnlockNpcAchievement(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandScheduledShutdownInfo(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandSetNpcSpawnId(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandToggleNpcCutsceneMode(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandNpcEvent(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandLoadout(Localization.read("/Server/ServerCommandLoadout.dat")));
			Commander.register(new CommandExperience(Localization.read("/Server/ServerCommandExperience.dat")));
			Commander.register(new CommandReputation(Localization.read("/Server/ServerCommandReputation.dat")));
			Commander.register(new CommandFlag(Localization.read("/Server/ServerCommandFlag.dat")));
			Commander.register(new CommandQuest(Localization.read("/Server/ServerCommandQuest.dat")));
			Commander.register(new CommandVehicle(Localization.read("/Server/ServerCommandVehicle.dat")));
			Commander.register(new CommandAnimal(Localization.read("/Server/ServerCommandAnimal.dat")));
			Commander.register(new CommandTeleport(Localization.read("/Server/ServerCommandTeleport.dat")));
			Commander.register(new CommandTimeout(Localization.read("/Server/ServerCommandTimeout.dat")));
			Commander.register(new CommandChatrate(Localization.read("/Server/ServerCommandChatrate.dat")));
			Commander.register(new CommandLog(Localization.read("/Server/ServerCommandLog.dat")));
			Commander.register(new CommandLogMemoryUsage(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandLogTransportConnections(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandCopyServerCode(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandCopyFakeIP(Localization.read("/Server/ServerCommandGive.dat")));
			Commander.register(new CommandDebug(Localization.read("/Server/ServerCommandDebug.dat")));
			Commander.register(new CommandResetConfig(Localization.read("/Server/ServerCommandResetConfig.dat")));
			Commander.register(new CommandBind(Localization.read("/Server/ServerCommandBind.dat")));
			Commander.register(new CommandSave(Localization.read("/Server/ServerCommandSave.dat")));
			Commander.register(new CommandShutdown(Localization.read("/Server/ServerCommandShutdown.dat")));
			Commander.register(new CommandGSLT(Localization.read("/Server/ServerCommandGSLT.dat")));
		}

		// Token: 0x0200092E RID: 2350
		// (Invoke) Token: 0x06004A8E RID: 19086
		public delegate void ServerUnityEventPermissionHandler(ServerTextChatMessenger messenger, string command, ref bool shouldAllow);
	}
}
