﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_educations")]
public class Education
{
    [Key, Column("id")]
    public int ID { get; set; }
    [Column("major", TypeName = "varchar(100)")]
    public string Major { get; set; }
    [Column("degree", TypeName = "varchar(10)")]
    public string Degree { get; set; }
    [Column("gpa", TypeName = "varchar(5)")]
    public string GPA { get; set; }
    [Column("university_id")]
    public int? UniversityID { get; set; }

    //Cardinality
    public University University { get; set; }
    public Profiling Profiling { get; set; }
}
