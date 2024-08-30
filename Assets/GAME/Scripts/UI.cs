using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI textPlayerPrincipal;
  [SerializeField] private TextMeshProUGUI textCount;
  [SerializeField] private TextMeshProUGUI textAchievementsAndRewards;
  [SerializeField] private Button clickButton;
  private string[] _achievements = new string[0];
  private string[] _rewards = new string[0];

  void Awake()
  {
    clickButton.interactable = false;
  }

  void OnEnable()
  {
    Login.OnPlayerPrincipalChanged += OnPlayerPrincipalChanged;
    ClickerGame.OnClickCountChanged += OnClickCountChanged;
    ClickerGame.OnAchievementsChanged += OnAchievementsChanged;
    ClickerGame.OnRewardsChanged += OnRewardsChanged;
  }
  void OnDisable()
  {
    Login.OnPlayerPrincipalChanged -= OnPlayerPrincipalChanged;
    ClickerGame.OnClickCountChanged -= OnClickCountChanged;
    ClickerGame.OnAchievementsChanged -= OnAchievementsChanged;
    ClickerGame.OnRewardsChanged -= OnRewardsChanged;
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
  }

  void UpdateAchievementsAndRewards()
  {
    textAchievementsAndRewards.text = $"Achievements:\n{string.Join("\n", _achievements)} \n\nRewards:\n{string.Join("\n", _rewards)}";
  }

}
