using System.Collections.Generic;
using System.Linq;

namespace v6cms.utils.premission
{
    public partial class auth_premissions
    {
        public static bool has_permissions(List<string> authority_code_list, params string[] authority_code)
        {
            return authority_code.Any(p => authority_code_list.Contains(p));
        }
    }
}
