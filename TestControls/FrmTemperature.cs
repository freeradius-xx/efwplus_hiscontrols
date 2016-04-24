using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Reflection;
using Temperature.Controls;

namespace TestControls
{
    public partial class FrmTemperature : Form
    {
       
        public FrmTemperature()
        {
            InitializeComponent();


            string jsonvalue = "{\\\"title\\\":{\\\"date\\\":[\\\"2015-5-1\\\",\\\"2015-5-2\\\",\\\"2015-5-3\\\",\\\"2015-5-4\\\",\\\"2015-5-5\\\",\\\"2015-5-6\\\",\\\"2015-5-7\\\"],\\\"day\\\":[\\\"1\\\",\\\"2\\\",\\\"3\\\",\\\"4\\\",\\\"5\\\",\\\"6\\\",\\\"7\\\"],\\\"operday\\\":[\\\"\\\",\\\"\\\",\\\"\\\",\\\"0\\\",\\\"1\\\",\\\"2\\\",\\\"3\\\"]},\\\"lines\\\":[{\\\"datatype\\\":1,\\\"date\\\":[{\\\"index\\\":1,\\\"text\\\":\\\"2015-5-1\\\",\\\"time\\\":[\\\"38.5\\\",\\\"40\\\",\\\"41\\\",\\\"D\\\",\\\"38\\\",\\\"39\\\"]},{\\\"index\\\":2,\\\"text\\\":\\\"2015-5-2\\\",\\\"time\\\":[\\\"38.5\\\",\\\"40\\\",\\\"41\\\",\\\"D\\\",\\\"38\\\",\\\"39\\\"]}]},{\\\"datatype\\\":4,\\\"date\\\":[{\\\"index\\\":1,\\\"text\\\":\\\"2015-5-1\\\",\\\"time\\\":[\\\"100\\\",\\\"105\\\",\\\"120\\\",\\\"99\\\",\\\"80\\\",\\\"100\\\"]},{\\\"index\\\":2,\\\"text\\\":\\\"2015-5-2\\\",\\\"time\\\":[\\\"80\\\",\\\"90\\\",\\\"100\\\",\\\"110\\\",\\\"120\\\",\\\"130\\\"]}]}],\\\"drawtext\\\":{\\\"timetext\\\":[{\\\"type\\\":\\\"入院\\\",\\\"value\\\":\\\"2015-5-1 8:20\\\"},{\\\"type\\\":\\\"手术\\\",\\\"value\\\":\\\"2015-5-2 8:20\\\"}]},\\\"other\\\":{}}";
            string scddata = "[{\"DataId\":\"scd_data\",\"JsonValue\":\""+jsonvalue+"\"}]";

            List<JsonData> list = temperaturebrower.ToList<JsonData>(scddata);
            temperaturebrower1.ShowView(list);
            txtJsonData.Text = JavaScriptConvert.SerializeObject(list);
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            List<JsonData> list = temperaturebrower.ToList<JsonData>(txtJsonData.Text);
            temperaturebrower1.ShowView(list);
        }

        private void btnGetJson_Click(object sender, EventArgs e)
        {
            List<JsonData> list = temperaturebrower1.GetViewData();
            txtJsonData.Text= JavaScriptConvert.SerializeObject(list);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            temperaturebrower1.PrintPreview();
        }


     
    }
}
