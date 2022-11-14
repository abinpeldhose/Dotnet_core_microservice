using System;
using System.Collections.Generic;

namespace customerSdmodule.Model1
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int BlogId { get; set; }
    }
}
