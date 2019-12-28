# csharp_vk_app_launch_params

## How to use:

```csharp
        static void Main()
        {
            var dict = new Dictionary<string, string>();
            var decodedParams = HttpUtility.ParseQueryString(UrlString.Split("?")[1]);
            foreach (var key in  decodedParams.AllKeys)
            {
                dict.Add(key, decodedParams[key]);
            }
            Console.WriteLine(IsValid(dict, SecretKey));
        }
```

## Lib written as code:
```csharp
        private static Boolean IsValid(IDictionary<string, string> queryParams, string secretKey)
        {
            var checkString = queryParams.Select(v => v)
                .Where(v => v.Key.StartsWith("vk_"))
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={kvp.Value}")
                .Aggregate((k1, k2) => $"{k1}&{k2}")
                .Replace(",", "%2C");
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
```
