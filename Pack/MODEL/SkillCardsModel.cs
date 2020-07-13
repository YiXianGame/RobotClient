﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make.MODEL
{
    /// <summary>
    /// 内含一种技能卡的不同等级
    /// </summary>

    public class SkillCardsModel 
    {

        private SkillCard[] skillCards;
        private bool is_Basic;
        private int iD;
        public SkillCard[] SkillCards { get => skillCards; set => skillCards = value; }
        public int ID { get => iD; set => iD = value; }
        public bool Is_Basic { get => is_Basic; set => is_Basic = value; }

        public SkillCardsModel()
        {
            ID = GetHashCode();

        }
        public SkillCardsModel(SkillCard[] Bind)
        {
            skillCards = Bind;
            ID = GetHashCode();
            foreach (SkillCard item in Bind) item.Father_ID = ID;
        }
        public void Save()
        {
            string json = JsonConvert.SerializeObject(this);
            string filepath = GeneralControl.directory + "\\技能卡\\" + ID + ".json";
            File.WriteAllText(filepath, json);
        }
        public void Delete()
        {
            string filepath = GeneralControl.directory + "\\技能卡\\" + ID + ".json";
            foreach(SkillCard item in skillCards)
            {
                GeneralControl.Skill_Card_Dictionary.Remove(item.Name);
            }
            File.Delete(filepath);
        }
    }
}