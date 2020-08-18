﻿using Native.Sdk.Cqp;
using Newtonsoft.Json;
using Pack.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Make.MODEL
{
    /// <summary>
    /// 总控
    /// </summary>
    public static class GeneralControl
    {
        public static int MaxLevel=5;   //技能卡的最大等级
        public static int MaxStates = 9; //当前状态的最大量
        public static bool LazyLoad_SkillCards = false; //是否惰性加载UI卡片
        public static string directory = System.IO.Directory.GetCurrentDirectory() + "\\app\\仙战";
        private static DateTime skill_Card_Date;//技能卡版本
        private static DateTime adventure_Date;//奇遇版本
        public static Socket_Client socket;
        public static CQApi CQApi;
        public static CQLog CQLog;
        public static Pack.MainWindow MainMenu;

        /// <summary>
        /// 技能卡MODEL
        /// </summary>
        public static List<SkillCardsModel> Skill_Cards = new  List<SkillCardsModel>();//总技能卡 //总引用,但UI层还有一层引用，删掉的同时记得删掉UI层
        /// <summary>
        /// 总技能卡
        /// </summary>
        public static Dictionary<string, SkillCard> Skill_Card_Dictionary = new Dictionary<string, SkillCard>(); //总奇遇
        /// <summary>
        /// 总奇遇
        /// </summary>
        public static List<Adventure> Adventures = new  List<Adventure>();//总引用,但UI层还有一层引用，删掉的同时记得删掉UI层
        /// <summary>
        /// 总状态
        /// </summary>
        public static Dictionary<string,State> States = new Dictionary<string,State>();

        public static DateTime Skill_Card_Date
        {
            get => skill_Card_Date;
            set
            {
                skill_Card_Date = value;
                Write("游戏配置", "Skill_Card_Date", skill_Card_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            }
        }

        public static DateTime Adventure_Date
        {
            get => adventure_Date;
            set
            {
                adventure_Date = value;
                Write("游戏配置", "Adventure_Date", adventure_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            }
        }
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);
        /// 读取INI文件值
        /// </summary>
        /// <param name="section">节点名</param>
        /// <param name="key">键</param>
        /// <param name="def">未取到值时返回的默认值</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>读取的值</returns>
        public static string Read(string section, string key, string def, string filePath)
        {
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, sb, 1024, filePath);
            return sb.ToString();
        }

        /// <summary>
        /// 写INI文件值
        /// </summary>
        /// <param name="section">欲在其中写入的节点名称</param>
        /// <param name="key">欲设置的项名</param>
        /// <param name="value">要写入的新字符串</param>
        /// <param name="filePath">INI文件完整路径</param>
        /// <returns>非零表示成功，零表示失败</returns>
        public static int Write(string section, string key, string value, string filePath)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }
        static GeneralControl()
        {
            if (File.Exists(GeneralControl.directory + @"\游戏配置\GeneralControl.ini"))
            {
                MaxLevel = int.Parse(Read("游戏配置", "MaxLevel", "5", GeneralControl.directory + @"\游戏配置\GeneralControl.ini"));
                MaxStates = int.Parse(Read("游戏配置", "MaxStates", "9", GeneralControl.directory + @"\游戏配置\GeneralControl.ini"));
                LazyLoad_SkillCards = bool.Parse(Read("游戏配置", "LazyLoad_SkillCards", "9", GeneralControl.directory + @"\游戏配置\GeneralControl.ini"));
                Skill_Card_Date = DateTime.Parse(Read("游戏配置", "Skill_Card_Date", "", GeneralControl.directory + @"\游戏配置\GeneralControl.ini"));
                Adventure_Date = DateTime.Parse(Read("游戏配置", "Adventure_Date", "", GeneralControl.directory + @"\游戏配置\GeneralControl.ini"));
            }
            else
            {
                Save();
            }
        }
        public static void Save()
        {
            Write("游戏配置", "MaxLevel", MaxLevel.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Write("游戏配置", "MaxStates", MaxStates.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Write("游戏配置", "LazyLoad_SkillCards", LazyLoad_SkillCards.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Write("游戏配置", "Skill_Card_Date", Skill_Card_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            Write("游戏配置", "Adventure_Date", Adventure_Date.ToString(), GeneralControl.directory + @"\游戏配置\GeneralControl.ini");
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Skill_Cards_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Skill_Cards_Class.Instance));
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Adventure_Cards_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Adventure_Cards_Class.Instance));
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_GameControl_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_GameControl_Class.Instance));
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Person_Informations_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Person_Informations_Class.Instance));
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Lience_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Lience_Class.Instance));
            File.WriteAllText(GeneralControl.directory + @"\游戏配置\Menu_Version_Informations_Class.json", JsonConvert.SerializeObject(GeneralControl.Menu_Version_Informations_Class.Instance));
        }
        public class Menu_Skill_Cards_Class
        {
            static Menu_Skill_Cards_Class()
            {
                Instance = new Menu_Skill_Cards_Class();
            }
            private Menu_Skill_Cards_Class()
            {

            }
            public static Menu_Skill_Cards_Class Instance { get; private set; } = null;
        }
        public class Menu_Adventure_Cards_Class
        {
            static Menu_Adventure_Cards_Class()
            {
                Instance = new Menu_Adventure_Cards_Class();
            }
            private Menu_Adventure_Cards_Class()
            {

            }
            public static Menu_Adventure_Cards_Class Instance { get; private set; } = null;
        }

        public class Menu_Version_Informations_Class
        {
            static Menu_Version_Informations_Class()
            {
                Instance = new Menu_Version_Informations_Class();
            }
            private Menu_Version_Informations_Class()
            {

            }
            public static Menu_Version_Informations_Class Instance { get; private set; } = null;

            public string Expiration_Date { get; set; } = DateTime.Now.ToString();
        }
        public class Menu_Person_Informations_Class : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            static Menu_Person_Informations_Class()
            {               
                Instance = new Menu_Person_Informations_Class();
            }
            private Menu_Person_Informations_Class()
            {

            }
            public static Menu_Person_Informations_Class Instance { get; private set; } = null;
            private Author author=new Author();
            public Author Author
            {
                get { return author; }
                set
                {
                    if (Author != value)
                    {
                        author = value;
                        PropertyChanged(this, new PropertyChangedEventArgs("Author"));
                    }
                } 
            }
        }

        public class Menu_Lience_Class
        {
            static Menu_Lience_Class()
            {
                Instance = new Menu_Lience_Class();
            }
            private Menu_Lience_Class()
            {

            }
            public static Menu_Lience_Class Instance { get; private set; } = null;
        }
        public class Menu_GameControl_Class
        {
            private static readonly Menu_GameControl_Class _instance = null;
            static Menu_GameControl_Class()
            {
                _instance = new Menu_GameControl_Class();
            }
            private Menu_GameControl_Class()
            {

            }
            public static Menu_GameControl_Class Instance
            {
                get { return _instance; }
            }
            public int Immediate_To_Round { get; set; } = 10;
        }
    }
}
