using EdjCase.ICP.Candid.Mapping;
using Rogues.ClickerGame.Models;
using System.Collections.Generic;
using System;

namespace Rogues.ClickerGame.Models
{
	[Variant]
	public class Result1
	{
		[VariantTagProperty]
		public Result1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result1(Result1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result1()
		{
		}

		public static Result1 Err(string info)
		{
			return new Result1(Result1Tag.Err, info);
		}

		public static Result1 Ok(List<PlayerAchievement> info)
		{
			return new Result1(Result1Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result1Tag.Err);
			return (string)this.Value!;
		}

		public List<PlayerAchievement> AsOk()
		{
			this.ValidateTag(Result1Tag.Ok);
			return (List<PlayerAchievement>)this.Value!;
		}

		private void ValidateTag(Result1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result1Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}