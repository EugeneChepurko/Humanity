using Humanity.Templates;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Windows.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace Humanity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Human> humans = new List<Human>();
        private uint id = 1;
        SqlConnection sqlConnection;
        string connectionString;
        //SqlDataAdapter adapter;
        //DataTable humanTable;
        //System.Timers.Timer timer = new System.Timers.Timer();

        public MainWindow()
        {
            InitializeComponent();

            connectionString = @"Data Source=GEKOS\SQLEXPRESS;Initial Catalog=Humans;Integrated Security=True";

            ReadBase();
            this.Closing += Window_Closing;
        }

        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            Human h = new Human();
            if (listbox.SelectedIndex != -1)
            {
                h.ShowSelectedItem((Human)listbox.SelectedItem);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddHumanToList();
            AddToBase();
            listbox.Items.Clear(); 
            ReadBase();
        }
        private async void ReadBase()
        {
            await Task.Delay(100);
            using (sqlConnection = new SqlConnection(connectionString))
            {
                SqlDataReader dataReader = null;
                SqlCommand command = new SqlCommand("SELECT * FROM myHumanity", sqlConnection);
                
                try
                {
                    await sqlConnection.OpenAsync();
                    dataReader = await command.ExecuteReaderAsync();

                    while (await dataReader.ReadAsync())
                    {
                        listbox.Items.Add(Convert.ToString(dataReader["name"]));
                    }
                }
                catch (TaskCanceledException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    dataReader?.Close();
                    sqlConnection?.Close();
                }
            }   
        }
        private async void AddToBase()
        {
            sqlConnection = null;

            using (sqlConnection = new SqlConnection(connectionString))
            {
                string sql = string.Format("Insert Into myHumanity" +
               "(name, surname, age, hp, mp, strength) Values(@name, @surname, @age, @hp, @mp, @strength)");

                try
                {
                    await sqlConnection.OpenAsync();
                    SqlCommand command = new SqlCommand(sql, sqlConnection);

                    command.Parameters.AddWithValue("@name", textBoxName.Text);
                    command.Parameters.AddWithValue("@surname", textBoxSName.Text);
                    command.Parameters.AddWithValue("@age", textBoxAge.Text);
                    command.Parameters.AddWithValue("@hp", textBoxHP.Text);
                    command.Parameters.AddWithValue("@mp", textBoxMana.Text);
                    command.Parameters.AddWithValue("@strength", textBoxStr.Text);

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection?.Close();
                }
            }
        }

        private void Btn4_Click(object sender, RoutedEventArgs e) => ShowListHumans();
        private void Btn5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listbox.SelectedIndex != -1)
                {
                    humans.RemoveAt(listbox.SelectedIndex);
                    listbox.Items.RemoveAt(listbox.SelectedIndex);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddHumanToList()
        {
            try
            {
                Human human = new Human();
                human.Name = textBoxName.Text;
                human.Surname = textBoxSName.Text;
                human.Age = Convert.ToUInt32(textBoxAge.Text);
                human.HitPoints = float.Parse(textBoxHP.Text, CultureInfo.InvariantCulture);
                human.Mana = float.Parse(textBoxMana.Text, CultureInfo.InvariantCulture);
                human.Strength = float.Parse(textBoxStr.Text, CultureInfo.InvariantCulture);
                human.Id = Convert.ToUInt32(textBoxId.Text = id++.ToString());//Guid.NewGuid().ToString();
                listbox.Items.Add(human);
                humans.Add(human);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }        
        }

        private void ShowListHumans()
        {
            foreach (Human human in humans)
            {
                MessageBox.Show(human.ToString());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
                sqlConnection.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            listbox.Items.Clear();
            ReadBase();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);
            using (sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE myHumanity SET name = @name, surname = @surname, age = @age, hp = @hp, mp = @mp, strength = @strength WHERE id = @id", sqlConnection);
                try
                {
                    await sqlConnection.OpenAsync();

                    command.Parameters.AddWithValue("id", textBoxId.Text);
                    command.Parameters.AddWithValue("name", textBoxName.Text);
                    command.Parameters.AddWithValue("surname", textBoxSName.Text);
                    command.Parameters.AddWithValue("age", textBoxAge.Text);
                    command.Parameters.AddWithValue("hp", textBoxHP.Text);
                    command.Parameters.AddWithValue("mp", textBoxMana.Text);
                    command.Parameters.AddWithValue("strength", textBoxStr.Text);
                    
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection?.Close();
                }
            }
            listbox.Items.Clear();
            ReadBase();
        }
        private async void Del_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(50);
            using (sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM myHumanity WHERE id = @id", sqlConnection);
                try
                {
                    await sqlConnection.OpenAsync();
                    command.Parameters.AddWithValue("id", textBoxId.Text);
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    sqlConnection?.Close();
                }
            }
            listbox.Items.Clear();
            ReadBase();
        }

        private async void Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //await Task.Delay(100);
            
            using (sqlConnection = new SqlConnection(connectionString))
            {
                if(listbox.SelectedIndex != -1)
                {
                    string q = "SELECT * FROM myHumanity where name = '" + listbox.SelectedItem.ToString() + "'";
                    SqlDataReader dataReader = null;
                    SqlCommand command = new SqlCommand(q, sqlConnection);
                    try
                    {
                        await sqlConnection.OpenAsync();
                        dataReader = await command.ExecuteReaderAsync();

                        while (await dataReader.ReadAsync())
                        {
                            textBoxId.Text = dataReader["id"].ToString();
                            textBoxName.Text = dataReader["name"].ToString();
                            textBoxSName.Text = dataReader["surname"].ToString();
                            textBoxAge.Text = dataReader["age"].ToString();
                            textBoxHP.Text = dataReader["hp"].ToString();
                            textBoxMana.Text = dataReader["mp"].ToString();
                            textBoxStr.Text = dataReader["strength"].ToString();
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        dataReader?.Close();
                        sqlConnection?.Close();
                    }
                }
                
            }
        }
        //private async void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    using (sqlConnection = new SqlConnection(connectionString))
        //    {
        //        string q = "SELECT * FROM myHumanity";
        //        SqlDataReader dataReader = null;
        //        SqlCommand command = new SqlCommand(q, sqlConnection);
        //        try
        //        {
        //            await sqlConnection.OpenAsync();
        //            dataReader = await command.ExecuteReaderAsync();
        //            dataGrid = (DataGrid)sender;
        //            DataRowView row_selected = dataGrid.SelectedItem as DataRowView;
        //            if(row_selected != null)
        //            {
        //                textBoxId.Text = row_selected["id"].ToString();
        //                textBoxName.Text = row_selected["name"].ToString();
        //                textBoxSName.Text = row_selected["surname"].ToString();
        //                textBoxAge.Text = row_selected["age"].ToString();
        //                textBoxHP.Text = row_selected["hp"].ToString();
        //                textBoxMana.Text = row_selected["mp"].ToString();
        //                textBoxStr.Text = row_selected["strength"].ToString();
        //            }
        //        }
        //        catch (TaskCanceledException ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        finally
        //        {
        //            dataReader?.Close();
        //            sqlConnection?.Close();
        //        }
        //    }
        //}
    }
}
