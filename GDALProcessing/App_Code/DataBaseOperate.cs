using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GDALProcessing
{
    public class DataBaseOperate
    {
        /// <summary>
        /// 创建SqlConnection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection getSqlCon()
        {
            string sqlcon = ConfigurationManager.ConnectionStrings["sqlcon"].ConnectionString;
            SqlConnection con = new SqlConnection(sqlcon);
            con.Open();
            return con;
        }

        /// <summary>
        /// 创建SqlCommand
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        public static SqlCommand getSqlCmd(string strsql, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(strsql, con);
            return cmd;
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string getResult(string strsql, SqlParameter param)
        {
            string result = "";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            cmd.Parameters.Add(param);
            SqlDataReader read = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (read.Read())
            {
                result = read[0].ToString();
            }
            con.Close();
            return result;
        }

        /// <summary>
        /// 获取作物代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCorpId(string name)
        {
            string strsql = "select CropCode from CROPINFO where CropTypes=@name";
            SqlParameter param = new SqlParameter("name", name);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 转换为PlotId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getPlotId(string id)
        {
            string strsql = "select PlotID from PLOT_DKINFO where RASTERID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 转换为名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getPlotName(string id)
        {
            string strsql = "select FULLNAME from PLOT_DKINFO where RASTERID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 获取统计结果列标题名(除土壤养分外)
        /// </summary>
        /// <param name="res0"></param>
        /// <param name="res1"></param>
        /// <param name="res2"></param>
        /// <returns></returns>
        public static string getTableTitleName(string res0, string res1)
        {
            string sTTName = "";
            string sCropName = getCropName(res1);
            //string sNUTRIENT_NAME = getNUTRIENT_NAME(res2);
            switch (res0)
            {
                case "CROPYIELD":
                    sTTName = "单产";
                    break;
                case "MATUREPERIOD":
                    sTTName = "成熟期";
                    break;
                case "WATERRETRIEVAL":
                    sTTName = "水分含量";
                    break;
                case "CHLOROPHYLLRETRIEVAL":
                    sTTName = "叶绿素含量";
                    break;
            }
            sTTName = sCropName + sTTName;
            return sTTName;
        }

        /// <summary>
        /// 获取统计结果列标题名(除土壤养分外)
        /// </summary>
        /// <param name="res0"></param>
        /// <param name="res1"></param>
        /// <param name="res2"></param>
        /// <returns></returns>
        public static string getNUTRIENTTableTitleName(string res2)
        {
            string sNUTRIENT_NAME = getNUTRIENT_NAME(res2);
            return sNUTRIENT_NAME;
        }

        /// <summary>
        /// 获取JMZ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getJMZ(string id)
        {
            string strsql = "select JMZ from PLOT_DKINFO where RASTERID=@id";
            SqlParameter param = new SqlParameter("id", id);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 获取作业站名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getVillName(string code)
        {
            string strsql = "select VillName from VILLAGE where VillCode=@code";
            SqlParameter param = new SqlParameter("code", code);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 获取作业站Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getVillCode(string name)
        {
            string strsql = "select VillCode from VILLAGE where VillName=@name";
            SqlParameter param = new SqlParameter("@name", name);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 获取GLQ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getGLQ(string id)
        {
            string strsql = "select GLQ from PLOT_DKINFO where RASTERID=@id";
            SqlParameter param = new SqlParameter("id", id);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 获取作业区Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getTownCode(string name)
        {
            string strsql = "select TowCode from TOWN where TownName=@name";
            SqlParameter param = new SqlParameter("@name", name);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 获取作业区名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string getTownName(string code)
        {
            string strsql = "select TownName from Town where TowCode=@code";
            SqlParameter param = new SqlParameter("code", code);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 数据入库
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="param"></param>
        public static int InsertDatabase(string sqlProcedure, SqlParameter[] param)
        {
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(sqlProcedure, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(param);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;
        }
        /// <summary>
        /// 获取普通作物的code
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getCrop_Code(string name)
        {
            string strsql = "select CropCode from CROPINFO where CropEName=@name";
            SqlParameter param = new SqlParameter("@name", name);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 获取普通作物的名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getCropName(string Ename)
        {
            string strsql = "select CropName from CROPINFO where CropEName=@Ename";
            SqlParameter param = new SqlParameter("@Ename", Ename);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 获取NUTRIENT的Code
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getNUTRIENT_CODE(string name)
        {
            string strsql = "select NUTRIENT_CODE from SOILNUTRIENT_CODE where NUTRIENT_ENAME=@name";
            SqlParameter param = new SqlParameter("@name", name);
            string result = getResult(strsql, param);
            return result;
        }
        /// <summary>
        /// 获取NUTRIENT的中文名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string getNUTRIENT_NAME(string name)
        {
            string strsql = "select NUTRIENT_NAME from SOILNUTRIENT_CODE where NUTRIENT_ENAME=@name";
            SqlParameter param = new SqlParameter("@name", name);
            string result = getResult(strsql, param);
            return result;
        }

        /// <summary>
        /// 获取作物代码和名称，并绑定数据源
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> setCropSource()
        {
            Dictionary<string, string> source = new Dictionary<string, string>();
            string strsql = "select [CropCode],[CropName] from CROPINFO where DELFLAG=1";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                source.Add(reader["CropCode"].ToString(), reader["CropName"].ToString());
            }
            con.Close();
            return source;
        }

       
        /// <summary>
        /// 获取处在某一时间段内的时间个数，用于循环汇总
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int getIncludeTimeCount(string strsql,SqlParameter[] param)
        {
            //string strsql = "select count(MONITORTIME) from MONITORTIME between @time1 and @time2";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            cmd.Parameters.AddRange(param);
            int count =Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            cmd.Parameters.Clear();
            return count;
        }

        /// <summary>
        /// 获取处在某一时间段内的具体时间，用于汇总传参
        /// </summary>
        /// <param name="strsql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<string> getIncludeTime(string strsql, SqlParameter[] param)
        {
            List<string> list = new List<string>();
            //string strsql = "select MONITORTIME from MONITORTIME between @time1 and @time2";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            cmd.Parameters.AddRange(param);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            con.Close();
            cmd.Parameters.Clear();
            return list;
        }

        /// <summary>
        /// 处在选择的时间范围内的天数
        /// </summary>
        /// <param name="time1">时间1</param>
        /// <param name="time2">时间2</param>
        /// <param name="corpName">具体表</param>
        /// <returns></returns>
        public static int get_DateCount(DateTime time1,DateTime time2,string corpName)
        {
            string str_datecount = "select count(distinct MONITORTIME) from "+corpName+ " where MONITORTIME between @time1 and @time2";
            SqlParameter[] param_date = new SqlParameter[] { 
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2)
            };

            int count = DataBaseOperate.getIncludeTimeCount(str_datecount, param_date);
            return count;
        }

        
        /// <summary>
        /// 获取具体的时间列表
        /// </summary>
        /// <param name="time1">时间1</param>
        /// <param name="time2">时间2</param>
        /// <param name="corpName">具体表</param>
        /// <returns></returns>
        public static List<string> get_DateDetail(DateTime time1, DateTime time2, string corpName)
        {
            string str_datevalue = "select distinct MONITORTIME from " + corpName + " where MONITORTIME between @time1 and @time2";
            SqlParameter[] param_date = new SqlParameter[] { 
                new SqlParameter("@time1",time1),
                new SqlParameter("@time2",time2)
            };
            List<string> date_list = DataBaseOperate.getIncludeTime(str_datevalue, param_date);
            return date_list;
        }
        

        /// <summary>
        /// 获取CropCode
        /// </summary>
        /// <param name="plotid"></param>
        /// <returns></returns>
        public static List<string> get_CropCode()
        {
            List<string> list = new List<string>();
            string strsql = "select distinct CropCode from CROPINFO where DELFLAG=1";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            con.Close();
            return list;
        }

        /// <summary>
        /// 获取CropCount
        /// </summary>
        /// <param name="plotid"></param>
        /// <returns></returns>
        public static int get_CropCount()
        {
            List<string> list = new List<string>();
            string strsql = "select count(distinct CropCode) from CROPINFO where DELFLAG=1";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return count;
        }
        /// <summary>
        /// 根据crop_code获取作物名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string get_CropCHName(string name)
        {
            string value = "";
            string strsql = "select [CropName] from CROPINFO where CropCode='" + name + "' and DELFLAG=1";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                value= reader[0].ToString();
            }
            con.Close();
            return value;
        }
        /// <summary>
        /// 根据作物名称获取crop_code
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string get_CropCode(string name)
        {
            string value = "";
            string strsql = "select CropCode from CROPINFO where [CropName]='" + name + "' and DELFLAG=1";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                value = reader[0].ToString();
            }
            con.Close();
            return value;
        }

        /// <summary>
        /// 获取NutrientCode
        /// </summary>
        /// <param name="plotid"></param>
        /// <returns></returns>
        public static List<string> get_NutrientCode()
        {
            List<string> list = new List<string>();
            string strsql = "select distinct NUTRIENT_CODE from SOILNUTRIENT_CODE where DELFLAG='1'";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader[0].ToString());
            }
            con.Close();
            return list;
        }

        /// <summary>
        /// 获取NutrientCount
        /// </summary>
        /// <param name="plotid"></param>
        /// <returns></returns>
        public static int get_NutrientCount()
        {
            List<string> list = new List<string>();
            string strsql = "select count(distinct NUTRIENT_CODE) from SOILNUTRIENT_CODE where DELFLAG='1'";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return count;
        }
        /// <summary>
        /// 根据nutrient_code获取养分名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string get_NutrientCHName(string name)
        {
            string value = "";
            string strsql = "select [NUTRIENT_NAME] from SOILNUTRIENT_CODE where NUTRIENT_CODE='" + name + "'";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                value = reader[0].ToString();
            }
            con.Close();
            return value;
        }

        /// <summary>
        /// 根据作物名称获取Nutrient_code
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string get_NutrientCode(string name)
        {
            string value = "";
            string strsql = "select NUTRIENT_CODE from SOILNUTRIENT_CODE where [NUTRIENT_NAME]='" + name + "'";
            SqlConnection con = DataBaseOperate.getSqlCon();
            SqlCommand cmd = DataBaseOperate.getSqlCmd(strsql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                value = reader[0].ToString();
            }
            con.Close();
            return value;
        }
        
    }
}