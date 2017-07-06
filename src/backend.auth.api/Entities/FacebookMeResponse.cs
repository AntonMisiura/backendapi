namespace backend.auth.api.Entities
{
    public class FacebookMeResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public FacebookAgeRange AgeRange { get; set; }
        public string PublicProfilePhotoUrl { get; set; }
    }
}