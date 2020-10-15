using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(List<DataJson> newDataJson)
        {
            InitializeComponent();

            SeriesCollection seriesViews = new SeriesCollection();

            ChartValues<decimal> price = new ChartValues<decimal>();

            List<string> dates = new List<string>();

            if (newDataJson != null)
            {
                for (int i = 0; i < newDataJson.Count; i++)
                {
                    price.Add(newDataJson[i].price);
                    dates.Add(newDataJson[i].secondDate.Date.ToShortDateString());
                }
                cartesianChart1.AxisX.Clear();
                cartesianChart1.AxisX.Add(new Axis { Labels = dates });

                LineSeries line = new LineSeries();
                line.Title = "Цена";
                line.Values = price;
                seriesViews.Add(line);
                cartesianChart1.Series = seriesViews;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }
    }
}
