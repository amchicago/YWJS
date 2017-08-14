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
            var projectModel = DAL.CommonDAL.GetProject(projectID);
            string path = AppDomain.CurrentDomain.BaseDirectory + "center.xlsx";

            OXExcel doce = new OXExcel(path);
            doce.LoadExcel();

            #region 初始化背景填充色
            int titlecl = doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(49,132,155));
            int rowcl = doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(197, 217, 241));
            int fucecl= doce.GetStyleIndexByBackColor(System.Drawing.Color.FromArgb(141, 180, 227));
            int titleFC = doce.GetCellFormatsIndex(System.Drawing.Color.FromArgb(49, 132, 155), "宋体", 21);
            int titleFC2= doce.GetCellFormatsIndex(System.Drawing.Color.FromArgb(141, 180, 227), "宋体", 21);

            int RedColore= doce.GetStyleIndexByBackColor(System.Drawing.Color.Red);
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

            #region 饮片数据
            string titleName = projectModel.DurgName;
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
            ziduan.Add(4, "稀释倍数");
            ziduan.Add(5, "对照浓度mg/ml");
            ziduan.Add(6, "对照峰面积");
            ziduan.Add(7, "含量%");
            ziduan.Add(8, "平均含量");
            ziduan.Add(9, "方差");

            sheet.Write(rowIndex, 1, "", rowcl, 14);
            foreach (var item in ziduan)
            {
                sheet.Write(rowIndex, item.Key + 1, item.Value, rowcl, 14);
            }

            foreach (var item in datalist.Where(s=>s.IsFuCe=="False"&&s.type.Trim()==Model.DrugType.饮片.ToString()))
            {
                rowIndex += 1;
                sheet.Write(rowIndex, 1, string.Empty, rowcl, 14);
                sheet.Write(rowIndex, 2, item.CodeNum, rowcl, 14);
                sheet.Write(rowIndex, 3, item.PJSFMJ, rowcl, 14);
                sheet.Write(rowIndex, 4, item.GSCYL, rowcl, 14);
                sheet.Write(rowIndex, 5, item.XSBS, rowcl, 14);
                sheet.Write(rowIndex, 6, item.DZLD, rowcl, 14);
                sheet.Write(rowIndex, 7, item.DZFMJ, rowcl, 14);
                sheet.Write(rowIndex, 8, item.HL, rowcl, 14);
                sheet.Write(rowIndex, 9, item.PJHL, rowcl, 14);
                sheet.Write(rowIndex, 10, item.FC, rowcl, 14);
            }

            //复测
            foreach (var item in datalist.Where(s => s.IsFuCe == "True" && s.type.Trim() == Model.DrugType.饮片.ToString()))
            {
                rowIndex += 1;
                sheet.Write(rowIndex, 1, "复测", RedColore, 14);
                sheet.Write(rowIndex, 2, item.CodeNum, rowcl, 14);
                sheet.Write(rowIndex, 3, item.PJSFMJ, rowcl, 14);
                sheet.Write(rowIndex, 4, item.GSCYL, rowcl, 14);
                sheet.Write(rowIndex, 5, item.XSBS, rowcl, 14);
                sheet.Write(rowIndex, 6, item.DZLD, rowcl, 14);
                sheet.Write(rowIndex, 7, item.DZFMJ, rowcl, 14);
                sheet.Write(rowIndex, 8, item.HL, rowcl, 14);
                sheet.Write(rowIndex, 9, item.PJHL, rowcl, 14);
                sheet.Write(rowIndex, 10, item.FC, rowcl, 14);
            }
            #endregion

            #region 汤剂

            int tjMax = rowIndex + 2;
            for (int i = rowIndex; i < tjMax; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    if (i == tjMax-1 && j == 5)
                    {
                        sheet.Write(i, j, titleName, titleFC2, 31);
                    }
                    else
                    {
                        sheet.Write(i, j, "", fucecl, 31);
                    }
                }
            }
            rowIndex += 2;
            sheet.Write(rowIndex, 1, "", rowcl, 14);
            foreach (var item in ziduan)
            {
                sheet.Write(rowIndex, item.Key + 1, item.Value, rowcl, 14);
            }

            foreach (var item in datalist.Where(s => s.IsFuCe == "False" && s.type.Trim() == Model.DrugType.汤剂.ToString()))
            {
                rowIndex += 1;
                sheet.Write(rowIndex, 1, string.Empty, rowcl, 14);
                sheet.Write(rowIndex, 2, item.CodeNum, rowcl, 14);
                sheet.Write(rowIndex, 3, item.PJSFMJ, rowcl, 14);
                sheet.Write(rowIndex, 4, item.GSCYL, rowcl, 14);
                sheet.Write(rowIndex, 5, item.XSBS, rowcl, 14);
                sheet.Write(rowIndex, 6, item.DZLD, rowcl, 14);
                sheet.Write(rowIndex, 7, item.DZFMJ, rowcl, 14);
                sheet.Write(rowIndex, 8, item.HL, rowcl, 14);
                sheet.Write(rowIndex, 9, item.PJHL, rowcl, 14);
                sheet.Write(rowIndex, 10, item.FC, rowcl, 14);
            }

            //复测
            foreach (var item in datalist.Where(s => s.IsFuCe == "True" && s.type.Trim() == Model.DrugType.汤剂.ToString()))
            {
                rowIndex += 1;
                sheet.Write(rowIndex, 1, "复测", RedColore, 14);
                sheet.Write(rowIndex, 2, item.CodeNum, rowcl, 14);
                sheet.Write(rowIndex, 3, item.PJSFMJ, rowcl, 14);
                sheet.Write(rowIndex, 4, item.GSCYL, rowcl, 14);
                sheet.Write(rowIndex, 5, item.XSBS, rowcl, 14);
                sheet.Write(rowIndex, 6, item.DZLD, rowcl, 14);
                sheet.Write(rowIndex, 7, item.DZFMJ, rowcl, 14);
                sheet.Write(rowIndex, 8, item.HL, rowcl, 14);
                sheet.Write(rowIndex, 9, item.PJHL, rowcl, 14);
                sheet.Write(rowIndex, 10, item.FC, rowcl, 14);
            }
            #endregion

            #region 统计信息


            rowIndex += 5;//分割5行


            Dictionary<int, string> tongjiziduan = new Dictionary<int, string>();

            tongjiziduan.Add(1, "编号");
            tongjiziduan.Add(2, string.Format("饮片{0}含量%",titleName));
            tongjiziduan.Add(3, string.Format("汤剂{0}含量%", titleName));
            tongjiziduan.Add(4, "含量转移率");
             

            sheet.Write(rowIndex, 1, "", rowcl, 14);
            foreach (var item in tongjiziduan)
            {
                sheet.Write(rowIndex, item.Key + 1, item.Value);
            }
            var tongjilist = DAL.CommonDAL.GetTongjiList(projectID);
            foreach (var item in tongjilist)
            {
                rowIndex += 1;
                sheet.Write(rowIndex, 1, string.Empty);
                sheet.Write(rowIndex, 2, item.GroupName);
                sheet.Write(rowIndex, 3, item.YPHL);
                sheet.Write(rowIndex, 4, item.TJHL);
                sheet.Write(rowIndex, 5, item.ZYLv);
            }
            #endregion

            sheet.Save();
            doce.Dispose();
        }
    }
}
