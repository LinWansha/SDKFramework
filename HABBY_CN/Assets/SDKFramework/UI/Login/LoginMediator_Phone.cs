using System.Text.RegularExpressions;
using SDKFramework.UI;

public partial class LoginMediator : UIMediator<LoginView>
{
    public string m_PhoneNum;
    private void InputPhoneNum(string phoneNum)
    {
        bool IsValidPhoneNumber(string phoneNumber)
        {
            //TODO：确认正则是否可行？是不是需要与服务器交互？
            string pattern = @"^1[3456789]\d{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        Regex regex = new Regex("[^0-9]");
        string modifiedStr = regex.Replace(phoneNum, "");

        if (modifiedStr.Length > 11)
        {
            modifiedStr = modifiedStr.Substring(0, 11);
        }

        // 更新输入框内容，如果不与当前内容相同的话
        if (phoneNum != modifiedStr)
        {
            View.phoneNumInput.text = modifiedStr;
            View.phoneNumInput.caretPosition = modifiedStr.Length; // 设置光标位置
        }

        View.btnNext.interactable = IsValidPhoneNumber(modifiedStr);
        m_PhoneNum = View.btnNext.interactable ? phoneNum : "";
    }

    protected override void OnHide()
    {
        View.ClearInputField();
        base.OnHide();
    }
}