using System.Collections.Generic;
using System.Linq;

namespace Fmg.Models.Common
{
    public class Page<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public string? Continuation { get; set; }
    }
}