using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    class ProductComparer : IComparer<Product>
    {

        public int Compare(Product x, Product y)
        {
            int comp = String.Compare(x.category, y.category);


            if (comp == 0)
            {
                int comp2 = String.Compare(x.name, y.name);
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
    class ProductReadonlyComparer : IComparer<ProductReadonly>
    {

        public int Compare(ProductReadonly x, ProductReadonly y)
        {
            int comp = String.Compare(x.category, y.category);


            if (comp == 0)
            {
                int comp2 = String.Compare(x.name, y.name);
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

            /*public int Compare(Product x, Product y)
            {
                int comp = String.Compare(x.name, y.name);


                if (comp == 0)
                    return 0;
                else if (comp < 0)
                    return -1;
                else
                    return 1;
            }*/

    class SLElementComparer : IComparer<ShoppingListElement>
    {
        public int Compare(ShoppingListElement x, ShoppingListElement y)
        {
            int comp = String.Compare(x.GetProduct().category, y.GetProduct().category);


            if (comp == 0)
            {
                int comp2 = String.Compare(x.name, y.name);
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
