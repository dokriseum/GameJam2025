using TMPro;
using UnityEngine;

namespace UIManager
{
    public class UIManager : MonoBehaviour
    {
        public TMP_InputField promptInput;
        public TMP_Dropdown modelDropdown;
        public string model;
        public LLMRunner llmRunner;

        public void OnSendRequest()
        {
            string prompt = promptInput.text;
            //string model = modelDropdown.options[modelDropdown.value].text;
            llmRunner.StartRequest(model, prompt);
        }
    }

}