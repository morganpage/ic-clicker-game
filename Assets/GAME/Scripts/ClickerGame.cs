using UnityEngine;
//using My.Namespace.MyClient.Models;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System.Linq;
using Rogues.ClickerGame;
using Rogues.ClickerGame.Models;

public class ClickerGame : MonoBehaviour
{
  public static event System.Action<int> OnClickCountChanged;
  public static event System.Action<string[]> OnAchievementsChanged;
  public static event System.Action<string[]> OnRewardsChanged;
  [SerializeField] private Login _login;

  void OnEnable()
  {
    Login.OnPlayerPrincipalChanged += OnPlayerPrincipalChanged;
  }
  void OnDisable()
  {
    Login.OnPlayerPrincipalChanged -= OnPlayerPrincipalChanged;
  }

  public async void Click()
  {
    var result = await _login.Client.Click();
    PlayerAchievement p = result.AsOk();
    Debug.Log($"{p.Progress} , {p.Player}, {p.Id}");
    OnClickCountChanged?.Invoke((int)p.Progress);
    GetPlayerAchievements();
    GetGameRewards();
  }

  private async void OnPlayerPrincipalChanged(string playerPrincipal)//Player logged in so we can get the clicks
  {
    UnboundedUInt clicks = await _login.Client.GetClicks();
    OnClickCountChanged?.Invoke((int)clicks);
    GetPlayerAchievements();
    GetGameRewards();
  }

  public async void GetPlayerAchievements()
  {
    var result = await _login.Client.GetPlayerAchievements();
    List<PlayerAchievement> achievements = result.AsOk();
    foreach (PlayerAchievement achievement in achievements)
    {
      Debug.Log($"{achievement.AchievementName} , {achievement.Progress} , {achievement.Earned}");
    }
    OnAchievementsChanged?.Invoke(achievements.Where(a => a.Earned).Select(a => a.AchievementName).ToArray());
  }

  public async void GetGameRewards()
  {
    var rewardsJSON = await _login.Client.GetGameRewards();
    Rewards rewards = JsonUtility.FromJson<Rewards>(rewardsJSON);
    OnRewardsChanged?.Invoke(rewards.rewards);
    //Debug.Log(rewards);
  }

  public class Rewards
  {
    public string[] rewards;
  }

}
