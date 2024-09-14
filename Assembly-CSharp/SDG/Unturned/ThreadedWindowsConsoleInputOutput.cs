using System;
using System.Threading;

namespace SDG.Unturned
{
	/// <summary>
	/// Windows-specific extensions of Windows console input.
	/// Uses the Win32 API to force a console to be created and destroyed.
	/// </summary>
	// Token: 0x020003E8 RID: 1000
	public class ThreadedWindowsConsoleInputOutput : ThreadedConsoleInputOutput
	{
		// Token: 0x06001DCC RID: 7628 RVA: 0x0006C9A4 File Offset: 0x0006ABA4
		public override void initialize(CommandWindow commandWindow)
		{
			WindowsConsole.conditionalAlloc();
			WindowsConsole.setCodePageToUTF8();
			WindowsConsole.RegisterCtrlHandler(new Action<WindowsConsole.ECtrlType>(this.OnWindowsQuitEvent));
			base.initialize(commandWindow);
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0006C9C8 File Offset: 0x0006ABC8
		public override void shutdown(CommandWindow commandWindow)
		{
			base.shutdown(commandWindow);
			WindowsConsole.conditionalFree();
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0006C9D6 File Offset: 0x0006ABD6
		private void OnWindowsQuitEvent(WindowsConsole.ECtrlType ctrlType)
		{
			Interlocked.Exchange(ref this.wantsToTerminate, 1);
		}
	}
}
