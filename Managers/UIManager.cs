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
            button.onClick = new Button.ButtonClickedEvent();
            TextMeshProUGUI textAsset = button.GetComponentInChildren<TextMeshProUGUI>();
            textAsset.transform.SetLocalX(0.2f);
            textAsset.text = "Archipelago";

            UtilityScript.Update<Component>(textAsset.gameObject.GetComponents<Component>(), delegate (Component c)
            {
                bool flag2 = c is FontSetter || c is LocalizeActionString;
                if (flag2)
                {
                    c.Destroy();
                }
            });

            /*UIManager.buttonPrefab = UnityEngine.Object.Instantiate<RectTransform>(Addressables.LoadAssetAsync<GameObject>("Event-Item").WaitForCompletion().GetComponent<ItemEventRoutine>().skipButton.transform.parent.parent as RectTransform, uiItems);
            UIManager.buttonPrefab.gameObject.GetOrAdd<LayoutLink>().enabled = false;
            UIManager.buttonPrefab.gameObject.GetOrAdd<LinkEnable>().enabled = false;
            TextFitter fitter = UIManager.buttonPrefab.GetComponentInChildren<TextMeshProUGUI>().gameObject.GetOrAdd<TextFitter>();
            fitter.transforms = new RectTransform[]
            {
                UIManager.buttonPrefab.transform as RectTransform
            };
            TextMeshProUGUI textAsset2 = UIManager.buttonPrefab.Find("Animator/Button").GetComponentInChildren<TextMeshProUGUI>();
            textAsset2.gameObject.GetOrAdd<LocalizeStringEvent>().enabled = false;
            textAsset2.text = "Button";
            textAsset2.maskable = true;
            yield return fitter.FitRoutine();
            Image image = UIManager.buttonPrefab.Find("Animator/Button").GetComponent<Image>();
            image.material = image.defaultMaterial;
            image.maskable = true;
            UIManager.buttonPrefab.GetComponentInChildren<ButtonAnimator>().interactable = true;
            UIManager.buttonPrefab.gameObject.SetActive(true);
            UIManager.OnSceneChanged(SceneManager.GetActive());*/
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
                /*
                UIManager.textAsset = apbutton.GetComponentInChildren<TextMeshProUGUI>();
                UIManager.textAsset.text = "Profile: " + SaveSystem.Profile;*/
                Button button = apbutton.transform.Find("Animator/Button").GetComponent<Button>();
                apbutton.GetComponent<UINavigationItem>().clickHandler = button.gameObject;
                button.onClick.AddListener(delegate ()
                {
                    CoroutineManager.Start(UIManager.OnClick());
                });
            }
        }

        // FROM THIS POINT ONWARD I HAVEN'T MODIFIED THE CODE TO FIT OUR NEEDS YET

        public static IEnumerator OnClick()
        {
            yield return SceneManager.Load("Mods", 2, null);
            UIManager.editing = false;
            UIManager.modsSceneManager = UnityEngine.Object.FindObjectOfType<ModsSceneManager>();
            bool flag = !UIManager.modPrefab;
            if (flag)
            {
                UIManager.modPrefab = UnityEngine.Object.Instantiate<GameObject>(UIManager.modsSceneManager.ModPrefab, uiItems);
                GameObject buttonLayout = new GameObject("Editor Buttons", new Type[]
                {
                    typeof(VerticalLayoutGroup),
                    typeof(ContentSizeFitter)
                });
                (buttonLayout.transform as RectTransform).SetSize(UtilityScript.FindObject(UIManager.modPrefab, "Buttons").transform as RectTransform);
                buttonLayout.transform.SetParent(UIManager.modPrefab.transform);
                buttonLayout.transform.SetLocalPositionAndRotation(new Vector2(3.5f, 0.8f), Quaternion.identity);
                buttonLayout.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                VerticalLayoutGroup layout = buttonLayout.GetComponent<VerticalLayoutGroup>();
                layout.childAlignment = TextAnchor.UpperCenter;
                layout.childControlHeight = false;
                layout.childControlWidth = false;
                foreach (string text in new string[]
                {
                    "Duplicate",
                    "Delete"
                })
                {
                    RectTransform button = UnityEngine.Object.Instantiate<RectTransform>(UIManager.buttonPrefab, buttonLayout.transform);
                    button.name = text;
                    Transform buttonTransform = button.Find("Animator/Button");
                    buttonTransform.GetComponentInChildren<TextMeshProUGUI>().SetText(text, true);
                    buttonTransform.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                    button = null;
                    buttonTransform = null;
                    //text = null;
                }
                string[] array = null;
                buttonLayout.SetActive(false);
                yield return null;
                buttonLayout = null;
                layout = null;
            }
            UIManager.scrollView = UIManager.modsSceneManager.Content.transform.parent.parent;
            UIManager.content = UIManager.scrollView.Find("Viewport/Content");
            UIManager.content.DestroyAllChildren();
            UIManager.profileHolders.Clear();
            foreach (string dir in ES3.GetDirectories(SaveSystem.profileFolder))
            {
                UnityEngine.Debug.LogWarning(dir);
                UIManager.Run(dir);
                //dir = null;
            }
            string[] array2 = null;
            UIManager.CreateMainEditButton();
            UIManager.CreateNewProfileButton();
            yield break;
        }

        public static void Run(string profileName = "Default")
        {
            GameObject gameObject = UIManager.modPrefab.InstantiateKeepName<GameObject>();
            gameObject.transform.SetParent(UIManager.content);
            bool flag = profileName.StartsWith("Default");
            if (flag)
            {
                gameObject.transform.SetAsFirstSibling();
            }
            gameObject.transform.SetLocalZ(0f);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localRotation = Quaternion.identity;
            ModHolder holder = gameObject.GetComponentInChildren<ModHolder>();
            //holder.Mod = new UIManager.ProfileDisplay(profileName, holder);
            holder.Mod.Load();
            holder.UpdateInfo();
            UIManager.profileHolders.Add(holder);
            Button selector = holder.bellRinger.transform.Find("Button (Base)/Animator/Button").GetComponent<Button>();
            selector.onClick = new Button.ButtonClickedEvent();
            /*selector.onClick.AddListener(delegate ()
            {
                UIManager.ProfileDisplay.Select(holder);
            });*/
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000027F0 File Offset: 0x000009F0
        private void Update()
        {
            bool flag = !UIManager.promptToggleEditor;
            if (!flag)
            {
                UIManager.promptToggleEditor = false;
                this.Toggle();
            }
        }

        public void Toggle()
        {
            UIManager.editing = !UIManager.editing;
            foreach (ModHolder holder in UIManager.profileHolders)
            {
                UtilityScript.FindObject(holder.gameObject, "Editor Buttons").SetActive(UIManager.editing);
                UtilityScript.FindObject(holder.gameObject, "Buttons").SetActive(!UIManager.editing);
            }
            UnityEngine.Debug.LogWarning("[Profile Manager] Editing? " + UIManager.editing.ToString());
        }

        public static void CreateMainEditButton()
        {
            Transform parent = UIManager.content.root.Find("SafeArea/Menu");
            GameObject editButton = UnityEngine.Object.Instantiate<GameObject>(UIManager.editButtonPrefab, parent.transform);
            editButton.transform.localRotation = Quaternion.identity;
            editButton.transform.localPosition = parent.Find("Back Button").localPosition.WithY(1.5f);
            Button button = editButton.transform.Find("Animator/Button").GetComponent<Button>();
            button.onClick.AddListener(delegate ()
            {
                UIManager.promptToggleEditor = true;
            });
            TextMeshProUGUI textAsset = button.GetComponentInChildren<TextMeshProUGUI>();
            textAsset.transform.SetLocalX(0.2f);
            textAsset.text = "Toggle Edit";
        }

        public static void CreateNewProfileButton()
        {
            Transform parent = UIManager.content.root.Find("SafeArea/Menu");
            GameObject editButton = UnityEngine.Object.Instantiate<GameObject>(UIManager.editButtonPrefab, parent.transform);
            editButton.transform.localRotation = Quaternion.identity;
            editButton.transform.localPosition = parent.Find("Back Button").localPosition.WithY(2.5f);
            Button button = editButton.transform.Find("Animator/Button").GetComponent<Button>();
            UnityEvent onClick = button.onClick;
            UnityAction call;
            if ((call = UIManager.<> O.< 0 > __CreateNewProfile) == null)
            {
                call = (UIManager.<> O.< 0 > __CreateNewProfile = new UnityAction(UIManager.CreateNewProfile));
            }
            onClick.AddListener(call);
            TextMeshProUGUI textAsset = button.GetComponentInChildren<TextMeshProUGUI>();
            textAsset.transform.SetLocalX(0.2f);
            textAsset.text = "New Profile";
        }

        private static void CreateNewProfile()
        {
            string date = DateTime.Today.ToShortDateString().Replace('/', '.');
            string newFolderName = "Default - " + date;
            bool flag = ES3.DirectoryExists(newFolderName);
            if (flag)
            {
                int i = 1;
                while (ES3.DirectoryExists(string.Format("{0} #{1}", newFolderName, i)))
                {
                    i++;
                }
                newFolderName = string.Format("{0} #{1}", newFolderName, i);
            }
            SaveSystem.SetProfile(newFolderName, true);
            CoroutineManager.Start(SceneManager.Unload("Mods"));
        }

        public static bool editing = false;
        public static bool promptToggleEditor = false;
        public static TextMeshProUGUI textAsset = null;
        public static GameObject editButtonPrefab = null;
        public static Transform scrollView = null;
        public static Transform content = null;
        public static ModsSceneManager modsSceneManager = null;
        public static GameObject modPrefab = null;
        public static RectTransform buttonPrefab = null;
        public static List<ModHolder> profileHolders = new List<ModHolder>();

        /*[CompilerGenerated]
        private static class <>O
		{
			public static UnityAction<0> __OnProfileChanged;
            public static UnityAction<Scene> <1>__OnSceneChanged;
		}*/
    }
}
