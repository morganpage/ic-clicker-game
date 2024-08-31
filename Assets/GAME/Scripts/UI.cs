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
  [SerializeField] private TMP_InputField inputProfileName;
  [SerializeField] private Button profileNameButton;

  [SerializeField] private UIReward _uiRewardPrefab;
  [SerializeField] private Transform _rewardParent;
  private string[] _achievements = new string[0];
  private string[] _rewards = new string[0];
  private Dictionary<string, Sprite> _rewardSprites = new Dictionary<string, Sprite>();

  void Awake()
  {
    clickButton.interactable = false;
    profileNameButton.interactable = false;
    inputProfileName.interactable = false;
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
    ClickerGame.OnProfileNameChanged += OnProfileNameChanged;
    ClickerGame.OnUpdatePending += OnUpdatePending;
  }
  void OnDisable()
  {
    Login.OnPlayerPrincipalChanged -= OnPlayerPrincipalChanged;
    ClickerGame.OnClickCountChanged -= OnClickCountChanged;
    ClickerGame.OnAchievementsChanged -= OnAchievementsChanged;
    ClickerGame.OnRewardsChanged -= OnRewardsChanged;
    ClickerGame.OnAvailableRewardsChanged -= OnAvailableRewardsChanged;
    ClickerGame.OnProfileNameChanged -= OnProfileNameChanged;
    ClickerGame.OnUpdatePending -= OnUpdatePending;
  }

  void OnPlayerPrincipalChanged(string playerPrincipal)
  {
    textPlayerPrincipal.text = $"Logged in as: {playerPrincipal}";
    clickButton.interactable = true;
    profileNameButton.interactable = true;
    inputProfileName.interactable = true;
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
  }

  async void OnProfileNameChanged(string profileName)
  {
    inputProfileName.text = profileName;
    profileNameButton.interactable = true;
    inputProfileName.interactable = true;
  }

  async void OnUpdatePending()
  {
    profileNameButton.interactable = false;
    inputProfileName.interactable = false;
  }


}
