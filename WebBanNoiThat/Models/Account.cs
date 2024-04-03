using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace WebBanNoiThat.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public bool Role { get; set; }
        public bool Status { get; set; }
        public string EmailConfirmationToken { get; internal set; }
        public bool EmailConfirmed { get; internal set; }
    }
}
