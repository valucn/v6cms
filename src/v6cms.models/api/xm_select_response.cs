using System.Collections.Generic;

namespace v6cms.models.api
{
    public class xm_select_response
    {
        public int id { get; set; }

        public string name { get; set; }

        public int value { get; set; }

        public List<xm_select_response> children { get; set; }
    }
}
