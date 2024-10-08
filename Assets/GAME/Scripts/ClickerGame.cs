using UnityEngine;
using EdjCase.ICP.Candid.Models;
using System.Collections.Generic;
using System.Linq;
using Rogues.ClickerGame;
using Rogues.ClickerGame.Models;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;


public class ClickerGame : MonoBehaviour
{
  public static event Action<int> OnClickCountChanged;
  public static event Action<string[]> OnAchievementsChanged;
  public static event Action<string[]> OnRewardsChanged;
  public static event Action<Dictionary<string, Sprite>> OnAvailableRewardsChanged;
  public static event Action<string> OnProfileNameChanged;
  public static event Action OnUpdatePending;
  [SerializeField] private Login _login;
  public string _profileNameLocal;//Local cache updated by input field changes

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
    await GetAvailableRewards();
    GetPlayerAchievements();
    GetGameRewards();
    GetProfileName();
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
    var resultJSON = await _login.Client.GetGameRewards();
    Rewards rewards = JsonUtility.FromJson<Rewards>(resultJSON);
    if (rewards != null && rewards.rewards != null) OnRewardsChanged?.Invoke(rewards.rewards);
  }

  public async Task GetAvailableRewards()
  {
    var resultJSON = await _login.Client.GetAvailableGameRewards();
    Debug.Log(resultJSON);
    AvailableRewards rewards = JsonUtility.FromJson<AvailableRewards>(resultJSON);
    foreach (AvailableRewards.Pet pet in rewards.pets)
    {
      Debug.Log($"{pet.name} , {pet.url}");
    }
    Dictionary<string, Sprite> rewardSprites = new Dictionary<string, Sprite>();
    for (int i = 0; i < rewards.pets.Length; i++)
    {
      string petName = rewards.pets[i].name;
      string petUrl = rewards.pets[i].url;
      if (!rewardSprites.ContainsKey(petName))
      {
        Sprite sprite = await GetSpriteAsync(petUrl);
        rewardSprites.Add(petName, sprite);
      }
    }
    OnAvailableRewardsChanged?.Invoke(rewardSprites);
  }




  async Task<Sprite> GetSpriteAsync(string imageUrl)
  {
    var request = UnityWebRequestTexture.GetTexture(imageUrl);
    await request.SendWebRequest();
    while (!request.isDone) await Task.Yield();
    Texture2D texture = DownloadHandlerTexture.GetContent(request);
    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    return sprite;
  }

  public void UpdateLocalProfileName(string profileName)
  {
    _profileNameLocal = profileName;
  }

  public async void UpdateProfileName()
  {
    OnUpdatePending?.Invoke();
    Result result = await _login.Client.UpdateProfileName(_profileNameLocal);
    KeyValue profileNameKV = result.AsOk();
    Debug.Log(profileNameKV.Value);
    OnProfileNameChanged?.Invoke(profileNameKV.Value);
  }

  private async Task GetProfileName()
  {
    Result result = await _login.Client.GetProfileName();
    KeyValue profileNameKV = result.AsOk();
    Debug.Log(profileNameKV.Value);
    OnProfileNameChanged?.Invoke(profileNameKV.Value);
  }


  public class Rewards
  {
    public string[] rewards;
  }

  public class AvailableRewards
  {
    [System.Serializable]
    public class Pet
    {
      public string name;
      public string url;
    }
    public Pet[] pets;
  }

}
