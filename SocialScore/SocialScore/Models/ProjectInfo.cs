using System;
using System.Collections.Generic;
using System.Text;

namespace SocialScore.Models
{
    public class ProjectInfo
    {
        public string name { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public string lastUpdate { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class ResultInfo
    {
        public bool isSuccess { get; set; }
        public string guid { get; set; }
        public ProjectInfo projectInfo { get; set; }
    }
}
