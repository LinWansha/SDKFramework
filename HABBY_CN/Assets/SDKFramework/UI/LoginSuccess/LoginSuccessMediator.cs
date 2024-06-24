using System.Collections;

using SDKFramework.Account;
using SDKFramework.Account.DataSrc;
using SDKFramework.UI;
using SDKFramework.Utils;
using UnityEngine;

public class LoginSuccessMediator : UIMediator<LoginSuccessView>
{
    private Vector2 _startPos = Vector2.zero;
    private Coroutine _animationCoroutine;

    protected override void OnInit()
    {
        base.OnInit();
        _startPos = View.root.anchoredPosition;
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        ShowAnimation();
    }

    public void ShowAnimation()
    {
        UserAccount account = HabbyFramework.Account.CurrentAccount;

        SetChannel(account.LoginChannel);
        View.nameText.text = account.UID;

        var topPixel = Screen.currentResolution.height - (Screen.safeArea.y + Screen.safeArea.height);
        Rect safe = Screen.safeArea;
        float height = Screen.height - safe.height;
        float frigeHeigh = 0;


        var targetPositionY = _startPos.y - View.root.rect.height - frigeHeigh;
        _animationCoroutine = AsyncScheduler.Instance.StartCoroutine(AnimateLoginTip(1f, targetPositionY));
    }

    private IEnumerator AnimateLoginTip(float duration, float targetPositionY)
    {
        CanvasGroup group = View.root.GetComponent<CanvasGroup>();

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            View.root.anchoredPosition = new Vector3(View.root.anchoredPosition.x,
                Mathf.Lerp(_startPos.y, targetPositionY, progress),
                0);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        timer = 0f;

        while (timer < duration * 2f)
        {
            timer += Time.deltaTime;

            float progress = timer / (duration * 2f);

            View.root.anchoredPosition = new Vector3(View.root.anchoredPosition.x,
                Mathf.Lerp(targetPositionY, _startPos.y, progress),
                0);

            if (timer >= duration * 1.8f)
                group.alpha = Mathf.Lerp(1f, 0f, (timer - duration * 1.8f) / (duration * 0.2f));

            yield return null;
        }

        OnAnimationDone();
    }

    private void SetChannel(string channelName)
    {
        View.weixin.SetActive(false);
        View.apple.SetActive(false);
        View.phone.SetActive(false);
        View.qq.SetActive(false);
        switch (channelName)
        {
            case UserAccount.ChannelWeiXin:
                View.weixin.SetActive(true);
                break;
            case UserAccount.ChannelPhone:
                View.phone.SetActive(true);
                break;
            case UserAccount.ChannelAppleId:
                View.apple.SetActive(true);
                break;
            case UserAccount.ChannelQQ:
                View.qq.SetActive(true);
                break;
            default:
                break;
        }
    }

    protected void OnAnimationDone()
    {
        if (_animationCoroutine != null)
        {
            AsyncScheduler.Instance.StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        View.root.anchoredPosition = new Vector2(_startPos.x, _startPos.y);
        Close();
    }
}