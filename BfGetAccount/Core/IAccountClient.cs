using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IAccountClient
    {
        string GetAcccount(int i);

        IEnumerable<string> GetAllAcccount(int i = 0);
    }

}
