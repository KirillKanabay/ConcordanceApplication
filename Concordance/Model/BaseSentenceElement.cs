﻿namespace Concordance.Model
{
    public abstract class BaseSentenceElement
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
