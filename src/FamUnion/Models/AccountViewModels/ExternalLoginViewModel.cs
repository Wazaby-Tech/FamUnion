using System.ComponentModel.DataAnnotations;

namespace FamUnion.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
