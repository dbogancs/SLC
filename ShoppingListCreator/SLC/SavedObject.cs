using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    [Serializable]
    class SavedObject
    {
        public List<Product> allProduct { get; }
        public List<ShoppingList> allLists { get; }

        public SavedObject()
        {
            allProduct = new List<Product>();
            allLists = new List<ShoppingList>();
        }

        public SavedObject(List<Product> lp, List<ShoppingList> ls)
        {
            allProduct = lp;
            allLists = ls;
        }
    }
}
