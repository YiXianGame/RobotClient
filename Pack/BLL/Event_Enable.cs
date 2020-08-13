﻿using Make.MODEL;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pack.BLL
{
    public class Event_Enable : IAppEnable
    {
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            GeneralControl.CQApi = e.CQApi;
            GeneralControl.CQLog = e.CQLog;
            Init init = new Init();
        }
    }
}
