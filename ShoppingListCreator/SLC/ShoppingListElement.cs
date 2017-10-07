using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCreator.SLC
{
    [Serializable]
    class ShoppingListElement
    {
        private ProductReadonly p = null;
        
        public String name
        {
            get
            {
                if (p == null) return "TÖRÖLT TERMÉK";
                else return p.name;
            }
        }
        public int unitPrice
        {
            get
            {
                if (p == null) return 0;
                else return p.unitPrice;
            }
        }

        public int count { get; set; }

        public int sumPrice
        {
            get { return (p==null) ? 0 : p.unitPrice * count; }
        }

        //public ShoppingListElement() { count = 0; }

        public ShoppingListElement(ProductReadonly p)
        {
            count = 0;
            this.p = p;
        }

        public ShoppingListElement(ProductReadonly p, int count)
        {
            this.count = count;
            this.p = p;
        }

        public void RemoveProduct()
        {
            p = null;
        }

        public String GetProductId()
        {
            return (p==null)?null:p.id;
        }

        public ProductReadonly GetProduct()
        {
            return (p == null) ? null : p;
        }
    }
}
