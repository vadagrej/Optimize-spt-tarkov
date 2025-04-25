using BepInEx;
using UnityEngine;
using System.Diagnostics;
using HarmonyLib;
using System.Reflection;
using Comfort.Common;
using EFT;
using System.Collections;

namespace TarkovFullPerformanceBooster
{
    [BepInPlugin("com.yourname.tarkovfullboost", "Tarkov Full Performance Booster Final Safe", "1.3.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static bool showMessage = true;
        private static float messageTime = 5f;
        private const float aiDisableDistance = 200f;
        private const float botUpdateInterval = 2.0f;

        private void Awake()
        {
            Logger.LogInfo("Full Booster Final Safe loaded.");
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            SetHighPriority();
            ApplyGraphicsTweaks();
            DisableOffscreenStuff();
            StartCoroutine(ManageBots());
        }

        private void Update()
        {
            if (showMessage)
            {
                messageTime -= Time.deltaTime;
                if (messageTime <= 0) showMessage = false;
            }
        }

        private void OnGUI()
        {
            if (showMessage)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 24,
                    normal = { textColor = Color.red }
                };
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 25, 400, 50), "FULL BOOST SAFE ACTIVE", style);
            }
        }

        private void SetHighPriority()
        {
            try { Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High; }
            catch (System.Exception e) { Logger.LogError("Priority error: " + e.Message); }
        }

        private void ApplyGraphicsTweaks()
        {
            QualitySettings.shadowCascades = 0;
            QualitySettings.antiAliasing = 0;
            QualitySettings.vSyncCount = 0;
            QualitySettings.pixelLightCount = 0;
            QualitySettings.lodBias = 0.5f;
            Application.targetFrameRate = 999;
        }

        private void DisableOffscreenStuff()
        {
            QualitySettings.maximumLODLevel = 1;
            DynamicGI.updateThreshold = 2f;
        }

        private IEnumerator ManageBots()
        {
            while (true)
            {
                try
                {
                    var world = Singleton<GameWorld>.Instance;
                    if (world != null && world.AllAlivePlayersList != null)
                    {
                        Vector3 playerPos = world.MainPlayer.Transform.position;
                        foreach (var bot in world.AllAlivePlayersList)
                        {
                            if (bot != null && bot != world.MainPlayer)
                            {
                                float dist = Vector3.Distance(bot.Transform.position, playerPos);
                                bool shouldBeActive = dist < aiDisableDistance;
                                bot.gameObject.SetActive(shouldBeActive);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.LogError("AI Control Error: " + ex.Message);
                }
                yield return new WaitForSeconds(botUpdateInterval);
            }
        }
    }
}