using BepInEx;
using UnityEngine;
using Comfort.Common;
using EFT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TarkovPerformanceBooster
{
    [BepInPlugin("com.yourname.tarkov.aiboost", "Tarkov AI Distance Optimizer", "1.1.1")]
    public class Plugin : BaseUnityPlugin
    {
        private static bool showMessage = true;
        private static float messageTime = 5f;
        private Coroutine aiControlCoroutine;

        private const float BotDisableDistanceMeters = 200f;
        private const float BotCheckIntervalSeconds = 2f;

        private void Awake()
        {
            Logger.LogInfo("AI Distance Optimizer loaded.");
            aiControlCoroutine = StartCoroutine(ManageBots());
        }

        private void Update()
        {
            if (showMessage)
            {
                messageTime -= Time.deltaTime;
                if (messageTime <= 0)
                    showMessage = false;
            }
        }

        private void OnGUI()
        {
            if (showMessage)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.LowerLeft,
                    fontSize = 18,
                    normal = { textColor = Color.cyan },
                    padding = new RectOffset(10, 0, 0, 10)
                };

                GUI.Label(new Rect(10, Screen.height - 40, 600, 30), "AI DISTANCE OPTIMIZER ACTIVE", style);
            }
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
                        Vector3 playerPosition = world.MainPlayer.Transform.position;
                        List<Player> botsSnapshot = world.AllAlivePlayersList.ToList();

                        foreach (var bot in botsSnapshot)
                        {
                            if (bot != null && bot != world.MainPlayer)
                            {
                                float distance = Vector3.Distance(bot.Transform.position, playerPosition);
                                bool shouldBeActive = distance < BotDisableDistanceMeters;
                                bot.gameObject.SetActive(shouldBeActive);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Logger.LogError("AI Distance Optimizer error: " + ex.Message);
                }

                yield return new WaitForSeconds(BotCheckIntervalSeconds);
            }
        }
    }
}