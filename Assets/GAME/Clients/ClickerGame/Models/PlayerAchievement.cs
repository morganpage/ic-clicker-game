using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Rogues.ClickerGame.Models
{
	public class PlayerAchievement
	{
		[CandidName("achievementName")]
		public string AchievementName { get; set; }

		[CandidName("earned")]
		public bool Earned { get; set; }

		[CandidName("gameName")]
		public string GameName { get; set; }

		[CandidName("id")]
		public string Id { get; set; }

		[CandidName("player")]
		public string Player { get; set; }

		[CandidName("progress")]
		public UnboundedUInt Progress { get; set; }

		[CandidName("updated")]
		public Time Updated { get; set; }

		public PlayerAchievement(string achievementName, bool earned, string gameName, string id, string player, UnboundedUInt progress, Time updated)
		{
			this.AchievementName = achievementName;
			this.Earned = earned;
			this.GameName = gameName;
			this.Id = id;
			this.Player = player;
			this.Progress = progress;
			this.Updated = updated;
		}

		public PlayerAchievement()
		{
		}
	}
}