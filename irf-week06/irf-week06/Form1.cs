﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using irf_week06.Entities;
using irf_week06.MnbServiceReference;

namespace irf_week06
{
    public partial class Form1 : Form
    {
        private BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<String> currencies;
        ComboBox box;
        DateTimePicker startDate;
        DateTimePicker endDate;


        public Form1()
        {
            InitializeComponent();
            currencies = getCurrencies();
            startDatePicker.Value = DateTime.Today.AddDays(-30);
            endDate.Value = DateTime.Now;
            RefreshData();
        }

        private void RefreshData()
        {
            box = currencyBox;
            startDate = startDatePicker;
            endDate = endDatePicker;
            Rates.Clear();
            var result = getSoapClient(currencyBox.SelectedItem.ToString(), startDatePicker.Value.ToString(), endDatePicker.Value.ToString());
            DataGridView view = new DataGridView();
            view.DataSource = Rates;
            currencyBox.DataSource = currencies;
            parseXML(result);
            showDataOnChart();
        }

        private BindingList<String> getCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = currency,
                startDate = startDate,
                endDate = endDate
            };

            // Ebben az esetben a "var" a GetExchangeRates visszatérési értékéből kapja a típusát.
            // Ezért a response változó valójában GetExchangeRatesResponseBody típusú.
            var response = mnbService.GetExchangeRates(request);

            // Ebben az esetben a "var" a GetExchangeRatesResult property alapján kapja a típusát.
            // Ezért a result változó valójában string típusú.
            var result = response.GetExchangeRatesResult;
        }

        private void showDataOnChart()
        {
            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void parseXML(String result)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            // Végigmegünk a dokumentum fő elemének gyermekein
            foreach (XmlElement element in xml.DocumentElement)
            {
                // Létrehozzuk az adatsort és rögtön hozzáadjuk a listához
                // Mivel ez egy referencia típusú változó, megtehetjük, hogy előbb adjuk a listához és csak később töltjük fel a tulajdonságait
                var rate = new RateData();

                // Dátum
                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                // Valuta
                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                // Érték
                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
                Rates.Add(rate);
            }
        }

        private static string getSoapClient(String currency, String startDate, String endDate)
        {
            // A változó deklarációk jobb oldalán a "var" egy dinamikus változó típus.
            // A "var" változó az első értékadás pillanatában a kapott érték típusát veszi fel, és később nem változtatható.
            // Jelen példa első sora tehát ekvivalens azzal, ha a "var" helyélre a MNBArfolyamServiceSoapClient-t írjuk.
            // Ebben a formában azonban olvashatóbb a kód, és változtatás esetén elég egy helyen átírni az osztály típusát.
            var mnbService = new MNBArfolyamServiceSoapClient();
           
            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = currency,
                startDate = startDate,
                endDate = endDate
            };

            // Ebben az esetben a "var" a GetExchangeRates visszatérési értékéből kapja a típusát.
            // Ezért a response változó valójában GetExchangeRatesResponseBody típusú.
            var response = mnbService.GetExchangeRates(request);

            // Ebben az esetben a "var" a GetExchangeRatesResult property alapján kapja a típusát.
            // Ezért a result változó valójában string típusú.
            var result = response.GetExchangeRatesResult;
            return result;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
