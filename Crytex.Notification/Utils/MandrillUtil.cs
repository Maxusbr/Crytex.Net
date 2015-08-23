namespace Crytex.Notification
{
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using Project.Model.Models.Notifications;

    public static class MandrillUtil
    {
        private const string MERGE_TAG_NAME = "DYNAMIC_CONTENT_";
        private const string MERGE_TAG_PREFIX = "*|";
        private const string MERGE_TAG_SUFFIX = "|*";

        public static string GenerateName(EmailTemplate template)
        {
            return CalculateMD5Hash(template.EmailTemplateType + template.Subject + template.Body);
        }

        public static string FormatBody(string bodyTemplate)
        {
            return bodyTemplate.Replace("{", MERGE_TAG_PREFIX + MERGE_TAG_NAME).Replace("}", MERGE_TAG_SUFFIX);
        }
        public static string GetMergeTag(string j)
        {
            return MERGE_TAG_NAME + j;
        }

        private static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GenerateTextFromTemplate(string str, List<KeyValuePair<string, string>> keyValuePairs)
        {
            var formatedString = str;
            foreach (var keyValuePair in keyValuePairs)
            {
                formatedString = formatedString.Replace("{" + keyValuePair.Key + "}", keyValuePair.Value);
            }
            return formatedString;
        }
    }
}