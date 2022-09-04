using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;


namespace FlowModelMobileApp
{
   public class InsertBasicInfo
   {
      
      public static void InsertProperties()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            Properties prop = new Properties
            {
               PropertyName = "Плотность",
               PropertyUnit = "кг/м^3",
               PropertyType = "Свойство материала"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Удельная теплоемкость",
               PropertyUnit = "Дж/(кг·°С)",
               PropertyType = "Свойство материала"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Температура плавления",
               PropertyUnit = "°С",
               PropertyType = "Свойство материала"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Температура приведения",
               PropertyUnit = "°С",
               PropertyType = "Эмпирический коэффициент"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Коэффициент консистенции приведения",
               PropertyUnit = "Па·с^n",
               PropertyType = "Эмпирический коэффициент"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Температурный коэффициент вязкости",
               PropertyUnit = "1/°С",
               PropertyType = "Эмпирический коэффициент"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Индекс течения",
               PropertyUnit = "-",
               PropertyType = "Эмпирический коэффициент"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
            prop = new Properties
            {
               PropertyName = "Коэффициент теплоотдачи крышки",
               PropertyUnit = "Вт/(м^2·°С)",
               PropertyType = "Эмпирический коэффициент"
            };
            conn.CreateTable<Properties>();
            conn.Insert(prop);
         }
      }
      public static void InsertValues()
      {
         using (SQLiteConnection conn = new SQLiteConnection(App.flowModelFilePath))
         {
            conn.CreateTable<Materials>();
            var matId = conn.Query<Materials>("Select MaterialId from Materials Where MaterialName = 'Поливинилхлорид'").ToArray()[0].MaterialId;
            conn.CreateTable<Properties>();
            var propIdPlot = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Плотность'").ToArray()[0].PropertyId;
            var propIdTepl = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Удельная теплоемкость'").ToArray()[0].PropertyId;
            var propIdPlav = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Температура плавления'").ToArray()[0].PropertyId;
            var propIdPriv = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Температура приведения'").ToArray()[0].PropertyId;
            var propIdCoef = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Коэффициент консистенции приведения'").ToArray()[0].PropertyId;
            var propIdTempVyaz = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Температурный коэффициент вязкости'").ToArray()[0].PropertyId;
            var propIdInd = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Индекс течения'").ToArray()[0].PropertyId;
            var propIdTempCap = conn.Query<Properties>("Select PropertyId from Properties Where PropertyName = 'Коэффициент теплоотдачи крышки'").ToArray()[0].PropertyId;
            conn.CreateTable<Material_has_Properties>();
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPlot + "','" + 1380 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTepl + "','" + 2500 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPlav + "','" + 145 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPriv + "','" + 165 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdCoef + "','" + 12000 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTempVyaz + "','" + 0.05 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdInd + "','" + 0.28 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTempCap + "','" + 400 + "');");
            matId = conn.Query<Materials>("Select MaterialId from Materials Where MaterialName = 'Полипропилен'").ToArray()[0].MaterialId;
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPlot + "','" + 890 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTepl + "','" + 2300 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPlav + "','" + 175 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdPriv + "','" + 180 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdCoef + "','" + 1550 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTempVyaz + "','" + 0.015 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdInd + "','" + 0.4 + "');");
            conn.Query<Material_has_Properties>("Insert into Material_has_Properties (MaterialId, PropertiesId, Value) values ('" + matId + "','" + propIdTempCap + "','" + 2000 + "');");

         }
      }
   }
}
