using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VerifyCodeTool : MonoBehaviour
{
    public string Text { get; set; }
    [SerializeField] private const int VERIFY_CODE_LENGTH = 4;
    [SerializeField] private InputField VCodeInput;
    
    public GameObject[] VCodes = new GameObject[VERIFY_CODE_LENGTH];
    private readonly Text[] VCodesText = new Text[VERIFY_CODE_LENGTH];
    private readonly Image[] VCodesFrame = new Image[VERIFY_CODE_LENGTH];
    private int m_lastVcodeLength = 0;
    private bool isFocused = false;
    private bool isFullInput = false;

    public delegate void OnInputValueChanged(string character);

    public event OnInputValueChanged OnInputValueChangedEvent;

    public void SetFocusToInputField()
    {
        StartCoroutine(FocusInputField());
    }

    IEnumerator FocusInputField()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(VCodeInput.gameObject, null);
        VCodeInput.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    void Awake()
    {
        for (int i = 0; i < VERIFY_CODE_LENGTH; i++)
        {
            VCodesText[i] = VCodes[i].GetComponentInChildren<Text>();
            VCodesText[i].text = string.Empty;
            VCodesFrame[i] = VCodes[i].transform.Find("Image").GetComponent<Image>();
            VCodesFrame[i].gameObject.SetActive(false);
        }

        VCodesFrame[0].gameObject.SetActive(true);
        VCodeInput.onValueChanged.AddListener(OnVCodeValueChanged);
    }

    private void OnEnable()
    {
        VCodeInput.text = string.Empty;
        for (int i = 0; i < VERIFY_CODE_LENGTH; i++)
        {
            VCodesText[i].text = string.Empty;
        }
    }

    void Update()
    {
        if (isFocused != VCodeInput.isFocused)
        {
            if (VCodeInput.isFocused)
            {
                CheckShowSelectBar();
                // VCodeInput.caretPosition = VCodeInput.text.Length;
                VCodeInput.selectionAnchorPosition = VCodeInput.text.Length;
                VCodeInput.selectionFocusPosition = VCodeInput.text.Length;
            }
            else
            {
                CheckHideSelectBar();
            }

            isFocused = VCodeInput.isFocused;
        }
    }

    private void OnVCodeValueChanged(string value)
    {
        // Debug.Log("OnVCodeValueChanged:" + value);
        if (value.Length > VERIFY_CODE_LENGTH)
        {
            VCodeInput.text = value.Substring(0, VERIFY_CODE_LENGTH);
            // Debug.Log("OnVCodeValueFixed:" + value.Substring(0, VERIFY_CODE_LENGTH));
            return;
        }

        Text = value;
        OnInputValueChangedEvent?.Invoke(value);

        bool next = false;

        string character = string.Empty;

        if (value.Length > m_lastVcodeLength)
        {
            next = true;
            character = value[value.Length - 1].ToString();
        }

        m_lastVcodeLength = value.Length;
        int which = value.Length - 1;
        OnMoveCursor(next, which, character);

        if (value.Length == VERIFY_CODE_LENGTH && !isFullInput)
        {
            // StartCoroutine(DeactivateInputField());  //todo：根据业务需求启用改功能
        }
    }

    IEnumerator DeactivateInputField()
    {
        yield return new WaitForEndOfFrame();
        Log.Info("DeactivateInputField");
        VCodeInput.DeactivateInputField();
    }

    private void OnMoveCursor(bool next, int which, string character)
    {
        // Debug.Log("next + which + character:" + next + which + character);
        //当最后一个格子填入字符后，不做边框显隐处理
        if (next)
        {
            VCodesText[which].text = character;
            if (which < VERIFY_CODE_LENGTH - 1)
            {
                RefreshFrame(which + 1);
            }
        }
        else
        {
            for (int i = which; i < VERIFY_CODE_LENGTH - 1; ++i)
            {
                VCodesText[i + 1].text = character;
            }

            if (which < VERIFY_CODE_LENGTH - 1)
            {
                RefreshFrame(which + 1);
                isFullInput = false;
            }
        }
    }

    private void OnFocusInput()
    {
    }

    private void RefreshFrame(int which)
    {
        for (int i = 0; i < VERIFY_CODE_LENGTH; i++)
        {
            if (i == which)
            {
                VCodesFrame[i].gameObject.SetActive(true);
            }
            else
            {
                VCodesFrame[i].gameObject.SetActive(false);
            }
        }
    }

    private void CheckShowSelectBar()
    {
        if (VCodeInput.text.Length >= VERIFY_CODE_LENGTH)
        {
            VCodesFrame[VERIFY_CODE_LENGTH - 1].gameObject.SetActive(true);
            isFullInput = true;
        }
    }

    private void CheckHideSelectBar()
    {
        if (VCodeInput.text.Length >= VERIFY_CODE_LENGTH)
        {
            VCodesFrame[VERIFY_CODE_LENGTH - 1].gameObject.SetActive(false);
            isFullInput = false;
        }
    }
}