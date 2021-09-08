using System;
using System.ComponentModel.DataAnnotations;


namespace University.BL.DTOs
{
    public class DepartmentDTO
    {
        [Required]
        public int DepartmentID { get; set; }
       
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }
        public InstructorDTO Instructor { get; set; }
    

    public string FullName
        {
            get
            {
                return string.Format("{0} {1}", Name);
            }
        }
    }
}
