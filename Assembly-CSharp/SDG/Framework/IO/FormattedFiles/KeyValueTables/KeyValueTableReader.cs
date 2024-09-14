using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	// Token: 0x020000C0 RID: 192
	public class KeyValueTableReader : IFormattedFileReader
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001423F File Offset: 0x0001243F
		// (set) Token: 0x0600051C RID: 1308 RVA: 0x00014247 File Offset: 0x00012447
		public Dictionary<string, object> table { get; protected set; }

		// Token: 0x0600051D RID: 1309 RVA: 0x00014250 File Offset: 0x00012450
		public virtual IEnumerable<string> getKeys()
		{
			return this.table.Keys;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001425D File Offset: 0x0001245D
		public virtual bool containsKey(string key)
		{
			return this.table.ContainsKey(key);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001426B File Offset: 0x0001246B
		public virtual void readKey(string key)
		{
			this.key = key;
			this.index = -1;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001427B File Offset: 0x0001247B
		public virtual int readArrayLength(string key)
		{
			this.readKey(key);
			return this.readArrayLength();
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001428C File Offset: 0x0001248C
		public virtual int readArrayLength()
		{
			object obj;
			if (this.table.TryGetValue(this.key, ref obj))
			{
				return (obj as List<object>).Count;
			}
			return 0;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x000142BB File Offset: 0x000124BB
		public virtual void readArrayIndex(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000142CB File Offset: 0x000124CB
		public virtual void readArrayIndex(int index)
		{
			this.index = index;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000142D4 File Offset: 0x000124D4
		public virtual string readValue(string key)
		{
			this.readKey(key);
			return this.readValue();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x000142E3 File Offset: 0x000124E3
		public virtual string readValue(int index)
		{
			this.readArrayIndex(index);
			return this.readValue();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x000142F2 File Offset: 0x000124F2
		public virtual string readValue(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00014308 File Offset: 0x00012508
		public virtual string readValue()
		{
			if (this.index == -1)
			{
				object obj;
				if (!this.table.TryGetValue(this.key, ref obj))
				{
					return null;
				}
				return (string)obj;
			}
			else
			{
				object obj2;
				if (this.table.TryGetValue(this.key, ref obj2))
				{
					return (string)(obj2 as List<object>)[this.index];
				}
				return null;
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00014369 File Offset: 0x00012569
		public virtual object readValue(Type type, string key)
		{
			this.readKey(key);
			return this.readValue(type);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00014379 File Offset: 0x00012579
		public virtual object readValue(Type type, int index)
		{
			this.readArrayIndex(index);
			return this.readValue(type);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00014389 File Offset: 0x00012589
		public virtual object readValue(Type type, string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue(type);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000143A0 File Offset: 0x000125A0
		public virtual object readValue(Type type)
		{
			if (typeof(IFormattedFileReadable).IsAssignableFrom(type))
			{
				IFormattedFileReadable formattedFileReadable = Activator.CreateInstance(type) as IFormattedFileReadable;
				formattedFileReadable.read(this);
				return formattedFileReadable;
			}
			return KeyValueTableTypeReaderRegistry.read(this, type);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000143CE File Offset: 0x000125CE
		public virtual T readValue<T>(string key)
		{
			this.readKey(key);
			return this.readValue<T>();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000143DD File Offset: 0x000125DD
		public virtual T readValue<T>(int index)
		{
			this.readArrayIndex(index);
			return this.readValue<T>();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000143EC File Offset: 0x000125EC
		public virtual T readValue<T>(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readValue<T>();
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00014402 File Offset: 0x00012602
		public virtual T readValue<T>()
		{
			if (typeof(IFormattedFileReadable).IsAssignableFrom(typeof(T)))
			{
				IFormattedFileReadable formattedFileReadable = Activator.CreateInstance<T>() as IFormattedFileReadable;
				formattedFileReadable.read(this);
				return (T)((object)formattedFileReadable);
			}
			return KeyValueTableTypeReaderRegistry.read<T>(this);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00014441 File Offset: 0x00012641
		public virtual IFormattedFileReader readObject(string key)
		{
			this.readKey(key);
			return this.readObject();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00014450 File Offset: 0x00012650
		public virtual IFormattedFileReader readObject(int index)
		{
			this.readArrayIndex(index);
			return this.readObject();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001445F File Offset: 0x0001265F
		public virtual IFormattedFileReader readObject(string key, int index)
		{
			this.readKey(key);
			this.readArrayIndex(index);
			return this.readObject();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00014478 File Offset: 0x00012678
		public virtual IFormattedFileReader readObject()
		{
			if (this.index == -1)
			{
				object obj;
				if (this.table.TryGetValue(this.key, ref obj))
				{
					return obj as IFormattedFileReader;
				}
				return null;
			}
			else
			{
				object obj2;
				if (this.table.TryGetValue(this.key, ref obj2))
				{
					return (obj2 as List<object>)[this.index] as IFormattedFileReader;
				}
				return null;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000144D9 File Offset: 0x000126D9
		protected virtual bool canContinueReadDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			return true;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000144DC File Offset: 0x000126DC
		public virtual void readDictionary(StreamReader input, Dictionary<string, object> scope)
		{
			this.dictionaryKey = null;
			this.dictionaryInQuotes = false;
			this.dictionaryIgnoreNextChar = false;
			while (!input.EndOfStream)
			{
				char c = (char)input.Read();
				if (this.dictionaryIgnoreNextChar)
				{
					KeyValueTableReader.builder.Append(c);
					this.dictionaryIgnoreNextChar = false;
				}
				else if (c == '\\')
				{
					this.dictionaryIgnoreNextChar = true;
				}
				else if (c == '"')
				{
					if (this.dictionaryInQuotes)
					{
						this.dictionaryInQuotes = false;
						if (string.IsNullOrEmpty(this.dictionaryKey))
						{
							this.dictionaryKey = KeyValueTableReader.builder.ToString();
						}
						else
						{
							string text = KeyValueTableReader.builder.ToString();
							if (!scope.ContainsKey(this.dictionaryKey))
							{
								scope.Add(this.dictionaryKey, text);
							}
							if (!this.canContinueReadDictionary(input, scope))
							{
								return;
							}
							this.dictionaryKey = null;
						}
					}
					else
					{
						this.dictionaryInQuotes = true;
						KeyValueTableReader.builder.Length = 0;
					}
				}
				else if (this.dictionaryInQuotes)
				{
					KeyValueTableReader.builder.Append(c);
				}
				else if (c == '{')
				{
					object obj;
					if (scope.TryGetValue(this.dictionaryKey, ref obj))
					{
						KeyValueTableReader keyValueTableReader = (KeyValueTableReader)obj;
						keyValueTableReader.readDictionary(input, keyValueTableReader.table);
					}
					else
					{
						KeyValueTableReader keyValueTableReader2 = new KeyValueTableReader(input);
						obj = keyValueTableReader2;
						scope.Add(this.dictionaryKey, keyValueTableReader2);
					}
					if (!this.canContinueReadDictionary(input, scope))
					{
						return;
					}
					this.dictionaryKey = null;
				}
				else
				{
					if (c == '}')
					{
						return;
					}
					if (c == '[')
					{
						object obj2;
						if (!scope.TryGetValue(this.dictionaryKey, ref obj2))
						{
							obj2 = new List<object>();
							scope.Add(this.dictionaryKey, obj2);
						}
						this.readList(input, (List<object>)obj2);
						if (!this.canContinueReadDictionary(input, scope))
						{
							return;
						}
						this.dictionaryKey = null;
					}
				}
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00014698 File Offset: 0x00012898
		public virtual void readList(StreamReader input, List<object> scope)
		{
			this.listInQuotes = false;
			this.listIgnoreNextChar = false;
			while (!input.EndOfStream)
			{
				char c = (char)input.Read();
				if (this.listIgnoreNextChar)
				{
					KeyValueTableReader.builder.Append(c);
					this.listIgnoreNextChar = false;
				}
				else if (c == '\\')
				{
					this.listIgnoreNextChar = true;
				}
				else if (c == '"')
				{
					if (this.listInQuotes)
					{
						this.listInQuotes = false;
						string text = KeyValueTableReader.builder.ToString();
						scope.Add(text);
					}
					else
					{
						this.listInQuotes = true;
						KeyValueTableReader.builder.Length = 0;
					}
				}
				else if (this.listInQuotes)
				{
					KeyValueTableReader.builder.Append(c);
				}
				else if (c == '{')
				{
					KeyValueTableReader keyValueTableReader = new KeyValueTableReader(input);
					scope.Add(keyValueTableReader);
				}
				else if (c == ']')
				{
					return;
				}
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00014763 File Offset: 0x00012963
		public KeyValueTableReader()
		{
			this.table = new Dictionary<string, object>();
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00014776 File Offset: 0x00012976
		public KeyValueTableReader(StreamReader input)
		{
			this.table = new Dictionary<string, object>();
			this.readDictionary(input, this.table);
		}

		// Token: 0x040001F2 RID: 498
		protected static StringBuilder builder = new StringBuilder();

		// Token: 0x040001F4 RID: 500
		protected string key;

		// Token: 0x040001F5 RID: 501
		protected int index;

		// Token: 0x040001F6 RID: 502
		protected string dictionaryKey;

		// Token: 0x040001F7 RID: 503
		protected bool dictionaryInQuotes;

		// Token: 0x040001F8 RID: 504
		protected bool dictionaryIgnoreNextChar;

		// Token: 0x040001F9 RID: 505
		protected bool listInQuotes;

		// Token: 0x040001FA RID: 506
		protected bool listIgnoreNextChar;
	}
}
