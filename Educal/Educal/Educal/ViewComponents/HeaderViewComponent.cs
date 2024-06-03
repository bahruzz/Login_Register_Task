using Educal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Educal.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser user = new();
            if(User.Identity.IsAuthenticated)
            {
                user=await _userManager.FindByNameAsync(User.Identity.Name);
            }

            HeaderVM vm = new()
            {
                UserFullName = user.FullName,
            };
            return await Task.FromResult(View(vm));
        }


       
    }
    public class HeaderVM
    {
        public string UserFullName { get; set; }
    }

}
