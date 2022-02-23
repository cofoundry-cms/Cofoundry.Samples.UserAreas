using Cofoundry.Domain;
using Cofoundry.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthenticationSample.Pages.Members
{
    public class PasswordChangeRequiredModel : PageModel
    {
        private readonly IAdvancedContentRepository _contentRepository;

        public PasswordChangeRequiredModel(
            IAdvancedContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        [BindProperty]
        [DataType(DataType.EmailAddress)]
        [Required]
        [EmailAddress(ErrorMessage = "Please use a valid email address")]
        public string Email { get; set; }

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

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var isSignedIn = await _contentRepository
                .Users()
                .Current()
                .IsSignedIn()
                .ExecuteAsync();

            if (isSignedIn)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task OnPostAsync()
        {
            ValidatePasswordMatch();

            await _contentRepository
                .WithModelState(this)
                .Users()
                .UpdatePasswordByCredentialsAsync(new UpdateUserPasswordByCredentialsCommand()
                {
                    UserAreaCode = MemberUserArea.Code,
                    Username = Email,
                    OldPassword = OldPassword,
                    NewPassword = NewPassword
                });

            if (ModelState.IsValid)
            {
                ReturnUrl = RedirectUrlHelper.GetAndValidateReturnUrl(this);
                IsSuccess = true;
            }
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