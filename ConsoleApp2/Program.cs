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

        private const String SecretKey1 = "aFZjGLBipm93ruTXZ7Fr";
        private const String UrlString1 = "https://thedeax-lab.github.io/vk_pikap_master_vkui_edition/?vk_access_token_settings=friends%2Cstories&vk_app_id=7185084&vk_are_notifications_enabled=0&vk_is_app_user=0&vk_is_favorite=0&vk_language=ru&vk_platform=desktop_web&vk_ref=other&vk_user_id=258201990&sign=XJitJyxlaBD1QAWbqawaVg2yvTsQ9pz6Y19e8idFhkE";
        private static readonly Encoding StringEncoder = Encoding.UTF8;

        static Boolean IsValid(IDictionary<String, String> QueryParams, String SecretKey)
        {
            var checkString = QueryParams.Select(v => v)
                .Where(v => v.Key.StartsWith("vk_"))
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((k1, k2) => $"{k1}&{k2}");
            checkString = checkString.Replace(",", "%2C");
            using (var hmac = new HMACSHA256(StringEncoder.GetBytes(SecretKey)))
            {
                var sign_byte = hmac.ComputeHash(StringEncoder.GetBytes(checkString));
                String sign = Convert.ToBase64String(sign_byte);
                sign = sign.Substring(0, sign.Length - 1);
                Console.WriteLine("{0} {1}", sign, QueryParams["sign"]);
                return sign.Equals(QueryParams["sign"]);
            }
        }

        static void Main()
        {
            var Dict = new Dictionary<String, String>();
            var DecodedParams = HttpUtility.ParseQueryString(UrlString1.Split("?")[1]);
            foreach (var key in  DecodedParams.AllKeys)
            {
                Console.WriteLine(key);
                Dict.Add(key, DecodedParams[key]);
            }
            Console.WriteLine(IsValid(Dict, SecretKey1));
        }
    }
}