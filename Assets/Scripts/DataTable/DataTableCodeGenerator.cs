using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF;
using NPOI.XSSF.UserModel;
using System;
using NPOI.SS.Formula.Eval;
using System.Text.RegularExpressions;

public static class DataTableCodeGenerator
{
    [MenuItem("Game/DataTable/GenerateCode")]
    public static void Generate()
    {
        string fmt = "array<int>";
        Regex reg = new Regex(@"^([a-z]+)<[a-z]+>$");
        MatchCollection matches = reg.Matches(fmt);
        foreach (Match e in matches)
            Debug.LogError(e.Groups[1]);
        return;

        using(var stream = new FileStream("Assets/DataTable/Test.xlsx", FileMode.Open, FileAccess.Read))
        {
            XSSFWorkbook wb = new XSSFWorkbook(stream);
            Debug.LogErrorFormat("Number of Sheet {0}", wb.NumberOfSheets);
            for(int ii = 0; ii < wb.NumberOfSheets; ++ii)
            {
                XSSFSheet sheet = wb.GetSheetAt(ii) as XSSFSheet;
                Debug.LogErrorFormat("{0}. {1} {2}", ii, sheet.SheetName, sheet.LastRowNum);

                for(int rr = 0; rr <= sheet.LastRowNum; ++rr)
                {
                    XSSFRow row = sheet.GetRow(rr) as XSSFRow;
                    if (row == null)
                        Debug.LogErrorFormat("{0}. row is null", rr);
                    else
                        Debug.LogErrorFormat("{0}. {1}", rr, row.LastCellNum);

                    for(int cc = 0; cc < row.LastCellNum; ++cc)
                    {
                        XSSFCell cell = row.GetCell(cc) as XSSFCell;
                        if(cell.CellType == CellType.Numeric)
                        {
                            if(DateUtil.IsCellDateFormatted(cell))
                            {
                                DateTime date = cell.DateCellValue;
                                ICellStyle style = cell.CellStyle;
                                string format = style.GetDataFormatString().Replace("m", "M");
                                Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), date.ToString(format));
                            }
                            else
                            {
                                Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), cell.ToString());
                            }
                        }
                        else if(cell.CellType == CellType.Formula)
                        {
                            switch (cell.CachedFormulaResultType)
                            {
                                case CellType.Boolean:
                                    Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), cell.BooleanCellValue.ToString());
                                    break;
                                case CellType.Numeric:
                                    Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), cell.NumericCellValue.ToString());
                                    break;
                                case CellType.String:
                                    Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), cell.StringCellValue.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            Debug.LogErrorFormat("{0}. {1}", cell.CellType.ToString(), cell.ToString());
                        }
                    }
                }
            }            
        }
        return;
    }
}
