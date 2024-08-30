using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Candid;
using System.Threading.Tasks;
using Rogues.ClickerGame;
using EdjCase.ICP.Agent.Responses;

namespace Rogues.ClickerGame
{
	public class ClickerGameApiClient
	{
		public IAgent Agent { get; }

		public Principal CanisterId { get; }

		public CandidConverter? Converter { get; }

		public ClickerGameApiClient(IAgent agent, Principal canisterId, CandidConverter? converter = default)
		{
			this.Agent = agent;
			this.CanisterId = canisterId;
			this.Converter = converter;
		}

		public async Task<string> CheckForReward()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "checkForReward", arg);
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.Result2> Click()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "click", arg);
			return reply.ToObjects<Models.Result2>(this.Converter);
		}

		public async Task<Principal> GameCanisterPrincipal()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "gameCanisterPrincipal", arg);
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<string> GetAvailableGameRewards()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getAvailableGameRewards", arg);
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<UnboundedUInt> GetClicks()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getClicks", arg);
			return reply.ToObjects<UnboundedUInt>(this.Converter);
		}

		public async Task<string> GetGameRewards()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getGameRewards", arg);
			return reply.ToObjects<string>(this.Converter);
		}

		public async Task<Models.Result1> GetPlayerAchievements()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getPlayerAchievements", arg);
			return reply.ToObjects<Models.Result1>(this.Converter);
		}

		public async Task<Models.Result> GetProfileName()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "getProfileName", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}

		public async Task<bool> IsGameCanisterAdmin()
		{
			CandidArg arg = CandidArg.FromCandid();
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "isGameCanisterAdmin", arg);
			return reply.ToObjects<bool>(this.Converter);
		}

		public async Task<Principal> PlayerPrincipal()
		{
			CandidArg arg = CandidArg.FromCandid();
			QueryResponse response = await this.Agent.QueryAsync(this.CanisterId, "playerPrincipal", arg);
			CandidArg reply = response.ThrowOrGetReply();
			return reply.ToObjects<Principal>(this.Converter);
		}

		public async Task<Models.Result> UpdateProfileName(string arg0)
		{
			CandidArg arg = CandidArg.FromCandid(CandidTypedValue.FromObject(arg0, this.Converter));
			CandidArg reply = await this.Agent.CallAndWaitAsync(this.CanisterId, "updateProfileName", arg);
			return reply.ToObjects<Models.Result>(this.Converter);
		}
	}
}