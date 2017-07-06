namespace backend.auth.api.Entities
{
    public class GoogleApiTokenInfo
    {
        public int Sub { get; set; }
        public string Aud { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
