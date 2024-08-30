using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIReward : MonoBehaviour
{
  private Image _image;

  void Awake()
  {
    _image = GetComponent<Image>();
  }

  public async void SetReward(Sprite sprite)
  {
    _image.sprite = sprite;
  }


}
