using System;
using Steamworks;

namespace SDG.Unturned
{
	// Token: 0x02000399 RID: 921
	public class CommandCamera : Command
	{
		// Token: 0x06001CC7 RID: 7367 RVA: 0x00066600 File Offset: 0x00064800
		protected override void execute(CSteamID executorID, string parameter)
		{
			string text = parameter.ToLower();
			ECameraMode cameraMode;
			if (text == this.localization.format("CameraFirst").ToLower())
			{
				cameraMode = ECameraMode.FIRST;
			}
			else if (text == this.localization.format("CameraThird").ToLower())
			{
				cameraMode = ECameraMode.THIRD;
			}
			else if (text == this.localization.format("CameraBoth").ToLower())
			{
				cameraMode = ECameraMode.BOTH;
			}
			else
			{
				if (!(text == this.localization.format("CameraVehicle").ToLower()))
				{
					CommandWindow.LogError(this.localization.format("NoCameraErrorText", text));
					return;
				}
				cameraMode = ECameraMode.VEHICLE;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.cameraMode = cameraMode;
			CommandWindow.Log(this.localization.format("CameraText", text));
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000666E8 File Offset: 0x000648E8
		public CommandCamera(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CameraCommandText");
			this._info = this.localization.format("CameraInfoText");
			this._help = this.localization.format("CameraHelpText");
		}
	}
}
