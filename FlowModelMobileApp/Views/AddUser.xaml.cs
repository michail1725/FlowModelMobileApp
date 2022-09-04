using System;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlowModelMobileApp.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class AddUser : ContentPage
   {
      string selectedRole;
      public AddUser()
      {
         InitializeComponent();
         RolePicker.Items.Insert(0, "Администратор");
         RolePicker.Items.Insert(1, "Исследователь");
      }
      private void RolePicker_OnSelectedIndexChanged_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (RolePicker.SelectedIndex == 0)
         {
            selectedRole = "admin";
         }
         else
         {
            selectedRole = "research";
         }
      }

      async private void UserAdd(object sender, EventArgs e)
      {
         Users users = new Users()
         {
            Login = LoginText.Text,
            Password = PasswordText.Text,
            Role = selectedRole
         };

         using (SQLiteConnection conn = new SQLiteConnection(App.usersFilePath))
         {
            conn.CreateTable<Users>();
            int rowsAdded = conn.Insert(users);
            await DisplayAlert("Успех", "Пользователь добавлен успешно!", "OK");
         }
      }
   }
}