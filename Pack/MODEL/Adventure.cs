﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Make.MODEL
{
    /// <summary>
    /// 奇遇
    /// </summary>
    public class Adventure
    {
        private string name = "";//奇遇名
        private int mp;//对这个玩家造成的MP
        private int hp;//对这个玩家造成的HP
        private int state=0;//奇遇状态（是否可用）
        private List<State> effect_States=new  List<State>();//奇遇所自带的状态效果
        private string messages="";//自带信息
        private int probability;//概率
        private string description="";//奇遇的描述（介绍）
        private string iD;
        private long author_ID;
        public long Author_ID { get => author_ID; set => author_ID = value; }
        public string ID { get => iD; set => iD = value; }
        private string cloud = "非云端";
        private int attack_Number = 1;
        public string Name
        { 
            get => name;
            set
            {
                if (!int.TryParse(value, out int result))
                {
                    if (GeneralControl.Adventures.Contains(this))
                    {
                        while ((from Adventure item in GeneralControl.Adventures where item.Name == value && item.Name != Name select item).Any()) value += "-副本";
                    }
                    name = value;
                }
                else return;
            }
        }
        public int Mp { get => mp; set => mp = value; }
        public int Hp { get => hp; set => hp = value; }

        public string Messages { get => messages; set => messages = value; }
        public int Probability { get => probability; set => probability = value; }
        public List<State> Effect_States { get => effect_States; set => effect_States = value; }
        public int State
        {
            get => state;
            set
            {
                state = value;
            }
        }
        public string Description { get => description; set => description = value; }
        public string Cloud { get => cloud; set => cloud = value; }
        public int Attack_Number { get => attack_Number; set => attack_Number = value; }

        public void SetName(string adventure_Name)
        {
            name = adventure_Name;
        }
        public Adventure()
        {
            string temp_id;
            do
            {
                temp_id = Guid.NewGuid().ToString();
            }
            while (File.Exists(GeneralControl.directory + "\\技能卡\\" + temp_id + ".json"));
            ID = temp_id;
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            string filepath = GeneralControl.directory + "\\奇遇\\" + ID + ".json";
            File.WriteAllText(filepath, json);
        }

        public void Delete()
        {
            string filepath = GeneralControl.directory + "\\奇遇\\" + ID + ".json";
            GeneralControl.Adventures.Remove(this);
            GeneralControl.Adventures_ID.Remove(ID);
            File.Delete(filepath);
        }
        public void Add_To_General()
        {
            while ((from Adventure item in GeneralControl.Adventures where item.Name == Name select item).Any()) Name += "-副本";
            while ((from Adventure item in GeneralControl.Adventures where item.ID == ID select item).Any()) ID = Guid.NewGuid().ToString();
            GeneralControl.Adventures.Add(this);
            GeneralControl.Adventures_ID.Add(ID, this);
        }
    }
}
