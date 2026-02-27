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
using System.ComponentModel;
using HarmonyLib;
using UnityEngine.TextCore.Text;

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


            foreach (UnityEngine.Component c in textAsset.GetComponents<UnityEngine.Component>())
            {
                bool flag2 = c is FontSetter || c is LocalizeActionString || c is LocalizeStringEvent;
                if (flag2)
                {
                    c.Destroy();
                }
            }

            StartCoroutine(InitMenu());

            yield break;
        }

        public static void OnSceneChanged(Scene scene)
        {
            bool flag = scene.name == "MainMenu" && !apbutton;
            if (flag)
            {
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


                foreach (UnityEngine.Component c in textAsset.GetComponents<UnityEngine.Component>())
                {
                    bool flag2 = c is FontSetter || c is LocalizeActionString || c is LocalizeStringEvent;
                    if (flag2)
                    {
                        c.Destroy();
                    }
                }
            }
        }

        public static IEnumerator InitMenu()
        {
            // for whatever reason loading the mods scene manually and then unloading it breaks the mods button and the back-out button on the mods scene and I have no idea why so im doing this really stupid method instead.
            GameObject.Find("Canvas/Safe Area/Menu/ButtonLayout/ModsButton/Animator/Button").GetComponent<Button>().onClick.Invoke();
            yield return new WaitUntil(() => SceneManager.IsLoaded("Mods"));
            GameObject canvas = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Mods").GetRootGameObjects().Single(obj => obj.GetComponent<WorldSpaceCanvasFitScreen>() != null);
            GameObject APCanvas = canvas.InstantiateKeepName();
            APCanvas.transform.SetParent(uiItems.transform);
            content = APCanvas.transform.FindRecursive("Content");
            content.DestroyAllChildren();
            content.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.UpperCenter;
            Button backButton = APCanvas.transform.FindRecursive("Back Button").Find("Animator/Button").GetComponent<Button>();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(delegate () {
                uiItems.gameObject.SetActive(false);
            });

            string sceneToCopyFrom = "UI";
            bool isLoaded = SceneManager.IsLoaded(sceneToCopyFrom);
            bool flag2 = !isLoaded;
            if (flag2 && renameSeq == null)
            {
                UIManager.PatchRenameButton.initialising = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToCopyFrom, LoadSceneMode.Additive);
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += GetRenameButton;
            }
            yield return new WaitUntil(() => renameSeq != null);


            GameObject hostLabel = text.InstantiateKeepName();
            hostLabel.name = "Host Label";
            hostLabel.transform.SetParent(content);
            hostLabel.transform.localPosition = new Vector3(3.7f, 0, 0);
            hostLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText("Host Address");

            GameObject host = renameSeq.InstantiateKeepName();
            host.name = "Host Text Field";
            host.transform.SetParent(content);
            host.GetComponent<RectTransform>().anchoredPosition = new Vector2(5.5f, 1);
            uriAndPort = host.GetComponentInChildren<TMP_InputField>();
            host.SetActive(true);

            GameObject slotLabel = hostLabel.InstantiateKeepName();
            slotLabel.name = "Player Slot Label";
            slotLabel.transform.SetParent(content);
            slotLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText("Player Slot");

            GameObject slot = renameSeq.InstantiateKeepName();
            slot.name = "Player Slot Text Field";
            slot.transform.SetParent(content);
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(5.5f, 1);
            playerSlot = slot.GetComponentInChildren<TMP_InputField>();
            slot.SetActive(true);

            GameObject passwordLabel = hostLabel.InstantiateKeepName();
            passwordLabel.name = "Password Label";
            passwordLabel.transform.SetParent(content);
            passwordLabel.GetComponent<TMPro.TextMeshProUGUI>().SetText("Password");

            GameObject passwordField = renameSeq.InstantiateKeepName();
            passwordField.name = "Password Text Field";
            passwordField.transform.SetParent(content);
            passwordField.GetComponent<RectTransform>().anchoredPosition = new Vector2(5.5f, 1);
            password = passwordField.GetComponentInChildren<TMP_InputField>();
            passwordField.SetActive(true);

            confirmButton.transform.SetAsLastSibling();

            renameSeq.transform.SetLocalX(7);

            /*GameObject spacing = new GameObject();
            spacing.AddComponent<RectTransform>();
            spacing.transform.SetParent(content);
            spacing.transform.localScale = new Vector3(1, 5, 1);
            spacing.transform.localPosition = new Vector3(0, 0, 0);
            spacing.transform.SetAsFirstSibling();*/

            // if we don't wait for a couple of frames (im not sure how many are necessary but 1 is not enough) then the unload will just fail for some reason and we'll be stuck with the mods menu on screen and unable to unload it via the button
            // i think it might be because the timing just so happens to line up that we're trying to unload the mods scene at the same time the game is trying to access save data and stuff gets janky? unsure
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            canvas.transform.FindRecursive("Back Button").Find("Animator/Button").GetComponent<Button>().onClick.Invoke();

            yield break;
        }

        public static void GetRenameButton(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "UI")
                return;

            renameSeq = MonoBehaviourSingleton<Deckpack>.instance.transform.root.FindRecursive("Rename Card").Find("Rename").gameObject.InstantiateKeepName<GameObject>();
            UnityEngine.Object.DontDestroyOnLoad(renameSeq);
            renameSeq.SetActive(false);
            renameSeq.GetComponentInChildren<TMP_InputField>().characterLimit = 0;

            confirmButton = MonoBehaviourSingleton<Deckpack>.instance.transform.root.FindRecursive("Rename Card").Find("Confirm Button").gameObject.InstantiateKeepName();
            confirmButton.name = "Connect Button";
            UnityEngine.Object.DontDestroyOnLoad(confirmButton);
            Button button = confirmButton.transform.Find("Animator/Button").GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate () {
                TryConnect();
            });
            text = button.gameObject.GetComponentInChildren<TextMeshProUGUI>().gameObject;
            confirmButton.transform.SetParent(content);

            foreach (UnityEngine.Component c in text.GetComponents<UnityEngine.Component>())
            {
                bool flag2 = c is FontSetter || c is LocalizeActionString || c is LocalizeStringEvent;
                if (flag2)
                {
                    c.Destroy();
                }
            }

            text.GetComponent<TextMeshProUGUI>().text = "Connect";

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UI");
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= GetRenameButton;
            UIManager.PatchRenameButton.initialising = false;
            UIManager.PatchRenameButton.initialised = true;
        }

        public static void TryConnect()
        {
            ServiceFactory.sessionManager.StartSession(uriAndPort.text, playerSlot.text, password.text);
        }

        [HarmonyPatch(typeof(ModifierDisplayCurrent), "OnEnable")]
        internal class PatchRenameButton
        {
            private static bool Prefix()
            {
                return !UIManager.PatchRenameButton.initialising;
            }
            public static bool initialising;

            public static bool initialised;
        }

        public static TMP_InputField uriAndPort = null;
        public static TMP_InputField playerSlot = null;
        public static TMP_InputField password = null;
        public static GameObject text = null;
        public static GameObject confirmButton = null;
        public static GameObject renameSeq = null;
        public static Transform scrollView = null;
        public static Transform content = null;
        public static GameObject modPrefab = null;
        public static RectTransform buttonPrefab = null;
    }
}
