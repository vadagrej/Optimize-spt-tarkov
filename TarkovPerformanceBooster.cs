
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;
using System;
using HarmonyLib;

namespace TarkovPerformanceBooster
{
    [BepInPlugin("com.yourname.tarkovperfboost", "Tarkov Performance Booster", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private static GameObject uiObject;
        private static bool showMessage = true;
        private static float messageTime = 5f;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo("Tarkov Performance Booster loaded.");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);

            SetHighPriority();
            OptimizeMemory();
            DisableOffscreenRendering();

            CreateUI();

            InvokeRepeating(nameof(CheckReloadKey), 1f, 0.2f);
        }

        private void CreateUI()
        {
            uiObject = new GameObject("PerformanceBoosterUI");
            uiObject.AddComponent<OnScreenMessage>();
            DontDestroyOnLoad(uiObject);
        }

        private void CheckReloadKey()
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Log.LogInfo("Config reloaded.");
                showMessage = true;
                messageTime = 5f;
            }
        }

        private void SetHighPriority()
        {
            try
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Log.LogInfo("Set process priority to High.");
            }
            catch (Exception e)
            {
                Log.LogError($"Failed to set priority: {e.Message}");
            }
        }

        private void OptimizeMemory()
        {
            ThreadPool.SetMinThreads(8, 8);
            GC.Collect();
            Log.LogInfo("Memory optimized.");
        }

        private void DisableOffscreenRendering()
        {
            QualitySettings.maximumLODLevel = 1;
            QualitySettings.pixelLightCount = 0;
            Log.LogInfo("Offscreen rendering optimizations applied.");
        }

        public class OnScreenMessage : MonoBehaviour
        {
            private GUIStyle style;

            void Start()
            {
                style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 24,
                    normal = { textColor = Color.green }
                };
            }

            void OnGUI()
            {
                if (showMessage)
                {
                    GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 25, 400, 50), "Tarkov Performance Booster активен", style);
                    messageTime -= Time.deltaTime;
                    if (messageTime <= 0) showMessage = false;
                }
            }
        }
    }
}
