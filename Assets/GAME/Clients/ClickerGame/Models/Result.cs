using EdjCase.ICP.Candid.Mapping;
using Rogues.ClickerGame.Models;
using System;

namespace Rogues.ClickerGame.Models
{
	[Variant]
	public class Result
	{
		[VariantTagProperty]
		public ResultTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result(ResultTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result()
		{
		}

		public static Result Err(string info)
		{
			return new Result(ResultTag.Err, info);
		}

		public static Result Ok(KeyValue info)
		{
			return new Result(ResultTag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(ResultTag.Err);
			return (string)this.Value!;
		}

		public KeyValue AsOk()
		{
			this.ValidateTag(ResultTag.Ok);
			return (KeyValue)this.Value!;
		}

		private void ValidateTag(ResultTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum ResultTag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}