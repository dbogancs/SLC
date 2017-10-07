using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    [Serializable]
    class ProductReadonly : IComparable<ProductReadonly>
    {
        private Product product;

        public string id { get { return product.id; } }
        public string name { get { return product.name; } }
        public int unitPrice { get { return product.unitPrice; } }
        public string category { get { return product.category; } }

        public ProductReadonly(Product p) { product = p; }
        
        public int CompareTo(ProductReadonly p)
        {
            int comp = string.Compare(this.category, p.category);

            if (comp == 0)
            {
                int comp2 = string.Compare(this.name, p.name);
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
