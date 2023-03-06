using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models;

public class Employee
{
    
    public int Eid { get; set; } = 0;

    [Required]
    public string? Ename { get; set; }
    [Required]
    public int? Eage { get; set; }
    [Required]
    public int? Esalary { get; set; }
    [Required]
    public string? Edept { get; set; }
}
