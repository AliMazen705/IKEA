using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Password is Required")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password Doesnot Match")]
        public string ConfirmPassword { get; set; }
    }
}
