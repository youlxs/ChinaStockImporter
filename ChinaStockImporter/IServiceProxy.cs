using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yootech.ChinaStockImporter
{
    public interface IServiceProxy<T>
    {
        void DoServiceWork(IEnumerable<T> matchsFromScratch);
    }
}
