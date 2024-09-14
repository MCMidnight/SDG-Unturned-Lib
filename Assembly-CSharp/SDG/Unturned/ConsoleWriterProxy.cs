using System;
using System.IO;
using System.Text;

namespace SDG.Unturned
{
	// Token: 0x020003E3 RID: 995
	public class ConsoleWriterProxy : TextWriter
	{
		// Token: 0x06001DA6 RID: 7590 RVA: 0x0006C6EE File Offset: 0x0006A8EE
		public ConsoleWriterProxy(StreamWriter streamWriter, TextWriter defaultConsoleWriter)
		{
			this.customWriter = streamWriter;
			this.defaultConsoleWriter = defaultConsoleWriter;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x0006C704 File Offset: 0x0006A904
		public override void Close()
		{
			this.customWriter.Close();
			this.defaultConsoleWriter.Close();
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0006C71C File Offset: 0x0006A91C
		public override Encoding Encoding
		{
			get
			{
				return this.defaultConsoleWriter.Encoding;
			}
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x0006C729 File Offset: 0x0006A929
		public override void Flush()
		{
			this.customWriter.Flush();
			this.defaultConsoleWriter.Flush();
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x0006C741 File Offset: 0x0006A941
		public override IFormatProvider FormatProvider
		{
			get
			{
				return this.defaultConsoleWriter.FormatProvider;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x0006C74E File Offset: 0x0006A94E
		// (set) Token: 0x06001DAC RID: 7596 RVA: 0x0006C75B File Offset: 0x0006A95B
		public override string NewLine
		{
			get
			{
				return this.defaultConsoleWriter.NewLine;
			}
			set
			{
				this.defaultConsoleWriter.NewLine = value;
			}
		}

		/// <summary>
		/// This is the only /required/ override of text writer.
		/// </summary>
		// Token: 0x06001DAD RID: 7597 RVA: 0x0006C769 File Offset: 0x0006A969
		public override void Write(char value)
		{
			this.customWriter.Write(value);
			this.defaultConsoleWriter.Write(value);
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x0006C783 File Offset: 0x0006A983
		public override void WriteLine()
		{
			this.customWriter.WriteLine();
			this.defaultConsoleWriter.WriteLine();
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x0006C79B File Offset: 0x0006A99B
		public override void WriteLine(string value)
		{
			this.customWriter.WriteLine(value);
			this.defaultConsoleWriter.WriteLine(value);
		}

		// Token: 0x04000DED RID: 3565
		protected TextWriter defaultConsoleWriter;

		// Token: 0x04000DEE RID: 3566
		private StreamWriter customWriter;
	}
}
