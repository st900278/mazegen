using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Canvas
{
    public const double SizeUnset = -1.0;
    OutputFormat format { get; set; }
    public double width { get; private set; } = SizeUnset;
    public double height { get; private set; } = SizeUnset;

    //public virtual stroke 
    
}