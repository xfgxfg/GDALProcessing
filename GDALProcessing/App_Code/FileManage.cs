using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;

    class FileManage
    {
        /// <summary>
        /// 移动文件共用方法
        /// </summary>
        /// <param name="list"></param>
        public void MoveFiles(List<string> list, string sOrignPath, string sDestinationPath)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string sName = list[i].ToString();
                //源文件目录
                string sOrignFile = sOrignPath + sName;
                //目标文件目录
                string destinationFile = sDestinationPath + sName;
                //目标文件目录是否已有文件判断
                if (File.Exists(destinationFile))
                {
                    FileInfo fi = new FileInfo(destinationFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    //删除已有文件
                    File.Delete(destinationFile);
                }
                if (File.Exists(sOrignFile))
                //移动文件到目标目录
                File.Move(sOrignFile, destinationFile);
            }
            MessageBox.Show("文件移动成功！", "成功", MessageBoxButtons.OK);
        }

        /// <summary>
        /// 获取指定扩展名的文件名列表
        /// </summary>
        /// <param name="orignList"></param>
        /// <returns></returns>
        public static ArrayList getNewList(ArrayList orignList, string sExtensionName)
        {
            ArrayList newlist = new ArrayList();
            for (int i = 0; i < orignList.Count; i++)
            {
                string sFullName = orignList[i].ToString();
                if (sFullName.Contains(sExtensionName) && !sFullName.Contains(".xml")) 
                {
                    newlist.Add(sFullName);
                }
                
            }
            return newlist;
        }

      /// <summary>
      /// 获取SHP文件的全部文件列表
      /// </summary>
      /// <param name="orignList"></param>
      /// <param name="sExtensionName"></param>
      /// <returns></returns>
        public static List<string> getShpList(List<string> orignList)
        {
            List<string> listExtensionNames = new List<string>();
            listExtensionNames.Add(".dbf");
            listExtensionNames.Add(".prj");
            //listExtensionNames.Add(".sbn");
            //listExtensionNames.Add(".sbx");
            listExtensionNames.Add(".shp");
            listExtensionNames.Add(".shx");
            List<string> newlist = new List<string>();
            string sNewName = "";
            for (int i = 0; i < orignList.Count; i++)
            {
                for (int k = 0; k < listExtensionNames.Count; k++)
                {
                    string sFullName = orignList[i].ToString();
                    string[] sShortName = sFullName.Split('.');
                    sNewName = sShortName[0];
                    //MessageBox.Show("sNewName=" + sNewName);
                    newlist.Add(sNewName + listExtensionNames[k]);
                }
            }
            return newlist;
        }

        /// <summary>
        /// 判断文件夹是否为空
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public static bool IsExistFile(string sFilePath)
        {
            bool b = true;
            System.IO.DirectoryInfo dI = new DirectoryInfo(sFilePath);
            if (dI.GetFiles().Length + dI.GetDirectories().Length == 0)
            {
                b = false;
            }

            return b;
        }

        /// <summary>
        /// 删除指定目录指定文件名的文件
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sPath"></param>
        public void DeteleFiles(List<string> list, string sPath)
        {
            for (int i = 0; i < list.Count; i++)
            {
                string sName = list[i].ToString();
                //文件目录
                string sDeteleFile = sPath + sName;

                if (File.Exists(sDeteleFile))
                {
                    FileInfo fi = new FileInfo(sDeteleFile);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    //测试看下有哪些内容，避免来回拷文件伤硬盘
                    //MessageBox.Show(list[i].ToString());
                    //如果弹对话框，可注释下面一行删除执行，避免多次删除文件损硬盘
                    File.Delete(sDeteleFile);
                }
            }
           // MessageBox.Show("所选文件已删除！", "成功", MessageBoxButtons.OK);
        }

        public static string getApplicatonPath()
        {
            string sProPath = Application.StartupPath;
            string sXmlPath = sProPath.Substring(0, sProPath.LastIndexOf("bin"));
            return sXmlPath;
        }


        /// <summary>
        /// 获取文件夹下所有文件名
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static ArrayList getAllFileNameFromFolder(string sPath)
        {
            ArrayList listNames = new ArrayList();
            string []sFiles=Directory.GetFiles(sPath);
            foreach (var sFile in sFiles)
	        {
                //MessageBox.Show(sFile);
                listNames.Add(sFile);
	        }
            return listNames;
        }

        /// <summary>
        /// 获取指定路径下指定扩展名的文件
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="sFilter"></param>
        /// <returns></returns>
        public static List<string> getAllFileNameFromFolder(string sPath,string sFilter)
        {
            List<string> listNames = new List<string>();
            string[] sFiles = Directory.GetFiles(sPath);
            foreach (var sFile in sFiles)
            {
                if (sFile.Contains(sFilter))
                {
                    listNames.Add(sFile);
                }
            }
            return listNames;
        }

        /// <summary>
        /// 获取文件夹及子文件夹下所有文件名
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public static List<string> getAllFolderAndFiles(string sPath,string Extentions)
        {
            List<string> sFiles = new List<string>();
            DirectoryInfo theFolder = new DirectoryInfo(sPath);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                // this.listBox1.Items.Add(NextFolder.Name);
                FileInfo[] fileInfo = NextFolder.GetFiles();
                foreach (FileInfo NextFile in fileInfo) //遍历文件
                {
                    if (NextFile.Name.Contains(Extentions))
                    {
                        string sFullPath = sPath + "\\" + NextFolder + "\\" + NextFile.Name;

                        sFiles.Add(sFullPath);
                    }
                }
            }

            return sFiles;
        }


    }
