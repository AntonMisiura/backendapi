using System;

namespace backend.auth.api.Entities
{
    public class Token
    {
        public string Key { get; set; }
        public DateTime Expiry { get; set; }
        public int UserId { get; set; }
        public string TokenType { get; set; }
    }
}
