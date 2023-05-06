namespace FINALIDS_1220019.Models
{
    public class JWTResult
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiration { get; set; }
    }
}
