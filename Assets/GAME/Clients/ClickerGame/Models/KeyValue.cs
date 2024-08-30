using EdjCase.ICP.Candid.Mapping;

namespace Rogues.ClickerGame.Models
{
	public class KeyValue
	{
		[CandidName("key")]
		public string Key { get; set; }

		[CandidName("value")]
		public string Value { get; set; }

		public KeyValue(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		public KeyValue()
		{
		}
	}
}