# csharp_vk_app_launch_params

## How to use:

```csharp
static void Main()
{
    Dictionary<string, string> dict = UrlString
        .Split("?")[1]
        .Split("&")
        .Select(t => t.Split("="))
        .ToDictionary(t => t[0], t => t[1]);
    Console.WriteLine(IsValid(dict, SecretKey));
}
```

## Lib written as code:
```csharp
static Boolean IsValid(IDictionary<string, string> queryParams, string secretKey)
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
```
