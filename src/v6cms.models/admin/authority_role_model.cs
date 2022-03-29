using System.Collections.Generic;

namespace v6cms.models.admin
{
    public class authority_role_model
    {
        public string role_name { get; set; }

        public List<authority_model> authority_list { get; set; }

        public class authority_model
        {
            public string key { get; set; }

            public string name { get; set; }
        }
    }
}
