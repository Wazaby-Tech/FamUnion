using System;
using System.Collections.Generic;
using System.Text;

namespace FamUnion.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    sealed class FeatureSetAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string featureSet;

        // This is a positional argument
        public FeatureSetAttribute(string featureSet)
        {
            this.featureSet = featureSet;
        }

        public string FeatureSet
        {
            get { return featureSet; }
        }
    }
}
