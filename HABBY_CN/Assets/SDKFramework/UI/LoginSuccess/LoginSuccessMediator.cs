using System.Collections;
using Habby.CNUser;
using SDKFramework.UI;
using UnityEngine;

public class LoginSuccessMediator : UIMediator<LoginSuccessView>
{
    private Vector3 _startPos = Vector3.zero;
    private Coroutine _animationCoroutine;

    protected override void OnInit(LoginSuccessView view)
    {
        base.OnInit(view);
    }

    protected override void OnShow(object arg)
    {
        base.OnShow(arg);
        ShowAnimation(null);
    }

    public void ShowAnimation(UserAccount account)
    {
        if (account == null)
        {
            account = AccountManager.Instance.CurrentAccount;
            if (account == null)
            {
                account = new UserAccount();
                account.LoginChannel = UserAccount.ChannelWeiXin;
                account.NickName = "xxxx";
            }
        }

        if (account != null)
        {
            SetChannel(account.LoginChannel);
            view.nameText.text = account.NickName;
        }

        var topPixel = Screen.currentResolution.height - (Screen.safeArea.y + Screen.safeArea.height);
        Rect safe = Screen.safeArea;
        float height = Screen.height - safe.height;
        float frigeHeigh = 0;

        //*** if (SDKHubUtils.IsIOS)
        // {
        //     if (topPixel > 0)
        //     {
        //         frigeHeigh = topPixel * transform.localScale.y;
        //     }
        // }
        // else
        // {
        //     frigeHeigh = 55;
        // }
        //
        var targetPositionY = _startPos.y - view.root.rect.height - frigeHeigh;
        _animationCoroutine = CoroutineScheduler.Instance.StartCoroutine(AnimateLoginTip(1f, targetPositionY));
    }

    private IEnumerator AnimateLoginTip(float duration, float targetPositionY)
    {
        CanvasGroup group = view.root.GetComponent<CanvasGroup>();

        var startPosition = view.root.transform.position;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            view.root.transform.position = new Vector3(view.root.transform.position.x,
                Mathf.Lerp(startPosition.y, targetPositionY, progress),
                0);

            yield return null;
        }

        yield return new WaitForSeconds(2);

        timer = 0f;

        while (timer < duration * 2f)
        {
            timer += Time.deltaTime;

            float progress = timer / (duration * 2f);

            view.root.transform.position = new Vector3(view.root.transform.position.x,
                Mathf.Lerp(targetPositionY, startPosition.y, progress),
                0);

            if (timer >= duration * 1.8f)
                group.alpha = Mathf.Lerp(1f, 0f, (timer - duration * 1.8f) / (duration * 0.2f));

            yield return null;
        }

        OnAnimationDone();
    }

    private void SetChannel(string channelName)
    {
        view.weixin.SetActive(false);
        view.apple.SetActive(false);
        view.phone.SetActive(false);
        view.qq.SetActive(false);
        switch (channelName)
        {
            case UserAccount.ChannelWeiXin:
                view.weixin.SetActive(true);
                break;
            case UserAccount.ChannelPhone:
                view.phone.SetActive(true);
                break;
            case UserAccount.ChannelAppleId:
                view.apple.SetActive(true);
                break;
            case UserAccount.ChannelQQ:
                view.qq.SetActive(true);
                break;
            default:
                break;
        }
    }

    protected void OnAnimationDone()
    {
        if (_animationCoroutine != null)
        {
            CoroutineScheduler.Instance.StopCoroutine(_animationCoroutine);
            _animationCoroutine = null;
        }

        view.root.transform.position = new Vector3(_startPos.x, _startPos.y, 0);
    }
}