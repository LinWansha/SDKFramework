using System.Text.RegularExpressions;
using SDKFramework.UI;

public partial class LoginMediator : UIMediator<LoginView>
{
    private string m_PhoneNum = "";

    private void InputPhoneNum(string phoneNum)
    {
        bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^1[3456789]\d{9}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        Regex regex = new Regex("[^0-9]");
        string modifiedStr = regex.Replace(phoneNum, "");

        if (modifiedStr.Length > 11)
        {
            modifiedStr = modifiedStr.Substring(0, 11);
        }

        if (phoneNum != modifiedStr)
        {
            View.phoneNumInput.text = modifiedStr;
            View.phoneNumInput.caretPosition = modifiedStr.Length; 
        }
        
        View.btnNext.interactable = IsValidPhoneNumber(modifiedStr);
        m_PhoneNum = View.btnNext.interactable ? phoneNum : "";
    }
}