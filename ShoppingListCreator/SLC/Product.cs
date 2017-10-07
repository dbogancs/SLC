using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    [Serializable]
    class Product : IComparable<Product>
    {
        public readonly String id;

        public String name { get; set; }
        public int unitPrice { get; set; }
        public String category { get; set; }

        public Product() { id = DateTime.Now.ToString("yyyyMMddHHmmssffff"); }
        public Product(String id) { this.id = id; }
        public Product(String name, int unitPrice, String category)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.category = category;
            this.id = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }

        public Product GetClone()
        {
            Product p = new Product(id);
            p.name = name;
            p.unitPrice = unitPrice;
            p.category = category;
            return p;
        }

        public int CompareTo(Product p)
        {
            int comp = String.Compare(this.category, p.category);

            if (comp == 0)
            {
                int comp2 = String.Compare(this.name, p.name);
                if (comp2 == 0)
                {
                    return 0;
                }
                else if (comp2 < 0)
                    return -1;
                else
                    return 1;
            }
            else if (comp < 0)
                return -1;
            else
                return 1;
        }
    }
}
