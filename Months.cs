using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApp1
{
    static class Months// класс для вычисления номера месяца по краткому названию
    {
        static string[] month = new string[] { "янв", "фев", "мар", "апр", "май", "июн", "июл", "авг", "сен", "окт", "ноя", "дек"};
        public static int GetNumbMonth(string s)
        {
            int rezult = 0 ;
            for (int i = 0; i < month.Length; i++) 
            {
                if (String.Compare(s, month[i]) == 0)
                {
                    rezult = i + 1;
                    break;
                }
            }
            return rezult;
        }
    }
}
