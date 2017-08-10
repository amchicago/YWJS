using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace JSCenter.Common
{
    public class OXExcel
    {
        //判断文件是否只读的相关定义  
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static IntPtr HFILE_ERROR = new IntPtr(-1);
        //OpenXml中涉及Excel的元素，定义全局变量，方便长时间读写Excel文档  
        //SpreadsheetDocument：Excel文档的变量类型  
        private SpreadsheetDocument spreadsheetDocument = null;
        //WorkbookPart：Excel文档的总管  
        private WorkbookPart workbookPart = null;
        //表格格式总管  
        private Stylesheet stylesheet = null;
        //文件路径+名字  
        private string path;
        //程序运行时，打开通过文件流载入的Excel文件会提示只读  
        private FileStream fileStream;
        //指定Excel的路径（包含名称）  
        public OXExcel(string path)
        {
            this.path = path;
        }
        //载入Excel  
        public bool LoadExcel()
        {
            //Excel是否存在  
            if (System.IO.File.Exists(path))
            {
                //Excel是否只读  
                if (!IsReadOnly())
                {
                    //使用文件流载入Excel，使得程序运行时，使用本程序外的软件打开Excel时，将会提示Excel只读，不能编辑。  
                    //可以防止在程序外部打开Excel，导致本程序失去对Excel的载入，从而程序崩溃、异常。  
                    fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                    //OpenSettings：Excel打开后的设置  
                    OpenSettings openSettings = new OpenSettings();
                    //设置Excel不自动保存  
                    openSettings.AutoSave = false;
                    spreadsheetDocument = SpreadsheetDocument.Open(fileStream, true, openSettings);
                    workbookPart = spreadsheetDocument.WorkbookPart;
                    //下列子元素若不存在，则需要重新创建Excel  
                    if (workbookPart == null || workbookPart.Workbook == null || workbookPart.Workbook.Sheets == null ||
                        workbookPart.SharedStringTablePart == null || workbookPart.SharedStringTablePart.SharedStringTable == null ||
                        workbookPart.WorkbookStylesPart == null || workbookPart.WorkbookStylesPart.Stylesheet == null)
                    {
                        createExcel();
                    }
                    stylesheet = workbookPart.WorkbookStylesPart.Stylesheet;
                }
                else
                {
                    //只读  
                    return false;
                }
            }
            else
            {
                createExcel();
            }
            return true;
        }
        //判断指定路径的Excel是否只读  
        private bool IsReadOnly()
        {
            IntPtr vHandle = _lopen(path, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                CloseHandle(vHandle);
                return true;
            }
            CloseHandle(vHandle);
            return false;
        }
        //创建Excel  
        private void createExcel()
        {
            //SpreadsheetDocument  
            if (spreadsheetDocument == null)
            {
                //以path中所指定的地址和名称创建一个Excel文档  
                //SpreadsheetDocumentType.Workbook：指明创建的文件类型为后缀名为.xlsx的Excel文档  
                spreadsheetDocument = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            }
            //WorkbookPart  
            workbookPart = spreadsheetDocument.WorkbookPart;
            if (workbookPart == null)
            {
                workbookPart = spreadsheetDocument.AddWorkbookPart();
            }
            //Workbook  
            if (workbookPart.Workbook == null)
            {
                workbookPart.Workbook = new Workbook();
            }
            //Sheets  
            if (workbookPart.Workbook.Sheets == null)
            {
                workbookPart.Workbook.Sheets = new Sheets();
            }
            //SharedStringTablePart  
            if (workbookPart.SharedStringTablePart == null)
            {
                workbookPart.AddNewPart<SharedStringTablePart>();
            }
            //SharedStringTabel  
            if (workbookPart.SharedStringTablePart.SharedStringTable == null)
            {
                workbookPart.SharedStringTablePart.SharedStringTable = new SharedStringTable();
            }
            workbookPart.SharedStringTablePart.SharedStringTable.Save();
            //WorkbookStylesPart  
            if (workbookPart.WorkbookStylesPart == null)
            {
                workbookPart.AddNewPart<WorkbookStylesPart>();
            }
            //Stylesheet  
            if (workbookPart.WorkbookStylesPart.Stylesheet == null)
            {
                workbookPart.WorkbookStylesPart.Stylesheet = new Stylesheet();
            }
            workbookPart.Workbook.Save();
            //经过我的大量实践，在创建Workbook和Worksheet后，均需调用spreadsheetDocument.Dispose()方法，否则Excel文件会有问题  
            //但是Dispose()后，所有资源均释放了，所以需要重新载入Excel  
            //也许有人会想，为什么不在所有关于Excel的操作(包括创建、读写)完成后，再调用Dispose()？  
            //我的回答是：程序总会出现异常，异常时，并不会自动调用Dispose  
            //请原谅我的强迫症，1、为了能在程序运行时，保持对Excel的载入。2、遇到程序异常时，Excel文件不会出现任何问题。  
            reLoad();
        }
        //释放定义的全局变量等所有资源，并重新载入Excel  
        private void reLoad()
        {
            Dispose();
            LoadExcel();
        }
        //判断Worksheet是否存在  
        public bool IsWorksheetExist(string sheetName)
        {
            //workbookPart：管理Workbook工作薄  
            //workbookPart.Workbook.Sheets：Workbook元素中的Sheets元素  
            //Sheets.Elements<Sheet>()：Sheets元素中Sheet元素的集合  
            //Elements<Sheet>().FirstOrDefault()：Sheet元素集合中第一个Sheet元素或默认的Sheet元素  
            //FirstOrDefault(s => s != null && s.Name != null && s.Name == sheetName)等同于如下代码  
            //foreach(Sheet s in workbookPart.Workbook.Sheets.Elements<Sheet>())  
            //{  
            //    寻找Name与sheetName相同的Sheet元素  
            //    if(s! = null && s.Name != null && s.Name == sheetName)  
            //    {  
            //       sheet = s;  
            //       break;  
            //    }  
            //}  
            //Sheet是工作表在宏观上的元素，它存储工作表的标识和名称  
            Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().
                FirstOrDefault(s => s != null && s.Name != null && s.Name == sheetName);
            if (sheet == null)
            {
                return false;
            }
            //WorksheetPart：管理工作表，一个WorksheetPart对应一个工作表  
            //Worksheet是工作表在微观上的元素，它主要存储工作表中非空表格的内容  
            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            //Worksheet,SheetData  
            if (worksheetPart.Worksheet == null || worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault() == null)
            {
                return false;
            }
            return true;
        }
        //添加一个工作表  
        public void AddWorksheetPart(string sheetName)
        {
            Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().
                FirstOrDefault(s => s != null && s.Name != null && s.Name == sheetName);
            WorksheetPart worksheetPart = null;
            if (sheet != null)
            {
                worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            }
            else
            {
                worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            }
            //Worksheet  
            if (worksheetPart.Worksheet == null)
            {
                worksheetPart.Worksheet = new Worksheet();
            }
            //SheetData  
            if (worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault() == null)
            {
                //workbookPart.AddNewPart<WorksheetPart>()：添加一个新的WorksheetPart  
                worksheetPart.Worksheet.AppendChild(new SheetData());
            }
            worksheetPart.Worksheet.Save();
            //Sheet  
            if (sheet == null)
            {
                sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().LastOrDefault();
                //定义默认的sheetId值，当工作薄中一个工作表也没有时，便以1作为新添加的Sheet元素的Id值  
                UInt32Value sheetId = 1;
                if (sheet != null)
                {
                    sheetId = sheet.SheetId + 1;
                }
                //AppendChild：添加一个子元素  
                workbookPart.Workbook.Sheets.AppendChild(new Sheet()
                {
                    //Id为字符串类型的Id属性  
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    //SheetId为数值类型的Id属性  
                    SheetId = sheetId,
                    //工作表的名称  
                    Name = sheetName
                });
            }
            workbookPart.Workbook.Save();
            //此处解释同上方CreateExcel()方法中一致  
            reLoad();
        }
        //获取工作表  
        public WorksheetPart GetWorksheetPart(string sheetName)
        {
            //WorksheetPart  
            Sheet sheet = workbookPart.Workbook.Sheets.Elements<Sheet>().FirstOrDefault(s => s != null && s.Name != null && s.Name == sheetName);
            
            WorksheetPart worksheetPart = null;
            if (sheet != null)
            {
                worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
            }
            else
            {
                return null;
            }
            //Worksheet,SheetData  
            if (worksheetPart.Worksheet == null || worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault() == null)
            {
                return null;
            }
            return worksheetPart;
        }
        //获取共享字符串表  
        public SharedStringTable GetSharedStringTable()
        {
            return workbookPart.SharedStringTablePart.SharedStringTable;
        }
        //根据背景颜色获取自定义的表格样式的索引，不存在则创建  
        public int GetStyleIndexByBackColor(System.Drawing.Color color)
        {
            checkStylesheet();
            int fillIndex = getFillIndex(color);
            int index = 0;
            foreach (CellFormat cellFormat in stylesheet.CellFormats.Elements<CellFormat>())
            {
                if (cellFormat != null && 
                    cellFormat.FillId != null &&
                    cellFormat.FillId == fillIndex &&
                    cellFormat.FontId == null && 
                    cellFormat.BorderId == null)
                {
                    return index;
                }
                index++;
            }
            stylesheet.CellFormats.AppendChild(new CellFormat()
            {
                FillId = (uint)fillIndex,
            });
            stylesheet.Save();
            return index;
        }
        /// <summary>
        /// 获取颜色和字体格式，不存在就创建
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public int GetCellFormatsIndex(System.Drawing.Color color,string fontName,uint fontSize)
        {
            checkStylesheet();
            int fillIndex = getFillIndex(color);
            int fontIndex = getFontIndex(fontName, fontSize);
            int index = 0;
            foreach (CellFormat cellFormat in stylesheet.CellFormats.Elements<CellFormat>())
            {
                if (cellFormat != null &&
                    cellFormat.FillId != null &&
                    cellFormat.FillId == fillIndex &&
                     cellFormat.FontId != null &&
                    cellFormat.FontId == fontIndex &&
                    cellFormat.BorderId == null)
                {
                    return index;
                }
                index++;
            }
            stylesheet.CellFormats.AppendChild(new CellFormat()
            {
                FillId = (uint)fillIndex,
                FontId=(uint)fontIndex
            });
            stylesheet.Save();
            return index;

        }
        //检验Stylesheet的完整性  
        private void checkStylesheet()
        {
            if (stylesheet.Fonts == null)
            {
                stylesheet.Fonts = new Fonts();
            }
            if (stylesheet.Fonts.ChildElements.Count == 0)
            {
                stylesheet.Fonts.AppendChild(new Font());
            }
            if (stylesheet.Fills == null)
            {
                stylesheet.Fills = new Fills();
            }
            if (stylesheet.Fills.ChildElements.Count == 0)
            {
                stylesheet.Fills.AppendChild(new Fill()
                {
                    PatternFill = new PatternFill()
                    {
                        PatternType = PatternValues.None
                    }
                });
                stylesheet.Fills.AppendChild(new Fill()
                {
                    PatternFill = new PatternFill()
                    {
                        PatternType = PatternValues.Gray125
                    }
                });
            }
            if (stylesheet.Borders == null)
            {
                stylesheet.Borders = new Borders();
            }
            if (stylesheet.Borders.ChildElements.Count == 0)
            {
                stylesheet.Borders.AppendChild(new Border());
            }
            if (stylesheet.CellFormats == null)
            {
                stylesheet.CellFormats = new CellFormats();
                if (stylesheet.CellFormats.ChildElements.Count == 0)
                {
                    stylesheet.CellFormats.AppendChild(new CellFormat());
                }
            }
            stylesheet.Save();
        }
        //根据背景颜色获取Stylesheet的子元素Fill的索引值，不存在则创建  
        private int getFillIndex(System.Drawing.Color color)
        {
            int fillIndex = 0;
            foreach (Fill fill in stylesheet.Fills.Elements<Fill>())
            {
                if (fill != null && fill.PatternFill != null && fill.PatternFill.PatternType != null &&
                    fill.PatternFill.PatternType == PatternValues.Solid && fill.PatternFill.ForegroundColor != null &&
                    fill.PatternFill.ForegroundColor.Rgb != null && fill.PatternFill.ForegroundColor.Rgb.Value != null &&
                    fill.PatternFill.ForegroundColor.Rgb.Value == color.ToArgb().ToString("X"))
                {
                    return fillIndex;
                }
                fillIndex++;
            }
            stylesheet.Fills.AppendChild(new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    ForegroundColor = new ForegroundColor()
                    {
                        Rgb = new HexBinaryValue(color.ToArgb().ToString("X"))
                    }
                }
            });
            stylesheet.Save();
            return fillIndex;
        }

        /// <summary>
        /// 获取字体元素缩影，不存在则创建
        /// </summary>
        /// <param name="fontName"></param>
        /// <returns></returns>
        private int getFontIndex(string fontName,uint size)
        {
            int fillIndex = 0;
            foreach (Font font in stylesheet.Fonts.Elements<Font>())
            {
                if (font != null && 
                    font.FontName.Val==fontName&&
                    font.FontSize.Val==size
                    )
                {
                    return fillIndex;
                }
                fillIndex++;
            }
            Font songti = new Font(
                        new FontSize() { Val = size },
                        new FontName() { Val = "宋体" },
                        new FontFamily() { Val = 2 },
                        new FontScheme() { Val = FontSchemeValues.Minor }
                        );
            stylesheet.Fonts.Append(songti);
            stylesheet.Save();
            return fillIndex;
        }
        // 释放所有资源  
        public void Dispose()
        {
            if (stylesheet != null)
            {
                stylesheet.Save();
                stylesheet = null;
            }
            if (workbookPart != null)
            {
                workbookPart.Workbook.Save();
                workbookPart = null;
            }

            if (spreadsheetDocument != null)
            {
                spreadsheetDocument.Close();
                spreadsheetDocument.Dispose();
                spreadsheetDocument = null;
            }
            if (fileStream != null)
            {
                fileStream.Flush();
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
            }
        }
    }
}
