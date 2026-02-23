using MelonLoader;
using System;
using System.Collections.Generic;
using System.Text;

namespace SephiriaModList
{
    public class OptionData
    {
        public static Dictionary<MelonMod, List<OptionData>> Data = [];
        public static Dictionary<MelonMod, bool> UseLocalizations = [];

        public static readonly string OptionName = "ModOptions";
        public static readonly string LocalizationName = "UseLocalizedSring";
        public static readonly string DescriptionName = "ModOptionsDescription";
        public static readonly string DefaultName = "ModOptionsDefault";

        public static void Init()
        {
            foreach(var melon in MelonMod.RegisteredMelons)
            {
                var pro = melon.GetType().GetProperty(OptionName);
                if (pro == null || pro.PropertyType != typeof(List<List<string>>))
                    continue;
                var proValue = pro.GetValue(melon);
                if (proValue is not List<List<string>> options)
                    continue;
                if (!Data.ContainsKey(melon))
                    Data[melon] = [];
                int index = 0;
                foreach(var option in options)
                {
                    Data[melon].Add(new OptionData(melon, option, null, index++));
                }

                var desc = melon.GetType().GetProperty(DescriptionName);
                if (desc != null && desc.PropertyType == typeof(List<string>))
                {
                    var descValue = desc.GetValue(melon);
                    if (descValue is List<string> descs)
                    {
                        int count = 0;
                        foreach(var d in descs)
                        {
                            if (count >= Data[melon].Count)
                                break;
                            Data[melon][count].Description = d;
                            count++;
                        }
                    }
                }
                var defa = melon.GetType().GetProperty(DefaultName);
                if (defa != null && defa.PropertyType == typeof(List<int>))
                {
                    var defaValue = defa.GetValue(melon);
                    if (defaValue is List<int> defaults)
                    {
                        int count = 0;
                        foreach (var d in defaults)
                        {
                            if (count >= Data[melon].Count)
                                break;
                            Data[melon][count].Default = d;
                            count++;
                        }
                    }
                }

                UseLocalizations[melon] = false;
                var local = melon.GetType().GetProperty(LocalizationName);
                if(local == null || local.PropertyType != typeof(bool))
                    continue;
                var localValue = pro.GetValue(melon);
                if(localValue is not bool useLocal)
                    continue;
                UseLocalizations[melon] = useLocal;
            }
        }
        public static void OnLoaded()
        {
            foreach (var data in Data)
            {
                var method = data.Key.GetType().GetMethod("OnModOptionLoaded");
                if (method == null)
                    continue;
                var parameters = method.GetParameters();
                if (parameters.Length != 2 || parameters[0].ParameterType != typeof(int) || parameters[1].ParameterType != typeof(int))
                    continue;

                foreach (var value in data.Value)
                {
                    if (value.CurrentValue.HasValue)
                        method.Invoke(data.Key, [value.Index, value.CurrentValue.Value]);
                }
            }
        }

        public OptionData(MelonMod melon, List<string> values, string description, int index)
        {
            Melon = melon;
            Values = values;
            Description = description;
            Index = index;
            Default = 0;
        }

        public MelonMod Melon;
        public List<string> Values;
        public string Description;
        public int Default;
        public int Index;
        public bool HasDescription => !string.IsNullOrEmpty(Description);
        public string FullDescription => HasDescription ? Description : new LocalizedString("UI_PausePanel_OptionsButton").ToString() + " " + (Index + 1);
        public bool UseLocalization => UseLocalizations.ContainsKey(Melon) && UseLocalizations[Melon];
        public string OptionKey => Melon.Info.Name + Index;
        public int? CurrentValue
        {
            get
            {
                if (ModOptionsBinding.Instance == null)
                    return null;
                if (!ModOptionsBinding.Instance.ModOptions.ContainsKey(OptionKey))
                    return null;
                return ModOptionsBinding.Instance.ModOptions.GetInt(OptionKey, 0);
            }
        }
    }
}
