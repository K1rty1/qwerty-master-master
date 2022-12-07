using qwerty.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace qwerty.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        Core db = new Core();
        List<ProductType> productTypes;
        int countPage = 0;
        int countElement = 10;
        int page;
        public ProductPage()
        {
            InitializeComponent();
            List<string> sortTypeList = new List<string>()
            {"аименование","остаток на складе","стоймость"
            };
            SortComboBox.ItemsSource = sortTypeList;
            productTypes = new List<ProductType>
            {
                new ProductType()
                { 
                    ID=0,
                    Title="Все типы"
                }
            };
            productTypes.AddRange(db.context.ProductType.ToList());
            FilterComboBox.ItemsSource = productTypes;
            UpdateUI();
        }
        private int GetPagesCount()
        {
            
            int count = GetRows().Count;
            if (count>countElement)
            {
                countPage = Convert.ToInt32(Math.Ceiling(count * 1.0 / countElement));
            }
            return countPage;
        }
        public void DisplayPagination(int page)
        {
            List<PageItem> source = new List<PageItem>();
            for (int i = 1; i <= GetPagesCount(); i++)
            {
                source.Add(new PageItem(i, i == page));
            }
            PaginationListView.ItemsSource = source;
            PrevTextBlock.Visibility = (page <= 1 ? Visibility.Hidden : Visibility.Visible);
            NextTextBlock.Visibility = (page >= GetPagesCount() ? Visibility.Hidden:Visibility.Visible);
        }
        private void UpdateUI()
        {
            if (GetRows().Count>10)
            {
                 DisplayPagination(page);
                 List<Product> displayProduct = GetRows().Skip((page-1)*countElement).Take(countElement).ToList();
                 ProductListView.ItemsSource = displayProduct;
                 foreach (var item in displayProduct)
                 {
                    Console.WriteLine(item.ID);
                 }
            }
            else
            {
                PaginationListView.Visibility = Visibility.Collapsed;
            }
        }
        private void PrevTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page <= 1)
            {
                page = 1;
                PrevTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                page -= 1;
                PrevTextBlock.Visibility = Visibility.Visible;
            }
        }
        private List<Product> GetRows()
        {
            List<Product> arrayproduct = db.context.Product.ToList();
            string searchData = SearchTextBox.Text;
            if (!String.IsNullOrEmpty(SearchTextBox.Text))
            {
                arrayproduct = arrayproduct.Where(x => x.Title.ToUpper().Contains(searchData)).ToList();
            }
            if (FilterComboBox.SelectedIndex == 0)
            {
                arrayproduct = arrayproduct.ToList();
            }
            else
            {
                int filter = Convert.ToInt32(FilterComboBox.SelectedValue);
                arrayproduct = arrayproduct.Where(x => x.ProductTypeID==filter).ToList();
            }
            int value = SortComboBox.SelectedIndex;
            if (value==0)
            {
                arrayproduct = arrayproduct.OrderBy(p => p.Title).ToList();
            }
            else if (value==1)
            {
                arrayproduct = arrayproduct.OrderBy(p => p.ProductionWorkshopNumber).ToList();
            }
            return arrayproduct;
        }
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new UpdateProductPage());
        }
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Введите наименование продукта")
            {
                SearchTextBox.Text = "";
            }
        }
        private void SearchTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Введите наименование продукта";
            }
        }
        private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
        }
        private void FilterComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateUI();
        }
        private void ReverceButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void SortComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void NextTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (page >=GetPagesCount())
            {
                page = GetPagesCount();
                NextTextBlock.Visibility = Visibility.Hidden;   
            }
            else
            {
                page += 1;
                NextTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
    class PageItem
    {
        public int Num { get; set; }
        public string Selected { get; set; }
        public string TextColor { get; set; }
        public string FontWeight { get; set; }

        public PageItem()
        {
            Num = 0;
            Selected = "None";
            TextColor = "Black";
            FontWeight = "Normal";
        }
        public PageItem(int num, bool selected)
        {
            Num = num;
            Selected = (selected ? "Underline" : "None");
            TextColor = "Black";
            FontWeight = "Normal";
        }
    }
}
