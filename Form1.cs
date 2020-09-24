using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<DataJson>));
        public Form1()
        {
            InitializeComponent();

            WebClient wc = new WebClient(); // класс для загрузки файлов
            try
            {
                wc.DownloadFile("https://data.gov.ru/opendata/7710349494-urals/data-20200817T1010-structure-20200817T1010.csv?encoding=UTF-8", "data.txt");// загружаем и сохраняем файл в корень программы "data.csv" 

                string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\data.txt";// сохраняем путь загруженного файла

                StreamReader streamReader = new StreamReader(directory.ToString());// объект класса для работы с загруженным файлом

                List<DataJson> dataJson = new List<DataJson>(); // массив в котором будут храниться данные

                json = new DataContractJsonSerializer(typeof(List<DataJson>)); // объект класса сериализации

                while (!streamReader.EndOfStream)//перебираем данные у загруженного файла (data.txt) построчно
                {

                    MatchCollection regex = Regex.Matches(streamReader.ReadLine(), @"(\d+.\w+.\d+)|(\d+)"); // шаблон сортировки
                    try// добавление данных
                    {
                        MatchCollection dayAndYear = Regex.Matches(regex[0].Value, @"(\d+)"); // шаблон для сортировки дня и года
                        Match month = Regex.Match(regex[0].Value, @".\d+.(\w+).+"); // шаблон сортировки названия месяца
                        int numbMonth = Months.GetNumbMonth(month.Groups[1].Value); // получаем номер месяца по названию
                        DateTime firstDate = new DateTime(2000 + Convert.ToInt32(dayAndYear[1].Value), numbMonth, Convert.ToInt32(dayAndYear[0].Value));// дата  начала периода мониторинга цен на нефть

                        dayAndYear = Regex.Matches(regex[1].Value, @"(\d+)"); // шаблон для сортировки дня и года
                        month = Regex.Match(regex[1].Value, @".\d+.(\w+).+"); // шаблон сортировки названия месяца
                        numbMonth = Months.GetNumbMonth(month.Groups[1].Value); // получаем номер месяца по названию
                        DateTime secondDate = new DateTime(2000 + Convert.ToInt32(dayAndYear[1].Value), numbMonth, Convert.ToInt32(dayAndYear[0].Value));// дата  окончания периода мониторинга цен на нефть

                        decimal price = Convert.ToDecimal(regex[2].Value); //получаем цену на нефть
                        dataJson.Add(new DataJson { firstDate = firstDate, secondDate = secondDate, price = price });// сохраняет экземпляр в коллекцию

                    }
                    catch (Exception ex) when (ex is System.ArgumentOutOfRangeException)
                    {
                        //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                using (var file = new FileStream("datajson.json", FileMode.OpenOrCreate))// создаем или открывает файл datajson.json
                {
                    json.WriteObject(file, dataJson);// сериализация
                }
            }

            catch (Exception ex) when (ex is System.Net.WebException)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Process.GetCurrentProcess().Kill();
            }


        }
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)// событие кнопки расчета цены на заданную дату
        {
            DateTime time = dateTimePicker3.Value;
            List<DataJson> newDataJson = Class1.GetDataJsons(json);
            if (newDataJson != null)
            {
                for (int i = 0; i < newDataJson.Count; i++)
                {
                    if (time >= newDataJson[i].firstDate && time <= newDataJson[i].secondDate) //выделяем промежуток дат
                    {
                        label3.Text = newDataJson[i].price.ToString();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)// событие кнопки расчета средней цены на диапазон
        {
            DateTime timeStart = dateTimePicker1.Value;
            DateTime timeFinish = dateTimePicker2.Value;
            List<DataJson> newDataJson = Class1.GetDataJsons(json);
            int start = 0;
            int finish = 0;
            decimal rezult = 0;
            if (newDataJson != null)
            {
                for (int i = 0; i < newDataJson.Count; i++)
                {
                    if (timeStart >= newDataJson[i].firstDate && timeStart <= newDataJson[i].secondDate)//выделяем диапазон дат
                    {
                        start = i;// начало диапазона
                    }
                    if (timeFinish >= newDataJson[i].firstDate && timeFinish <= newDataJson[i].secondDate)
                    {
                        finish = i; // конец дипазона
                    }
                }
                if (start <= finish)
                {
                    for (int i = start; i <= finish; i++)
                    {
                        rezult += newDataJson[i].price / (finish + 1 - start);// высчитываем средднюю цену
                    }
                    label4.Text = Math.Round(rezult, 2).ToString();
                }
                else if (start > finish)
                {
                    MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)// событие кнопки вывода файла json max min
        {
            DateTime timeStart = dateTimePicker4.Value;
            DateTime timeFinish = dateTimePicker5.Value;
            List<DataJson> newDataJson = Class1.GetDataJsons(json);
            int start = 0;
            int finish = 0;
            decimal[] rezult;
            if (newDataJson != null)
            {
                for (int i = 0; i < newDataJson.Count; i++)
                {
                    if (timeStart >= newDataJson[i].firstDate && timeStart <= newDataJson[i].secondDate)//выделяем диапазон дат
                    {
                        start = i;// начало диапазона
                    }
                    if (timeFinish >= newDataJson[i].firstDate && timeFinish <= newDataJson[i].secondDate)
                    {
                        finish = i; // конец дипазона
                    }
                }
                if (start <= finish)
                {
                    int j = 0;
                    rezult = new decimal[finish + 1 - start];// массив для хранения price в диапазоне указанного выше
                    for (int i = start; i <= finish; i++)
                    {
                        rezult[j] = newDataJson[i].price;// записываем price в массив
                        j++;
                    }

                    using (var file = new FileStream("jsonmaxmin.json", FileMode.OpenOrCreate))// создаем или открывает файл jsonmaxmin.json"
                    {
                        MinMax minMax = new MinMax// экземпляр класса для min и max значения
                        {
                            min = rezult.Min(),
                            max = rezult.Max()
                        };
                        string c = JsonConvert.SerializeObject(minMax);// сериализуем данные в json строку
                        byte[] array = System.Text.Encoding.Default.GetBytes(c);// последовательность байтов для указанной выше строки json 
                        file.Write(array, 0, array.Length);// записываем данные в файл

                        if (file != null)
                        {
                            Process.Start("notepad.exe", "jsonmaxmin.json");// открываем полученный файл
                        }
                    }
                }
                else if (start > finish)
                {
                    MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)// событие для отображение json всего записей
        {
            List<DataJson> newDataJson = Class1.GetDataJsons(json);

            using (var file = new FileStream("jsonallrecord.json", FileMode.OpenOrCreate))// создаем или открывает файл jsonmaxmin.json"
            {
                AllRecords allRecords = new AllRecords { allrecords = newDataJson.Count };
                string c = JsonConvert.SerializeObject(allRecords);// сериализуем данные в json строку
                byte[] array = System.Text.Encoding.Default.GetBytes(c);// последовательность байтов для указанной выше строки json 
                file.Write(array, 0, array.Length);// записываем данные в файл

                if (file != null)
                {
                    Process.Start("notepad.exe", "jsonallrecord.json");// открываем полученный файл
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<DataJson> newDataJson = Class1.GetDataJsons(json);
            Form2 form2 = new Form2(newDataJson);
            form2.Show();
        }// кнопка построения графика
    }
}
