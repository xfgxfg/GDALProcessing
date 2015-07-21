using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

    class StringFormater
    {

        /// <summary>
        /// 长字串拆分后封装到LIST中
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> getListByStringSplit(string str)
        {
            List<string> list = new List<string>();
            string[] sStrs = str.Substring(0,str.Length-1).Split('@');
            foreach (var item in sStrs)
            {
                list.Add(item);
            }
            
            return list;
        }
        
        public static string getNewName(string sName, string sImageOutPath)
        {
            int lastIndex = sName.LastIndexOf(@"\");
            string sInputPath = sName.Substring(0, lastIndex) + "\\";
            string sFullFileName = sName.Substring(lastIndex + 1);
            string[] sNames = sFullFileName.Split('.');
            string sShortName = sNames[0];
            string outputname = sImageOutPath + "\\" + sShortName + "_clip.tif";
            return outputname;
        }

        public static string GetMarkedDirectory(string sDir)
        {
            char cLastWord = sDir[sDir.Length - 1];
            if (cLastWord != '\\')
            {
                string sNewDIR = sDir + "\\";
                return sNewDIR;
            }
            return sDir;
        }

        /// <summary>  
        /// 把一个一维数组转换为DataTable  
        /// </summary>  
        /// <param name="ColumnName">列名</param>  
        /// <param name="Array">一维数组</param>  
        /// <returns>返回DataTable</returns>  
        public static DataTable Convert(string ColumnName, string[] Array)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(ColumnName, typeof(string));

            for (int i = 0; i < Array.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr[ColumnName] = Array[i].ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// 把一个一维数组转换为M行N列的DataTable
        /// </summary>
        /// <param name="ColumnNames"></param>
        /// <param name="Array"></param>
        /// <returns></returns>
        public static DataTable Convert(string []ColumnNames, string[] Array)
        {
            DataTable dt = new DataTable();
            foreach (string ColumnName in ColumnNames)
            {
                dt.Columns.Add(ColumnName, typeof(string));
            }

            for (int i = 0; i < Array.Length; i++)
            {
                string[] NewArray = Array[i].Split(',');
                DataRow dr = dt.NewRow();
                for (int j = 0; j < NewArray.Length; j++)
                {
                    dr[j] = NewArray[j].ToString();
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>  
        /// 反一个M行N列的二维数组转换为DataTable  
        /// </summary>  
        /// <param name="ColumnNames">一维数组，代表列名，不能有重复值</param>  
        /// <param name="Arrays">M行N列的二维数组</param>  
        /// <returns>返回DataTable</returns>  
        public static DataTable Convert(string[] ColumnNames, string[,] Arrays)
        {
            DataTable dt = new DataTable();

            foreach (string ColumnName in ColumnNames)
            {
                dt.Columns.Add(ColumnName, typeof(string));
            }

            for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    dr[i] = Arrays[i1, i].ToString();
                }
                dt.Rows.Add(dr);
            }

            return dt;

        }


        /// <summary>  
        /// 反一个M行N列的二维数组转换为DataTable  
        /// </summary>  
        /// <param name="Arrays">M行N列的二维数组</param>  
        /// <returns>返回DataTable</returns>   
        public static DataTable Convert(string[,] Arrays)
        {
            DataTable dt = new DataTable();

            int a = Arrays.GetLength(0);
            for (int i = 0; i < Arrays.GetLength(1); i++)
            {
                dt.Columns.Add("col" + i.ToString(), typeof(string));
            }

            for (int i1 = 0; i1 < Arrays.GetLength(0); i1++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < Arrays.GetLength(1); i++)
                {
                    dr[i] = Arrays[i1, i].ToString();
                }
                dt.Rows.Add(dr);
            }

            return dt;

        }  


    }