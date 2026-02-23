using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace SephiriaModList.UI
{
    public class UI_ModPausePanel : MonoBehaviour
    {
        public UI_PausePanel PausePanel;
        public UI_ModOptionsPanel ModOptionsPanel;

        public GameObject settingButton;
        public UI_HorayButton settingHorayButton;
        public UI_CombineLocalizationStringText settingText;
        public void Init(UI_PausePanel panel)
        {
            PausePanel = panel;
            ModOptionsPanel = UI_ModOptionsPanel.Create(panel);
        }
        public void Start()
        {
            settingButton = Instantiate(PausePanel.giveupButton, PausePanel.giveupButton.transform.parent);
            settingButton.name = "ModOptionButton";
            settingButton.transform.SetSiblingIndex(5);
            settingButton.SetActive(true);
            settingHorayButton = settingButton.GetComponent<UI_HorayButton>();
            settingHorayButton.onClick = new Button.ButtonClickedEvent();
            settingHorayButton.onClick.AddListener(OpenModSetting);
            var text = settingButton.transform.GetChild(0).GetComponent<UI_LocalizationStringText>();
            settingText = settingButton.AddComponent<UI_CombineLocalizationStringText>();
            settingText.From(text);
            Destroy(text);
            settingText.leftString = "Mod ";
            settingText.UpdateKey("UI_PausePanel_OptionsButton");
        }

        public void OpenModSetting()
        {
            ModOptionsPanel.Open();
        }

        [HarmonyPatch(typeof(UI_PausePanel), nameof(UI_PausePanel.Connect))]
        public static class InitPatch
        {
            static void Postfix(UI_PausePanel __instance)
            {
                if (__instance.gameObject.TryGetComponent<UI_ModPausePanel>(out _))
                    return;

                __instance.AddComponent<UI_ModPausePanel>().Init(__instance);
            }
        }
    }
}
