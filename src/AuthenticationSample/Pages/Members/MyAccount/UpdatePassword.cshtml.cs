using Cofoundry.Domain;
using Cofoundry.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthenticationSample.Pages.Members.MyAccount
{
    [AuthorizeUserArea(MemberUserArea.Code)]
    public class UpdatePasswordModel : PageModel
    {
        private readonly IAdvancedContentRepository _contentRepository;

        public UpdatePasswordModel(
            IAdvancedContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Current password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Confirm new password")]
        [DataType(DataType.Password)]
        //[Compare(nameof(NewPassword), ErrorMessage = "Password does not match")]
        public string ConfirmNewPassword { get; set; }

        public bool IsSuccess { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            ValidatePasswordMatch();

            await _contentRepository
                .WithModelState(this)
                .Users()
                .Current()
                .UpdatePasswordAsync(new UpdateCurrentUserPasswordCommand()
                {
                    OldPassword = OldPassword,
                    NewPassword = NewPassword
                });

            IsSuccess = ModelState.IsValid;
        }

        /// <summary>
        /// Workaround for issue with CompareAttribute in RazorPages.
        /// Fixed in .NET 5, so this can be removed once we upgrade
        /// See https://github.com/dotnet/aspnetcore/issues/4895
        /// </summary>
        private void ValidatePasswordMatch()
        {
            if (NewPassword != ConfirmNewPassword)
            {
                ModelState.AddModelError(nameof(ConfirmNewPassword), "Password does not match");
            }
        }
    }
}