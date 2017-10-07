using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;

namespace ShoppingListCreator.SLC
{
    static class ProductHandler
    {
        private static readonly String ALL_PRODUCT_FILE = "all_products";

        private static List<Product> allProduct = new List<Product>();
        private static List<ProductReadonly> allReadonlyProduct = new List<ProductReadonly>();
        public static ReadOnlyCollection<ProductReadonly> allReadonlyProductReadonlyList = new ReadOnlyCollection<ProductReadonly>(allReadonlyProduct);

        private static bool AskAQuestion(String title, String question)
        {
            DialogResult dialogResult = MessageBox.Show(question, title, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                return true;
            }
            else if (dialogResult == DialogResult.No)
            {
                return false;
            }
            else
            {
                throw new SLCException("ERROR: No answer choosed!");
            }
        }

        private static void SayAWarning(String title, String question)
        {
            DialogResult dialogResult = MessageBox.Show(question, title, MessageBoxButtons.OK);
        }

        public static ProductReadonly FindProduct(String name, int unitPrice)
        {
            return allReadonlyProduct.FirstOrDefault(x => (x.name.Equals(name) && x.unitPrice.Equals(unitPrice)));
        }

        private static Product FindProductById(String id)
        {
            try
            {
                //return allProduct.FirstOrDefault(x => (x.id.Equals(id)));

                
                for (int i = 0; i < allProduct.Count; i++)
                {
                    if (allProduct[i].id.Equals(id))
                        return allProduct[i];
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ProductReadonly FindReadonlyProductById(String id)
        {
            try
            {
                return allReadonlyProduct.FirstOrDefault(x => (x.id.Equals(id)));
            }
            catch (Exception)
            {

                return null;
            }
        }

        private static bool ExistingProduct(String name, int unitPrice)
        {
            return allReadonlyProduct.Exists(x => (x.name.Equals(name) && x.unitPrice.Equals(unitPrice)));
        }

        private static bool ExistingProductName(String name)
        {
            return allReadonlyProduct.Exists(x => x.name.Equals(name));
        }

        /*public static bool AddProduct(Product product)
        {
            return AddProduct(product.name, product.unitPrice, product.category);
        }*/

        public static bool AddProduct(String name, int unitPrice, String category)
        {
            Product p = new Product(name, unitPrice, category);

            bool acceptable = true;

            if (ExistingProduct(name, unitPrice))
            {
                SayAWarning("Létező termék!", "Ilyen egységáru és nevű termék már létezik. Használd a meglévőt!");
                acceptable = false;
            }
            else if (ExistingProductName(name))
            {
                acceptable = AskAQuestion("Egyező nevek!", "Ilyen névvel már létezik termék. Biztosan újként akarod menteni?");
            }

            if (acceptable)
            {
                allProduct.Add(p);
                ProductReadonly pro = new ProductReadonly(p);
                allReadonlyProduct.Add(pro);
                allProduct.Sort(new ProductComparer());
                allReadonlyProduct.Sort(new ProductReadonlyComparer());
            }

            return acceptable;
        }

        public static ReadOnlyCollection<ProductReadonly> GetAllProduct()
        {
            return allReadonlyProductReadonlyList;
        }

        public static List<Product> GetAllOriginalProduct()
        {
            return allProduct;
        }

        public static void SetAllOriginalProduct(List<Product> pl)
        {
            allProduct = pl;
            allProduct.Sort(new ProductComparer());
            allReadonlyProduct = new List<ProductReadonly>();
            foreach (var p in allProduct)
            {
                allReadonlyProduct.Add(new ProductReadonly(p));
            }
            allReadonlyProductReadonlyList = new ReadOnlyCollection<ProductReadonly>(allReadonlyProduct);
            allReadonlyProduct.Sort(new ProductReadonlyComparer());
        }

        public static bool UpdateProduct(String id, String name, int unitPrice, String category)
        {
            Product p = FindProductById(id);

            bool acceptable = true;

            if (ExistingProduct(name, unitPrice))
            {
                SayAWarning("Termékadatok ütköznek!", "Ilyen egységáru és nevű termék már létezik. Adj meg más adatokat, vagy használd a már meglévő terméket!");
                acceptable = false;
            }
            else if (!p.name.Equals(name) && ExistingProductName(name))
            {
                acceptable = AskAQuestion("Egyező nevek!", "Ilyen névvel már létezik termék más árral. Biztosan így akarod menteni?");
            }

            if (acceptable)
            {
                p.name = name;
                p.unitPrice = unitPrice;
                p.category = category;
                allProduct.Sort(new ProductComparer());
                allReadonlyProduct.Sort(new ProductReadonlyComparer());
                return true;
            }
            return false;
        }

        /*public static void SaveProducts()
        {
            //serialize
            using (Stream stream = File.Open(ALL_PRODUCT_FILE, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, allProduct);
            }
        }

        public static void LoadProducts()
        {
            //deserialize
            using (Stream stream = File.Open(ALL_PRODUCT_FILE, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                List<Product> ap = (List<Product>)bformatter.Deserialize(stream);
                if (ap == null) allProduct = new List<Product>();
                else allProduct = ap;



                allReadonlyProduct = new List<ProductReadonly>();
                foreach (var p in allProduct)
                {
                    allReadonlyProduct.Add(new ProductReadonly(p));
                }
                allReadonlyProductReadonlyList = new ReadOnlyCollection<ProductReadonly>(allReadonlyProduct);
            }
        }*/

        public static bool DeleteProduct(String id)
        {
            if (AskAQuestion("Törlés", "Ha törlöd ezt a terméket, törlődni fog az összes olyan mentett bevásárlólistában is, amihez hozzá lett adva! Biztosan törlöd ezt a terméket?"))
            {
                ShoppingListHandler.RemoveProduct(id);
                allProduct.Remove(FindProductById(id));
                allReadonlyProduct.Remove(FindReadonlyProductById(id));
                return true;
            }
            else return false;
        }

        public static ShoppingListElement GetShoppingListElement(String name, int unitPrice)
        {
            ProductReadonly p = FindProduct(name, unitPrice);
            ShoppingListElement sle = new ShoppingListElement(p);
            return sle;
        }

    }
}
