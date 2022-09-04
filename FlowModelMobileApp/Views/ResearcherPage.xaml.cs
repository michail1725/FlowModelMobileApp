using FlowModelMobileApp.Objects;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FlowModelMobileApp.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class ResearcherPage : ContentPage
   {
      bool isFine = true;
      public List<Prop> Props { get; set; }

      delegate bool IsEqual(string x);

      public Dictionary<string, double> props;

      public ResearcherPage()
      {
         InitializeComponent();
         UpdatePickers();
      }

      public void UpdatePickers()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            var data = conn.Table<Materials>();
            var materials = data.ToList();
            for (int i = 0; i < materials.Count; i++)
            {
               MaterialPicker.Items.Insert(i, materials[i].MaterialName);
            }
         }
      }

      private void MaterialPicker_SelectedIndexChanged(object sender, EventArgs e)
      {
         StartSimulation.IsEnabled = true;
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            var matId = conn.Query<Materials>("Select MaterialId from Materials Where MaterialName = '" + MaterialPicker.Items[MaterialPicker.SelectedIndex] + "'").ToArray()[0].MaterialId;
            var que1 = conn.Query<Material_has_Properties>("SELECT MaterialId, PropertiesId, Value FROM Material_has_properties where MaterialId ='" + matId + "';");
            var que2 = conn.Query<Properties>("SELECT PropertyId, PropertyName, PropertyUnit FROM Properties Where PropertyId IN (SELECT PropertyId FROM Material_has_properties where MaterialId ='" + matId + "') "); //AND PropertyType = 'Свойство материала'
            Props = new List<Prop>();
            for (int i = 0; i < que2.Count; i++)
            {
               Props.Add(new Prop { Name = que2[i].PropertyName, Unit = que2[i].PropertyUnit, Value = que1[i].Value.ToString() });
            }
            PropsGrid.ItemsSource = Props;
            PropsGrid.Refresh();
         }
      }

      public class Prop
      {
         public string Name { get; set; }
         public string Unit { get; set; }
         public string Value { get; set; }
      }

      private void StartSimulation_Clicked(object sender, EventArgs e)
      {
         CheckValues();
         if (isFine == false)
         {
            return;
         }
         props = new Dictionary<string, double>();
         foreach (Prop prop in Props)
         {
            if (prop.Value != "Значение")
            {
               props.Add(prop.Name, Convert.ToDouble(prop.Value));
            }
         }

         SimulationObject.Material = new Material();
         SimulationObject.Material.Alpha_u = ThisProp(x => x == "Коэффициент теплоотдачи крышки");
         SimulationObject.Material.B = ThisProp(x => x == "Температурный коэффициент вязкости");
         SimulationObject.Material.C = ThisProp(x => x == "Удельная теплоемкость");
         SimulationObject.Material.Material_name = MaterialPicker.SelectedItem.ToString();
         SimulationObject.Material.Mu0 = ThisProp(x => x == "Коэффициент консистенции приведения");
         SimulationObject.Material.N = ThisProp(x => x == "Индекс течения");
         SimulationObject.Material.Ro = ThisProp(x => x == "Плотность");
         SimulationObject.Material.T0 = ThisProp(x => x == "Температура плавления");
         SimulationObject.Material.Tr = ThisProp(x => x == "Температура приведения");
         SimulationObject.Canal = new Canal();
         SimulationObject.Canal.Width = Convert.ToDouble(G_Width.Text);
         SimulationObject.Canal.Height = Convert.ToDouble(G_Depth.Text);
         SimulationObject.Canal.Length = Convert.ToDouble(G_Lenght.Text);
         SimulationObject.Canal.Cap = new Cap();
         SimulationObject.Canal.Cap.Tu = Convert.ToDouble(Cap_Temp.Text);
         SimulationObject.Canal.Cap.Vu = Convert.ToDouble(Cap_Speed.Text);
         SimulationObject.Step = Convert.ToDouble(Step.Text);

         Navigation.PushAsync(new SimulationOverview());
      }

      double ThisProp(IsEqual func)
      {
         foreach (var prop in props)
         {
            if (func(prop.Key))
            {
               return prop.Value;
            }
         }

         return 0;
      }
      async private void CheckValues()
      {
         isFine = true;
         try
         {
            double val;

            if (Double.TryParse(G_Width.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nШирина может быть только числом!");
            }

            double tmp = Convert.ToDouble(G_Width.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение ширины должно быть больше нуля!");
            }
            if (tmp > 100)
            {
               throw new Exception("Значение ширины слишком большое!");
            }

            if (Double.TryParse(G_Depth.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nГлубина может быть только числом!");
            }

            tmp = Convert.ToDouble(G_Depth.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение глубины должно быть больше нуля!");
            }
            if (tmp > 1000)
            {
               throw new Exception("Значение глубины слишком большое!");
            }
            if (Double.TryParse(G_Lenght.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nДлина может быть только числом!");
            }

            tmp = Convert.ToDouble(G_Lenght.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение длины должно быть больше нуля!");
            }
            if (tmp > 1000)
            {
               throw new Exception("Значение длины слишком большое!");
            }
            if (Double.TryParse(Cap_Temp.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nТемпература может быть только числом!");
            }

            tmp = Convert.ToDouble(Cap_Temp.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение температуры должно быть больше нуля!");
            }
            if (tmp > 10000)
            {
               throw new Exception("Значение температуры слишком большое!");
            }
            if (Double.TryParse(Cap_Speed.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nСкорость может быть только числом!");
            }

            tmp = Convert.ToDouble(Cap_Speed.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение скорости должно быть больше нуля!");
            }
            if (tmp > 1000)
            {
               throw new Exception("Значение скорости слишком большое!");
            }
            if (Double.TryParse(Step.Text, out val))
            {
            }
            else
            {
               throw new Exception("Вы ввели текст!\nШаг может быть только числом!");
            }

            tmp = Convert.ToDouble(Step.Text);

            if (tmp <= 0)
            {
               throw new Exception("Значение температуры должно быть больше нуля!");
            }
            if (tmp <= 0)
            {
               throw new Exception("Шаг по длине канала должен быть больше нуля!");
            }

            if (tmp > Convert.ToDouble(G_Lenght.Text))
            {
               throw new Exception("Шаг не может превышать длину канала!");
            }
         }
         catch (Exception ex)
         {
            isFine = false;
            await DisplayAlert("Ошибка", ex.Message, "OK");
         }
      }
   }
}