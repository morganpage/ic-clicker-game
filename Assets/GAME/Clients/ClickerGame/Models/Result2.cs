using EdjCase.ICP.Candid.Mapping;
using Rogues.ClickerGame.Models;
using System;

namespace Rogues.ClickerGame.Models
{
	[Variant]
	public class Result2
	{
		[VariantTagProperty]
		public Result2Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Result2(Result2Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Result2()
		{
		}

		public static Result2 Err(string info)
		{
			return new Result2(Result2Tag.Err, info);
		}

		public static Result2 Ok(PlayerAchievement info)
		{
			return new Result2(Result2Tag.Ok, info);
		}

		public string AsErr()
		{
			this.ValidateTag(Result2Tag.Err);
			return (string)this.Value!;
		}

		public PlayerAchievement AsOk()
		{
			this.ValidateTag(Result2Tag.Ok);
			return (PlayerAchievement)this.Value!;
		}

		private void ValidateTag(Result2Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum Result2Tag
	{
		[CandidName("err")]
		Err,
		[CandidName("ok")]
		Ok
	}
}