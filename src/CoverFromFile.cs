using System;
using System.IO;
using System.Reflection;
using Assets.Scripts;
using Assets.Scripts.UI.Sharing;
using Jundroo.Juicy.Widgets;
using SFB;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SP2CoverFromFile
{
    /// <summary>
    /// 在“上传作品 / Share”对话框的截图列表里加一个 “From File” 按钮：
    /// 直接从电脑里选一张图作为封面，不用先把它当蓝图导入、再在游戏里截图。
    ///
    /// 游戏的上传流程是：把截图列表里的每个 Texture2D 用 EncodeToPNG 编码，
    /// 作为 multipart 字段提交（第一张就是封面）。所以只要往列表里塞一个我们
    /// 自己加载的 Texture2D 即可，上传逻辑完全不用改。
    /// </summary>
    public static class CoverFromFile
    {
        private const int MaxScreenshots = 3;

        public static void Attach(UploadDialogScript dialog)
        {
            if (dialog == null || !Plugin.AllowCoverFromFile.Value)
            {
                return;
            }

            Widget root = dialog.Widget;
            if (root == null || root.FindWidget(CoverButtonElement.ButtonId) != null)
            {
                return;
            }

            Widget parent = root.FindWidget("screenshots-parent");
            Widget addButton = root.FindWidget("add-screenshot-button");
            if (parent == null || addButton == null)
            {
                Plugin.Log.LogWarning("上传对话框里找不到截图列表，跳过“From File”按钮。");
                return;
            }

            Widget button = root.Context.CreateWidget(CoverButtonElement.Create(), parent, root.Stylesheet);
            if (button == null)
            {
                Plugin.Log.LogWarning("创建“From File”按钮失败。");
                return;
            }

            // 放到 “Add Screenshot” 上面。
            button.SetIndex(addButton.Index);
            button.gameObject.AddComponent<CoverFromFileButton>().Initialize(dialog);

            Plugin.Log.LogInfo("已在上传对话框里加入“From File”封面按钮。");
        }

        /// <summary>
        /// 把指定文件作为截图（第一张，即封面）加进上传列表。
        /// </summary>
        public static void AddCoverFromFile(UploadDialogScript dialog, string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!texture.LoadImage(bytes))
            {
                UnityEngine.Object.Destroy(texture);
                ShowMessage("这个文件无法作为图片读取：" + Path.GetFileName(path));
                return;
            }

            texture.name = Path.GetFileName(path);
            texture.wrapMode = TextureWrapMode.Clamp;

            Widget root = dialog.Widget;
            Widget parent = root.FindWidget("screenshots-parent");
            Widget addButton = root.FindWidget("add-screenshot-button");
            if (parent == null || addButton == null)
            {
                UnityEngine.Object.Destroy(texture);
                return;
            }

            if (CountScreenshots(dialog) >= MaxScreenshots)
            {
                UnityEngine.Object.Destroy(texture);
                ShowMessage("最多只能有 " + MaxScreenshots + " 张截图。请先删掉一张，再添加新的封面。");
                return;
            }

            Widget itemWidget = root.Context.CreateWidgetFromTemplate("screenshot", parent, null, null);
            if (itemWidget == null)
            {
                UnityEngine.Object.Destroy(texture);
                ShowMessage("创建截图项失败。");
                return;
            }

            ScreenshotListItemScript item = itemWidget.GetComponent<ScreenshotListItemScript>();
            if (item == null)
            {
                UnityEngine.Object.Destroy(texture);
                return;
            }

            item.Texture = texture;
            itemWidget.Height = texture.height / (float)texture.width * addButton.Rect.sizeDelta.x;
            itemWidget.gameObject.AddComponent<OwnedTexture>().Initialize(texture);

            // 第一张是封面。
            itemWidget.SetIndex(0);

            Invoke(dialog, "RefreshUserScreenshotsList");
            Invoke(dialog, "UpdateAddScreenshotButton");

            Plugin.Log.LogInfo("已把 " + Path.GetFileName(path) + "（" + texture.width + "x" +
                                texture.height + "）设为上传封面。");
        }

        internal static void UpdateButtonVisibility(UploadDialogScript dialog)
        {
            if (dialog == null)
            {
                return;
            }

            Widget button = dialog.Widget != null
                ? dialog.Widget.FindWidget(CoverButtonElement.ButtonId)
                : null;
            if (button == null)
            {
                return;
            }

            button.Visible = CountScreenshots(dialog) < MaxScreenshots;
        }

        internal static int CountScreenshots(UploadDialogScript dialog)
        {
            Widget root = dialog != null ? dialog.Widget : null;
            Widget parent = root != null ? root.FindWidget("screenshots-parent") : null;
            if (parent == null)
            {
                return 0;
            }

            return parent.GetComponentsInChildren<ScreenshotListItemScript>(true).Length;
        }

        private static void ShowMessage(string message)
        {
            Plugin.Log.LogWarning(message);
            try
            {
                Assets.Scripts.Game.Instance.UserInterface.CreateMessageDialog(
                    message, "Cover Image", "Okay", null);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("显示提示框失败：" + e);
            }
        }

        private static void Invoke(UploadDialogScript dialog, string methodName)
        {
            MethodInfo method = typeof(UploadDialogScript).GetMethod(
                methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
                method.Invoke(dialog, null);
            }
        }
    }

    /// <summary>
    /// 点 “From File” 时打开系统文件选择框。
    /// </summary>
    public class CoverFromFileButton : MonoBehaviour, IPointerClickHandler
    {
        private UploadDialogScript _dialog;

        internal void Initialize(UploadDialogScript dialog)
        {
            _dialog = dialog;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_dialog == null)
            {
                return;
            }

            try
            {
                string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                ExtensionFilter[] filters =
                {
                    new ExtensionFilter("Image", new[] { "png", "jpg", "jpeg" })
                };

                StandaloneFileBrowser.OpenFilePanelAsync(
                    "Choose cover image", directory, filters, false, OnFileSelected);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("打开文件选择框失败：" + e);
            }
        }

        private void OnFileSelected(string[] paths)
        {
            if (paths == null || paths.Length == 0 || string.IsNullOrEmpty(paths[0]))
            {
                return;
            }

            string path = paths[0];
            if (!File.Exists(path))
            {
                Plugin.Log.LogWarning("选择的文件不存在：" + path);
                return;
            }

            try
            {
                CoverFromFile.AddCoverFromFile(_dialog, path);
            }
            catch (Exception e)
            {
                Plugin.Log.LogError("加载封面图片失败：" + e);
            }
        }
    }

    /// <summary>
    /// 对话框关掉时释放我们加载的纹理，避免泄漏。
    /// </summary>
    public class OwnedTexture : MonoBehaviour
    {
        private Texture2D _texture;

        internal void Initialize(Texture2D texture)
        {
            _texture = texture;
        }

        private void OnDestroy()
        {
            if (_texture != null)
            {
                Destroy(_texture);
                _texture = null;
            }
        }
    }
}
