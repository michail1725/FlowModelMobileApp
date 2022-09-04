using System;
using System.Collections.Generic;
using System.Diagnostics;
using obj = FlowModelMobileApp.Objects.SimulationObject;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.SfDataGrid.XForms.Exporting;
using System.IO;

namespace FlowModelMobileApp.Views
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class SimulationOverview : ContentPage
   {
      public double q_ch, eta_prod, t_prod;
      public List<double> eta_ch;
      public List<double> t_ch;
      public List<CanalPoint> MaterialCondition { get; set; }
      public SimulationOverview()
      {
         InitializeComponent();
         Simulation();
      }
      public void Simulation()
      {

         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start();
         double gamma, ae, F, q_gamma, q_alpha;
         F = 0.125 * Math.Pow(obj.Canal.Height / obj.Canal.Width, 2) - 0.625 * (obj.Canal.Height / obj.Canal.Width) + 1.0;
         q_ch = F * obj.Canal.Width * obj.Canal.Cap.Vu * obj.Canal.Height / 2.0;
         gamma = obj.Canal.Cap.Vu / obj.Canal.Height;
         q_gamma = obj.Canal.Width * obj.Canal.Height * obj.Material.Mu0 * Math.Pow(gamma, obj.Material.N + 1.0);
         q_alpha = obj.Canal.Width * obj.Material.Alpha_u * (1.0 / obj.Material.B + obj.Material.Tr - obj.Canal.Cap.Tu);
         eta_ch = new List<double>();
         t_ch = new List<double>();
         MaterialCondition = new List<CanalPoint>();
         CanalPoint canalPoint;
         for (double z = 0.0; Math.Round(z, GetDecimalDigitsCount(obj.Step)) < obj.Canal.Length; z += obj.Step)
         {
            z = Math.Round(z, GetDecimalDigitsCount(obj.Step));
            ae = ((obj.Material.B * q_gamma + obj.Canal.Width * obj.Material.Alpha_u) / (obj.Material.B * q_alpha)) *
                 (1.0 - Math.Exp(-(z * obj.Material.B * q_alpha / (obj.Material.Ro * obj.Material.C * q_ch)))) +
                 Math.Exp(obj.Material.B * (obj.Material.T0 - obj.Material.Tr -
                                            (z * q_alpha / (obj.Material.Ro * obj.Material.C * q_ch))));
            double t_temp1 = obj.Material.Tr + 1.0 * Math.Log(ae) / obj.Material.B;
            t_ch.Add(Math.Round(t_temp1, 2));
            eta_ch.Add(Math.Round(
               Math.Pow(gamma, obj.Material.N - 1) * obj.Material.Mu0 *
               Math.Exp(-obj.Material.B * (t_temp1 - obj.Material.Tr)), 1));
            canalPoint = new CanalPoint();
            canalPoint.x = z;
            canalPoint.t = t_ch[t_ch.Count - 1];
            canalPoint.eta = eta_ch[eta_ch.Count - 1];
            MaterialCondition.Add(canalPoint);
         }
         ae = ((obj.Material.B * q_gamma + obj.Canal.Width * obj.Material.Alpha_u) / (obj.Material.B * q_alpha)) *
              (1.0 - Math.Exp(
                 -(obj.Canal.Length * obj.Material.B * q_alpha / (obj.Material.Ro * obj.Material.C * q_ch)))) +
              Math.Exp(obj.Material.B * (obj.Material.T0 - obj.Material.Tr -
                                         (obj.Canal.Length * q_alpha / (obj.Material.Ro * obj.Material.C * q_ch))));
         double t_temp = obj.Material.Tr + 1.0 * Math.Log(ae) / obj.Material.B;
         t_ch.Add(Math.Round(t_temp, 2));
         eta_ch.Add(Math.Round(Math.Pow(gamma, obj.Material.N - 1) * obj.Material.Mu0 *
         Math.Exp(-obj.Material.B * (t_temp - obj.Material.Tr)), 1));
         eta_prod = eta_ch[eta_ch.Count - 1];
         t_prod = t_ch[t_ch.Count - 1];
         canalPoint = new CanalPoint();
         canalPoint.x = obj.Canal.Length;
         canalPoint.t = t_ch[t_ch.Count - 1];
         canalPoint.eta = eta_ch[eta_ch.Count - 1];
         MaterialCondition.Add(canalPoint);
         PerfomanceLabel.Text = "Производительность (кг/ч): " + Math.Round(q_ch * 3600 * 1380).ToString();
         TemperatureLabel.Text = "Температура продукта(°C): " + t_prod.ToString();
         ViscosityLabel.Text = "Вязкость продукта(Па*с): " + eta_prod.ToString();
         stopwatch.Stop();

         T_Chart.ItemsSource = MaterialCondition;
         V_Chart.ItemsSource = MaterialCondition;
         ResultsGrid.ItemsSource = MaterialCondition;
         CalcTime.Text = "Затрачено времени: " + stopwatch.ElapsedMilliseconds.ToString() + " мс";
         CalcMem.Text = "Затрачено памяти: " + Math.Round((Process.GetCurrentProcess().PeakWorkingSet64 / Math.Pow(1024, 2)), 2).ToString() + " Мб";

      }

      private void SaveReport_Clicked(object sender, EventArgs e)
      {
         DataGridExcelExportingOption option = new DataGridExcelExportingOption();
         option.StartRowIndex = 16;
         DataGridExcelExportingController excelExport = new DataGridExcelExportingController();
         var excelEngine = excelExport.ExportToExcel(this.ResultsGrid, option);
         var workbook = excelEngine.Excel.Workbooks[0];
         var worksheet = workbook.Worksheets[0];
         worksheet.Range["A1"].Text = "Название параметра";
         worksheet.Range["B1"].Text = "Единица измерения";
         worksheet.Range["C1"].Text = "Значение";
         worksheet.Range["A2"].Text = "Ширина канала";
         worksheet.Range["A3"].Text = "Глубина канала";
         worksheet.Range["A4"].Text = "Длина канала";
         worksheet.Range["A5"].Text = "Плотность материала";
         worksheet.Range["A6"].Text = "Удельная теплоемкость";
         worksheet.Range["A7"].Text = "Температура плавления";
         worksheet.Range["A8"].Text = "Скорость крышки";
         worksheet.Range["A9"].Text = "Температура крышки";
         worksheet.Range["A10"].Text = "Коэффициент консистенции материала при температуре приведения";
         worksheet.Range["A11"].Text = "Температурный коэффициент вязкости материала";
         worksheet.Range["A12"].Text = "Температура приведения";
         worksheet.Range["A13"].Text = "Индекс течения материала";
         worksheet.Range["A14"].Text = "Коэффициент теплоотдачи от крышки канала к материалу";
         worksheet.Range["B2"].Text = "м";
         worksheet.Range["B3"].Text = "м";
         worksheet.Range["B4"].Text = "м";
         worksheet.Range["B5"].Text = "кг/м^3";
         worksheet.Range["B6"].Text = "Дж/(кг·°С)";
         worksheet.Range["B7"].Text = "°С";
         worksheet.Range["B8"].Text = "м/с";
         worksheet.Range["B9"].Text = "°С";
         worksheet.Range["B10"].Text = "Па·с^n";
         worksheet.Range["B11"].Text = "1/°С";
         worksheet.Range["B12"].Text = "°С";
         worksheet.Range["B13"].Text = "–";
         worksheet.Range["B14"].Text = "Вт/(м2·°С)";
         worksheet.Range["C2"].Text = obj.Canal.Width.ToString();
         worksheet.Range["C3"].Text = obj.Canal.Height.ToString();
         worksheet.Range["C4"].Text = obj.Canal.Length.ToString();
         worksheet.Range["C5"].Text = obj.Material.Ro.ToString();
         worksheet.Range["C6"].Text = obj.Material.C.ToString();
         worksheet.Range["C7"].Text = obj.Material.T0.ToString();
         worksheet.Range["C8"].Text = obj.Canal.Cap.Vu.ToString();
         worksheet.Range["C9"].Text = obj.Canal.Cap.Tu.ToString();
         worksheet.Range["C10"].Text = obj.Material.Mu0.ToString();
         worksheet.Range["C11"].Text = obj.Material.B.ToString();
         worksheet.Range["C12"].Text = obj.Material.Tr.ToString();
         worksheet.Range["C13"].Text = obj.Material.N.ToString();
         worksheet.Range["C14"].Text = obj.Material.Alpha_u.ToString();

         worksheet.Range["I1"].Text = "Название критериального показателя";
         worksheet.Range["J1"].Text = "Единица измерения";
         worksheet.Range["K1"].Text = "Значение";
         worksheet.Range["I2"].Text = "Производительность";
         worksheet.Range["J2"].Text = "кг/ч";
         worksheet.Range["K2"].Text = PerfomanceLabel.Text;
         worksheet.Range["I3"].Text = "Температура продукта";
         worksheet.Range["J3"].Text = "°C";
         worksheet.Range["K3"].Text = TemperatureLabel.Text;
         worksheet.Range["I4"].Text = "Вязкость продукта";
         worksheet.Range["J4"].Text = "Па*с";
         worksheet.Range["K4"].Text = ViscosityLabel.Text;
         MemoryStream stream = new MemoryStream();
         workbook.SaveAs(stream);
         workbook.Close();
         excelEngine.Dispose();

         DateTime dateTime = DateTime.Now;
         Xamarin.Forms.DependencyService.Get<ISave>().Save($"SimulationResults-{dateTime.Day}-{dateTime.Month}-{dateTime.Year}_{dateTime.Hour}-{dateTime.Minute}.xlsx", "application/msexcel", stream);

         DisplayAlert("Уведомление", "Файл успешно сохранен", "ОK");
        }

      static int GetDecimalDigitsCount(double value)
      {
         string[] str = value.ToString(new System.Globalization.NumberFormatInfo() { NumberDecimalSeparator = "." })
            .Split('.');
         return str.Length == 2 ? str[1].Length : 0;
      }

      public class CanalPoint
      {
         public double x { get; set; }
         public double t { get; set; }
         public double eta { get; set; }
      }

   }
}