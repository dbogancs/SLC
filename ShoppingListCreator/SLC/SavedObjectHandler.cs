using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ShoppingListCreator.SLC
{
    static class SavedObjectHandler
    {
        public static readonly String ALL_OBJECT = "all_object";

        public static void SaveObject()
        {
            SavedObject so = new SavedObject(ProductHandler.GetAllOriginalProduct(), ShoppingListHandler.GetAllOriginalList());

            //serialize
            using (Stream stream = File.Open(ALL_OBJECT, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, so);
            }
        }

        public static void LoadObject()
        {
            try
            {

                //deserialize
                using (Stream stream = File.Open(ALL_OBJECT, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    SavedObject so = (SavedObject)bformatter.Deserialize(stream);
                    if (so == null) so = new SavedObject();

                    ProductHandler.SetAllOriginalProduct(so.allProduct);
                    ShoppingListHandler.SetAllOriginalList(so.allLists);
                }
            }
            catch (FileNotFoundException)
            {
                // Nothing <3
            }
        }
    }
}
