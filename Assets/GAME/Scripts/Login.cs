using UnityEngine;
using System.Runtime.InteropServices;
using EdjCase.ICP.Agent.Agents;
using EdjCase.ICP.Candid.Models;
using EdjCase.ICP.Agent.Identities;
using EdjCase.ICP.Agent.Responses;
using System.Threading.Tasks;
using System.Collections.Generic;
using EdjCase.ICP.Candid;
using EdjCase.ICP.Candid.Utilities;
using TMPro;
using Newtonsoft.Json;
using IC.GameKit;
using EdjCase.ICP.Agent;
using EdjCase.ICP.Agent.Models;
using System;
using System.Threading.Tasks;
using WebSocketServer;
using Rogues.ClickerGame;
using Rogues.ClickerGame.Models;

public class Login : MonoBehaviour
{
  public static event Action<string> OnPlayerPrincipalChanged;
  Ed25519Identity mEd25519Identity = null;
  DelegationIdentity mDelegationIdentity = null;
  public string sessionkey = "";
  public string unity_login = "https://gl6wc-baaaa-aaaam-adb3a-cai.icp0.io/"; //Needed to enable internet identity login
  public string backendCanister = "mdx42-oyaaa-aaaam-ac2ba-cai"; //The clicker game motoko canister

  public Principal BackEndCanisterPrincipal => Principal.FromText(backendCanister);

  public ClickerGameApiClient Client
  {
    get
    {
      if (client == null)
      {
        var httpClient = new UnityHttpClient();
        var agent = new HttpAgent(httpClient, mDelegationIdentity);
        client = new ClickerGameApiClient(agent, BackEndCanisterPrincipal);
      }
      return client;
    }
  }

  private ClickerGameApiClient client;

  [DllImport("__Internal")]
  private static extern void Initialize();

  [DllImport("__Internal")]
  private static extern void ShowLoginIframe(string url);

  [DllImport("__Internal")]
  private static extern void HideLoginIframe();


  void Start()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    Initialize(); //Sets up a istener for the postMessage from the unity_login page
#endif
    mEd25519Identity = Ed25519Identity.Generate();
    sessionkey = ByteUtilPublic.ToHexString(mEd25519Identity.PublicKey.ToDerEncoding());
    Debug.Log("sessionkey: " + sessionkey);
    OpenBrowser();//Immediately tries to login via browser
  }
  public void OpenBrowser()
  {
    var target = unity_login + "?sessionkey=" + sessionkey;
#if UNITY_WEBGL && !UNITY_EDITOR
    ShowLoginIframe(target);
#else
    Application.OpenURL(target);
#endif
  }
  public void HandleJsonDelegation(string jsonDelegation)
  {
    mDelegationIdentity = ConvertJsonToDelegationIdentity(jsonDelegation);
    if (mDelegationIdentity != null)
    {
      GetPlayerPrincipal();
    }
#if UNITY_WEBGL && !UNITY_EDITOR
    HideLoginIframe();
#endif
  }

  public void CanisterClickAnon()
  {
    CallCanisterClickAnon();
  }

  public async Task CallCanisterClickAnon() //Just for an example, this is not used in the project
  {
    var httpClient = new UnityHttpClient();
    var agent = new HttpAgent(httpClient);
    var client = new ClickerGameApiClient(agent, BackEndCanisterPrincipal);
    var result = await client.Click();
    PlayerAchievement p = result.AsOk();
    Debug.Log(p.Progress);
  }


  internal DelegationIdentity ConvertJsonToDelegationIdentity(string jsonDelegation)
  {
    Debug.Log(jsonDelegation);
    var delegationChainModel = JsonConvert.DeserializeObject<DelegationChainModel>(jsonDelegation);
    if (delegationChainModel == null && delegationChainModel.delegations.Length == 0)
    {
      Debug.LogError("Invalid delegation chain.");
      return null;
    }

    // Initialize DelegationIdentity.
    var delegations = new List<SignedDelegation>();
    foreach (var signedDelegationModel in delegationChainModel.delegations)
    {
      var pubKey = SubjectPublicKeyInfo.FromDerEncoding(ByteUtilPublic.FromHexString(signedDelegationModel.delegation.pubkey));
      var expiration = ICTimestamp.FromNanoSeconds(Convert.ToUInt64(signedDelegationModel.delegation.expiration, 16));
      var delegation = new Delegation(pubKey, expiration);

      var signature = ByteUtilPublic.FromHexString(signedDelegationModel.signature);
      var signedDelegation = new SignedDelegation(delegation, signature);
      delegations.Add(signedDelegation);
    }

    var chainPublicKey = SubjectPublicKeyInfo.FromDerEncoding(ByteUtilPublic.FromHexString(delegationChainModel.publicKey));
    var delegationChain = new DelegationChain(chainPublicKey, delegations);
    var delegationIdentity = new DelegationIdentity(mEd25519Identity, delegationChain);

    return delegationIdentity;
  }

  private async void GetPlayerPrincipal()
  {
    Principal p = await Client.PlayerPrincipal();
    OnPlayerPrincipalChanged?.Invoke(p.ToText());
  }


  #region WebSocketCallbacks
  /// <summary>
  /// The code below is for the WebSocket server, only used in the Unity Editor for ease of testing, webGL builds use postMessage instead to pass the delegated identity.
  /// </summary>
  public void OnOpenWS(WebSocketConnection connection)
  {
    Debug.Log("OnOpenWS");
  }

  public void OnMessageWS(WebSocketMessage message)
  {
    Debug.Log("OnMessageWS: " + message.data);
    HandleJsonDelegation(message.data);
  }
  #endregion
}
