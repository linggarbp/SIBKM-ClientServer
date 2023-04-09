﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_profilings")]
public class Profiling
{
    [Key, Column("employee_nik", TypeName = "char(5)")]
    public string EmployeeNIK { get; set; }
    [Column("education_id")]
    public int EducationID { get; set; }
}
