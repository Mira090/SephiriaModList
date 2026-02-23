using HeathenEngineering.SteamworksIntegration;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace SephiriaModList.UI
{
    public class UI_OptionBox : MonoBehaviour
    {
        public UI_LocalizationStringText valueText;
        public TextMeshProUGUI text;

        public List<LocalizedString> valueDescriptions = [];
        public List<string> valueRawDescriptions = [];

        public UI_HorizontalSelectionBox box;

        public string OptionKey = "";

        public OptionData Data;

        private void OnEnable()
        {
            box.OnValueChanged += HandleValueChanged;
        }

        private void OnDisable()
        {
            box.OnValueChanged -= HandleValueChanged;
        }

        private void HandleValueChanged(int idx)
        {
            if (Data.UseLocalization)
                valueText.UpdateKey(valueDescriptions[idx].key);
            else
                text.text = valueRawDescriptions[idx];
            if (string.IsNullOrEmpty(OptionKey))
                return;
            ModOptionsBinding.Instance.ModOptions[OptionKey] = idx;
            ModOptionsBinding.Instance.OnModOptionChanged(Data, idx);
        }

        public void Init(OptionData data)
        {
            Data = data;
            OptionKey = data.OptionKey;
            box.numberOfElements = data.Values.Count;
            valueDescriptions.Clear();
            valueRawDescriptions.Clear();
            foreach (var value in data.Values)
            {
                valueDescriptions.Add(new LocalizedString(value));
                valueRawDescriptions.Add(value);
            }
            valueText.enabled = data.UseLocalization;

            var title = transform.GetChild(0).gameObject;
            if (title.TryGetComponent<UI_LocalizationStringText>(out var t))
            {
                if (data.UseLocalization)
                {
                    t.UpdateKey(data.FullDescription);
                }
                else
                {
                    t.enabled = false;
                    t.text.text = data.FullDescription;
                }
            }

            int current = ModOptionsBinding.Instance.ModOptions.GetInt(OptionKey, Data?.Default ?? 0);
            box.ChangeValueWithoutNotify(current);
            if (Data.UseLocalization)
                valueText.UpdateKey(valueDescriptions[current].key);
            else
                this.text.text = valueRawDescriptions[current];
        }
    }
}
