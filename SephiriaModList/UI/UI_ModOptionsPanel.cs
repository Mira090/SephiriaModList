using MelonLoader;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SephiriaModList.UI
{
    public class UI_ModOptionsPanel : UIBase
    {
        public override void OnClosed()
        {
            base.OnClosed();
            ModOptionsBinding.Instance.Save();
        }

        private void Init()
        {
            //Melon<Core>.Logger.Msg("Init 1");
            var optionsPanel = ParentRoot.transform.Find("OptionPanel").GetComponent<UI_OptionsPanel>();
            var originalTabArea = optionsPanel.transform.GetChild(0).GetChild(0);
            //Melon<Core>.Logger.Msg("Init 2");

            var baseObject = new GameObject("Base");
            baseObject.transform.SetParent(transform, false);
            var rect = baseObject.AddComponent<RectTransform>();
            rect.anchorMax = Vector2.one * 0.5f;
            rect.anchorMin = Vector2.one * 0.5f;
            rect.pivot = Vector2.one * 0.5f;
            rect.anchoredPosition = Vector2.zero;
            rect.offsetMax = new Vector2(244.5f, 140);
            rect.offsetMin = new Vector2(-244.5f, -140);
            baseObject.AddCanvasRenderer();
            var image = baseObject.AddComponent<Image>();
            //Melon<Core>.Logger.Msg("Init 3");
            image.sprite = optionsPanel.transform.GetChild(0).GetComponent<Image>().sprite;
            //Melon<Core>.Logger.Msg("Init 4");
            var animator = baseObject.AddComponent<Animator2D_UI>();
            animator.image = image;
            animator.ChangeSet(optionsPanel.transform.GetChild(0).GetComponent<Animator2D_UI>().currentSet);
            animator.frameMoveType = EAnimator2DFrameMoveType.UNSCALED;

            var area = new GameObject("TabArea");
            area.transform.SetParent(baseObject.transform, false);
            var rect2 = area.AddComponent<RectTransform>();
            rect2.anchorMax = Vector2.one * 0.5f;
            rect2.anchorMin = Vector2.one * 0.5f;
            rect2.pivot = Vector2.one * 0.5f;
            rect2.anchoredPosition = new Vector2(0, 110);
            rect2.offsetMax = new Vector2(216.5f, 110);
            rect2.offsetMin = new Vector2(-216.5f, -108);
            //Melon<Core>.Logger.Msg("Init 5");

            var tab = new GameObject("Tab-Option");
            tab.transform.SetParent(area.transform, false);
            var rect3 = tab.AddComponent<RectTransform>();
            rect3.anchorMax = new Vector2(0.5f, 1);
            rect3.anchorMin = new Vector2(0.5f, 1);
            rect3.pivot = new Vector2(0.5f, 1);
            rect3.anchoredPosition = new Vector2(0, 0);
            rect3.offsetMax = new Vector2(210, 0);
            rect3.offsetMin = new Vector2(-210, -218);
            var optionTab = tab.AddComponent<UI_TabContent>();
            optionTab.parent = this;
            optionTab.selectionOnOpened = null;
            tab.AddCanvasRenderer();
            var optionTabImage = tab.AddComponent<Image>();
            optionTabImage.sprite = originalTabArea.GetChild(0).GetComponent<Image>().sprite;


            var scroll = Instantiate(originalTabArea.GetChild(0).GetChild(0), rect3);
            //Melon<Core>.Logger.Msg("Init 6");

            InitOptions(scroll.gameObject);
        }

        private void InitOptions(GameObject scroll)
        {
            var content = scroll.transform.GetChild(0).GetChild(0).gameObject;
            for(int q = 4;q < content.transform.childCount; q++)
            {
                Destroy(content.transform.GetChild(q).gameObject);
            }
            Destroy(content.transform.GetChild(1).gameObject);
            Destroy(content.transform.GetChild(2).gameObject);

            var melonObjectOriginal = content.transform.GetChild(0).gameObject;
            melonObjectOriginal.SetActive(false);
            var nameOriginal = melonObjectOriginal.transform.GetChild(0).gameObject;
            if (nameOriginal.TryGetComponent<UI_LocalizationStringText>(out var localized))
            {
                Destroy(localized);
            }
            var optionObjectOriginal = content.transform.GetChild(3).gameObject;
            optionObjectOriginal.SetActive(false);
            if(optionObjectOriginal.TryGetComponent<UI_CastModeTypeBox>(out var box))
            {
                Destroy(box);
            }
            var optionBoxOriginal = optionObjectOriginal.AddComponent<UI_OptionBox>();
            if(optionObjectOriginal.TryGetComponent<UI_HorizontalSelectionBox>(out var selectionBox))
            {
                optionBoxOriginal.box = selectionBox;
            }
            optionBoxOriginal.valueText = optionObjectOriginal.transform.GetChild(1).GetComponent<UI_LocalizationStringText>();
            optionBoxOriginal.text = optionBoxOriginal.valueText.text;



            foreach(var pair in OptionData.Data)
            {
                var melonObject = Instantiate(melonObjectOriginal, content.transform);
                var name = melonObject.transform.GetChild(0).gameObject;
                if (name.TryGetComponent<TextMeshProUGUI>(out var text))
                {
                    text.text = pair.Key.Info.Name;
                }
                melonObject.SetActive(true);

                foreach (var option in pair.Value)
                {
                    var optionObject = Instantiate(optionObjectOriginal, content.transform);
                    if(optionObject.TryGetComponent<UI_OptionBox>(out var optionBox))
                    {
                        optionBox.Init(option);
                    }
                    optionObject.SetActive(true);
                }
            }
        }

        public static UI_ModOptionsPanel Create(UI_PausePanel pause)
        {
            var obj = new GameObject("ModOptionPanel");
            obj.transform.SetParent(pause.transform.parent, false);
            obj.transform.SetAsLastSibling();
            var rect = obj.AddComponent<RectTransform>();
            rect.anchorMax = Vector2.one;
            rect.anchorMin = Vector2.zero;
            rect.pivot = Vector2.one * 0.5f;
            rect.anchoredPosition = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;

            var group = obj.AddComponent<CanvasGroup>();
            group.alpha = 1;
            group.blocksRaycasts = false;
            group.ignoreParentGroups = false;
            group.interactable = false;

            var renderer = obj.AddCanvasRenderer();

            var image = obj.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.7529f);

            var panel = obj.AddComponent<UI_ModOptionsPanel>();
            panel.SetRoot(pause.ParentRoot);
            panel.hasControl = true;

            obj.SetActive(false);

            panel.Init();
            return panel;
        }
    }
}
