using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SephiriaModList
{
    public class ModOptionsBinding : MonoBehaviour
    {
        public static ModOptionsBinding Instance { get; private set; }

        public SaveData ModOptions { get; private set; }

        public event Action OnOptionsChanged;

        private void Awake()
        {
            Instance = this;
            LoadOptions();
        }

        public void LoadOptions()
        {
            ModOptions = new SaveData(useEncryption: false, ".xml");
            if (!ModOptions.Load("ModOptions"))
            {
                ModOptions.CreateNew("ModOptions");
            }
            OptionData.OnLoaded();
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            ModOptions = null;
        }

        public void Save()
        {
            if (ModOptions == null)
            {
                Melon<Core>.Logger.Warning("ModOptions is null!");
                return;
            }

            ModOptions.Save();
            this.OnOptionsChanged?.Invoke();
        }
        public void OnModOptionChanged(OptionData data, int value)
        {
            var method = data.Melon.GetType().GetMethod("OnModOptionChanged");
            if (method == null)
                return;
            var parameters = method.GetParameters();
            if (parameters.Length != 2 || parameters[0].ParameterType != typeof(int) || parameters[1].ParameterType != typeof(int))
                return;
            method.Invoke(data.Melon, [data.Index, value]);
        }

        [HarmonyPatch(typeof(OptionsBinding), "Awake")]
        public static class InitPatch
        {
            static void Postfix(OptionsBinding __instance)
            {
                __instance.gameObject.AddComponent<ModOptionsBinding>();
            }
        }
    }
}
