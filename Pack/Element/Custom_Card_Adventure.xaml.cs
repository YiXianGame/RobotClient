﻿using Make.MODEL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Pack;
using System.Globalization;

namespace Pack.Element
{   
    /// <summary>
    /// Custom_Card.xaml 的交互逻辑
    /// </summary>
    public partial class Custom_Card_Adventure
    {
        public Adventure Adventure;
        public Custom_Card_Adventure(Adventure adventure)
        {
            InitializeComponent();
            DataContext = adventure;
            Adventure = adventure;
            InitializeComponent();
            Cloud.Content = adventure.Cloud;
        }
        public Custom_Card_Adventure()
        {
            InitializeComponent();
        }
    }
}
