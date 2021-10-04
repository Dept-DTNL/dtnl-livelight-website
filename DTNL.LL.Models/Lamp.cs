using System;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Models
{
    public class Lamp
    {
        [Required]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
