using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSGeo.GDAL;
using OSGeo.OGR;
using System.Windows.Forms;

namespace GDALProcessing
{
    public class ReadShape
    {

        public static DataSource open(string strVectorFile)
        {
            Gdal.AllRegister();
            // 为了支持中文路径，请添加下面这句代码
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");

            // 注册所有的驱动
            Ogr.RegisterAll();

            //打开数据
            DataSource ds = Ogr.Open(strVectorFile, 0);
            if (ds == null)
            {
                MessageBox.Show("打开文件【{0}】失败！", strVectorFile);
                return ds;
            }

            int iLayerCount = ds.GetLayerCount();
            return ds;
        }
        
        public static List<string> getShapeFieldDataList(string strVectorFile,string sFileName)
        {
            List<string> list = new List<string>();

            #region ReadShape
            Gdal.AllRegister();
            // 为了支持中文路径，请添加下面这句代码
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");

            // 注册所有的驱动
            Ogr.RegisterAll();

            //打开数据
            DataSource ds = Ogr.Open(strVectorFile, 0);
            if (ds == null)
            {
                MessageBox.Show("打开文件【{0}】失败！", strVectorFile);
                return list;
            }
            //MessageBox.Show("打开文件【{0}】成功！", strVectorFile);

            // 获取该数据源中的图层个数，一般shp数据图层只有一个，如果是mdb、dxf等图层就会有多个
            int iLayerCount = ds.GetLayerCount();

            // 获取第一个图层
            Layer oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                MessageBox.Show("获取第{0}个图层失败！\n", "0");
                return list;
            }

            // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空
            oLayer.ResetReading();
            FeatureDefn oDefn = oLayer.GetLayerDefn();
            // 输出图层中的要素个数
            int iFieldCount = oDefn.GetFieldCount();
            Feature oFeature = null;
            // 下面开始遍历图层中的要素
            while ((oFeature = oLayer.GetNextFeature()) != null)
            {
                // 获取要素中的属性表内容
                for (int iField = 0; iField < iFieldCount; iField++)
                {
                    FieldDefn oFieldDefn = oDefn.GetFieldDefn(iField);
                    FieldType type = oFieldDefn.GetFieldType();
                    string sFieldName = oFieldDefn.GetNameRef();
                    switch (type)
                    {
                        case FieldType.OFTString:
                            //MessageBox.Show(iField+"=="+oFeature.GetFieldAsString(iField));
                            break;
                        case FieldType.OFTReal:
                            //MessageBox.Show(iField + "==" + oFeature.GetFieldAsDouble(iField));
                            break;
                        case FieldType.OFTInteger:
                            //MessageBox.Show(iField + "==" + oFeature.GetFieldAsInteger(iField));
                            break;
                        default:
                            //MessageBox.Show(iField + "==" + oFeature.GetFieldAsString(iField));
                            //list.Add(oFeature.GetFieldAsString(iField));
                            break;
                    }
                    if (sFieldName.Equals(sFileName))
                    {
                        list.Add(oFeature.GetFieldAsString(iField));
                    }
                }
            }

            #endregion
            list.Sort();
            return list;
        }

        /// <summary>
        /// 获取所有列名
        /// </summary>
        /// <param name="strVectorFile"></param>
        /// <returns></returns>
        public static List<string> getShapeFieldList(string strVectorFile)
        {
            List<string> list = new List<string>();

            #region ReadShape
            {
                Gdal.AllRegister();
                // 为了支持中文路径，请添加下面这句代码
                OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
                // 为了使属性表字段支持中文，请添加下面这句
                OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");

                // 注册所有的驱动
                Ogr.RegisterAll();

                //打开数据
                DataSource ds = Ogr.Open(strVectorFile, 0);
                if (ds == null)
                {
                    MessageBox.Show("打开文件【{0}】失败！,文件名中不能包含中文！", strVectorFile);
                    return list;
                }
                //MessageBox.Show("打开文件【{0}】成功！", strVectorFile);

                // 获取该数据源中的图层个数，一般shp数据图层只有一个，如果是mdb、dxf等图层就会有多个
                int iLayerCount = ds.GetLayerCount();

                // 获取第一个图层
                Layer oLayer = ds.GetLayerByIndex(0);
                if (oLayer == null)
                {
                    MessageBox.Show("获取第{0}个图层失败！\n", "0");
                    return list;
                }

                // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空
                oLayer.ResetReading();
                FeatureDefn oDefn = oLayer.GetLayerDefn();
                // 输出图层中的要素个数
                int iFieldCount = oDefn.GetFieldCount();
                for (int iAttr = 0; iAttr < iFieldCount; iAttr++)
                {
                    FieldDefn oField = oDefn.GetFieldDefn(iAttr);

                    list.Add(oField.GetNameRef());
                }
            }
            #endregion
            list.Sort();
            return list;

        }

        public static int getShapeCount(string strVectorFile)
        {
            int nCount = 0;
            Gdal.AllRegister();
            // 为了支持中文路径，请添加下面这句代码
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            // 为了使属性表字段支持中文，请添加下面这句
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", "");

            // 注册所有的驱动
            Ogr.RegisterAll();

            //打开数据
            DataSource ds = Ogr.Open(strVectorFile, 0);
            if (ds == null)
            {
                MessageBox.Show("打开文件【{0}】失败！", strVectorFile);
                return nCount;
            }

            nCount = ds.GetLayerCount();
            // 获取第一个图层
            Layer oLayer = ds.GetLayerByIndex(0);
            if (oLayer == null)
            {
                MessageBox.Show("获取第{0}个图层失败！\n", "0");
                return nCount;
            }

            // 对图层进行初始化，如果对图层进行了过滤操作，执行这句后，之前的过滤全部清空
            oLayer.ResetReading();
            //FeatureDefn oDefn = oLayer.GetLayerDefn();
            int FeatureCount = oLayer.GetFeatureCount(0);
            return FeatureCount;
        }



    }
}
