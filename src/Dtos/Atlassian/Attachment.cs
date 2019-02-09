using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Atlassian
{
    public class AtlassianBaseDto
    {
        public Links _Links { get; set; }
    }

    public class Attachments : AtlassianBaseDto
    {
        public List<Attachment> Results { get; set; }

        public int Start { get; set; }

        public int Limit { get; set; }

        public int Size { get; set; }
    }


    public class Attachment : AtlassianBaseDto
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }
        
        public string MediaType { get; set; }

        public long FileSize { get; set; }

        public string Comment { get; set; }

        public string MediaTypeDescription { get; set; }
    }

    public class Links
    {
        public string Webui { get; set; }

        public string Self { get; set; }

        public string Download { get; set; }

        public string Base { get; set; }

        public string Context { get; set; }
    }
}
