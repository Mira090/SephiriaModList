using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace SephiriaModList.UI
{
    public class UI_CombineLocalizationStringText : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public LocalizedString valueString = new LocalizedString("");

        public string leftString = string.Empty;
        public string rightString = string.Empty;

        public bool enableTagParsing;

        private void OnEnable()
        {
            LocalizationManager.Instance.OnLanguageChanged += HandleLanguageChanged;
            UpdateText();
        }

        private void OnDisable()
        {
            LocalizationManager.Instance.OnLanguageChanged -= HandleLanguageChanged;
        }

        public void From(UI_LocalizationStringText localizationText)
        {
            text = localizationText.text;
            valueString = localizationText.valueString;
            enableTagParsing = localizationText.enableTagParsing;
            UpdateText();
        }

        public void UpdateKey(string key)
        {
            valueString.key = key;
            UpdateText();
        }

        private void HandleLanguageChanged(string language)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (enableTagParsing)
            {
                text.text = leftString + KeywordDatabase.Convert(valueString.ToString()) + rightString;
            }
            else
            {
                text.text = leftString + valueString.ToString() + rightString;
            }
        }
    }
}
