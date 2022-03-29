namespace v6cms.models
{
    /// <summary>
    /// 部分得分实体类
    /// </summary>
    public class dept_count_model
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string dept_name { get; set; }

        /// <summary>
        /// 公安部得分
        /// </summary>
        public decimal score_gab { get; set; }

        /// <summary>
        /// 厅省得分
        /// </summary>
        public decimal score_province { get; set; }

        /// <summary>
        /// 市局得分
        /// </summary>
        public decimal score_city { get; set; }

        /// <summary>
        /// 分局得分
        /// </summary>
        public decimal score_branch { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public decimal score_total { get; set; }
    }
}
