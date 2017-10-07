using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    class SLCException : Exception
    {
        public SLCException(String message) : base(message)
        {
            
        }
    }
}
