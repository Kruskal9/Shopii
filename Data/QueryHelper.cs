using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class QueryHelper
    {
        const int PAGE_SIZE = 250;
        public static IEnumerable<T> Standard<T>(
            IEnumerable<T> entities, 
            int pageNumber, 
            int pageSize = 10)
        {
            if(pageSize > PAGE_SIZE) pageSize = 10;
            return entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
