using MelonLoader;

[assembly: MelonInfo(typeof(SephiriaModList.Core), "Sephiria ModList", "1.0.0", "Mira", null)]
[assembly: MelonGame("TEAMHORAY", "Sephiria")]

namespace SephiriaModList
{
    public class Core : MelonMod
    {
        /*
         * 
         * 設定追加の例
         * 
         
        public List<List<string>> ModOptions => [["Option 1", "Option 2"], ["Example 1", "Example 2", "Example 3"]];

        public List<string> ModOptionsDescription => ["Example Mod Setting"];

        public void OnModOptionChanged(int index, int value)
        {
            Melon<Core>.Logger.Msg("Changed: " + ModOptions[index][value]);
        }
        public void OnModOptionLoaded(int index, int value)
        {
            Melon<Core>.Logger.Msg("Loaded: " + ModOptions[index][value]);
        }

         */

        public override void OnInitializeMelon()
        {
            OptionData.Init();
            LoggerInstance.Msg("Initialized.");
        }
    }
}