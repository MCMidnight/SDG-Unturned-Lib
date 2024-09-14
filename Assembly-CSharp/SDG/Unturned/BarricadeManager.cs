using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SDG.NetPak;
using SDG.NetTransport;
using Steamworks;
using UnityEngine;
using Unturned.SystemEx;

namespace SDG.Unturned
{
	// Token: 0x0200053F RID: 1343
	public class BarricadeManager : SteamCaller
	{
		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060029F9 RID: 10745 RVA: 0x000B2B14 File Offset: 0x000B0D14
		// (remove) Token: 0x060029FA RID: 10746 RVA: 0x000B2B48 File Offset: 0x000B0D48
		public static event RepairBarricadeRequestHandler OnRepairRequested;

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060029FB RID: 10747 RVA: 0x000B2B7C File Offset: 0x000B0D7C
		// (remove) Token: 0x060029FC RID: 10748 RVA: 0x000B2BB0 File Offset: 0x000B0DB0
		public static event RepairedBarricadeHandler OnRepaired;

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060029FD RID: 10749 RVA: 0x000B2BE3 File Offset: 0x000B0DE3
		public static BarricadeManager instance
		{
			get
			{
				return BarricadeManager.manager;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000B2BEA File Offset: 0x000B0DEA
		// (set) Token: 0x060029FF RID: 10751 RVA: 0x000B2BF1 File Offset: 0x000B0DF1
		public static BarricadeRegion[,] regions { get; private set; }

		/// <summary>
		/// Exposed for Rocket transition to modules backwards compatibility.
		/// </summary>
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002A00 RID: 10752 RVA: 0x000B2BF9 File Offset: 0x000B0DF9
		// (set) Token: 0x06002A01 RID: 10753 RVA: 0x000B2C00 File Offset: 0x000B0E00
		public static BarricadeRegion[,] BarricadeRegions
		{
			get
			{
				return BarricadeManager.regions;
			}
			set
			{
				BarricadeManager.regions = value;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000B2C08 File Offset: 0x000B0E08
		// (set) Token: 0x06002A03 RID: 10755 RVA: 0x000B2C0F File Offset: 0x000B0E0F
		public static IReadOnlyList<VehicleBarricadeRegion> vehicleRegions { get; private set; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002A04 RID: 10756 RVA: 0x000B2C17 File Offset: 0x000B0E17
		[Obsolete("Please update to vehicleRegions instead (this property copies the list)")]
		public static List<BarricadeRegion> plants
		{
			get
			{
				if (BarricadeManager.backwardsCompatVehicleRegions == null)
				{
					BarricadeManager.backwardsCompatVehicleRegions = new List<BarricadeRegion>(BarricadeManager.vehicleRegions);
				}
				return BarricadeManager.backwardsCompatVehicleRegions;
			}
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000B2C34 File Offset: 0x000B0E34
		public static void getBarricadesInRadius(Vector3 center, float sqrRadius, List<RegionCoordinate> search, List<Transform> result)
		{
			if (BarricadeManager.regions == null)
			{
				return;
			}
			for (int i = 0; i < search.Count; i++)
			{
				RegionCoordinate regionCoordinate = search[i];
				if (BarricadeManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y] != null)
				{
					foreach (BarricadeDrop barricadeDrop in BarricadeManager.regions[(int)regionCoordinate.x, (int)regionCoordinate.y].drops)
					{
						Transform model = barricadeDrop.model;
						if (!(model == null) && (model.position - center).sqrMagnitude < sqrRadius)
						{
							result.Add(model);
						}
					}
				}
			}
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000B2D04 File Offset: 0x000B0F04
		public static void getBarricadesInRadius(Vector3 center, float sqrRadius, ushort plant, List<Transform> result)
		{
			if (BarricadeManager.vehicleRegions == null)
			{
				return;
			}
			if ((int)plant >= BarricadeManager.vehicleRegions.Count)
			{
				return;
			}
			foreach (BarricadeDrop barricadeDrop in BarricadeManager.vehicleRegions[(int)plant].drops)
			{
				Transform model = barricadeDrop.model;
				if (!(model == null) && (model.position - center).sqrMagnitude < sqrRadius)
				{
					result.Add(model);
				}
			}
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000B2D9C File Offset: 0x000B0F9C
		public static void getBarricadesInRadius(Vector3 center, float sqrRadius, List<Transform> result)
		{
			if (BarricadeManager.vehicleRegions == null)
			{
				return;
			}
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
			{
				if (vehicleBarricadeRegion != null && vehicleBarricadeRegion.drops.Count >= 1)
				{
					Transform parent = vehicleBarricadeRegion.parent;
					if (!(parent == null) && (parent.position - center).sqrMagnitude < 65536f)
					{
						foreach (BarricadeDrop barricadeDrop in vehicleBarricadeRegion.drops)
						{
							Transform model = barricadeDrop.model;
							if (!(model == null) && (model.position - center).sqrMagnitude < sqrRadius)
							{
								result.Add(model);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000B2EA0 File Offset: 0x000B10A0
		[Obsolete]
		public void tellBarricadeOwnerAndGroup(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ulong newOwner, ulong newGroup)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000B2EAC File Offset: 0x000B10AC
		public static void changeOwnerAndGroup(Transform transform, ulong newOwner, ulong newGroup)
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootTransform(transform);
			if (barricadeDrop == null)
			{
				return;
			}
			BarricadeDrop.SendOwnerAndGroup.InvokeAndLoopback(barricadeDrop.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), newOwner, newGroup);
			barricadeDrop.serversideData.owner = newOwner;
			barricadeDrop.serversideData.group = newGroup;
			BarricadeManager.sendHealthChanged(x, y, plant, barricadeDrop);
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000B2F20 File Offset: 0x000B1120
		public static void transformBarricade(Transform transform, Vector3 point, Quaternion rotation)
		{
			BarricadeDrop barricadeDrop = BarricadeDrop.FindByRootFast(transform);
			if (barricadeDrop == null)
			{
				return;
			}
			BarricadeDrop.SendTransformRequest.Invoke(barricadeDrop.GetNetId(), ENetReliability.Reliable, point, rotation);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000B2F4B File Offset: 0x000B114B
		[Obsolete]
		public void tellTransformBarricade(CSteamID steamID, byte x, byte y, ushort plant, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000B2F58 File Offset: 0x000B1158
		public static bool ServerSetBarricadeTransform(Transform transform, Vector3 position, Quaternion rotation)
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				return false;
			}
			BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootTransform(transform);
			if (barricadeDrop == null)
			{
				return false;
			}
			BarricadeManager.InternalSetBarricadeTransform(x, y, plant, barricadeDrop, position, rotation);
			return true;
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000B2F98 File Offset: 0x000B1198
		internal static void InternalSetBarricadeTransform(byte x, byte y, ushort plant, BarricadeDrop barricade, Vector3 point, Quaternion rotation)
		{
			BarricadeDrop.SendTransform.InvokeAndLoopback(barricade.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), x, y, plant, point, rotation);
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000B2FC5 File Offset: 0x000B11C5
		[Obsolete]
		public void askTransformBarricade(CSteamID steamID, byte x, byte y, ushort plant, uint instanceID, Vector3 point, byte angle_x, byte angle_y, byte angle_z)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000B2FD4 File Offset: 0x000B11D4
		[Obsolete]
		public static void poseMannequin(Transform transform, byte poseComp)
		{
			InteractableMannequin component = transform.GetComponent<InteractableMannequin>();
			if (component != null)
			{
				component.ClientSetPose(poseComp);
			}
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000B2FF8 File Offset: 0x000B11F8
		[Obsolete]
		public void tellPoseMannequin(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte poseComp)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000B3004 File Offset: 0x000B1204
		public static bool ServerSetMannequinPose(InteractableMannequin mannequin, byte poseComp)
		{
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(mannequin.transform, out x, out y, out plant, out barricadeRegion))
			{
				return false;
			}
			BarricadeManager.InternalSetMannequinPose(mannequin, x, y, plant, poseComp);
			return true;
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000B3033 File Offset: 0x000B1233
		internal static void InternalSetMannequinPose(InteractableMannequin mannequin, byte x, byte y, ushort plant, byte poseComp)
		{
			InteractableMannequin.SendPose.InvokeAndLoopback(mannequin.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), poseComp);
			mannequin.rebuildState();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000B3056 File Offset: 0x000B1256
		[Obsolete]
		public void askPoseMannequin(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte poseComp)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000B3062 File Offset: 0x000B1262
		[Obsolete]
		public void tellUpdateMannequin(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte[] state)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000B3070 File Offset: 0x000B1270
		[Obsolete]
		public static void updateMannequin(Transform transform, EMannequinUpdateMode updateMode)
		{
			InteractableMannequin component = transform.GetComponent<InteractableMannequin>();
			if (component != null)
			{
				component.ClientRequestUpdate(updateMode);
			}
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000B3094 File Offset: 0x000B1294
		[Obsolete]
		public void askUpdateMannequin(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte mode)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000B30A0 File Offset: 0x000B12A0
		[Obsolete]
		public static void rotDisplay(Transform transform, byte rotComp)
		{
			InteractableStorage component = transform.GetComponent<InteractableStorage>();
			if (component != null)
			{
				component.ClientSetDisplayRotation(rotComp);
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000B30C4 File Offset: 0x000B12C4
		[Obsolete]
		public void tellRotDisplay(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte rotComp)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000B30D0 File Offset: 0x000B12D0
		[Obsolete]
		public void askRotDisplay(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte rotComp)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000B30DC File Offset: 0x000B12DC
		[Obsolete]
		public void tellBarricadeHealth(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte hp)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000B30E8 File Offset: 0x000B12E8
		public static void salvageBarricade(Transform transform)
		{
			BarricadeDrop barricadeDrop = BarricadeManager.FindBarricadeByRootTransform(transform);
			if (barricadeDrop != null)
			{
				BarricadeDrop.SendSalvageRequest.Invoke(barricadeDrop.GetNetId(), ENetReliability.Reliable);
			}
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000B3110 File Offset: 0x000B1310
		[Obsolete]
		public void askSalvageBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000B311C File Offset: 0x000B131C
		[Obsolete]
		public void tellTank(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ushort amount)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000B3128 File Offset: 0x000B1328
		[Obsolete("Moved into InteractableTank.ServerSetAmount")]
		public static void updateTank(Transform transform, ushort amount)
		{
			InteractableTank component = transform.GetComponent<InteractableTank>();
			if (component != null)
			{
				component.ServerSetAmount(amount);
			}
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000B314C File Offset: 0x000B134C
		[Obsolete]
		public static void updateSign(Transform transform, string newText)
		{
			InteractableSign component = transform.GetComponent<InteractableSign>();
			if (component != null)
			{
				component.ClientSetText(newText);
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000B3170 File Offset: 0x000B1370
		[Obsolete]
		public void tellUpdateSign(CSteamID steamID, byte x, byte y, ushort plant, ushort index, string newText)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x000B317C File Offset: 0x000B137C
		public static bool ServerSetSignText(InteractableSign sign, string newText)
		{
			if (sign == null)
			{
				throw new ArgumentNullException("sign");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(sign.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			string text = sign.trimText(newText);
			if (!sign.isTextValid(text))
			{
				return false;
			}
			BarricadeManager.ServerSetSignTextInternal(sign, region, x, y, plant, text);
			return true;
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x000B31D8 File Offset: 0x000B13D8
		internal static void ServerSetSignTextInternal(InteractableSign sign, BarricadeRegion region, byte x, byte y, ushort plant, string trimmedText)
		{
			if (trimmedText == null)
			{
				trimmedText = string.Empty;
			}
			InteractableSign.SendChangeText.InvokeAndLoopback(sign.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), trimmedText);
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(sign.transform);
			Array state = barricadeDrop.serversideData.barricade.state;
			byte[] bytes = Encoding.UTF8.GetBytes(trimmedText);
			byte[] array = new byte[17 + bytes.Length];
			Buffer.BlockCopy(state, 0, array, 0, 16);
			array[16] = (byte)bytes.Length;
			if (bytes.Length != 0)
			{
				Buffer.BlockCopy(bytes, 0, array, 17, bytes.Length);
			}
			barricadeDrop.serversideData.barricade.state = array;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x000B3274 File Offset: 0x000B1474
		[Obsolete]
		public void askUpdateSign(CSteamID steamID, byte x, byte y, ushort plant, ushort index, string newText)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000B3280 File Offset: 0x000B1480
		[Obsolete]
		public static void updateStereoTrack(Transform transform, Guid newTrack)
		{
			InteractableStereo component = transform.GetComponent<InteractableStereo>();
			if (component != null)
			{
				component.ClientSetTrack(newTrack);
			}
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x000B32A4 File Offset: 0x000B14A4
		public static bool ServerSetStereoTrack(InteractableStereo stereo, Guid track)
		{
			if (stereo == null)
			{
				throw new ArgumentNullException("stereo");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(stereo.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetStereoTrackInternal(stereo, x, y, plant, region, track);
			return true;
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x000B32E8 File Offset: 0x000B14E8
		internal static void ServerSetStereoTrackInternal(InteractableStereo stereo, byte x, byte y, ushort plant, BarricadeRegion region, Guid track)
		{
			InteractableStereo.SendTrack.InvokeAndLoopback(stereo.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), track);
			byte[] state = region.FindBarricadeByRootFast(stereo.transform).serversideData.barricade.state;
			GuidBuffer guidBuffer = new GuidBuffer(track);
			guidBuffer.Write(state, 0);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x000B333F File Offset: 0x000B153F
		[Obsolete]
		public void tellUpdateStereoTrack(CSteamID steamID, byte x, byte y, ushort plant, ushort index, Guid newTrack)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x000B334B File Offset: 0x000B154B
		[Obsolete]
		public void askUpdateStereoTrack(CSteamID steamID, byte x, byte y, ushort plant, ushort index, Guid newTrack)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000B3358 File Offset: 0x000B1558
		[Obsolete]
		public static void updateStereoVolume(Transform transform, byte newVolume)
		{
			InteractableStereo component = transform.GetComponent<InteractableStereo>();
			if (component != null)
			{
				component.ClientSetVolume(newVolume);
			}
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x000B337C File Offset: 0x000B157C
		[Obsolete]
		public void tellUpdateStereoVolume(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte newVolume)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x000B3388 File Offset: 0x000B1588
		[Obsolete]
		public void askUpdateStereoVolume(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte newVolume)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x000B3394 File Offset: 0x000B1594
		[Obsolete]
		public static void transferLibrary(Transform transform, byte transaction, uint delta)
		{
			InteractableLibrary component = transform.GetComponent<InteractableLibrary>();
			if (component != null)
			{
				component.ClientTransfer(transaction, delta);
			}
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x000B33B9 File Offset: 0x000B15B9
		[Obsolete]
		public void tellTransferLibrary(CSteamID steamID, byte x, byte y, ushort plant, ushort index, uint newAmount)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x000B33C5 File Offset: 0x000B15C5
		[Obsolete]
		public void askTransferLibrary(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte transaction, uint delta)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x000B33D4 File Offset: 0x000B15D4
		[Obsolete]
		public static void toggleSafezone(Transform transform)
		{
			InteractableSafezone component = transform.GetComponent<InteractableSafezone>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x000B33F8 File Offset: 0x000B15F8
		public static bool ServerSetSafezonePowered(InteractableSafezone safezone, bool isPowered)
		{
			if (safezone == null)
			{
				throw new ArgumentNullException("safezone");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(safezone.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetSafezonePoweredInternal(safezone, x, y, plant, region, isPowered);
			return true;
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x000B343C File Offset: 0x000B163C
		internal static void ServerSetSafezonePoweredInternal(InteractableSafezone safezone, byte x, byte y, ushort plant, BarricadeRegion region, bool isPowered)
		{
			InteractableSafezone.SendPowered.InvokeAndLoopback(safezone.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isPowered);
			region.FindBarricadeByRootFast(safezone.transform).serversideData.barricade.state[0] = (safezone.isPowered ? 1 : 0);
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x000B348F File Offset: 0x000B168F
		[Obsolete]
		public void tellToggleSafezone(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000B349B File Offset: 0x000B169B
		[Obsolete]
		public void askToggleSafezone(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000B34A8 File Offset: 0x000B16A8
		[Obsolete]
		public static void toggleOxygenator(Transform transform)
		{
			InteractableOxygenator component = transform.GetComponent<InteractableOxygenator>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x000B34CC File Offset: 0x000B16CC
		public static bool ServerSetOxygenatorPowered(InteractableOxygenator oxygenator, bool isPowered)
		{
			if (oxygenator == null)
			{
				throw new ArgumentNullException("oxygenator");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(oxygenator.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetOxygenatorPoweredInternal(oxygenator, x, y, plant, region, isPowered);
			return true;
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x000B3510 File Offset: 0x000B1710
		internal static void ServerSetOxygenatorPoweredInternal(InteractableOxygenator oxygenator, byte x, byte y, ushort plant, BarricadeRegion region, bool isPowered)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(oxygenator.transform);
			InteractableOxygenator.SendPowered.InvokeAndLoopback(oxygenator.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isPowered);
			barricadeDrop.serversideData.barricade.state[0] = (oxygenator.isPowered ? 1 : 0);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x000B3563 File Offset: 0x000B1763
		[Obsolete]
		public void tellToggleOxygenator(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000B356F File Offset: 0x000B176F
		[Obsolete]
		public void askToggleOxygenator(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x000B357C File Offset: 0x000B177C
		[Obsolete]
		public static void toggleSpot(Transform transform)
		{
			InteractableSpot component = transform.GetComponent<InteractableSpot>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000B35A0 File Offset: 0x000B17A0
		public static bool ServerSetSpotPowered(InteractableSpot spot, bool isPowered)
		{
			if (spot == null)
			{
				throw new ArgumentNullException("spot");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(spot.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetSpotPoweredInternal(spot, x, y, plant, region, isPowered);
			return true;
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000B35E4 File Offset: 0x000B17E4
		internal static void ServerSetSpotPoweredInternal(InteractableSpot spot, byte x, byte y, ushort plant, BarricadeRegion region, bool isPowered)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(spot.transform);
			InteractableSpot.SendPowered.InvokeAndLoopback(spot.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isPowered);
			barricadeDrop.serversideData.barricade.state[0] = (spot.isPowered ? 1 : 0);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x000B3637 File Offset: 0x000B1837
		[Obsolete]
		public void tellToggleSpot(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000B3643 File Offset: 0x000B1843
		[Obsolete]
		public void askToggleSpot(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000B3650 File Offset: 0x000B1850
		public static void sendFuel(Transform transform, ushort fuel)
		{
			InteractableGenerator component = transform.GetComponent<InteractableGenerator>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				InteractableGenerator.SendFuel.InvokeAndLoopback(component.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), fuel);
			}
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000B3697 File Offset: 0x000B1897
		[Obsolete]
		public void tellFuel(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ushort fuel)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000B36A4 File Offset: 0x000B18A4
		[Obsolete]
		public static void toggleGenerator(Transform transform)
		{
			InteractableGenerator component = transform.GetComponent<InteractableGenerator>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000B36C8 File Offset: 0x000B18C8
		public static bool ServerSetGeneratorPowered(InteractableGenerator generator, bool isPowered)
		{
			if (generator == null)
			{
				throw new ArgumentNullException("generator");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(generator.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetGeneratorPoweredInternal(generator, x, y, plant, region, isPowered);
			return true;
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000B370C File Offset: 0x000B190C
		internal static void ServerSetGeneratorPoweredInternal(InteractableGenerator generator, byte x, byte y, ushort plant, BarricadeRegion region, bool isPowered)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(generator.transform);
			InteractableGenerator.SendPowered.InvokeAndLoopback(generator.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isPowered);
			barricadeDrop.serversideData.barricade.state[0] = (generator.isPowered ? 1 : 0);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000B375F File Offset: 0x000B195F
		[Obsolete]
		public void tellToggleGenerator(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isPowered)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000B376B File Offset: 0x000B196B
		[Obsolete]
		public void askToggleGenerator(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x000B3778 File Offset: 0x000B1978
		[Obsolete]
		public static void toggleFire(Transform transform)
		{
			InteractableFire component = transform.GetComponent<InteractableFire>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000B379C File Offset: 0x000B199C
		public static bool ServerSetFireLit(InteractableFire fire, bool isLit)
		{
			if (fire == null)
			{
				throw new ArgumentNullException("fire");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(fire.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetFireLitInternal(fire, x, y, plant, region, isLit);
			return true;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000B37E0 File Offset: 0x000B19E0
		internal static void ServerSetFireLitInternal(InteractableFire fire, byte x, byte y, ushort plant, BarricadeRegion region, bool isLit)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(fire.transform);
			InteractableFire.SendLit.InvokeAndLoopback(fire.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isLit);
			barricadeDrop.serversideData.barricade.state[0] = (fire.isLit ? 1 : 0);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x000B3833 File Offset: 0x000B1A33
		[Obsolete]
		public void tellToggleFire(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isLit)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x000B383F File Offset: 0x000B1A3F
		[Obsolete]
		public void askToggleFire(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x000B384C File Offset: 0x000B1A4C
		[Obsolete]
		public static void toggleOven(Transform transform)
		{
			InteractableOven component = transform.GetComponent<InteractableOven>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000B3870 File Offset: 0x000B1A70
		public static bool ServerSetOvenLit(InteractableOven oven, bool isLit)
		{
			if (oven == null)
			{
				throw new ArgumentNullException("oven");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(oven.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetOvenLitInternal(oven, x, y, plant, region, isLit);
			return true;
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000B38B4 File Offset: 0x000B1AB4
		internal static void ServerSetOvenLitInternal(InteractableOven oven, byte x, byte y, ushort plant, BarricadeRegion region, bool isLit)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(oven.transform);
			InteractableOven.SendLit.InvokeAndLoopback(oven.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isLit);
			barricadeDrop.serversideData.barricade.state[0] = (oven.isLit ? 1 : 0);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000B3907 File Offset: 0x000B1B07
		[Obsolete]
		public void tellToggleOven(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isLit)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000B3913 File Offset: 0x000B1B13
		[Obsolete]
		public void askToggleOven(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x000B3920 File Offset: 0x000B1B20
		[Obsolete]
		public static void farm(Transform transform)
		{
			InteractableFarm component = transform.GetComponent<InteractableFarm>();
			if (component != null)
			{
				component.ClientHarvest();
			}
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x000B3943 File Offset: 0x000B1B43
		[Obsolete]
		public void tellFarm(CSteamID steamID, byte x, byte y, ushort plant, ushort index, uint planted)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x000B394F File Offset: 0x000B1B4F
		[Obsolete]
		public void askFarm(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x000B395C File Offset: 0x000B1B5C
		public static void updateFarm(Transform transform, uint planted, bool shouldSend)
		{
			InteractableFarm component = transform.GetComponent<InteractableFarm>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				if (shouldSend)
				{
					InteractableFarm.SendPlanted.InvokeAndLoopback(component.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), planted);
				}
				BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootFast(transform);
				BitConverter.GetBytes(planted).CopyTo(barricadeDrop.serversideData.barricade.state, 0);
			}
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x000B39CD File Offset: 0x000B1BCD
		[Obsolete]
		public void tellOil(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ushort fuel)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x000B39DC File Offset: 0x000B1BDC
		public static void sendOil(Transform transform, ushort fuel)
		{
			InteractableOil component = transform.GetComponent<InteractableOil>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				InteractableOil.SendFuel.InvokeAndLoopback(component.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), fuel);
			}
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x000B3A23 File Offset: 0x000B1C23
		[Obsolete]
		public void tellRainBarrel(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isFull)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x000B3A30 File Offset: 0x000B1C30
		public static void updateRainBarrel(Transform transform, bool isFull, bool shouldSend)
		{
			InteractableRainBarrel component = transform.GetComponent<InteractableRainBarrel>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				if (shouldSend)
				{
					InteractableRainBarrel.SendFull.InvokeAndLoopback(component.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isFull);
				}
				barricadeRegion.FindBarricadeByRootFast(transform).serversideData.barricade.state[0] = (isFull ? 1 : 0);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x000B3A9C File Offset: 0x000B1C9C
		public static void sendStorageDisplay(Transform transform, Item item, ushort skin, ushort mythic, string tags, string dynamicProps)
		{
			InteractableStorage component = transform.GetComponent<InteractableStorage>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				ushort arg;
				byte arg2;
				byte[] arg3;
				if (item != null)
				{
					arg = item.id;
					arg2 = item.quality;
					arg3 = item.state;
				}
				else
				{
					arg = 0;
					arg2 = 0;
					arg3 = new byte[0];
				}
				InteractableStorage.SendDisplay.Invoke(component.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherClientConnections(x, y, plant), arg, arg2, arg3, skin, mythic, tags, dynamicProps);
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x000B3B19 File Offset: 0x000B1D19
		[Obsolete]
		public void tellStorageDisplay(CSteamID steamID, byte x, byte y, ushort plant, ushort index, ushort id, byte quality, byte[] state, ushort skin, ushort mythic, string tags, string dynamicProps)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x000B3B28 File Offset: 0x000B1D28
		[Obsolete]
		public static void storeStorage(Transform transform, bool quickGrab)
		{
			InteractableStorage component = transform.GetComponent<InteractableStorage>();
			if (component != null)
			{
				component.ClientInteract(quickGrab);
			}
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000B3B4C File Offset: 0x000B1D4C
		[Obsolete]
		public void askStoreStorage(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool quickGrab)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x000B3B58 File Offset: 0x000B1D58
		[Obsolete]
		public static void toggleDoor(Transform transform)
		{
			InteractableDoor component = transform.GetComponent<InteractableDoor>();
			if (component != null)
			{
				component.ClientToggle();
			}
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000B3B7B File Offset: 0x000B1D7B
		[Obsolete]
		public void tellToggleDoor(CSteamID steamID, byte x, byte y, ushort plant, ushort index, bool isOpen)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000B3B88 File Offset: 0x000B1D88
		public static bool ServerSetDoorOpen(InteractableDoor door, bool isOpen)
		{
			if (door == null)
			{
				throw new ArgumentNullException("door");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(door.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetDoorOpenInternal(door, x, y, plant, region, isOpen);
			return true;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000B3BCC File Offset: 0x000B1DCC
		internal static void ServerSetDoorOpenInternal(InteractableDoor door, byte x, byte y, ushort plant, BarricadeRegion region, bool isOpen)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(door.transform);
			InteractableDoor.SendOpen.InvokeAndLoopback(door.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), isOpen);
			barricadeDrop.serversideData.barricade.state[16] = (isOpen ? 1 : 0);
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000B3C1C File Offset: 0x000B1E1C
		[Obsolete]
		public void askToggleDoor(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000B3C28 File Offset: 0x000B1E28
		private static bool tryGetBedInRegion(BarricadeRegion region, CSteamID owner, ref Vector3 point, ref byte angle)
		{
			foreach (BarricadeDrop barricadeDrop in region.drops)
			{
				if (barricadeDrop.serversideData.barricade.state.Length != 0)
				{
					InteractableBed interactableBed = barricadeDrop.interactable as InteractableBed;
					if (interactableBed != null && interactableBed.owner == owner && Level.checkSafeIncludingClipVolumes(interactableBed.transform.position))
					{
						point = interactableBed.transform.position;
						float modelYaw = HousingConnections.GetModelYaw(interactableBed.transform);
						angle = MeasurementTool.angleToByte(modelYaw + 90f);
						int num = Physics.OverlapCapsuleNonAlloc(point + new Vector3(0f, PlayerStance.RADIUS, 0f), point + new Vector3(0f, 2.5f - PlayerStance.RADIUS, 0f), PlayerStance.RADIUS, BarricadeManager.checkColliders, RayMasks.BLOCK_STANCE, QueryTriggerInteraction.Ignore);
						for (int i = 0; i < num; i++)
						{
							if (BarricadeManager.checkColliders[i].gameObject != interactableBed.gameObject)
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000B3D9C File Offset: 0x000B1F9C
		public static bool tryGetBed(CSteamID owner, out Vector3 point, out byte angle)
		{
			point = Vector3.zero;
			angle = 0;
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					if (BarricadeManager.tryGetBedInRegion(BarricadeManager.regions[(int)b, (int)b2], owner, ref point, ref angle))
					{
						return true;
					}
				}
			}
			ushort num = 0;
			while ((int)num < BarricadeManager.vehicleRegions.Count)
			{
				if (BarricadeManager.tryGetBedInRegion(BarricadeManager.vehicleRegions[(int)num], owner, ref point, ref angle))
				{
					return true;
				}
				num += 1;
			}
			return false;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000B3E20 File Offset: 0x000B2020
		private static bool UnclaimBedsInRegion(CSteamID owner, BarricadeRegion region, byte x, byte y, ushort plant)
		{
			ushort num = 0;
			while ((int)num < region.drops.Count)
			{
				BarricadeDrop barricadeDrop = region.drops[(int)num];
				if (barricadeDrop.serversideData.barricade.state.Length != 0)
				{
					InteractableBed interactableBed = barricadeDrop.interactable as InteractableBed;
					if (interactableBed != null && interactableBed.owner == owner)
					{
						InteractableBed.SendClaim.InvokeAndLoopback(interactableBed.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), CSteamID.Nil);
						BitConverter.GetBytes(interactableBed.owner.m_SteamID).CopyTo(barricadeDrop.serversideData.barricade.state, 0);
						return true;
					}
				}
				num += 1;
			}
			return false;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000B3ED8 File Offset: 0x000B20D8
		public static void unclaimBeds(CSteamID owner)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					BarricadeRegion region = BarricadeManager.regions[(int)b, (int)b2];
					if (BarricadeManager.UnclaimBedsInRegion(owner, region, b, b2, 65535))
					{
						return;
					}
				}
			}
			ushort num = 0;
			while ((int)num < BarricadeManager.vehicleRegions.Count)
			{
				BarricadeRegion region2 = BarricadeManager.vehicleRegions[(int)num];
				if (BarricadeManager.UnclaimBedsInRegion(owner, region2, 255, 255, num))
				{
					return;
				}
				num += 1;
			}
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000B3F60 File Offset: 0x000B2160
		[Obsolete]
		public static void claimBed(Transform transform)
		{
			InteractableBed component = transform.GetComponent<InteractableBed>();
			if (component != null)
			{
				component.ClientClaim();
			}
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000B3F83 File Offset: 0x000B2183
		[Obsolete]
		public void tellClaimBed(CSteamID steamID, byte x, byte y, ushort plant, ushort index, CSteamID owner)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000B3F8F File Offset: 0x000B218F
		[Obsolete]
		public void askClaimBed(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000B3F9C File Offset: 0x000B219C
		public static bool ServerUnclaimBed(InteractableBed bed)
		{
			if (bed == null)
			{
				throw new ArgumentNullException("bed");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(bed.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.ServerSetBedOwnerInternal(bed, x, y, plant, region, CSteamID.Nil);
			return true;
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000B3FE4 File Offset: 0x000B21E4
		public static bool ServerClaimBedForPlayer(InteractableBed bed, Player player)
		{
			if (bed == null)
			{
				throw new ArgumentNullException("bed");
			}
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion region;
			if (!BarricadeManager.tryGetRegion(bed.transform, out x, out y, out plant, out region))
			{
				return false;
			}
			BarricadeManager.unclaimBeds(player.channel.owner.playerID.steamID);
			BarricadeManager.ServerSetBedOwnerInternal(bed, x, y, plant, region, player.channel.owner.playerID.steamID);
			return true;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000B406C File Offset: 0x000B226C
		internal static void ServerSetBedOwnerInternal(InteractableBed bed, byte x, byte y, ushort plant, BarricadeRegion region, CSteamID steamID)
		{
			BarricadeDrop barricadeDrop = region.FindBarricadeByRootFast(bed.transform);
			InteractableBed.SendClaim.InvokeAndLoopback(bed.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), steamID);
			BitConverter.GetBytes(bed.owner.m_SteamID).CopyTo(barricadeDrop.serversideData.barricade.state, 0);
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000B40C8 File Offset: 0x000B22C8
		[Obsolete]
		public void tellShootSentry(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x000B40D4 File Offset: 0x000B22D4
		public static void sendShootSentry(Transform transform)
		{
			InteractableSentry component = transform.GetComponent<InteractableSentry>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				InteractableSentry.SendShoot.InvokeAndLoopback(component.GetNetId(), ENetReliability.Unreliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant));
			}
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x000B411A File Offset: 0x000B231A
		[Obsolete]
		public void tellAlertSentry(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte yaw, byte pitch)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x000B4128 File Offset: 0x000B2328
		public static void sendAlertSentry(Transform transform, float yaw, float pitch)
		{
			InteractableSentry component = transform.GetComponent<InteractableSentry>();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (component != null && BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				InteractableSentry.SendAlert.InvokeAndLoopback(component.GetNetId(), ENetReliability.Unreliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), MeasurementTool.angleToByte(yaw), MeasurementTool.angleToByte(pitch));
			}
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x000B417C File Offset: 0x000B237C
		public static void damage(Transform transform, float damage, float times, bool armor, CSteamID instigatorSteamID = default(CSteamID), EDamageOrigin damageOrigin = EDamageOrigin.Unknown)
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootTransform(transform);
			if (barricadeDrop == null)
			{
				return;
			}
			if (!barricadeDrop.serversideData.barricade.isDead)
			{
				ItemBarricadeAsset asset = barricadeDrop.asset;
				if (asset == null)
				{
					return;
				}
				if (!asset.canBeDamaged)
				{
					return;
				}
				if (armor)
				{
					times *= Provider.modeConfigData.Barricades.getArmorMultiplier(asset.armorTier);
				}
				ushort num = (ushort)(damage * times);
				bool flag = true;
				DamageBarricadeRequestHandler damageBarricadeRequestHandler = BarricadeManager.onDamageBarricadeRequested;
				if (damageBarricadeRequestHandler != null)
				{
					damageBarricadeRequestHandler(instigatorSteamID, transform, ref num, ref flag, damageOrigin);
				}
				if (!flag || num < 1)
				{
					return;
				}
				barricadeDrop.serversideData.barricade.askDamage(num);
				if (barricadeDrop.serversideData.barricade.isDead)
				{
					EffectAsset effectAsset = asset.FindExplosionEffectAsset();
					if (effectAsset != null)
					{
						EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
						{
							position = transform.position + Vector3.down * asset.offset,
							relevantDistance = EffectManager.MEDIUM
						});
					}
					asset.SpawnItemDropsOnDestroy(transform.position);
					BarricadeManager.destroyBarricade(barricadeDrop, x, y, plant);
					return;
				}
				BarricadeManager.sendHealthChanged(x, y, plant, barricadeDrop);
			}
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x000B42B8 File Offset: 0x000B24B8
		[Obsolete("Please replace the methods which take an index")]
		public static void destroyBarricade(BarricadeRegion region, byte x, byte y, ushort plant, ushort index)
		{
			BarricadeManager.destroyBarricade(region.drops[(int)index], x, y, plant);
		}

		/// <summary>
		/// Remove barricade instance on server and client.
		/// </summary>
		// Token: 0x06002A70 RID: 10864 RVA: 0x000B42D0 File Offset: 0x000B24D0
		public static void destroyBarricade(BarricadeDrop barricade, byte x, byte y, ushort plant)
		{
			ThreadUtil.assertIsGameThread();
			BarricadeRegion barricadeRegion;
			if (BarricadeManager.tryGetRegion(x, y, plant, out barricadeRegion))
			{
				barricadeRegion.barricades.Remove(barricade.serversideData);
				BarricadeManager.SendDestroyBarricade.InvokeAndLoopback(ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), barricade.GetNetId());
			}
		}

		/// <summary>
		/// Used by ownership change and damaged event to tell relevant clients the new health.
		/// </summary>
		// Token: 0x06002A71 RID: 10865 RVA: 0x000B431C File Offset: 0x000B251C
		private static void sendHealthChanged(byte x, byte y, ushort plant, BarricadeDrop barricade)
		{
			if (plant == 65535)
			{
				BarricadeDrop.SendHealth.Invoke(barricade.GetNetId(), ENetReliability.Unreliable, Provider.GatherClientConnectionsMatchingPredicate((SteamPlayer client) => client.player != null && Regions.checkArea(x, y, client.player.movement.region_x, client.player.movement.region_y, BarricadeManager.BARRICADE_REGIONS) && OwnershipTool.checkToggle(client.playerID.steamID, barricade.serversideData.owner, client.player.quests.groupID, barricade.serversideData.group)), (byte)((float)barricade.serversideData.barricade.health / (float)barricade.serversideData.barricade.asset.health * 100f));
				return;
			}
			BarricadeDrop.SendHealth.Invoke(barricade.GetNetId(), ENetReliability.Unreliable, Provider.GatherClientConnectionsMatchingPredicate((SteamPlayer client) => OwnershipTool.checkToggle(client.playerID.steamID, barricade.serversideData.owner, client.player.quests.groupID, barricade.serversideData.group)), (byte)((float)barricade.serversideData.barricade.health / (float)barricade.serversideData.barricade.asset.health * 100f));
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x000B4410 File Offset: 0x000B2610
		public static void repair(Transform transform, float damage, float times)
		{
			BarricadeManager.repair(transform, damage, times, default(CSteamID));
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000B4430 File Offset: 0x000B2630
		public static void repair(Transform transform, float damage, float times, CSteamID instigatorSteamID = default(CSteamID))
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				return;
			}
			BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootTransform(transform);
			if (barricadeDrop == null)
			{
				return;
			}
			if (!barricadeDrop.serversideData.barricade.isDead && !barricadeDrop.serversideData.barricade.isRepaired)
			{
				float value = damage * times;
				bool flag = true;
				RepairBarricadeRequestHandler onRepairRequested = BarricadeManager.OnRepairRequested;
				if (onRepairRequested != null)
				{
					onRepairRequested(instigatorSteamID, transform, ref value, ref flag);
				}
				ushort num = MathfEx.RoundAndClampToUShort(value);
				if (!flag || num < 1)
				{
					return;
				}
				barricadeDrop.serversideData.barricade.askRepair(num);
				BarricadeManager.sendHealthChanged(x, y, plant, barricadeDrop);
				RepairedBarricadeHandler onRepaired = BarricadeManager.OnRepaired;
				if (onRepaired == null)
				{
					return;
				}
				onRepaired(instigatorSteamID, transform, (float)num);
			}
		}

		/// <summary>
		/// Legacy function for UseableBarricade.
		/// </summary>
		// Token: 0x06002A74 RID: 10868 RVA: 0x000B44EC File Offset: 0x000B26EC
		public static Transform dropBarricade(Barricade barricade, Transform hit, Vector3 point, float angle_x, float angle_y, float angle_z, ulong owner, ulong group)
		{
			ThreadUtil.assertIsGameThread();
			if (barricade.asset == null)
			{
				return null;
			}
			bool flag = true;
			DeployBarricadeRequestHandler deployBarricadeRequestHandler = BarricadeManager.onDeployBarricadeRequested;
			if (deployBarricadeRequestHandler != null)
			{
				deployBarricadeRequestHandler(barricade, barricade.asset, hit, ref point, ref angle_x, ref angle_y, ref angle_z, ref owner, ref group, ref flag);
			}
			if (!flag)
			{
				return null;
			}
			Quaternion rotation = BarricadeManager.getRotation(barricade.asset, angle_x, angle_y, angle_z);
			if (hit != null && hit.transform.CompareTag("Vehicle"))
			{
				return BarricadeManager.dropPlantedBarricade(hit, barricade, point, rotation, owner, group);
			}
			return BarricadeManager.dropNonPlantedBarricade(barricade, point, rotation, owner, group);
		}

		/// <summary>
		/// Common code between dropping barricade onto vehicle or into world.
		/// </summary>
		// Token: 0x06002A75 RID: 10869 RVA: 0x000B457C File Offset: 0x000B277C
		private static Transform dropBarricadeIntoRegionInternal(BarricadeRegion region, Barricade barricade, Vector3 point, Quaternion rotation, ulong owner, ulong group)
		{
			uint newInstanceID = BarricadeManager.instanceCount += 1U;
			BarricadeData barricadeData = new BarricadeData(barricade, point, rotation, owner, group, Provider.time, newInstanceID);
			NetId netId = NetIdRegistry.ClaimBlock(3U);
			Transform transform = BarricadeManager.manager.spawnBarricade(region, barricade.asset.GUID, barricade.state, barricadeData.point, rotation, 100, barricadeData.owner, barricadeData.group, netId);
			if (transform != null)
			{
				region.drops.GetTail<BarricadeDrop>().serversideData = barricadeData;
				region.barricades.Add(barricadeData);
			}
			return transform;
		}

		/// <summary>
		/// Spawn a new barricade attached to a vehicle and replicate it.
		/// </summary>
		// Token: 0x06002A76 RID: 10870 RVA: 0x000B460C File Offset: 0x000B280C
		public static Transform dropPlantedBarricade(Transform parent, Barricade barricade, Vector3 point, Quaternion rotation, ulong owner, ulong group)
		{
			ThreadUtil.assertIsGameThread();
			VehicleBarricadeRegion vehicleBarricadeRegion = BarricadeManager.FindVehicleRegionByTransform(parent);
			if (vehicleBarricadeRegion == null)
			{
				return null;
			}
			Transform transform = BarricadeManager.dropBarricadeIntoRegionInternal(vehicleBarricadeRegion, barricade, point, rotation, owner, group);
			if (transform != null)
			{
				BarricadeDrop tail = vehicleBarricadeRegion.drops.GetTail<BarricadeDrop>();
				BarricadeData serversideData = tail.serversideData;
				BarricadeManager.SendSingleBarricade.Invoke(ENetReliability.Reliable, Provider.GatherRemoteClientConnections(), vehicleBarricadeRegion._netId, barricade.asset.GUID, barricade.state, serversideData.point, serversideData.rotation, serversideData.owner, serversideData.group, tail.GetNetId());
				BarricadeSpawnedHandler barricadeSpawnedHandler = BarricadeManager.onBarricadeSpawned;
				if (barricadeSpawnedHandler == null)
				{
					return transform;
				}
				barricadeSpawnedHandler(vehicleBarricadeRegion, tail);
			}
			return transform;
		}

		/// <summary>
		/// Spawn a new barricade and replicate it.
		/// </summary>
		// Token: 0x06002A77 RID: 10871 RVA: 0x000B46AC File Offset: 0x000B28AC
		public static Transform dropNonPlantedBarricade(Barricade barricade, Vector3 point, Quaternion rotation, ulong owner, ulong group)
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			if (!Regions.tryGetCoordinate(point, out x, out y))
			{
				return null;
			}
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(x, y, 65535, out barricadeRegion))
			{
				return null;
			}
			Transform transform = BarricadeManager.dropBarricadeIntoRegionInternal(barricadeRegion, barricade, point, rotation, owner, group);
			if (transform != null)
			{
				BarricadeDrop tail = barricadeRegion.drops.GetTail<BarricadeDrop>();
				BarricadeData serversideData = tail.serversideData;
				BarricadeManager.SendSingleBarricade.Invoke(ENetReliability.Reliable, Regions.GatherRemoteClientConnections(x, y, BarricadeManager.BARRICADE_REGIONS), NetId.INVALID, barricade.asset.GUID, barricade.state, serversideData.point, serversideData.rotation, serversideData.owner, serversideData.group, tail.GetNetId());
				BarricadeSpawnedHandler barricadeSpawnedHandler = BarricadeManager.onBarricadeSpawned;
				if (barricadeSpawnedHandler == null)
				{
					return transform;
				}
				barricadeSpawnedHandler(barricadeRegion, tail);
			}
			return transform;
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000B4767 File Offset: 0x000B2967
		[Obsolete]
		public void tellTakeBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort index)
		{
			throw new NotSupportedException("Removed during barricade NetId rewrite");
		}

		/// <summary>
		/// Not an instance method because structure might not exist yet, in which case we cancel instantiation.
		/// </summary>
		// Token: 0x06002A79 RID: 10873 RVA: 0x000B4774 File Offset: 0x000B2974
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveDestroyBarricade(in ClientInvocationContext context, NetId netId)
		{
			BarricadeDrop barricadeDrop = NetIdRegistry.Get<BarricadeDrop>(netId);
			if (barricadeDrop == null)
			{
				BarricadeManager.CancelInstantiationByNetId(netId);
				return;
			}
			byte b;
			byte b2;
			ushort num;
			BarricadeRegion barricadeRegion;
			if (!BarricadeManager.tryGetRegion(barricadeDrop.model, out b, out b2, out num, out barricadeRegion))
			{
				return;
			}
			barricadeDrop.CustomDestroy();
			barricadeRegion.drops.Remove(barricadeDrop);
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x000B47BC File Offset: 0x000B29BC
		[Obsolete]
		public void tellClearRegionBarricades(CSteamID steamID, byte x, byte y)
		{
			BarricadeManager.ReceiveClearRegionBarricades(x, y);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000B47C5 File Offset: 0x000B29C5
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER, legacyName = "tellClearRegionBarricades")]
		public static void ReceiveClearRegionBarricades(byte x, byte y)
		{
			if (!Provider.isServer && !BarricadeManager.regions[(int)x, (int)y].isNetworked)
			{
				return;
			}
			BarricadeRegion region = BarricadeManager.regions[(int)x, (int)y];
			BarricadeManager.DestroyAllInRegion(region);
			BarricadeManager.CancelInstantiationsInRegion(region);
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000B47FC File Offset: 0x000B29FC
		public static void askClearRegionBarricades(byte x, byte y)
		{
			if (Provider.isServer)
			{
				if (!Regions.checkSafe((int)x, (int)y))
				{
					return;
				}
				BarricadeRegion barricadeRegion = BarricadeManager.regions[(int)x, (int)y];
				if (barricadeRegion.drops.Count > 0)
				{
					barricadeRegion.barricades.Clear();
					BarricadeManager.SendClearRegionBarricades.InvokeAndLoopback(ENetReliability.Reliable, Regions.GatherRemoteClientConnections(x, y, BarricadeManager.BARRICADE_REGIONS), x, y);
				}
			}
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000B485C File Offset: 0x000B2A5C
		public static void askClearAllBarricades()
		{
			if (Provider.isServer)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						BarricadeManager.askClearRegionBarricades(b, b2);
					}
				}
			}
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x000B489C File Offset: 0x000B2A9C
		public static Quaternion getRotation(ItemBarricadeAsset asset, float angle_x, float angle_y, float angle_z)
		{
			return Quaternion.Euler(0f, angle_y, 0f) * Quaternion.Euler((float)((asset.build == EBuild.DOOR || asset.build == EBuild.GATE || asset.build == EBuild.SHUTTER || asset.build == EBuild.HATCH) ? 0 : -90) + angle_x, 0f, 0f) * Quaternion.Euler(0f, angle_z, 0f);
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x000B4910 File Offset: 0x000B2B10
		private Transform spawnBarricade(BarricadeRegion region, Guid assetGuid, byte[] state, Vector3 point, Quaternion rotation, byte hp, ulong owner, ulong group, NetId netId)
		{
			ThreadUtil.ConditionalAssertIsGameThread();
			ItemBarricadeAsset itemBarricadeAsset = Assets.find(assetGuid) as ItemBarricadeAsset;
			if (!Provider.isServer)
			{
				ClientAssetIntegrity.QueueRequest(assetGuid, itemBarricadeAsset, "Barricade");
			}
			if (itemBarricadeAsset == null || itemBarricadeAsset.barricade == null)
			{
				return null;
			}
			Transform transform = null;
			try
			{
				if (itemBarricadeAsset.eligibleForPooling)
				{
					int instanceID = itemBarricadeAsset.barricade.GetInstanceID();
					Stack<GameObject> orAddNew = DictionaryEx.GetOrAddNew<int, Stack<GameObject>>(this.pool, instanceID);
					while (orAddNew.Count > 0)
					{
						GameObject gameObject = orAddNew.Pop();
						if (gameObject != null)
						{
							transform = gameObject.transform;
							transform.parent = region.parent;
							transform.localPosition = point;
							transform.localRotation = rotation;
							transform.localScale = Vector3.one;
							gameObject.SetActive(true);
							break;
						}
					}
				}
				if (transform == null)
				{
					if (region.parent == null)
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(itemBarricadeAsset.barricade, point, rotation);
						transform = gameObject2.transform;
					}
					else
					{
						GameObject gameObject2 = Object.Instantiate<GameObject>(itemBarricadeAsset.barricade, region.parent);
						transform = gameObject2.transform;
						transform.localPosition = point;
						transform.localRotation = rotation;
					}
					transform.localScale = Vector3.one;
					transform.name = itemBarricadeAsset.id.ToString();
					bool useWaterHeightTransparentSort = itemBarricadeAsset.useWaterHeightTransparentSort;
					if (Provider.isServer && itemBarricadeAsset.nav != null)
					{
						Transform transform2 = Object.Instantiate<GameObject>(itemBarricadeAsset.nav).transform;
						transform2.name = "Nav";
						if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SHUTTER || itemBarricadeAsset.build == EBuild.HATCH)
						{
							Transform transform3 = transform.Find("Skeleton").Find("Hinge");
							if (transform3 != null)
							{
								transform2.parent = transform3;
							}
							else
							{
								transform2.parent = transform;
							}
						}
						else
						{
							transform2.parent = transform;
						}
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
					}
					Transform transform4 = transform.FindChildRecursive("Burning");
					if (transform4 != null)
					{
						transform4.gameObject.AddComponent<TemperatureTrigger>().temperature = EPlayerTemperature.BURNING;
					}
					Transform transform5 = transform.FindChildRecursive("Warm");
					if (transform5 != null)
					{
						transform5.gameObject.AddComponent<TemperatureTrigger>().temperature = EPlayerTemperature.WARM;
					}
				}
				else
				{
					bool useWaterHeightTransparentSort2 = itemBarricadeAsset.useWaterHeightTransparentSort;
				}
				if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SHUTTER || itemBarricadeAsset.build == EBuild.HATCH)
				{
					InteractableDoor orAddComponent = transform.GetOrAddComponent<InteractableDoor>();
					orAddComponent.updateState(itemBarricadeAsset, state);
					Transform transform6 = transform.Find("Skeleton").Find("Hinge");
					if (transform6 != null)
					{
						transform6.gameObject.GetOrAddComponent<InteractableDoorHinge>().door = orAddComponent;
					}
					Transform transform7 = transform.Find("Skeleton").Find("Left_Hinge");
					if (transform7 != null)
					{
						transform7.gameObject.GetOrAddComponent<InteractableDoorHinge>().door = orAddComponent;
					}
					Transform transform8 = transform.Find("Skeleton").Find("Right_Hinge");
					if (transform8 != null)
					{
						transform8.gameObject.GetOrAddComponent<InteractableDoorHinge>().door = orAddComponent;
					}
				}
				else if (itemBarricadeAsset.build == EBuild.BED)
				{
					transform.GetOrAddComponent<InteractableBed>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.STORAGE || itemBarricadeAsset.build == EBuild.STORAGE_WALL)
				{
					transform.GetOrAddComponent<InteractableStorage>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.FARM)
				{
					transform.GetOrAddComponent<InteractableFarm>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.TORCH || itemBarricadeAsset.build == EBuild.CAMPFIRE)
				{
					transform.GetOrAddComponent<InteractableFire>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.OVEN)
				{
					transform.GetOrAddComponent<InteractableOven>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.SPIKE || itemBarricadeAsset.build == EBuild.WIRE)
				{
					transform.Find("Trap").GetOrAddComponent<InteractableTrap>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.CHARGE)
				{
					InteractableCharge orAddComponent2 = transform.GetOrAddComponent<InteractableCharge>();
					orAddComponent2.updateState(itemBarricadeAsset, state);
					orAddComponent2.owner = owner;
					orAddComponent2.group = group;
				}
				else if (itemBarricadeAsset.build == EBuild.GENERATOR)
				{
					transform.GetOrAddComponent<InteractableGenerator>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.SPOT || itemBarricadeAsset.build == EBuild.CAGE)
				{
					transform.GetOrAddComponent<InteractableSpot>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.SAFEZONE)
				{
					transform.GetOrAddComponent<InteractableSafezone>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.OXYGENATOR)
				{
					transform.GetOrAddComponent<InteractableOxygenator>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.SIGN || itemBarricadeAsset.build == EBuild.SIGN_WALL || itemBarricadeAsset.build == EBuild.NOTE)
				{
					transform.GetOrAddComponent<InteractableSign>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.CLAIM)
				{
					InteractableClaim orAddComponent3 = transform.GetOrAddComponent<InteractableClaim>();
					orAddComponent3.owner = owner;
					orAddComponent3.group = group;
					orAddComponent3.updateState(itemBarricadeAsset);
				}
				else if (itemBarricadeAsset.build == EBuild.BEACON)
				{
					transform.GetOrAddComponent<InteractableBeacon>().updateState(itemBarricadeAsset);
				}
				else if (itemBarricadeAsset.build == EBuild.BARREL_RAIN)
				{
					transform.GetOrAddComponent<InteractableRainBarrel>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.OIL)
				{
					transform.GetOrAddComponent<InteractableOil>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.TANK)
				{
					transform.GetOrAddComponent<InteractableTank>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.SENTRY || itemBarricadeAsset.build == EBuild.SENTRY_FREEFORM)
				{
					InteractableSentry orAddComponent4 = transform.GetOrAddComponent<InteractableSentry>();
					InteractablePower orAddComponent5 = transform.GetOrAddComponent<InteractablePower>();
					orAddComponent4.power = orAddComponent5;
					orAddComponent4.updateState(itemBarricadeAsset, state);
					orAddComponent5.RefreshIsConnectedToPower();
				}
				else if (itemBarricadeAsset.build == EBuild.LIBRARY)
				{
					transform.GetOrAddComponent<InteractableLibrary>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.MANNEQUIN)
				{
					transform.GetOrAddComponent<InteractableMannequin>().updateState(itemBarricadeAsset, state);
				}
				else if (itemBarricadeAsset.build == EBuild.STEREO)
				{
					transform.GetOrAddComponent<InteractableStereo>().updateState(itemBarricadeAsset, state);
				}
				else
				{
					EBuild build = itemBarricadeAsset.build;
				}
				if (!itemBarricadeAsset.isUnpickupable)
				{
					Interactable2HP orAddComponent6 = transform.GetOrAddComponent<Interactable2HP>();
					orAddComponent6.hp = hp;
					if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SHUTTER || itemBarricadeAsset.build == EBuild.HATCH)
					{
						Transform transform9 = transform.Find("Skeleton").Find("Hinge");
						if (transform9 != null)
						{
							Interactable2SalvageBarricade orAddComponent7 = transform9.GetOrAddComponent<Interactable2SalvageBarricade>();
							orAddComponent7.root = transform;
							orAddComponent7.hp = orAddComponent6;
							orAddComponent7.owner = owner;
							orAddComponent7.group = group;
							orAddComponent7.salvageDurationMultiplier = itemBarricadeAsset.salvageDurationMultiplier;
							orAddComponent7.shouldBypassPickupOwnership = itemBarricadeAsset.shouldBypassPickupOwnership;
						}
						Transform transform10 = transform.Find("Skeleton").Find("Left_Hinge");
						if (transform10 != null)
						{
							Interactable2SalvageBarricade orAddComponent8 = transform10.GetOrAddComponent<Interactable2SalvageBarricade>();
							orAddComponent8.root = transform;
							orAddComponent8.hp = orAddComponent6;
							orAddComponent8.owner = owner;
							orAddComponent8.group = group;
							orAddComponent8.salvageDurationMultiplier = itemBarricadeAsset.salvageDurationMultiplier;
							orAddComponent8.shouldBypassPickupOwnership = itemBarricadeAsset.shouldBypassPickupOwnership;
						}
						Transform transform11 = transform.Find("Skeleton").Find("Right_Hinge");
						if (transform11 != null)
						{
							Interactable2SalvageBarricade orAddComponent9 = transform11.GetOrAddComponent<Interactable2SalvageBarricade>();
							orAddComponent9.root = transform;
							orAddComponent9.hp = orAddComponent6;
							orAddComponent9.owner = owner;
							orAddComponent9.group = group;
							orAddComponent9.salvageDurationMultiplier = itemBarricadeAsset.salvageDurationMultiplier;
							orAddComponent9.shouldBypassPickupOwnership = itemBarricadeAsset.shouldBypassPickupOwnership;
						}
					}
					else
					{
						Interactable2SalvageBarricade orAddComponent10 = transform.GetOrAddComponent<Interactable2SalvageBarricade>();
						orAddComponent10.root = transform;
						orAddComponent10.hp = orAddComponent6;
						orAddComponent10.owner = owner;
						orAddComponent10.group = group;
						orAddComponent10.salvageDurationMultiplier = itemBarricadeAsset.salvageDurationMultiplier;
						orAddComponent10.shouldBypassPickupOwnership = itemBarricadeAsset.shouldBypassPickupOwnership;
					}
				}
				if (region.parent != null)
				{
					BarricadeManager.barricadeColliders.Clear();
					transform.GetComponentsInChildren<Collider>(BarricadeManager.barricadeColliders);
					foreach (Collider collider in BarricadeManager.barricadeColliders)
					{
						bool flag = collider is MeshCollider;
						if (flag)
						{
							collider.enabled = false;
						}
						if (collider.GetComponent<Rigidbody>() == null)
						{
							Rigidbody rigidbody = collider.gameObject.AddComponent<Rigidbody>();
							rigidbody.useGravity = false;
							rigidbody.isKinematic = true;
						}
						if (flag)
						{
							collider.enabled = true;
						}
						if (collider.gameObject.layer == 27)
						{
							collider.gameObject.layer = 14;
						}
					}
					transform.gameObject.SetActive(false);
					transform.gameObject.SetActive(true);
					InteractableVehicle component = region.parent.GetComponent<InteractableVehicle>();
					if (component != null)
					{
						component.ignoreCollisionWith(BarricadeManager.barricadeColliders, true);
					}
				}
				BarricadeDrop barricadeDrop = new BarricadeDrop(transform, transform.GetComponent<Interactable>(), itemBarricadeAsset);
				barricadeDrop.AssignNetId(netId);
				transform.GetOrAddComponent<BarricadeRefComponent>().tempNotSureIfBarricadeShouldBeAComponentYet = barricadeDrop;
				region.drops.Add(barricadeDrop);
			}
			catch (Exception e)
			{
				UnturnedLog.warn("Exception while spawning barricade: {0}", new object[]
				{
					itemBarricadeAsset
				});
				UnturnedLog.exception(e);
				if (transform != null)
				{
					Object.Destroy(transform.gameObject);
					transform = null;
				}
			}
			return transform;
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x000B523C File Offset: 0x000B343C
		[Obsolete]
		public void tellBarricade(CSteamID steamID, byte x, byte y, ushort plant, ushort id, byte[] state, Vector3 point, byte angle_x, byte angle_y, byte angle_z, ulong owner, ulong group, uint instanceID)
		{
			throw new NotSupportedException("Barricades no longer function without a unique NetId");
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000B5248 File Offset: 0x000B3448
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveSingleBarricade(in ClientInvocationContext context, NetId parentNetId, Guid assetId, byte[] state, Vector3 point, Quaternion rotation, ulong owner, ulong group, NetId netId)
		{
			BarricadeRegion barricadeRegion;
			if (parentNetId.IsNull())
			{
				byte x;
				byte y;
				if (!Regions.tryGetCoordinate(point, out x, out y))
				{
					return;
				}
				if (!BarricadeManager.tryGetRegion(x, y, 65535, out barricadeRegion))
				{
					return;
				}
			}
			else
			{
				barricadeRegion = NetIdRegistry.Get<BarricadeRegion>(parentNetId);
				if (barricadeRegion == null)
				{
					return;
				}
			}
			if (!Provider.isServer && !barricadeRegion.isNetworked)
			{
				return;
			}
			float sortOrder = 0f;
			if (MainCamera.instance != null)
			{
				sortOrder = (MainCamera.instance.transform.position - point).sqrMagnitude;
			}
			BarricadeInstantiationParameters barricadeInstantiationParameters = default(BarricadeInstantiationParameters);
			barricadeInstantiationParameters.region = barricadeRegion;
			barricadeInstantiationParameters.assetId = assetId;
			barricadeInstantiationParameters.state = state;
			barricadeInstantiationParameters.position = point;
			barricadeInstantiationParameters.rotation = rotation;
			barricadeInstantiationParameters.hp = 100;
			barricadeInstantiationParameters.owner = owner;
			barricadeInstantiationParameters.group = group;
			barricadeInstantiationParameters.netId = netId;
			barricadeInstantiationParameters.sortOrder = sortOrder;
			NetInvocationDeferralRegistry.MarkDeferred(netId, 3U);
			BarricadeManager.pendingInstantiations.Insert(BarricadeManager.pendingInstantiations.FindInsertionIndex(barricadeInstantiationParameters), barricadeInstantiationParameters);
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000B5348 File Offset: 0x000B3548
		[Obsolete]
		public void tellBarricades(CSteamID steamID)
		{
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000B534C File Offset: 0x000B354C
		[SteamCall(ESteamCallValidation.ONLY_FROM_SERVER)]
		public static void ReceiveMultipleBarricades(in ClientInvocationContext context)
		{
			NetPakReader reader = context.reader;
			byte x;
			SystemNetPakReaderEx.ReadUInt8(reader, ref x);
			byte y;
			SystemNetPakReaderEx.ReadUInt8(reader, ref y);
			NetId netId;
			reader.ReadNetId(out netId);
			BarricadeRegion barricadeRegion;
			if (netId == NetId.INVALID)
			{
				if (!BarricadeManager.tryGetRegion(x, y, 65535, out barricadeRegion))
				{
					return;
				}
			}
			else
			{
				barricadeRegion = NetIdRegistry.Get<BarricadeRegion>(netId);
				if (barricadeRegion == null)
				{
					return;
				}
			}
			byte b;
			SystemNetPakReaderEx.ReadUInt8(reader, ref b);
			if (netId == NetId.INVALID)
			{
				if (b == 0)
				{
					if (barricadeRegion.isNetworked)
					{
						return;
					}
					BarricadeManager.DestroyAllInRegion(barricadeRegion);
				}
				else if (!barricadeRegion.isNetworked)
				{
					return;
				}
			}
			barricadeRegion.isNetworked = true;
			ushort num;
			SystemNetPakReaderEx.ReadUInt16(reader, ref num);
			if (num > 0)
			{
				float sortOrder;
				SystemNetPakReaderEx.ReadFloat(reader, ref sortOrder);
				BarricadeManager.instantiationsToInsert.Clear();
				for (ushort num2 = 0; num2 < num; num2 += 1)
				{
					BarricadeInstantiationParameters barricadeInstantiationParameters = default(BarricadeInstantiationParameters);
					barricadeInstantiationParameters.region = barricadeRegion;
					barricadeInstantiationParameters.sortOrder = sortOrder;
					SystemNetPakReaderEx.ReadGuid(reader, ref barricadeInstantiationParameters.assetId);
					byte b2;
					SystemNetPakReaderEx.ReadUInt8(reader, ref b2);
					byte[] array = new byte[(int)b2];
					reader.ReadBytes(array);
					barricadeInstantiationParameters.state = array;
					UnityNetPakReaderEx.ReadClampedVector3(reader, ref barricadeInstantiationParameters.position, 13, 11);
					UnityNetPakReaderEx.ReadQuaternion(reader, ref barricadeInstantiationParameters.rotation, 9);
					SystemNetPakReaderEx.ReadUInt8(reader, ref barricadeInstantiationParameters.hp);
					SystemNetPakReaderEx.ReadUInt64(reader, ref barricadeInstantiationParameters.owner);
					SystemNetPakReaderEx.ReadUInt64(reader, ref barricadeInstantiationParameters.group);
					reader.ReadNetId(out barricadeInstantiationParameters.netId);
					NetInvocationDeferralRegistry.MarkDeferred(barricadeInstantiationParameters.netId, 3U);
					BarricadeManager.instantiationsToInsert.Add(barricadeInstantiationParameters);
				}
				BarricadeManager.pendingInstantiations.InsertRange(BarricadeManager.pendingInstantiations.FindInsertionIndex(BarricadeManager.instantiationsToInsert[0]), BarricadeManager.instantiationsToInsert);
			}
			Level.isLoadingBarricades = false;
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000B550A File Offset: 0x000B370A
		[Obsolete]
		public void askBarricades(CSteamID steamID, byte x, byte y, ushort plant)
		{
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000B550C File Offset: 0x000B370C
		internal void SendRegion(SteamPlayer client, BarricadeRegion region, byte x, byte y, NetId parentNetId, float sortOrder)
		{
			if (region.drops.Count > 0)
			{
				byte packet = 0;
				int index = 0;
				int count = 0;
				while (index < region.drops.Count)
				{
					int num = 0;
					while (count < region.drops.Count)
					{
						num += 44 + region.drops[count].serversideData.barricade.state.Length;
						int count2 = count;
						count = count2 + 1;
						if (num > Block.BUFFER_SIZE / 2)
						{
							break;
						}
					}
					BarricadeManager.SendMultipleBarricades.Invoke(ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
					{
						SystemNetPakWriterEx.WriteUInt8(writer, x);
						SystemNetPakWriterEx.WriteUInt8(writer, y);
						writer.WriteNetId(parentNetId);
						SystemNetPakWriterEx.WriteUInt8(writer, packet);
						int index;
						SystemNetPakWriterEx.WriteUInt16(writer, (ushort)(count - index));
						SystemNetPakWriterEx.WriteFloat(writer, sortOrder);
						while (index < count)
						{
							BarricadeDrop barricadeDrop = region.drops[index];
							BarricadeData serversideData = barricadeDrop.serversideData;
							InteractableStorage interactableStorage = barricadeDrop.interactable as InteractableStorage;
							SystemNetPakWriterEx.WriteGuid(writer, barricadeDrop.asset.GUID);
							if (interactableStorage != null)
							{
								byte[] array;
								if (interactableStorage.isDisplay)
								{
									string text = (interactableStorage.displayTags != null) ? interactableStorage.displayTags : string.Empty;
									byte[] bytes = Encoding.UTF8.GetBytes(text);
									string text2 = (interactableStorage.displayDynamicProps != null) ? interactableStorage.displayDynamicProps : string.Empty;
									byte[] bytes2 = Encoding.UTF8.GetBytes(text2);
									array = new byte[20 + ((interactableStorage.displayItem != null) ? interactableStorage.displayItem.state.Length : 0) + 4 + 1 + bytes.Length + 1 + bytes2.Length + 1];
									if (interactableStorage.displayItem != null)
									{
										Array.Copy(BitConverter.GetBytes(interactableStorage.displayItem.id), 0, array, 16, 2);
										array[18] = interactableStorage.displayItem.quality;
										array[19] = (byte)interactableStorage.displayItem.state.Length;
										Array.Copy(interactableStorage.displayItem.state, 0, array, 20, interactableStorage.displayItem.state.Length);
										Array.Copy(BitConverter.GetBytes(interactableStorage.displaySkin), 0, array, 20 + interactableStorage.displayItem.state.Length, 2);
										Array.Copy(BitConverter.GetBytes(interactableStorage.displayMythic), 0, array, 20 + interactableStorage.displayItem.state.Length + 2, 2);
										array[20 + interactableStorage.displayItem.state.Length + 4] = (byte)bytes.Length;
										Array.Copy(bytes, 0, array, 20 + interactableStorage.displayItem.state.Length + 5, bytes.Length);
										array[20 + interactableStorage.displayItem.state.Length + 5 + bytes.Length] = (byte)bytes2.Length;
										Array.Copy(bytes2, 0, array, 20 + interactableStorage.displayItem.state.Length + 5 + bytes.Length + 1, bytes2.Length);
										array[20 + interactableStorage.displayItem.state.Length + 5 + bytes.Length + 1 + bytes2.Length] = interactableStorage.rot_comp;
									}
								}
								else
								{
									array = new byte[16];
								}
								Array.Copy(serversideData.barricade.state, 0, array, 0, 16);
								SystemNetPakWriterEx.WriteUInt8(writer, (byte)array.Length);
								writer.WriteBytes(array);
							}
							else
							{
								SystemNetPakWriterEx.WriteUInt8(writer, (byte)serversideData.barricade.state.Length);
								writer.WriteBytes(serversideData.barricade.state);
							}
							UnityNetPakWriterEx.WriteClampedVector3(writer, serversideData.point, 13, 11);
							UnityNetPakWriterEx.WriteQuaternion(writer, serversideData.rotation, 9);
							SystemNetPakWriterEx.WriteUInt8(writer, (byte)Mathf.RoundToInt((float)serversideData.barricade.health / (float)serversideData.barricade.asset.health * 100f));
							SystemNetPakWriterEx.WriteUInt64(writer, serversideData.owner);
							SystemNetPakWriterEx.WriteUInt64(writer, serversideData.group);
							writer.WriteNetId(barricadeDrop.GetNetId());
							index = index;
							index++;
						}
					});
					byte packet2 = packet;
					packet = packet2 + 1;
				}
				return;
			}
			BarricadeManager.SendMultipleBarricades.Invoke(ENetReliability.Reliable, client.transportConnection, delegate(NetPakWriter writer)
			{
				SystemNetPakWriterEx.WriteUInt8(writer, x);
				SystemNetPakWriterEx.WriteUInt8(writer, y);
				writer.WriteNetId(NetId.INVALID);
				SystemNetPakWriterEx.WriteUInt8(writer, 0);
				SystemNetPakWriterEx.WriteUInt16(writer, 0);
			});
		}

		/// <summary>
		/// Clean up before loading vehicles.
		/// </summary>
		// Token: 0x06002A86 RID: 10886 RVA: 0x000B565F File Offset: 0x000B385F
		public static void clearPlants()
		{
			BarricadeManager.internalVehicleRegions = new List<VehicleBarricadeRegion>();
			BarricadeManager.vehicleRegions = BarricadeManager.internalVehicleRegions.AsReadOnly();
			BarricadeManager.backwardsCompatVehicleRegions = null;
		}

		/// <summary>
		/// Register a new vehicle as a valid parent for barricades.
		/// Each train car is registered after the root of the train.
		/// Note: Nobody knows why these are called plants.
		/// </summary>
		// Token: 0x06002A87 RID: 10887 RVA: 0x000B5680 File Offset: 0x000B3880
		[Obsolete("Plugins should not be calling this")]
		public static void waterPlant(Transform parent)
		{
			InteractableVehicle vehicle = DamageTool.getVehicle(parent);
			BarricadeManager.registerVehicleRegion(parent, vehicle, 0, NetId.INVALID);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000B56A4 File Offset: 0x000B38A4
		internal static void registerVehicleRegion(Transform parent, InteractableVehicle vehicle, int subvehicleIndex, NetId netId)
		{
			VehicleBarricadeRegion vehicleBarricadeRegion = new VehicleBarricadeRegion(parent, vehicle, subvehicleIndex);
			vehicleBarricadeRegion.isNetworked = true;
			vehicleBarricadeRegion._netId = netId;
			NetIdRegistry.Assign(netId, vehicleBarricadeRegion);
			BarricadeManager.internalVehicleRegions.Add(vehicleBarricadeRegion);
			BarricadeManager.backwardsCompatVehicleRegions = null;
		}

		/// <summary>
		/// Called before destroying a vehicle GameObject because storage needed to be ManualDestroyed.
		/// </summary>
		// Token: 0x06002A89 RID: 10889 RVA: 0x000B56E0 File Offset: 0x000B38E0
		public static void uprootPlant(Transform parent)
		{
			ushort num = 0;
			while ((int)num < BarricadeManager.vehicleRegions.Count)
			{
				VehicleBarricadeRegion vehicleBarricadeRegion = BarricadeManager.vehicleRegions[(int)num];
				if (vehicleBarricadeRegion.parent == parent)
				{
					vehicleBarricadeRegion.barricades.Clear();
					BarricadeManager.DestroyAllInRegion(vehicleBarricadeRegion);
					BarricadeManager.CancelInstantiationsInRegion(vehicleBarricadeRegion);
					NetIdRegistry.Release(vehicleBarricadeRegion._netId);
					BarricadeManager.internalVehicleRegions.RemoveAt((int)num);
					BarricadeManager.backwardsCompatVehicleRegions = null;
					return;
				}
				num += 1;
			}
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000B5754 File Offset: 0x000B3954
		public static void trimPlant(Transform parent)
		{
			ushort num = 0;
			while ((int)num < BarricadeManager.vehicleRegions.Count)
			{
				BarricadeRegion barricadeRegion = BarricadeManager.vehicleRegions[(int)num];
				if (barricadeRegion.parent == parent)
				{
					barricadeRegion.barricades.Clear();
					BarricadeManager.DestroyAllInRegion(barricadeRegion);
					BarricadeManager.CancelInstantiationsInRegion(barricadeRegion);
					return;
				}
				num += 1;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000B57A9 File Offset: 0x000B39A9
		[Obsolete]
		public static void askPlants(CSteamID steamID)
		{
		}

		/// <summary>
		/// Send all vehicle-mounted barricades to client.
		/// Called after sending vehicles so all plant indexes will be valid.
		/// </summary>
		// Token: 0x06002A8C RID: 10892 RVA: 0x000B57AC File Offset: 0x000B39AC
		internal static void SendVehicleRegions(SteamPlayer client)
		{
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
			{
				if (vehicleBarricadeRegion.drops.Count > 0)
				{
					float sqrMagnitude = (client.player.transform.position - vehicleBarricadeRegion.parent.position).sqrMagnitude;
					BarricadeManager.manager.SendRegion(client, vehicleBarricadeRegion, byte.MaxValue, byte.MaxValue, vehicleBarricadeRegion._netId, sqrMagnitude);
				}
			}
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000B5848 File Offset: 0x000B3A48
		public static BarricadeDrop FindBarricadeByRootTransform(Transform transform)
		{
			byte b;
			byte b2;
			ushort num;
			BarricadeRegion barricadeRegion;
			if (BarricadeManager.tryGetRegion(transform, out b, out b2, out num, out barricadeRegion))
			{
				return barricadeRegion.FindBarricadeByRootTransform(transform);
			}
			return null;
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000B5870 File Offset: 0x000B3A70
		[Obsolete("Please use FindBarricadeByRootTransform instead")]
		public static bool tryGetInfo(Transform barricade, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region)
		{
			x = 0;
			y = 0;
			plant = 0;
			index = 0;
			region = null;
			if (BarricadeManager.tryGetRegion(barricade, out x, out y, out plant, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (barricade == region.drops[(int)index].model)
					{
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000B58DC File Offset: 0x000B3ADC
		[Obsolete("Please use FindBarricadeByRootTransform instead")]
		public static bool tryGetInfo(Transform barricade, out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region, out BarricadeDrop drop)
		{
			x = 0;
			y = 0;
			plant = 0;
			index = 0;
			region = null;
			drop = null;
			if (BarricadeManager.tryGetRegion(barricade, out x, out y, out plant, out region))
			{
				index = 0;
				while ((int)index < region.drops.Count)
				{
					if (barricade == region.drops[(int)index].model)
					{
						drop = region.drops[(int)index];
						return true;
					}
					index += 1;
				}
			}
			return false;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000B5960 File Offset: 0x000B3B60
		public static bool tryGetPlant(Transform parent, out byte x, out byte y, out ushort plant, out BarricadeRegion region)
		{
			x = byte.MaxValue;
			y = byte.MaxValue;
			plant = ushort.MaxValue;
			region = null;
			if (parent == null)
			{
				return false;
			}
			plant = 0;
			while ((int)plant < BarricadeManager.vehicleRegions.Count)
			{
				region = BarricadeManager.vehicleRegions[(int)plant];
				if (region.parent == parent)
				{
					return true;
				}
				plant += 1;
			}
			return false;
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000B59D0 File Offset: 0x000B3BD0
		public static bool tryGetRegion(Transform barricade, out byte x, out byte y, out ushort plant, out BarricadeRegion region)
		{
			x = 0;
			y = 0;
			plant = 0;
			region = null;
			if (barricade == null)
			{
				return false;
			}
			if (barricade.parent != null && barricade.parent.CompareTag("Vehicle"))
			{
				plant = 0;
				while ((int)plant < BarricadeManager.vehicleRegions.Count)
				{
					region = BarricadeManager.vehicleRegions[(int)plant];
					if (region.parent == barricade.parent)
					{
						return true;
					}
					plant += 1;
				}
			}
			else
			{
				plant = ushort.MaxValue;
				if (Regions.tryGetCoordinate(barricade.position, out x, out y))
				{
					region = BarricadeManager.regions[(int)x, (int)y];
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x000B5A81 File Offset: 0x000B3C81
		public static InteractableVehicle getVehicleFromPlant(ushort plant)
		{
			if ((int)plant < BarricadeManager.vehicleRegions.Count)
			{
				return BarricadeManager.vehicleRegions[(int)plant].vehicle;
			}
			return null;
		}

		// Token: 0x06002A93 RID: 10899 RVA: 0x000B5AA2 File Offset: 0x000B3CA2
		public static BarricadeRegion getRegionFromVehicle(InteractableVehicle vehicle)
		{
			return BarricadeManager.findRegionFromVehicle(vehicle, 0);
		}

		// Token: 0x06002A94 RID: 10900 RVA: 0x000B5AAC File Offset: 0x000B3CAC
		public static VehicleBarricadeRegion findRegionFromVehicle(InteractableVehicle vehicle, int subvehicleIndex = 0)
		{
			if (vehicle == null)
			{
				return null;
			}
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
			{
				if (vehicleBarricadeRegion.vehicle == vehicle && vehicleBarricadeRegion.subvehicleIndex == subvehicleIndex)
				{
					return vehicleBarricadeRegion;
				}
			}
			return null;
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x000B5B1C File Offset: 0x000B3D1C
		public static VehicleBarricadeRegion findVehicleRegionByNetInstanceID(uint instanceID, int subvehicleIndex = 0)
		{
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
			{
				if (vehicleBarricadeRegion.vehicle.instanceID == instanceID && vehicleBarricadeRegion.subvehicleIndex == subvehicleIndex)
				{
					return vehicleBarricadeRegion;
				}
			}
			return null;
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000B5B80 File Offset: 0x000B3D80
		public static VehicleBarricadeRegion FindVehicleRegionByTransform(Transform parent)
		{
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.internalVehicleRegions)
			{
				if (vehicleBarricadeRegion.parent == parent)
				{
					return vehicleBarricadeRegion;
				}
			}
			return null;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000B5BE0 File Offset: 0x000B3DE0
		public static bool tryGetRegion(byte x, byte y, ushort plant, out BarricadeRegion region)
		{
			region = null;
			if (plant < 65535)
			{
				if ((int)plant < BarricadeManager.vehicleRegions.Count)
				{
					region = BarricadeManager.vehicleRegions[(int)plant];
					return true;
				}
				return false;
			}
			else
			{
				if (Regions.checkSafe((int)x, (int)y))
				{
					region = BarricadeManager.regions[(int)x, (int)y];
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000B5C30 File Offset: 0x000B3E30
		[Obsolete]
		public void tellBarricadeUpdateState(CSteamID steamID, byte x, byte y, ushort plant, ushort index, byte[] newState)
		{
			throw new NotSupportedException("Moved into instance method as part of barricade NetId rewrite");
		}

		/// <summary>
		/// Original server-only version that does not replicate changes to clients.
		/// </summary>
		// Token: 0x06002A99 RID: 10905 RVA: 0x000B5C3C File Offset: 0x000B3E3C
		public static void updateState(Transform transform, byte[] state, int size)
		{
			BarricadeManager.updateStateInternal(transform, state, size, false);
		}

		/// <summary>
		/// Only used by plugins. Replicates state change to clients.
		/// </summary>
		// Token: 0x06002A9A RID: 10906 RVA: 0x000B5C47 File Offset: 0x000B3E47
		public static void updateReplicatedState(Transform transform, byte[] state, int size)
		{
			BarricadeManager.updateStateInternal(transform, state, size, true);
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x000B5C54 File Offset: 0x000B3E54
		private static void updateStateInternal(Transform transform, byte[] state, int size, bool shouldReplicate = false)
		{
			ThreadUtil.assertIsGameThread();
			byte x;
			byte y;
			ushort plant;
			BarricadeRegion barricadeRegion;
			if (BarricadeManager.tryGetRegion(transform, out x, out y, out plant, out barricadeRegion))
			{
				BarricadeDrop barricadeDrop = barricadeRegion.FindBarricadeByRootTransform(transform);
				if (barricadeDrop.serversideData.barricade.state.Length != size)
				{
					barricadeDrop.serversideData.barricade.state = new byte[size];
				}
				Array.Copy(state, barricadeDrop.serversideData.barricade.state, size);
				if (shouldReplicate)
				{
					BarricadeDrop.SendUpdateState.InvokeAndLoopback(barricadeDrop.GetNetId(), ENetReliability.Reliable, BarricadeManager.GatherRemoteClientConnections(x, y, plant), state);
				}
			}
		}

		// Token: 0x06002A9C RID: 10908 RVA: 0x000B5CE4 File Offset: 0x000B3EE4
		private static void updateActivity(BarricadeRegion region, CSteamID owner, CSteamID group)
		{
			foreach (BarricadeDrop barricadeDrop in region.drops)
			{
				BarricadeData serversideData = barricadeDrop.serversideData;
				if (OwnershipTool.checkToggle(owner, serversideData.owner, group, serversideData.group))
				{
					serversideData.objActiveDate = Provider.time;
				}
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000B5D58 File Offset: 0x000B3F58
		private static void updateActivity(CSteamID owner, CSteamID group)
		{
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					BarricadeManager.updateActivity(BarricadeManager.regions[(int)b, (int)b2], owner, group);
				}
			}
			ushort num = 0;
			while ((int)num < BarricadeManager.vehicleRegions.Count)
			{
				BarricadeManager.updateActivity(BarricadeManager.vehicleRegions[(int)num], owner, group);
				num += 1;
			}
		}

		/// <summary>
		/// Not ideal, but there was a problem because onLevelLoaded was not resetting these after disconnecting.
		/// </summary>
		// Token: 0x06002A9E RID: 10910 RVA: 0x000B5DC2 File Offset: 0x000B3FC2
		internal static void ClearNetworkStuff()
		{
			BarricadeManager.pendingInstantiations = new List<BarricadeInstantiationParameters>();
			BarricadeManager.instantiationsToInsert = new List<BarricadeInstantiationParameters>();
			BarricadeManager.regionsPendingDestroy = new List<BarricadeRegion>();
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000B5DE4 File Offset: 0x000B3FE4
		private void onLevelLoaded(int level)
		{
			if (level > Level.BUILD_INDEX_SETUP)
			{
				BarricadeManager.regions = new BarricadeRegion[(int)Regions.WORLD_SIZE, (int)Regions.WORLD_SIZE];
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						BarricadeManager.regions[(int)b, (int)b2] = new BarricadeRegion(null);
					}
				}
				BarricadeManager.barricadeColliders = new List<Collider>();
				BarricadeManager.version = BarricadeManager.SAVEDATA_VERSION;
				BarricadeManager.instanceCount = 0U;
				this.pool = new Dictionary<int, Stack<GameObject>>();
				if (Provider.isServer)
				{
					BarricadeManager.load();
				}
			}
		}

		// Token: 0x06002AA0 RID: 10912 RVA: 0x000B5E74 File Offset: 0x000B4074
		private void onRegionUpdated(Player player, byte old_x, byte old_y, byte new_x, byte new_y, byte step, ref bool canIncrementIndex)
		{
			if (step == 0)
			{
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						if (Provider.isServer)
						{
							if (player.movement.loadedRegions[(int)b, (int)b2].isBarricadesLoaded && !Regions.checkArea(b, b2, new_x, new_y, BarricadeManager.BARRICADE_REGIONS))
							{
								player.movement.loadedRegions[(int)b, (int)b2].isBarricadesLoaded = false;
							}
						}
						else if (player.channel.IsLocalPlayer && BarricadeManager.regions[(int)b, (int)b2].isNetworked && !Regions.checkArea(b, b2, new_x, new_y, BarricadeManager.BARRICADE_REGIONS))
						{
							if (BarricadeManager.regions[(int)b, (int)b2].drops.Count > 0)
							{
								BarricadeManager.regions[(int)b, (int)b2].isPendingDestroy = true;
								BarricadeManager.regionsPendingDestroy.Add(BarricadeManager.regions[(int)b, (int)b2]);
							}
							BarricadeManager.CancelInstantiationsInRegion(BarricadeManager.regions[(int)b, (int)b2]);
							BarricadeManager.regions[(int)b, (int)b2].isNetworked = false;
						}
					}
				}
			}
			if (step == 2 && Regions.checkSafe((int)new_x, (int)new_y))
			{
				Vector3 position = player.transform.position;
				for (int i = (int)(new_x - BarricadeManager.BARRICADE_REGIONS); i <= (int)(new_x + BarricadeManager.BARRICADE_REGIONS); i++)
				{
					for (int j = (int)(new_y - BarricadeManager.BARRICADE_REGIONS); j <= (int)(new_y + BarricadeManager.BARRICADE_REGIONS); j++)
					{
						if (Regions.checkSafe((int)((byte)i), (int)((byte)j)) && !player.movement.loadedRegions[i, j].isBarricadesLoaded)
						{
							player.movement.loadedRegions[i, j].isBarricadesLoaded = true;
							float sortOrder = Regions.HorizontalDistanceFromCenterSquared(i, j, position);
							this.SendRegion(player.channel.owner, BarricadeManager.regions[i, j], (byte)i, (byte)j, NetId.INVALID, sortOrder);
						}
					}
				}
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000B607C File Offset: 0x000B427C
		private void onPlayerCreated(Player player)
		{
			PlayerMovement movement = player.movement;
			movement.onRegionUpdated = (PlayerRegionUpdated)Delegate.Combine(movement.onRegionUpdated, new PlayerRegionUpdated(this.onRegionUpdated));
			if (Provider.isServer)
			{
				BarricadeManager.updateActivity(player.channel.owner.playerID.steamID, player.quests.groupID);
			}
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x000B60DC File Offset: 0x000B42DC
		private void Start()
		{
			BarricadeManager.manager = this;
			CommandLogMemoryUsage.OnExecuted = (Action<List<string>>)Delegate.Combine(CommandLogMemoryUsage.OnExecuted, new Action<List<string>>(this.OnLogMemoryUsage));
			Level.onPreLevelLoaded = (PreLevelLoaded)Delegate.Combine(Level.onPreLevelLoaded, new PreLevelLoaded(this.onLevelLoaded));
			Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x000B6150 File Offset: 0x000B4350
		private void OnLogMemoryUsage(List<string> results)
		{
			int num = 0;
			int num2 = 0;
			BarricadeRegion[,] regions = BarricadeManager.regions;
			int upperBound = regions.GetUpperBound(0);
			int upperBound2 = regions.GetUpperBound(1);
			for (int i = regions.GetLowerBound(0); i <= upperBound; i++)
			{
				for (int j = regions.GetLowerBound(1); j <= upperBound2; j++)
				{
					BarricadeRegion barricadeRegion = regions[i, j];
					if (barricadeRegion.drops.Count > 0)
					{
						num++;
					}
					num2 += barricadeRegion.drops.Count;
				}
			}
			results.Add(string.Format("Barricade regions: {0}", num));
			results.Add(string.Format("Barricades placed on ground: {0}", num2));
			int num3 = 0;
			int num4 = 0;
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.internalVehicleRegions)
			{
				if (vehicleBarricadeRegion.drops.Count > 0)
				{
					num3++;
				}
				num4 += vehicleBarricadeRegion.drops.Count;
			}
			results.Add(string.Format("Barricade vehicle regions: {0}", BarricadeManager.internalVehicleRegions.Count));
			results.Add(string.Format("Vehicles with barricades: {0}", num3));
			results.Add(string.Format("Barricades placed on vehicles: {0}", num4));
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x000B62B8 File Offset: 0x000B44B8
		public static void load()
		{
			bool flag = false;
			if (LevelSavedata.fileExists("/Barricades.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Barricades.dat", true);
				BarricadeManager.version = river.readByte();
				if (BarricadeManager.version > 6)
				{
					BarricadeManager.serverActiveDate = river.readUInt32();
				}
				else
				{
					BarricadeManager.serverActiveDate = Provider.time;
				}
				if (BarricadeManager.version < 15)
				{
					BarricadeManager.instanceCount = 0U;
				}
				else
				{
					BarricadeManager.instanceCount = river.readUInt32();
				}
				if (BarricadeManager.version > 0)
				{
					for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
					{
						for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
						{
							BarricadeRegion region = BarricadeManager.regions[(int)b, (int)b2];
							BarricadeManager.loadRegion(BarricadeManager.version, river, region);
						}
					}
					if (BarricadeManager.version > 1)
					{
						if (BarricadeManager.version > 13)
						{
							ushort num = river.readUInt16();
							for (ushort num2 = 0; num2 < num; num2 += 1)
							{
								uint num3 = river.readUInt32();
								int num4;
								if (BarricadeManager.version < 16)
								{
									num4 = 0;
								}
								else
								{
									num4 = (int)river.readByte();
								}
								BarricadeRegion barricadeRegion = BarricadeManager.findVehicleRegionByNetInstanceID(num3, num4);
								if (barricadeRegion == null)
								{
									CommandWindow.LogWarning(string.Format("Barricades associated with missing vehicle instance ID '{0}' subindex {1} were lost", num3, num4));
									barricadeRegion = BarricadeManager.regions[0, 0];
								}
								BarricadeManager.loadRegion(BarricadeManager.version, river, barricadeRegion);
							}
						}
						else
						{
							ushort num5 = river.readUInt16();
							num5 = (ushort)Mathf.Min((int)num5, BarricadeManager.vehicleRegions.Count);
							for (int i = 0; i < (int)num5; i++)
							{
								BarricadeRegion region2 = BarricadeManager.vehicleRegions[i];
								BarricadeManager.loadRegion(BarricadeManager.version, river, region2);
							}
						}
					}
				}
				if (BarricadeManager.version < 11)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag && LevelObjects.buildables != null)
			{
				int num6 = 0;
				for (byte b3 = 0; b3 < Regions.WORLD_SIZE; b3 += 1)
				{
					for (byte b4 = 0; b4 < Regions.WORLD_SIZE; b4 += 1)
					{
						List<LevelBuildableObject> list = LevelObjects.buildables[(int)b3, (int)b4];
						if (list != null && list.Count != 0)
						{
							BarricadeRegion barricadeRegion2 = BarricadeManager.regions[(int)b3, (int)b4];
							for (int j = 0; j < list.Count; j++)
							{
								LevelBuildableObject levelBuildableObject = list[j];
								if (levelBuildableObject != null)
								{
									ItemBarricadeAsset itemBarricadeAsset = levelBuildableObject.asset as ItemBarricadeAsset;
									if (itemBarricadeAsset != null)
									{
										Barricade barricade = new Barricade(itemBarricadeAsset);
										BarricadeData barricadeData = new BarricadeData(barricade, levelBuildableObject.point, levelBuildableObject.rotation, 0UL, 0UL, uint.MaxValue, BarricadeManager.instanceCount += 1U);
										NetId netId = NetIdRegistry.ClaimBlock(3U);
										if (BarricadeManager.manager.spawnBarricade(barricadeRegion2, barricade.asset.GUID, barricade.state, barricadeData.point, barricadeData.rotation, (byte)Mathf.RoundToInt((float)barricade.health / (float)itemBarricadeAsset.health * 100f), 0UL, 0UL, netId) != null)
										{
											barricadeRegion2.drops.GetTail<BarricadeDrop>().serversideData = barricadeData;
											barricadeRegion2.barricades.Add(barricadeData);
											num6++;
										}
										else
										{
											UnturnedLog.warn(string.Format("Failed to spawn default barricade object {0} at {1}", itemBarricadeAsset.name, levelBuildableObject.point));
										}
									}
								}
							}
						}
					}
				}
				UnturnedLog.info(string.Format("Spawned {0} default barricades from level", num6));
			}
			Level.isLoadingBarricades = false;
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x000B6628 File Offset: 0x000B4828
		public static void save()
		{
			River river = LevelSavedata.openRiver("/Barricades.dat", false);
			river.writeByte(19);
			river.writeUInt32(Provider.time);
			river.writeUInt32(BarricadeManager.instanceCount);
			for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
			{
				for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
				{
					BarricadeRegion region = BarricadeManager.regions[(int)b, (int)b2];
					BarricadeManager.saveRegion(river, region);
				}
			}
			ushort num = 0;
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion in BarricadeManager.vehicleRegions)
			{
				InteractableVehicle vehicle = vehicleBarricadeRegion.vehicle;
				if (vehicle != null && !vehicle.isAutoClearable)
				{
					num += 1;
				}
			}
			river.writeUInt16(num);
			foreach (VehicleBarricadeRegion vehicleBarricadeRegion2 in BarricadeManager.vehicleRegions)
			{
				InteractableVehicle vehicle2 = vehicleBarricadeRegion2.vehicle;
				if (vehicle2 != null && !vehicle2.isAutoClearable)
				{
					river.writeUInt32(vehicle2.instanceID);
					river.writeByte((byte)vehicleBarricadeRegion2.subvehicleIndex);
					BarricadeManager.saveRegion(river, vehicleBarricadeRegion2);
				}
			}
			river.closeRiver();
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000B677C File Offset: 0x000B497C
		[Conditional("LOG_BARRICADE_LOADING")]
		private static void LogLoading(string message)
		{
			UnturnedLog.info(message);
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x000B6784 File Offset: 0x000B4984
		private static void loadRegion(byte version, River river, BarricadeRegion region)
		{
			ushort num = river.readUInt16();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				ItemBarricadeAsset itemBarricadeAsset;
				if (version < 17)
				{
					ushort id = river.readUInt16();
					itemBarricadeAsset = (Assets.find(EAssetType.ITEM, id) as ItemBarricadeAsset);
				}
				else
				{
					itemBarricadeAsset = (Assets.find(river.readGUID()) as ItemBarricadeAsset);
				}
				uint newInstanceID;
				if (version < 15)
				{
					newInstanceID = (BarricadeManager.instanceCount += 1U);
				}
				else
				{
					newInstanceID = river.readUInt32();
				}
				ushort num3 = river.readUInt16();
				byte[] array = river.readBytes();
				Vector3 vector = river.readSingleVector3();
				Quaternion quaternion;
				if (version < 19)
				{
					byte b = 0;
					if (version > 2)
					{
						b = river.readByte();
					}
					byte b2 = river.readByte();
					byte b3 = 0;
					if (version > 3)
					{
						b3 = river.readByte();
					}
					if (version < 10 && itemBarricadeAsset != null)
					{
						quaternion = BarricadeManager.getRotation(itemBarricadeAsset, (float)(b * 2), (float)(b2 * 2), (float)(b3 * 2));
					}
					else
					{
						quaternion = Quaternion.Euler((float)b * 2f, (float)b2 * 2f, (float)b3 * 2f);
					}
				}
				else
				{
					quaternion = river.readSingleQuaternion();
				}
				ulong num4 = 0UL;
				ulong num5 = 0UL;
				if (version > 4)
				{
					num4 = river.readUInt64();
					num5 = river.readUInt64();
				}
				uint newObjActiveDate;
				if (version > 5)
				{
					newObjActiveDate = river.readUInt32();
					if (Provider.time - BarricadeManager.serverActiveDate > Provider.modeConfigData.Barricades.Decay_Time / 2U)
					{
						newObjActiveDate = Provider.time;
					}
				}
				else
				{
					newObjActiveDate = Provider.time;
				}
				byte b4;
				if (version >= 18)
				{
					b4 = river.readByte();
				}
				else
				{
					b4 = byte.MaxValue;
				}
				if (itemBarricadeAsset != null)
				{
					if (version >= 18 && b4 != (byte)itemBarricadeAsset.build)
					{
						UnturnedLog.info("Discarding barricade \"" + itemBarricadeAsset.FriendlyName + "\" because asset Build property changed which might cause bigger problems (public issue #3725)");
					}
					else
					{
						if (itemBarricadeAsset.type == EItemType.TANK && array.Length < 2)
						{
							array = itemBarricadeAsset.getState(EItemOrigin.ADMIN);
						}
						if (itemBarricadeAsset.build == EBuild.OIL && array.Length < 2)
						{
							array = itemBarricadeAsset.getState(EItemOrigin.ADMIN);
						}
						NetId netId = NetIdRegistry.ClaimBlock(3U);
						if (BarricadeManager.manager.spawnBarricade(region, itemBarricadeAsset.GUID, array, vector, quaternion, (byte)Mathf.RoundToInt((float)num3 / (float)itemBarricadeAsset.health * 100f), num4, num5, netId) != null)
						{
							BarricadeDrop tail = region.drops.GetTail<BarricadeDrop>();
							BarricadeData barricadeData = new BarricadeData(new Barricade(itemBarricadeAsset, num3, array), vector, quaternion, num4, num5, newObjActiveDate, newInstanceID);
							tail.serversideData = barricadeData;
							region.barricades.Add(barricadeData);
						}
					}
				}
			}
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000B69DC File Offset: 0x000B4BDC
		private static void saveRegion(River river, BarricadeRegion region)
		{
			uint time = Provider.time;
			ushort num = 0;
			foreach (BarricadeDrop barricadeDrop in region.drops)
			{
				BarricadeData serversideData = barricadeDrop.serversideData;
				if ((Provider.modeConfigData.Barricades.Decay_Time == 0U || time < serversideData.objActiveDate || time - serversideData.objActiveDate < Provider.modeConfigData.Barricades.Decay_Time) && serversideData.barricade.asset.isSaveable)
				{
					num += 1;
				}
			}
			river.writeUInt16(num);
			foreach (BarricadeDrop barricadeDrop2 in region.drops)
			{
				BarricadeData serversideData2 = barricadeDrop2.serversideData;
				if ((Provider.modeConfigData.Barricades.Decay_Time == 0U || time < serversideData2.objActiveDate || time - serversideData2.objActiveDate < Provider.modeConfigData.Barricades.Decay_Time) && serversideData2.barricade.asset.isSaveable)
				{
					river.writeGUID(barricadeDrop2.asset.GUID);
					river.writeUInt32(serversideData2.instanceID);
					river.writeUInt16(serversideData2.barricade.health);
					river.writeBytes(serversideData2.barricade.state);
					river.writeSingleVector3(serversideData2.point);
					river.writeSingleQuaternion(serversideData2.rotation);
					river.writeUInt64(serversideData2.owner);
					river.writeUInt64(serversideData2.group);
					river.writeUInt32(serversideData2.objActiveDate);
					river.writeByte((byte)barricadeDrop2.asset.build);
				}
			}
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000B6BD0 File Offset: 0x000B4DD0
		public static PooledTransportConnectionList GatherClientConnections(byte x, byte y, ushort plant)
		{
			if (plant == 65535)
			{
				return Regions.GatherClientConnections(x, y, BarricadeManager.BARRICADE_REGIONS);
			}
			return Provider.GatherClientConnections();
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000B6BEC File Offset: 0x000B4DEC
		[Obsolete("Replaced by GatherClients")]
		public static IEnumerable<ITransportConnection> EnumerateClients(byte x, byte y, ushort plant)
		{
			return BarricadeManager.GatherClientConnections(x, y, plant);
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000B6BF6 File Offset: 0x000B4DF6
		public static PooledTransportConnectionList GatherRemoteClientConnections(byte x, byte y, ushort plant)
		{
			if (plant == 65535)
			{
				return Regions.GatherRemoteClientConnections(x, y, BarricadeManager.BARRICADE_REGIONS);
			}
			return Provider.GatherRemoteClientConnections();
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000B6C12 File Offset: 0x000B4E12
		[Obsolete("Replaced by GatherRemoteClients")]
		public static IEnumerable<ITransportConnection> EnumerateClients_Remote(byte x, byte y, ushort plant)
		{
			return BarricadeManager.GatherRemoteClientConnections(x, y, plant);
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000B6C1C File Offset: 0x000B4E1C
		private static void DestroyAllInRegion(BarricadeRegion region)
		{
			if (region.drops.Count > 0)
			{
				region.DestroyAll();
			}
			if (region.isPendingDestroy)
			{
				region.isPendingDestroy = false;
				BarricadeManager.regionsPendingDestroy.RemoveFast(region);
			}
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000B6C50 File Offset: 0x000B4E50
		private static void CancelInstantiationsInRegion(BarricadeRegion region)
		{
			for (int i = BarricadeManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (BarricadeManager.pendingInstantiations[i].region == region)
				{
					NetInvocationDeferralRegistry.Cancel(BarricadeManager.pendingInstantiations[i].netId, 3U);
					BarricadeManager.pendingInstantiations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000B6CA8 File Offset: 0x000B4EA8
		private static void CancelInstantiationByNetId(NetId netId)
		{
			for (int i = BarricadeManager.pendingInstantiations.Count - 1; i >= 0; i--)
			{
				if (BarricadeManager.pendingInstantiations[i].netId == netId)
				{
					NetInvocationDeferralRegistry.Cancel(netId, 3U);
					BarricadeManager.pendingInstantiations.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000B6CF8 File Offset: 0x000B4EF8
		internal void DestroyOrReleaseBarricade(ItemBarricadeAsset asset, GameObject instance)
		{
			Transform transform = instance.transform;
			EffectManager.ClearAttachments(transform);
			if (asset.eligibleForPooling)
			{
				if (transform.parent != null)
				{
					BarricadeManager.barricadeColliders.Clear();
					instance.GetComponentsInChildren<Collider>(BarricadeManager.barricadeColliders);
					foreach (Collider collider in BarricadeManager.barricadeColliders)
					{
						if (collider.gameObject.layer == 14)
						{
							collider.gameObject.layer = 27;
						}
					}
					InteractableVehicle component = transform.parent.GetComponent<InteractableVehicle>();
					if (component != null)
					{
						component.ignoreCollisionWith(BarricadeManager.barricadeColliders, false);
					}
				}
				instance.SetActive(false);
				transform.parent = null;
				int instanceID = asset.barricade.GetInstanceID();
				DictionaryEx.GetOrAddNew<int, Stack<GameObject>>(this.pool, instanceID).Push(instance);
				return;
			}
			Object.Destroy(instance);
		}

		// Token: 0x0400169E RID: 5790
		private static Collider[] checkColliders = new Collider[2];

		/// <summary>
		/// Barricade asset's EBuild included in saves to fix state length problems. (public issue #3725)
		/// </summary>
		// Token: 0x0400169F RID: 5791
		public const byte SAVEDATA_VERSION_INCLUDE_BUILD_ENUM = 18;

		// Token: 0x040016A0 RID: 5792
		public const byte SAVEDATA_VERSION_REPLACE_EULER_ANGLES_WITH_QUATERNION = 19;

		// Token: 0x040016A1 RID: 5793
		private const byte SAVEDATA_VERSION_NEWEST = 19;

		// Token: 0x040016A2 RID: 5794
		public static readonly byte SAVEDATA_VERSION = 19;

		// Token: 0x040016A3 RID: 5795
		public static readonly byte BARRICADE_REGIONS = 2;

		// Token: 0x040016A4 RID: 5796
		public static DeployBarricadeRequestHandler onDeployBarricadeRequested;

		// Token: 0x040016A5 RID: 5797
		[Obsolete("Please use BarricadeDrop.OnSalvageRequested_Global instead")]
		public static SalvageBarricadeRequestHandler onSalvageBarricadeRequested;

		// Token: 0x040016A6 RID: 5798
		public static DamageBarricadeRequestHandler onDamageBarricadeRequested;

		// Token: 0x040016A9 RID: 5801
		[Obsolete("Please use InteractableFarm.OnHarvestRequested_Global instead")]
		public static SalvageBarricadeRequestHandler onHarvestPlantRequested;

		// Token: 0x040016AA RID: 5802
		public static BarricadeSpawnedHandler onBarricadeSpawned;

		// Token: 0x040016AB RID: 5803
		public static ModifySignRequestHandler onModifySignRequested;

		// Token: 0x040016AC RID: 5804
		public static OpenStorageRequestHandler onOpenStorageRequested;

		// Token: 0x040016AD RID: 5805
		public static TransformBarricadeRequestHandler onTransformRequested;

		// Token: 0x040016AE RID: 5806
		private static BarricadeManager manager;

		// Token: 0x040016AF RID: 5807
		public static byte version = BarricadeManager.SAVEDATA_VERSION;

		/// <summary>
		/// Writable list of vehicle regions. Public add/remove methods should not be necessary.
		/// </summary>
		// Token: 0x040016B1 RID: 5809
		private static List<VehicleBarricadeRegion> internalVehicleRegions;

		// Token: 0x040016B3 RID: 5811
		private static List<BarricadeRegion> backwardsCompatVehicleRegions;

		// Token: 0x040016B4 RID: 5812
		private static List<BarricadeInstantiationParameters> pendingInstantiations;

		// Token: 0x040016B5 RID: 5813
		private static List<BarricadeInstantiationParameters> instantiationsToInsert;

		// Token: 0x040016B6 RID: 5814
		private static List<BarricadeRegion> regionsPendingDestroy;

		// Token: 0x040016B7 RID: 5815
		private static List<Collider> barricadeColliders;

		// Token: 0x040016B8 RID: 5816
		private static uint instanceCount;

		// Token: 0x040016B9 RID: 5817
		private static uint serverActiveDate;

		// Token: 0x040016BA RID: 5818
		private static readonly ClientStaticMethod<NetId> SendDestroyBarricade = ClientStaticMethod<NetId>.Get(new ClientStaticMethod<NetId>.ReceiveDelegateWithContext(BarricadeManager.ReceiveDestroyBarricade));

		// Token: 0x040016BB RID: 5819
		private static readonly ClientStaticMethod<byte, byte> SendClearRegionBarricades = ClientStaticMethod<byte, byte>.Get(new ClientStaticMethod<byte, byte>.ReceiveDelegate(BarricadeManager.ReceiveClearRegionBarricades));

		// Token: 0x040016BC RID: 5820
		private static readonly ClientStaticMethod<NetId, Guid, byte[], Vector3, Quaternion, ulong, ulong, NetId> SendSingleBarricade = ClientStaticMethod<NetId, Guid, byte[], Vector3, Quaternion, ulong, ulong, NetId>.Get(new ClientStaticMethod<NetId, Guid, byte[], Vector3, Quaternion, ulong, ulong, NetId>.ReceiveDelegateWithContext(BarricadeManager.ReceiveSingleBarricade));

		// Token: 0x040016BD RID: 5821
		private static readonly ClientStaticMethod SendMultipleBarricades = ClientStaticMethod.Get(new ClientStaticMethod.ReceiveDelegateWithContext(BarricadeManager.ReceiveMultipleBarricades));

		/// <summary>
		/// Maps prefab unique id to inactive list.
		/// </summary>
		// Token: 0x040016BE RID: 5822
		private Dictionary<int, Stack<GameObject>> pool;

		// Token: 0x040016BF RID: 5823
		internal const int POSITION_FRAC_BIT_COUNT = 11;

		/// <summary>
		/// +0 = BarricadeDrop
		/// +1 = root transform
		/// +2 = Interactable (if exists)
		/// </summary>
		// Token: 0x040016C0 RID: 5824
		internal const int NETIDS_PER_BARRICADE = 3;
	}
}
