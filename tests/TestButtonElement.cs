using System;
using System.Xml.Linq;
using SP2CoverFromFile;

internal static class TestButtonElement
{
    private static int _failures;

    private static void Check(bool ok, string message)
    {
        Console.WriteLine((ok ? "PASS  " : "FAIL  ") + message);
        if (!ok)
        {
            _failures++;
        }
    }

    private static void Main()
    {
        XElement button = CoverButtonElement.Create();

        Check(button.Name == "Button", "root element is <Button>");
        Check((string)button.Attribute("id") == "add-screenshot-file-button", "button id is set");
        Check(((string)button.Attribute("class") ?? "").Contains("btn"), "button uses the game's btn style");
        Check(!string.IsNullOrEmpty((string)button.Attribute("tooltip")), "button has a tooltip");

        XElement text = button.Element("Text");
        Check(text != null, "button has a <Text> child");
        Check(text != null && (string)text.Attribute("text") == "From File", "label is \"From File\"");

        // Parsing twice must produce independent elements (no shared state).
        XElement second = CoverButtonElement.Create();
        Check(second != null && !ReferenceEquals(button, second), "each call builds a fresh element");

        Console.WriteLine();
        Console.WriteLine(_failures == 0 ? "ALL TESTS PASSED" : _failures + " TEST(S) FAILED");
        Environment.Exit(_failures == 0 ? 0 : 1);
    }
}
