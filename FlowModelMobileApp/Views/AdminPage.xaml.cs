using System;
using System.Collections.Generic;
using SQLite;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlowModelMobileApp.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class AdminPage : ContentPage
   {
      bool isFine = true;
      string currentTable;
      public List<UsersTable> AuthList { get; set; }
      public List<MaterialsTable> MatList { get; set; }
      public List<PropsTable> PropList { get; set; }
      public List<LinkTables> LinkList { get; set; }

      public class UsersTable
      {
         public string Login { get; set; }
         public string Password { get; set; }
         public string Role { get; set; }
      }
      public class MaterialsTable
      {
         public int MaterialId { get; set; }
         public string MaterialName { get; set; }
      }
      public class PropsTable
      {
         public string PropertyName { get; set; }
         public string PropertyUnit { get; set; }
         public string PropertyType { get; set; }
      }
      public class LinkTables
      {
         public int MaterialId { get; set; }
         public int PropertiesId { get; set; }
         public double Value { get; set; }
      }

      public AdminPage()
      {
         InitializeComponent();
         ChoosenTable.Items.Insert(0, "Пользователи");
         ChoosenTable.Items.Insert(1, "Материалы");
         ChoosenTable.Items.Insert(2, "Свойства");
         ChoosenTable.Items.Insert(3, "Таблица связи");
         UnitType.Items.Insert(0, "Свойство материала");
         UnitType.Items.Insert(1, "Эмпирический коэффициент модели");
         UpdatePickers();
      }



      private void AddUserButtonClick(object sender, EventArgs e)
      {
         Navigation.PushAsync(new AddUser());
      }

      private void ChoosenTableChanged(object sender, EventArgs e)
      {
         if (ChoosenTable.SelectedIndex == 0)
         {
            currentTable = "Users";
            ResultTable.Columns.Clear();
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Login", MappingName = "Login" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Password", MappingName = "Password" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Role", MappingName = "Role" });
            
            selectFromTableUsers();
         }
         else if (ChoosenTable.SelectedIndex == 1)
         {
            currentTable = "Material";
            ResultTable.Columns.Clear();
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Номер материала", MappingName = "MaterialId" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Название материала", MappingName = "MaterialName" });
            selectFromTableMaterials();
         }
         else if (ChoosenTable.SelectedIndex == 2)
         {
            currentTable = "Properties";
            ResultTable.Columns.Clear();
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Название свойства", MappingName = "PropertyName" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Единица измерения", MappingName = "PropertyUnit" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Тип", MappingName = "PropertyType" });
            selectFromTableProperties();
         }
         else if (ChoosenTable.SelectedIndex == 3)
         {
            currentTable = "Material_has_properties";
            ResultTable.Columns.Clear();
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Номер материала", MappingName = "MaterialId" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Номер свойства", MappingName = "PropertiesId" });
            ResultTable.Columns.Add(new GridTextColumn() { HeaderText = "Значение", MappingName = "Value" });
            selectFromTableMaterialHasProperties();
         }
      }

      public void selectFromTableUsers()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.usersFilePath))
         {
            conn.CreateTable<Users>();
            var data = conn.Table<Users>();
            var users = data.ToList();
            AuthList = new List<UsersTable>();
            for(int i = 0; i < users.Count; i++)
            {
               AuthList.Add(new UsersTable { Login = users[i].Login, Password = users[i].Password, Role = users[i].Role });
            }

            ResultTable.ItemsSource = AuthList;
         }

         ResultTable.Refresh();
      }

      async private void MaterialAdd(object sender, EventArgs e)
      {
         Materials materials = new Materials()
         {
            MaterialName = MaterialNameText.Text
         };
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            int rowsAdded = conn.Insert(materials);
            await DisplayAlert("Успех", "Материал добавлен успешно!", "OK");
            MaterialNameText.Text = "";
         }
         UpdatePickers();
      }

      public void selectFromTableMaterials()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            var data = conn.Table<Materials>();
            var materialses = data.ToList();
            MatList = new List<MaterialsTable>();
            for (int i = 0; i < materialses.Count; i++)
            {
               MatList.Add(new MaterialsTable { MaterialId = materialses[i].MaterialId, MaterialName = materialses[i].MaterialName });
            }

            ResultTable.ItemsSource = MatList;
         }
         ResultTable.Refresh();
      }

      public void selectFromTableProperties()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Properties>();
            var data = conn.Table<Properties>();
            var props = data.ToList();
            PropList = new List<PropsTable>();
            for (int i = 0; i < props.Count; i++)
            {
               PropList.Add(new PropsTable { PropertyName = props[i].PropertyName, PropertyUnit = props[i].PropertyUnit, PropertyType = props[i].PropertyType});
            }

            ResultTable.ItemsSource = PropList;
         }

         ResultTable.Refresh();
      }

      public void selectFromTableMaterialHasProperties()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Material_has_Properties>();
            var data = conn.Table<Material_has_Properties>();
            var props = data.ToList();
            LinkList = new List<LinkTables>();
            for (int i = 0; i < props.Count; i++)
            {
               LinkList.Add(new LinkTables { MaterialId = props[i].MaterialId, PropertiesId = props[i].PropertiesId, Value = props[i].Value });
            }

            ResultTable.ItemsSource = LinkList;
         }

         ResultTable.Refresh();
      }

      async private void CheckValue()
      {
         isFine = true;
         try
         {
            double val;

            if (Double.TryParse(PropValue.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nПрограмный комплекс принимает только числа!");
            }

            double tmp = Convert.ToDouble(PropValue.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение должно быть больше нуля!");
            }
            if (tmp > 20000)
            {
               throw new Exception("Значение слишком большое!");
            }

         }
         catch (Exception ex)
         {
            isFine = false;
            await DisplayAlert("Ошибка", ex.Message, "OK");
         }
      }

      async private void AddLinkButton_OnClicked(object sender, EventArgs e)
      {
         CheckValue();
         if (isFine == false) 
         {
            return;
         }
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            var matId = conn.Query<Materials>("Select MaterialId from Materials Where MaterialName = '"+ LinkMaterial.Items[LinkMaterial.SelectedIndex] + "'").ToArray()[0].MaterialId;
            var propId = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = '"+ LinkProperties.Items[LinkProperties.SelectedIndex] + "'").ToArray()[0].PropertyId;
            Material_has_Properties link = new Material_has_Properties()
            {
               MaterialId = matId,
               PropertiesId = propId,
               Value = Convert.ToDouble(PropValue.Text)
            };

            conn.CreateTable<Material_has_Properties>();
            int rowsAdded = conn.Insert(link);
            await DisplayAlert("Успех", "Значение добавлено успешно!", "OK");
            PropNameText.Text = "";
            UnitNameText.Text = "";
         }
      }


      private string selectedUnitType;
      private void UnitType_OnSelectedIndexChanged(object sender, EventArgs e)
      {
         if (UnitType.SelectedIndex == 0)
         {
            selectedUnitType = "Свойство материала";
         }
         if (UnitType.SelectedIndex == 1)
         {
            selectedUnitType = "Эмпирический коэффициент модели";
         }
      }


      async private void AddPropButton_OnClicked(object sender, EventArgs e)
      {
         Properties prop = new Properties()
         {
            PropertyName = PropNameText.Text,
            PropertyUnit = UnitNameText.Text,
            PropertyType = selectedUnitType
         };
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Properties>();
            int rowsAdded = conn.Insert(prop);
            await DisplayAlert("Успех", "Свойство добавлено успешно!", "OK");
            PropNameText.Text = "";
            UnitNameText.Text = "";
         }
         UpdatePickers();
      }

      public void UpdatePickers()
      {
         LinkProperties.Items.Clear();
         LinkMaterial.Items.Clear();
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            var data = conn.Table<Materials>();
            var materials = data.ToList();
            for(int i = 0; i< materials.Count; i++)
            {
               LinkMaterial.Items.Insert(i, materials[i].MaterialName);
            }
            conn.CreateTable<Properties>();
            var data2 = conn.Table<Properties>();
            var properties = data2.ToList();
            for (int i = 0; i < properties.Count; i++)
            {
               LinkProperties.Items.Insert(i, properties[i].PropertyName);
            }
           
         }
      }
      
      public void Delete_Value(object sender, EventArgs e)
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            
            conn.Query<Material_has_Properties>("Delete From Material_has_Properties Where MaterialId = 0");
            
            conn.Query<Materials>("Delete from Materials where MaterialId > 0");
            
            conn.Query<Properties>("Delete from Properties where PropertyId > 0  ");
            
            
         }
      }
      public class NumericInput : Entry
      {
         public static BindableProperty AllowNegativeProperty = BindableProperty.Create("AllowNegative", typeof(bool), typeof(NumericInput), false, BindingMode.TwoWay);
         public static BindableProperty AllowFractionProperty = BindableProperty.Create("AllowFraction", typeof(bool), typeof(NumericInput), false, BindingMode.TwoWay);

         public NumericInput()
         {
            this.Keyboard = Keyboard.Numeric;
         }

         public bool AllowNegative
         {
            get { return (bool)GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
         }

         public bool AllowFraction
         {
            get { return (bool)GetValue(AllowFractionProperty); }
            set { SetValue(AllowFractionProperty, value); }
         }
      }
   }
}