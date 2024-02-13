namespace OpenApiEmulation.Utils.AntiCsrf;

public class AntiCsrfConfig
{
    
    public int dataSize = 16;
    public int expiryInSeconds = 3600;
    public string hmac_alg = "SHA256";
    public char split = '\n';
    public bool useBase64 = true;
}