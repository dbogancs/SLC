using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    [Serializable]
    class ShoppingList
    {
        public string name { get; set; }
        public List<ShoppingListElement> list { get; set; }

        public ShoppingList()
        {
            list = new List<ShoppingListElement>();
        }

        public ShoppingList(String name, List<ShoppingListElement> list)
        {
            this.name = name;
            SetList(list);
        }

        public void SetList(List<ShoppingListElement> list)
        {
            this.list = new List<ShoppingListElement>();
            foreach (var element in list)
            {
                this.list.Add(new ShoppingListElement(element.GetProduct(),element.count));
            }
        }
    }
}
