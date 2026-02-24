using Deadpan.Enums.Engine.Components.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

namespace Wildfrost_Archipelago.Managers
{
    // Code adapted from the profile manager mod code by hopeful_phan
    public class UIManager : MonoBehaviour
    {
        public static Transform uiItems;
        public static GameObject apbutton = null;

        internal void Start()
        {
            base.StartCoroutine(this.Initialize());
        }

        public IEnumerator Initialize()
        {
            yield return new WaitUntil(() => SceneManager.IsLoaded("MainMenu"));
            bool flag = GameObject.Find("Canvas/Safe Area/Menu/ButtonLayout/APButton");
            if (flag)
            {
                yield break;
            }
            GameObject modsbutton = GameObject.Find("Canvas/Safe Area/Menu/ButtonLayout/ModsButton");
            apbutton = UnityEngine.Object.Instantiate<GameObject>(modsbutton, modsbutton.transform.position, Quaternion.identity, modsbutton.transform.parent);
            apbutton.name = "APButton";
            Button button = apbutton.transform.Find("Animator/Button").GetComponent<Button>();
            apbutton.GetComponent<UINavigationItem>().clickHandler = button.gameObject;
            apbutton.transform.SetSiblingIndex(3);
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(delegate ()
            {
                uiItems.gameObject.SetActive(true);
            });
            TextMeshProUGUI textAsset = button.GetComponentInChildren<TextMeshProUGUI>();
            textAsset.text = "Archipelago";

            UtilityScript.Update<Component>(textAsset.gameObject.GetComponents<Component>(), delegate (Component c) // no idea what this does, but im keeping it here i suppose
            {
                bool flag2 = c is FontSetter || c is LocalizeActionString;
                if (flag2)
                {
                    c.Destroy();
                }
            });

            StartCoroutine(InitMenu());

            yield break;
        }

        public static void OnSceneChanged(Scene scene)
        {
            bool flag = scene.name != "MainMenu" || !apbutton;
            if (!flag)
            {
                GameObject modsbutton = GameObject.Find("Canvas/Safe Area/Menu/ButtonLayout/ModsButton");
                GameObject apbutton = UnityEngine.Object.Instantiate<GameObject>(modsbutton, modsbutton.transform.position, Quaternion.identity, modsbutton.transform.parent);
                apbutton.transform.SetLocalY(4.5f);
                apbutton.name = "APButton";
                Button button = apbutton.transform.Find("Animator/Button").GetComponent<Button>();
                apbutton.GetComponent<UINavigationItem>().clickHandler = button.gameObject;
                button.onClick.AddListener(delegate ()
                {
                    uiItems.gameObject.SetActive(true);
                });
            }
        }

        public static IEnumerator InitMenu()
        {
            // for whatever reason loading the mods scene manually and then unloading it breaks the mods button and the back-out button on the mods scene and I have no idea why so im doing this really stupid method instead.
            GameObject.Find("Canvas/Safe Area/Menu/ButtonLayout/ModsButton/Animator/Button").GetComponent<Button>().onClick.Invoke();
            yield return new WaitUntil(() => SceneManager.IsLoaded("Mods"));
            GameObject canvas = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Mods").GetRootGameObjects().Single(obj => obj.GetComponent<WorldSpaceCanvasFitScreen>() != null);
            canvas.transform.FindRecursive("Back Button").Find("Animator/Button").GetComponent<Button>().onClick.Invoke();
            GameObject APCanvas = canvas.InstantiateKeepName();
            APCanvas.transform.SetParent(uiItems.transform);
            content = APCanvas.transform.FindRecursive("Content");
            content.DestroyAllChildren();
            Button backButton = APCanvas.transform.FindRecursive("Back Button").Find("Animator/Button").GetComponent<Button>();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(delegate () {
                uiItems.gameObject.SetActive(false);
            });

            // CODE THAT ADDS THE ACTUAL CONTENT OF THE MENU WOULD GO HERE
        }

        public static Transform scrollView = null;
        public static Transform content = null;
        public static GameObject modPrefab = null;
        public static RectTransform buttonPrefab = null;
    }
}
