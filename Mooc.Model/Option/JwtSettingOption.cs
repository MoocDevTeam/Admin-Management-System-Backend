 namespace Mooc.Model.Option;

public class JwtSettingOption
{
    public const string Section = "JwtSetting";

    /// <summary>
    /// secret key
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    /// encryption algorithm
    /// </summary>
    public string ENAlgorithm { get; set; }

    /// <summary>
    /// Issuer
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Audience
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Expiration time unit: seconds
    /// </summary>
    public int ExpireSeconds { get; set; }
}
