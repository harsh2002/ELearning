using System;
using System.Collections.Generic;
using System.Text;

namespace ELearning.Model
{
    public class TestModel
    {
        public int TestId { get; set; }
        public string Question { get; set; }
        public byte[] Image { get; set; }
        public string Answer { get; set; }
        public ModuleModel Module { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
    }
}
