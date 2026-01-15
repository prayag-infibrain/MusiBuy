using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusiBuy.Common.Common;

namespace MusiBuy.Common.Models;

public partial class PsmmatrixFileViewModel
{
    public int Id { get; set; }

    public int Createby { get; set; }

    public DateTime Createdon { get; set; }

    public string FileName { get; set; } = null!;


    public string ? fPSM {  get; set; } 
    public string ? fClinical {  get; set; }    
    public string ? fProteinSummary {  get; set; }    
    public string ? fProteinData {  get; set; }    


    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "RequiredField")]
    [Display(Name = "File")]
    public IFormFile formFile { get; set; }

    public bool IsPsmFileExists { get; set; }


}
