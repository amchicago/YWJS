using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using JSCenter.Common;
using System;
using System.Collections.Generic;
using System.Linq;


namespace JSCenter.Win
{
    public  class ExcelManager
    {

        /// <summary>
        /// 导出excle
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="exclePath">eg.C:\\form</param>
        public static void WriteExcel(string projectID,string exclePath)
        {
            var datalist = DAL.CommonDAL.GetProjectItemList(projectID);
            string path = AppDomain.CurrentDomain.BaseDirectory + "center.xlsx";

            OXExcel doce = new OXExcel(path);
            doce.LoadExcel();

            #region 初始化背景填充色
            int titlecl = doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(49,132,155));
            int rowcl = doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(197, 217, 241));
            int fucecl= doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(141, 180, 227));

            int titleFC = doce.GetCellFormatsIndex(System.Drawing.Color.FromArgb(49, 132, 155), "宋体", 21);
            #endregion

            var worksheetpart = doce.GetWorksheetPart("Sheet1");

            #region 设置列宽
            Columns coluus = worksheetpart.Worksheet.Descendants<Columns>().FirstOrDefault();

            if (coluus==null)
            {
                coluus = new Columns();
                for(int i = 0; i < 10; i++)
                {
                    coluus.AppendChild(new Column() { Min = 1U, Max = 1U, Width = 15, CustomWidth = true });
                }
                worksheetpart.Worksheet.InsertAfter(coluus, worksheetpart.Worksheet.SheetFormatProperties);
            }

            foreach (Column item in coluus)
            {
                item.Width = 15;
                item.CustomWidth = true;
            }
            #endregion

            var sharetable =doce.GetSharedStringTable();
            OXSheet sheet = new OXSheet(worksheetpart, sharetable);

            string titleName = "药物名称";
            int rowIndex = 1;
            for (int i = 1; i < 3; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (i == 2 && j == 5)
                    {
                        sheet.Write(i, j, titleName, titleFC, 31);
                    }
                    else
                    {
                        sheet.Write(i, j, "", titlecl, 31);
                    }
                }
                rowIndex = i;
            }
            //写入数据
            rowIndex += 1;
            //表头
            Dictionary<int, string> ziduan = new Dictionary<int, string>();

            ziduan.Add(1, "编号");
            ziduan.Add(2, "平均供试峰面积");
            ziduan.Add(3, "供试称样量");
            ziduan.Add(4, "对照浓度mg/ml");
            ziduan.Add(5, "对照峰面积");
            ziduan.Add(6, "含量%");
            ziduan.Add(7, "平均含量");
            ziduan.Add(8, "方差");
            ziduan.Add(9, "编号");

            sheet.Write(rowIndex, 1, "", rowcl, 14);
            foreach ( var item  in ziduan)
            {
                sheet.Write(rowIndex, item.Key+1, item.Value, rowcl, 14);
            }
           

            foreach ( var item in list)
            {
                rowIndex += 1;

            }
          
            sheet.Save();
            doce.Dispose();
        }
    }
}
