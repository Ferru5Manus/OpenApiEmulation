using System.Security.Cryptography;
using System.Text;

namespace OpenApiEmulation.Utils.AntiCsrf;

public class AntiCsrfGenerator
{
    public async Task<string> GenerateToken(string userId, string key)
    {
        AntiCsrfConfig config = new AntiCsrfConfig();
        if (config.dataSize <= 0) {
            throw new ArgumentOutOfRangeException("config.dataSize");
        }
        if (config.expiryInSeconds <= 0) {
            throw new ArgumentOutOfRangeException("config.expiryInSeconds");
        }

        var data = new byte[config.dataSize];
        using (var rnd = RandomNumberGenerator.Create()) {
            rnd.GetBytes(data);
        }

        var expires = DateTime.UtcNow.AddSeconds(config.expiryInSeconds).Ticks;
        var raw = BitConverter.ToString(data).Replace("-", "") + config.split +
                  userId + config.split + expires;

        string signature;

        using (var hmac = HMACGenerator.Create(config.hmac_alg)) {
            hmac.Key = Encoding.UTF8.GetBytes(key);
            var braw = Encoding.UTF8.GetBytes(raw);

            signature = BitConverter.ToString(hmac.ComputeHash(braw)).Replace("-", "");
        }
        if (signature.Length < 1) {
            throw new Exception("Could not generate signature.");
        }

        var retVal = $"{raw}{config.split}{signature}";
        if (config.useBase64) {
            var retValb = Encoding.UTF8.GetBytes(retVal);
            return Convert.ToBase64String(retValb);
        }
        return retVal;
    }
}