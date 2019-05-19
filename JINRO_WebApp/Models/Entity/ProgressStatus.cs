using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JINRO_WebApp.Models.Entity
{
    public class ProgressStatus
    {
        public int turnCount { get; set; }
        public List<string> victims { get; set; }
    }
}