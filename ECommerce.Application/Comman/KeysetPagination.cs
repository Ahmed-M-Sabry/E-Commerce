using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Comman
{
    public class KeysetPagination<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int? LastIndex { get; set; }
        public bool HasMore { get; set; }
        public string? LastCursor { get; set; }
    }
}
