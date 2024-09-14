using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	// Token: 0x020003CE RID: 974
	public class CommandTeleport : Command
	{
		/// <summary>
		/// Cast a ray from the sky to find highest point.
		/// </summary>
		// Token: 0x06001D42 RID: 7490 RVA: 0x0006A628 File Offset: 0x00068828
		protected bool raycastFromSkyToPosition(ref Vector3 position)
		{
			position.y = 1024f;
			RaycastHit raycastHit;
			if (Physics.Raycast(position, Vector3.down, out raycastHit, 2048f, RayMasks.WAYPOINT))
			{
				position = raycastHit.point + Vector3.up;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Cast a ray from slightly above point so indoor teleport nodes work.
		/// </summary>
		// Token: 0x06001D43 RID: 7491 RVA: 0x0006A678 File Offset: 0x00068878
		protected void raycastFromNearPosition(ref Vector3 position)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(position + new Vector3(0f, 4f, 0f), Vector3.down, out raycastHit, 8f, RayMasks.WAYPOINT))
			{
				position = raycastHit.point + Vector3.up;
			}
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x0006A6D4 File Offset: 0x000688D4
		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("NotRunningErrorText"));
				return;
			}
			string[] componentsFromSerial = Parser.getComponentsFromSerial(parameter, '/');
			if (componentsFromSerial.Length < 1 || componentsFromSerial.Length > 2)
			{
				CommandWindow.LogError(this.localization.format("InvalidParameterErrorText"));
				return;
			}
			bool flag = componentsFromSerial.Length == 1;
			SteamPlayer steamPlayer;
			if (flag)
			{
				steamPlayer = PlayerTool.getSteamPlayer(executorID);
			}
			else
			{
				PlayerTool.tryGetSteamPlayer(componentsFromSerial[0], out steamPlayer);
			}
			if (steamPlayer == null)
			{
				CommandWindow.LogError(this.localization.format("NoPlayerErrorText", componentsFromSerial[0]));
				return;
			}
			SteamPlayer steamPlayer2;
			if (!PlayerTool.tryGetSteamPlayer(componentsFromSerial[flag ? 0 : 1], out steamPlayer2))
			{
				if (componentsFromSerial[flag ? 0 : 1].Equals(this.localization.format("WaypointCommand"), 3) && steamPlayer.player.quests.isMarkerPlaced)
				{
					Vector3 markerPosition = steamPlayer.player.quests.markerPosition;
					if (this.raycastFromSkyToPosition(ref markerPosition))
					{
						if (steamPlayer.player.teleportToLocation(markerPosition, steamPlayer.player.transform.rotation.eulerAngles.y))
						{
							CommandWindow.Log(this.localization.format("TeleportText", steamPlayer.playerID.playerName, this.localization.format("WaypointText")));
							return;
						}
						CommandWindow.LogError(this.localization.format("TeleportObstructed", steamPlayer.playerID.playerName, this.localization.format("WaypointText")));
						return;
					}
				}
				else if (componentsFromSerial[flag ? 0 : 1].Equals(this.localization.format("BedCommand"), 3))
				{
					if (steamPlayer.player.teleportToBed())
					{
						CommandWindow.Log(this.localization.format("TeleportText", steamPlayer.playerID.playerName, this.localization.format("BedText")));
						return;
					}
					CommandWindow.LogError(this.localization.format("TeleportObstructed", steamPlayer.playerID.playerName, this.localization.format("BedText")));
					return;
				}
				else
				{
					LocationDevkitNode locationDevkitNode = null;
					foreach (LocationDevkitNode locationDevkitNode2 in LocationDevkitNodeSystem.Get().GetAllNodes())
					{
						if (NameTool.checkNames(componentsFromSerial[flag ? 0 : 1], locationDevkitNode2.locationName))
						{
							locationDevkitNode = locationDevkitNode2;
							break;
						}
					}
					if (locationDevkitNode != null)
					{
						Vector3 position = locationDevkitNode.transform.position;
						this.raycastFromNearPosition(ref position);
						if (steamPlayer.player.teleportToLocation(position, steamPlayer.player.transform.rotation.eulerAngles.y))
						{
							CommandWindow.Log(this.localization.format("TeleportText", steamPlayer.playerID.playerName, locationDevkitNode.name));
							return;
						}
						CommandWindow.LogError(this.localization.format("TeleportObstructed", steamPlayer.playerID.playerName, locationDevkitNode.name));
						return;
					}
					else
					{
						CommandWindow.LogError(this.localization.format("NoLocationErrorText", componentsFromSerial[flag ? 0 : 1]));
					}
				}
				return;
			}
			if (steamPlayer2.player.movement.getVehicle() != null)
			{
				CommandWindow.LogError(this.localization.format("NoVehicleErrorText"));
				return;
			}
			if (steamPlayer.player.teleportToPlayer(steamPlayer2.player))
			{
				CommandWindow.Log(this.localization.format("TeleportText", steamPlayer.playerID.playerName, steamPlayer2.playerID.playerName));
				return;
			}
			CommandWindow.LogError(this.localization.format("TeleportObstructed", steamPlayer.playerID.playerName, steamPlayer2.playerID.playerName));
		}

		// Token: 0x06001D45 RID: 7493 RVA: 0x0006AAAC File Offset: 0x00068CAC
		public CommandTeleport(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("TeleportCommandText");
			this._info = this.localization.format("TeleportInfoText");
			this._help = this.localization.format("TeleportHelpText");
		}
	}
}
