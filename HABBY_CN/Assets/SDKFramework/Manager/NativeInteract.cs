using System.Collections;
using UnityEngine;

public class NativeInteract : MonoBehaviour
{
    public static NativeInteract Instance { get; private set; }
    public static bool sdkLoginFinish = false;

    private static bool _xiaomiTestHttp = false;

    private void Awake()
    {
        Instance = this;
    }

    private void loginFail(string msg)
    {
    }

    private void loginCancel(string msg)
    {
    }

    public class LoginData
    {
        public string uid = string.Empty;
        public string token = string.Empty;
        public string verification = string.Empty;

        public int yybType;
        public string yybOpenId;
        public string yybOpenKey;
        public string yybPf;
        public string yybPfKey;
    }

    public LoginData _loginData = new LoginData();

    /// <summary>
    /// 登录成功
    /// </summary>
    /// <param name="strResult"></param>
    private void loginSuccess(string strResult)
    {
    }

    private void UC_RequestUserID(bool res, string msg)
    {
    }

    private void DoLogin(string uid)
    {
    }

    /// <summary>
    /// 支付成功
    /// </summary>
    /// <param name="strResult"></param>
    private void paySuccess(string json)
    {
        Debug.Log("paySuccess：" + json);
    }

    private void payFail(string msg)
    {
        Debug.Log("payFail：" + msg);
    }

    private void payCancel(string msg)
    {
        Debug.Log("payCancel：" + msg);
    }

    private void payAnalyze(string msg)
    {
        Debug.Log("payAnalyze：" + msg);
    }

    public void OnAdLoad(string adId)
    {
        Debug.Log("OnAdLoad：" + adId);
    }

    public void OnAdLoadFailed(string str)
    {
        Debug.Log("OnAdLoadFailed：" + str);
    }

    private IEnumerator RequestHttp(string url, string adIdStr)
    {
        yield break;
    }

    public void OnAdOpened(string adId)
    {
        Debug.Log("OnAdOpened：" + adId);
    }

    public void OnAdShowFailed(string str)
    {
        Debug.Log("OnAdShowFailed：" + str);
    }

    public void OnAdClose(string adId)
    {
        Debug.Log("OnAdClose：" + adId);
    }

    public void OnAdRewarded(string adId)
    {
        Debug.Log("OnAdRewarded：" + adId);
    }

    public void SetPlatform(string type)
    {
    }

    private void exitGame(string str)
    {
    }

    private void logoutSuccess(string msg)
    {
        Debug.Log("logoutSuccess " + msg);
        sdkLoginFinish = false;
    }
}