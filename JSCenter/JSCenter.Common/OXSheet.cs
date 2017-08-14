using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSCenter.Common
{
    public class OXSheet
    {
        //定义WorksheetPart子元素节点  
        private WorksheetPart worksheetPart = null;
        private SharedStringTable sharedStringTable = null;
        //SheetData：记录非空单元格的位置及内容  
        private SheetData sheetData = null;
        //指向当前行Row  
        private Row currentRow = null;
        //根据ASCII码获取字母A的int编码值  
        private int begin = 'A' - 1;

        #region public
        public OXSheet(WorksheetPart worksheetPart, SharedStringTable sharedStringTable)
        {
            this.worksheetPart = worksheetPart;
            sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();
            currentRow = sheetData.Elements<Row>().LastOrDefault();
            if (currentRow == null)
            {
                AddRow();
            }
            this.sharedStringTable = sharedStringTable;
        }
        /// <summary>
        /// 添加一行  
        /// </summary>
        public void AddRow()
        {
            currentRow = new Row();
            sheetData.AppendChild(currentRow);
        }
        /// <summary>
        /// 在当前行中添加一个的普通表格  
        /// </summary>
        /// <param name="data"></param>
        public void AddCell(object data)
        {
            addCell(data, -1);
        }
        /// <summary>
        /// 在当前行中添加一个指定表格样式的普通表格 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="styleIndex"></param>

        public void AddCell(object data, int styleIndex)
        {
            addCell(data, styleIndex);
        }
        /// <summary>
        /// 在当前行中添加一个类型为共享字符串的表格  
        /// </summary>
        /// <param name="data"></param>
        public void AddSharedStringCell(object data)
        {
            addSharedStringCell(data, -1);
        }
        /// <summary>
        /// 在当前行中添加一个类型为共享字符串、指定表格样式的表格  
        /// </summary>
        /// <param name="data"></param>
        /// <param name="styleIndex"></param>
        public void AddSharedStringCell(object data, int styleIndex)
        {
            addSharedStringCell(data, styleIndex);
        }

        /// <summary>
        /// 在当前行中添加一个超链接表格  
        /// </summary>
        /// <param name="data"></param>
        public void AddHyperLink(object data)
        {
            addHyperlink(data, -1);
        }
        /// <summary>
        /// 在当前行中添加一个超链接表格  
        /// </summary>
        /// <param name="data"></param>
        /// <param name="styleIndex"></param>
        public void AddHyperLink(object data, int styleIndex)
        {
            addHyperlink(data, styleIndex);
        }

        /// <summary>
        /// 指定行和列定位表格，写入数据  
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="data"></param>
        public void Write(int rowIndex, int colIndex, object data)
        {
            Cell cell = getCell(getRow(rowIndex), colIndex);
            cell.DataType = CellValues.SharedString;
            int index = getSharedStringItemIndex(data.ToString());
            cell.CellValue = new CellValue(index.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cloIndex"></param>
        /// <param name="data"></param>
        /// <param name="fillId">填充ID</param>
        public void Write(int rowIndex,int cloIndex,object data,int fillId,int lineHeight)
        {
            Row row = getRow(rowIndex);
            row.CustomHeight = true;
            row.Height = lineHeight;
            Cell cell = getCell(row, cloIndex);
            
            cell.StyleIndex = Convert.ToUInt32(fillId);
            if (data == null)
            {
                return;
            }
            cell.DataType = CellValues.SharedString;
            int index = getSharedStringItemIndex(data.ToString());
            cell.CellValue = new CellValue(index.ToString());
        }
        /// <summary>
        /// 读取指定行和列的内容  
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <returns></returns>
        public string Read(int rowIndex, int colIndex)
        {
            Row row = getRow(rowIndex);
            Cell cell = getCell(row, colIndex);
            string value = string.Empty;
            if (cell.CellValue != null && cell.CellValue.InnerText != null)
            {
                value = cell.CellValue.InnerText;
                if (cell.DataType != null && cell.DataType == CellValues.SharedString)
                {
                    return sharedStringTable.ElementAt(Int32.Parse(value)).InnerText;
                }
                else
                {
                    return value;
                }
            }
            return value;
        }
        /// <summary>
        /// 保存工作表  
        /// </summary>
        public void Save()
        {
            worksheetPart.Worksheet.Save();
        }
        #endregion



        private void addCell(object data, int styleIndex)
        {
            Cell cell = currentRow.AppendChild(new Cell()
            {
                //CellValue:单元格内容  
                CellValue = new CellValue() { Text = data.ToString() },
                //DataType：单元格类型  
                DataType = CellValues.String,
            });
            if (styleIndex != -1)
            {
                cell.StyleIndex = (uint)styleIndex;
            }
        }
        private void addSharedStringCell(object data, int styleIndex)
        {
            Cell cell = currentRow.AppendChild(new Cell()
            {
                CellValue = new CellValue(
                    Convert.ToString(getSharedStringItemIndex(data.ToString()))),
                DataType = CellValues.SharedString,
            });
            if (styleIndex != -1)
            {
                cell.StyleIndex = (uint)styleIndex;
            }
        }
        //获取指定字符串在SharedStringTable中的索引值，不存在就创建  
        private int getSharedStringItemIndex(string value)
        {
            //字符串从0开始标记  
            int index = 0;
            //寻找是否有与value相同的字符串，若有，则将index设置为对应的标记值，并返回  
            //SharedStringItem：共享字符串的数据类型  
            //sharedStringTable：共享字符串表  
            foreach (SharedStringItem item in sharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == value)
                {
                    return index;
                }
                index++;
            }
            //若没有与value相同的字符串，则添加一个字符串到共享字符串表中，并将其内容设置为value  
            sharedStringTable.AppendChild(new SharedStringItem(new Text(value)));
            sharedStringTable.Save();
            return index;
        }

        private void addHyperlink(object data, int styleIndex)
        {
            Cell cell = currentRow.AppendChild(new Cell()
            {
                CellFormula = new CellFormula() { Text = string.Format("HYPERLINK({0}{1}{0},{0}{1}{0})", "\"", data.ToString()) },
                CellValue = new CellValue(data.ToString()),
                DataType = CellValues.String,
            });
            if (styleIndex != -1)
            {
                cell.StyleIndex = (uint)styleIndex;
            }
        }

        //根据行索引值获取列Row  
        private Row getRow(int rowIndex)
        {
            int rowcount = sheetData.Elements<Row>().Count();

            Row row = sheetData.Elements<Row>().FirstOrDefault(r => r != null && r.RowIndex != null && r.RowIndex == rowIndex);
            if (row == null)
            {
                row = new Row() { RowIndex = (uint)rowIndex };
                Row lastRow = sheetData.Elements<Row>().LastOrDefault();
                if ((lastRow != null && lastRow.RowIndex != null && lastRow.RowIndex < rowIndex) || lastRow == null)
                {
                    sheetData.AppendChild(row);
                }
                else
                {
                    Row refRow = sheetData.Elements<Row>().FirstOrDefault(r => r != null && r.RowIndex != null && r.RowIndex > rowIndex);
                    sheetData.InsertBefore(row, refRow);
                }
            }
            return row;
        }
        //根据指定行和列索引获取单元格Cell  
        private Cell getCell(Row row, int colIndex)
        {
            Cell cell = row.Elements<Cell>().FirstOrDefault(c => c != null && c.CellReference != null && c.CellReference.Value == getColNameByColIndex(colIndex) + row.RowIndex);
            if (cell == null)
            {
                cell = new Cell() { CellReference = getColNameByColIndex(colIndex) + row.RowIndex, DataType = CellValues.String };

                Cell lastCell = row.Elements<Cell>().LastOrDefault();
                if ((lastCell != null && getColIndexByCellReference(lastCell.CellReference.Value) < colIndex) || lastCell == null)
                {
                    row.AppendChild(cell);
                }
                else
                {
                    Cell nextCell = row.Elements<Cell>().FirstOrDefault(c => c != null && c.CellReference != null && getColIndexByCellReference(c.CellReference.Value) > colIndex);
                    row.InsertBefore(cell, nextCell);
                }
            }
            return cell;
        }
        //根据第一行中的表格内容，通过与给定的字符串进行匹配获取列的索引值  
        public int GetColIndexByHeaderName(string headerName)
        {
            int index = 1;
            Row row = getRow(1);
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (cell != null && cell.CellValue != null && cell.CellValue.InnerText != null &&
                    cell.CellValue.InnerText == headerName)
                {
                    return index;
                }
                index++;
            }
            row.AppendChild(new Cell() { CellValue = new CellValue() { Text = headerName.ToString() } });
            return index;
        }
        //根据表格的索引值(如A1)获取列的索引值  
        private int getColIndexByCellReference(string cellReference)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[A-Z]{1,}");
            System.Text.RegularExpressions.Match match = regex.Match(cellReference);
            string value = match.Value;
            return getColIndexByColName(value);
        }
        //根据列的名称(如A)获取列的索引值  
        private int getColIndexByColName(string colName)
        {
            int index = 0;
            char[] names = colName.ToCharArray();
            int length = names.Length;
            for (int i = 0; i < length; i++)
            {
                index += (names[i] - begin) * (int)Math.Pow(26, length - i - 1);
            }
            return index;
        }
        //根据列的索引值获取列名(如A)  
        private string getColNameByColIndex(int index)
        {
            string colName = "";
            if (index < 0)
            {
                return colName;
            }
            while (index > 26)
            {
                colName += ((char)(index % 26 + begin)).ToString();
                index = index / 26;
            }
            colName += ((char)(index % 26 + begin)).ToString();
            return colName;
        }

    }
}
