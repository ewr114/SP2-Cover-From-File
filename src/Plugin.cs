using System;
using System.Reflection;
using Assets.Scripts.UI.Sharing;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SP2CoverFromFile
{
    /// <summary>
    /// SimplePlanes 2 —— 上传作品时从电脑选择封面图。
    ///
    /// 原版流程：先把图片当蓝图导入设计器，再在上传界面“拍摄”一张截图当封面。
    /// 这个插件直接在 Share 对话框的截图列表里加一个 “From File” 按钮，
    /// 选一张 png/jpg 就会作为第一张截图（封面）加入，之后照常上传。
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "sp2.cover-from-file";
        public const string PluginName = "SP2 Cover From File";
        public const string PluginVersion = "1.0.0";

        internal static Plugin Instance;
        internal static ManualLogSource Log;

        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<bool> AllowCoverFromFile;

        private Harmony _harmony;
        private bool _patched;

        private void Awake()
        {
            Instance = this;
            Log = Logger;

            Enabled = Config.Bind(
                "General", "Enabled", true,
                "是否启用本插件。 / Enable this plugin.");

            AllowCoverFromFile = Config.Bind(
                "General", "AllowCoverFromFile", true,
                "是否在“上传作品”对话框里显示“From File”按钮，用于从电脑上直接选封面图。 / Show a \"From File\" button in the upload dialog to pick a cover image from disk.");

            _harmony = new Harmony(PluginGuid);
            TryPatch();

            if (!_patched)
            {
                // 游戏程序集可能尚未加载，等它加载完成后再挂接。
                AppDomain.CurrentDomain.AssemblyLoad += OnAssemblyLoad;
            }

            Log.LogInfo(PluginName + " " + PluginVersion + " 已加载。");
        }

        private void Start()
        {
            TryPatch();
        }

        private void OnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            TryPatch();
            if (_patched)
            {
                AppDomain.CurrentDomain.AssemblyLoad -= OnAssemblyLoad;
            }
        }

        private void TryPatch()
        {
            if (_patched)
            {
                return;
            }

            try
            {
                MethodInfo init = AccessTools.Method(
                    typeof(UploadDialogScript), "Initialize", new[] { typeof(IScreenshotDialogHandler) });
                MethodInfo update = AccessTools.Method(typeof(UploadDialogScript), "UpdateAddScreenshotButton");

                if (init == null || update == null)
                {
                    Log.LogError("未找到上传对话框的 Initialize/UpdateAddScreenshotButton，无法添加封面按钮。");
                    return;
                }

                _harmony.Patch(init, postfix: new HarmonyMethod(typeof(Plugin).GetMethod(
                    "OnUploadDialogInitialized", BindingFlags.NonPublic | BindingFlags.Static)));
                _harmony.Patch(update, postfix: new HarmonyMethod(typeof(Plugin).GetMethod(
                    "OnUpdateAddScreenshotButton", BindingFlags.NonPublic | BindingFlags.Static)));

                _patched = true;
                Log.LogInfo("已挂接上传对话框，可用“From File”从电脑选择封面图。");
            }
            catch (Exception e)
            {
                Log.LogError("挂接上传对话框失败：" + e);
            }
        }

        private static void OnUploadDialogInitialized(UploadDialogScript __instance)
        {
            try
            {
                if (!Enabled.Value || !(__instance is UploadCraftDialogScript))
                {
                    return;
                }

                CoverFromFile.Attach(__instance);
            }
            catch (Exception e)
            {
                Log.LogError("初始化上传对话框封面按钮时出错：" + e);
            }
        }

        private static void OnUpdateAddScreenshotButton(UploadDialogScript __instance)
        {
            try
            {
                if (Enabled.Value && __instance is UploadCraftDialogScript)
                {
                    CoverFromFile.UpdateButtonVisibility(__instance);
                }
            }
            catch (Exception e)
            {
                Log.LogError("更新封面按钮状态时出错：" + e);
            }
        }
    }
}
