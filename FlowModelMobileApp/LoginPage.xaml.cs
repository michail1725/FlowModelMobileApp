using FlowModelMobileApp.Views;
using System;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace FlowModelMobileApp
{
   public partial class MainPage : ContentPage
   {
      public MainPage()
      {
         InitializeComponent();
      }

      public void check()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.usersFilePath))
         {
            conn.CreateTable<Users>();
            var data = conn.Table<Users>();
            var size = data.ToList();
            int i = size.Count;
            if (i == 0)
            {
               Users users = new Users()
               {
                  Login = "admin",
                  Password = "admin",
                  Role = "admin"
               };
               conn.CreateTable<Users>();
               users = new Users()
               {
                  Login = "user",
                  Password = "user",
                  Role = "research"
               };
               conn.CreateTable<Users>();

            }

         }
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            var data = conn.Table<Materials>();
            var size = data.ToList();
            int i = size.Count;
            if (i == 0)
            {
               Materials materials = new Materials()
               {
                  MaterialId = 1,
                  MaterialName = "Поливинилхлорид"
               };
               conn.CreateTable<Materials>();
               conn.Insert(materials);
               materials = new Materials()
               {
                  MaterialId = 2,
                  MaterialName = "Полипропилен"
               };
               conn.CreateTable<Materials>();
               conn.Insert(materials);
            }
            conn.CreateTable<Properties>();
            var data1 = conn.Table<Properties>();
            if (data1.ToList().Count == 0)
            {
               InsertBasicInfo.InsertProperties();
            }
            conn.CreateTable<Material_has_Properties>();
            var data2 = conn.Table<Material_has_Properties>();
            if (data2.ToList().Count == 0)
            {
               InsertBasicInfo.InsertValues();
            }
         }

      }

      private void LoginClicked(object sender, EventArgs eventArgs)
      {
            check();
            string currentRole = "";
         using (SQLiteConnection conn = new SQLiteConnection(App.usersFilePath))
         {
            conn.CreateTable<Users>();
            var data = conn.Table<Users>();
            var users = data.ToList();
            var d1 = (from user in users
               where user.Login == LoginText.Text && user.Password == PasswordText.Text
               select user.Role);
            currentRole = d1.FirstOrDefault();
         }
            if (currentRole == null) { 
            DisplayAlert("Предупреждение", "Пользователя с такими входными данными не существует!", "ОK");
                LoginText.Text = "";
                PasswordText.Text = "";
                return;
            }
         LoginText.Text = "";
         PasswordText.Text = "";

         if (currentRole == "admin")
         {
            Navigation.PushAsync(new AdminPage());
         }
         else if (currentRole == "research")
         {
            Navigation.PushAsync(new ResearcherPage());
         }

      }



   }
}
