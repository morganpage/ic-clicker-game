using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI textPlayerPrincipal;
  [SerializeField] private TextMeshProUGUI textCount;
  [SerializeField] private TextMeshProUGUI textAchievementsAndRewards;
  [SerializeField] private Button clickButton;

  [SerializeField] private UIReward _uiRewardPrefab;
  [SerializeField] private Transform _rewardParent;
  private string[] _achievements = new string[0];
  private string[] _rewards = new string[0];
  private Dictionary<string, Sprite> _rewardSprites = new Dictionary<string, Sprite>();

  void Awake()
  {
    clickButton.interactable = false;
    foreach (Transform child in _rewardParent)
    {
      Destroy(child.gameObject);
    }
  }

  void OnEnable()
  {
    Login.OnPlayerPrincipalChanged += OnPlayerPrincipalChanged;
    ClickerGame.OnClickCountChanged += OnClickCountChanged;
    ClickerGame.OnAchievementsChanged += OnAchievementsChanged;
    ClickerGame.OnRewardsChanged += OnRewardsChanged;
    ClickerGame.OnAvailableRewardsChanged += OnAvailableRewardsChanged;
  }
  void OnDisable()
  {
    Login.OnPlayerPrincipalChanged -= OnPlayerPrincipalChanged;
    ClickerGame.OnClickCountChanged -= OnClickCountChanged;
    ClickerGame.OnAchievementsChanged -= OnAchievementsChanged;
    ClickerGame.OnRewardsChanged -= OnRewardsChanged;
    ClickerGame.OnAvailableRewardsChanged -= OnAvailableRewardsChanged;
  }

  void OnPlayerPrincipalChanged(string playerPrincipal)
  {
    textPlayerPrincipal.text = $"Logged in as: {playerPrincipal}";
    clickButton.interactable = true;
  }

  void OnClickCountChanged(int count)
  {
    textCount.text = $"{count}";
  }

  void OnAchievementsChanged(string[] achievements)
  {
    _achievements = achievements;
    UpdateAchievementsAndRewards();
  }
  void OnRewardsChanged(string[] rewards)
  {
    _rewards = rewards;
    UpdateAchievementsAndRewards();
    foreach (Transform child in _rewardParent)
    {
      Destroy(child.gameObject);
    }
    foreach (string reward in rewards)
    {
      if (_rewardSprites.ContainsKey(reward))
      {
        UIReward uiReward = Instantiate(_uiRewardPrefab, _rewardParent);
        uiReward.SetReward(_rewardSprites[reward]);
      }
    }
  }

  void UpdateAchievementsAndRewards()
  {
    textAchievementsAndRewards.text = $"Achievements:\n{string.Join("\n", _achievements)} \n\nRewards:\n{string.Join("\n", _rewards)} \n\nAvailable Rewards:\n{string.Join("\n", _rewardSprites.Keys)}";
  }

  async void OnAvailableRewardsChanged(Dictionary<string, Sprite> rewardSprites)
  {
    _rewardSprites = rewardSprites;
    UpdateAchievementsAndRewards();
    // _rewardSprites.Clear();
    // for (int i = 0; i < availableRewards.pets.Length; i++)
    // {
    //   string petName = availableRewards.pets[i].name;
    //   string petUrl = availableRewards.pets[i].url;
    //   if (!_rewardSprites.ContainsKey(petName))
    //   {
    //     Sprite sprite = await GetSpriteAsync(petUrl);
    //     _rewardSprites.Add(petName, sprite);
    //   }
    // }
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



}
