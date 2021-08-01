using System.Collections.Generic;

namespace Case.Model
{
    public class Symbol
    {
        public Meta meta { get; set; }
        public List<Value> values { get; set; }
        public string status { get; set; }
    }
}