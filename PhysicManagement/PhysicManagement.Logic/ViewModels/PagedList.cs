using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.ViewModels
{
    public interface IPagedList<T>
    {
        int TotalRecords { get; set; }
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        IEnumerable<T> Records { get; set; }
    }
    public class PagedList<T> : IPagedList<T>
    {
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public IEnumerable<T> Records { get; set; } = null;
        public PagedList()
        {
            Records = new List<T>();
        }
    }
}
