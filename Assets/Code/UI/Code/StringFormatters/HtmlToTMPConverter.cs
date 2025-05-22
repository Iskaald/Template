using System;

namespace Core.UI
{
    public static class HtmlToTMPConverter
    {
        public static string ConvertToTMP(string html)
        {
            var result = html;
            result = result.Replace("<div>", "").Replace("</div>", "\n");
            result = result.Replace("<ol>", "").Replace("</ol>", "");

            var hasBold = result.Contains("<b>");
            if (hasBold)
            {
                result = result.Replace("<b>", "").Replace("</b>", ""); 
            }

            var counter = 0;
            result = System.Text.RegularExpressions.Regex.Replace(result, "<li>(.*?)</li>", (match) =>
            {
                counter++;
                var itemText = match.Groups[1].Value;
                return hasBold ? $"<b>{counter}. {itemText}</b>\n" : $"{counter}. {itemText}\n";
            });

            result = System.Text.RegularExpressions.Regex.Replace(result, "<br\\s*/?>", "\n");
    
            return result.Trim();
        }
        
        public static (string description, string disclaimer) SplitDescriptionAndDisclaimer(string convertedText)
        {
            // 1. Try to split using "--" first
            var dashSplit = convertedText.Split(new[] { "--" }, StringSplitOptions.None);
            if (dashSplit.Length == 2)
            {
                return (dashSplit[0].Trim(), dashSplit[1].Trim());
            }

            // 3. Fallback: try to split after last numbered step (e.g., "4. Do X")
            var fallbackIndex = convertedText.LastIndexOf("\n");
            if (fallbackIndex > 0)
            {
                var desc = convertedText.Substring(0, fallbackIndex).Trim();
                var disc = convertedText.Substring(fallbackIndex).Trim();
                return (desc, disc);
            }

            // 4. No disclaimer found — return all as description
            return (convertedText.Trim(), string.Empty);
        }
    }
}