using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShopDemo.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return (await Task.FromResult(View()));
        }
    }
}
