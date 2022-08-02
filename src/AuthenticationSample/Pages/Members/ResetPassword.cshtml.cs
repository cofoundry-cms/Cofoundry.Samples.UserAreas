using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationSample.Pages.Members
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IAdvancedContentRepository _contentRepository;

        public ResetPasswordModel(
            IAdvancedContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

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

        public AuthorizedTaskTokenValidationResult TokenValidationResult { get; set; }

        public bool IsSuccess { get; set; }

        /// <param name="t">
        /// An account reecovery request is authorized using a token
        /// which is included in the URL as a query parameter
        /// named "t". You can access this parameter a number of ways
        /// but here we simply bind it as a parameter to the handler.
        /// </param>
        public async Task<IActionResult> OnGetAsync(string t)
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

            TokenValidationResult = await _contentRepository
                .Users()
                .AccountRecovery()
                .Validate(new ValidateUserAccountRecoveryByEmailQuery()
                {
                    UserAreaCode = MemberUserArea.Code,
                    Token = t
                })
                .ExecuteAsync();

            return Page();
        }

        public async Task OnPostAsync(string t)
        {
            ValidatePasswordMatch();

            await _contentRepository
                .WithModelState(this)
                .Users()
                .AccountRecovery()
                .CompleteAsync(new CompleteUserAccountRecoveryViaEmailCommand()
                {
                    UserAreaCode = MemberUserArea.Code,
                    Token = t,
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
