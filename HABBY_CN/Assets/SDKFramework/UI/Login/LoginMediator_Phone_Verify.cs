using System.Collections;
using SDKFramework.UI;
using UnityEngine;

public partial class LoginMediator : UIMediator<LoginView>
{
    private void SendSMSVerificationCode()
    {
        string phoneNum = "15610937070";
        View.waitObj.SetActive(true);
        View.btnSend.gameObject.SetActive(false);
        View.showNumText.text = $"    已发送至\n{phoneNum}";
        View.StartCoroutine(StartCountdown(60));
    }

    private IEnumerator StartCountdown(int duration)
    {
        int remainingSeconds = duration;

        while (remainingSeconds > 0)
        {
            View.resendText.text = $"{remainingSeconds}秒后可重发";
            yield return new WaitForSeconds(1);
            remainingSeconds--;
        }

        View.waitObj.SetActive(false);
        View.btnSend.gameObject.SetActive(true);
        View.showNumText.text = "请输入手机验证码";
    }
    
    // private int currentInputIndex = 0;
    private string previousText = "";
    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        
        // // 检查按下的按键是否为数字（ASCII码：48-57）
        // for (int key = 48; key <= 57; key++)
        // {
        //     if (Input.GetKeyDown((KeyCode)key))
        //     {
        //         // 设置Text为输入的数字
        //         char digit = (char)key;
        //         View.verifyCodeInput[currentInputIndex].text = digit.ToString();
        //
        //         // 移动到下一个输入框（如果有）
        //         currentInputIndex = Mathf.Min(currentInputIndex + 1,  View.verifyCodeInput.Count - 1);
        //     }
        // }
        
        // 处理退格键
        // if (Input.GetKeyDown(KeyCode.Backspace))
        // {
        //     if (currentInputIndex > 0)
        //     {
        //         // 移动到前一个输入框并清除
        //         View.verifyCodeInput[currentInputIndex].text = "";
        //         currentInputIndex--;
        //     }
        //     else if (currentInputIndex == 0)
        //     {
        //         // 清除第一个输入框的内容
        //         View.verifyCodeInput[currentInputIndex].text = "";
        //     }
        // }
        
        // // 处理退格键
        // if (Input.GetKeyDown(KeyCode.Backspace))
        // {
        //     if (allowBackspace)
        //     {
        //         if (currentInputIndex > 0)
        //         {
        //             // 移动到前一个输入框并清除
        //             currentInputIndex--;
        //             View.verifyCodeInput[currentInputIndex].text = "";
        //         }
        //         else if (currentInputIndex == 0)
        //         {
        //             // 清除第一个输入框的内容
        //             View.verifyCodeInput[currentInputIndex].text = "";
        //         }
        //     }
        //     else
        //     {
        //         allowBackspace = true; // 当退格键首次按下时，将其设置为true
        //     }
        // }
        // else if (!Input.GetKey(KeyCode.Backspace))
        // {
        //     allowBackspace = false; // 如果按下的键不是退格键，则设置为false
        // }
    }

    private void HandleCodeInput(string str)
    {
        Debug.Log(str);
        // HLogger.Log(str.Length.ToString(),Color.blue);
        // HLogger.Log(currentInputIndex.ToString(),Color.blue);
        // HLogger.Log( View.verifyCodeInput.Count.ToString(),Color.blue);

        // if (str.Length > 0 && currentInputIndex < View.verifyCodeInput.Count)
        // {
        //     char lastChar = str[str.Length - 1];
        //
        //     if (char.IsDigit(lastChar))
        //     {
        //         View.verifyCodeInput[currentInputIndex].text = lastChar.ToString();
        //         currentInputIndex = Mathf.Min(currentInputIndex + 1, View.verifyCodeInput.Count - 1);
        //         Debug.LogError(currentInputIndex);
        //     }
        //
        //     View.hideInput.text = "";
        // }

        
        string inputText = "";
        foreach (char c in str)
        {
            if (char.IsDigit(c))
            {
                inputText += c;
            }
        }

        if (!str.Equals(inputText))
        {
            View.hideInput.text = inputText;
            View.hideInput.caretPosition = inputText.Length;  // 设置光标位置
        }

        for (int i = 0; i < View.verifyCodeInput.Count; i++)
        {
            if (i < inputText.Length)
            {
                View.verifyCodeInput[i].text = inputText[i].ToString();
            }
            else
            {
                View.verifyCodeInput[i].text = "";
            }
        }
        
        // if (str.Length < previousText.Length)
        // {
        //     //currentInputIndex--;
        //     Debug.LogError("Backspace key pressed:::"+currentInputIndex);
        //     View.verifyCodeInput[currentInputIndex].text = "";
        //     currentInputIndex--;
        // }
        // previousText = str;

        
        // if (str.Length > 0)
        // {
        //     // 获取新输入的字符
        //     char lastChar = str[str.Length - 1];
        //
        //     if (char.IsDigit(lastChar))
        //     {
        //         View.verifyCodeInput[currentInputIndex].text = lastChar.ToString();
        //
        //         // 移动到下一个输入框（如果有）
        //         if (currentInputIndex < View.verifyCodeInput.Count - 1)
        //         {
        //             currentInputIndex++;
        //             allowBackspace = false; // 输入时禁用退格，避免误删
        //         }
        //     }
        // }
        //
        // View.hideInput.text = "";
        
//         
//         if (str.Length == 0 && previousText.Length == 1 && currentInputIndex > 0) // 检查是否退格
//         {
//             currentInputIndex++;
//             //View.verifyCodeInput[currentInputIndex].text = "";
//             previousText = str;
//             Debug.LogError("?");
//         }
//         else if (str.Length > 0 && previousText.Length == 0)
//         {            Debug.LogError("?");
//
//             char lastChar = str[str.Length - 1];
//
//             if (char.IsDigit(lastChar))
//             {
//                 View.verifyCodeInput[currentInputIndex].text = lastChar.ToString();
//                 Debug.LogError("?");
// Debug.LogError(View.verifyCodeInput[currentInputIndex].text+"@@@@@");
//                 // 移动到下一个输入框（如果有），但不超过最大索引
//                 if (currentInputIndex < View.verifyCodeInput.Count - 1)
//                 {
//                     currentInputIndex++;
//                 }
//             }
//
//             // 记录字符串以便在下一个变化时进行比较
//             previousText = str;
//         }
//         else
//         {
//             Debug.LogError("?");
//             // 如果文本长度没有发生增减，则将previousText设置为当前值
//             previousText = str;
//         }
//         // 清空隐藏输入框，以便获得下一个字符
//         View.hideInput.text = "";
    }

    private void HandleCodeDelete(string str)
    {
        // 检测按下的键是否为删除键
        if (str.Length < previousText.Length) 
        {
            Debug.Log("Backspace key pressed");
        }
        // 编写其他处理逻辑如检测数字输入等

        previousText = str;
    }
}
//推翻以前的设计，我的数据结构是 List<InputField> verifyCodeInput;集合里存了四个输入框，每个输入框中只能输入一个字且只能是数字，当四个输入框全部输完之后