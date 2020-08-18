﻿using Make.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections;
using MaterialDesignThemes;
using MaterialDesignColors;
using Pack.BLL;

namespace Pack.Element
{
    /// <summary>
    /// CardPanle.xaml 的交互逻辑
    /// </summary>
    public partial class CardPanle:UserControl
    {
        public int NowFirst = 0;
        SkillCard Filter_Skill = new SkillCard();
        public CardPanle()
        {
            InitializeComponent();
            Filter_Skill.State = -1;
            Author.DataContext = new Make.MODEL.Author();
            Filter_Bar.DataContext = Filter_Skill;
        }

        public Custom_Card_SkillCard Add_Card(SkillCardsModel skillCards)
        {
            Custom_Card_SkillCard card = new Custom_Card_SkillCard(skillCards);
            CardsPanel.Children.Add(card);
            if (Make.MODEL.GeneralControl.LazyLoad_SkillCards) if (CardsPanel.Children.Count >= 96) card.Visibility = Visibility.Collapsed;
            card.EditButton.Click += EditButton_Click;
            card.AuthorButton.Click += AuthorButton_Click;
            card.Cloud.Content = skillCards.Cloud;
            return card;
        }

        private void AuthorButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_SkillCard)) ptr = VisualTreeHelper.GetParent(ptr);
            XY.Send_To_Server("作者查询#" + (ptr as Custom_Card_SkillCard).SkillCardsModel.Author_ID);
            Author.Visibility = Visibility.Visible;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject ptr = sender as DependencyObject;
            while (!(ptr is Custom_Card_SkillCard))ptr= VisualTreeHelper.GetParent(ptr);
            EditCard.Open_Edit((Custom_Card_SkillCard)ptr);
        }
        private void Fitler()
        {
            Filter_Skill.Name = Template_Skill_Name.Text;
            IEnumerable<SkillCardsModel> array = Make.MODEL.Filter.SkillCardsModel(Make.MODEL.GeneralControl.Skill_Cards, Rate.Value - 1, Filter_Skill);
            foreach (Custom_Card_SkillCard item in CardsPanel.Children)
            {  
                if (array!=null && array.Where<SkillCardsModel>(x => item.SkillCardsModel == x).Count() != 0)
                {
                    item.Visibility = Visibility.Visible;
                    item.Rate.Value = Rate.Value;
                }
                else item.Visibility = Visibility.Collapsed;
            }
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_1(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_2(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_3(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void CheckBox_Click_4(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void Rate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            Fitler();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Fitler();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SkillCard[] skillCards = new SkillCard[5] { new SkillCard(),new SkillCard(),new SkillCard(),new SkillCard(),new SkillCard()};
            int cnt = 0;
            string temp;
            SkillCardsModel skillCardsModel = new SkillCardsModel();
            foreach (SkillCard item in skillCards)
            {
                item.Level = cnt;
                item.Name = "新技能" + (++cnt).ToString();
                item.Father_ID = skillCardsModel.ID;
            }
            skillCardsModel.SkillCards = skillCards;
            skillCardsModel.Author_ID = GeneralControl.Menu_Person_Informations_Class.Instance.Author.ID;
            skillCardsModel.Add_To_General();
            skillCardsModel.Save();
            Add_Card(skillCardsModel);
        }

        private void Template_Skill_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Fitler();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (from Custom_Card_SkillCard item in CardsPanel.Children where item.SkillCardsModel.Cloud == "云端" select item).ToList().ForEach(delegate (Custom_Card_SkillCard item)
            {
                item.SkillCardsModel.Delete();
                GeneralControl.Skill_Cards.Remove(item.SkillCardsModel);
                foreach (SkillCard obj in item.SkillCardsModel.SkillCards)
                {
                    GeneralControl.Skill_Card_Dictionary.Remove(obj.Name);
                }
                GeneralControl.Skill_Card_Date = DateTime.Now;
                CardsPanel.Children.Remove(item);
            });
            XY.Send_To_Server("获取技能卡#" + GeneralControl.Skill_Card_Date);
        }
    }
}
