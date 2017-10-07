using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ShoppingListCreator.SLC
{
    static class ShoppingListHandler
    {
        private static readonly String ALL_LIST_FILE = "all_list_file";

        public static List<ShoppingList> allLists = new List<ShoppingList>();

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

        public static void SaveLists()
        {
            //serialize
            using (Stream stream = File.Open(ALL_LIST_FILE, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, allLists);
            }
        }

        public static void LoadLists()
        {
            //deserialize
            using (Stream stream = File.Open(ALL_LIST_FILE, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                List<ShoppingList> al = (List<ShoppingList>)bformatter.Deserialize(stream);
                if (al == null) allLists = new List<ShoppingList>();
                else allLists = al;
            }
        }

        public static ShoppingList FindListById(String id)
        {
            throw new NotImplementedException();
        }

        public static ShoppingList FindList(String name)
        {
            return allLists.FirstOrDefault(x => (x.name.Equals(name)));
        }

        public static bool CreateList(ShoppingList sl)
        {
            ShoppingList p = FindList(sl.name);

            bool acceptable = true;

            if (sl.name.Equals(""))
            {
                SayAWarning("Üres mező!", "Add meg az új lista nevét!");
                acceptable = false;
            }
            if (p != null)
            {
                SayAWarning("Foglalt listanév!", "Ilyen néven már van mentett lista. Adj meg másik nevet!");
                acceptable = false;
            }

            if (acceptable)
            {
                allLists.Add(new ShoppingList(sl.name,sl.list));
                return true;
            }
            return false;
        }
        
        private static void SayAWarning(String title, String question)
        {
            DialogResult dialogResult = MessageBox.Show(question, title, MessageBoxButtons.OK);
        }

        public static void DeleteList(String name)
        {
            if (AskAQuestion("Törlés", "A bevásárlólista törlése végleges, nem visszanvonható. Biztosan törölni akarod ezt a listát?"))
            {
                ShoppingList sl = allLists.FirstOrDefault(x => x.name.Equals(name));
                allLists.Remove(sl);
            }
        }

        public static List<String> GetAllListNames()
        {
            List<String> names = new List<String>();

            for (int i = 0; i < allLists.Count; i++)
            {
                names.Add(allLists[i].name);
            }
            return names;
        }

        public static ShoppingList GetList(String name)
        {
            ShoppingList ls = allLists.FirstOrDefault(x => x.name.Equals(name));
            return (ls!=null) ? new ShoppingList(name, ls.list) : null;
        }

        public static List<ShoppingList> GetAllOriginalList()
        {
            return allLists;
        }

        public static void SetAllOriginalList(List<ShoppingList> l)
        {
            allLists = l;
        }

        public static void UpdateList(String name, List<ShoppingListElement> list)
        {
            ShoppingList sl = allLists.First(x => x.name.Equals(name));
            sl.SetList(list);
        }

        public static void RemoveProduct(String id)
        {
            for (int i = 0; i < allLists.Count; i++)
            {
                var currentList = allLists[i].list;
                for (int j = 0; j < currentList.Count; j++)
                {
                    if (currentList[j].GetProductId()!=null && currentList[j].GetProductId().Equals(id)) {
                        currentList[j].RemoveProduct();
                    }
                }
            }
        }
    }
}
