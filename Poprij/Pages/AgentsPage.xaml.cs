using Poprij.Model;
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

namespace Poprij.Pages
{
    /// <summary>
    /// Логика взаимодействия для AgentsPage.xaml
    /// </summary>
    public partial class AgentsPage : Page
    {
        private int start = 0;
        private int fullCount = 0;
        int order;
        public AgentsPage(Frame frame)
        {
            InitializeComponent();
            
            fullCount = helper.GetContext().Agent.Count();
            full.Text = fullCount.ToString();
            DataContext = this;
            Load();

        }

        private void paginButto_Click(object sender, RoutedEventArgs e)
        {
            start = Convert.ToInt32(((Button)sender).Tag.ToString());
            Load();
        }

        private void turnButton()
        {
            if (start == 0) { back.IsEnabled = false; }
            else { back.IsEnabled = true; };
            if ((start + 1) * 10 > fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; };
        }
        private void agentGrid_LoadingRow_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Agent agent = (Agent)e.Row.DataContext;
            int sum = 0;
            double fsum = 0;
            foreach (ProductSale ps in agent.ProductSale)
            {
                List<ProductMaterial> mtr = new List<ProductMaterial> { };
                mtr = helper.GetContext().ProductMaterial.Where(ProductMaterial => ProductMaterial.ProductID == ps.ProductID).ToList();
                foreach (ProductMaterial mt in mtr)
                {
                    double f = decimal.ToDouble(mt.Material.Cost);
                    fsum += f * (double)mt.Count;
                }
                fsum = fsum * ps.ProductCount;
                if (ps.SaleDate.AddDays(365).CompareTo(DateTime.Today) > 0)
                    sum += ps.ProductCount;
            }
            agent.sale = sum;
            agent.percent = 0;
            if (fsum >= 10000 && fsum < 50000) agent.percent = 5;
            if (fsum >= 50000 && fsum < 150000) agent.percent = 10;
            if (fsum >= 150000 && fsum < 500000) agent.percent = 20;
            if (fsum >= 500000) agent.percent = 25;
            if (agent.percent == 25)
            {
                SolidColorBrush hb = new SolidColorBrush(Colors.LightGreen);
                e.Row.Background = hb;
            };
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            order = Convert.ToInt32(selectedItem.Tag.ToString());
            Load();
        }
        

        public void Load()
        {
            agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            int ost = fullCount % 10;
            int pag = (fullCount - ost) / 10;
            if (ost > 0) pag++;
            pagin.Children.Clear();
            for (int i = 0; i < pag; i++)
            {
                Button myButton = new Button();
                myButton.Height = 30;
                myButton.Content = i + 1;
                myButton.Width = 20;
                myButton.HorizontalAlignment = HorizontalAlignment.Center;
                myButton.Tag = i;
                myButton.Click += new RoutedEventHandler(paginButto_Click); ;
                pagin.Children.Add(myButton);
            }

            try
            {
                fullCount = helper.GetContext().Agent.Count();
                if (order == 0) agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                if (order == 1) agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                if (order == 2) agentGrid.ItemsSource = helper.GetContext().Agent.OrderByDescending(Agent => Agent.Title).Skip(start * 10).Take(10).ToList();
                if (order == 3) agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();
                if (order == 4) agentGrid.ItemsSource = helper.GetContext().Agent.OrderByDescending(Agent => Agent.Priority).Skip(start * 10).Take(10).ToList();

            }
            catch
            {
                return;
            };

            turnButton();

        }


        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void revButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            start++;
            Load();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
