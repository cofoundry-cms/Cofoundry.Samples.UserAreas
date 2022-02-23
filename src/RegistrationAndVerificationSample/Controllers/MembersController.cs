using Cofoundry.Domain;
using Cofoundry.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RegistrationAndVerificationSample.Controllers
{
    [Route("members")]
    [AuthorizeUserArea(MemberUserArea.Code)]
    public class MembersController : Controller
    {
        private readonly IAdvancedContentRepository _contentRepository;

        public MembersController(
            IAdvancedContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var member = await _contentRepository
                .Users()
                .Current()
                .Get()
                .AsMicroSummary()
                .ExecuteAsync();

            return View(member);
        }
    }
}