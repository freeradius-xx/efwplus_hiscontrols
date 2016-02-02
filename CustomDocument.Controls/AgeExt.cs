using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatHIS.Controls.CommonControl
{
    public class AgeValue
    {
        public int Y_num;
        public int M_num;
        public int D_num;
        public int H_num;

        public AgeValue()
        {
        }

        public AgeValue(string age_str)
        {
            string[] ages = age_str.Split(new string[] { "岁", "月", "天", "时" }, StringSplitOptions.RemoveEmptyEntries);
            if (ages.Length == 4)
            {
                Y_num = Convert.ToInt32(ages[0].Trim() == "" ? "0" : ages[0].Trim());
                M_num = Convert.ToInt32(ages[1].Trim() == "" ? "0" : ages[1].Trim());
                D_num = Convert.ToInt32(ages[2].Trim() == "" ? "0" : ages[2].Trim());
                H_num = Convert.ToInt32(ages[3].Trim() == "" ? "0" : ages[3].Trim());
            }
        }

        public string ReturnAgeStr()
        {
            return Y_num.ToString().PadLeft(3, '0') + "岁" + M_num.ToString().PadLeft(2, '0') + "月" + D_num.ToString().PadLeft(2, '0') + "天" + H_num.ToString().PadLeft(2, '0') + "时";
        }

        public string ReturnAgeStr_EN()
        {
            if (Y_num > 0)
                return "Y" + Y_num;
            else if (M_num > 0)
                return "M" + M_num;
            else if (D_num > 0)
                return "D" + D_num;
            else if (H_num > 0)
                return "H" + H_num;
            else
                return "D1";
        }
    }

    /// <summary>
    /// 定义年龄结构
    /// </summary>
    public class AgeExtend
    {
        /// <summary>
        /// 将年龄转换为出生日期
        /// </summary>
        /// <param name="age">年龄</param>
        /// <returns>出生日期</returns>
        public static DateTime GetDateTime(AgeValue age)
        {
            System.DateTime date = DateTime.Now;

            date = date.AddYears((-1) * age.Y_num);

            date = date.AddMonths((-1) * age.M_num);

            date = date.AddDays((-1) * age.D_num);

            date = date.AddHours((-1) * age.H_num);

            return date;
        }
        /// <summary>
        /// 将出生日期转换为年龄
        /// </summary>
        /// <param name="birthday">出生日期</param>
        /// <returns>年龄</returns>
        public static AgeValue GetAgeValue(DateTime birthday)
        {
            AgeValue age = new AgeValue();
            System.DateTime current = DateTime.Now;

            int _year = current.Year - birthday.Year;
            int _month = current.Month - birthday.Month;
            int _day = current.Day - birthday.Day;
            int _hour = current.Hour - birthday.Hour;
            int _minute = current.Minute - birthday.Minute;

            if (_minute < 0)
            {
                _hour = _hour - 1;
                _minute = _minute + 60;
            }
            if (_hour < 0)
            {
                _day = _day - 1;
                _hour = _hour + 24;
            }
            if (_day < 0)
            {
                _month = _month - 1;
                _day = _day + DateTime.DaysInMonth(birthday.Year, birthday.Month);
            }
            if (_month < 0)
            {
                _year = _year - 1;
                _month = _month + 12;
            }

            age.Y_num = _year;
            age.M_num = _month;
            age.D_num = _day;
            age.H_num = _hour;

            if (_year >= 15)
            {
                age.M_num = 0;
                age.D_num = 0;
                age.H_num = 0;
            }
            else if (_year >= 1)
            {
                age.D_num = 0;
                age.H_num = 0;
            }
            else if (_month >= 1)
            {
                age.H_num = 0;
            }
            #region old
            /*
            if (birthday.Year != current.Year)
            {
                if (birthday.Month > current.Month && current.Year - birthday.Year == 1)
                {
                    age.Y_num = 0;
                    age.M_num = current.Month + 12 - birthday.Month;
                    age.D_num = 0;
                    age.H_num = 0;
                }
                else
                {
                    age.Y_num =current.Year - birthday.Year;
                    age.M_num = 0;
                    age.D_num = 0;
                    age.H_num = 0;
                }
            }
            else if (birthday.Month != current.Month)
            {
                if (current.Month - birthday.Month == 1 && current.Day < birthday.Day)
                {
                    age.Y_num = 0;
                    age.M_num = 0;
                    age.D_num = current.Day + 30 - birthday.Day;
                    age.H_num = 0;
                }
                else
                {
                    age.Y_num = 0;
                    age.M_num = current.Month - birthday.Month;
                    age.D_num = 0;
                    age.H_num = 0;
                }
            }
            else if (birthday.Day != current.Day)
            {
                if (current.Day - birthday.Day == 1 && current.Hour < birthday.Hour)
                {
                    age.Y_num = 0;
                    age.M_num = 0;
                    age.D_num = 0;
                    age.H_num = current.Hour - birthday.Hour + 24;
                }
                else
                {
                    age.Y_num = 0;
                    age.M_num = 0;
                    age.D_num = current.Day - birthday.Day;
                    age.H_num = 0;
                }
            }
            else if (birthday.Hour != current.Hour)
            {
                age.Y_num = 0;
                age.M_num = 0;
                age.D_num = 0;
                age.H_num = current.Hour - birthday.Hour;
            }*/
            #endregion
            return age;
        }
    }
}
