using System.Xml.Linq;

namespace SP2CoverFromFile
{
    /// <summary>
    /// 上传对话框里那个 “From File” 按钮的 XML 定义。
    /// 单独拆出来是为了能在没有任何游戏程序集的情况下做单元测试。
    /// </summary>
    public static class CoverButtonElement
    {
        public const string ButtonId = "add-screenshot-file-button";
        public const string ButtonText = "From File";

        public static XElement Create()
        {
            return XElement.Parse(
                "<Button id=\"" + ButtonId + "\" class=\"btn btn-default\" " +
                "tooltip=\"Use an image file from your computer as the cover\">" +
                "<Text text=\"" + ButtonText + "\" /></Button>");
        }
    }
}
