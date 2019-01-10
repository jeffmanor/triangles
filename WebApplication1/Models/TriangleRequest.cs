using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class TriangleRequest
    {
        /// <summary>
        /// the row... something like "A", "B", etc
        /// </summary>
        [Required]
        [StringLength(1)]
        public string RowText { get; set; }

        /// <summary>
        /// the column (1-12)
        /// </summary>
        [Range(1,12)]
        public int ColumnText { get; set; }
    }
}