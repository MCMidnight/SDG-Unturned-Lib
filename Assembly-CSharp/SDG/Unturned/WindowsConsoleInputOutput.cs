using System;
using System.Threading;

namespace SDG.Unturned
{
	/// <summary>
	/// Windows-specific extensions of console input.
	/// Uses the Win32 API to force a console to be created and destroyed.
	/// </summary>
	// Token: 0x020003EA RID: 1002
	public class WindowsConsoleInputOutput : ConsoleInputOutput
	{
		// Token: 0x06001DDB RID: 7643 RVA: 0x0006CB36 File Offset: 0x0006AD36
		public override void initialize(CommandWindow commandWindow)
		{
			WindowsConsole.conditionalAlloc();
			WindowsConsole.setCodePageToUTF8();
			WindowsConsole.RegisterCtrlHandler(new Action<WindowsConsole.ECtrlType>(this.OnWindowsQuitEvent));
			base.initialize(commandWindow);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x0006CB5A File Offset: 0x0006AD5A
		public override void shutdown(CommandWindow commandWindow)
		{
			base.shutdown(commandWindow);
			WindowsConsole.conditionalFree();
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x0006CB68 File Offset: 0x0006AD68
		private void OnWindowsQuitEvent(WindowsConsole.ECtrlType ctrlType)
		{
			Interlocked.Exchange(ref this.wantsToTerminate, 1);
		}
	}
}
