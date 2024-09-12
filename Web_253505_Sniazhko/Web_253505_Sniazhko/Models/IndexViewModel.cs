using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web_253505_Sniazhko.Models
{
    public class IndexViewModel
    {
        public SelectList ListDemo { get; }

        public IndexViewModel(IEnumerable<ListDemo> listDemo)
        {
            ListDemo = new SelectList(listDemo, "Id", "Name");
        }
    }
}
