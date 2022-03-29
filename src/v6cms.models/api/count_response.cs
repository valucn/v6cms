using System.Collections.Generic;

namespace v6cms.models.api
{
    public class count_response
    {
        public List<data_model> data { get; set; }
        public class data_model
        {
            public int ask_id { get; set; }
            public int comment_count { get; set; }
            public int up_count { get; set; }
        }
    }
}
