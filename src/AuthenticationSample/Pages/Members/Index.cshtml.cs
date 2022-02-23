using Cofoundry.Domain;
using Cofoundry.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AuthenticationSample.Pages.Members
{
    [AuthorizeUserArea(MemberUserArea.Code)]
    public class IndexModel : PageModel
    {
        private readonly IAdvancedContentRepository _contentRepository;

        public IndexModel(
            IAdvancedContentRepository contentRepository
            )
        {
            _contentRepository = contentRepository;
        }

        public UserMicroSummary Member { get; set; }

        public async Task OnGetAsync()
        {
            Member = await _contentRepository
                .Users()
                .Current()
                .Get()
                .AsMicroSummary()
                .ExecuteAsync();
        }
    }
}