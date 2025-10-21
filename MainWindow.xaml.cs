using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CustomerManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSqlLocalDB;Initial Catalog=Custer;Integrated Security=True");

        public void clearData()
        {
            name_txt.Clear();
            age_txt.Clear();
            gender_txt.Clear();
            search_txt.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("select * from FirstTable", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            datagrid.ItemsSource = dt.DefaultView;
        }

        private void ClearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public bool isVailed()
        {
            if (name_txt.Text == string.Empty)
            {
                MessageBox.Show("名前は必須です", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (age_txt.Text == string.Empty)
            {
                MessageBox.Show("年齢は必須です", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (gender_txt.Text == string.Empty)
            {
                MessageBox.Show("性別は必須です", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isVailed())
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO FirstTable VALUES (@Name, @Age, @Gender)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", name_txt.Text);
                    cmd.Parameters.AddWithValue("@Age", age_txt.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender_txt.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoadGrid();
                    MessageBox.Show("登録に成功しました", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from FirstTable where ID = " + search_txt.Text + " ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("レコードは削除されました", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                con.Close();
                clearData();
                LoadGrid();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("削除できませんでした" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("update FirstTable set Name = N'" + name_txt.Text + "', Age = '" + age_txt.Text + "', Gender = N'" + gender_txt.Text + "' Where ID = '" + search_txt.Text + "' ", con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("レコードは編集されました", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                clearData();
                LoadGrid();
            }
        }
    }
}