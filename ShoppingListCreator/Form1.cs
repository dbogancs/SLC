using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShoppingListCreator.SLC;
using System.IO;
using System.Drawing.Printing;
using System.Collections.ObjectModel;

namespace ShoppingListCreator
{
    public partial class ShoppingListCreator : Form
    {
        private ReadOnlyCollection<ProductReadonly> allProductForDataGridView;
        private String selectedProductId;
        private int selectedProductIndex = 0;
        private List<ShoppingListElement> selectedList;

        BindingSource PSource = new BindingSource();
        BindingSource SLESource = new BindingSource();

        public ShoppingListCreator()
        {
            InitializeComponent();
            SavedObjectHandler.LoadObject();
            //ProductHandler.LoadProducts();
            //ShoppingListHandler.LoadLists();

            // without this: index out of range (-1)
            //ShoppingListElement sle = new ShoppingListElement(new Product("fgh", 321, "asd"));
            //selectedList.Add(sle);
            //selectedListDataGridViewRefresh();
            //selectedList.Remove(sle);

            allProductForDataGridView = ProductHandler.GetAllProduct();//.FindAll(x => x.name.Contains(SearchTextBox.Text));
            PSource.DataSource = allProductForDataGridView;
            allProductDataGridView.DataSource = PSource;
            allProductDataGridView.Columns[0].Visible = false;
            allProductDataGridView.Columns[1].HeaderText = "Név";
            allProductDataGridView.Columns[2].HeaderText = "Egységár";
            allProductDataGridView.Columns[3].HeaderText = "Kategória";
            allProductDataGridView.Update();
            allProductDataGridView.Refresh();
            //allProductDataGridView.Sort(allProductDataGridView.Columns[2], ListSortDirection.Ascending);

            selectedList = new List<ShoppingListElement>();
            selectedList.Sort(new SLElementComparer());
            SLESource.DataSource = selectedList;
            selectedListDataGridView.DataSource = SLESource;
            selectedListDataGridView.Columns[0].HeaderText = "Név";
            selectedListDataGridView.Columns[1].HeaderText = "Egységár";
            selectedListDataGridView.Columns[2].HeaderText = "Darabszám";
            selectedListDataGridView.Columns[3].HeaderText = "Összesen";
            allProductDataGridView.Update();
            allProductDataGridView.Refresh();
            for (var i = 0; i < selectedListDataGridView.Columns.Count; i++)
            {
                var column = selectedListDataGridView.Columns[i];
                if (column.Name.Equals("count")) column.ReadOnly = false;
                else column.ReadOnly = true;
            }


            editListTabControl.SelectedIndex = 0;
            
            savedListsComboBoxRefresh();

            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        private void allProductDataGridViewRefresh() { allProductDataGridViewRefresh(null); }
        private void allProductDataGridViewRefresh(Product p)
        {
            if (SearchTextBox.Text.Length > 0) setSearchResult();

            PSource.ResetBindings(false);
            allProductDataGridView.Update();
            allProductDataGridView.Refresh();

            selectedListDataGridViewRefresh(p);
        }

        private void selectedListDataGridViewRefresh() { selectedListDataGridViewRefresh(null); }
        private void selectedListDataGridViewRefresh(Product p)
        {
            SLESource.ResetBindings(false);
            selectedListDataGridView.Update(); // DataSource = null;
            selectedListDataGridView.Refresh();
            sumPriceLabelRefresh();
            countNullValueRefresh();
        }

        private void sumPriceLabelRefresh()
        {
            int sumPrice = 0;

            foreach (var element in selectedList)
            {
                sumPrice += element.sumPrice;
            }

            sumPriceLabel.Text = sumPrice.ToString() + " Ft";
        }

        private void savedListsComboBoxRefresh()
        {
            var listNames = ShoppingListHandler.GetAllListNames();
            oldListComboBox.DataSource = listNames;
        }

        private void countNullValueRefresh()
        {
            for (var i = 0; i < selectedListDataGridView.Rows.Count; i++)
            {
                var val1 = selectedListDataGridView.Rows[i].Cells[2].Value;
                var val2 = "0";
                if (val1 != null && val1.ToString().Equals(val2))
                {
                    selectedListDataGridView.Rows[i].Cells[2].Style.BackColor = Color.Red;
                }
                else
                {
                    selectedListDataGridView.Rows[i].Cells[2].Style.BackColor = Color.White;
                }
            }
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }

        private void label2_Click_3(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void createProductButton_Click(object sender, EventArgs e)
        {
            if (ProductHandler.AddProduct(
                newProductNameTextBox.Text,
                int.Parse((newProductUnitPriceTextBox.Text.Equals("")) ? "0" : newProductUnitPriceTextBox.Text),
                newProductCategoryTextBox.Text))
            {
                SavedObjectHandler.SaveObject();

                allProductDataGridViewRefresh();

                for (int i = 0; i < allProductDataGridView.Rows.Count; i++)
                {
                    if (allProductDataGridView.Rows[i].Cells[1].Value.Equals(newProductNameTextBox.Text)
                        && allProductDataGridView.Rows[i].Cells[2].Value.Equals((newProductUnitPriceTextBox.Text.Equals("")) ? "0" : newProductUnitPriceTextBox.Text))
                    {
                        allProductDataGridView.ClearSelection();
                        allProductDataGridView.Rows[i].Selected = true;
                        break;
                    }
                }

                newProductNameTextBox.Text = "";
                newProductUnitPriceTextBox.Text = "";
                editProductTabControl.SelectedIndex = 0;
            }

        }

        private void allListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void productUnitPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            pressOnlyNumber(sender, e);
        }

        private void pressOnlyNumber(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void allProductDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            allProductDataGridView_Click(sender, e);
        }

        private void updateProductButton_Click(object sender, EventArgs e)
        {
            /*for (int i = 0; i < allProductDataGridView.Rows.Count; i++)
            {
                if (allProductDataGridView.Rows[i].Selected == true) { 
                    selectedProductIndex = i;
                    break;
                }
            }*/

            Product product = new Product(
                editProductNameTextBox.Text,
                int.Parse(editProductUnitPriceTextBox.Text),
                editProductCategoryTextBox.Text
                );
            
            if (ProductHandler.UpdateProduct(selectedProductId, product.name,product.unitPrice,product.category))
            {
                SavedObjectHandler.SaveObject();
                selectedProductNameLabel.Text = product.name + " (" + product.unitPrice + " Ft)";
                allProductDataGridViewRefresh();
            }

            //????????????
            //var openedList = ShoppingListHandler.GetList(selectedListGroupBox.Text);
            //if (openedList != null) selectedList = openedList.list;

            //allProductDataGridView.Rows[0].Selected = false;
            //allProductDataGridView.Rows[selectedProductIndex].Selected = true;
        }

        private void deleteProductButton_Click(object sender, EventArgs e)
        {
            if (ProductHandler.DeleteProduct(selectedProductId))
            {
                SavedObjectHandler.SaveObject();
                selectedProductId = null;
                editProductNameTextBox.Text = "";
                editProductUnitPriceTextBox.Text = "";
                editProductPanel.Enabled = false;
                editProductTabControl.SelectedIndex = 0;
                selectedProductNameLabel.Text = "(nincs kiválasztva)";
                
                //????????????????????
                //var openedList = ShoppingListHandler.GetList(selectedListGroupBox.Text);
                //if (openedList != null) selectedList = openedList.list;

                allProductDataGridViewRefresh();
            }
        }

        private void allProductDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (allProductDataGridView.SelectedCells.Count > 0)
            {
                int selectedRow = allProductDataGridView.SelectedCells[0].RowIndex;

                String name = allProductDataGridView.Rows[selectedRow].Cells[1].Value.ToString();
                int unitPrice = int.Parse(allProductDataGridView.Rows[selectedRow].Cells[2].Value.ToString());

                bool alreadyIn = false;
                for (int i = 0; i < selectedList.Count; i++)
                {
                    if(selectedList[i].name.Equals(name) && selectedList[i].unitPrice == unitPrice)
                    {
                        alreadyIn = true;
                    }
                }

                if (!alreadyIn)
                {
                    ShoppingListElement sle = ProductHandler.GetShoppingListElement(name, unitPrice);
                    selectedList.Add(sle);
                    selectedList.Sort(new SLElementComparer());
                    selectedListDataGridViewRefresh();

                    SaveWarningLabel.Visible = true;
                }

                selectedListDataGridView.Focus();
                selectedListDataGridView.ClearSelection();
                for (int i = 0; i < selectedList.Count; i++)
                {
                    if (selectedList[i].name.Equals(name) && selectedList[i].unitPrice == unitPrice)
                    {
                        //selectedListDataGridView.Rows[i].Cells[2].Selected = true;
                        selectedListDataGridView.CurrentCell = selectedListDataGridView.Rows[i].Cells[2];
                    }
                }
            }
        }

        private void allProductDataGridView_Click(object sender, EventArgs e)
        {
            if (allProductDataGridView.SelectedCells.Count > 0)
            {
                int selectedRow = allProductDataGridView.SelectedCells[0].RowIndex;

                String name = allProductDataGridView.Rows[selectedRow].Cells[1].Value.ToString();
                int unitPrice = int.Parse(allProductDataGridView.Rows[selectedRow].Cells[2].Value.ToString());
                String category = allProductDataGridView.Rows[selectedRow].Cells[3].FormattedValue.ToString();

                editProductNameTextBox.Text = name;
                editProductUnitPriceTextBox.Text = unitPrice.ToString();
                editProductCategoryTextBox.Text = category;
                selectedProductNameLabel.Text = name + " (" + unitPrice + " Ft)";
                editProductPanel.Enabled = true;
                //editProductTabControl.SelectedIndex = 1;

                ProductReadonly product = ProductHandler.FindProduct(name, unitPrice);
                selectedProductId = product.id;
            }
        }

        private void selectedListDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (selectedListDataGridView.SelectedCells.Count > 0)
            {
                int selectedRow = selectedListDataGridView.SelectedCells[0].RowIndex;

                String selectedName = selectedListDataGridView.Rows[selectedRow].Cells[1].Value.ToString();
                int selectedUnitPrice = int.Parse(selectedListDataGridView.Rows[selectedRow].Cells[2].Value.ToString());
                
                ProductReadonly selectedProduct = ProductHandler.FindProduct(selectedName, selectedUnitPrice);

                if (selectedProduct != null)
                {
                    selectedProductId = selectedProduct.id;

                    for (int i = 0; i < allProductDataGridView.Rows.Count; i++)
                    {
                        String name = allProductDataGridView.Rows[i].Cells[1].Value.ToString();
                        int unitPrice = int.Parse(allProductDataGridView.Rows[i].Cells[2].Value.ToString());
                        if (name.Equals(selectedName) && unitPrice == selectedUnitPrice)
                        {
                            allProductDataGridView.ClearSelection();
                            allProductDataGridView.Rows[i].Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void selectedListDataGridView_Click(object sender, EventArgs e)
        {
            
        }
        
        private void selectedListDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (selectedListDataGridView.SelectedCells.Count > 0
                && selectedListDataGridView.SelectedCells[0].ColumnIndex != 2)
            {
                int selectedRow = selectedListDataGridView.SelectedCells[0].RowIndex;

                String selectedName = selectedListDataGridView.Rows[selectedRow].Cells[0].Value.ToString();
                int selectedUnitPrice = int.Parse(selectedListDataGridView.Rows[selectedRow].Cells[1].Value.ToString());

                selectedList.Remove(selectedList.FirstOrDefault(x => x.name.Equals(selectedName) && x.unitPrice == selectedUnitPrice));

                SaveWarningLabel.Visible = true;

                selectedListDataGridViewRefresh();
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            String selectedListName = oldListComboBox.SelectedValue.ToString();
            selectedListGroupBox.Text = selectedListName;

            var openedList = ShoppingListHandler.GetList(selectedListGroupBox.Text);
            if (openedList != null)
            {
                selectedList = openedList.list;
                SLESource.DataSource = selectedList;
                selectedListDataGridView.DataSource = SLESource;
                selectedListDataGridViewRefresh();

                updateButton.Enabled = true;
                deleteButton.Enabled = true;
                openButton.Enabled = false;
                SaveWarningLabel.Visible = false;
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (oldListComboBox.SelectedValue != null)
            {
                String selectedListName = oldListComboBox.SelectedValue.ToString();
                ShoppingListHandler.DeleteList(selectedListName);
                SavedObjectHandler.SaveObject();
                selectedListGroupBox.Text = "Új lista (nincs mentve)";
                selectedList.Clear();
                SaveWarningLabel.Visible = false;
                savedListsComboBoxRefresh();
                selectedListDataGridViewRefresh();
            }
        }

        private void oldListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldListComboBox.SelectedValue.Equals(selectedListGroupBox.Text))
            {
                updateButton.Enabled = true;
                deleteButton.Enabled = true;
                openButton.Enabled = false;
            }
            else
            {
                updateButton.Enabled = false;
                deleteButton.Enabled = false;
                openButton.Enabled = true;
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            ShoppingListHandler.UpdateList(selectedListGroupBox.Text, selectedList);
            SavedObjectHandler.SaveObject();
            SaveWarningLabel.Visible = false;
            selectedListDataGridViewRefresh();
        }
        
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            setSearchResult();
            allProductDataGridViewRefresh();
        }

        private void setSearchResult()
        {
            PSource.DataSource = allProductForDataGridView.Where(x => x.name.ToLower().Contains(SearchTextBox.Text.ToLower()));
            allProductDataGridView.DataSource = PSource;
        }

        
        private PrintDocument printDocument1 = new PrintDocument();
        private string stringToPrint = "";

        private void printButton_Click(object sender, EventArgs e)
        {
            WriteDocument();
            printDocument1.Print();
        }
        
        private void WriteDocument()
        {
            String separator = "  ";
            String sepLine = "__";

            stringToPrint = "";
            stringToPrint += "Darab" + separator + "Termék         " + separator + "Egységár" + separator + "Összesen\r\n";
            stringToPrint += "_____" + sepLine   + "_______________" + sepLine +   "________" + sepLine +   "________\r\n";

            for (int i = 0; i < selectedList.Count; i++)
            {
                String selectedName = selectedListDataGridView.Rows[i].Cells[0].Value.ToString();
                String selectedUnitPrice = selectedListDataGridView.Rows[i].Cells[1].Value.ToString() + " Ft";
                String count = selectedListDataGridView.Rows[i].Cells[2].Value.ToString() + " x";
                String price = selectedListDataGridView.Rows[i].Cells[3].Value.ToString() + " Ft";
                
                while (count.Length < 5) count = " " + count;
                while (selectedName.Length < 15) selectedName += ".";
                while (selectedUnitPrice.Length < 8) selectedUnitPrice = " " + selectedUnitPrice;
                while (price.Length < 8) price = " " + price;

                stringToPrint += count + separator + selectedName + separator + selectedUnitPrice + separator + price + "\r\n";
            }

            stringToPrint += "_____" + sepLine + "_______________" + sepLine + "________" + sepLine + "________\r\n";
            stringToPrint += "Végösszeg: " + sumPriceLabel.Text;

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            FontFamily fontFamily = new FontFamily("Consolas");
            Font font = new Font(
               fontFamily,
               14,
               FontStyle.Regular,
               GraphicsUnit.Pixel);

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
        }

        private void selectedListDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void selectedListDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            countNullValueRefresh();
            selectedListDataGridView.Refresh();
            sumPriceLabelRefresh();
            SaveWarningLabel.Visible = true;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            String selectedListName = "";
            if (oldListComboBox.SelectedValue != null) selectedListName = oldListComboBox.SelectedValue.ToString();
            else selectedListName = "Új lista";

            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            WriteDocument();

            System.IO.File.WriteAllText(Path.Combine(strPath, selectedListName + ".txt"), stringToPrint);
        }

        private void newListCreateButton_Click(object sender, EventArgs e)
        {
            List<ShoppingListElement> newSelectedList = new List<ShoppingListElement>(selectedList);

            ShoppingList sl = new ShoppingList(newListNameTextBox.Text, newSelectedList);
            if (ShoppingListHandler.CreateList(sl))
            {
                selectedList = newSelectedList;
                SLESource.DataSource = selectedList;
                selectedListDataGridView.DataSource = SLESource;
                SavedObjectHandler.SaveObject();

                selectedListGroupBox.Text = newListNameTextBox.Text;
                savedListsComboBoxRefresh();
                oldListComboBox.SelectedIndex = oldListComboBox.Items.IndexOf(newListNameTextBox.Text);
                newListNameTextBox.Text = "";
                editListTabControl.SelectedIndex = 1;
                selectedListDataGridViewRefresh();
            }
            SaveWarningLabel.Visible = false;
        }

        private void newEmptyListCreateButton_Click(object sender, EventArgs e)
        {
            List<ShoppingListElement> newSelectedList = new List<ShoppingListElement>();

            ShoppingList sl = new ShoppingList(newListNameTextBox.Text, newSelectedList);
            if (ShoppingListHandler.CreateList(sl))
            {
                selectedList = newSelectedList;
                SLESource.DataSource = selectedList;
                selectedListDataGridView.DataSource = SLESource;
                SavedObjectHandler.SaveObject();

                selectedListGroupBox.Text = newListNameTextBox.Text;
                savedListsComboBoxRefresh();
                oldListComboBox.SelectedIndex = oldListComboBox.Items.IndexOf(newListNameTextBox.Text);
                newListNameTextBox.Text = "";
                editListTabControl.SelectedIndex = 1;
                selectedListDataGridViewRefresh();
            }
            SaveWarningLabel.Visible = false;
        }

        private void selectedListDataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            pressOnlyNumber(sender, e);
        }

        private void createProductButton_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                createProductButton.PerformClick();
            }
        }

        private void updateProductButton_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                updateProductButton.PerformClick();
            }
        }

        private void allProductDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < allProductDataGridView.Rows.Count; i++)
            {
                allProductDataGridView.Rows[i].Selected = false;
            }
            allProductDataGridView.Rows[e.RowIndex].Selected = true;
        }

        private void selectedListDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(pressOnlyNumber);
            if (selectedListDataGridView.CurrentCell.ColumnIndex == 2) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(pressOnlyNumber);
                }
            }
        }

        private void selectedListDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (selectedListDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                selectedListDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "0";
        }
    }
}
