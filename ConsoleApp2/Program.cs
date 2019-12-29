using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ConsoleApp2
{
    class Program
    {

        private const String SecretKey = "aFZjGLBipm93ruTXZ7Fr";
        private const String UrlString = "https://thedeax-lab.github.io/vk_pikap_master_vkui_edition/?vk_access_token_settings=friends%2Cstories&vk_app_id=7185084&vk_are_notifications_enabled=0&vk_is_app_user=0&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_user_id=258201990&sign=XJitJyxlaBD1QAWbqawaVg2yvTsQ9pz6Y19e8idFhkE";
        private static readonly Encoding StringEncoder = Encoding.Default;

        private static Boolean IsValid(IDictionary<string, string> queryParams, string secretKey)
        {
            var checkString = queryParams
                .Where(v => v.Key.StartsWith("vk_"))
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((k1, k2) => $"{k1}&{k2}");
            using (var hmac = new HMACSHA256(StringEncoder.GetBytes(secretKey)))
            {
                var signByte = hmac.ComputeHash(StringEncoder.GetBytes(checkString));
                var sign = Convert.ToBase64String(signByte)
                    .Replace('+','-')
                    .Replace("=", string.Empty)
                    .Replace('/', '_');
                return sign.Equals(queryParams["sign"]);
            }
        }

        static void Main()
        {
            Dictionary<string, string> dict = UrlString
                .Split("?")[1]
                .Split("&")
                .Select(t => t.Split("="))
                .ToDictionary(t => t[0], t => t[1]);
            Console.WriteLine(IsValid(dict, SecretKey));
        }
    }
}